using System;

namespace AspNetCoreLocalizer.Exceptions
{
    public class EntryNotFoundException : Exception
    {
        public EntryNotFoundException(string msg) : base(msg)
        {
            
        }
    }
}
