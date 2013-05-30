using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tharga.Toolkit.Test
{
    [TestClass]
    public class UserInformationTest
    {
        [TestMethod]
        public void GetCurrent()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var ui = UserInformation.GetCurrent();

            //------------------------------------------
            // Act
            //------------------------------------------
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var nameArray = wi.Name.Split('\\');

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(nameArray[0], ui.Domain) == 0, "The domain name is not correct");
            Assert.IsTrue(string.Compare(nameArray[1], ui.UserName) == 0, "The user name is not correct");
        }
    }
}
