﻿using System.Collections.Generic;
using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class SomeTestStructAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(SomeTestStruct);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestStruct'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (SomeTestStruct?)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestStruct?'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new SomeTestStruct
            {
                StringMember = "A1",
                StringProperty = "B1",
                StringListMember = new List<string> {"A1"},
                StructListProperty = new List<SomeTestStruct> { }
            };
            obj.StructListProperty = new List<SomeTestStruct>
            {
                new SomeTestStruct
                {
                    StringMember = "A2",
                    StringProperty = "B2",
                    StringListMember = new List<string> {"A2"},
                    StructListProperty = new List<SomeTestStruct> { obj }
                }
            };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_properties()
        {
            //Arrange
            var obj = new SomeTestStruct { };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestStruct'."));
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_properties_with_missing_property()
        {
            //Arrange
            var obj = new SomeTestStruct
            {
                StringMember = "A",
                StringProperty = null,
                StringListMember = new List<string> { "A1" },
                StructListProperty = new List<SomeTestStruct> { }
            };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestStruct.StringProperty'."));
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_properties_with_missing_member()
        {
            //Arrange
            var obj = new SomeTestStruct
            {
                StringMember = null,
                StringProperty = "A",
                StringListMember = new List<string> { "A1" },
                StructListProperty = new List<SomeTestStruct> { }
            };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestStruct.StringMember'."));
        }
    }
}