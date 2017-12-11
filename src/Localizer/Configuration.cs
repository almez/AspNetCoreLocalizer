using System;
using System.Globalization;
using CachingManager;
using AspNetCoreLocalizer.Abstraction;

namespace AspNetCoreLocalizer
{
    public class Configuration
    {
        #region Provider Configuration

        private static ILocalizationProvider _localizationProvider;

        public static ILocalizationProvider LocalizationProvider
        {
            set => _localizationProvider = value;
            get
            {
                if (_localizationProvider == null)
                {
                    throw new Exception("Localization provider has not been configured yet.")
                    {
                        HelpLink = "https://github.com/almez/Localizer"
                    };
                }
                return _localizationProvider;
            }
        }

        public static bool IsProviderRegistered => LocalizationProvider != null;

        #endregion

        #region Cache Configuration

        public static bool CacheEnabled { get; set; } = true;

        /// <summary>
        /// Cache size-limit per culture
        /// </summary>
        public static long CacheLimitSize { get; set; } = 1 * 1024 * 1024;

        #endregion

        public static CultureInfo DefaultCulture { set; get; } = CultureInfo.InvariantCulture;
    }
}
