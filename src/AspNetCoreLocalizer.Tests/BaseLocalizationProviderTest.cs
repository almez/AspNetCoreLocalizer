using System;
using System.Collections.Generic;
using System.Linq;
using AspNetCoreLocalizer.Domain;
using AspNetCoreLocalizer.Exceptions;
using AspNetCoreLocalizer.Factories;
using AspNetCoreLocalizer.Providers;
using CachingManager;
using Xunit;

namespace AspNetCoreLocalizer.Tests
{
    public class BaseLocalizationProvider
    {

        #region Fields

        private InMemoryLocalizationProvider _provider;

        #endregion

        #region C'tor

        public BaseLocalizationProvider() { }

        #endregion

        #region Setup

        public void InitializeProvider()
        {
            this._provider = new InMemoryLocalizationProvider();
            CacheManager.Instance.ClearAll();
        }

        #endregion

        #region AddOrUpdate()

        //[Fact(DisplayName = "BaseLocalizationProvider: AddOrUpdate() with new entry performs adding.")]
        //public void AddOrUpdateEntry_WithNewEntry_PerformsAdding()
        //{
        //    //Arrange
        //    this.InitializeProvider();

        //    //Act
        //    _provider.AddOrUpdateEntry("Welcome", "Merhaba", "tr-TR");

        //    //Assert
        //    var result = _provider.LocalizerEntries.Count;

        //    Assert.Equal(1, result);
        //}


        [Fact(DisplayName = "BaseLocalizationProvider: AddOrUpdate() with existing entry performs updating.")]
        public void AddOrUpdateEntry_WithExistingEntry_PerformsUpdating()
        {
            //Arrange
            this.InitializeProvider();

            var entry = LocalizerEntryFactory.CreateLocalizerEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.AddOrUpdateEntry(entry.Key, entry.Value, entry.Culture);
            _provider.AddOrUpdateEntry(entry.Key, entry.Value, entry.Culture);

            //Assert
            var result = _provider.LocalizerEntries.Count;

            Assert.Equal(1, result);
        }

        #endregion

        #region AddEntry()

        [Fact(DisplayName = "BaseLocalizationProvider: AddEntry() increments entries count by 1")]
        public void AddEntry_WithNewEntry_IncrementsEntriesCountByOne()
        {
            //Arrange
            this.InitializeProvider();

            var random = new Random();

            //Act
            Enumerable.Range(0, random.Next(10)).ToList().ForEach(i =>
            {
                _provider.AddOrUpdateEntry($"test_key_{i}", $"test_value_{i}", (i % 2 == 0 ? "en-US" : "tr-TR"));
            });
            var countBeforeAddEntryMethod = _provider.LocalizerEntries.Count;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Assert
            var countAfterAddEntryMethod = _provider.LocalizerEntries.Count;

            Assert.Equal(1, (countAfterAddEntryMethod - countBeforeAddEntryMethod));
        }

        [Fact(DisplayName = "BaseLocalizationProvider: AddEntry() raises exception with existing entry")]
        public void AddEntry_WithEXistingEntry_RaisesException()
        {
            //Arrange
            this.InitializeProvider();

            //Act
            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Assert

            Assert.Throws<DuplicateEntryException>(() =>
            {
                _provider.AddEntry("Welcome", "Merhaba", "tr-TR");
            });
        }

        [Fact(DisplayName = "BaseLocalizationProvider: AddEntry() adds entry to cache if CacheEnabled")]
        public void AddEntry_CacheEnabled_AddsEntryToCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = true;

            //Act
            _provider.AddEntry("Welcome", "Welcome (TR)", "tr-TR");
            _provider.AddEntry("Welcome", "Welcome (AR)", "ar-AR");
            _provider.AddEntry("Welcome", "Welcome (En)", "en-US");
            _provider.AddEntry("Welcome", "Welcome (Invariant)", "");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var welcomeTr = _provider.GetEntry("Welcome", "tr-TR")?.Value;
            var welcomeAr = _provider.GetEntry("Welcome", "ar-AR")?.Value;
            var welcomeEn = _provider.GetEntry("Welcome", "en-US")?.Value;
            var welcomeInvariant = _provider.GetEntry("Welcome", "")?.Value;

