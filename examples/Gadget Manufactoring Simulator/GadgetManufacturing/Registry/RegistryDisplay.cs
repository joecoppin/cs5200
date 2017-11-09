using System.Windows.Forms;

namespace Registry
{
    public partial class RegistryDisplay : Form
    {
        public RegistryDisplay()
        {
            InitializeComponent();
        }

        public Registry MyProcess { get; set; }

        private void refreshTimer_Tick(object sender, System.EventArgs e)
        {
            publicEndPoint.Text = MyProcess.BestLocalEndPoint;
            registrationCount.Text = MyProcess.RegisteredProcessCount.ToString() ?? "";
        }

        private void shutdownButton_Click(object sender, System.EventArgs e)
        {
            MyProcess.Shutdown -= MyProcess_Shutdown;
            MyProcess.StartShutdown();
            Close();
        }

        private void RegistryDisplay_Load(object sender, System.EventArgs e)
        {
            MyProcess.Shutdown += MyProcess_Shutdown;
            refreshTimer.Start();
        }

        private void MyProcess_Shutdown(CommSub.StateChange changeInfo)
        {
            Close();
        }
    }
}
