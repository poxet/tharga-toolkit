using System;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class TimeSpanAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(TimeSpan);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'TimeSpan'."));
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(TimeSpan?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'TimeSpan?'."));
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (TimeSpan?)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
            Assert.That(isAssigned.Message, Is.EqualTo("No assignment for 'TimeSpan?'."));
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new TimeSpan(2010, 1, 1);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
            Assert.That(isAssigned.Message, Is.Null);
        }
    }
}