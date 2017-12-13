using System.ComponentModel;

namespace AspNetCoreLocalizer.Attributes
{
    public  class LocalizedDisplayName : DisplayNameAttribute
    {
        private readonly string _resourceKey;

        #region C'tor

        public LocalizedDisplayName(string resourceKey)
        {
            this._resourceKey = resourceKey;
        }

        #endregion

        #region Public Properties

        public override string DisplayName =>  Localizer.Instance[_resourceKey];

        #endregion
    }
}
