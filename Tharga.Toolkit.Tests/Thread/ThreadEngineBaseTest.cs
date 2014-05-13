//using System;
//using System.Threading;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Tharga.Toolkit.Thread;

//namespace Tharga.Toolkit.Test.Thread
//{
//    public abstract class TestDataBuilder<T>
//    {
//        protected abstract T OnBuild();

//        public T Build()
//        {
//            return OnBuild();
//        }
//    }

//    public class MyThreadEngineBuilder : TestDataBuilder<MyThreadEngine>
//    {
//        private readonly bool _runOnce;
//        private readonly int _delay;

//        public MyThreadEngineBuilder(bool runOnce, int delay = 0)
//        {
//            _runOnce = runOnce;
//            _delay = delay;
//        }

//        protected override MyThreadEngine OnBuild()
//        {
//            return new MyThreadEngine(Guid.NewGuid().ToString(), true, ThreadPriority.Lowest, _runOnce, _delay);
//        }
//    }

//    public class MyThreadEngine : ThreadEngineBase
//    {
//        public int MyLapCounter { get; private set; }

//        private readonly int _delay;

//        public MyThreadEngine(string threadName, bool isBackground, ThreadPriority priority, bool runOnce, int delay)
//            : base(threadName, isBackground, priority, runOnce)
//        {
//            MyLapCounter = 0;
//            _delay = delay;
//        }

//        public override void DoAction()
//        {
//            //NOTE: Try without sleep and try with a longer sleep
//            System.Threading.Thread.Sleep(_delay);
//            MyLapCounter++;
//        }
//    }

//    [TestClass]
//    public class ThreadEngineBaseTest
//    {
//        [TestMethod]
//        public void RunSomeTimesWaitToClose()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            const int count = 10;
//            var mte = new MyThreadEngineBuilder(false).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();
//            for (var i = 0; i < count - 1; i++)
//                mte.RunAgain();
//            mte.BeguinClose();
//            System.Threading.Thread.Sleep(10);
//            mte.WaitClose();      //Wait for it to close            

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.LapRequestCounter == count, "Not all laps where requested");
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.MyLapCounter == count, string.Format("Did not run {0} (requested {1}) times, it ran {2} times.", count, mte.LapRequestCounter, mte.MyLapCounter));
//            Assert.IsTrue(mte.LapRequestCounter == mte.LapCounter, "Did not run all laps requested");
//        }

//        [TestMethod]
//        public void RunAgainWhenCloseRequestedNotAllowed()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new MyThreadEngineBuilder(false).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();          //Run first time
//            mte.BeguinClose();    //Tell the process to close
//            try
//            {
//                mte.RunAgain();       //Run again after close not allowed
//                Assert.Fail("Run again after a close request was allowed");
//            }
//            catch (InvalidCommandStateException)
//            {
//            }
//            mte.WaitClose();      //Wait for it to close            

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.LapRequestCounter == 1, "Not one lap requested");
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.MyLapCounter == 1, string.Format("Did not run one time."));
//            Assert.IsTrue(mte.LapRequestCounter == mte.LapCounter, "Did not run all laps requested");
//        }

//        [TestMethod]
//        public void RunSomeTimesTerminate()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            const int count = 10;
//            var mte = new MyThreadEngineBuilder(false,50).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();
//            for (int i = 0; i < count - 1; i++ )
//                mte.RunAgain();
//            mte.WaitClose(false);

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.Closed, "State is not closed.");
//            Assert.IsTrue(mte.MyLapCounter != count, "Ran all times");
//        }

//        [TestMethod]
//        public void RunAgainWithoutStartNotAllowed()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new MyThreadEngineBuilder(false).Build();
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
//            var mte = new MyThreadEngineBuilder(true).Build();
//            Assert.IsTrue(mte.EngineState == ThreadEngineBase.ThreadEngineState.NeverStarted, "State is not never started.");

//            //------------------------------------------
//            // Act
//            //------------------------------------------
//            mte.Start();
//            mte.WaitClose();

//            //------------------------------------------
//            // Assert
//            //------------------------------------------
//            Assert.IsTrue(mte.MyLapCounter == 1, "Did not run once");
//        }

//        [TestMethod]
//        public void RunOnceCloseNotAllowed()
//        {
//            //------------------------------------------
//            // Arrange
//            //------------------------------------------
//            var mte = new MyThreadEngineBuilder(true).Build();
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

//        //NOTE: Start two threads with the same name is not allowed
//    }
//}
