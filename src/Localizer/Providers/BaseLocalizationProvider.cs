using Localizer.Abstraction;
using Localizer.Domain;
using Localizer.Factories;

namespace Localizer.Providers
{
    public abstract class BaseLocalizationProvider : ILocalizationProvider
    {
        #region Public API

        public void AddOrUpdateEntry(string key, string value, string culture)
        {
            if (Exists(key, culture))
            {
                this.UpdateEntry(key, value, culture);
            }
            else
            {
                this.AddEntry(key, value, culture);
            }
        }

        public void AddEntry(string key, string value, string culture)
        {
            var entry = LocalizerEntryFactory.CreateLocalizerEntry(key, value, culture);

            this.AddEntryToStore(entry);

            //todo: add to cache
        }

        public void UpdateEntry(string key, string value, string culture)
        {
            var entry = this.GetEntry(key, culture);

            this.UpdateEntryInStore(entry);

            //todo : update cache
        }

        public void DeleteEntry(string key, string culture)
        {
            var entry = this.GetEntry(key, culture);

            this.DeleteEntryFromStore(entry.Id);

            //todo : delete frmo cache
        }

        public LocalizerEntry GetEntry(string key, string culture)
        {
            //todo : try to get from cache

            var entry = this.GetEntryFromStore(key, culture);

            return entry;
        }

        public bool Exists(string key, string culture)
        {
            return GetEntry(key, culture) != null;
        }

        #endregion

        #region Abstraction

        public abstract LocalizerEntry GetEntryFromStore(string key, string culture);

        public abstract void AddEntryToStore(LocalizerEntry entry);

        public abstract void UpdateEntryInStore(LocalizerEntry entry);

        public abstract void DeleteEntryFromStore(string id);

        #endregion
    }
}
