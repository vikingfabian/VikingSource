using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;

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

        public static readonly CraftBlueprint CraftSettler = new CraftBlueprint(
            CraftResultType.NoSet,
            0,
            1,
            new UseResource[]
            {
                new UseResource(ItemResourceType.Men, 60),
                new UseResource(ItemResourceType.SharpStick, 30),
                new UseResource(ItemResourceType.Food_G, 400),
                new UseResource(ItemResourceType.Wood_Group, 300),
                new UseResource(ItemResourceType.SkinLinen_Group, 500)
            },
            XP.WorkExperienceType.NONE,
            XP.ExperienceLevel.Beginner_1,
            CraftRequirement.None
        );
    }

}
