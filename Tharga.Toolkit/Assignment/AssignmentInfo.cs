using System.Collections.Generic;

namespace Tharga.Toolkit
{
    public class AssignmentInfo : IAssignmentInfo
    {
        protected bool Equals(AssignmentInfo other)
        {
            return Equals(_propertyList, other._propertyList) && IsAssigned == other.IsAssigned && string.Equals(Message, other.Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((AssignmentInfo) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (_propertyList != null ? _propertyList.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ IsAssigned.GetHashCode();
                hashCode = (hashCode * 397) ^ (Message != null ? Message.GetHashCode() : 0);
                return hashCode;
            }
        }

        private readonly List<string> _propertyList = new List<string>();

        public bool IsAssigned { get; }
        public string Message { get; }
        public string PropertyTree => string.Join(".", _propertyList);

        internal AssignmentInfo(bool isAssigned, string propertyName, string message)
        {
            IsAssigned = isAssigned;
            Message = message;

            if (!string.IsNullOrEmpty(propertyName))
                _propertyList.Add(propertyName);
        }

        public static implicit operator bool(AssignmentInfo foo)
        {
            return !ReferenceEquals(foo, null) && foo.IsAssigned;
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
            return x != null && x.IsAssigned == y;
        }

        public static bool operator !=(AssignmentInfo x, bool y)
        {
            return !(x == y);
        }

        public AssignmentInfo PrependPropertyName(string propertyName)
        {
            _propertyList.Insert(0, propertyName);
            return this;
        }
    }
}