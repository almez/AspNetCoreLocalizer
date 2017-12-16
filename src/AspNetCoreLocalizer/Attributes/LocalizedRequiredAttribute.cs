using System.ComponentModel.DataAnnotations;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Services;

namespace AspNetCoreLocalizer.Attributes
{
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
            this(Localizer.Instance.LocalizerService, resourceKey)
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
