using System;
using System.Threading;

namespace ByronSouthParkDemo.Common
{
    public class TimeService
    {
        private const int _FREQUENCY = 1000;
        Thread _workThread ;
        private ManualResetEvent _mre;

        public event Action<DateTime> TimesUp;

        public void Start()
        {
            Stop();
            _workThread = new Thread(Work);
            _mre = new ManualResetEvent(false);
            _workThread.Start();
        }

        private void Work()
        {
            while (false == _mre.WaitOne(_FREQUENCY))
            {
                if (TimesUp != null)
                {
                    TimesUp(DateTime.Now);
                }
            }
        }

        public void Stop()
        {
            if (_mre != null)
            {
                _mre.Set();
                _workThread.Join(_FREQUENCY * 2);
                _mre = null;
                _workThread = null;
            }
        }
    }
}