using System;
using NUnit.Framework;
using Tharga.Toolkit.Test.Mocking;

namespace Tharga.Toolkit.Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void AssignRandomValues()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var someEntity = new SomeEntity();

            //------------------------------------------
            // Act
            //------------------------------------------
            someEntity.AssignRandomValues();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.AreNotEqual(Guid.Empty, someEntity.Id);
        }

        [Test]
        public void AssignRandomValues_for_sub_entities()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var someParentEntity = new SomeParentEntity {SomeField = new SomeEntity{ Id = Guid.Empty}};

            //------------------------------------------
            // Act
            //------------------------------------------
            someParentEntity.AssignRandomValues();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.AreNotEqual(Guid.Empty, someParentEntity.SomeField.Id);
            Assert.AreNotEqual(Guid.Empty, someParentEntity.SomeProperty.Id);
        }

        [Test]
        public void AssignRandomValues_for_empty_sub_entities()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var someParentEntity = new SomeParentEntity { SomeField = null };

            //------------------------------------------
            // Act
            //------------------------------------------
            someParentEntity.AssignRandomValues();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.AreNotEqual(Guid.Empty, someParentEntity.SomeField.Id);
            Assert.AreNotEqual(Guid.Empty, someParentEntity.SomeProperty.Id);
        }
    }
}
