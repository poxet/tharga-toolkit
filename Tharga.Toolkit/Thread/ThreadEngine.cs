//using System;
//using System.Threading;

//namespace Tharga.Toolkit.Thread
//{
//    public class ThreadEngine<T> : ThreadEngineBase
//    {
//        #region Members


//        private Action<T> _doActionMethod;
//        private T _data;


//        #endregion
//        #region Factory


//        public static ThreadEngine<T> Create(Action<T> doActionMethod, T data, bool runOnce = true, bool start = true)
//        {
//            var threadName = string.Format("{0}_{1}", doActionMethod.Method.ToString().Replace(" ", "_"), Guid.NewGuid());
//            const bool isBackground = true;
//            const ThreadPriority priority = ThreadPriority.Normal;

//            return Create(threadName, isBackground, priority, runOnce, start, doActionMethod, data);
//        }

//        public static ThreadEngine<T> Create(string threadName, bool isBackground, ThreadPriority priority, bool runOnce,
//                                             bool start, Action<T> doActionMethod, T data)
//        {
//            var threadEngine = new ThreadEngine<T>(threadName, isBackground, priority, runOnce) { _doActionMethod = doActionMethod, _data = data };
//            if (start)
//                threadEngine.Start();
//            return threadEngine;
//        }


//        #endregion
//        #region Constructors


//        private ThreadEngine(string threadName, bool isBackground, ThreadPriority priority, bool runOnce)
//            : base(threadName, isBackground, priority, runOnce)
//        {

//        }


//        #endregion
//        #region Engine


//        public override void DoAction()
//        {
//            _doActionMethod.Invoke(_data);
//        }


//        #endregion
//    }

//    public class ThreadEngine : ThreadEngineBase
//    {
//        #region Members


//        private Action _doActionMethod;


//        #endregion
//        #region Factory


//        public static ThreadEngine Create(Action doActionMethod, bool runOnce = true, bool start = true)
//        {
//            var threadName = string.Format("{0}_{1}", doActionMethod.Method.ToString().Replace(" ", "_"), Guid.NewGuid());
//            const bool isBackground = true;
//            const ThreadPriority priority = ThreadPriority.Normal;

//            return Create(threadName, isBackground, priority, runOnce, start, doActionMethod);
//        }

//        public static ThreadEngine Create(string threadName, bool isBackground, ThreadPriority priority, bool runOnce,
//            bool start, Action doActionMethod)
//        {
//            var threadEngine = new ThreadEngine(threadName, isBackground, priority, runOnce) { _doActionMethod = doActionMethod };
//            if (start)
//                threadEngine.Start();
//            return threadEngine;
//        }


//        #endregion
//        #region Constructors


//        protected ThreadEngine(string threadName, bool isBackground, ThreadPriority priority, bool runOnce)
//            : base(threadName, isBackground, priority, runOnce)
//        {

//        }


//        #endregion
//        #region Engine


//        public override void DoAction()
//        {
//            _doActionMethod.Invoke();
//        }


//        #endregion
//    }
//}