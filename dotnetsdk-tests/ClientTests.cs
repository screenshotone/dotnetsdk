using System.Net;
using System.Text;
using FluentAssertions;
using Moq;
using Moq.Protected;
using ScreenshotOne;
using TimeZone = ScreenshotOne.TimeZone;

namespace dotnetsdk_tests
{
    public class ClientTests
    {
        private readonly Client _client;
        private const string AccessKey = "_OzqMIjpCw-ARQ";
        private const string? SecretKey = "1ts-QfZmRVsxuA";

        public ClientTests()
        {
            _client = CreateClient();
        }

        [Fact]
        public void AccessKey_Null_Throws_ArgumentException()
        {
            Action act = () => new Client(null);
            
            act.Should().Throw<ArgumentNullException>()
                .WithMessage("accessKey must be provided (Parameter 'accessKey')");
        }

        [Fact]
        public void GenerateTakeUrl_TakeOptions_Null_Throws_ArgumentException()
        {
            var client = new Client(AccessKey);

            client.Invoking(x => client.GenerateTakeUrl(null!))
                .Should().Throw<ArgumentNullException>()
                .WithMessage("Value cannot be null. (Parameter 'takeOptions cannot be null')");
        }

        [Fact]
        public void GenerateTakeUrl_Is_Signed()
        {
            var options = TakeOptions.Html("<h1>Test</h1>");
            AssertTakeUrl(options,
                $"https://api.screenshotone.com/take?html=<h1>Test<%2Fh1>&access_key={AccessKey}&signature=7e4f15e9996e8350e40bc2f7b32fbd60e766011d133fef355fb98a6034e05301");
        }

        [Fact]
        public void GenerateTakeUrl_Is_Not_Signed()
        {
            var client = new Client(AccessKey);
            var options = TakeOptions.Html("<h1>Test</h1>");
            AssertTakeUrl(options,
                $"https://api.screenshotone.com/take?html=<h1>Test<%2Fh1>&access_key={AccessKey}",
                client);
        }
        
        [Fact]
        public void GenerateTakeUrl_Null_TakeOptions_Throws_ArgumentException()
        {
            _client.Invoking(x => x.GenerateTakeUrl(null!))
                .Should().Throw<ArgumentException>()
                .WithMessage("Value cannot be null. (Parameter 'takeOptions cannot be null')");
        }

        [Fact]
        public void GenerateTakeUrl_Contains_Correct_Key()
        {
            var d = new Dictionary<string, Func<TakeOptions, TakeOptions>>
            {
                { "selector", takeOptions => takeOptions.Selector("x") },
                { "format", takeOptions => takeOptions.Format(Format.GIF) },
                { "response_type", takeOptions => takeOptions.ResponseType(ResponseType.ByFormat) },
                { "full_page", takeOptions => takeOptions.FullPage(true) },
                { "full_page_scroll", takeOptions => takeOptions.FullPageScroll(true) },
                { "full_page_scroll_delay", takeOptions => takeOptions.FullPageScrollDelay(1) },
                { "full_page_scroll_by", takeOptions => takeOptions.FullPageScrollBy(1) },
                { "viewport_device", takeOptions => takeOptions.ViewportDevice("x") },
                { "viewport_width", takeOptions => takeOptions.ViewportWidth(1200) },
                { "viewport_height", takeOptions => takeOptions.ViewportHeight(1200) },
                { "device_scale_factor", takeOptions => takeOptions.DeviceScaleFactor(1) },
                { "viewport_mobile", takeOptions => takeOptions.ViewportMobile(true) },
                { "viewport_has_touch", takeOptions => takeOptions.ViewportHasTouch(true) },
                { "viewport_landscape", takeOptions => takeOptions.ViewportLandscape(true) },
                { "image_quality", takeOptions => takeOptions.ImageQuality(1) },
                { "image_width", takeOptions => takeOptions.ImageWidth(2) },
                { "image_height", takeOptions => takeOptions.ImageHeight(3) },
                { "omit_background", takeOptions => takeOptions.OmitBackground(false) },
                { "dark_mode", takeOptions => takeOptions.DarkMode(false) },
                { "reduced_motion", takeOptions => takeOptions.ReducedMotion(true) },
                { "media_type", takeOptions => takeOptions.MediaType(MediaType.Screen) },
                { "hide_selectors", takeOptions => takeOptions.HideSelectors("x") },
                { "scripts", takeOptions => takeOptions.Scripts("x") },
                { "scripts_wait_until", takeOptions => takeOptions.ScriptsWaitUntil(WaitUntil.Load, WaitUntil.DomContentLoaded) },
                { "styles", takeOptions => takeOptions.Styles("x") },
                { "click", takeOptions => takeOptions.Click("x") },
                { "block_cookie_banners", takeOptions => takeOptions.BlockCookieBanners(true) },
                { "block_chats", takeOptions => takeOptions.BlockChats(true) },
                { "block_ads", takeOptions => takeOptions.BlockAds(true) },
                { "block_trackers", takeOptions => takeOptions.BlockTrackers(true) },
                { "block_requests", takeOptions => takeOptions.BlockRequests("x") },
                { "block_resources", takeOptions => takeOptions.BlockResources(BlockResource.Other) },
                { "geolocation_latitude", takeOptions => takeOptions.GeolocationLatitude(1) },
                { "geolocation_longitude", takeOptions => takeOptions.GeolocationLongitude(2) },
                { "geolocation_accuracy", takeOptions => takeOptions.GeolocationAccuracy(3) },
                { "proxy", takeOptions => takeOptions.Proxy("x") },
                { "user_agent", takeOptions => takeOptions.UserAgent("x") },
                { "authorization", takeOptions => takeOptions.Authorization("x") },
                { "cookies", takeOptions => takeOptions.Cookies("x") },
                { "headers", takeOptions => takeOptions.Headers("x") },
                { "time_zone", takeOptions => takeOptions.TimeZone(TimeZone.EuropeLondon) },
                { "wait_until", takeOptions => takeOptions.WaitUntil(WaitUntil.Load, WaitUntil.Networkidle0) },
                { "delay", takeOptions => takeOptions.Delay(123) },
                { "timeout", takeOptions => takeOptions.Timeout(20) },
                { "wait_for_selector", takeOptions => takeOptions.WaitForSelector("x") },
                { "cache", takeOptions => takeOptions.Cache(true) },
                { "cache_ttl", takeOptions => takeOptions.CacheTtl(1234) },
                { "cache_key", takeOptions => takeOptions.CacheKey("x") },
                { "store", takeOptions => takeOptions.Store(true) },
                { "storage_path", takeOptions => takeOptions.StoragePath("x") },
                { "storage_bucket", takeOptions => takeOptions.StorageBucket("x") },
                { "storage_class", takeOptions => takeOptions.StorageClass(StorageClass.Standard) },
                { "error_on_selector_not_found", takeOptions => takeOptions.ErrorOnSelectorNotFound(true) }
            };

            var options = TakeOptions.Url("http://www.example.com");

            foreach (var func in d)
            {
                func.Value(options);

                AssertTakeUrlContains(options, func.Key);
            }
        }

