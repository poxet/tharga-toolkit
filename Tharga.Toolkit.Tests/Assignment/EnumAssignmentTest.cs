using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class EnumAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(SomeEnum);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeEnum'."));
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(SomeEnum?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeEnum?'."));
        }

        [Test]
        public void Explicit_default_null_assignment()
        {
            //Arrange
            var obj = (SomeEnum?)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeEnum?'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = SomeEnum.A;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'SomeEnum'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = SomeEnum.B;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned, isAssigned.Message);
            Assert.That(isAssigned.Message, Is.Null);
        }
    }
}