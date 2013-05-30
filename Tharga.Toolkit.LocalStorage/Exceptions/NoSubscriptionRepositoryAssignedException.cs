using System;

namespace Tharga.Toolkit.LocalStorage.Exceptions
{
    class NoSubscriptionRepositoryAssignedException : SystemException
    {
        public NoSubscriptionRepositoryAssignedException()
            : base("There is no subscription service repository set.")
        {
            
        }
    }
}