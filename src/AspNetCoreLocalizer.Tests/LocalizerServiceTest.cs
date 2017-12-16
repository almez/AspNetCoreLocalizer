using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Domain;
using AspNetCoreLocalizer.Services;
using Moq;
using Xunit;

namespace AspNetCoreLocalizer.Tests
{
    public class LocalizerServiceTest
    {
        #region Fields

        private Mock<ILocalizationProvider> _mockLocalizationProvider;
        private LocalizerService _localizerService;

        #endregion

        #region C'tor

        public LocalizerServiceTest()
        {
            
        }

        #endregion

        #region Setup

        private void Initialize()
        {
            _mockLocalizationProvider = new Mock<ILocalizationProvider>();
            _localizerService = new LocalizerService(_mockLocalizationProvider.Object);

            this._mockLocalizationProvider.Setup(p => p.FallbackEnabled).Returns(true);
        }

        #endregion

        #region SetEntry()

        [Fact(DisplayName = "LocalizerService: service.SetEntry() calls provider.AddOrUpdateEntry() Once")]
        public void SetEntry_WithValidInput_ShouldCallProvidersAddOrUpdateEntryOnce()
        {
            //Arrange
            this.Initialize();

            var key = "key";
            var value = "value";
            var culture = "culture";
            _mockLocalizationProvider.Setup(p => p.AddOrUpdateEntry(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //Act
            _localizerService.SetEntry(key, value, culture);

            //Assert
            _mockLocalizationProvider.Verify(p=> p.AddOrUpdateEntry(key, value, culture), Times.Once);
        }

        #endregion

        #region GetLocalizedValue()

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with existing entry in specific culture calls provider.GetEntry() Once")]
        public void GetLocalizedValue_WithExistingEntryInSpecificCulture_CallsProvidersGetEntryOnce()
        {
            //Arrange
            this.Initialize();

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "en-US")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (en-US)",
                Culture = "en-US"
            });

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", "en-US"), Times.Once);
        }

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with not existed entry in specific culture && Disabled config.fallback calls provider.GetEntry() Once")]
        public void GetLocalizedValue_WithNonExistedEntryInSpecificCultureAndFallBackDisabled_CallsProvidersGetEntryOnce()
        {
            //Arrange
            this.Initialize();

            this._mockLocalizationProvider.Setup(p => p.FallbackEnabled).Returns(false);

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", "en-US"), Times.Once);
        }

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with not existed entry in specific culture && enabled config.fallback calls provider.GetEntry() more than nce")]
        public void GetLocalizedValue_WithNonExistedEntryInEnUsCultureAndFallBackEnabled_CallsProvidersGetEntryThreeTimes()
        {
            //Arrange
            this.Initialize();

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", It.IsAny<string>()));

            this._mockLocalizationProvider.Setup(p => p.FallbackEnabled).Returns(true);

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", "en-US"), Times.Once);
            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", "en"), Times.Once);
            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", ""), Times.Once);

            _mockLocalizationProvider.Verify(p => p.GetEntry("Welcome", It.IsAny<string>()), Times.Exactly(3));
        }

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with existing entry in specific culture returns value in that particular culture")]
        public void GetLocalizedValue_WithExistingEntryInSpecificCulture_ReturnsTheValueInThatCulture()
        {
            //Arrange
            this.Initialize();

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "en-US")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (en-US)",
                Culture = "en-US"
            });

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "en")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (en)",
                Culture = "en"
            });

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (Invariant)",
                Culture = ""
            });

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            Assert.Equal("Welcome (en-US)", result);
        }

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with existing entry in parent culture  not in specified culture returns value from that parent culture")]
        public void GetLocalizedValue_WithExistingEntryInParentCultureButNotInSpecifiedCulture_ReturnsTheValueInParentCulture()
        {
            //Arrange
            this.Initialize();

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "en")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (en)",
                Culture = "en"
            });

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (Invariant)",
                Culture = ""
            });

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            Assert.Equal("Welcome (en)", result);
        }

        [Fact(DisplayName = "LocalizerService: service.GetLocalizedValue() with existing entry in invariant culture  not in specified culture nor in its parents returns value from invariant culture")]
        public void GetLocalizedValue_WithExistingEntryInInvariantCultureButNotInSpecifiedCultureNortInItsParents_ReturnsTheValueInInvariantCulture()
        {
            //Arrange
            this.Initialize();

            _mockLocalizationProvider.Setup(p => p.GetEntry("Welcome", "")).Returns(new LocalizerEntry()
            {
                Key = "Welcome",
                Value = "Welcome (Invariant)",
                Culture = ""
            });

            //Act
            var result = _localizerService.GetLocalizedValue("Welcome", "en-US");

            //Assert
            Assert.Equal("Welcome (Invariant)", result);
        }

        #endregion
    }
}
