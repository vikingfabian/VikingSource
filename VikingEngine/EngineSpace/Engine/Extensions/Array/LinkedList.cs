using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    
    class LinkedList<T>
    {
        public LinkedListMember<T> First = null;
        public LinkedListMember<T> Last = null;
        public int Count { get; private set; }

        public void Add(T value)
        {
            Insert(null, value);
        }

        public void Insert(LinkedListMember<T> beforeMember, T value)
        {
            ++Count;
            LinkedListMember<T> newMember = new LinkedListMember<T>(value);
            if (First == null)
            {
                First = newMember;
                Last = First;
            }
            else if (beforeMember == null)
            {//place last
                Last.AddNextMember(newMember);
                Last = newMember;
            }
            else
            {
                beforeMember.AddNextMember(newMember);
                if (beforeMember == First)
                {
                    First = newMember;
                }
            }
        }

        public void Remove(T value)
        {
            LinkedListMember<T> current = First;
            while (Next(ref current))
            {
                if (current.Value.Equals(value))
                {
                    RemoveMember(current);
                    return;
                }
            }
        }

        public bool Next(ref LinkedListMember<T> current)
        {
            if (current != null && current.Next != null)
            {
                current = current.Next;
                return true;
            }
            return false;
        }
        public bool Previous(ref LinkedListMember<T> current)
        {
            if (current != null && current.Previous != null)
            {
                current = current.Previous;
                return true;
            }
            return false;
        }

        public void RemoveMember(LinkedListMember<T> member)
        {
            --Count;
            member.Remove();
            if (Count == 0)
            {
                Clear();
            }
            else
            {
                if (member == First)
                {
                    First = member.Next;
                }
                if (member == Last)
                {
                    Last = member.Previous;
                }
            }
        }

        public void Clear()
        {
            Count = 0;
            First = null; Last = null;
        }
    }

    class LinkedListMember<T>
    {
        public T Value;
        public LinkedListMember<T> Previous;
        public LinkedListMember<T> Next;

        public LinkedListMember(T value)
        {
            this.Value = value;
        }

        public void AddNextMember(LinkedListMember<T> next)
        {
            LinkedListMember<T> oldNext = Next;
            Next = next;
            next.Previous = this;
            if (oldNext != null)
            { //is inserted
                next.AddNextMember(oldNext);
            }
        }

        public void Remove()
        {
            if (Previous != null)
            {
                Previous.Next = this.Next;
            }
            if (Next != null)
            {
                Next.Previous = this.Previous;
            }
        }
    }
}
