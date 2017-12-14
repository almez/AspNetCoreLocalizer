using System;
using CachingManager.Abstraction;

namespace AspNetCoreLocalizer.Domain
{
    public class LocalizerEntry : IMesurable
    {
        public string Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        /// <summary>
        /// Comment to translators
        /// </summary>
        public string Comment { get; set; }

        public string Culture { get; set; }

        public DateTime CreatedUtc { get; set; }

        public DateTime LastUpdatedUtc { get; set; }

        public long GetSizeInBytes()
        {
            return sizeof(char) * (Id?.Length ?? 0) +
                   sizeof(char) * (Key?.Length ?? 0) +
                   sizeof(char) * (Value?.Length ?? 0) +
                   sizeof(char) * (Comment?.Length ?? 0) +
                   sizeof(char) * (Culture?.Length ?? 0) +
                   8 * 2;
        }
    }
}
