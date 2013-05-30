using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tharga.Toolkit.Test.Mocking;

namespace Tharga.Toolkit.Test.MongoDB.Local
{
    [TestClass]
    class MongoRepository
    {
        [TestMethod]
        public void Save_some_entity()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var someEntity = new SomeEntity {Id = Guid.NewGuid()};
            
            //------------------------------------------
            // Act
            //------------------------------------------
            LocalStorage.Repository.MongoRepository.Instance.Save(someEntity);

            //------------------------------------------
            // Assert
            //------------------------------------------
        }

        [TestMethod]
        public void Get_all_entities()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------

            //------------------------------------------
            // Act
            //------------------------------------------
            var items = LocalStorage.Repository.MongoRepository.Instance.GetAll<SomeEntity>();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(items.Any(),"There are no entities in the testdatabase.");
        }
    }
}
