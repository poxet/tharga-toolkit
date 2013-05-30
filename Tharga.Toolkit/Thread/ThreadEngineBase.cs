//using System;
//using System.Diagnostics;
//using System.Text;
//using System.Threading;
//using ThreadState = System.Threading.ThreadState;

//namespace Tharga.Toolkit.Thread
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public abstract class ThreadEngineBase
//    {
//        #region Enumerators


//        /// <summary>
//        /// The state of the engine that runes the thread
//        /// </summary>
//        public enum ThreadEngineState
//        {
//            /// <summary>
//            /// Initial state of the thread engine
//            /// </summary>
//            NeverStarted,
//            /// <summary>
//            /// Operating state when the thread is working
//            /// </summary>
//            Running,
//            /// <summary>
//            /// State when the thread is waiting
//            /// </summary>
//            Waiting,
//            /// <summary>
//            /// The engine has terminated
//            /// </summary>
//            Closed
//        }

//        public enum ThreadCommandState
//        {
//            NeverStarted,
//            Started,
//            CloseRequested,
//            Closed
//        }


//        #endregion
//        #region Members


//        private static readonly object SyncRoot = new object();
//        private readonly bool _runOnce;
//        private readonly System.Threading.Thread _thread;
//        private readonly AutoResetEvent _threadEvent;
//        private readonly AutoResetEvent _threadWaitCloseEvent;
//        private readonly AutoResetEvent _threadWaitStartEvent = new AutoResetEvent(true);
//        private int _lapRequestCounter;
//        private int _lapCounter;
//        private bool _processLaps = true;
//        private int? _resetLapRequestCounter;
//        private ThreadEngineState _engineState = ThreadEngineState.NeverStarted;
//        private ThreadCommandState _commandState = ThreadCommandState.NeverStarted;


//        #endregion
//        #region Properties


//        public ThreadState ThreadState { get { return _thread.ThreadState; } }
//        public ThreadCommandState CommandState
//        {
//            get { return _commandState; }
//            private set
//            {
//                if (_commandState == ThreadCommandState.Closed) throw new InvalidOperationException(string.Format("CommandState is closed, state cannot be changed. (Trying to change to {0})", value));
//                if (_commandState == value) throw new InvalidOperationException(string.Format("CommandState is already set to {0}.", _commandState));
//                if (_commandState == ThreadCommandState.CloseRequested && value != ThreadCommandState.Closed) throw new InvalidOperationException(string.Format("CommandState is set to CloseRequested. The only valid state after that is Closed (Trying to change to {0})", value));
//                //if (value == ThreadCommandState.NeverStarted) throw new InvalidOperationException("CommandState cannot be set to NeverStarted.");

//                var previousState = _commandState;
//                _commandState = value;

//                InvokeCommandStateChangeEvent(new CommandStateChangeEventArgs(previousState, _commandState));
//            }
//        }
//        public ThreadEngineState EngineState
//        {
//            get { return _engineState; }
//            private set
//            {
//                if (_engineState == ThreadEngineState.Closed) throw new InvalidOperationException(string.Format("EngineState is closed, state cannot be changed. (Trying to change to {0})", value));
//                //if (_engineState == value) throw new InvalidOperationException(string.Format("EngineState is already set to {0}.", _engineState));
//                //if (_engineState == ThreadEngineState.NeverStarted) throw new InvalidOperationException("EngineState cannot be set to NeverStarted.");



//                if (_engineState != value)
//                {
//                    var previousState = _engineState;
//                    _engineState = value;

//                    InvokeEngineStateChangeEvent(new EngineStateChangeEventArgs(previousState, _engineState, LapCounter, LapRequestCounter));
//                }
//            }
//        }
//        private bool IsRunning { get { return _thread != null && _thread.IsAlive; } }
//        public int LapRequestCounter
//        {
//            get { return _lapRequestCounter; }
//            private set
//            {
//                if (_lapRequestCounter != value)
//                {
//                    _lapRequestCounter = value;
//                    InvokeLapStateChangeEvent(new LapStateChangeEventArgs(LapCounter, LapRequestCounter));
//                }
//            }
//        }
//        public int LapCounter
//        {
//            get { return _lapCounter; }
//            private set
//            {
//                if (_lapCounter != value)
//                {
//                    _lapCounter = value;
//                    if (_resetLapRequestCounter != null)
//                    {
//                        _lapRequestCounter = _resetLapRequestCounter.Value;
//                        _resetLapRequestCounter = null;
//                        if (_lapCounter > _lapRequestCounter)
//                            _lapRequestCounter = _lapCounter;
//                    }
//                    InvokeLapStateChangeEvent(new LapStateChangeEventArgs(LapCounter, LapRequestCounter));
//                }
//            }
//        }

