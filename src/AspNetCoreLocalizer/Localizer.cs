using System;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Services;

namespace AspNetCoreLocalizer
{
    public class Localizer
    {
        #region Fields

        private ILocalizationProvider _localizationProvider;

        private ILocalizerService _localizerService;

        private static object _lock = new object();

        #endregion

        #region Propertieis

        public ILocalizationProvider LocalizationProvider
        {
            set => _localizationProvider = value;
            get
            {
                if (_localizationProvider == null)
                {
                    throw new Exception("Localization provider has not been configured yet.")
                    {
                        HelpLink = "https://github.com/almez/AspNetCoreLocalizer"
                    };
                }
                return _localizationProvider;
            }
        }

        public ILocalizerService LocalizerService
        {
            set => _localizerService = value;
            get
            {
                if (_localizerService == null)
                {
                    _localizerService = new LocalizerService(this.LocalizationProvider);
                }
                return _localizerService;
            }
        }

        #endregion

        #region C'tor

        private Localizer()
        {

        }

        #endregion

        #region Singleton Implementation

        private static Localizer _localizer;

        public static Localizer Instance
        {
            set => _localizer = value;
            get
            {
                lock (_lock)
                {
                    if (_localizer == null)
                    {
                        _localizer = new Localizer();
                    }
                }

                return _localizer;
            }
        }

        #endregion

        #region Indexers

        public string this[string key, string culture]
        {
            get => this.LocalizerService.GetLocalizedValue(key, culture);
            set => this.LocalizerService.SetEntry(key, value, culture);
        }

        public string this[string key]
        {
            get => this.LocalizerService.GetLocalizedValue(key);
        }

        #endregion

        #region Public API's

        public void ClearAll() => this.LocalizerService.ClearAll();

        #endregion

        #region Internal Methods

        internal void RegisterProvider(ILocalizationProvider localizationProvider)
        {
            this.LocalizerService = new LocalizerService(localizationProvider);
        }

        #endregion

    }
}
