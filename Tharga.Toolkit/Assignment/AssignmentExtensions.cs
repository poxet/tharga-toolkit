namespace Tharga.Toolkit.Assignment
{
    public static class AssignmentExtensions
    {
        public static bool IsAssigned<T>(this T item)
        {
            var defaultTypeValue = default(T);

            if (defaultTypeValue == null && item == null)
                return false;

            if (item.Equals(defaultTypeValue))
                return false;

            //Go deeper and check properties
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(item);
                if (!IsAssigned(propValue))
                    return false;
            }

            return true;
        }
    }
}