//        #endregion
//        #region Event


//        public class EngineStateChangeEventArgs : EventArgs
//        {
//            public ThreadEngineState PreviousState { get; private set; }
//            public ThreadEngineState CurrentState { get; private set; }
//            public int LapCounter { get; private set; }
//            public int LapRequestCounter { get; private set; }

//            public EngineStateChangeEventArgs(ThreadEngineState previousState, ThreadEngineState currentState,
//                int lapCounter, int lapRequestCounter)
//            {
//                PreviousState = previousState;
//                CurrentState = currentState;
//                LapCounter = lapCounter;
//                LapRequestCounter = lapRequestCounter;
//            }
//        }

//        public class CommandStateChangeEventArgs : EventArgs
//        {
//            public ThreadCommandState PreviousState { get; private set; }
//            public ThreadCommandState CurrentState { get; private set; }

//            public CommandStateChangeEventArgs(ThreadCommandState previousState, ThreadCommandState currentState)
//            {
//                PreviousState = previousState;
//                CurrentState = currentState;
//            }
//        }

//        public class LapStateChangeEventArgs : EventArgs
//        {
//            public int LapCounter { get; private set; }
//            public int LapRequestCounter { get; private set; }

//            public LapStateChangeEventArgs(int lapCounter, int lapRequestCounter)
//            {
//                LapCounter = lapCounter;
//                LapRequestCounter = lapRequestCounter;
//            }
//        }

//        public event EventHandler<EngineStateChangeEventArgs> EngineStateChangeEvent;
//        public event EventHandler<CommandStateChangeEventArgs> CommandStateChangeEvent;
//        public event EventHandler<LapStateChangeEventArgs> LapStateChangeEvent;

//        private void InvokeEngineStateChangeEvent(EngineStateChangeEventArgs e)
//        {
//            var handler = EngineStateChangeEvent;
//            if (handler != null)
//                handler(this, e);
//        }

//        public void InvokeCommandStateChangeEvent(CommandStateChangeEventArgs e)
//        {
//            var handler = CommandStateChangeEvent;
//            if (handler != null)
//                handler(this, e);
//        }

//        public void InvokeLapStateChangeEvent(LapStateChangeEventArgs e)
//        {
//            var handler = LapStateChangeEvent;
//            if (handler != null)
//                handler(this, e);
//        }


//        #endregion
//        #region Constructors


//        protected ThreadEngineBase(string threadName, bool isBackground, ThreadPriority priority, bool runOnce)
//        {
//            //NOTE: Make sure that multiple threads does not get the same name
//            ////Precondition
//            //if (Process.GetCurrentProcess().Threads.Cast<Thread>().Any(thread => string.Compare(thread.Name, threadName) == 0))
//            //    throw new InvalidOperationException(string.Format("There is already a thread named {0}.", threadName));

//            EngineStateChangeEvent += ThreadEngineBase_EngineStateChangeEvent;

//            _thread = new System.Threading.Thread(Engine) { Name = threadName, IsBackground = isBackground, Priority = priority };
//            _runOnce = runOnce;
//            if (!_runOnce)
//                _threadEvent = new AutoResetEvent(true);
//            _threadWaitCloseEvent = new AutoResetEvent(true);
//        }

//        ~ThreadEngineBase()
//        {
//            if (_thread != null && _thread.IsAlive)
//            {
//                Trace.TraceWarning("Thread {0} was forced to abort by class destructor.", _thread.Name);
//                _thread.Abort();
//            }
//        }


//        #endregion
//        #region Engine


//        /// <summary>
//        /// Thread engine that run when the thread is alive.
//        /// </summary>
//        private void Engine()
//        {
//            if (CommandState != ThreadCommandState.Started) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not Started ({1}).", _thread.Name, CommandState));
//            if (EngineState != ThreadEngineState.NeverStarted) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not NeverStarted ({1}).", _thread.Name, EngineState));

//            try
//            {
//                while (CommandState == ThreadCommandState.Started)
//                {
//                    while (LapCounter < LapRequestCounter && _processLaps)
//                    {
//                        EngineState = ThreadEngineState.Running; //Engine starts to run
//                        DoAction();
//                        LapCounter++;
//                    }

