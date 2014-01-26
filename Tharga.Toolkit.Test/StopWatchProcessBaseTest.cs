using NUnit.Framework;

namespace Tharga.Toolkit.Test
{
    [TestFixture]
    public class StopWatchProcessBaseTest
    {
        private class MyStopWatch : StopWatchProcessBase
        {
            public MyStopWatch(string processName, string instanceName) 
                : base(processName, instanceName)
            {
            }
        }

        [Test]
        public void StartStepComplete()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const string process = "A";     //Name of the process to measure
            const string instance = "B";    //The instance of the measurement (Same things can be mearured again and again)
            var msw = new MyStopWatch(process, instance);    //Create and start
            

            //------------------------------------------
            // Act
            //------------------------------------------
            msw.Step("A");  //First step
            msw.Step("B");  //Second step
            msw.Complete();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(msw.ProcessList.Count == 3, "There are not three entries in the process list");
        }

        [Test]
        public void TwoParallelWatches()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const string process = "A";     //Name of the process to measure
            const string instance1 = "B1";
            const string instance2 = "B2";
            var msw1 = new MyStopWatch(process, instance1);
            var msw2 = new MyStopWatch(process, instance2);

            //------------------------------------------
            // Act
            //------------------------------------------
            msw1.Step("A");  //First step
            msw1.Step("B");  //Second step
            msw2.Step("A");  //First step
            msw2.Step("B");  //Second step
            msw2.Complete();
            msw1.Complete();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(msw1.ProcessList.Count == 3, "There are not three entries in the process list");
            Assert.IsTrue(msw2.ProcessList.Count == 3, "There are not three entries in the process list");
        }
    }
}
