using System;
using Localizer.Abstraction;

namespace Localizer
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
    }
}
