using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1_Adv
{
    internal class Range<T> where T : IComparable<T>
    {
        private T Min;
        private T Max;
            public Range(T min, T max)
        {
            Min = min;
            Max = max;
        }

        public bool IsInRange(T value)
        {
            return value.CompareTo(Min) >= 0 && value.CompareTo(Max) <= 0;
        }

        public double length()
        
        { return Convert.ToDouble(Max) - Convert.ToDouble(Min); }




    }
}
