using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class ArmyStatus
    {
        public int[] typeCount = new int[(int)UnitFilterType.NUM];

        public Dictionary<UnitFilterType, int> getTypeCounts()
        {
            Dictionary<UnitFilterType, int> result = new Dictionary<UnitFilterType, int>();
            for (int i = 0; i < typeCount.Length; i++) 
            {
                if (typeCount[i] > 0)
                {
                    result.Add((UnitFilterType)i, typeCount[i]);
                }
            }

            return result;
        }

        public List<KeyValuePair<UnitFilterType, int>> getTypeCounts_Sorted()
        {
            var counts = getTypeCounts();
            var sortedList = counts.ToList();
            sortedList.Sort((pair1, pair2) => pair2.Value.CompareTo(pair1.Value));
            return sortedList;
        }

    }

    //class UnitTypeStatus
    //{ 
    //    public 
    //}
}
