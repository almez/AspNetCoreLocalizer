using System;
using Localizer.Abstraction;
using Localizer.Domain;

namespace Localizer.Providers
{
    public abstract class BaseLocalizationProvider : ILocalizationProvider
    {
        #region Public API

        public void AddOrUpdateEntry(string key, string value, string culture)
        {
            throw new NotImplementedException();
        }

        public void DeleteEntry(string key, string value, string culture)
        {
            throw new NotImplementedException();
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
