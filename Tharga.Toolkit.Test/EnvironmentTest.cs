using System;
using System.Reflection;
using NUnit.Framework;

namespace Tharga.Toolkit.Test
{
    [TestFixture]
    public class EnvironmentTest
    {
        [Test]
        public void AssemblyTitle()
        {            
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Title();

            //------------------------------------------
            // Act
            //------------------------------------------
            
            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(data, "Tharga Toolkit Test") == 0, "The title is not right");
        }

        [Test]
        public void Description()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Description();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly description attribute contains no data");
        }

        [Test]
        public void Company()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Company();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly company attribute contains no data");
            Assert.IsTrue(string.Compare(data, "Thargelion AB") == 0, "The company name is not right");
        }

        [Test]
        public void Product()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Product();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly product attribute contains no data");
            Assert.IsTrue(string.Compare(data, "Toolkit Test") == 0, "The product name is not right");
        }

        [Test]
        public void Copyright()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Copyright();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly copyright attribute contains no data");
            Assert.IsTrue(string.Compare(data, "Copyright © Thargelion AB 2011") == 0, "The copyright name is not right");
        }

        [Test]
        public void Trademark()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Trademark();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly trademark attribute contains no data");
        }

        [Test]
        public void Version()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Version();
            var dummyVersion = new Version();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(data.ToString(), dummyVersion.ToString()) != 0, "The assembly version attribute looks like a new empty instance.");
            Assert.IsTrue(data is Version, "The assembly version attribute data is not of type Version.");
        }

        [Test]
        public void KeyName()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.KeyName();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(data), "Assembly key name contains no data");
            Assert.IsTrue(data.Contains("."), "Assembly key name is not correctly formatted");
        }

        [Test]
        public void Location()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var data = assembly.Location();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(data == assembly.Location, "Location returns incorrect value");
        }

        [Test]
        public void ToolkitVersion()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var data = Environment.ToolkitVersion;
            var dummyVersion = new Version();

            //------------------------------------------
            // Act
            //------------------------------------------                        

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(data.ToString(), dummyVersion.ToString()) != 0, "The toolkit version looks like a new empty instance.");
            Assert.IsTrue(data is Version, "The toolkit version data is not of type Version.");
        }

        [Test]
        public void DataPath()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var assembly = Assembly.GetExecutingAssembly();
            var path = assembly.LocalDataPath();

            //------------------------------------------
            // Act
            //------------------------------------------

            //------------------------------------------
            // Assert
            //------------------------------------------
            //Assert.IsTrue(string.Compare(path, "") != 0, "Nothing returned");
            Assert.IsFalse(string.IsNullOrEmpty(path), "Nothing returned");
        }
    }
}
