using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// Array that keeps track of which member is selected
    /// </summary>
    class ArrayWithSelection<T>
    {
        public int selectedIndex = 0;
        public T[] array;

        public ArrayWithSelection()
        {
            array = null;
        }
        public ArrayWithSelection(int count)
        {
            array = new T[count];
        }
        public ArrayWithSelection(T[] array)
        {
            this.array = array;
        }

        public void AddAfterSeleted(T newMember, bool selectNewMember)
        {
            if (Set(++selectedIndex, newMember) == false)
            {
                throw new IndexOutOfRangeException("ArrayWithSelection: AddAfterSeleted " + selectedIndex.ToString());
            }

            //if (list.Length == 0)
            //{
            //    list.Add(newMember);
            //    selectedIndex = 0;
            //}
            //else
            //{
            //    list.Insert(selectedIndex + 1, newMember);
            //    if (selectNewMember)
            //    {
            //        ++selectedIndex;
            //    }
            //}
        }

        public bool inRange(int index)
        {
            return index >= 0 && index < array.Length;
        }
        public bool Set(int index, T value)
        {
            if (index >= 0 && index < array.Length)
            {
                array[index] = value;
                return true;
            }
            return false;
        }



        //public void Add(T newMember, bool selectNewMember)
        //{
        //    list.Add(newMember);
        //    if (selectNewMember)
        //        selectedIndex = list.Length - 1;
        //}

        public T Selected()
        {
            if (selectedIndex >= 0 && selectedIndex < array.Length)
                return array[selectedIndex];
            return default(T);
        }

        /// <summary>
        /// Moves the selection forward one step
        /// </summary>
        /// <returns>Reached end</returns>
        public bool Next(bool rollover)
        {
            ++selectedIndex;
            if (selectedIndex >= array.Length)
            {
                if (rollover)
                    selectedIndex = 0;
                else
                    selectedIndex = array.Length - 1;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Moves the selection backward one step
        /// </summary>
        public void Previous(bool rollover)
        {
            --selectedIndex;
            if (selectedIndex < 0)
            {
                if (rollover)
                    selectedIndex = array.Length - 1;
                else
                    selectedIndex = 0;
            }
        }

        ///// <summary>
        ///// Move the selected member in the list
        ///// </summary>
        //public void Move(int dir)
        //{
        //    T member = Selected();
        //    list.RemoveAt(selectedIndex);
        //    int placement = Bound.SetBounds(selectedIndex + dir, 0, list.Length);
        //    list.Insert(placement, member);
        //    selectedIndex = placement;
        //}

        /// <summary>
        /// Removes the selected member
        /// </summary>
        //public void Remove()
        //{
        //    list.RemoveAt(selectedIndex);
        //    selectedIndex = Bound.SetBounds(selectedIndex, 0, list.Length - 1);
        //}

        public int GetIndex(T member)
        {
            for (int i = 0; i < array.Length; ++i)
            {
                if (array[i].Equals(member))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool HasSelection
        { get { return selectedIndex >= 0 && selectedIndex < array.Length; } }

        public void Unselect()
        {
            selectedIndex = -1;
        }

        public void SelectRandom()
        {
            selectedIndex = Ref.rnd.Int(array.Length);
        }
        public void SelectRandom(Random rnd)
        {
            selectedIndex = rnd.Next(array.Length);
        }

        public void SelectRandom(PcgRandom rnd)
        {
            selectedIndex = rnd.Int(array.Length);
        }

        public int Count
        {
            get { return array.Length; }
        }
    }

}