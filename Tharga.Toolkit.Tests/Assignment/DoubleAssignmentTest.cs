using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class DoubleAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(double);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Double'."));
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(double?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Double?'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (double)0;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Double'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = (double)1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.That(isAssigned.Message, Is.Null);
            Assert.IsTrue(isAssigned);
        }

        [Test]
        public void Explicit_nullable_non_default_assignment()
        {
            //Arrange
            var obj = (double?)1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }
    }
}