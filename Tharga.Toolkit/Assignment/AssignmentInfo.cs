using System;

namespace Tharga.Toolkit
{
    public class AssignmentInfo : IAssignmentInfo
    {
        public bool IsAssigned { get; }
        public string Message { get; }

        internal AssignmentInfo(bool isAssigned, string message = null)
        {
            IsAssigned = isAssigned;
            Message = message;

            if (!IsAssigned && string.IsNullOrEmpty(Message)) throw new InvalidOperationException("If an assignment is not made, a message needs to be specified.");
        }

        public static implicit operator bool(AssignmentInfo foo)
        {
            return !object.ReferenceEquals(foo, null) && foo.IsAssigned;
        }

        public static bool operator true(AssignmentInfo x)
        {
            return x.IsAssigned;
        }

        public static bool operator false(AssignmentInfo x)
        {
            return !x.IsAssigned;
        }

        public static bool operator ==(AssignmentInfo x, bool y)
        {
            return x.IsAssigned == y;
        }

        public static bool operator !=(AssignmentInfo x, bool y)
        {
            return !(x == y);
        }
    }
}