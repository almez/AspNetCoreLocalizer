using System.Collections.Generic;
using System.Linq;
using AspNetCoreLocalizer.Domain;

namespace AspNetCoreLocalizer.Providers
{
    public class InMemoryLocalizationProvider : BaseLocalizationProvider
    {
        #region Fields

        public List<LocalizerEntry> LocalizerEntries { get; set; }

        #endregion

        #region C'tor

        public InMemoryLocalizationProvider()
        {
            this.LocalizerEntries = new List<LocalizerEntry>();
        }

        #endregion

        #region Overriden Methods

        public override LocalizerEntry GetEntryFromStore(string key, string culture)
        {
            return this.LocalizerEntries.SingleOrDefault(x => x.Key == key && x.Culture == culture);
        }

        public override void AddEntryToStore(LocalizerEntry entry)
        {
            this.LocalizerEntries.Add(entry);
        }

        public override void UpdateEntryInStore(LocalizerEntry entry)
        {
            var entryInStore = this.LocalizerEntries.SingleOrDefault(x => x.Id == entry.Id);
            this.LocalizerEntries.Remove(entryInStore);
            this.LocalizerEntries.Add(entry);

        }

        public override void DeleteEntryFromStore(string id)
        {
            var entryInStore = this.LocalizerEntries.SingleOrDefault(x => x.Id == id);
            this.LocalizerEntries.Remove(entryInStore);
        }

        public override void ClearAllFromStore()
        {
            this.LocalizerEntries.Clear();
            this.LocalizerEntries = new List<LocalizerEntry>();
        }

        #endregion
    }
}
