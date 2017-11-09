using System.Threading;
using System.Threading.Tasks;

namespace AppLayer
{
    public delegate void UpdateHandler(int number);

    public class Player
    {
        private int _counter;
        private bool _keepingGoing;
        public string Name { get; set; }
        public UpdateHandler Handler { get; set; }

        public void Start()
        {
            _counter = 0;
            _keepingGoing = true;

            Task.Factory.StartNew(Run);         // Start a new thread that run the Run method
        }

        public void Stop()
        {
            _keepingGoing = false;
        }

        private void Run()
        {
            while (_keepingGoing)
            {
                Thread.Sleep(1000);
                _counter++;

                Handler?.Invoke(_counter);
            }
        }
    }
}
