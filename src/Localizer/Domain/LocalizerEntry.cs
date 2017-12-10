using System;

namespace Localizer.Domain
{
    public class LocalizerEntry
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public string Culture { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }
}
