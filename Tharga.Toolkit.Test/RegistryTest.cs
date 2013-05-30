//using System;
//using System.Collections.Generic;
//using System.Reflection;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Tharga.Toolkit.Test.Thread;

//namespace Tharga.Toolkit.Test
//{
//    public class RegistryBuilder : TestDataBuilder<Registry>
//    {
//        private readonly RegistryHKey _registryHKey;
//        private readonly Assembly _assembly;

//        public RegistryBuilder(RegistryHKey registryHKey, Assembly assembly)
//        {
//            _registryHKey = registryHKey;
//            _assembly = assembly;
//        }

//        protected override Registry OnBuild()
//        {
//            return new Registry(new RegistryRepositoryMock(), _registryHKey, _assembly);
//        }
//    }

//    public class RegistryRepositoryMock : IRegistryRepository
//    {
//        private readonly Dictionary<string, object> _dataStore = new Dictionary<string, object>();

//        public object GetSetting(RegistryHKey registryHKey, string path, string keyName, object defaultValue)
//        {
//            var key = CreateKey(registryHKey, path, keyName);

//            if (!_dataStore.ContainsKey(key))
//            {
//                SetSetting(registryHKey, path, keyName, defaultValue);
//                return defaultValue;
//            }
//            return _dataStore[key];
//        }

//        public void SetSetting(RegistryHKey registryHKey, string path, string keyName, object value)
//        {
//            var key = CreateKey(registryHKey, path, keyName);
//            if (!_dataStore.ContainsKey(key))
//                _dataStore.Add(key, value);
//            else
//                _dataStore[key] = value;
//        }

//        public void RemoveSetting(RegistryHKey registryHKey, string path, string keyName)
//        {
//            var key = CreateKey(registryHKey, path, keyName);
//            if (_dataStore.ContainsKey(key))
//                _dataStore.Remove(key);

//            if (_dataStore.ContainsKey(key))
//                throw new InvalidOperationException("The key is still there after remove.");
//        }

//        public bool HasSetting(RegistryHKey registryHKey, string path, string keyName)
//        {
//            var key = CreateKey(registryHKey, path, keyName);
//            return _dataStore.ContainsKey(key);
//        }

//        public void SetAutoStart(RegistryHKey registryHKey, string keyName, string location)
//        {
//            SetSetting(registryHKey, Registry.AutoStartLocation, keyName, location);
//        }

//        public void RemoveAutoStart(RegistryHKey registryHKey, string keyName)
//        {
//            RemoveSetting(registryHKey, Registry.AutoStartLocation, keyName);
//        }

//        public bool IsAutoStartEnabled(RegistryHKey registryHKey, string keyName, string location)
//        {
//            var result = GetSetting(registryHKey, Registry.AutoStartLocation, keyName, null) as string;
//            return string.Compare(result, location, StringComparison.InvariantCultureIgnoreCase) == 0;
//        }

//        #region Private support methods


//        private static string CreateKey(RegistryHKey registryHKey, string path, string keyName)
//        {
//            return string.Format("{0}\\{1}\\{2}", registryHKey, path, keyName);
//        }


//        #endregion
//    }


//    [TestClass]
//    public class RegistryTest
//    {
//        [TestMethod]
//        public void SetAutoStart()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            Assert.IsFalse(rgs.IsAutoStartEnabled(), "Auto Start is enabled before start");
//            rgs.SetAutoStart();
//            Assert.IsTrue(rgs.IsAutoStartEnabled(), "Auto Start is not enabled after set");
//            rgs.RemoveAutoStart();

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsFalse(rgs.IsAutoStartEnabled(), "Auto Start is enabled after remove");
//        }

//        [TestMethod]
//        public void GetSetting()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var defaultValue = Guid.NewGuid().ToString();
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            var returnValue = rgs.GetSetting(key, defaultValue) as string;
//            var otherReturnValue = rgs.GetSetting(key, "DUMMY") as string;

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(returnValue == defaultValue, "Return value was not same as default value provided");
//            Assert.IsTrue(returnValue == otherReturnValue, "Second return value was not same as the first one");
//        }

