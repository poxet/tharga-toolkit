using System;
using System.Collections.Generic;
using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;
using Tharga.Toolkit.Assignment;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class DefaultAssignmentTest
    {
        [Test]
        public void By_default_int_is_not_assigned()
        {
            //Arrange
            var obj = default(int);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_dateTime_is_not_assigned()
        {
            //Arrange
            var obj = default(DateTime);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeSimpleClass_is_not_assigned()
        {
            //Arrange
            var obj = default(SomeSimpleClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_string_is_not_assigned()
        {
            //Arrange
            var obj = default(string);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeCircleClass_is_not_assigned()
        {
            //Arrange
            var obj = default(SomeCircleClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_string_array_is_not_assigned()
        {
            //Arrange
            var obj = default(string[]);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_dictionary_is_not_assigned()
        {
            //Arrange
            var obj = default(Dictionary<int, string>);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_list_of_SomeSimpleClass_is_not_assigned()
        {
            //Arrange
            var obj = default(List<SomeSimpleClass>);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_list_of_string_is_not_assigned()
        {
            //Arrange
            var obj = default(List<string>);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_guid_is_not_assigned()
        {
            //Arrange
            var obj = default(Guid);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeTestClass_is_not_assigned()
        {
            //Arrange
            var obj = default(SomeTestClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeTestStruct_is_not_assigned()
        {
            //Arrange
            var obj = default(SomeTestStruct);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_SomeClass_is_not_assigned()
        {
            //Arrange
            var obj = default(SomeClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_object_is_not_assigned()
        {
            //Arrange
            var obj = default(object);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_decimal_is_not_assigned()
        {
            //Arrange
            var obj = default(decimal);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }

        [Test]
        public void By_default_double_is_not_assigned()
        {
            //Arrange
            var obj = default(double);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.False(isAssigned);
        }
    }
}