        [Fact]
        public async Task Take_Returns_Expected_Content()
        {
            var expectedUri = new Uri($"https://api.screenshotone.com/take?url=https%3A%2F%2Fapple.com&full_page=true&block_resources=fetch&block_resources=image&format=jpg&access_key={AccessKey}&signature=8a195491b01443bd41167ec89874c50a284b3894a9729cd599027e07421e3b4a");
            var contentValue = Guid.NewGuid().ToString();
            var httpClient = CreateMockedHttpClient(expectedUri, HttpStatusCode.OK, contentValue);
            var client = CreateClient(httpClient);

            var options = TakeOptions.Url("https://apple.com")
                .FullPage(true)
                .BlockResources(BlockResource.Fetch, BlockResource.Image)
                .Format(Format.JPG);

            var result = await client.Take(options);

            result.Should().NotBeNull();
            Encoding.ASCII.GetString(result).Should().Be(contentValue);
        }

        [Fact]
        public async Task Take_Throws_HttpRequestException_On_NonSuccessStatusCode()
        {
            var expectedUri = new Uri($"https://api.screenshotone.com/take?url=https%3A%2F%2Fapple.com&access_key={AccessKey}&signature=066fd5ae85b9469989bcd3813177c01a803b0ef5c5de1fd5ca5793e990d3d615");
            var httpClient = CreateMockedHttpClient(expectedUri, HttpStatusCode.BadRequest);
            var client = CreateClient(httpClient);
            var options = TakeOptions.Url("https://apple.com");

            await client.Invoking(x => x.Take(options))
                .Should().ThrowAsync<HttpRequestException>()
                .WithMessage("Failed to take a screenshot, the server responded with 400 BadRequest");
        }

        private static Client CreateClient(HttpClient? httpClient = null) => new(AccessKey, SecretKey, httpClient);
        
        private static HttpClient CreateMockedHttpClient(Uri expectedUri, HttpStatusCode requiredStatusCode, string? requiredContent = null)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get
                        && req.RequestUri == expectedUri
                    ),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = requiredStatusCode,
                    Content = requiredContent != null ? new ByteArrayContent(Encoding.ASCII.GetBytes(requiredContent)) : null
                })
                .Verifiable();

            return new HttpClient(handlerMock.Object);
        }

        private void AssertTakeUrl(TakeOptions takeOptions, string expectedTakeUrl, Client? client = null) =>
            (client ?? _client).GenerateTakeUrl(takeOptions).Should().Be(expectedTakeUrl);

        private void AssertTakeUrlContains(TakeOptions takeOptions, string expectedTakeUrl, Client? client = null) =>
            (client ?? _client).GenerateTakeUrl(takeOptions).Should().Contain(expectedTakeUrl);
    }
}