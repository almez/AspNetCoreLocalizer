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
            //todo : Log if there's a missing here.

            return Configuration.LocalizationProvider.GetEntry(key, culture)?.Value;
        }
    }
}
