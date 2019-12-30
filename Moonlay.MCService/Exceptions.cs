using System;

namespace Moonlay.MCService
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException() : base("Record Not Found")
        {
        }

        public RecordNotFoundException(string message) : base(message)
        {
        }
    }
}