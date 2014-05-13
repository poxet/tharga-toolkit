//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Reflection;

//namespace Tharga.Toolkit
//{
//    public class ObjectMap
//    {
//        //Singleton
//        private static readonly List<ObjectMap> ObjectMapList = new List<ObjectMap>();

//        private Type To { get; set; }
//        private Type From { get; set; }
//        private readonly List<PropertyMap> _properties = new List<PropertyMap>();
//        private bool _verified;

//        private ObjectMap(Type to, Type from)
//        {
//            To = to;
//            From = from;
//        }

//        /// <summary>
//        /// Adds the static property map.
//        /// </summary>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="staticValue">The static value.</param>
//        public void AddStaticPropertyMap<TFrom>(string toPropertyName, TFrom staticValue)
//        {
//            var to = To.GetProperty(toPropertyName);

//            if (to == null) throw new InvalidOperationException(string.Format("The to-object {0} does not contain the property {1}.", To.Name, toPropertyName));

//            AddPropertyMap(new StaticPropertyMap<TFrom>(to, staticValue));
//        }

//        /// <summary>
//        /// Adds the static property map.
//        /// </summary>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="toPropertyName">Name of to property.</param>
//        public void AddStaticPropertyMap<TFrom>(string toPropertyName)
//        {
//            var to = To.GetProperty(toPropertyName);

//            if (to == null) throw new InvalidOperationException(string.Format("The to-object {0} does not contain the property {1}.", To.Name, toPropertyName));

//            AddPropertyMap(new StaticPropertyMap<TFrom>(to, default(TFrom)));
//        }

//        /// <summary>
//        /// Adds the object property map.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="loader">The loader.</param>
//        public void AddObjectPropertyMap<TTo>(string toPropertyName, Func<object, TTo> loader)
//        {
//            var to = To.GetProperty(toPropertyName);

//            if (to == null) throw new InvalidOperationException(string.Format("The to-object {0} does not contain the property {1}.", To.Name, toPropertyName));

//            AddPropertyMap(new ObjectPropertyMap<TTo>(to, loader));
//        }

//        /// <summary>
//        /// Sets the static property map value.
//        /// </summary>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="staticValue">The static value.</param>
//        public void SetStaticPropertyMapValue<TFrom>(string toPropertyName, TFrom staticValue)
//        {
//            var to = _properties.Find(itm => itm.To.Name == toPropertyName);
//            if (to is StaticPropertyMap<TFrom>)
//            {
//                var tos = (StaticPropertyMap<TFrom>)to;
//                tos.StaticValue = staticValue;
//            }
//            else
//                throw new InvalidOperationException(string.Format("Map for property {0} is not a StaticPropertyMap<{1}>.", toPropertyName, typeof(TFrom)));
//        }

//        /// <summary>
//        /// Adds the property function map.
//        /// </summary>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="fromFunctionName">Name of from function.</param>
//        public void AddPropertyFunctionMap(string toPropertyName, string fromFunctionName)
//        {
//            var to = To.GetProperty(toPropertyName);
//            var from = From.GetMethod(fromFunctionName);

//            AddPropertyMap(new FunctionPropertyMap(to, from));
//        }

//        /// <summary>
//        /// Adds the property map.
//        /// </summary>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="fromPropertyName">Name of from property.</param>
//        public void AddPropertyMap(string toPropertyName, string fromPropertyName)
//        {
//            AddPropertyMap(toPropertyName, fromPropertyName, null);
//        }

//        /// <summary>
//        /// Adds the property map.
//        /// </summary>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="fromPropertyName">Name of from property.</param>
//        /// <param name="converter">The converter.</param>
//        public void AddPropertyMap(string toPropertyName, string fromPropertyName, Func<object, object> converter)
//        {
//            var to = To.GetProperty(toPropertyName);
//            var from = From.GetProperty(fromPropertyName);

