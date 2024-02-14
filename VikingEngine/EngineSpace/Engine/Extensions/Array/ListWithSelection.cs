using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    /// <summary>
    /// List that keeps track of which member is selected
    /// </summary>
    public class ListWithSelection<T>
    {
        public int selectedIndex = 0;
        public List<T> list;

        public ListWithSelection()
        {
            list = new List<T>();
        }
        public ListWithSelection(int count)
        {
            list = new List<T>(count);
        }
        public ListWithSelection(List<T> list)
        {
            this.list = list;
        }

        //public void AddAfterSeleted(T newMember, bool selectNewMember)
        //{
        //    if (list.Count == 0)
        //    {
        //        list.Add(newMember);
        //        selectedIndex = 0;
        //    }
        //    else
        //    {
        //        list.Insert(selectedIndex + 1, newMember);
        //        if (selectNewMember)
        //        {
        //            ++selectedIndex;
        //        }
        //    }
        //}

        public void Add(T newMember, bool selectNewMember)
        {
            list.Add(newMember);
            if (selectNewMember)
                selectedIndex = list.Count - 1;
        }

        public T Selected()
        {
            if (selectedIndex >= 0 && selectedIndex < list.Count)
                return list[selectedIndex];
            return default(T);
        }

        public T First => list[0];
        public T Last => list[list.Count -1];


        /// <summary>
        /// Moves the selection forward one step
        /// </summary>
        /// <returns>Reached end</returns>
        public bool Next_IsEnd(bool rollover)
        {
            bool reachedEnd;

            PeekNext(rollover, out selectedIndex, out reachedEnd);
            return reachedEnd;
        }

        /// <summary>
        /// Moves the selection forward one step
        /// </summary>
        /// <returns>Found Next</returns>
        public bool Next(bool rollover)
        {
            bool reachedEnd;

            PeekNext(rollover, out selectedIndex, out reachedEnd);
            return !reachedEnd;
        }

        public T PeekNext(bool rollover, out int nextIx, out bool reachedEnd)
        {
            if (list.Count == 0)
            {
                nextIx = 0;
                reachedEnd = true;
                return default(T);
            }

            nextIx = selectedIndex +1;
            if (nextIx >= list.Count)
            {
                if (rollover)
                { nextIx = 0; }
                else
                { nextIx = list.Count - 1; }

                reachedEnd = true;
            }
            else
            {
                reachedEnd = false;
            }

            return list[nextIx];
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
                    selectedIndex = list.Count - 1;
                else
                    selectedIndex = 0;
            }
        }

        /// <summary>
        /// Move the selected member in the list
        /// </summary>
        //public void Move(int dir)
        //{
        //    T member = Selected();
        //    list.RemoveAt(selectedIndex);
        //    int placement = Bound.Set(selectedIndex + dir, 0, list.Count);
        //    list.Insert(placement, member);
        //    selectedIndex = placement;
        //}

        /// <summary>
        /// Removes the selected member
        /// </summary>
        public void Remove()
        {
            list.RemoveAt(selectedIndex);
            selectedIndex = Bound.Set(selectedIndex, 0, list.Count - 1);
        }

        public void RemoveAndUnselect()
        {
            Remove();
            Unselect();
        }

        public int GetIndex(T member)
        {
            for (int i = 0; i < list.Count; ++i)
            {
                if (list[i].Equals(member))
                {
                    return i;
                }
            }
            return -1;
        }

        public bool HasSelection
        { get { return selectedIndex >= 0 && selectedIndex < list.Count; } }

        public void Unselect()
        {
            selectedIndex = -1;
        }

        public void SelectRandom()
        {
            selectedIndex = Ref.rnd.Int(list.Count);
        }

        public int Count
        {
            get
            {
                if (list != null)
                {
                    return list.Count;
                }
                return 0;
            }
        }

        public override string ToString()
        {
            return list.ToString();
        }
    }

}