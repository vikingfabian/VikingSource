using System.Collections.Generic;

namespace VikingEngine
{
    struct ForListLoop<T>
    {
        bool forwardLoop;
        public List<T> list;
        public int selIndex;
        public T sel;
        public bool endPosition;

        public ForListLoop(List<T> list, bool forward = true)
        {
            this.list = list;
            this.forwardLoop = forward;
            sel = default(T);
            endPosition = false;

            if (list != null)
            {
                selIndex = forward ? -1 : list.Count;
            }
            else
            {
                selIndex = -1;
            }
        }

        public void reset()
        {
            selIndex = forwardLoop ? -1 : list.Count;
        }

        public bool next()
        {
            if (list == null)
            {
                return false;
            }

            if (forwardLoop)
            {
                endPosition = ++selIndex >= list.Count;
            }
            else
            {
                endPosition = --selIndex < 0 || list.Count == 0;
            }

            if (endPosition)
            {
                sel = default(T);
                return false;
            }

            sel = list[selIndex];
            return true;
        }

        public void removeCurrent()
        {
            list.RemoveAt(selIndex);
            if (forwardLoop) --selIndex;
        }
    }
}
