using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1_Adv
{
    public class Helper<T>
    {
        public static void sort(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)

            {
                bool swapped = false;
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (Comparer<T>.Default.Compare(arr[j], arr[j + 1]) > 0)
                    {
                        T temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                        swapped = true;
                    }
                }
                if (!swapped)
                {
                    break;

                }
            }
        }

    }
}
