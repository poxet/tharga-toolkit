using System;
using System.Runtime.InteropServices;

namespace Tharga.Toolkit.ServerStorage.Utility
{
    class StopWatch
    {
        #region Imports


        [DllImport("kernel32.dll")]
        extern static int QueryPerformanceCounter(ref long x);

        [DllImport("kernel32.dll")]
        extern static int QueryPerformanceFrequency(ref long x);


        #endregion
        #region Members


        private long? _startTime;
        private long _elapsed;
        private long? _frequency;


        #endregion
        #region Properties


        public bool IsRunning { get; private set; }


        #endregion
        #region Constructors


        public StopWatch(bool start = true)
        {
            Init(start);
        }


        #endregion
        #region Public functions


        /// <summary>
        /// Resets this instance.
        /// </summary>
        /// <returns>Time elapsed since start.</returns>
        public TimeSpan Reset()
        {
            var elapsed = Elapsed;

            _startTime = IsRunning ? (long?)GetValue() : null;
            _elapsed = 0;

            return elapsed;
        }

        public void Start()
        {
            _startTime = GetValue();
            IsRunning = true;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>Time elapsed since start.</returns>
        public TimeSpan Stop()
        {
            if (_startTime == null) throw new InvalidOperationException("The stop watch have never been started.");

            _elapsed += Peek();
            IsRunning = false;

            return Elapsed;
        }

        public TimeSpan Elapsed { get { return new TimeSpan(IsRunning ? _elapsed + Peek() : _elapsed); } }


        #endregion
        #region Internal functions


        private void Init(bool start)
        {
            _frequency = GetFrequency();
            Reset();
            if (start)
                Start();
        }

        private long Peek()
        {
            if (_frequency == null) throw new InvalidOperationException("Frequency has not been set.");

            if (_startTime == null) throw new InvalidOperationException("The stop watch have never been started.");
            return 1000 * (long)(((GetValue() - _startTime.Value) / (double)_frequency) * 10000);
        }

        private static long GetValue()
        {
            long ret = 0;
            if (QueryPerformanceCounter(ref ret) == 0)
                throw new NotSupportedException("Error while querying the high-resolution performance counter.");
            return ret;
        }

        private static long GetFrequency()
        {
            long val = 0;
            if (QueryPerformanceFrequency(ref val) == 0)
                throw new NotSupportedException("Error while querying the performance counter frequency.");
            return val;
        }


        #endregion
    }
}