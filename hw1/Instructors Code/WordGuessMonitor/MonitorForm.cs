using System;
using System.Net;
using System.Windows.Forms;

using Common;
using Common.Messages;
using log4net;

namespace WordGuessMonitor
{
    public partial class MonitorForm : Form
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(MonitorForm));
        private readonly MonitorSettings _settings = new MonitorSettings();

        private string _serverHostname;
        private int _serverPort;
        private IPAddress _serverHost;
        private IPEndPoint _serverEndPoint;

        private Communicator _comm;
        private Listener _listener;
        private Worker _worker;

        private Timer _refreshTimer;
        private bool _expectingStatusMessage;
        private bool _isMonitoring;

        public MonitorForm()
        {
            InitializeComponent();
        }

        private void _refreshTimer_Tick(object sender, EventArgs e)
        {
            Log.Debug($"Send a GetStatusMessage to {_serverEndPoint}");
            var msg = new GetStatusMessage();

            ComputeServerEndPoint();
            _comm.Send(msg, _serverEndPoint);
            _expectingStatusMessage = true;
        }

        private void WorkerGotStatus(Common.Messages.Message msg)
        {
            if (InvokeRequired)
            {
                var handler = new MessageHandler(WorkerGotStatus);
                Invoke(handler, msg);
            }
            else
            {
                Log.Debug("Got a StatusMessage");

                StatusMessage statusMessage = msg as StatusMessage;
                Log.Debug($"_expecting status message {_expectingStatusMessage}");
                if (!_expectingStatusMessage || statusMessage == null)
                    return;

                _expectingStatusMessage = false;

                gameCount.Text = statusMessage.ActiveGameCount.ToString();
                playerCount.Text = statusMessage.Students.Count.ToString();

                Log.Debug($"Game Count = {gameCount.Text}");
                Log.Debug($"Player Count = {playerCount.Text}");

                playersListView.Items.Clear();
                foreach (StatusMessage.PlayerInfo s in statusMessage.Students)
                {
                    var item = new ListViewItem(new[]
                    {
                       s.Id.ToString(),
                       s.Alias,
                       s.GameCount.ToString(),
                       s.GuessCount.ToString(),
                       s.HintCount.ToString(),
                       s.ExitCount.ToString(),
                       s.HeartbeatCount.ToString(),
                       s.LastMessage.ToShortTimeString(),
                       s.HighScore.ToString()
                    });
                    playersListView.Items.Add(item);
                }
            }
        }

        private void ParseServerEndPoint()
        {
            serverEndPointError.Visible = false;
            if (string.IsNullOrWhiteSpace(serverEndPoint.Text))
            {
                serverEndPointError.Text = @"The server end point cannot be blank";
                serverEndPointError.Visible = true;
                return;
            }

            string[] endPointParts = serverEndPoint.Text.Split(':');
            if (endPointParts.Length!=2)
            {
                serverEndPointError.Text = @"The server end point must be specified as hostname:port";
                serverEndPointError.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(endPointParts[0]))
            {
                serverEndPointError.Text = @"The server's hostname cannot be blank";
                serverEndPointError.Visible = true;
                return;
            }

            if (string.IsNullOrWhiteSpace(endPointParts[1]))
            {
                serverEndPointError.Text = @"The server's port number cannot be blank";
                serverEndPointError.Visible = true;
                return;
            }

            _serverHostname = endPointParts[0];
            _serverPort = ParseInt(endPointParts[1]);

            if (_serverPort < 1 || _serverPort > short.MaxValue)
            {
                serverEndPointError.Text = @"Invalid port number -- it must be a integer between 1 - 	65535";
                serverEndPointError.Visible = true;
            }

        }

        private void ComputeServerEndPoint()
        {
            _serverHost = LookupAddress(_serverHostname);
            if (_serverHost == null)
            {
                serverEndPointError.Text = $@"Cannot find an IP Address for {_serverHostname}";
                serverEndPointError.Visible = true;
                return;
            }

            _serverEndPoint = new IPEndPoint(_serverHost, _serverPort);
        }

        private static IPAddress LookupAddress(string host)
        {
            IPAddress result = null;
            if (!string.IsNullOrWhiteSpace(host))
            {
                IPAddress[] addressList = Dns.GetHostAddresses(host);
                for (int i = 0; i < addressList.Length && result == null; i++)
                    if (addressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        result = addressList[i];
            }
            return result;
        }

        private int ParseInt(string text)
        {
            var result = 0;
            if (!string.IsNullOrWhiteSpace(text))
                int.TryParse(text, out result);
            return result;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Setup();
        }

        private void Setup()
        {
            gameCount.Text = "";
            playerCount.Text = "";

            if (!string.IsNullOrEmpty(_settings.ServerList))
            {
                var possibleServers = _settings.ServerList.Split(',');
                foreach (var serverEp in possibleServers)
                {
                    serverEndPoint.Items.Add(serverEp);
                }
                serverEndPoint.SelectedIndex = 0;
            }

            Log.Debug("Creating a communicator");
            _comm = new Communicator(0);
            Log.Debug($"Local end point {_comm.LocalEndPoint}");
            Log.Debug("Creating a listener");
            _listener = new Listener(_comm, _settings.Timeout);
            Log.Debug("Creating a worker");
            _worker = new Worker(_comm, _settings.Timeout);
            Log.Debug("Starting a listener");
            _listener.Start();
            Log.Debug("Starting a worker");
            _worker.Start();

            _worker.GotStatus += WorkerGotStatus;
        }

        private void startStop_Click(object sender, EventArgs e)
        {
            if (_isMonitoring)
                StopMonitoring();
            else
                StartMonitoring();
        }

        private void StartMonitoring()
        {
            _isMonitoring = true;
            startStopButton.Text = @"Stop";

            _refreshTimer = new Timer();
            _refreshTimer.Tick += _refreshTimer_Tick;
            _refreshTimer.Interval = _settings.RefreshFrequency;
            _refreshTimer.Start();
        }

        private void StopMonitoring()
        {
            _isMonitoring = false;
            startStopButton.Text = @"Start";

            _refreshTimer.Stop();
            _refreshTimer = null;

            gameCount.Text = "";
            playerCount.Text = "";
            playersListView.Items.Clear();
        }

        private void MonitorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isMonitoring)
                StopMonitoring();

            _listener.Stop();
            _worker.Stop();
            _comm.Close();
        }

        private void serverEndPoint_Leave(object sender, EventArgs e)
        {
            if (_isMonitoring)
                StopMonitoring();

            ParseServerEndPoint();
        }

        private void serverEndPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_isMonitoring)
                StopMonitoring();

            ParseServerEndPoint();
        }
    }
}
