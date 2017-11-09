using System;
using System.Windows.Forms;
using AppLayer;

namespace GuiLayer
{
    public partial class SampleForm : Form
    {
        private Player _player;
        public SampleForm()
        {
            InitializeComponent();
        }

        private void _startButton_Click(object sender, EventArgs e)
        {
            if (_player != null) return;

            _player = new Player
            {
                Name = _name.Text,
                Handler = CounterDisplayHandler
            };
            _player.Start();
        }

        private void _stopButton_Click(object sender, EventArgs e)
        {
            if (_player == null) return;

            _player.Stop();
            _player = null;
        }

        private void CounterDisplayHandler(int counter)
        {
            if (InvokeRequired)
            {
                UpdateHandler mth = CounterDisplayHandler;
                BeginInvoke(mth, counter);
            }
            else
            {
                _countDisplay.Text = counter.ToString();
            }
        }
    }
}
