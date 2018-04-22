using System.Collections.Generic;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class DictionaryAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(Dictionary<int, string>);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (Dictionary<int, string>)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment_with_content()
        {
            //Arrange
            var obj = new Dictionary<int, string> {{1, "a"}, {2, "b"}};

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment_empty()
        {
            //Arrange
            var obj = new Dictionary<int, string> { };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_content()
        {
            //Arrange
            var obj = new Dictionary<int, string> { { default(int), default(string) } };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment_with_default_content_value()
        {
            //Arrange
            var obj = new Dictionary<int, string> { { 1, default(string) } };

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }
    }
}