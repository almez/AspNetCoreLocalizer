using System.Globalization;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Services;

namespace AspNetCoreLocalizer
{
    public class Localizer
    {

        #region Fields

        private ILocalizerService _localizerService;

        private static object _lock = new object();

        #endregion

        #region C'tor

        private Localizer()
        {
            _localizerService = new LocalizerService();
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
            get => _localizerService.GetLocalizedValue(key, culture);
            set => _localizerService.SetEntry(key, value, culture);
        }

        public string this[string key]
        {
            get => _localizerService.GetLocalizedValue(key, CultureInfo.CurrentCulture.Name);
        }

        #endregion

        public void ClearAll() => _localizerService.ClearAll();

    }
}