//        [TestMethod]
//        public void SetSetting()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var defaultValue = Guid.NewGuid().ToString();
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            rgs.SetSetting(key, defaultValue);
//            var returnValue = rgs.GetSetting(key, "DUMMY") as string;

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(returnValue == defaultValue, "Return value was not same as default value provided");
//        }

//        [TestMethod]
//        public void GetSetOverwrite()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var firstValue = Guid.NewGuid().ToString();
//            var secondValue = Guid.NewGuid().ToString();
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            var firstReturnValue = rgs.GetSetting(key, firstValue) as string;
//            rgs.SetSetting(key, secondValue);
//            var secondReturnValue = rgs.GetSetting(key, "DUMMY") as string;

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(firstReturnValue == firstValue, "The first default and return values are not the same");
//            Assert.IsTrue(secondReturnValue == secondValue, "The second set and return values are not the same");
//        }

//        [TestMethod]
//        public void RemoveSetting()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var firstValue = Guid.NewGuid().ToString();
//            var secondValue = Guid.NewGuid().ToString();
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            rgs.SetSetting(key, firstValue);
//            rgs.RemoveSetting(key);
//            var secondReturnValue = rgs.GetSetting(key, secondValue) as string;

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(secondReturnValue == secondValue, "The second set and return values are not the same");
//        }

//        [TestMethod]
//        public void HasSetting()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var firstValue = Guid.NewGuid().ToString();
//            var rgs = new RegistryBuilder(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly()).Build();

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            Assert.IsFalse(rgs.HasSetting(key), "Has setting before it has been set");
//            rgs.SetSetting(key, firstValue);
//            Assert.IsTrue(rgs.HasSetting(key), "Does not have setting after it has been set");
//            rgs.RemoveSetting(key);

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsFalse(rgs.HasSetting(key), "Has setting after it has been removed");
//        }

//        [TestMethod]
//        public void SettingIntegration()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var key = Guid.NewGuid().ToString();
//            var firstValue = Guid.NewGuid().ToString();
//            var secondValue= Guid.NewGuid().ToString();
//            var rgs = new Registry(RegistryHKey.CurrentUser, Assembly.GetExecutingAssembly());

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            Assert.IsFalse(rgs.HasSetting(key), "Has setting before it has been set");
//            var firstReturnValue = rgs.GetSetting(key, firstValue) as string;
//            Assert.IsTrue(rgs.HasSetting(key), "Does not have setting after get with default value");
//            rgs.SetSetting(key, secondValue);
//            Assert.IsTrue(rgs.HasSetting(key), "Does not have setting after set with default value");
//            var secondReturnValue = rgs.GetSetting(key, "DUMMY") as string;
//            rgs.RemoveSetting(key);
//            Assert.IsFalse(rgs.HasSetting(key), "Still have setting after remove");

//            //------------------------------------------
//            // Assert
//            //------------------------------------------                    
//            Assert.IsTrue(firstReturnValue == firstValue, "The second set and return values are not the same");
//            Assert.IsTrue(secondReturnValue == secondValue, "The second set and return values are not the same");
//        }

//        //[TestMethod]
//        //public void AutoStartIntegration()
//        //{
//        //    //------------------------------------------
//        //    // Arrange
//        //    //------------------------------------------
//        //    var rgs = new Registry(RegistryHKey.CurrentUser,Assembly.GetCallingAssembly());

//        //    //------------------------------------------
//        //    // Act
//        //    //------------------------------------------
//        //    Assert.IsFalse(rgs.IsAutoStartEnabled(), "Auto Start is enabled before start");
//        //    rgs.SetAutoStart();
//        //    Assert.IsTrue(rgs.IsAutoStartEnabled(), "Auto Start is not enabled after set");
//        //    rgs.RemoveAutoStart();

//        //    //------------------------------------------
//        //    // Assert
//        //    //------------------------------------------
//        //    Assert.IsFalse(rgs.IsAutoStartEnabled(), "Auto Start is enabled after remove");
//        //}
    
//    }
//}
