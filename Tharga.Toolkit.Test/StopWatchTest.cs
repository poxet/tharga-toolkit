using NUnit.Framework;

namespace Tharga.Toolkit.Test
{
    [TestFixture]
    public class StopWatchTest
    {
        [Test]
        [Ignore]
        public void StartStop()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const int shortDelay = 10;
            const int longDelay = 200;
            var sw = new StopWatch(false);

            //------------------------------------------
            // Act
            //------------------------------------------
            System.Threading.Thread.Sleep(longDelay);
            sw.Start();
            System.Threading.Thread.Sleep(shortDelay*2);
            var e1 = sw.Stop();
            System.Threading.Thread.Sleep(longDelay);
            var elapsed = sw.Elapsed;

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsFalse(sw.IsRunning, "The stop watch is running");
            Assert.IsTrue(elapsed.TotalMilliseconds >= shortDelay, "Elapsed time is shorter than short delay.");
            Assert.IsTrue(elapsed.TotalMilliseconds < longDelay, "Elapsed time is longer than long delay.");
        }

        [Test]
        [Ignore]
        public void Reset()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const int shortDelay = 10;
            const int longDelay = 200;
            var sw = new StopWatch();

            //------------------------------------------
            // Act
            //------------------------------------------
            System.Threading.Thread.Sleep(longDelay);
            var e1 = sw.Reset();
            System.Threading.Thread.Sleep(shortDelay);
            var elapsed = sw.Elapsed;

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(sw.IsRunning, "The stop watch is not running");
            Assert.IsTrue(elapsed.TotalMilliseconds >= shortDelay, "Elapsed time is shorter than short delay.");
            Assert.IsTrue(elapsed.TotalMilliseconds < longDelay, "Elapsed time is longer than long delay.");
        }

        [Test]
        [Ignore]
        public void StartStopStart()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const int shortDelay = 10;
            const int longDelay = 200;
            var sw = new StopWatch();

            //------------------------------------------
            // Act
            //------------------------------------------
            System.Threading.Thread.Sleep(shortDelay*2);
            var e2 = sw.Stop();
            System.Threading.Thread.Sleep(longDelay);
            sw.Start();
            var e3 = sw.Stop();
            System.Threading.Thread.Sleep(longDelay);
            sw.Start();
            var elapsed = sw.Elapsed;

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(sw.IsRunning, "The stop watch is not running");
            Assert.IsTrue(elapsed.TotalMilliseconds >= shortDelay, string.Format("Elapsed time is shorter than short delay (delay {0}, elapsed {1}).", shortDelay, elapsed.TotalMilliseconds));
            Assert.IsTrue(elapsed.TotalMilliseconds < longDelay, "Elapsed time is longer than long delay.");
        }

        [Test]
        public void Elapsed()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            const int delay = 10;
            var sw = new StopWatch();

            //------------------------------------------
            // Act
            //------------------------------------------
            System.Threading.Thread.Sleep(delay*2);
            var elapsed = sw.Elapsed;

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(sw.IsRunning, "The stop watch is not running");
            Assert.IsTrue(elapsed.TotalMilliseconds >= delay, string.Format("Elapsed time was not at least the delay. (delay {0}, elapsed {1})", delay, elapsed.TotalMilliseconds));
        }
    }
}
