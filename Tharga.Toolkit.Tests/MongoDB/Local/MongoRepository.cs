using System;
using NUnit.Framework;
using Tharga.Toolkit.Test.Mocking;

namespace Tharga.Toolkit.Test.MongoDB.Local
{
    [TestFixture]
    class MongoRepository
    {
        [Test]
        [Ignore("")]
        public void Save_some_entity()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var someEntity = new SomeEntity {Id = Guid.NewGuid()};

            //------------------------------------------
            // Act
            //------------------------------------------
            throw new NotImplementedException();
            //LocalStorage.Repository.MongoRepository.Instance.Save(someEntity);

            //------------------------------------------
            // Assert
            //------------------------------------------
        }

        [Test]
        [Ignore("")]
        public void Get_all_entities()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------

            //------------------------------------------
            // Act
            //------------------------------------------
            throw new NotImplementedException();
            //var items = LocalStorage.Repository.MongoRepository.Instance.GetAll<SomeEntity>();

            //------------------------------------------
            // Assert
            //------------------------------------------
            throw new NotImplementedException();
            //Assert.IsTrue(items.Any(),"There are no entities in the testdatabase.");
        }
    }
}
