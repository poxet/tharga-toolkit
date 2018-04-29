using HM.Order.OrderService.Business.Tests.UnitTests.CompareExtensions;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class SomeTestClassAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(SomeTestClass);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestClass'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (SomeTestClass)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestClass'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = SomeTestClass.Create();

            //Act
            var isAssigned = obj.IsAssigned(new[] { "SomeTestClass.ClassListProperty[0].ClassListProperty" });

            //Assert
            Assert.IsTrue(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_properties()
        {
            //Arrange
            var obj = new SomeTestClass(0);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeTestClass.IntReadProperty'."));
        }
    }
}