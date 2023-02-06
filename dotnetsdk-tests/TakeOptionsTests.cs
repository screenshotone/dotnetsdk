using FluentAssertions;
using ScreenshotOne;
using TimeZone = ScreenshotOne.TimeZone;

namespace dotnetsdk_tests
{
    public class TakeOptionsTests
    {
        [Fact]
        public void Url_Is_Set()
        {
            (TakeOptions.Url("http://www.example.com").Query["url"][0]).Should().Be("http://www.example.com");
        }

        [Fact]
        public void Html_Is_Set()
        {
            (TakeOptions.Html("<h1>Test</h1>").Query["html"][0]).Should().Be("<h1>Test</h1>");
        }

        [Fact]
        public void Markdown_Is_Set()
        {
            (TakeOptions.Markdown("*** Test ***").Query["markdown"][0]).Should().Be("*** Test ***");
        }

        [Fact]
        public void Options_Add_Uses_Correct_Name()
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
                func.Value(options).Query.Should().ContainKey(func.Key);
            }
        }
    }
}