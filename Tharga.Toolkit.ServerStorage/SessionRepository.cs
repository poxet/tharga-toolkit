using System;
using System.Collections.Generic;
using System.Linq;
using Tharga.Toolkit.Storage;

namespace Tharga.Toolkit.ServerStorage
{
    //TODO: Make this presistent and pluggable so that users of the assembly can create their own implementation
    static class SessionRepository
    {
        private static readonly List<ISession> Sessions = new List<ISession>();

        public static void Add(ISession session)
        {
            Sessions.Add(session);
        }

        public static ISession Get(Guid sessionToken)
        {
            if ( Sessions.Count == 0) throw new InvalidOperationException("There are no sessions in the session repository.");

            var session = Sessions.SingleOrDefault(x => x.SessionToken == sessionToken);

            if (session == null) throw new InvalidOperationException(string.Format("Session {0} does not exist in the session repository.", sessionToken));

            return session;
        }
    }
}