namespace Acme.Services
{
    public class LinkGenerator
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LinkGenerator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetBaseUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;

            if (request == null)
            {
                return string.Empty;
            }

            return $"{request.Scheme}://{request.Host}";
        }

        public string GenerateFormName(string formName)
        {
            var normalizedFormName = NormalizeName(formName);

            var url = GetBaseUrl();
            return $"{url}/api/formFilling/{normalizedFormName}";
        }

        public string NormalizeName(string text)
        {
            return text.Replace(" ", "-").ToLower();
        }
    }
}
