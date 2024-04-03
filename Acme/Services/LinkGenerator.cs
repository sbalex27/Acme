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
            var normalizedFormName = NormalizeText(formName);

            var url = GetBaseUrl();
            return $"{url}/fillForm/{normalizedFormName}";
        }

        public string NormalizeText(string text)
        {
            return text.Replace(" ", "-").ToLower();
        }
    }
}
