using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.MapData;
using VikingEngine.DSSWars.Map.MapLib;

namespace VikingEngine.Core.DSSWars.Map.MapData
{
    class MineLocations
    {
        public const int ShallowMine = 1;
        public const int DeepMine = 2;
        int totalCount;
        int[] typesCountLeft;

        public MineLocations(WorldData world, int totalMineCount)
        {
            typesCountLeft = new int[(int)MineLocationType.NUM];

            int mithrilCount = 0;
            switch (world.metaData.mapScale.sizeCategory)
            {
                //Tiny, Small, Medium, Large, Huge, Epic
                default:
                    mithrilCount = 1;
                    break;

                case MapSizeCategory.Medium:
                    mithrilCount = 2;
                    break;

                case MapSizeCategory.Large:
                    mithrilCount = 3;
                    break;

                case MapSizeCategory.Huge:
                case MapSizeCategory.Epic:
                    mithrilCount = 4;
                    break;
            }

            if (world.rnd.Chance(0.4))
            {
                ++mithrilCount;

                if (world.rnd.Chance(0.1))
                {
                    ++mithrilCount;
                }
            }

            addCount(MineLocationType.Mithril, mithrilCount);
            addCount(MineLocationType.SilverOre, MathExt.MultiplyInt(world.rnd.Double(0.04, 0.05), totalMineCount));
            addCount(MineLocationType.GoldOre, MathExt.MultiplyInt(world.rnd.Double(0.02, 0.03), totalMineCount));


            addCount(MineLocationType.WaterNaturalSpring, MathExt.MultiplyInt(world.rnd.Double(0.035, 0.045), totalMineCount));
            addCount(MineLocationType.BogIron, MathExt.MultiplyInt(world.rnd.Double(0.14, 0.16), totalMineCount));
            addCount(MineLocationType.IronOre, MathExt.MultiplyInt(world.rnd.Double(0.07, 0.08), totalMineCount));
            addCount(MineLocationType.TinOre, MathExt.MultiplyInt(world.rnd.Double(0.04, 0.05), totalMineCount));
            addCount(MineLocationType.CopperOre, MathExt.MultiplyInt(world.rnd.Double(0.05, 0.06), totalMineCount));
            addCount(MineLocationType.LeadOre, MathExt.MultiplyInt(world.rnd.Double(0.035, 0.045), totalMineCount));
            addCount(MineLocationType.Sulfur, MathExt.MultiplyInt(world.rnd.Double(0.035, 0.045), totalMineCount));
            addCount(MineLocationType.Salt, MathExt.MultiplyInt(world.rnd.Double(0.06, 0.07), totalMineCount));
            addCount(MineLocationType.StoneBlock, MathExt.MultiplyInt(world.rnd.Double(0.06, 0.07), totalMineCount));
            addCount(MineLocationType.Coal, MathExt.MultiplyInt(world.rnd.Double(0.05, 0.06), totalMineCount));

            totalCount = 0;
            for (int i = 0; i < typesCountLeft.Length; ++i)
            {
                totalCount += typesCountLeft[i];
            }

            if (totalCount < totalMineCount)
            {
                addCount(MineLocationType.None, totalMineCount - totalCount);
            }
        }

        void addCount(MineLocationType type, int count)
        {
            typesCountLeft[(int)type] = count;
        }

        public bool LandSurveyResult(int depthLevel, out MapTile1_1 result)
        {
            MineLocationType locationFound = MineLocationType.None;

            if (totalCount > 0)
            {
                int index = Ref.rnd.Int(totalCount);
                int exend;
                if (depthLevel <= 1)
                {
                    exend = (int)MineLocationType.SilverOre;
                    result.terrainAmount = (byte)DssConst.MineDeposits_Shallow.GetRandom_Inc();
                }
                else
                {
                    exend = typesCountLeft.Length;
                    if (Ref.rnd.Chance(DssConst.MineDeposits_Endless_Chance))
                    {
                        result.terrainAmount = DssConst.MineDeposits_Endless;
                    }
                    else
                    {
                        result.terrainAmount = (byte)DssConst.MineDeposits_Medium.GetRandom_Inc();
                    }
                }

                for (int i = 0; i < exend; ++i)
                {
                    index -= typesCountLeft[i];
                    if (index < 0)
                    {
                        //Found
                        typesCountLeft[i]--;
                        locationFound = (MineLocationType)i;
                        break;
                    }
                }
                totalCount--;
            }

            result = new MapTile1_1() { mainTerrain = TerrainMainType.Mine };
            switch (locationFound)
            {
                default:
                case MineLocationType.None:
                    result.subTerrain = (int)TerrainMineType.Explored1;
                    return false;
                case MineLocationType.WaterNaturalSpring:
                    result.subTerrain = (int)TerrainMineType.WaterNaturalSpring;
                    break;
                case MineLocationType.BogIron:
                    result.subTerrain = (int)TerrainMineType.BogIron;
                    break;
                case MineLocationType.ClayPit:
                    result.subTerrain = (int)TerrainMineType.ClayPit;
                    break;
                case MineLocationType.SaltPit:
                    result.subTerrain = (int)TerrainMineType.SaltPit;
                    break;
                case MineLocationType.Salt:
                    result.subTerrain = (int)TerrainMineType.Salt;
                    break;
                case MineLocationType.Sulfur:
                    result.subTerrain = (int)TerrainMineType.Sulfur;
                    break;
                case MineLocationType.Coal:
                    result.subTerrain = (int)TerrainMineType.Coal;
                    break;
                case MineLocationType.StoneBlock:
                    result.subTerrain = (int)TerrainMineType.StoneBlock;
                    break;
                case MineLocationType.IronOre:
                    result.subTerrain = (int)TerrainMineType.IronOre;
                    break;
                case MineLocationType.TinOre:
                    result.subTerrain = (int)TerrainMineType.TinOre;
                    break;
                case MineLocationType.CopperOre:
                    result.subTerrain = (int)TerrainMineType.CopperOre;
                    break;
                case MineLocationType.SilverOre:
                    result.subTerrain = (int)TerrainMineType.SilverOre;
                    break;
                case MineLocationType.GoldOre:
                    result.subTerrain = (int)TerrainMineType.GoldOre;
                    break;
                case MineLocationType.LeadOre:
                    result.subTerrain = (int)TerrainMineType.LeadOre;
                    break;
                case MineLocationType.Mithril:
                    result.subTerrain = (int)TerrainMineType.Mithril;
                    break;
            }
            return true;
        }
    }

    enum MineLocationType
    {
        None,
        WaterNaturalSpring,

        StoneBlock,
        BogIron,
        ClayPit,
        SaltPit,
        Salt,
        Sulfur,
        Coal,
        IronOre,
        TinOre,
        LeadOre,
        CopperOre,

        //These cant be found in shallow ground
        SilverOre,
        GoldOre,
        Mithril,
        NUM
    }
}
