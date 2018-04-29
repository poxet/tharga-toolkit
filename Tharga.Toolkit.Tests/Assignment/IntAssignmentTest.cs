using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class IntAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(int);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Int32'."));
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(int?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Int32?'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = 0;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Int32'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = 1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_nullable_non_default_assignment()
        {
            //Arrange
            var obj = (int?)1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }
    }
}