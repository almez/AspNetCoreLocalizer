using System;
using AspNetCoreLocalizer.Abstraction;

namespace AspNetCoreLocalizer.Services
{
    public class LocalizerService : ILocalizerService
    {
        #region Fields

        private readonly ILocalizationProvider _provider;

        #endregion

        #region C'tor

        public LocalizerService()
        {
            this._provider = Configuration.LocalizationProvider;
        }

        #endregion

        #region Public Methods

        public void SetEntry(string key, string value, string culture)
        {
            this._provider.AddOrUpdateEntry(key, value, culture);
        }

        public string GetLocalizedValue(string key, string culture)
        {
            var result = this._provider.GetEntry(key, culture);

            // failback logic
            if (Configuration.DefaultCulture != null && !String.Equals(Configuration.DefaultCulture.Name, culture, StringComparison.CurrentCultureIgnoreCase))
            {
                //todo : some log here
                result =  this._provider.GetEntry(key, Configuration.DefaultCulture.Name);
            }

            return result?.Value;
        }

        public void ClearAll()
        {
            this._provider.ClearAll();
        }

        #endregion
    }
}
