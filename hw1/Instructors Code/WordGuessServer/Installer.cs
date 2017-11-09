using System;
using System.ComponentModel;
using System.ServiceProcess;


namespace MDSC.SystemServices
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;

        public Installer()
        {
            InitializeComponent();

            string name = "WordGuessServer";

            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // serviceInstaller1
            // 
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            this.serviceInstaller1.Description = string.Format("{0} - {1}", name, version);
            this.serviceInstaller1.DisplayName = name;
            this.serviceInstaller1.ServiceName = name;

            this.serviceInstaller1.ServicesDependedOn = new string[] { "" };
            // 
            // Installer
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[]
                        {
                            this.serviceProcessInstaller1,
                            this.serviceInstaller1
                        }
                  );
        }

        protected override void OnAfterInstall(System.Collections.IDictionary savedState)
        {
            base.OnAfterInstall(savedState);

            if (serviceInstaller1.StartType == ServiceStartMode.Automatic)
            {
                ServiceController sc = GetController();
                sc.Start();
            }
        }

        protected override void OnBeforeUninstall(System.Collections.IDictionary savedState)
        {
            base.OnBeforeUninstall(savedState);

            ServiceController sc = GetController();
            if (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.Paused)
            {
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 30));
            }
        }

        private ServiceController GetController()
        {
            foreach (ServiceController sc in ServiceController.GetServices())
            {
                if (sc.ServiceName == serviceInstaller1.ServiceName)
                    return sc;
            }

            return null;
        }
    }
}
