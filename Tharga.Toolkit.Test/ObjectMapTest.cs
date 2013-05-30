using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tharga.Toolkit.Test
{
    #region Data classes
    
    //A new class for each test is used, so that the tests does not interfear with eachoter

    public interface ITestData
    {
        string StringData { get; set; }
        int IntData { get; set; }
        DateTime DateTimeData { get; set; }
        bool? NullableBoolData { get; set; }
    }

    public class TestDataA : ITestData
    {
        public string StringData { get; set;  }
        public int IntData { get; set; }
        public DateTime DateTimeData { get; set; }
        public bool? NullableBoolData { get; set; }
    }

    public class TestDataB : ITestData
    {
        public string StringData { get; set; }
        public int IntData { get; set; }
        public DateTime DateTimeData { get; set; }
        public bool? NullableBoolData { get; set; }
    }

    public class TestDataA1 : TestDataA
    {
    }

    public class TestDataB1 : TestDataB
    {
    }

    public class TestDataA2 : TestDataA
    {
    }

    public class TestDataB2 : TestDataB
    {
    }

    public class TestDataA3 : TestDataA
    {
        
    }

    public class TestDataB3 : TestDataB
    {
        public string DataThatDoesNotExistInA { get; set; }
    }

    public class TestDataA4 : TestDataA
    {
        public string DatatypeMissmatch { get; set; }
    }

    public class TestDataB4 : TestDataB
    {
        public int DatatypeMissmatch { get; set; }
    }

    public class TestDataA5 : TestDataA
    {
    }

    public class TestDataB5 : TestDataB
    {
        public string StaticData { get; set; }
    }

    public class TestDataA6 : TestDataA
    {
        public string GetData() { return StringData; }
    }

    public class TestDataB6 : TestDataB
    {
        public string Data { get; set; }
    }

    public class TestDataA7 : TestDataA
    {
        public DateTime OtherDate { get; set; }
    }

    public class TestDataB7 : TestDataB
    {
        public string OtherDate { get; set; }
    }

    public class TestDataA8 : TestDataA
    {
        public TestDataA8Sub TestDataSub { get; set; }
    }

    public class TestDataB8 : TestDataB
    {
        public TestDataB8Sub TestDataSub { get; set; }
    }

    public class TestDataA8Sub : TestDataA
    {
        
    }

    public class TestDataB8Sub : TestDataB
    {

    }

    public class TestDataA9 : TestDataA
    {

    }

    public class TestDataB9 : TestDataB
    {
    
    }

    public class TestDataA10 : TestDataA
    {

    }

    public class TestDataB10 : TestDataB
    {

    }

    public class TestDataA11 : TestDataA
    {

    }

    public class TestDataB11
    {

    }


    #endregion

    /// <summary>
    /// Summary description for ObjectMapTest
    /// </summary>
    [TestClass]
    public class ObjectMapTest
    {
        [TestMethod]
        public void MapObject()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true};

            //------------------------------------------
            // Act
            //------------------------------------------
            var objectB = objectA.MapObject<TestDataB, TestDataA>();

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsTrue(Compare(objectA, objectB) == 0, "The objects does not contain the same data");
        }

        [TestMethod]
        public void CreateFindAutoVerify()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA1 { StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true };
            var om1 = ObjectMap.Find<TestDataB1, TestDataA1>();
            if (om1 == null)
                om1 = ObjectMap.Create<TestDataB1, TestDataA1>(true);
            var objectB2 = new TestDataB1();

            //------------------------------------------
            // Act
            //------------------------------------------
            var om2 = ObjectMap.Find<TestDataB1, TestDataA1>();
            
            var objectB1 = ObjectMap.Map<TestDataB1, TestDataA1>(objectA);
            ObjectMap.Map(objectB2, objectA);

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsNotNull(om2, "Object map was not found");
            Assert.IsTrue(Compare(objectA, objectB1) == 0, "The objects does not contain the same data");
            Assert.IsTrue(Compare(objectA, objectB2) == 0, "The objects does not contain the same data");
        }

        [TestMethod]
        public void CreateFindManualVerify()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA2 { StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true };
            if (ObjectMap.Find<TestDataB2, TestDataA2>() != null) return;
            var om1 = ObjectMap.Create<TestDataB2, TestDataA2>(false);

            //------------------------------------------
            // Act
            //------------------------------------------
            var om2 = ObjectMap.Find<TestDataB2, TestDataA2>();
            Assert.IsNull(om2, "Object map was found even though the map has not been verified");

            om1.Verify();
            var om3 = ObjectMap.Find<TestDataB2, TestDataA2>();

            var objectB = ObjectMap.Map<TestDataB2, TestDataA2>(objectA);

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsNotNull(om3, "Object map was not found");
            Assert.IsTrue(Compare(objectA, objectB) == 0, "The objects does not contain the same data");
        }

        [TestMethod]
        public void VerifyInvalidFieldMissing()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var om1 = ObjectMap.Create<TestDataB3, TestDataA3>();

            //------------------------------------------
            // Act
            //------------------------------------------
            try
            {
                om1.Verify();
                Assert.Fail("Map to object B with field without a source was allowed");
            }
            catch (InvalidOperationException exp)
            {
                //NOTE: Create specific exception
                if (exp.Message != "There is no property definition for property DataThatDoesNotExistInA in the to-object TestDataB3.")
                    throw;
            }

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
        }

        //NOTE: Add multiple properties not allowed
        //NOTE: Add properties after verify not allowed

        [TestMethod]
        public void VerifyInvalidDatatype()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA4 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true, DatatypeMissmatch = "123"};
            if (ObjectMap.Find<TestDataB4, TestDataA4>() != null) return;
            var om1 = ObjectMap.Create<TestDataB4, TestDataA4>();

            //------------------------------------------
            // Act
            //------------------------------------------
            try
            {
                om1.Verify(); //NOTE: Perhaps throw exception here, if a map of invalid types cannot be done. Instead of throwing in runtime

                var objectB = objectA.MapObject<TestDataB4, TestDataA4>();
                Assert.Fail("Map object that has missmathing datatypes was allowed");
            }
            catch (ArgumentException exp)
            {
                //"Object of type 'System.String' cannot be converted to type 'System.Int32'. This might be a hierary object. Add a Converter function to this property map. Trying to convert to Int32 DatatypeMissmatch from System.String DatatypeMissmatch."
            }

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
        }

        [TestMethod]
        public void MapStatic()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const string staticValue = "SomeStaticValue";
            var objectA = new TestDataA5 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true};
            var om1 = ObjectMap.Find<TestDataB5, TestDataA5>();
            if (om1 == null)
            {
                om1 = ObjectMap.Create<TestDataB5, TestDataA5>();
                om1.AddStaticPropertyMap<string>("StaticData");
                om1.Verify();
            }

            //------------------------------------------
            // Act
            //------------------------------------------
            om1.SetStaticPropertyMapValue("StaticData", staticValue);
            var objectB = objectA.MapObject<TestDataB5, TestDataA5>();
            
            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsTrue(Compare(objectA, objectB) == 0, "Objects A and B does not contain the same data");
            Assert.IsTrue(objectB.StaticData == staticValue, "Static data has not been set correctly");
        }

        [TestMethod]
        public void MapFunction()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA6 { StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true };
            var om1 = ObjectMap.Find<TestDataB6, TestDataA6>();
            if (om1 == null)
            {
                om1 = ObjectMap.Create<TestDataB6, TestDataA6>();
                om1.AddPropertyFunctionMap("Data", "GetData");
                om1.Verify();
            }

            //------------------------------------------
            // Act
            //------------------------------------------
            var objectB = objectA.MapObject<TestDataB6, TestDataA6>();

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsTrue(Compare(objectA, objectB) == 0, "Objects A and B does not contain the same data");
            Assert.IsTrue(objectB.Data == objectA.GetData(), "Function data has not been set correctly");
        }

        [TestMethod]
        public void MapConvertProperty()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const int intData = 77;
            const string stringData = "X";
            var date = DateTime.Now;
            var objectA = new TestDataA7 { StringData = "A", IntData = intData, DateTimeData = DateTime.Now, NullableBoolData = true, OtherDate = date };
            var om1 = ObjectMap.Find<TestDataB7, TestDataA7>();
            if (om1 == null)
            {
                om1 = ObjectMap.Create<TestDataB7, TestDataA7>();
                om1.SetConverter("StringData", delegate { return ConvertA7A(stringData); }); //Use other data. Detatch method, and provide data.
                om1.SetConverter("IntData", ConvertA7B);    //Manipulate data, Send the source property value to the method.
                om1.SetConverter("OtherDate", ConvertA7C);  //Convert from one data type to another.
                om1.Verify();
            }

            //------------------------------------------
            // Act
            //------------------------------------------
            var objectB = objectA.MapObject<TestDataB7, TestDataA7>();

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsFalse(Compare(objectA, objectB) == 0, "Objects A and B does contain the same data");
            Assert.IsTrue(objectB.StringData == ConvertA7A(stringData), "Converted string data does not match");
            Assert.IsTrue(objectB.IntData == (int)ConvertA7B(intData), "Converted int data does not match");
            Assert.IsTrue(objectB.OtherDate == (string)ConvertA7C(date), "Converted int data does not match");
        }

        //Manipulate data
        internal static string ConvertA7A(string obj)
        {
            return obj + obj + obj;
        }

        //Manipulate data
        internal static object ConvertA7B(object obj)
        {
            return (int)obj * 2;
        }

        //Convert from DateTime to string
        internal static object ConvertA7C(object obj)
        {
            var x = (DateTime) obj;
            return string.Format("{0} {1}", x.ToShortDateString(), x.ToLongTimeString());
        }

        [TestMethod]
        public void MapWithSubObjects()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA8 { StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true, TestDataSub = new TestDataA8Sub { StringData = "B", IntData = 99, DateTimeData = DateTime.Now.AddDays(1), NullableBoolData = false } };
            var om1 = ObjectMap.Find<TestDataB8, TestDataA8>();
            if (om1 == null)
            {
                om1 = ObjectMap.Create<TestDataB8, TestDataA8>();
                om1.SetConverter("TestDataSub", ConvertTestDataSub);
                om1.Verify();
            }

            //------------------------------------------
            // Act
            //------------------------------------------
            var objectB = objectA.MapObject<TestDataB8, TestDataA8>();

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsTrue(Compare(objectA, objectB) == 0, "Objects A and B does not contain the same data");
            Assert.IsTrue(Compare(objectA.TestDataSub, objectB.TestDataSub) == 0, "Sub objects A and B does not contain the same data");
        }

        //Map child-object
        internal static object ConvertTestDataSub(object obj)
        {
            var x = (TestDataA8Sub)obj;
            return x.MapObject<TestDataB8Sub, TestDataA8Sub>();
        }

        [TestMethod]
        public void MapWithObjectConvert()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var objectA = new TestDataA9 { StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true };
            var om1 = ObjectMap.Find<TestDataB9, TestDataA9>();
            if (om1 == null)
            {
                om1 = ObjectMap.Create<TestDataB9, TestDataA9>();
                om1.RemovePropertyMap("StringData");    //Remove the original mapping
                om1.AddObjectPropertyMap("StringData", MapObjectConverter); //Provide the parent object to the converter function (Not just the property value)
                om1.Verify();
            }

            //------------------------------------------
            // Act
            //------------------------------------------
            var objectB = objectA.MapObject<TestDataB9, TestDataA9>();

            //------------------------------------------
            // Assert
            //------------------------------------------            
            Assert.IsNotNull(om1, "Object map was not returned on create");
            Assert.IsFalse(Compare(objectA, objectB) == 0, "Objects A and B does contain the same data");
            Assert.IsFalse(objectA.StringData == MapObjectConverter(objectA), "String data not set correctly");
        }

        [TestMethod]
        public void MapList()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var sourceLsit = new List<TestDataA10>
                                 {
                                     new TestDataA10 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true},
                                     new TestDataA10 {StringData = "B", IntData = 78, DateTimeData = DateTime.Now, NullableBoolData = true},
                                     new TestDataA10 {StringData = "C", IntData = 79, DateTimeData = DateTime.Now, NullableBoolData = true}
                                 };

            //------------------------------------------
            // Act
            //------------------------------------------
            var resultList = sourceLsit.MapObject<TestDataB10, TestDataA10>();

            //------------------------------------------
            // Assert
            //------------------------------------------
            for (var i = 0; i < sourceLsit.Count; i++ )
                Assert.IsTrue(Compare(sourceLsit[i], resultList[i]) == 0, "The items are not the same");
        }

        [TestMethod]
        public void MapCollection()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var sourceLsit = new Collection<TestDataA10>
                                 {
                                     new TestDataA10 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true},
                                     new TestDataA10 {StringData = "B", IntData = 78, DateTimeData = DateTime.Now, NullableBoolData = true},
                                     new TestDataA10 {StringData = "C", IntData = 79, DateTimeData = DateTime.Now, NullableBoolData = true}
                                 };

            //------------------------------------------
            // Act
            //------------------------------------------
            var resultList = sourceLsit.MapObject<TestDataB10, TestDataA10>();

            //------------------------------------------
            // Assert
            //------------------------------------------
            for (var i = 0; i < sourceLsit.Count; i++)
                Assert.IsTrue(Compare(sourceLsit[i], resultList[i]) == 0, "The items are not the same");
        }

        [TestMethod]
        public void MapDictionary()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var sourceList = new Dictionary<string, TestDataA10>
                                 {
                                     { Guid.NewGuid().ToString(), new TestDataA10 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true} },
                                     { Guid.NewGuid().ToString(), new TestDataA10 {StringData = "B", IntData = 78, DateTimeData = DateTime.Now, NullableBoolData = true} },
                                     { Guid.NewGuid().ToString(), new TestDataA10 {StringData = "C", IntData = 79, DateTimeData = DateTime.Now, NullableBoolData = true} }
                                 };

            //------------------------------------------
            // Act
            //------------------------------------------
            var resultList = sourceList.MapObject<string, TestDataB10, TestDataA10>();

            //------------------------------------------
            // Assert
            //------------------------------------------
            foreach (var item in sourceList)
            {
                var resultItem = resultList[item.Key];
                Assert.IsNotNull(resultItem, "A result item is missing.");
                Assert.IsTrue(Compare(item.Value, resultItem) == 0, "The items are not the same");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MapWithNoTargetNotAllowed()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var sourceItem = new TestDataA11 {StringData = "A", IntData = 77, DateTimeData = DateTime.Now, NullableBoolData = true};

            //------------------------------------------
            // Act
            //------------------------------------------
            var result = sourceItem.MapObject<TestDataB11, TestDataA11>();

            //------------------------------------------
            // Assert
            //------------------------------------------       
        }

        //Map using the entire source object (not just the property)
        public string MapObjectConverter(object obj)
        {
            var x = (TestDataA9)obj;
            return x.StringData + x.StringData + x.StringData;
        }

        public static int Compare (ITestData one, ITestData other)
        {
            if (string.Compare(one.StringData, other.StringData) != 0) return string.Compare(one.StringData, other.StringData);
            if (one.IntData != other.IntData) return one.IntData - other.IntData;
            if (DateTime.Compare(one.DateTimeData, other.DateTimeData) != 0) return DateTime.Compare(one.DateTimeData, other.DateTimeData);
            if (one.NullableBoolData != other.NullableBoolData) return -1;

            return 0;
        }


    }
}
