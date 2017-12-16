using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Attributes;
using Moq;
using Xunit;

namespace AspNetCoreLocalizer.Tests
{
    public class LocalizedRequiredSelectAttributeTest
    {
        #region Fields

        private Mock<ILocalizerService> _mockLocalizerService;
        private LocalizedRequiredSelectAttribute _localizedRequiredSelectAttribute;

        #endregion

        #region Setup

        private void Initialize()
        {
        }

        #endregion

        [Fact(DisplayName = "LocalizedRequiredSelect: If no resource key providere, return localized value of default resource key")]
        public void LoclizedRequiredSelect_WithoutResourceKey_ReturnsLoccalizedValueOfDefualtResourceKeyInCurrentCulture()
        {
            //Arrange
            _mockLocalizerService = new Mock<ILocalizerService>();

            _mockLocalizerService.Setup(m => m.GetLocalizedValue(Constants.DefaultRequiredSelectAttributeResourceKey))
                .Returns("Default Localized Value");

            this._localizedRequiredSelectAttribute = new LocalizedRequiredSelectAttribute(_mockLocalizerService.Object);

            //Act
            var result = _localizedRequiredSelectAttribute.ErrorMessage;

            //Assert
            Assert.Equal("Default Localized Value", result);
        }

        [Fact(DisplayName = "LocalizedRequiredSelect: If no resource key providere, return localized value of that resource key")]
        public void LoclizedRequiredSelect_WithResourceKey_ReturnsLoccalizedValueOfThatResourceKeyInCurrentCulture()
        {
            //Arrange
            _mockLocalizerService = new Mock<ILocalizerService>();

            _mockLocalizerService.Setup(m => m.GetLocalizedValue(Constants.DefaultRequiredSelectAttributeResourceKey))
                .Returns("Default Localized Value");

            _mockLocalizerService.Setup(m => m.GetLocalizedValue("Custome RequiredSelect Resuorce Key"))
                .Returns("Custom Localized Value");

            this._localizedRequiredSelectAttribute = new LocalizedRequiredSelectAttribute(_mockLocalizerService.Object, "Custome RequiredSelect Resuorce Key");

            //Act
            var result = _localizedRequiredSelectAttribute.ErrorMessage;

            //Assert
            Assert.Equal("Custom Localized Value", result);
        }
    }
}
