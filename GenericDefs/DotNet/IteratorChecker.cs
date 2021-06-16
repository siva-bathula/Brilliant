using System;
using System.Collections.Generic;
using System.Threading;

namespace GenericDefs.DotNet
{
    /// <summary>
    /// Self-publishing iteration checker. Requires IterationStat.
    /// </summary>
    public sealed class IterationChecker
    {
        private static volatile IterationChecker instance;
        private static object syncRoot = new object();

        private IterationChecker() { }

        public static IterationChecker Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) instance = new IterationChecker();
                    }
                }
                if (_currentState == null)
                {
                    lock (syncRoot)
                    {
                        if (_currentState == null) _currentState = new Dictionary<string, IterationStat>();
                    }
                }
                return instance;
            }
        }

        static Dictionary<string, IterationStat> _currentState = null;

        /// <summary>
        /// Returns a token for the stat that is being cached.
        /// </summary>
        /// <param name="stat"></param>
        /// <returns></returns>
        public string Create(IterationStat stat)
        {
            string guid = Guid.NewGuid().ToString();
            lock (syncRoot)
            {
                _currentState.Add(guid, stat);
            }
            return guid;
        }

        public bool Update(string guidKey, IterationStat stat)
        {
            if (!_currentState.ContainsKey(guidKey)) throw new ArgumentException("Given key was not found in dictionary.");
            IterationStat _cached = _currentState[guidKey];
            lock (syncRoot)
            {
                _cached.Update(stat.Current);
            }
            return true;
        }

        public IterationStat GetStat(string guidKey)
        {
            if (!_currentState.ContainsKey(guidKey)) throw new ArgumentException("Given key was not found in dictionary.");
            return _currentState[guidKey];
        }
    }

    /// <summary>
    /// Self-publishing iteration stat.
    /// </summary>
    public class IterationStat
    {
        public long Current { get; set; }
        public long Total { get; set; }
        public double Percentage { get; set; }
        public void Update(long n)
        {
            Current = n;
            Percentage = Current * 100.0 / Total;
        }

        private StatusUpdater _s;
        public IterationStat(int publishSec)
        {
            Func<StatusUpdater, StatusUpdater> supdater = delegate (StatusUpdater su)
            {
                string message = "Total numbers done checking " + Current;
                message += Environment.NewLine;
                message += "Percentage completed : " + (Current * 100.0 / Total).ToString("0.00");
                su.SetMessage(message);
                return su;
            };
            _s = new StatusUpdater(1000 * publishSec, supdater);
        }

        public void Start()
        {
            _s.Start();
        }

        public void Stop()
        {
            _s.PublishLastAndStop();
            _s = null;
        }
    }
}