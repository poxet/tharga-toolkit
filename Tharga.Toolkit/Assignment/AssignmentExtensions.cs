using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tharga.Toolkit
{
    public static class AssignmentExtensions
    {
        private const int MaxRecursiveLevel = 20;

        public static AssignmentInfo IsAssigned<T>(this T item, IEnumerable<string> exclude = null)
        {
            return CheckAssignment(null, item, exclude?.ToArray() ?? new string[] { }, new List<object>(), 0);
        }

        private static AssignmentInfo CheckAssignment<T>(string parentObject1Name, T item, string[] exclude, List<object> visited, int level)
        {
            if (level >= MaxRecursiveLevel)
                return new AssignmentInfo(true, $"Max recursive level {MaxRecursiveLevel} reached.");
            level++;

            if (visited.Any(x => ReferenceEquals(x, item)))
                return new AssignmentInfo(true);

            visited.Add(item);

            var defaultTypeValue = default(T);

            var name = item?.GetType().Name ?? "?";
            var item1Name = parentObject1Name != null ? $"{parentObject1Name}.{name}" : name;

            if (exclude.Any(x => x == item1Name))
                return new AssignmentInfo(true, $"Excluded '{item1Name}'.");

            if (defaultTypeValue == null && item == null)
                return new AssignmentInfo(false, $"No assignment for '{parentObject1Name}'.");

            if (item.Equals(defaultTypeValue))
                return new AssignmentInfo(false, $"No assignment for '{parentObject1Name}'.");

            //loop over iterator
            if (item is IEnumerable)
            {
                var enumr1 = (item as IEnumerable).GetEnumerator();
                while (enumr1.MoveNext())
                {
                    var assignmentInfo = CheckAssignment(item1Name, enumr1.Current, exclude, visited, level);
                    if (assignmentInfo == false)
                        return assignmentInfo;
                }
            }
            else
            {
                //Do not go deeper into theese types
                if (item is DateTime)
                    return new AssignmentInfo(true);

                //Go deeper and check properties
                var props = item.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(item);
                    var assignmentInfo = CheckAssignment(item1Name, propValue, exclude, visited, level);
                    if (assignmentInfo == false)
                        return assignmentInfo;
                }
            }

            return new AssignmentInfo(true);
        }
    }
}