﻿using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class SomeSimpleClassAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(SomeSimpleClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeSimpleClass'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (SomeSimpleClass) null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeSimpleClass'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new SomeSimpleClass {Data = "ABC123"};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_properties()
        {
            //Arrange
            var obj = new SomeSimpleClass {  };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeSimpleClass.Data'."));
        }
    }
}