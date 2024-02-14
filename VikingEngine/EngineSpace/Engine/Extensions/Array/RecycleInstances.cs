using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    class RecycleInstances<T> where T : class
    {
        StaticList<T> bin;

        public RecycleInstances(int maxLength)
        {
            bin = new StaticList<T>(maxLength);
        }

        /// <summary>
        /// Add an object that is deleted
        /// </summary>
        /// <returns>recycle bin is full</returns>
        public bool PushToBin(T obj)
        {
            if (bin.IsFull)
                return true;
            bin.Add(obj);

            return false;
        }

        public T PullFromBin()
        {
            if (bin.Count <= 0)
            {
                return null;
            }
            return bin.PullLast();
        }

        public int Count
        {
            get { return bin.Count; }
        }
    }
}
