using Localizer.Abstraction;
using Localizer.Services;

namespace Localizer
{
    public class Localizer
    {

        #region Fields

        private ILocalizerService _localizerService;

        private object _lock = new object();

        #endregion

        #region C'tor

        private Localizer()
        {
            _localizerService = new LocalizerService();
        }

        #endregion

        #region Singleton Implementation

        private Localizer _localizer;

        public Localizer Instance
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

        #endregion

    }
}
