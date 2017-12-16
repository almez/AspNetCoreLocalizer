using System;
using AspNetCoreLocalizer.Abstraction;
using AspNetCoreLocalizer.Adapters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreLocalizer
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddLocalizer(this IServiceCollection services)
        {
            services.AddSingleton<IValidationAttributeAdapterProvider, LocalizedRequiredAdapterProvider>();

            return services;
        }

        public static IApplicationBuilder UseLocalizer(this IApplicationBuilder app, Func<ILocalizationProvider> config)
        {
            return app;
        }
    }
}
