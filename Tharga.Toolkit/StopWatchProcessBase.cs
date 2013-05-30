using System;
using System.Collections.Generic;

namespace Tharga.Toolkit
{
    public abstract class StopWatchProcessBase
    {
        #region Members


        private readonly StopWatch _stopWatch = new StopWatch(false);
        private TimeSpan _previousTimeSpan;
        private readonly List<KeyValuePair<string, TimeSpan>> _processList = new List<KeyValuePair<string, TimeSpan>>();


        #endregion
        #region Properties


        public string ProcessName { get; private set; }
        public string InstanceName { get; private set; }
        public DateTime StartTime { get; private set; }

        public TimeSpan TotalTime { get { return _stopWatch.Elapsed; } }
        public List<KeyValuePair<string, TimeSpan>> ProcessList { get { return _processList; } }


        #endregion
        #region Constructors

        
        protected StopWatchProcessBase(string processName, string instanceName)
        {
            //The constructor has to be as fast as possible so that the impact of the timing is as little as possible.
            ProcessName = processName;
            InstanceName = instanceName;
            StartTime = DateTime.Now;

            _stopWatch.Start();
        }


        #endregion
        #region Public functions


        public virtual void Step(string action)
        {
            if (!_stopWatch.IsRunning) throw new InvalidOperationException("The stop watch is no longer running. The process has completed.");
            var stepTime = _stopWatch.Elapsed;
            _processList.Add(new KeyValuePair<string, TimeSpan>(action, stepTime - _previousTimeSpan));
            _previousTimeSpan = stepTime;
        }

        public virtual void Complete()
        {
            if (!_stopWatch.IsRunning) throw new InvalidOperationException("The stop watch is no longer running. The process has already completed.");
            Step("Complete");
            _stopWatch.Stop();
        }

        protected void Stop()
        {
            _stopWatch.Stop();
        }

        protected void Reset()
        {
            if (!_stopWatch.IsRunning) throw new InvalidOperationException("The stop watch is no longer running. The process has already completed.");
            _stopWatch.Reset();
        }


        #endregion
    }
}
