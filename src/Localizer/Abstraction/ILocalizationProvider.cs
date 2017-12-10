using Localizer.Domain;

namespace Localizer.Abstraction
{
    public interface ILocalizationProvider
    {
        #region Public API

        void AddOrUpdateEntry(string key, string value, string culture);
        void DeleteEntry(string key, string value, string culture);

        #endregion

        #region Abstract

        LocalizerEntry GetEntryFromStore(string key, string culture);
        void AddEntryToStore(LocalizerEntry entry);
        void UpdateEntryInStore(LocalizerEntry entry);
        void DeleteEntryFromStore(string id);

        #endregion
    }
}
