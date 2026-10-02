using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment1_Adv
{
    internal class FixedSizeList<T>
    {

        private T[] items;
        private int count;
        public FixedSizeList(int size)
        {
            items = new T[size];
            count = 0;
        }
        public void Add(T item)
        {
            if (count < items.Length)
            {
                items[count] = item;
                count++;
            }
            else
            {
                throw new InvalidOperationException("List is full.");
            }
        }
        public T Get(int index)
        {
            if (index >= 0 && index < count)
            {
                return items[index];
            }
            else
            {
                throw new IndexOutOfRangeException("Index is out of range.");
            }
        }
        public int Count
        {
            get { return count; }
        }
    }
}
