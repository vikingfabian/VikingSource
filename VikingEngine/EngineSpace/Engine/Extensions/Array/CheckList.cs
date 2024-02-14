using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine
{
    interface ICheckListMember
    {
        bool CheckList_Enabled
        {
            get;
            set;
        }
    }

    class CheckList<T> : ListWithSelection<T> where T : ICheckListMember
    {
        public CheckList()
            : base()
        { }

        public CheckList(int count)
            : base(count)
        { }

        public void DisableAndMoveToNext()
        {
            var sel = Selected();
            if (sel != null)
            {
                sel.CheckList_Enabled = false;
            }
            NextEnabled(true);
        }

        /// <summary>
        /// Moves the selection forward one step
        /// </summary>
        /// <returns>Found member</returns>
        public bool NextEnabled(bool rollover)
        {
            if (selectedIndex < 0 || selectedIndex >= list.Count)
            {
                if (list.Count == 0)
                {
                    selectedIndex = -1;
                    return false;
                }
                selectedIndex = 0;
            }
            nextEnabled(selectedIndex, 1, rollover);
            return selectedIndex >= 0;
        }

        /// <summary>
        /// Moves the selection forward one step
        /// </summary>
        /// <returns>Found member</returns>
        public bool PreviousEnabled(bool rollover)
        {
            if (selectedIndex < 0 || selectedIndex >= list.Count)
            {
                if (list.Count == 0)
                {
                    selectedIndex = -1;
                    return false;
                }
                selectedIndex = 0;
            }
            nextEnabled(selectedIndex, -1, rollover);
            return selectedIndex >= 0;
        }

        void nextEnabled(int startIndex, int dir, bool rollover)
        {
            selectedIndex += dir;
            if (selectedIndex < 0)
            {
                if (rollover)
                {
                    selectedIndex = list.Count - 1;
                }
                else
                {
                    selectedIndex = -1;
                    return;
                }
            }
            if (selectedIndex >= list.Count)
            {
                if (rollover)
                {
                    selectedIndex = 0;
                }
                else
                {
                    selectedIndex = -1;
                    return;
                }
            }

            if (selectedIndex == startIndex)
            {
                if (!list[selectedIndex].CheckList_Enabled)
                {
                    selectedIndex = -1;
                }
                return;

            }
            if (list[selectedIndex].CheckList_Enabled)
            {
                return;
            }
            else
            {
                nextEnabled(startIndex, dir, rollover);
            }
          
        }

        public bool HasEnabledMember()
        {
            foreach (T m in list)
            {
                if (m.CheckList_Enabled)
                    return true;
            }
            return false;
        }

        public int EnabledCount()
        {
            int result = 0;
            foreach (T m in list)
            {
                if (m.CheckList_Enabled)
                    ++result;
            }
            return result;
        }

     

        public void EnableAll(bool enable)
        {
            foreach (T m in list)
            {
                m.CheckList_Enabled = enable;
            }
        }
    }


}
