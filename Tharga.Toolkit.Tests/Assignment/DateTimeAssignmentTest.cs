using System;
using NUnit.Framework;

namespace Tharga.Toolkit.Tests.Assignment
{
    [TestFixture]
    public class DateTimeAssignmentTest
    {
        [Test]
        public void Default_assignment()
        {
            //Arrange
            var obj = default(DateTime);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Default_nullable_assignment()
        {
            //Arrange
            var obj = default(DateTime?);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_default_assignment()
        {
            //Arrange
            var obj = (DateTime?)null;

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsFalse(isAssigned);
        }

        [Test]
        public void Explicit_non_default_assignment()
        {
            //Arrange
            var obj = new DateTime(2010, 1, 1);

            //Act
            var isAssigned = obj.IsAssigned();

            //Assert
            Assert.IsTrue(isAssigned);
        }
    }
}