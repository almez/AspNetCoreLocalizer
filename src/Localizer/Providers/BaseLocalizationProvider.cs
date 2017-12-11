using System;
using System.Collections.Generic;
using CachingManager;
using CachingManager.Abstraction;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Domain;
using AspNetCoreLocalizer.Exceptions;
using AspNetCoreLocalizer.Factories;
using System.Linq;

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
                throw new DuplicateEntryException($"An entry with same key and culture is already existed [{key}, {culture}]");
            }

            this.AddEntryToStore(entry);

            if (Configuration.CacheEnabled)
            {
                var cache = this.GetCacheByCulture(culture, true);

                if (cache != null)
                {
                    cache[key] = entry;
                }
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

            if (Configuration.CacheEnabled)
            {
                var cache = this.GetCacheByCulture(culture, true);

                if (cache != null)
                {
                    cache[key] = entry;
                }
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

            if (Configuration.CacheEnabled)
            {
                var cache = this.GetCacheByCulture(culture, false);

                if (cache != null)
                {
                    cache[key] = null;
                }
            }
        }

        public LocalizerEntry GetEntry(string key, string culture)
        {
            LocalizerEntry entry = null;

            if (Configuration.CacheEnabled)
            {
                var cache = this.GetCacheByCulture(culture, false);

                if (cache != null)
                {
                    entry = cache[key];
                }
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

        public void ClearAll()
        {
            if (Configuration.CacheEnabled)
            {
                var cacheList = (List<ICache>)CacheManager.Instance.ListCaches();
                cacheList = cacheList.Where(x => x.Name.StartsWith(Constants.CacheNamePrefix)).ToList();

                foreach (var cache in cacheList)
                {
                    cache.Clear();
                }
            }
            
            this.ClearAllFromStore();

        }

        #endregion

        #region Abstraction

        public abstract LocalizerEntry GetEntryFromStore(string key, string culture);

        public abstract void AddEntryToStore(LocalizerEntry entry);

        public abstract void UpdateEntryInStore(LocalizerEntry entry);

        public abstract void DeleteEntryFromStore(string id);

        public abstract void ClearAllFromStore();

        #endregion

        #region Private Methods

        private ICache<LocalizerEntry, string> GetCacheByCulture(string culture, bool createIfNotExist = false)
        {
            if (!Configuration.CacheEnabled)
            {
                throw new Exception("Cache is not enabled [Configuration.CacheEnabled]");
            }

            var cacheName = $"{Constants.CacheNamePrefix}{culture}";

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
