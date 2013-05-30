namespace Tharga.Toolkit
{
    /// <summary>
    /// Class containing user principal information.
    /// </summary>
    public class UserInformation
    {
        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; private set; }

        /// <summary>
        /// Gets the currently logged on user.
        /// </summary>
        /// <returns></returns>
        public static UserInformation GetCurrent()
        {
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var name = wi != null ? wi.Name.Split('\\') : new[] {"Unknown", "Unknown"};

            return new UserInformation {Domain = name[0], UserName = name[1]};
        }
    }
}