//                    if (!_runOnce)
//                    {
//                        EngineState = ThreadEngineState.Waiting; //Engine is waiting for next run
//                        _threadEvent.WaitOne();
//                    }
//                    else
//                        CommandState = ThreadCommandState.CloseRequested;
//                }
//            }
//            finally
//            {
//                EngineState = ThreadEngineState.Closed;   //Engine has closed
//            }

//            //Postcondition
//            if (_processLaps)
//                if (LapCounter < LapRequestCounter)
//                    throw new InvalidOperationException(string.Format("Exiting the engine before all laps has been processeed. (Requested {0}, Ran {1})", LapRequestCounter, LapCounter));
//        }

//        public abstract void DoAction();


//        #endregion
//        #region Commands


//        /// <summary>
//        /// Starts the thread and enters the engine loop.
//        /// This method can only be run once.
//        /// </summary>
//        public void Start()
//        {
//            if (CommandState != ThreadCommandState.NeverStarted) throw new InvalidOperationException(string.Format("Thread {0} has already been started.", _thread.Name));
//            if (EngineState != ThreadEngineState.NeverStarted) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not NeverStarted ({1}).", _thread.Name, EngineState));

//            lock (SyncRoot) //Check-lock-check pattern
//            {
//                if (CommandState != ThreadCommandState.NeverStarted) throw new InvalidOperationException(string.Format("Thread {0} has already been started.", _thread.Name));

//                CommandState = ThreadCommandState.Started;
//                LapRequestCounter++;
//                _thread.Start();
//            }

//            //Wait for the engine to start
//            while (EngineState == ThreadEngineState.NeverStarted)
//                _threadWaitStartEvent.WaitOne(1000);
//        }

//        /// <summary>
//        /// Tells the engine to run again.
//        /// Cannot be called for threads that is set to run once.
//        /// </summary>
//        public void RunAgain(bool onlyRunIfWaiting = false)
//        {
//            if (_runOnce) throw new NotSupportedException(string.Format("Thread {0} is set to run once and cannot be run again.", _thread.Name));
//            if (CommandState != ThreadCommandState.Started) throw new InvalidCommandStateException(_thread.Name, ThreadCommandState.Started, CommandState);
//            lock (SyncRoot) //NOTE: Create a check state function that locks, checks and fires a message within the lock
//                if (EngineState != ThreadEngineState.Running && EngineState != ThreadEngineState.Waiting) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not Running or Waiting ({1}).", _thread.Name, EngineState));

//            if (!onlyRunIfWaiting || EngineState == ThreadEngineState.Waiting)
//            {
//                LapRequestCounter++;
//                _threadEvent.Set();
//            }
//        }

//        ////NOTE: Not fully tested. Enable this function if needed.
//        //public void ResetLapRequests()
//        //{
//        //    if (_runOnce) throw new NotSupportedException(string.Format("Thread {0} is set to run once and cannot be run again.", _thread.Name));
//        //    if (CommandState != ThreadCommandState.Started) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to Started. ({1})", _thread.Name, CommandState));
//        //    if (EngineState != ThreadEngineState.Running && EngineState != ThreadEngineState.Waiting) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not Running or Waiting ({1}).", _thread.Name, EngineState));

//        //    _resetLapRequestCounter = _lapCounter;
//        //}

//        /// <summary>
//        /// Tells the engine to start closing.
//        /// Does not interrupt the current action for the engine and does not wait for it to close.
//        /// </summary>
//        public void BeguinClose(bool processLaps = true)
//        {
//            if (_runOnce) throw new NotSupportedException("This thread is set to run once and closes automatically when done.");
//            if (CommandState != ThreadCommandState.Started) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to Started. ({1})", _thread.Name, CommandState));
//            if (EngineState != ThreadEngineState.Running && EngineState != ThreadEngineState.Waiting) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not Running or Waiting ({1}).", _thread.Name, EngineState));

//            lock (SyncRoot) //Check-lock-check pattern
//            {
//                if (CommandState != ThreadCommandState.Started) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to Started. ({1})", _thread.Name, CommandState));

//                _processLaps = processLaps;
//                CommandState = ThreadCommandState.CloseRequested;
//                _threadEvent.Set(); //Tell the thread to continue
//            }
//        }

//        /// <summary>
//        /// Wait for the engine to close.
//        /// The engine is told to close done (without interruption) if it has not yet been told to close (BeguinClose call).
//        /// </summary>
//        public void WaitClose(bool processLaps = true)
//        {
//            var data = GetCompleteState();
//            try
//            {
//                //Debug.WriteLine(data);

