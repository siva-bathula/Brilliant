using System;
using System.Timers;

namespace GenericDefs.DotNet
{
    public class StatusUpdater
    {
        Timer _w = new Timer();
        DateTime _startTime;
        DateTime _lastChecked;
        Func<StatusUpdater, StatusUpdater> _statusChecker;
        public StatusUpdater(int intervalMiSec, Func<StatusUpdater, StatusUpdater> statusChecker)
        {
            _w.Interval = intervalMiSec;
            _w.Enabled = false;
            _w.Elapsed += _w_Elapsed;
            _statusChecker = statusChecker;
        }

        internal string Message { set; get; }
        public void SetMessage(string message)
        {
            message += Environment.NewLine;
            message += "Time elapsed : " + (_lastChecked - _startTime).TotalSeconds;
            Message = message;
        }

        private void _w_Elapsed(object sender, ElapsedEventArgs e)
        {
            _lastChecked = DateTime.Now;
            _statusChecker.Invoke(this);
            QueuedConsole.WriteStatusMessage(Message);
        }

        public void Start()
        {
            _w.Start();
            _startTime = DateTime.Now;
        }

        public void PublishLastAndStop()
        {
            QueuedConsole.WriteStatusMessage("Closing status updater....");
            _statusChecker = null;
            _w.Elapsed -= _w_Elapsed;
            _w.Dispose();
            _w = null;
            QueuedConsole.WriteStatusMessage(Message);
        }

        public void StopAndDispose()
        {
            _statusChecker = null;
            _w.Elapsed -= _w_Elapsed;
            _w.Dispose();
            _w = null;
        }
    }
}