//            if (to == null) throw new InvalidOperationException(string.Format("The to-object {0} does not contain the property {1}.", To.Name, toPropertyName));
//            if (from == null) throw new InvalidOperationException(string.Format("The from-object {0} does not contain the property {1}.", From.Name, fromPropertyName));

//            AddPropertyMap(new PropertyMap(to, from, converter));
//        }

//        private void AddPropertyMap(PropertyInfo to, PropertyInfo from)
//        {
//            if (to == null) throw new ArgumentNullException("to", "PropertyInfo to is null");
//            if (from == null) throw new ArgumentNullException("from", "PropertyInfo from is null");

//            AddPropertyMap(new PropertyMap(to, from));
//        }

//        private void AddPropertyMap(PropertyMap propertyMap)
//        {
//            if (_verified) throw new InvalidOperationException("Cannot add property map on ObjectMap that have been verified.");

//            //Verify that the property is not already added
//            if (_properties.FindAll(itm => string.Compare(itm.To.Name, propertyMap.To.Name) == 0).Count > 0)
//                throw new InvalidOperationException(string.Format("There is already a definition for property {0} in the to-object.", propertyMap.To.Name));

//            //Verify that the to-object has the property
//            var prop = To.GetProperty(propertyMap.To.Name);
//            if (prop == null) throw new InvalidOperationException(string.Format("There is no property named {0} in the to-object.", propertyMap.To.Name));

//            _properties.Add(propertyMap);
//        }

//        /// <summary>
//        /// Removes the property map.
//        /// </summary>
//        /// <param name="toPropertyName">Name of to property.</param>
//        public void RemovePropertyMap(string toPropertyName)
//        {
//            if (_verified) throw new InvalidOperationException("Cannot add property map on ObjectMap that have been verified.");

//            var property = _properties.Find(itm => string.Compare(itm.To.Name, toPropertyName) == 0);
//            if (property == null) throw new InvalidOperationException(string.Format("There is no To property named {0}.", toPropertyName));

//            _properties.Remove(property);
//        }

//        /// <summary>
//        /// Sets the converter.
//        /// </summary>
//        /// <param name="toPropertyName">Name of to property.</param>
//        /// <param name="converter">The converter.</param>
//        public void SetConverter(string toPropertyName, Func<object, object> converter)
//        {
//            //No need to check if the map is verified or not, this does not effect the to-property structure.

//            var property = _properties.Find(itm => string.Compare(itm.To.Name, toPropertyName) == 0);
//            if (property == null) throw new InvalidOperationException(string.Format("There is no To property named {0}.", toPropertyName));

//            property.Converter = converter;
//        }

//        /// <summary>
//        /// Sets the converter.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="toPropertyName">Name of to property.</param>
//        public void SetConverter<TTo, TFrom>(string toPropertyName) where TTo : new()
//        {
//            //No need to check if the map is verified or not, this does not effect the to-property structure.

//            var toProperty = _properties.Find(itm => string.Compare(itm.To.Name, toPropertyName) == 0);
//            if (toProperty == null) throw new InvalidOperationException(string.Format("There is no To property named {0}.", toPropertyName));
//            if (toProperty.To.Name != typeof(TTo).Name) throw new InvalidOperationException(string.Format("The property named {0} is of type {1} and differs from specified type {2}.", toPropertyName, toProperty.To.Name, typeof(TTo).Name));

//            toProperty.Converter = BasicConvert<TTo, TFrom>;
//        }

//        private static object BasicConvert<TTo, TFrom>(object data) where TTo : new()
//        {
//            return ((TFrom)data).MapObject<TTo, TFrom>();
//        }

//        private void MapEx<TTo, TFrom>(TTo to, TFrom from)
//        {
//            if (!_verified) throw new InvalidOperationException(string.Format("The ObjectMap {0}->{1} has not been verified. If the map is custom created you have to call the verify function before it can be used.", typeof(TFrom), typeof(TTo)));

//            foreach (var property in _properties)
//                property.MapProperty(to, from);
//        }

