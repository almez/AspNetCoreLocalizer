using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Attributes;
using Moq;
using Xunit;

namespace AspNetCoreLocalizer.Tests
{
    public class LocalizedDisplayNameAttributeTest
    {
        #region Fields

        private Mock<ILocalizerService> _mockLocalizerService;

        private LocalizedDisplayNameAttribute _localizedDisplayNameAttribute;

        #endregion

        [Fact(DisplayName = "LocalizedDisplayNameAttribute: Set the localized value of the provided resource key as DisplayName")]
        public void LocalizedDisplayName_WithExistingResourceKey_ReturnsTheLocalizedValue()
        {
            //Arrange
            //Arrange
            _mockLocalizerService = new Mock<ILocalizerService>();

            _mockLocalizerService.Setup(m => m.GetLocalizedValue("Email")).Returns("Email Address");

            this._localizedDisplayNameAttribute = new LocalizedDisplayNameAttribute(_mockLocalizerService.Object, "Email");

            //Act
            var result = _localizedDisplayNameAttribute.DisplayName;

            //Assert
            Assert.Equal("Email Address", result);
        }
    }
}
