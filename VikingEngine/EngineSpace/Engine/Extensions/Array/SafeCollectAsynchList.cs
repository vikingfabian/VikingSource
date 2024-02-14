using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// Made to be used by multiple threads, while one update the list and the other use it
    /// </summary>
    class SafeCollectAsynchList<T>
    {
        /*
         * ## Expected layout ##
         * SafeCollectAsynchList<T> mylist;
         * 
         * void update()
         * {
         *      mylist.checkForUpdatedList();
         *      //use mylist.list
         * }
         * 
         * void asynchUpdate()
         * {
         *   if (mylist.ReadyForAsynchProcessing())
         *   {
         *      mylist.processList.Clear();
         *      //mylist.processList.Add(...
         *      mylist.onAsynchProcessComplete();
         *   }
         * }
         */
        
        public List<T> list;
        public List<T> processList;

        public SafeCollectAsynchList()
            : this(0)
        { }

        public SafeCollectAsynchList(int size)
        {
            list = new List<T>(size);
            processList = new List<T>(size);
        }

        bool readyForSwap = false;
        public void checkForUpdatedList()
        {
            if (readyForSwap)
            {
                lib.SwitchPointers(ref list, ref processList);
                readyForSwap = false;
            }
        }

        public bool ReadyForAsynchProcessing()
        {
            return !readyForSwap;
        }

        public void onAsynchProcessComplete()
        {
            readyForSwap = true;
        }
    }

    class SafeCollectAsynchStaticList<T>
    {
        public StaticList<T> list;
        public StaticList<T> processList;

        public T sel;
        int selIndex;
        public SafeCollectAsynchStaticList(int size)
        {
            list = new StaticList<T>(size);
            processList = new StaticList<T>(size);
        }

        public void loopBegin()
        {
            selIndex = 0;
            sel = default(T);
        }

        public bool loopNext()
        {
            ++selIndex;

            if (selIndex < list.Array.Length)
            {
                sel = list.Array[selIndex];
                return sel != null;
            }

            return false;
        }

        public void swap()
        {
            lib.SwitchPointers(ref list, ref processList);
        }
    }
}
