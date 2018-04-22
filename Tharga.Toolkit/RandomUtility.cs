using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Tharga.Toolkit
{
    public static class RandomUtility
    {
        private static readonly Random Rng = new Random();

        public static string GetRandomString(int minLength = 6, int maxLength = 20, string chars = null)
        {
            var size = GetRandomInt(minLength, maxLength);

            if (string.IsNullOrEmpty(chars))
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var buffer = new char[size];

            for (var i = 0; i < size; i++)
                buffer[i] = chars[Rng.Next(chars.Length)];
            return new string(buffer);
        }

        public static int GetRandomInt(int min = 0, int max = 10000000)
        {
            return Rng.Next(max - min) + min;
        }

        public static List<T> GetRandomList<T>(Func<T> func, int? minCount = null, int? maxCount = null)
        {
            var list = new List<T>();

            if (minCount == null && maxCount == null)
            {
                minCount = 2;
                maxCount = 99;
            }

            if (minCount == null)
                minCount = 2;

            if (maxCount == null)
                maxCount = minCount;

            var count = GetRandomInt(minCount.Value, maxCount.Value);

            for (var i = 0; i < count; i++)
                list.Add(func.Invoke());

            return list;
        }

        public static string GetRandomPhoneNumber()
        {
            return string.Format("0{0}-{1}", GetRandomInt(1, 99), GetRandomInt(100000, 100000000));
        }

        public static string GetRandomEmail()
        {
            const string chars = "abcdefghijklmnopqrstuvxyz";
            return string.Format("{0}@{1}.{2}", GetRandomString(3, 10, chars), GetRandomString(3, 10, chars), GetRandomString(2, 3, chars));
        }

        public static string GetRandomOrgNo()
        {
            //var orgNo = string.Format("{0}{1}", GetRandomInt(100000, 1000000), GetRandomInt(100, 1000));
            var orgNoPart1 = GetRandomInt(100000, 1000000).ToString(CultureInfo.InvariantCulture);
            var orgNoPart2 = GetRandomInt(100, 1000).ToString(CultureInfo.InvariantCulture);
            var orgNo = Checksum.CreateLuhnString(orgNoPart1 + orgNoPart2, false);
            return string.Format("{0}-{1}", orgNoPart1, orgNoPart2 + orgNo.Substring(9, 1));
        }

        public static bool GetRandomBool()
        {
            return GetRandomInt(0, 1) == 0;
        }

        public static decimal GetRandomDecimal(decimal min = 0, decimal max = 100)
        {
            var value = min + (decimal)Rng.NextDouble() * (max - min);
            return value;
        }

        public static DateTime GetRandomDate()
        {
            return new DateTime(GetRandomInt(1900, 2100), GetRandomInt(1, 12), GetRandomInt(1, 28), GetRandomInt(0, 23), GetRandomInt(0, 59), GetRandomInt(0, 59));
        }

        private static byte[] GetRandomByteArray()
        {
            var byteArray = new byte[GetRandomInt(10, 100)];
            for (var i = 0; i < byteArray.Length; i++)
                byteArray[i] = (byte)GetRandomInt(0, 255);

            return byteArray;
        }

        public static T AssignRandomValues<T>(this T entity)
        {
            var properties = typeof(T).GetProperties().Where(x => x.Name != "ExtensionData").ToList();

            if (properties.Count == 0)
                properties = typeof(T).GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.Name != "ExtensionData").ToList();

            if (properties.Count == 0)
                properties = entity.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.Name != "ExtensionData").ToList();

            foreach (var property in properties)
            {
                if (property.CanWrite)
                {
                    var value = GetRandomValueEx(property.PropertyType);
                    if (value == null)
                        value = GetRandomValueMain(property);
                    property.SetValue(entity, value, null);
                }
            }

            //Assign random values to fields as well
            var fields = typeof (T).GetFields().ToList();

            if (properties.Count == 0)
                fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.Name != "ExtensionData").ToList();

            if (properties.Count == 0)
                fields = entity.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(x => x.Name != "ExtensionData").ToList();

            foreach (var field in fields)
            {
                var value = GetRandomValueEx(field.FieldType);
                if (value == null)
                    value = GetRandomValueMain(field);
                field.SetValue(entity, value);
            }

            return entity;
        }

        private static object GetRandomValueMain(FieldInfo field)
        {
            var value = Activator.CreateInstance(field.FieldType);

            if (value is IEnumerable)
                value = GetRandomList(value);
            else
                value = AssignRandomValues(value);

            return value;
        }

        private static object GetRandomValueMain(PropertyInfo property)
        {
            var value = Activator.CreateInstance(property.PropertyType);

            if (value is IEnumerable)
                value = GetRandomList(value);
            else
                value = AssignRandomValues(value);

            return value;
        }

        private static object GetRandomList(object value)
        {
            var collection = (IList)value;
            var subType = collection.GetType().GetProperty("Item").PropertyType;

            for (var i = 0; i < 2; i++)
            {
                var subItem = GetRandomValueEx(subType);
                if (subItem == null)
                {
                    subItem = Activator.CreateInstance(subType);
                    subItem = AssignRandomValues(subItem);
                }
                collection.Add(subItem);
            }

            return value;
        }

        private static object GetRandomValueEx(Type type)
        {
            var typeName = type.ToString();
            if (type.ToString().StartsWith("System.Nullable"))
            {
                var pos = type.ToString().IndexOf("[", StringComparison.Ordinal);
                var posE = type.ToString().IndexOf("]", pos, StringComparison.Ordinal);
                typeName = type.ToString().Substring(pos + 1, posE - pos - 1);
            }

            switch (typeName)
            {
                case "System.Guid":
                    return Guid.NewGuid();
                case "System.String":
                    return GetRandomString();
                case "System.Decimal":
                    return GetRandomDecimal();
                case "System.Boolean":
                    return GetRandomBool();
                case "System.Int32":
                    return GetRandomInt();
                case "System.DateTime":
                    return GetRandomDate();
                case "System.Byte[]":
                    return GetRandomByteArray();
                default:
                    return null;
                //throw new ArgumentOutOfRangeException(string.Format("Unknown type {0}.", type));
            }
        }

        public static string GetRandomStreetAddress()
        {
            return string.Format("{0}{1}gatan {2}", GetRandomString(1, 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ"), GetRandomString(4, 10, "abcdefghijklmnopqrstuvwxyzåäö"), GetRandomInt(1, 200));
        }

        public static string GetRandomCity()
        {
            return string.Format("{0}{1}köping", GetRandomString(1, 1, "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ"), GetRandomString(6, 10, "abcdefghijklmnopqrstuvwxyzåäö"));
        }
    }
}
