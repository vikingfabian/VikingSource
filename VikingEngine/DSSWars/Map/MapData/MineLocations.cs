using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Data;
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

        public MineLocations()
        {
            typesCountLeft = new int[(int)MineLocationType.NUM];
        }

        public MineLocations(WorldData world, int totalMineCount)
            : this()
        {
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

            const double DefaultCommoness = 0.03;

            addCount(MineLocationType.Mithril, mithrilCount);
            addCount(MineLocationType.SilverOre, MathExt.MultiplyInt(world.rnd.Double(1, 1.2) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.GoldOre, MathExt.MultiplyInt(world.rnd.Double(0.5, 0.6) * DefaultCommoness, totalMineCount));


            addCount(MineLocationType.WaterNaturalSpring, MathExt.MultiplyInt(world.rnd.Double(0.9, 1.1) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.BogIron, MathExt.MultiplyInt(world.rnd.Double(4, 4.5) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.IronOre, MathExt.MultiplyInt(world.rnd.Double(2, 2.5) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.TinOre, MathExt.MultiplyInt(world.rnd.Double(1, 1.2) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.CopperOre, MathExt.MultiplyInt(world.rnd.Double(0.05, 0.06), totalMineCount));
            addCount(MineLocationType.LeadOre, MathExt.MultiplyInt(world.rnd.Double(0.9, 1.1) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.Sulfur, MathExt.MultiplyInt(world.rnd.Double(0.9, 1.1) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.ClayPit, MathExt.MultiplyInt(world.rnd.Double(0.9, 1.1) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.Salt, MathExt.MultiplyInt(world.rnd.Double(0.8, 0.85) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.StoneBlock, MathExt.MultiplyInt(world.rnd.Double(1.5, 1.8) * DefaultCommoness, totalMineCount));
            addCount(MineLocationType.Coal, MathExt.MultiplyInt(world.rnd.Double(1.5, 1.8) * DefaultCommoness, totalMineCount));

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

        public void write(System.IO.BinaryWriter w)
        {
            for (int i = 0; i < typesCountLeft.Length; ++i)
            {
                w.Write((ushort)typesCountLeft[i]);
            }
        }
        public void read(System.IO.BinaryReader r, int subversion)
        {
            for (int i = 0; i < typesCountLeft.Length; ++i)
            {
                int count = r.ReadUInt16();
                typesCountLeft[i] = count;
                totalCount += count;
            }
        }

        void addCount(MineLocationType type, int count)
        {
            typesCountLeft[(int)type] = count;
        }

        public void PullOneBogMine(int depthLevel, out MapTile1_1 result)
        {
            if (typesCountLeft[(int)MineLocationType.BogIron] > 0)
            {
                typesCountLeft[(int)MineLocationType.BogIron]--;
            }
            result = new MapTile1_1() { mainTerrain = TerrainMainType.Mine, subTerrain = (byte)TerrainMineType.BogIron };
            depthToResultRange(depthLevel, ref result);
        }

        public bool LandSurveyResult(int depthLevel, out MapTile1_1 result)
        {
            MineLocationType locationFound = MineLocationType.None;
            result = new MapTile1_1() { mainTerrain = TerrainMainType.Mine };

            if (totalCount > 0)
            {
                int index = Ref.rnd.Int(totalCount);
                int exend = depthToResultRange(depthLevel, ref result);

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

        private int depthToResultRange(int depthLevel, ref MapTile1_1 result)
        {
            int exend;
            if (depthLevel <= 1)
            {
                exend = (int)MineLocationType.SilverOre;
                result.terrainAmount = (byte)DssConst.MineDeposits_VeryLow.GetRandom_Inc();
            }
            else
            {
                exend = typesCountLeft.Length;
                if (Ref.rnd.Chance(DssConst.MineDeposits_Endless_Chance))
                {
                    result.terrainAmount = (byte)DssConst.MineDeposits_High.GetRandom_Inc();
                }
                else
                {
                    result.terrainAmount = (byte)DssConst.MineDeposits_Low.GetRandom_Inc();
                }
            }

            return exend;
        }

    }

    enum MineLocationType
    {
        None,
        WaterNaturalSpring,

        StoneBlock,
        BogIron,
        ClayPit,
        Salt,
        //Salt,
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
