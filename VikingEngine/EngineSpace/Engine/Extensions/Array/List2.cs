using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine
{
    class List2<T> : List<T>
    {
        bool forwardLoop;
        public int selIndex = -1;
        public T sel;
        public bool endPosition;

        public List2()
            : base()
        { }

        public List2(int capacity)
            : base(capacity)
        { }

        public List2(IEnumerable<T> collection)
            : base(collection)
        { }

        public bool SelectItem(T selectItem)
        {
            for (int i = 0; i < this.Count; ++i)
            {
                if (this[i].Equals(selectItem))
                {
                    sel = this[i];
                    selIndex = i;
                    return true;
                }
            }

            return false;
        }

        public void SelectIndex(int index)
        {
            sel = this[index];
            selIndex = index;
        }

        public void Add(T item, bool select)
        {
            if (select)
            {
                sel = item;
                selIndex = this.Count;
            }
            this.Add(item);
        }

        public void AddFirst(T item)
        {
            Insert(0, item);
        }

        public void selectNext_Rollover()
        {
            if (++selIndex >= Count)
            {
                selIndex = 0;
            }

            sel = this[selIndex];
        }

        public void selectFirst()
        {
            selIndex = 0;
            if (Count > 0)
            {
                sel = this[selIndex];
            }
        }

        public void selectNext()
        {
            if (selIndex + 1 < Count)
            {
                ++selIndex;
                sel = this[selIndex];
            }
        }

        public bool Next_IsEnd(bool rollover)
        {
            selectNext();
            return IsLast;
        }

        public void selectPrev()
        {
            if (selIndex > 0)
            {
                --selIndex;
                sel = this[selIndex];
            }
        }

        public T PeekNext(bool rollover, out int nextIx, out bool reachedEnd)
        {
            nextIx = selIndex + 1;
            if (nextIx >= this.Count)
            {
                if (rollover)
                { nextIx = 0; }
                else
                { nextIx = this.Count - 1; }

                reachedEnd = true;
            }
            else
            {
                reachedEnd = false;
            }

            return this[nextIx];
        }

        public void loopBegin(int startIndex, bool forwardLoop = true)
        {
            this.forwardLoop = forwardLoop;
            endPosition = false;

            selIndex = startIndex + (forwardLoop? -1 : 1);
        }

        public void loopBegin(bool forwardLoop = true)
        {
            this.forwardLoop = forwardLoop;
            endPosition = false;
            loopReset();
        }

        public void loopReset()
        {
            selIndex = forwardLoop ? -1 : Count;
        }

        public bool loopNext()
        {
            if (forwardLoop)
            {
                endPosition = ++selIndex >= Count;
            }
            else
            {
                endPosition = --selIndex < 0 || Count == 0;
            }

            if (endPosition)
            {
                sel = default(T);
                return false;
            }

            sel = this[selIndex];
            return true;
        }

        public void loopRemove()
        {
            RemoveAt(selIndex);
            if (forwardLoop) --selIndex;
        }

        public void loopRemove(T item)
        {
            if (item.Equals(sel))
            {
                loopRemove();
            }
            else
            {
                this.Remove(item);
            }
        }

        public void set(T item)
        {
            this[selIndex] = item;
            sel = this[selIndex];
        }
        public void setEmpty()
        {
            this[selIndex] = default(T);
            sel = this[selIndex];
        }

        public bool IsEmpty => this[selIndex].Equals(default(T));
     
        public bool IsFirst => selIndex == 0;

        public bool IsLast => selIndex == Count - 1;

        public T First => this[0];
        public T Last => this[this.Count - 1];
        
        public void Unselect()
        {
            sel = default(T);
            selIndex = -1;
        }

        public bool HasSelection => selIndex >= 0 && selIndex < Count;

        public bool NoSelection => selIndex < 0 || selIndex >= Count;

        public void DeleteAll()
        {
            for (int i = Count - 1; i >= 0; --i)
            {
                if (this[i] is IDeleteable &&
                    this[i] != null)
                {
                    ((IDeleteable)this[i]).DeleteMe();
                }
            }
            Clear();            
        }

        public void SetMaxCount(int maxCount, bool removeLast = true)
        {
            while (Count > maxCount)
            {
                if (removeLast)
                {
                    RemoveLast();
                }
                else
                {
                    RemoveFirst();
                }
            }
        }

        public T RemoveFirst()
        {
            var result = this[0];
            RemoveAt(0);

            return result;
        }

        public T RemoveLast()
        {
            int ix = this.Count - 1;
            var result = this[this.Count - 1];
            RemoveAt(ix);

            return result;
        }


    }

}
