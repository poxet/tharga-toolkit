using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Tharga.Toolkit
{
    public static class AssignmentExtensions
    {
        private const int MaxObjectGraphDepth = 20;
        private const int MaxCheckTime = 2000;

        public static AssignmentInfo IsAssigned<T>(this T item, IEnumerable<string> exclude = null)
        {
            var r = Task.Run(() =>
            {
                var result = CheckAssignment(null, item, exclude?.ToArray() ?? new string[] { }, new List<object>(), 0, GetName<T>());
                result = new AssignmentInfo(result.IsAssigned, null,
                    string.IsNullOrEmpty(result.Message) ? null : $"{result.Message} for '{result.PropertyTree}'.");
                return result;
            });

            if (Debugger.IsAttached)
            {
                r.Wait();
                return r.Result;
            }
            else
            {
                return !r.Wait(MaxCheckTime)
                    ? new AssignmentInfo(false, null, $"The allotted time {MaxCheckTime}ms has expired.")
                    : r.Result;
            }
        }

        private static AssignmentInfo CheckAssignment<T>(string parentObject1Name, T item, string[] exclude, List<object> visited, int level, string name)
        {
            if (level >= MaxObjectGraphDepth)
                return new AssignmentInfo(false, name, $"Max object graph depth {MaxObjectGraphDepth} reached");
            level++;

            if (visited.Any(x => ReferenceEquals(x, item)))
                return new AssignmentInfo(true, name, null);

            visited.Add(item);

            var defaultTypeValue = GetDefault(item?.GetType() ?? typeof(T));

            var excludeName = parentObject1Name != null ? $"{parentObject1Name}.{name}" : name;
            if (exclude.Any(x => x == excludeName))
                return new AssignmentInfo(true, name, "Excluded");

            if (defaultTypeValue == null && item == null)
                return new AssignmentInfo(false, name, "No assignment");

            if (item == null)
                throw new NullReferenceException($"Null value for {name}.");

            if (item.Equals(defaultTypeValue))
                return new AssignmentInfo(false, name, "No assignment");

            //loop over iterator
            if (item is IEnumerable)
            {
                var index = 0;
                var enumr1 = ((IEnumerable)item).GetEnumerator();
                while (enumr1.MoveNext())
                {
                    var n = name.Contains("[]") ? name.Replace("[]", $"[{index}]") : name + $"[{index}]";
                    var assignmentInfo = CheckAssignment(parentObject1Name, enumr1.Current, exclude, visited, level, n);
                    if (assignmentInfo == false)
                    {
                        return assignmentInfo;
                    }
                    index++;
                }
            }
            else
            {
                //Do not go deeper into theese types
                if (IsSimple(item.GetType()))
                    return new AssignmentInfo(true, name, null);

                //Go deeper and check properties
                var props = item.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var propValue = prop.GetValue(item);
                    var assignmentInfo = CheckAssignment(excludeName, propValue, exclude, visited, level, prop.Name);
                    if (assignmentInfo == false)
                    {
                        assignmentInfo.PrependPropertyName(name);
                        return assignmentInfo;
                    }
                }

                var mems = item.GetType().GetFields();
                foreach (var mem in mems)
                {
                    var val = mem.GetValue(item);
                    var assignmentInfo = CheckAssignment(excludeName, val, exclude, visited, level, mem.Name);
                    if (assignmentInfo == false)
                    {
                        assignmentInfo.PrependPropertyName(name);
                        return assignmentInfo;
                    }
                }
            }

            return new AssignmentInfo(true, name, null);
        }

        private static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
                   || type.IsEnum
                   || type == typeof(Guid)
                   || type == typeof(DateTime)
                   || type == typeof(TimeSpan)
                   || type == typeof(string)
                   || type == typeof(decimal);
        }

        private static string GetName<T>()
        {
            var n = typeof(T).Name;
            if (n == "Nullable`1")
            {
                var fullName = typeof(T).FullName;
                if (fullName != null) n = fullName.Split('[')[2];
                n = n.Split(',')[0];
                n = n.Split('.').Last();
                n += "?";
            }
            return n;
        }

        public static object GetDefault(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }
    }
}