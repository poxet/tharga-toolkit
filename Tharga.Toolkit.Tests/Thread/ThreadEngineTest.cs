//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Tharga.Toolkit.Thread;

//namespace Tharga.Toolkit.Test.Thread
//{
//    public class ThreadEngineBuilder : TestDataBuilder<ThreadEngine>
//    {
//        private readonly bool _runOnce;
//        private readonly int _delayMilliSeconds;

//        public ThreadEngineBuilder(bool runOnce, int delayMilliSeconds)
//        {
//            _runOnce = runOnce;
//            _delayMilliSeconds = delayMilliSeconds;
//        }

//        protected override ThreadEngine OnBuild()
//        {
//            return ThreadEngine.Create(Do, _runOnce, false);
//        }

//        public void Do()
//        {
//            System.Threading.Thread.Sleep(_delayMilliSeconds);
//        }
//    }

//    public class ThreadEngineBuilder<T> : TestDataBuilder<ThreadEngine<T>>
//    {
//        private readonly bool _runOnce;
//        private readonly int _delayMilliSeconds;
//        private readonly T _data;

//        public ThreadEngineBuilder(bool runOnce, int delayMilliSeconds, T data)
//        {
//            _runOnce = runOnce;
//            _delayMilliSeconds = delayMilliSeconds;
//            _data = data;
//        }

//        protected override ThreadEngine<T> OnBuild()
//        {
//            return ThreadEngine<T>.Create(Do, _data, _runOnce, false);
//        }

//        public void Do(T data)
//        {
//            System.Threading.Thread.Sleep(_delayMilliSeconds);            
//        }
//    }

//    [TestClass]
//    public class ThreadEngineTest
//    {
//        #region Basic
        

//        [TestMethod]
//        public void RunSomeTimesWaitToClose()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            const int count = 5;
//            var mte = new ThreadEngineBuilder(false, 10).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();    //Run first time
//            for (var i = 0; i < count - 1; i++)
//                mte.RunAgain(); //Run more times
//            Assert.IsTrue(mte.LapRequestCounter != mte.LapCounter, "Ran all laps requested");
//            mte.WaitClose(true);

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.LapCounter == count, "Did not run all times");
//            Assert.IsTrue(mte.LapRequestCounter == mte.LapCounter, "Did not run all laps requested");
//        }

//        [TestMethod]
//        public void RunSomeTimesTerminate()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            const int count = 5;
//            var mte = new ThreadEngineBuilder(false,10).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();    //Run first time
//            for (int i = 0; i < count-1; i++)
//                mte.RunAgain(); //Run more times
//            Assert.IsTrue(mte.LapRequestCounter != mte.LapCounter, "Ran all laps requested");
//            mte.WaitClose(false);

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.LapCounter != count, "Ran all times");            
//        }

//        [TestMethod]
//        public void RunAgainWithoutStartNotAllowed()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new ThreadEngineBuilder(false, 0).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            try
//            {
//                mte.RunAgain();
//                Assert.Fail("Run again when not started was allowed");
//            }
//            catch (InvalidCommandStateException)
//            {
//            }

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");
//        }

//        [TestMethod]
//        public void RunOnce()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new ThreadEngineBuilder(true, 0).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();
//            mte.WaitClose();

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.LapCounter == 1, "Did not run once");
//        }

//        [TestMethod]
//        public void RunOnceCloseNotAllowed()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new ThreadEngineBuilder(true, 0).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();

//            try
//            {
//                mte.BeguinClose();
//            }
//            catch (NotSupportedException exp)
//            {
//                //NOTE: Change to a specific exception
//                if (exp.Message != "This thread is set to run once and closes automatically when done.")
//                    throw;
//            }

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//        }


//        #endregion
//        #region Generic


//        //NOTE: Implement theese tests

//        [TestMethod]
//        public void RunSomeTimesWaitToCloseCheckResult()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            const int count = 5;
//            var mte = new ThreadEngineBuilder<string>(false, 10, "qwerty").Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();    //Run first time
//            for (var i = 0; i < count - 1; i++)
//                mte.RunAgain(); //Run more times
//            Assert.IsTrue(mte.LapRequestCounter != mte.LapCounter, "Ran all laps requested");
//            mte.WaitClose(true);

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.LapCounter == count, "Did not run all times");
//            Assert.IsTrue(mte.LapRequestCounter == mte.LapCounter, "Did not run all laps requested");
//        }


//        #endregion
//    }
//}
