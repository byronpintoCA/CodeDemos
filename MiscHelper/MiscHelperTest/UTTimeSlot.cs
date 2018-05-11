using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using TimeSlotSelector;

namespace MiscHelperTest
{
    [TestClass]
    public class UTTimeSlot
    {
        DateTime START_DATE = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 6, 30, 0);
        private const int LENGTH2D = 780;
        private const int slotSize = 30000;

        [TestMethod]
        public void TestSeek()
        {
            Dictionary<int, Tuple<DateTime, int, List<TestObject>>> masterList = new Dictionary<int, Tuple<DateTime, int, List<TestObject>>>();
            masterList.Add(0, CreateListOfObjs());
            masterList.Add(1, CreateListOfObjs());
            var daArray = TimeSlot<TestObject>.ToArray(masterList, LENGTH2D, (t) => { return t.daDate; });

            var locationRangeStart = (START_DATE.AddMilliseconds(slotSize * 5) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0],locationRangeStart, slotSize, out TestObject found);
            var foundRangeStart = (found.daDate - START_DATE).TotalMilliseconds;
            var foundRangeEnd = foundRangeStart + slotSize;
            Assert.IsTrue(locationRangeStart >= foundRangeStart && locationRangeStart < foundRangeEnd );
            
            locationRangeStart = (START_DATE.AddMilliseconds((slotSize * 5) + 15) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0], locationRangeStart, slotSize, out found);
            foundRangeStart = (found.daDate - START_DATE).TotalMilliseconds;
            foundRangeEnd = foundRangeStart + slotSize;
            Assert.IsTrue(locationRangeStart >= foundRangeStart && locationRangeStart < foundRangeEnd);

            locationRangeStart = (START_DATE.AddMilliseconds((slotSize * 5) - 15) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0],locationRangeStart, slotSize, out found);
            foundRangeStart = (found.daDate - START_DATE).TotalMilliseconds;
            foundRangeEnd = foundRangeStart + slotSize;
            Assert.IsTrue(locationRangeStart >= foundRangeStart && locationRangeStart < foundRangeEnd);

            locationRangeStart = (START_DATE.AddMilliseconds((slotSize * 779)) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0], locationRangeStart, slotSize, out found);

            foundRangeStart = (found.daDate - START_DATE).TotalMilliseconds;
            foundRangeEnd = foundRangeStart + slotSize;
            Assert.IsTrue(locationRangeStart >= foundRangeStart && locationRangeStart < foundRangeEnd);

            
        }

        [TestMethod]
        public void OutOfRange()
        {
            Dictionary<int, Tuple<DateTime, int, List<TestObject>>> masterList = new Dictionary<int, Tuple<DateTime, int, List<TestObject>>>();
            masterList.Add(0, CreateListOfObjs());
            var daArray = TimeSlot<TestObject>.ToArray(masterList, LENGTH2D, (t) => { return t.daDate; });

            var locationRangeStart = (START_DATE.AddMilliseconds(-125) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0], locationRangeStart, slotSize, out TestObject found);
            Assert.IsNull(found);

            locationRangeStart = (START_DATE.AddMilliseconds((slotSize * 780)) - START_DATE).TotalMilliseconds;
            TimeSlot<TestObject>.GetTimeSlot(daArray[0], locationRangeStart, slotSize, out found);
            Assert.IsNull(found);

        }



        private Tuple<DateTime, int, List<TestObject>> CreateListOfObjs()
        {

            List<TestObject> listObjs = new List<TestObject>();
            DateTime start = START_DATE;

            for (int i = 0; i < LENGTH2D; i++)
            {
                listObjs.Add(new TestObject(start));
                start = start.AddSeconds(30);
            }

            Tuple<DateTime, int, List<TestObject>> retTup = new Tuple<DateTime, int, List<TestObject>>(START_DATE, 30 * 1000, listObjs);

            return retTup;
        }

        private class TestObject
        {
            public TestObject(DateTime start)
            {
                this.daDate = start;
            }

            public DateTime daDate { get; set; }

        }
    }
}
