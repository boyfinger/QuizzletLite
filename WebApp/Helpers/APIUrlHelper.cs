namespace WebApp.Helpers
{
    public class APIUrlHelper
    {
        private static string _baseUrl = null!;

        public static void Initialize(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _baseUrl = configuration["GivenAPIBaseUrl"]
                ?? throw new InvalidOperationException("GivenAPIBaseUrl is not configured in appsettings.json");
        }

        public static string GetBaseUrl()
        {
            return _baseUrl;
        }
    }
}
