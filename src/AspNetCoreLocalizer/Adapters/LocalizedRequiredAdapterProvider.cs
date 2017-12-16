using System;
using System.ComponentModel.DataAnnotations;
using AspNetCoreLocalizer.Attributes;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.DataAnnotations.Internal;
using Microsoft.Extensions.Localization;

namespace AspNetCoreLocalizer.Adapters
{

    public class LocalizedRequiredAdapterProvider : IValidationAttributeAdapterProvider
    {
        private IValidationAttributeAdapterProvider innerProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            var type = attribute.GetType();

            if (type == typeof(LocalizedRequiredAttribute))
                return new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);

            if (type == typeof(LocalizedRequiredSelectAttribute))
                return new RequiredAttributeAdapter((RequiredAttribute)attribute, stringLocalizer);

            return innerProvider.GetAttributeAdapter(attribute, stringLocalizer);
        }
    }
}
