using System.ComponentModel.DataAnnotations;

namespace AspNetCoreLocalizer.Attributes
{

    /*
        To use this attribute properly we should set the following at start up
        DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(LocalizedRequired), typeof(RequiredAttributeAdapter));
    */
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        #region Fields

        private readonly string _resourceKey;

        #endregion

        #region C'tor

        public LocalizedRequiredAttribute(string resourceKey = null)
        {
            this._resourceKey = resourceKey ?? Constants.RequiredAttributeResourceKey;

            this.LocalizedErrorMessage();
        }

        #endregion

        #region Private Methods

        private void LocalizedErrorMessage()
        {
            this.ErrorMessage = Localizer.Instance[this._resourceKey];
        }

        #endregion
    }
}
