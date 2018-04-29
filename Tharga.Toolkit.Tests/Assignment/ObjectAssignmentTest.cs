using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class ObjectAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(object);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'Object'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new object();

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }
    }
}