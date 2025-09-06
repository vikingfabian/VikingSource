using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;

namespace VikingEngine.DSSWars.Conscript
{
    static class ConscriptDataLib
    {
        public static readonly BuildAndExpandType[] BarrackTypes = new BuildAndExpandType[]
            {
                BuildAndExpandType.SoldierBarracks,
                BuildAndExpandType.ArcherBarracks,
                BuildAndExpandType.WarmachineBarracks,
                BuildAndExpandType.GunBarracks,
                BuildAndExpandType.CannonBarracks,
                BuildAndExpandType.KnightsBarracks,
            };
        public static Dictionary<BuildAndExpandType, int> TypeToBarrackTypeIx;

        public static void Init()
        {
            TypeToBarrackTypeIx = new Dictionary<BuildAndExpandType, int>(BarrackTypes.Length);
            for (int i = 0; i < BarrackTypes.Length; i++)
            {
                TypeToBarrackTypeIx.Add(BarrackTypes[i], i);
            }
        }
    }

}
