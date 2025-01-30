using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace ScreenshotOne
{
    public class TakeOptions
    {
        private TakeOptions(string key, string value)
        {
            Add(key, value);
        }

        private readonly Dictionary<string, List<string>> _query = new Dictionary<string, List<string>>();

        #region Essentials
        public static TakeOptions Url(string url) => new TakeOptions("url", url);

        public static TakeOptions Html(string html) => new TakeOptions("html", html);

        public static TakeOptions Markdown(string markdown) => new TakeOptions("markdown", markdown);

        /**
         * A CSS-like selector of the element to take a screenshot of. It is optional.
         */
        public TakeOptions Selector(string selector) => Add("selector", selector);

        /**
         * Sets response format, one of: "png", "jpeg", "webp" or "jpg".
         */
        public TakeOptions Format(Format format) => Add("format", format.GetDescriptionOrValue());

        /**
         * Available response types:
            
            by_format or empty — return only status or error. 
            
            It is suitable when you want to upload the screenshot to storage and don’t care about the results. It also speeds up the response since there are no networking costs involved.
         */
        public TakeOptions ResponseType(ResponseType responseType) => Add("response_type", responseType.ToString());
        #endregion

        #region Full Page
        /**
         * Renders the full page.
         */
        public TakeOptions FullPage(bool fullPage) => Add("full_page", fullPage.ToTrueFalseString());

        /**
         * If set to true, scrolls to the bottom of the page and back to the top. Default value is false.
         */
        public TakeOptions FullPageScroll(bool fullPageScroll) => Add("full_page_scroll", fullPageScroll.ToTrueFalseString());

        /**
         * The default value is 400 microseconds. Use it to specify how fast you want to scroll the page to the bottom.
         */
        public TakeOptions FullPageScrollDelay(int fullPageScrollDelay) => Add("full_page_scroll_delay", fullPageScrollDelay.ToString());

        /**
         * The default value is the height of the viewport. Use it to specify how fast you want to scroll the page to the bottom.
         */
        public TakeOptions FullPageScrollBy(int fullPageScrollBy) => Add("full_page_scroll_by", fullPageScrollBy.ToString());
        #endregion

        #region Viewport

        /**
         * Instead of manually specifying viewport parameters like width and height, you can specify a device to use for emulation. In addition, other parameters of the viewport, including the user agent, will be set automatically.
         */
        public TakeOptions ViewportDevice(string viewportDevice) => Add("viewport_device", viewportDevice);
        
        /**
         * Sets the width of the browser viewport (pixels).
         */
        public TakeOptions ViewportWidth(int viewportWidth) => Add("viewport_width", viewportWidth.ToString());

        /**
         * Sets the height of the browser viewport (pixels).
         */
        public TakeOptions ViewportHeight(int viewportHeight) => Add("viewport_height", viewportHeight.ToString());

        /**
         * Sets the device scale factor. The acceptable value is between the range of 1 and 5, including real numbers, like 2.25.
         */
        public TakeOptions DeviceScaleFactor(int deviceScaleFactor) => Add("device_scale_factor", deviceScaleFactor.ToString());

        /**
         * Whether the meta viewport tag is taken into account. Defaults to false.
         */
        public TakeOptions ViewportMobile(bool viewportMobile) => Add("viewport_mobile", viewportMobile.ToTrueFalseString());

        /**
         * The default value is false. Set to true if the viewport supports touch events.
         */
        public TakeOptions ViewportHasTouch(bool viewportHasTouch) => Add("viewport_has_touch", viewportHasTouch.ToTrueFalseString());

        /**
         * The default value is false. Set to true if the viewport is in landscape mode.
         * The parameter can override the value set by viewport_device option.
         */
        public TakeOptions ViewportLandscape(bool viewportLandscape) => Add("viewport_landscape", viewportLandscape.ToTrueFalseString());

        #endregion

        #region Image
        /**
         * Renders image with the specified quality. Available for the next formats: "jpeg" ("jpg"), "webp".
         */
        public TakeOptions ImageQuality(int imageQuality) => Add("image_quality", imageQuality.ToString());

        /**
         * The image_width and image_height parameters allow you to create a thumbnail of the screenshot, rendered HTML or PDF.
         */
        public TakeOptions ImageWidth(int imageWidth) => Add("image_width", imageWidth.ToString());

        /**
         * The image_width and image_height parameters allow you to create a thumbnail of the screenshot, rendered HTML or PDF.
         */
        public TakeOptions ImageHeight(int imageHeight) => Add("image_height", imageHeight.ToString());

        /**
         * Renders a transparent background for the image. Works only if the site has not defined background color.
         * Available for the following response formats: "png", "webp".
         */
        public TakeOptions OmitBackground(bool omitBackground) => Add("omit_background", omitBackground.ToTrueFalseString());
        #endregion

        #region Emulations
        /**
         * Set true to request site rendering, if supported, in the dark mode. Set false to request site rendering in the light mode if supported. If you don’t set the parameter. The site is rendered in the default mode.
         */
        public TakeOptions DarkMode(bool darkMode) => Add("dark_mode", darkMode.ToTrueFalseString());

        /**
         * When reduced_motion set to true, the API will request the site to minimize the amount of non-essential motion it uses. When the site supports it, it should use animations as least as possible.
         */
        public TakeOptions ReducedMotion(bool reducedMotion) => Add("reduced_motion", reducedMotion.ToTrueFalseString());

        /**
         * If you want to request the page and it is supported to be rendered for printers, specify print. If the page is by default rendered for printers and you want it to be rendered for screens, use screen.
         */
        public TakeOptions MediaType(MediaType mediaType) => Add("media_type", mediaType.ToString());

        #endregion

        #region Customization
        /**
         * The hide_selectors option allows hiding elements before taking a screenshot. You can specify as many selectors as you wish. All elements that match each selector will be hidden by setting the display style property to none !important.
         */
        public TakeOptions HideSelectors(params string[] hideSelectors) => Add("hide_selectors", hideSelectors);

        /**
         * scripts parameter allows to inject custom JavaScript and customize the page behavior.
         */
        public TakeOptions Scripts(string scripts) => Add("scripts", scripts);

        /**
         * The default value of scripts_wait_until is [] — nothing, no wait at all.
         * The scripts_wain_until option allows you to wait until a given set of events after the scripts were executed.
         */
        public TakeOptions ScriptsWaitUntil(params WaitUntil[] scriptsWaitUntil) => Add("scripts_wait_until", scriptsWaitUntil.Select(x => x.GetDescriptionOrValue()).ToList());


        /**
         * styles parameter allows to inject custom styles and customize the page. It might help generate beautiful images for the Open Graph protocol.
         */
        public TakeOptions Styles(string styles) => Add("styles", styles);

        /**
         * Specify the CSS selector of an element to click on before taking the screenshot. It could be anything, including a button, link, or even a regular div element.
         */
        public TakeOptions Click(string click) => Add("click", click);

        #endregion

        #region Blocking

        /**
         * Blocks cookie banners, GDPR overlay windows, and other privacy-related notices. Default value is false.
         * It is useful when you want to take “clean” screenshots.
         */
        public TakeOptions BlockCookieBanners(bool blockCookieBanners) => Add("block_cookie_banners", blockCookieBanners.ToTrueFalseString());

        /**
         * Blocks chats like Crisp, Facebook Messenger, Intercom, Drift, Tawk, User.com, Zoho SalesIQ and many others. Default value is false.
         * It is useful when you want to take “clean” screenshots.
         */

        public TakeOptions BlockChats(bool blockChats) => Add("block_chats", blockChats.ToTrueFalseString());

        /**
         * Blocks ads. Default value is false.
         */
        public TakeOptions BlockAds(bool blockAds) => Add("block_ads", blockAds.ToTrueFalseString());

        /**
         * Block trackers. Default value is false.
         */
        public TakeOptions BlockTrackers(bool blockTrackers) => Add("block_trackers", blockTrackers.ToTrueFalseString());

        /**
         * Blocks requests by specifying URL, domain, or even a simple pattern.
         */
        public TakeOptions BlockRequests(params string[] blockRequests) => Add("block_requests", blockRequests);


        /**
         * Blocks loading resources by type. Available resource types are: "document", "stylesheet", "image", "media",
         * "font", "script", "texttrack", "xhr", "fetch", "eventsource", "websocket", "manifest", "other".
         */
        public TakeOptions BlockResources(params BlockResource[] blockResources) => Add("block_resources", blockResources.Select(x => x.GetDescriptionOrValue()).ToList());
        #endregion

        #region Geolocation
        /**
         * Sets geolocation latitude for the request.
         * Both latitude and longitude are required if one of them is set.
         */
        public TakeOptions GeolocationLatitude(double latitude) => Add("geolocation_latitude", new decimal(latitude).ToString(CultureInfo.InvariantCulture));

        /**
         * Sets geolocation longitude for the request. Both latitude and longitude are required if one of them is set.
         */
        public TakeOptions GeolocationLongitude(double longitude) => Add("geolocation_longitude", new decimal(longitude).ToString(CultureInfo.InvariantCulture));

        /**
         * Sets the geolocation accuracy in meters.
         */
        public TakeOptions GeolocationAccuracy(int accuracy) => Add("geolocation_accuracy", accuracy.ToString());

        #endregion

        #region Request
        /**
         * You can use your custom proxy to take screenshots or render HTML with the proxy option.
         */
        public TakeOptions Proxy(string proxy) => Add("proxy", proxy);

        /**
         * Sets a user agent for the request.
         */
        public TakeOptions UserAgent(string userAgent) => Add("user_agent", userAgent);

        /**
         * Sets an authorization header for the request.
         */
        public TakeOptions Authorization(string authorization) => Add("authorization", authorization);

        /**
         * Set cookies for the request.
         */
        public TakeOptions Cookies(params string[] cookies) => Add("cookies", cookies);

        /**
         * Sets extra headers for the request.
         */
        public TakeOptions Headers(params string[] headers) => Add("headers", headers);

        /**
         * TimeZone sets time zone for the request.
         * Available time zones are: "America/Santiago", "Asia/Shanghai", "Europe/Berlin", "America/Guayaquil",
         * "Europe/Madrid", "Pacific/Majuro", "Asia/Kuala_Lumpur", "Pacific/Auckland", "Europe/Lisbon", "Europe/Kiev",
         * "Asia/Tashkent", "Europe/London".
         */
        public TakeOptions TimeZone(TimeZone timeZone) => Add("time_zone", timeZone.GetDescriptionOrValue());
        #endregion

        #region Wait
        /**
         * Use wait_until to wait until an event occurred before taking a screenshot or rendering HTML or PDF.
         * The default value of wait_until is ['load', 'domcontentloaded']
         */
        public TakeOptions WaitUntil(params WaitUntil[] waitUntil) => Add("wait_until", waitUntil.Select(x => x.GetDescriptionOrValue()).ToList());

        /**
         * Specify the delay option in seconds to wait before taking a screenshot or rendering HTML or PDF.
         */
        public TakeOptions Delay(int delay) => Add("delay", delay.ToString());

        /**
         * Specify timeout (in seconds) of when to abort the request if screenshot or rendering is still impossible. The default and max value is 30.
         */
        public TakeOptions Timeout(int timeout) => Add("timeout", timeout.ToString());

        /**
         * Specify wait_for_selector to wait until the element appears in DOM, which is not necessarily visible.
         */
        public TakeOptions WaitForSelector(string waitForSelector) => Add("wait_for_selector", waitForSelector);
        #endregion

        #region Caching
        /**
         * The cache option enables or disables caching of a screenshot, rendering HTML, or PDF. The default value is false.
         */
        public TakeOptions Cache(bool cache) => Add("cache", cache.ToTrueFalseString());

        /**
         * The cache_ttl option (in seconds) hints at how long the cached screenshot should be stored. The minimum value is 14400 seconds (4 hours), and the maximum value is 2592000 seconds (one month).
         */
        public TakeOptions CacheTtl(int cacheTtl) => Add("cache_ttl", cacheTtl.ToString());

        /**
         * Screenshots are cached by the combination of all specified request options. The cache_key option allows having different cached versions of the same screenshot.
         */
        public TakeOptions CacheKey(string cacheKey) => Add("cache_key", cacheKey);
        #endregion

        #region Storing
        /**
         * Default value is false. Use store=true to trigger upload of the taken screenshot, rendered HTML or PDF to the configured S3 bucket. Make sure you configured access to S3.
         */
        public TakeOptions Store(bool store) => Add("store", store.ToTrueFalseString());

        /**
         * The parameter is required if you set store=true. You must specify the key for the file, but don’t specify an extension, it will be added automatically based on the format you specified.
         */
        public TakeOptions StoragePath(string storagePath) => Add("storage_path", storagePath);

        /**
         * You can override the default bucket you configured with storage_bucket=<bucket name>.
         */
        public TakeOptions StorageBucket(string storageBucket) => Add("storage_bucket", storageBucket);

        /**
         * The default value is standard.
         * Storage class allows you to specify the object storage class.
         */
        public TakeOptions StorageClass(StorageClass storageClass) => Add("storage_class", storageClass.GetDescriptionOrValue());
        #endregion

        #region Error options
        /**
         * If a selector is specified and error_on_selector_not_found=true, the error will be returned if the element by selector is not visible or it took more than timeout seconds to render it, but not more than 30 seconds.
         */
        public TakeOptions ErrorOnSelectorNotFound(bool errorOn) => Add("error_on_selector_not_found", errorOn.ToTrueFalseString());
        #endregion
        
        public ReadOnlyDictionary<string, List<string>> Query => new ReadOnlyDictionary<string, List<string>>(_query);
        
        private TakeOptions Add(string key, params string[] values) => Add(key, values.ToList());
        
        private TakeOptions Add(string key, List<string> values)
        {
            _query.Add(key, values);
            return this;
        }
    }

    public enum Format
    {
        [Description("png")]
        PNG,
        [Description("jpeg")]
        JPEG,
        [Description("webp")]
        WEBP,
        [Description("jpg")]
        JPG,
        [Description("gif")]
        GIF,
        [Description("jp2")]
        JP2,
        [Description("tiff")]
        TIFF,
        [Description("avif")]
        AVIF,
        [Description("heif")]
        HEIF,
        [Description("html")]
        HTML,
        [Description("pdf")]
        PDF
    }

    public enum TimeZone
    {
        [Description("America/Santiago")]
        AmericaSantiago,
        [Description("Asia/Shanghai")]
        AsiaShanghai,
        [Description("Europe/Berlin")]
        EuropeBerlin,
        [Description("America/Guayaquil")]
        AmericaGuayaquil,
        [Description("Europe/Madrid")]
        EuropeMadrid,
        [Description("Pacific/Majuro")]
        PacificMajuro,
        [Description("Asia/Kuala_Lumpur")]
        AsiaKualaLumpur,
        [Description("Pacific/Auckland")]
        PacificAuckland,
        [Description("Europe/Lisbon")]
        EuropeLisbon,
        [Description("Europe/Kiev")]
        EuropeKiev,
        [Description("Asia/Tashkent")]
        AsiaTashkent,
        [Description("Europe/London")]
        EuropeLondon
    }

    public enum BlockResource
    {
        [Description("document")]
        Document,
        [Description("stylesheet")]
        StyleSheet,
        [Description("image")]
        Image,
        [Description("media")]
        Media,
        [Description("font")]
        Font,
        [Description("script")]
        Script,
        [Description("texttrack")]
        TextTrack,
        [Description("xhr")]
        XHR,
        [Description("fetch")]
        Fetch,
        [Description("eventsource")]
        EventSource,
        [Description("websocket")]
        Websocket,
        [Description("manifest")]
        Manifest,
        [Description("other")]
        Other
    }

    public enum ResponseType
    {
        [Description("by_format")]
        ByFormat,
        [Description("empty")] 
        Empty
    }

    public enum MediaType
    {
        [Description("print")]
        Print,
        [Description("screen")]
        Screen
    }

    public enum WaitUntil
    {
        [Description("load")]
        Load,
        [Description("domcontentloaded")]
        DomContentLoaded,
        [Description("networkidle0")]
        Networkidle0,
        [Description("networkidle2")]
        Networkidle2
    }

    public enum StorageClass
    {
        [Description("standard")]
        Standard,
        [Description("reduced_redundancy")]
        ReducedRedundancy,
        [Description("standard_ia")]
        StandardIA,
        [Description("onezone_ia")]
        OnezoneIA,
        [Description("intelligent_tiering")]
        IntelligentTiering,
        [Description("glacier")]
        Glacier,
        [Description("deep_archive")]
        DeepArchive,
        [Description("outposts")]
        Outposts,
        [Description("glacier_ir")]
        GlacierIR
    }
}