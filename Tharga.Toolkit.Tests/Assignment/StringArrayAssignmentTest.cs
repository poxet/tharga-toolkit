using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class StringArrayAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(string[]);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'String[]'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (string[])null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'String[]'."));
        }

        [Test]
        public void Explicit_non_default_assignment_with_content()
        {
            //Arrange
            var obj = new []{ "ABC123" };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_non_default_assignment_empty()
        {
            //Arrange
            var obj = new string[] { };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_content()
        {
            //Arrange
            var obj = new [] {default(string)};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'String[0]'."));
        }
    }
}