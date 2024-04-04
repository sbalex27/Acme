using Acme.Services;
using Microsoft.Extensions.Configuration;

//using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
//using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class SecurityToolsTest
    {
        private SecurityTools _securityTools;

        [SetUp]
        public void Setup()
        {

            var myConfiguration = new ConfigurationManager();
            myConfiguration.AddInMemoryCollection([
                new KeyValuePair<string, string?>("Security:Pepper", "pepper"),
                new KeyValuePair<string, string?>("Jwt:Key", "SecretKeyForAuthenticationOfDevelApplication"),
                new KeyValuePair<string, string?>("Jwt:Issuer", "Devel.com")
                ]);

            _securityTools = new SecurityTools(myConfiguration);
        }

        [Test]
        public void GenerateSaltTest()
        {
            // Arrange
            var expected = 24;

            // Act
            var actual = _securityTools.GenerateSalt();

            // Assert
            Assert.That(actual, Has.Length.EqualTo(expected));
        }

        [Test]
        public void GetPepperTest()
        {
            // Arrange
            var expected = "pepper";

            // Act
            var actual = _securityTools.GetPepper();

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void ComputeHashTest()
        {
            // Arrange
            var password = "password";
            var salt = "salt";
            var expectedLength = 44;

            // Act
            var actual = _securityTools.ComputeHash(password, salt);

            // Assert
            Assert.That(actual, Has.Length.EqualTo(expectedLength));
        }

        [Test]
        public void ComputeHashTestWithIteration()
        {
            // Arrange
            var password = "Hello World 123";
            var salt = _securityTools.GenerateSalt();
            var iteration = 5;
            var expectedLength = 44;

            // Act
            var actual = _securityTools.ComputeHash(password, salt, iteration);

            // Assert
            Assert.That(actual, Has.Length.EqualTo(expectedLength));
        }

        [Test]
        public void CompareHashTrueTest()
        {
            // Arrange
            var salt = _securityTools.GenerateSalt();
            var hash = _securityTools.ComputeHash("Hello World 123", salt);

            // Act
            var actual = _securityTools.CompareHash("Hello World 123", salt, hash);

            // Assert
            Assert.That(actual, Is.True);
        }

        [Test]
        public void CompareHashFalseTest()
        {
            // Arrange
            var salt = _securityTools.GenerateSalt();
            var hash = _securityTools.ComputeHash("Hello World 123", salt);

            // Act
            var actual = _securityTools.CompareHash("Hello World 1234", salt, hash);

            // Assert
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GenerateJwtPartsTest()
        {
            // Arrange
            var expectedParts = 3;

            // Act
            var actual = _securityTools.GenerateJwt();

            // Assert
            Assert.That(actual.Split('.'), Has.Length.EqualTo(expectedParts));
        }
    }
}
