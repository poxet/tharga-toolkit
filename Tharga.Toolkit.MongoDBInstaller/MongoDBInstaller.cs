using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Text;


namespace Tharga.Toolkit.MongoDBInstaller
{
    [RunInstaller(true)]
    public partial class MongoDBInstaller : Installer
    {
        public MongoDBInstaller()
        {
            InitializeComponent();
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            var sb = new StringBuilder();
            var path = @"C:\Tharga.Toolkit.MongoDBInstallerLog.txt";
            try
            {
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");

                const string name = "ThargaMongoDB"; //"MarathonMemberActivityDB"; //NOTE: This should be an option
                ExecuteCommand(string.Format("\"{0}\\mongod.exe\"", path), string.Format("--install --logpath \"{0}\\MongoLog.txt\" --serviceName {1}", path, name), ref sb);
                ExecuteCommand("cmd", string.Format("/c net start \"{0}\"", name), ref sb);
                //ExecuteCommand("ocsetup", string.Format("MSMQ-Container;MSMQ-Server /norestart"), ref sb);
                //ExecuteCommand("ocsetup", string.Format("MSMQ-Container;MSMQ-Server"), ref sb);
                ExecuteCommand("ocsetup", string.Format("MSMQ-Server"), ref sb); //NOTE: This should be an option

                sb.AppendLine("Done");
            }
            catch (Exception exception)
            {
                sb.AppendLine(exception.Message);
            }
            finally
            {
                var logFile = string.Format("{0}\\Tharga.Toolkit.MongoDBInstallerLog.txt", path);

                System.IO.File.WriteAllText(logFile, sb.ToString());
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            var sb = new StringBuilder();
            var path = @"C:\Tharga.Toolkit.MongoDBInstallerLogU.txt";
            try
            {
                const string name = "ThargaMongoDB"; //"MarathonMemberActivityDB"; //TODO: This should be an option
                path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace(@"file:\", "");
                ExecuteCommand("cmd", string.Format("/c net stop \"{1}\"", name), ref sb);
                ExecuteCommand(string.Format("\"{0}\\mongod.exe\"", path), string.Format("--remove --serviceName {0}",name), ref sb);
            }
            catch (Exception exception)
            {
                sb.AppendLine(exception.Message);
            }
            finally
            {
                var logFile = string.Format("{0}\\Tharga.Toolkit.MongoDBInstallerLogU.txt", path);

                System.IO.File.WriteAllText(logFile, sb.ToString());
            }
        }

        private static void ExecuteCommand(string command, string arguments, ref StringBuilder sb)
        {
            sb.AppendLine(string.Format("Execute: {0} {1}", command, arguments));

            var procStartInfo = new System.Diagnostics.ProcessStartInfo(command, arguments) { RedirectStandardOutput = true, UseShellExecute = false, CreateNoWindow = true };
            var proc = new System.Diagnostics.Process { StartInfo = procStartInfo };
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();

            sb.AppendLine(string.Format("Result: {0}", result));
            sb.AppendLine();
        }
    }
}