//                //bool engineAlreadyMarkedAsClosed = true;
//                bool waitedForCommandStateToClose = false;
//                bool waitedForThreadToClose = false;
//                bool sentBeguinCloseRequest = false;
//                //if (EngineState != ThreadEngineState.Closed) //Already closed, no need to wait
//                //{
//                //    engineAlreadyMarkedAsClosed = false;

//                if (!_runOnce)
//                {
//                    //if (CommandState != ThreadCommandState.Started && CommandState != ThreadCommandState.CloseRequested) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to Started or CloseRequested. ({1})", _thread.Name, CommandState));
//                    //if (EngineState != ThreadEngineState.Running && EngineState != ThreadEngineState.Waiting) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not Running or Waiting ({1}).", _thread.Name, EngineState));

//                    //If close have not been requested.
//                    if (CommandState == ThreadCommandState.Started)
//                    {
//                        sentBeguinCloseRequest = true;
//                        BeguinClose(processLaps);
//                    }
//                }

//                //Wait for the thread to close
//                while (IsRunning)
//                {
//                    waitedForThreadToClose = true;
//                    _threadWaitCloseEvent.WaitOne(1000);
//                }

//                while (CommandState != ThreadCommandState.Closed)
//                {
//                    waitedForCommandStateToClose = true;
//                    _threadWaitCloseEvent.WaitOne(1000);
//                }
//                //}

//                //Post condition
//                if (CommandState != ThreadCommandState.Closed) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to Closed. (current state: {1})", _thread.Name, CommandState));
//                if (EngineState != ThreadEngineState.Closed) throw new InvalidOperationException(string.Format("EngineState for thread {0} is not set to Closed. ({1})", _thread.Name, EngineState));
//                if (IsRunning) throw new InvalidOperationException(string.Format("Thread {0} is still running.", _thread.Name));
//                if (processLaps && LapRequestCounter != LapCounter) throw new InvalidOperationException(string.Format("Did not run {0} times as requested, it ran {1} times", LapRequestCounter, LapCounter));
//            }
//            catch (Exception exp)
//            {
//                Console.WriteLine(data);
//                exp.Data.Add("State", data);
//                throw;
//            }
//            //}
//        }

//        private string GetCompleteState()
//        {
//            var data = new StringBuilder();
//            lock (SyncRoot)
//            {
//                data.AppendFormat("CommandState: {0}{1}", CommandState, System.Environment.NewLine);
//                data.AppendFormat("EngineState: {0}{1}", EngineState, System.Environment.NewLine);
//                data.AppendFormat("IsRunning: {0}{1}", IsRunning, System.Environment.NewLine);
//                data.AppendFormat("Lap {0} of {1}: {2}", LapCounter, LapRequestCounter, System.Environment.NewLine);
//            }
//            return data.ToString();
//        }


//        #endregion
//        #region Event handlers


//        private void ThreadEngineBase_EngineStateChangeEvent(object sender, EngineStateChangeEventArgs e)
//        {
//            //The engine changed state to Closed
//            if (e.CurrentState == ThreadEngineState.Closed)
//            {
//                if (CommandState != ThreadCommandState.CloseRequested) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to CloseRequested. ({1})", _thread.Name, CommandState));
//                lock (SyncRoot) //Check-lock-check pattern
//                {
//                    if (CommandState != ThreadCommandState.CloseRequested) throw new InvalidOperationException(string.Format("CommandState for thread {0} is not set to CloseRequested. ({1})", _thread.Name, CommandState));
//                    CommandState = ThreadCommandState.Closed;
//                    //NOTE: TRY THIS... if ( _threadWaitCloseEvent != null )
//                    _threadWaitCloseEvent.Set();
//                }
//            }

//            //The entine changed starte from NeverStarted
//            if (e.PreviousState == ThreadEngineState.NeverStarted)
//            {
//                _threadWaitStartEvent.Set();
//            }
//        }


//        #endregion
//    }

//    internal class InvalidCommandStateException : Exception
//    {
//        public InvalidCommandStateException(string threadName, ThreadEngineBase.ThreadCommandState expectedCommandState,
//                ThreadEngineBase.ThreadCommandState currentCommandState)
//            : base(string.Format("CommandState for thread {0} is not set to {1}. ({2})", threadName, expectedCommandState, currentCommandState))
//        {

//        }
//    }
//}
