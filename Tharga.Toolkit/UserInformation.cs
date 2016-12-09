namespace Tharga.Toolkit
{
    public class UserInformation
    {
        public string UserName { get; private set; }
        public string Domain { get; private set; }

        public static UserInformation GetCurrent()
        {
            var wi = System.Security.Principal.WindowsIdentity.GetCurrent();
            var name = wi != null ? wi.Name.Split('\\') : new[] {"Unknown", "Unknown"};

            return new UserInformation {Domain = name[0], UserName = name[1]};
        }
    }
}