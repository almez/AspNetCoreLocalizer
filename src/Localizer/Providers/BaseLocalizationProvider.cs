using System;
using System.Collections.Generic;
using CachingManager;
using CachingManager.Abstraction;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Domain;
using AspNetCoreLocalizer.Exceptions;
using AspNetCoreLocalizer.Factories;

namespace AspNetCoreLocalizer.Providers
{
    public abstract class BaseLocalizationProvider : ILocalizationProvider
    {

        #region Fields

        private object _lock  = new object();
        
        #endregion

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

            if (Exists(entry.Key, entry.Culture))
            {
                throw new DuplicateEntryException("An entry with same key and culture is already existed");
            }

            this.AddEntryToStore(entry);

            var cache = this.GetCacheByCulture(culture, true);

            if (cache != null)
            {
                cache[key] = entry;
            }
        }

        public void UpdateEntry(string key, string value, string culture)
        {
            var entry = this.GetEntry(key, culture);

            if (entry == null)
            {
                throw new EntryNotFoundException("An attempt to update not-existed entry, to to add it first.");
            }

            entry.Value = value;
            entry.LastUpdatedUtc = DateTime.UtcNow;

            this.UpdateEntryInStore(entry);

            var cache = this.GetCacheByCulture(culture, true);

            if (cache != null)
            {
                cache[key] = entry;
            }
        }

        public void DeleteEntry(string key, string culture)
        {
            var entry = this.GetEntry(key, culture);

            if (entry == null)
            {
                throw new EntryNotFoundException("An attempt to delete not-existed entry");
            }

            this.DeleteEntryFromStore(entry.Id);

            var cache = this.GetCacheByCulture(culture, false);

            if (cache != null)
            {
                cache[key] = null;
            }
        }

        public LocalizerEntry GetEntry(string key, string culture)
        {
            LocalizerEntry entry = null;

            var cache = this.GetCacheByCulture(culture, false);

            if (cache != null)
            {
                entry = cache[key];
            }

            if (entry == null)
            {
                entry = this.GetEntryFromStore(key, culture);
            }

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

        #region Private Methods

        private ICache<LocalizerEntry, string> GetCacheByCulture(string culture, bool createIfNotExist = false)
        {
            var cacheName = $"localizer_{culture}";

            var cache  = (ICache<LocalizerEntry, string>)CacheManager.Instance.FindCacheByName(cacheName);

            if (createIfNotExist)
            {
                lock (_lock)
                {
                    if (cache == null)
                    {
                        cache = CacheFactory.CreateCache<LocalizerEntry, string>(cacheName, Configuration.CacheLimitSize);
                    }
                }
            }
           
            return cache;
        }

        #endregion
    }
}
