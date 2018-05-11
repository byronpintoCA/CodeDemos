using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimeSlotSelector
{

    public class TimeSlot<T>
    {
        public T daObj { get; set; }
        public long StartLocation { get; private set; }
        public long EndLocation { get; private set; }

        public TimeSlot(T obj,DateTime baseDateTime , int slotSizeInMilli, Func<T, DateTime> getDate)
        {
            daObj = obj;
            TimeSpan difference = getDate(obj) - baseDateTime;
            StartLocation = slotSizeInMilli * Convert.ToInt64(Math.Floor(difference.TotalMilliseconds / (double)slotSizeInMilli));
            EndLocation = StartLocation + slotSizeInMilli;
        }

        public static Dictionary<int , TimeSlot<T>[]> ToArray(Dictionary<int,Tuple<DateTime,int,List<T>>>  inputList, int length2D, Func<T, DateTime> getDate)
        {
            Dictionary<int, TimeSlot<T>[]> daArrayDict = new Dictionary<int, TimeSlot<T>[]>();
            
            foreach (var masterItem in inputList)
            {
                int childItemLocation = 0;
                TimeSlot<T>[] arr = new TimeSlot<T>[length2D];
                
                foreach (var childItem in masterItem.Value.Item3)
                {
                    arr[childItemLocation++] = new TimeSlot<T>(childItem,masterItem.Value.Item1 , masterItem.Value.Item2, getDate );
                }
                daArrayDict.Add(masterItem.Key, arr);
            }
            return daArrayDict;
        }

        public static bool GetTimeSlot(TimeSlot<T>[] source, double timeDiffInMilli, int slotSizeInMilli, out T theSlot)
        {
            bool found = false; theSlot = default(T);

            if (source != null && source.Length > 0)
            {
                int location = Convert.ToInt32(Math.Floor(timeDiffInMilli / (double)slotSizeInMilli));

                if (location < 0) return false;

                theSlot = location < source.Length ? source[location].daObj : default(T);

                found = true;
            }

            return found;
        }

    }
}
