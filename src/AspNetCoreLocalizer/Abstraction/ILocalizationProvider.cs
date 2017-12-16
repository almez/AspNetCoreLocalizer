using AspNetCoreLocalizer.Domain;

namespace AspNetCoreLocalizer.Abstraction
{
    public interface ILocalizationProvider
    {
        #region Provider configuration

        /// <summary>
        /// Whether localization cache is enabled or not.
        /// </summary>
        bool CacheEnabled { get; set; }

        /// <summary>
        /// Cache size-limit per culture
        /// </summary>
        long CacheLimitSize { get; set; }

        /// <summary>
        /// If enabled, and the localized value for specified key is not existed in the current culture, the provider will search in anscestor cultures.
        /// </summary>
        bool FallbackEnabled { set; get; }

        #endregion

        #region Public API

        /// <summary>
        /// Adds the specified values if it's not existed previously, updates if exists. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="value">Resource Value</param>
        /// <param name="culture">Resource Culture</param>
        void AddOrUpdateEntry(string key, string value, string culture);

        /// <summary>
        /// Adds the specified entry. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="value">Resource Value</param>
        /// <param name="culture">Resource Culture</param>
        void AddEntry(string key, string value, string culture);

        /// <summary>
        /// update the specified values. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="value">Resource Value</param>
        /// <param name="culture">Resource Culture</param>
        void UpdateEntry(string key, string value, string culture);

        /// <summary>
        /// Delete the entry. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="culture">Resource Culture</param>
        void DeleteEntry(string key, string culture);

        /// <summary>
        /// Fetches the LocalizerEntry by key and culture. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="culture">Resource Culture</param>
        LocalizerEntry GetEntry(string key, string culture);

        /// <summary>
        /// Checks whether the specified entry is existed or not. (Cache will be involved if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="culture">Resource Culture</param>
        bool Exists(string key, string culture);

        /// <summary>
        /// Clear all localization entries from the store (Cache will be involved if it's enabled)
        /// </summary> 
        void ClearAll();

        #endregion

        #region Abstract

        /// <summary>
        /// Fetches the localizerEntry from the store directly. (Cache will NOT be involved even if it's enabled)
        /// </summary>
        /// <param name="key">Resource Key</param>
        /// <param name="culture">Resource Culture</param>
        LocalizerEntry GetEntryFromStore(string key, string culture);

        /// <summary>
        /// Adds the specified entry to the store. (Cache will NOT be involved even if it's enabled)
        /// </summary>
        /// <param name="entry">Resource Entry</param>
        void AddEntryToStore(LocalizerEntry entry);

        /// <summary>
        /// update the specified values in the store. (Cache will NOT be involved even if it's enabled)
        /// </summary>
        /// <param name="entry"></param>
        void UpdateEntryInStore(LocalizerEntry entry);

        /// <summary>
        /// Delete the entry from the store. (Cache will NOT be involved even if it's enabled)
        /// </summary>
        /// <param name="id"></param>
        void DeleteEntryFromStore(string id);

        /// <summary>
        ///  Clear all entries from the store. (Cache will NOT be involved even if it's enabled)
        /// </summary>
        void ClearAllFromStore();

        #endregion
    }
}
