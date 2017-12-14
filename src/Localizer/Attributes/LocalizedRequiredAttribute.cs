using System.ComponentModel.DataAnnotations;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Services;

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

        private readonly ILocalizerService _localizerService;

        #endregion

        #region C'tor

        private LocalizedRequiredAttribute(ILocalizerService localizerService)
        {
            this._localizerService = localizerService;
        }

        internal LocalizedRequiredAttribute(ILocalizerService localizerService, string resourceKey = null) : this(localizerService)
        {
            this._resourceKey = resourceKey ?? Constants.RequiredAttributeResourceKey;

            this.LocalizeErrorMessage();
        }

        public LocalizedRequiredAttribute(string resourceKey = null) : this(new LocalizerService(Configuration.LocalizationProvider))
        {
            this._resourceKey = resourceKey ?? Constants.RequiredAttributeResourceKey;

            this.LocalizeErrorMessage();
        }
        
        #endregion

        #region Private Methods

        private void LocalizeErrorMessage()
        {
            this.ErrorMessage = _localizerService.GetLocalizedValue(this._resourceKey);
        }

        #endregion
    }
}
