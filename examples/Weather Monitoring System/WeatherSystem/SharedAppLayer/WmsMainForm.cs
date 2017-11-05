using System;
using System.Windows.Forms;
using CommSubsystem;
using log4net;

namespace SharedAppLayer
{
    public partial class WmsMainForm : Form
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(WmsMainForm));

        protected WmsAppWorker Worker;
        protected bool NeedToRepaintDisplays;

        public RuntimeOptions Options { get; set; }

        protected WmsMainForm()
        {
            InitializeComponent();
        }

        private void WmsMainForm_Load(object sender, EventArgs e)
        {
            Logger.Info("Load WnsMainForm");
            SetupWorker();
            SetupEventHandlers();

            if (Options == null || !Options.AutoStart) return;

            Worker.Start();
            _startStopButton.Text = @"Stop";
        }

        private void WmsMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Info("Close WnsMainForm");
            RemoveEventHandlers();
            Worker.Stop();
        }

        protected virtual void SetupWorker() { }
        protected virtual void SetupEventHandlers() { }
        protected virtual void RemoveEventHandlers() { }

        private void _startStopButton_Click(object sender, EventArgs e)
        {
            Logger.Info("Start/Stop Button Clicked");

            if (_startStopButton.Text == @"Start")
            {
                Worker.Start();
                _startStopButton.Text = @"Stop";
            }
            else
            {
                Worker.Stop();
                _startStopButton.Text = @"Start";
            }
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            if (!NeedToRepaintDisplays) return;

            processId.Text = Options.ProcessId.ToString();
            status.Text = Worker.Status.ToString();
            DisplayDataServers();
        }

        private void DisplayDataServers()
        {
            _serverList.Items.Clear();
            if (Worker == null) return;

            var servers = Worker.GetDataServers();
            foreach (var server in servers)
            {
                var item = new ListViewItem(new[] { server.ProcessId.ToString(), server.EndPoint.ToString() });
                _serverList.Items.Add(item);
            }
        }
    }
}
