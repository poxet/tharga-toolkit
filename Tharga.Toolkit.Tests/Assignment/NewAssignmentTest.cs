using System;
using System.Collections.Generic;
using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;
using Tharga.Toolkit.Assignment;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class NewAssignmentTest
    {
        [Test]
        public void By_default_dateTime_is_not_assigned()
        {
            //Arrange
            var obj = new DateTime();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeSimpleClass_is_not_assigned()
        {
            //Arrange
            var obj = new SomeSimpleClass();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_string_is_not_assigned()
        {
            //Arrange
            var obj = new string(new char[] { });

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeCircleClass_is_not_assigned()
        {
            //Arrange
            var obj = new SomeCircleClass();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_string_array_is_not_assigned()
        {
            //Arrange
            var obj = new string[] {};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_dictionary_is_not_assigned()
        {
            //Arrange
            var obj = new Dictionary<int, string> { };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_list_of_SomeSimpleClass_is_not_assigned()
        {
            //Arrange
            var obj = new List<SomeSimpleClass>{};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_list_of_string_is_not_assigned()
        {
            //Arrange
            var obj = new List<string> {};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_guid_is_not_assigned()
        {
            //Arrange
            var obj = new Guid();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeTestClass_is_not_assigned()
        {
            //Arrange
            var obj = new SomeTestClass(0);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeTestStruct_is_not_assigned()
        {
            //Arrange
            var obj = new SomeTestStruct();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeClass_is_not_assigned()
        {
            //Arrange
            var obj = new SomeClass();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_object_is_not_assigned()
        {
            //Arrange
            var obj = new object();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }
    }
}