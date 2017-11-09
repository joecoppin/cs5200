using System;
using System.Windows.Forms;

namespace AppShared
{
    public partial class AppProcessDisplayForm : Form
    {
        public AppProcessDisplayForm()
        {
            InitializeComponent();
        }

        public AppProcess MyProcess { get; set; }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            publicEndPoint.Text = MyProcess.BestLocalEndPoint;
            processId.Text = MyProcess.ProcessId.ToString();
            Text = $"{MyProcess.ProcessType} (Id={processId.Text})";
        }

        private void AppProcessDisplayForm_Load(object sender, EventArgs e)
        {
            MyProcess.Shutdown += MyProcess_Shutdown;
            refreshTimer.Start();
        }
        private void MyProcess_Shutdown(CommSub.StateChange changeInfo)
        {
            if (InvokeRequired)
            {
                CommSub.StateChange.Handler handler = MyProcess_Shutdown;
                Invoke(handler, changeInfo);
            }
            else
                Close();
        }

        private void AppProcessDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            MyProcess.Stop();
        }
    }
}
