using AspNetCoreLocalizer.Abstraction;

namespace AspNetCoreLocalizer.Services
{
    public class LocalizerService : ILocalizerService
    {
        public void SetEntry(string key, string value, string culture)
        {
            Configuration.LocalizationProvider.AddOrUpdateEntry(key, value, culture);
        }

        public string GetLocalizedValue(string key, string culture)
        {
            //todo : fail back logic should be here

            //// failback logic
            //if (Configuration.DefaultCulture != null && !String.Equals(Configuration.DefaultCulture.Name, culture, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    //todo : some log here

            //    return this.GetEntry(key, Configuration.DefaultCulture.Name);
            //}
            return Configuration.LocalizationProvider.GetEntry(key, culture)?.Value;
        }

        public void ClearAll()
        {
            Configuration.LocalizationProvider.ClearAll();
        }
    }
}
