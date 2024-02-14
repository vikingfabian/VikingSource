using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class ArmyStatus
    {
        public int[] typeCount = new int[(int)UnitType.NUM];

        public Dictionary<UnitType, int> getTypeCounts()
        {
            Dictionary<UnitType, int> result = new Dictionary<UnitType, int>();
            for (int i = 0; i < typeCount.Length; i++) 
            {
                if (typeCount[i] > 0)
                {
                    result.Add((UnitType)i, typeCount[i]);
                }
            }

            return result;
        }
    }

    //class UnitTypeStatus
    //{ 
    //    public 
    //}
}
