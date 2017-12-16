using System.ComponentModel.DataAnnotations;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Services;

namespace AspNetCoreLocalizer.Attributes
{
    public class LocalizedRequiredSelectAttribute : RequiredAttribute
    {
        #region Fields

        private readonly string _resourceKey;

        private readonly ILocalizerService _localizerService;

        #endregion

        #region C'tor

        private LocalizedRequiredSelectAttribute(ILocalizerService localizerService)
        {
            this._localizerService = localizerService;
        }

        internal LocalizedRequiredSelectAttribute(ILocalizerService localizerService, string resourceKey = null) : this(localizerService)
        {
            this._resourceKey = resourceKey ?? Constants.DefaultRequiredSelectAttributeResourceKey;

            this.LocalizeErrorMessage();
        }

        public LocalizedRequiredSelectAttribute(string resourceKey = null) : 
            this(new LocalizerService(Configuration.LocalizationProvider), resourceKey)
        {
            
        }
        
        #endregion

        #region Private Methods

        private void LocalizeErrorMessage()
        {
            var result = _localizerService.GetLocalizedValue(this._resourceKey);

            this.ErrorMessage = string.IsNullOrEmpty(result) ? Constants.DefaultRequiredSelectAttributeResourceValue : result;
        }

        #endregion

        public override string FormatErrorMessage(string name)
        {
            this.LocalizeErrorMessage();

            return base.FormatErrorMessage(name);
        }
    }
}
