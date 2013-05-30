using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tharga.Toolkit.Test.Mocking;

namespace Tharga.Toolkit.Test
{
    [TestClass]
    public class Test
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