            Assert.Equal(welcomeTr, "Welcome (TR)");
            Assert.Equal(welcomeAr, "Welcome (AR)");
            Assert.Equal(welcomeEn, "Welcome (En)");
            Assert.Equal(welcomeInvariant, "Welcome (Invariant)");
        }

        [Fact(DisplayName = "BaseLocalizationProvider: AddEntry() adds nothing to cache if CacheDisbled")]
        public void AddEntry_CacheDisabled_AddsNothingToCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = false;

            //Act
            _provider.AddEntry("Welcome", "Welcome (TR)", "tr-TR");
            _provider.AddEntry("Welcome", "Welcome (AR)", "ar-AR");
            _provider.AddEntry("Welcome", "Welcome (En)", "en-US");
            _provider.AddEntry("Welcome", "Welcome (Invariant)", "");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var welcomeTr = _provider.GetEntry("Welcome", "tr-TR");
            var welcomeAr = _provider.GetEntry("Welcome", "ar-AR");
            var welcomeEn = _provider.GetEntry("Welcome", "en-US");
            var welcomeInvariant = _provider.GetEntry("Welcome", "");

            Assert.Null(welcomeTr);
            Assert.Null(welcomeAr);
            Assert.Null(welcomeEn);
            Assert.Null(welcomeInvariant);
        }

        #endregion

        #region UpdateEntry()

        [Fact(DisplayName = "BaseLocalizationProvider: UpdateEntry() updates the existing entry")]
        public void UpdateEntry_WithExistingEntry_UpdatesTheEntry()
        {
            //Arrange
            this.InitializeProvider();

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.UpdateEntry("Welcome", "hoş geldiniz", "tr-TR");

            //Assert
            var result = _provider.GetEntry("Welcome", "tr-TR");

            Assert.Equal("hoş geldiniz", result.Value);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: UpdateEntry() raises exception with not existed entry")]
        public void UpdateEntry_WithNotExistingEntry_RaisesException()
        {
            //Arrange
            this.InitializeProvider();

            //Act & Assert
            Assert.Throws<EntryNotFoundException>(() =>
            {
                _provider.UpdateEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "tr-TR");
            });
        }

        [Fact(DisplayName = "BaseLocalizationProvider: UpdateEntry() updates cache if CacheEnabled")]
        public void UpdateEntry_CacheEnabled_UpdatesCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = true;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.UpdateEntry("Welcome", "hoş geldiniz", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var result = _provider.GetEntry("Welcome", "tr-TR");

            Assert.NotNull(result);
            Assert.Equal("hoş geldiniz", result.Value);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: UpdateEntry() updates nothing to cache if CacheDisabled")]
        public void UpdateEntry_CacheDisabled_DoesntUpdatesCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = false;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.UpdateEntry("Welcome", "hoş geldiniz", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var result = _provider.GetEntry("Welcome", "tr-TR");

            Assert.Null(result);
        }

        #endregion

        #region DeleteEntry()

        [Fact(DisplayName = "BaseLocalizationProvider: DeleteEntry() decrements the entries count by one")]
        public void DeleteEntry_WithExistingEntry_DecrementsTheEntriesByOne()
        {
            //Arrange
            this.InitializeProvider();

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");
            var countBeforeDelete = _provider.LocalizerEntries.Count;

            //Act
            _provider.DeleteEntry("Welcome", "tr-TR");

            //Assert
            var countAfterDelete = _provider.LocalizerEntries.Count;

            Assert.Equal(1, (countBeforeDelete - countAfterDelete));
        }

        [Fact(DisplayName = "BaseLocalizationProvider: DeleteEntry() raises exception with not existed entry")]
        public void DeleteEntry_WithNotExistingEntry_RaisesException()
        {
            //Arrange
            this.InitializeProvider();

            //Act & Assert
            Assert.Throws<EntryNotFoundException>(() =>
            {
                _provider.DeleteEntry(Guid.NewGuid().ToString(), "tr-TR");
            });
        }

        [Fact(DisplayName = "BaseLocalizationProvider: DeleteEntry() deletes entry from cache if cache enabled")]
        public void DeleteEntry_CacheEnabled_DeletesEntryFromCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = true;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.DeleteEntry("Welcome", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var result = _provider.GetEntry("Welcome", "tr-TR");

            Assert.Null(result);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: DeleteEntry() nothing to delete from cache if cache disabled")]
        public void DeleteEntry_CacheDisabled_NothingToDeleteFromCache()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = false;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            _provider.DeleteEntry("Welcome", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Assert
            var result = _provider.GetEntry("Welcome", "tr-TR");

            Assert.Null(result);
        }

        #endregion

        #region GetEntry()

        [Fact(DisplayName = "BaseLocalizationProvider: GetEntry() returns entry for existed key")]
        public void GetEntry_WithExistingKey_ReturnsTheEntry()
        {
            //Arrange
            this.InitializeProvider();

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            var result = _provider.GetEntry("Welcome", "tr-TR");

            //Assert
            Assert.NotNull(result);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: GetEntry() returns null for not-existed key")]
        public void GetEntry_WithNotExistingKey_ReturnsNull()
        {
            //Arrange
            this.InitializeProvider();

            //Act
            var result = _provider.GetEntry(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Assert
            Assert.Null(result);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: GetEntry() retreives entry from cache first [CacheEnabled]")]
        public void GetEntry_CacheEnabled_RetreivesEntryFromCacheFirst()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = true;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Act
            var result = _provider.GetEntry("Welcome", "tr-TR");

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Merhaba", result.Value);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: GetEntry() retreives entry from store if [CacheDisabled]")]
        public void GetEntry_CachDisabled_RetreivesEntryFromStore()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = false;

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            _provider.LocalizerEntries = new List<LocalizerEntry>();

            //Act
            var result = _provider.GetEntry("Welcome", "tr-TR");

            //Assert
            Assert.Null(result);
        }

        #endregion

        #region Exists()

        [Fact(DisplayName = "BaseLocalizationProvider: Exists() returns TUE for existing key for existed key")]
        public void Exists_WithExistingKey_ReturnsTrue()
        {
            //Arrange
            this.InitializeProvider();

            _provider.AddEntry("Welcome", "Merhaba", "tr-TR");

            //Act
            var result = _provider.Exists("Welcome", "tr-TR");

            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "BaseLocalizationProvider: Exists() returns FALSE for existing key for not-existed key")]
        public void Exists_WithNotExistingKey_ReturnsFalse()
        {
            //Arrange
            this.InitializeProvider();

            //Act
            var result = _provider.Exists(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            //Assert
            Assert.False(result);
        }

        #endregion

        #region ClearAll()

        [Fact(DisplayName = "BaseLocalizationProvider: ClearAll() clear all localizer caches")]
        public void ClearAll_CacheEnabled_ClearAllRelateCaches()
        {
            //Arrange
            this.InitializeProvider();

            this._provider.CacheEnabled = true;

            _provider.AddEntry("Welcome", "Welcome (TR)", "tr-TR");
            _provider.AddEntry("Welcome", "Welcome (AR)", "ar-AR");
            _provider.AddEntry("Welcome", "Welcome (En)", "en-US");
            _provider.AddEntry("Welcome", "Welcome (Invariant)", "");

            //Act
            _provider.ClearAll();

            //Assert
            var welcomeTr = _provider.GetEntry("Welcome", "tr-TR");
            var welcomeAr = _provider.GetEntry("Welcome", "ar-AR");
            var welcomeEn = _provider.GetEntry("Welcome", "en-US");
            var welcomeInvariant = _provider.GetEntry("Welcome", "");

            Assert.Null(welcomeTr);
            Assert.Null(welcomeAr);
            Assert.Null(welcomeEn);
            Assert.Null(welcomeInvariant);
        }
        #endregion
    }
}
