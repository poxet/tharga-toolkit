using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class SomeClassAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(SomeClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeClass'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (SomeClass)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeClass'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new SomeClass { Data1 = "ABC123", Data2 = 1, Data3 = 1.2F };

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
            var obj = new SomeClass { };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeClass.Data1'."));
        }

        [Test]
        public void Explicit_non_default_assignment_with_some_default_properties()
        {
            //Arrange
            var obj = new SomeClass { Data1 = "ABC123" };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeClass.Data2'."));
        }
    }
}