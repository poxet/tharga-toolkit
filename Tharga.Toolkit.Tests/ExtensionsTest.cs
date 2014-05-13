using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NUnit.Framework;

namespace Tharga.Toolkit.Test
{
    [TestFixture]
    public class ExtensionsTest
    {
        [Test]
        public void TakeAllButLast()
        {
            // Arrange
            var strings = new List<string>{"A", "B", "C"};

            // Act
            var result = strings.TakeAllButLast().ToList();

            // Assert
            Assert.AreEqual(strings.Count - 1, result.Count);
            Assert.AreEqual(strings[0], result[0]);
            Assert.AreEqual(strings[1], result[1]);
        }

        [Test]
        public void TakeAllButLast_when_empty()
        {
            // Arrange
            var strings = new List<string>();

            // Act
            var result = strings.TakeAllButLast().ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void TakeAllButLast_when_contains_only_one()
        {
            // Arrange
            var strings = new List<string>{"A"};

            // Act
            var result = strings.TakeAllButLast().ToList();

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [Test]
        public void ToDateTimeString()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var dateTime = DateTime.Now;

            //------------------------------------------
            // Act
            //------------------------------------------
            var result = dateTime.ToDateTimeString();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(string.Format("{0} {1}", dateTime.ToShortDateString(), dateTime.ToLongTimeString()), result) == 0, "Parsed DateTime to 'Date Time String' is not right.");
        }

        [Test]
        public void ToTimeString()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var dateTime = DateTime.Now;

            //------------------------------------------
            // Act
            //------------------------------------------
            var diff = dateTime.AddSeconds(400) - dateTime;
            var result = diff.ToTimeString();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(string.Compare(string.Format("{0}:{1}:{2}", diff.Hours, diff.Minutes.ToString("00"), diff.Seconds.ToString("00")), result) == 0, "Parsed TimeSpan to 'Time String' is not right");
        }

        [Test]
        public void GetRandomItemFromList()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var list = new List<string> {"A", "B", "C"};

            //------------------------------------------
            // Act
            //------------------------------------------
            var item = list.TakeRandom();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(item),"The item is empty");
        }

        [Test]
        public void GetRandomItemFromEmptyList()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var list = new List<string> { };

            //------------------------------------------
            // Act
            //------------------------------------------
            var item = list.TakeRandom();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsNull(item, "The item is not null");
        }

        [Test]
        public void GetRandomItemFromListWithOneEntity()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var list = new List<string> { "A" };

            //------------------------------------------
            // Act
            //------------------------------------------
            var item = list.TakeRandom();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(item == "A", "The item is not A");
        }

        [Test]
        public void GetRandomItemFromCollection()
        {
            //------------------------------------------
            // Arrange
            //------------------------------------------
            var list = new Collection<string> { "A", "B", "C" };

            //------------------------------------------
            // Act
            //------------------------------------------
            var item = list.TakeRandom();

            //------------------------------------------
            // Assert
            //------------------------------------------
            Assert.IsTrue(!string.IsNullOrEmpty(item), "The item is empty");
        }
    }
}
