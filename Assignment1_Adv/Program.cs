using System;
using System.Collections;
using System.Collections.Generic;

namespace Assignment1_Adv
{
    internal class Program
    {
        private static void Reserve_array(ArrayList arrayList)
        {
            int left = 0;
            int right = arrayList.Count - 1;

            while (left < right)
            {
                object temp = arrayList[left];
                arrayList[left] = arrayList[right];
                arrayList[right] = temp;

                left++;
                right--;
            }
        }

        static int FirstNonRepeatedChar(string str)
        {
            Dictionary<char, int> frequency = new Dictionary<char, int>();

            
            foreach (char c in str)
            {
                if (frequency.ContainsKey(c))
                    frequency[c]++;
                else
                    frequency[c] = 1;
            }

            
            for (int i = 0; i < str.Length; i++)
            {
                if (frequency[str[i]] == 1)
                    return i;
            }

            return -1;
        }

        static void Main(string[] args)
        {
            int[] arr = { 64, 34, 25, 12, 22, 11, 90 };
            Helper<int>.sort(arr);

            foreach(var i in arr) { 
                Console.WriteLine(i);
            }
            Console.WriteLine("-------------------------------");

            Range<int> range = new Range<int>(10, 20);
            Console.WriteLine(range.IsInRange(15));
            Console.WriteLine( range.length());
            Console.WriteLine("-------------------------------");

            ArrayList arraylist = new ArrayList();
            arraylist.Add(1);
            arraylist.Add(2);
            arraylist.Add(3);
            arraylist.Add(4);
            arraylist.Add(5);

            Reserve_array(arraylist);

            foreach (var item in arraylist)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("-------------------------------");

            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            list.Add(5);

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] % 2 == 0 )
                Console.WriteLine(list[i]);
            }

            Console.WriteLine("-------------------------------");
            
            FixedSizeList<int> fixedList = new FixedSizeList<int>(3);
            fixedList.Add(10);
            fixedList.Add(20);
            fixedList.Add(30);

            Console.WriteLine("FixedSizeList contents:");
            for (int i = 0; i < fixedList.Count; i++)
            {
                Console.WriteLine(fixedList.Get(i));
            }

            Console.WriteLine("FixedSizeList Count: " + fixedList.Count);

            try
            {
                
                fixedList.Add(40);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Expected error when adding to full FixedSizeList: " + ex.Message);
            }

            Console.WriteLine("-------------------------------");


            string str = "sos";

            Console.WriteLine(FirstNonRepeatedChar(str));
        }
    }
}
