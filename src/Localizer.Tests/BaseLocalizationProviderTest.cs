using System;
using System.Linq;
using Localizer.Domain;
using Localizer.Exceptions;
using Localizer.Factories;
using Localizer.Providers;
using Xunit;

namespace Localizer.Tests
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
    }
}
