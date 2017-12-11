using System;
using AspNetCoreLocalizer.Domain;

namespace AspNetCoreLocalizer.Factories
{
    public class LocalizerEntryFactory
    {
        public static LocalizerEntry CreateLocalizerEntry(string key, string value, string culture)
        {
            var entry = new LocalizerEntry()
            {
                Key = key,
                Value = value,
                Culture = culture
            };

            entry.Id = Guid.NewGuid().ToString();

            var utcNow = DateTime.UtcNow;

            entry.CreatedUtc = utcNow;
            entry.LastUpdatedUtc = utcNow;

            return entry;
        }

    }
}
