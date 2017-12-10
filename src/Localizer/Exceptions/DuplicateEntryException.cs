using System;

namespace Localizer.Exceptions
{
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string msg) : base(msg)
        {
            
        }
    }
}
