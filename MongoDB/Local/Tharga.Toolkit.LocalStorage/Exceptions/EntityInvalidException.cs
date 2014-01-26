using System;

namespace Tharga.Toolkit.LocalStorage.Exceptions
{
    public class EntityInvalidException : InvalidOperationException
    {
        public EntityInvalidException(string message)
            : base(message)
        {

        }
    }
}