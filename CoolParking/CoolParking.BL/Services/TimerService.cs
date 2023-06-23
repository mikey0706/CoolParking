// TODO: implement class TimerService from the ITimerService interface.
//       Service have to be just wrapper on System Timers.
using CoolParking.BL.Interfaces;
using System.Timers;

namespace CoolParking.BL.Services
{
    public class TimerService : ITimerService
    {
        private Timer _timer;
        public double Interval { get; set; }

        public event ElapsedEventHandler Elapsed;

        public void Dispose()
        {
            _timer.Dispose();
        }

        public void Start()
        {
            _timer = new Timer();
            _timer.Elapsed += Elapsed;
            _timer.AutoReset = true;
            _timer.Enabled = true;
            _timer.Start();
        }

        public void Stop()
        {
            if (_timer != null)
            {
                _timer.Stop();
            }
        }
    }
}