using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
{
    static class Minting
    {
        public static readonly CraftBlueprint ConvertGoldOre = new CraftBlueprint(
          CraftResultType.Resource,
          (int)ItemResourceType.Gold,
            DssConst.GoldOreSellValue,
         new UseResource[]
         {
               new UseResource(ItemResourceType.GoldOre, 1),
         },
            XP.WorkExperienceType.CraftMetal, XP.ExperienceLevel.Beginner_1, CraftRequirement.Smelter
        );

        public static readonly CraftBlueprint CopperCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           5 * DssConst.CopperSellValue,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Copper, 5),
           },
            XP.WorkExperienceType.NONE
       );

        public static readonly CraftBlueprint BronzeCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           5 * DssConst.BronzeSellValue,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Bronze, 5),
           },
            XP.WorkExperienceType.NONE
       );

        public static readonly CraftBlueprint SilverCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          5 * DssConst.SilverSellValue,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Silver, 5),
          },
           XP.WorkExperienceType.NONE
      );

        public static readonly CraftBlueprint ElfCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          DssConst.MithrilSellValue,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Mithril, 1),
          },
           XP.WorkExperienceType.NONE
      );

    }
}