//        internal static void Map<TTo, TFrom>(TTo to, TFrom from)
//        {
//            var mo = FindOrCreate<TTo, TFrom>();
//            mo.MapEx(to, from);
//        }

//        internal static TTo Map<TTo, TFrom>(TFrom from) where TTo : new()
//        {
//            var to = new TTo();
//            Map(to, from);
//            return to;
//        }

//        private static ObjectMap FindOrCreate<TTo, TFrom>()
//        {
//            var mo = Find<TTo, TFrom>();
//            if (mo == null)
//            {
//                lock (ObjectMapList) //Check-lock-check pattern
//                {
//                    mo = Find<TTo, TFrom>() ?? Create<TTo, TFrom>(true);
//                }
//            }
//            return mo;
//        }

//        /// <summary>
//        /// Finds this instance.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <returns></returns>
//        public static ObjectMap Find<TTo, TFrom>()
//        {
//            return ObjectMapList.Find(itm => itm.To == typeof(TTo) && itm.From == typeof(TFrom));
//        }

//        /// <summary>
//        /// Creates the specified auto verify.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="autoVerify">if set to <c>true</c> [auto verify].</param>
//        /// <returns></returns>
//        public static ObjectMap Create<TTo, TFrom>(bool autoVerify = false)
//        {
//            if (Find<TTo, TFrom>() != null) throw new InvalidOperationException(string.Format("There is already a map between {0} and {1}.", typeof(TTo), typeof(TFrom)));

//            var mo = new ObjectMap(typeof(TTo), typeof(TFrom));

//            var properties = typeof(TTo).GetProperties();
//            foreach (var toProperty in properties)
//            {
//                var fromProperty = typeof(TFrom).GetProperty(toProperty.Name);
//                if (fromProperty != null)
//                    mo.AddPropertyMap(toProperty, fromProperty);
//                else if (autoVerify)
//                    throw new InvalidOperationException(string.Format("The property {0} does not exist in the source class {1}. Cannot convert from {1} to {2}.", toProperty.Name, typeof(TFrom), typeof(TTo)));
//            }

//            if (autoVerify)
//            {
//                if (properties.Length == 0) throw new InvalidOperationException(string.Format("There is nothing to map since class {0} does not have any properties.", typeof(TTo)));

//                mo._verified = true;
//                ObjectMapList.Add(mo);
//            }

//            return mo;
//        }

//        private class ObjectPropertyMap<TTo> : PropertyMap
//        {
//            private readonly Func<object, TTo> _loader;

//            internal ObjectPropertyMap(PropertyInfo to, Func<object, TTo> loader)
//                : base(to, null)
//            {
//                _loader = loader;
//            }

//            internal override object GetValue<TFrom>(TFrom from)
//            {
//                return _loader.Invoke(from);
//            }
//        }

//        private class StaticPropertyMap<T> : PropertyMap
//        {
//            internal T StaticValue;

//            internal StaticPropertyMap(PropertyInfo to, T staticValue)
//                : base(to, null)
//            {
//                StaticValue = staticValue;
//            }

//            internal override object GetValue<TFrom>(TFrom from)
//            {
//                return StaticValue;
//            }
//        }

//        private class FunctionPropertyMap : PropertyMap
//        {
//            private readonly MethodInfo _from;

//            internal FunctionPropertyMap(PropertyInfo to, MethodInfo from)
//                : base(to, null)
//            {
//                _from = from;
//            }

//            internal override object GetValue<TFrom>(TFrom from)
//            {
//                return _from.Invoke(from, null);
//            }
//        }

//        private class PropertyMap
//        {
//            internal PropertyInfo To { get; private set; }
//            private PropertyInfo From { get; set; }

//            internal Func<object, object> Converter;

//            internal PropertyMap(PropertyInfo to, PropertyInfo from, Func<object, object> converter = null)
//            {
//                if (to == null) throw new ArgumentNullException("to", "PropertyInfo to is null");

