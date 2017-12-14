using System;
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
            this._resourceKey = resourceKey ?? Constants.DefaultRequiredAttributeResourceKey;

            this.LocalizeErrorMessage();
        }

        public LocalizedRequiredAttribute(string resourceKey = null) : 
            this(new LocalizerService(Configuration.LocalizationProvider), resourceKey)
        {
            
        }
        
        #endregion

        #region Private Methods

        private void LocalizeErrorMessage()
        {
            var result = _localizerService.GetLocalizedValue(this._resourceKey);

            this.ErrorMessage = string.IsNullOrEmpty(result) ? Constants.DefaultRequiredAttributeResourceValue : result;
        }

        #endregion

        public override string FormatErrorMessage(string name)
        {
            this.LocalizeErrorMessage();

            return base.FormatErrorMessage(name);
        }
    }
}
