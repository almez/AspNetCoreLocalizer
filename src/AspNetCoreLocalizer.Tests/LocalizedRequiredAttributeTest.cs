using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Attributes;
using Moq;
using Xunit;

namespace AspNetCoreLocalizer.Tests
{
    public class LocalizedRequiredAttributeTest
    {
        #region Fields

        private Mock<ILocalizerService> _mockLocalizerService;
        private LocalizedRequiredAttribute _localizedRequiredAttribute;

        #endregion

        #region Setup

        private void Initialize()
        {
        }

        #endregion

        [Fact(DisplayName = "LocalizedRequired: If no resource key providere, return localized value of default resource key")]
        public void LoclizedRequired_WithoutResourceKey_ReturnsLoccalizedValueOfDefualtResourceKeyInCurrentCulture()
        {
            //Arrange
            _mockLocalizerService = new Mock<ILocalizerService>();

            _mockLocalizerService.Setup(m => m.GetLocalizedValue(Constants.DefaultRequiredAttributeResourceKey))
                .Returns("Default Localized Value");

            this._localizedRequiredAttribute = new LocalizedRequiredAttribute(_mockLocalizerService.Object);

            //Act
            var result = _localizedRequiredAttribute.ErrorMessage;

            //Assert
            Assert.Equal("Default Localized Value", result);
        }

        [Fact(DisplayName = "LocalizedRequired: If no resource key providere, return localized value of that resource key")]
        public void LoclizedRequired_WithResourceKey_ReturnsLoccalizedValueOfThatResourceKeyInCurrentCulture()
        {
            //Arrange
            _mockLocalizerService = new Mock<ILocalizerService>();

            _mockLocalizerService.Setup(m => m.GetLocalizedValue(Constants.DefaultRequiredAttributeResourceKey))
                .Returns("Default Localized Value");

            _mockLocalizerService.Setup(m => m.GetLocalizedValue("Custome Required Resuorce Key"))
                .Returns("Custom Localized Value");

            this._localizedRequiredAttribute = new LocalizedRequiredAttribute(_mockLocalizerService.Object, "Custome Required Resuorce Key");

            //Act
            var result = _localizedRequiredAttribute.ErrorMessage;

            //Assert
            Assert.Equal("Custom Localized Value", result);
        }
    }
}
