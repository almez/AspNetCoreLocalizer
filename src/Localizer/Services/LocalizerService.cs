using System;
using System.Globalization;
using AspNetCoreLocalizer.Abstraction;

namespace AspNetCoreLocalizer.Services
{
    public class LocalizerService : ILocalizerService
    {
        #region Fields

        private readonly ILocalizationProvider _localizationProvider;

        #endregion

        #region C'tor

        public LocalizerService(ILocalizationProvider localizationProvider)
        {
            this._localizationProvider = localizationProvider;
        }

        #endregion

        #region Public Methods

        public void SetEntry(string key, string value, string culture)
        {
            this._localizationProvider.AddOrUpdateEntry(key, value, culture);
        }

        public string GetLocalizedValue(string key, string culture)
        {
            var result = this._localizationProvider.GetEntry(key, culture);

            // fallback logic
            if (result == null & Configuration.FallbackEnabled)
            {
                CultureInfo cultureInfo = new CultureInfo(culture);
                do
                {
                    if (cultureInfo.Name != cultureInfo.Parent.Name)
                    {
                        //todo : some log here
                        result = this._localizationProvider.GetEntry(key, cultureInfo.Parent.Name);
                    }
                    cultureInfo = cultureInfo.Parent;

                } while (cultureInfo.Name != cultureInfo.Parent.Name && result == null);
            }

            return result?.Value;
        }

        public void ClearAll()
        {
            this._localizationProvider.ClearAll();
        }

        #endregion
    }
}
