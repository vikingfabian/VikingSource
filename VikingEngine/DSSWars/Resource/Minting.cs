using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
{
    static class Minting
    {
        public static CraftBlueprint[] CoinCraftTypes = { ElfCoin, SilverCoin, BronzeCoin, CopperCoin };

        public static readonly CraftBlueprint ConvertGoldOre = new CraftBlueprint(
          CraftResultType.Resource,
          (int)ItemResourceType.Gold,
          5 * DssConst.GoldOreSellValue,
         new UseResource[]
         {
               new UseResource(ItemResourceType.GoldOre, 5),
               new UseResource(ItemResourceType.Fuel_G, 5),
         },
            XP.WorkExperienceType.Smelting, XP.ExperienceLevel.Beginner_1,  Build.BuildAndExpandType.Smelter
        );

        public static readonly CraftBlueprint CopperCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           5 * DssConst.CopperSellValue,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Copper, 5),
           },
            XP.WorkExperienceType.CraftMetal,  XP.ExperienceLevel.Beginner_1, Build.BuildAndExpandType.CoinMinter
       )
        { workTag = (int)ItemResourceType.CopperCoin };

        public static readonly CraftBlueprint BronzeCoin = new CraftBlueprint(
            CraftResultType.Resource,
            (int)ItemResourceType.Gold,
           5 * DssConst.BronzeSellValue,
           new UseResource[]
           {
               new UseResource(ItemResourceType.Bronze, 5),
           },
            XP.WorkExperienceType.CraftMetal, XP.ExperienceLevel.Beginner_1, Build.BuildAndExpandType.CoinMinter
       )
        { workTag = (int)ItemResourceType.BronzeCoin };

        public static readonly CraftBlueprint SilverCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          5 * DssConst.SilverSellValue,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Silver, 5),
          },
           XP.WorkExperienceType.CraftMetal, XP.ExperienceLevel.Beginner_1, Build.BuildAndExpandType.CoinMinter
      )
        { workTag = (int)ItemResourceType.SilverCoin };

        public static readonly CraftBlueprint ElfCoin = new CraftBlueprint(
           CraftResultType.Resource,
           (int)ItemResourceType.Gold,
          DssConst.MithrilSellValue,
          new UseResource[]
          {
               new UseResource(ItemResourceType.Mithril, 1),
          },
           XP.WorkExperienceType.CraftMetal, XP.ExperienceLevel.Beginner_1, Build.BuildAndExpandType.CoinMinter
      )
        { workTag = (int)ItemResourceType.ElfCoin };

    }
}
