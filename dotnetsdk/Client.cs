using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ScreenshotOne
{
    public class Client
    {
        private const string BaseUrl = "https://api.screenshotone.com/take";
        private readonly string _accessKey;
        private readonly string? _secretKey;
        private readonly HttpClient? _httpClient;

        public Client(string accessKey, string? secretKey, HttpClient httpClient)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                throw new ArgumentNullException(nameof(accessKey), "accessKey must be provided");
            }

            _accessKey = accessKey;
            _secretKey = secretKey;
            _httpClient = httpClient;
        }

        public Client(string accessKey, string? secretKey) : this(accessKey, secretKey, null!)
        { }

        public Client(string accessKey, HttpClient httpClient) : this(accessKey, null, httpClient)
        { }

        public Client(string accessKey) : this(accessKey, null, null!)
        { }

        public string GenerateTakeUrl(TakeOptions takeOptions)
        {
            if (takeOptions == null)
            {
                throw new ArgumentNullException($"{nameof(takeOptions)} cannot be null");
            }
            
            var queryString = $"{BuildQueryString(takeOptions.Query)}&access_key={_accessKey}";
            var finalQueryString = SignIfRequired(queryString);

            if (!Uri.TryCreate(BaseUrl + "?" + finalQueryString, UriKind.Absolute, out var uri))
            {
                throw new ArgumentException($"Unable to create a valid TakeUrl {queryString}");
            }

            return uri.ToString();
        }

        public async Task<byte[]> Take(TakeOptions takeOptions)
        {
            if (takeOptions == null)
            {
                throw new ArgumentException($"{nameof(takeOptions)} cannot be null");
            }

            var url = GenerateTakeUrl(takeOptions);
            var httpClient = _httpClient ?? new HttpClient();
            var result = await httpClient.GetAsync(url);
            
            if (!result.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to take a screenshot, the server responded with {(int)result.StatusCode} {result.StatusCode}");
            }

            return await result.Content.ReadAsByteArrayAsync();
        }

        private static string BuildQueryString(IReadOnlyDictionary<string, List<string>> queryParams)=> 
            string.Join("&", queryParams.SelectMany(x => x.Value.Select(val => $"{x.Key}={UrlEncoder.Default.Encode(val)}")));
        
        private string SignIfRequired(string queryString)
        {
            if (_secretKey is null)
                return queryString;
            
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secretKey));
            var data = Encoding.UTF8.GetBytes(queryString);
            var hash = hmac.ComputeHash(data);

            // Convert the hash to a hexadecimal string
            var signature = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return $"{queryString}&signature={signature}";
        }
    }
}