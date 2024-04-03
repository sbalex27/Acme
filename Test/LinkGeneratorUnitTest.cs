using Acme.Services;
using Microsoft.AspNetCore.Http;

namespace Test
{
    public class LinkGeneratorUnitTest
    {
        private LinkGenerator _linkGenerator;
        private IHttpContextAccessor _httpContextAccessor;

        [SetUp]
        public void Setup()
        {
            _httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };

            _httpContextAccessor.HttpContext!.Request.Scheme = "http";
            _httpContextAccessor.HttpContext!.Request.Host = new HostString("localhost");

            _linkGenerator = new LinkGenerator(_httpContextAccessor);
        }

        [Test]
        public void BaseUrl()
        {
            // Arrange
            var expected = "http://localhost";

            // Act
            var result = _linkGenerator.GetBaseUrl();

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FullUrl()
        {
            // Arrange
            var expected = "http://localhost/fillForm/my-form-created";

            // Act
            var result = _linkGenerator.GenerateFormName("My Form Created");

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("My Form Created", "my-form-created")]
        [TestCase("Foo bar", "foo-bar")]
        [TestCase("Foo", "foo")]
        [TestCase("Bar", "bar")]
        [TestCase("My second form 2", "my-second-form-2")]
        public void NormalizeText(string input, string expected)
        {
            // Act
            var result = _linkGenerator.NormalizeText(input);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}