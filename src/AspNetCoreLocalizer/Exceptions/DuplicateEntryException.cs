using System;

namespace AspNetCoreLocalizer.Exceptions
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string msg) : base(msg)
        {
            
        }
    }
}
