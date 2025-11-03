namespace NewsBiasChecker.Utils
{
    public static class UrlHelpers
    {
        public static string? OutletFrom(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return null;
            try
            {
                var host = new Uri(url).Host;
                return host.StartsWith("www.") ? host[4..] : host;
            }
            catch
            {
                return null;
            }
        }
    }
}