//                To = to;
//                From = from;
//                Converter = converter;
//            }

//            internal virtual void MapProperty<TTo, TFrom>(TTo to, TFrom from)
//            {
//                //Pick out data from property
//                var value = GetValue(from);

//                //If data needs to be converted, do that here
//                if (Converter != null)
//                    value = Converter.Invoke(value);

//                try
//                {
//                    //Assign data to property
//                    To.SetValue(to, value, null);
//                }
//                catch (ArgumentException exp)
//                {
//                    var message = string.Format("{0} This might be a hierary object. Add a Converter function to this property map. Trying to convert to {1} from {2}.", exp.Message, To, From);
//                    throw new ArgumentException(message, exp);
//                }
//            }

//            internal virtual object GetValue<TFrom>(TFrom from)
//            {
//                return From.GetValue(from, null);
//            }
//        }

//        /// <summary>
//        /// Verifies this instance.
//        /// </summary>
//        public void Verify()
//        {
//            if (_verified) throw new InvalidOperationException(string.Format("Object Map to {0} from {1} has already been verified.", To, From));


//            //Check so that there is an exact 1 ot 1 relationship between the to object and the defined map.
//            var properties = To.GetProperties();
//            foreach (var toProperty in properties)
//            {
//                var property = toProperty;
//                if (_properties.Find(itm => itm.To.Name == property.Name) == null)
//                    throw new InvalidOperationException(string.Format("There is no property definition for property {0} in the to-object {1}.", toProperty.Name, To.Name));
//            }
//            _verified = true;
//            ObjectMapList.Add(this);
//        }
//    }


//    public static partial class Extensions
//    {
//        /// <summary>
//        /// Maps the object.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="from">From.</param>
//        /// <returns></returns>
//        public static TTo MapObject<TTo, TFrom>(this TFrom from) where TTo : new()
//        {
//            var to = new TTo();

//            if (from is Enum) throw new InvalidOperationException(string.Format("Cannot map {0} (enum type) using the MapObject function, use MapEnum instead.", from.GetType()));
//            if (to is Enum) throw new InvalidOperationException(string.Format("Cannot map {0} (enum type) using the MapObject function, use MapEnum instead.", to.GetType()));

//            ObjectMap.Map(to, from);
//            return to;
//        }

//        /// <summary>
//        /// Maps a list of objects.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="list">The list.</param>
//        /// <returns></returns>
//        public static IList<TTo> MapObject<TTo, TFrom>(this IList<TFrom> list) where TTo : new()
//        {
//            var response = new List<TTo>();
//            foreach (var item in list)
//                response.Add(item.MapObject<TTo, TFrom>());
//            return response;
//        }

//        /// <summary>
//        /// Maps a collection of objects.
//        /// </summary>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="collection">The list.</param>
//        /// <returns></returns>
//        public static ICollection<TTo> MapObject<TTo, TFrom>(this ICollection<TFrom> collection) where TTo : new()
//        {
//            var response = new Collection<TTo>();
//            foreach (var item in collection)
//                response.Add(item.MapObject<TTo, TFrom>());
//            return response;
//        }

//        /// <summary>
//        /// Maps a dictionary.
//        /// </summary>
//        /// <typeparam name="TKey">The type of the key.</typeparam>
//        /// <typeparam name="TTo">The type of to.</typeparam>
//        /// <typeparam name="TFrom">The type of from.</typeparam>
//        /// <param name="dictionary">The dictionary.</param>
//        /// <returns></returns>
//        public static IDictionary<TKey, TTo> MapObject<TKey, TTo, TFrom>(this IDictionary<TKey, TFrom> dictionary) where TTo : new()
//        {
//            var response = new Dictionary<TKey, TTo>();
//            foreach (var item in dictionary)
//                response.Add(item.Key, item.Value.MapObject<TTo, TFrom>());
//            return response;
//        }
//    }
//}
