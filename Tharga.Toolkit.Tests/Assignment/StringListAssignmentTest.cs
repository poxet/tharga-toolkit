using System.Collections.Generic;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class StringListAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(List<string>);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'List`1'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (List<string>)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'List`1'."));
        }

        [Test]
        public void Explicit_non_default_assignment_with_content()
        {
            //Arrange
            var obj = new List<string> { "ABC123" };

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
            var obj = new List<string>[] { };

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
            var obj = new List<string> { default(string) };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'List`1[0]'."));
        }
    }
}