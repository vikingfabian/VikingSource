using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;

namespace VikingEngine.DSSWars.GameObject
{
    class ArmyStatus
    {
        public int[] typeCount = new int[(int)UnitNameType.NUM];

        public Dictionary<UnitNameType, int> getTypeCounts(PFaction faction)
        {
            Dictionary<UnitNameType, int> result = new Dictionary<UnitNameType, int>();

            if (typeCount != null && typeCount.Length > 0)
            {

                for (int i = 0; i < typeCount.Length; i++)
                {
                    if (typeCount[i] > 0)
                    {
                        result.Add((UnitNameType)i, typeCount[i]);
                    }
                }

                if (faction.TryGetPlayer(out var player) && player.IsLocalPlayer())
                {
                    if (result.ContainsKey(UnitNameType.MithrilKnight) && result.ContainsKey(UnitNameType.MithrilBow))
                    {
                        DssRef.achieve.UnlockAchievement(AchievementIndex.knights_of_lumini);
                    }
                }
            }

            return result;
        }

        public List<KeyValuePair<UnitNameType, int>> getTypeCounts_Sorted(PFaction faction)
        {
            var counts = getTypeCounts(faction);
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
