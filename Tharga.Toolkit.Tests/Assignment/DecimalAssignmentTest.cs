using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class DecimalAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(decimal);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(decimal?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (decimal)0;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = (decimal)1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
        }

        [Test]
        public void Explicit_nullable_non_default_assignment()
        {
            //Arrange
            var obj = (decimal?)1;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
        }
    }
}