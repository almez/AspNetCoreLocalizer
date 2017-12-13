using System.ComponentModel;

namespace AspNetCoreLocalizer.Attributes
{
    public  class LocalizedDisplayNameAttribute : DisplayNameAttribute
    {
        private readonly string _resourceKey;

        #region C'tor

        public LocalizedDisplayNameAttribute(string resourceKey)
        {
            this._resourceKey = resourceKey;
        }

        #endregion

        #region Public Properties

        public override string DisplayName =>  Localizer.Instance[_resourceKey];

        #endregion
    }
}
