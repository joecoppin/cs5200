using CommSubsystem;
using SharedAppLayer;

namespace WeatherStation
{
    public partial class MainForm
    {
        public WeatherStationRuntimeOptions MyOptions => Options as WeatherStationRuntimeOptions;

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void SetupWorker()
        {
            Worker = new WeatherStationWorker() { Options = Options };
        }

        protected override void SetupEventHandlers()
        {
            Worker.Startup += WorkerOnStartup;
            Worker.Update += WorkerOnUpdate;
            Worker.Error += WorkerOnError;
            Worker.Shutdown += WorkerOnShutdown;
        }

        protected override void RemoveEventHandlers()
        {
            Worker.Startup -= WorkerOnStartup;
            Worker.Update -= WorkerOnUpdate;
            Worker.Error -= WorkerOnError;
            Worker.Shutdown -= WorkerOnShutdown;
        }

        private void WorkerOnStartup(StateChange changeInfo)
        {
            NeedToRepaintDisplays = true;
        }

        private void WorkerOnUpdate(StateChange changeInfo)
        {
            NeedToRepaintDisplays = true;
        }

        private void WorkerOnError(StateChange changeInfo)
        {
            NeedToRepaintDisplays = true;
        }

        private void WorkerOnShutdown(StateChange changeInfo)
        {
            NeedToRepaintDisplays = true;
        }
    }
}
