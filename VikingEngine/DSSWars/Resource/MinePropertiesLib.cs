using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.DSSWars.Map.MapData;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.DSSWars.Map.MapProcess;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.ToGG;

namespace VikingEngine.Core.DSSWars.Resource
{
    struct MineProperties
    {
        public WorkExperienceType xp;
        public ItemResourceType resourceType;
        public int mineAmount;

        public bool needDigSite;

        public bool hasExtractLimit;

        public MineProperties(
        WorkExperienceType xp,
        ItemResourceType resourceType,
        int mineAmount,
        bool needDigSite,
        bool hasExtractLimit)
        {
            this.xp = xp;
            this.resourceType = resourceType;
            this.mineAmount = mineAmount;
            this.needDigSite = needDigSite;
            this.hasExtractLimit = hasExtractLimit;
        }
    }

    static class MinePropertiesLib
    {
        static MineProperties[] mines;

        public static void Init()
        {
            mines = new MineProperties[(int)TerrainMineType.NUM_NONE];

            mines[(int)TerrainMineType.BogIron] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.IronOre_G,
                TerrainContent.DefaultMineAmount, true, true);
            // Non-mine / water / exploration sub-terrains (needDigSite = false, hasExtractLimit = false)
            mines[(int)TerrainMineType.PointOfInterest] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.Explored1] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.Explored2] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.ExploredFully] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.ClosedMine] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.WaterNaturalSpring] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);
            mines[(int)TerrainMineType.WaterWell] = new MineProperties(WorkExperienceType.Mining, ItemResourceType.NONE, 0, false, false);

            // Extractable mine / resource nodes (needDigSite = true, hasExtractLimit = true)
            mines[(int)TerrainMineType.BogIron] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.IronOre_G,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.ClayPit] = new MineProperties(
                WorkExperienceType.StoneCutter,
                ItemResourceType.Clay,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.Salt] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.Salt,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.Sulfur] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.Sulfur,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.Coal] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.Coal,
                TerrainContent.MineAmount_Coal,
                true,
                true);

            mines[(int)TerrainMineType.StoneBlock] = new MineProperties(
                WorkExperienceType.StoneCutter,
                ItemResourceType.Brick,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.IronOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.IronOre_G,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.TinOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.TinOre,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.CopperOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.CopperOre,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.SilverOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.SilverOre,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.GoldOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.GoldOre,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.LeadOre] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.LeadOre,
                TerrainContent.DefaultMineAmount,
                true,
                true);

            mines[(int)TerrainMineType.Mithril] = new MineProperties(
                WorkExperienceType.Mining,
                ItemResourceType.RawMithril,
                TerrainContent.DefaultMineAmount,
                true,
                true);
        }

        public static MineProperties Get(TerrainMineType type)
        {
            return mines[(int)type];
        }

        public static void GetExtractionLevel(int terrainAmount, out ExtractionLevel level, out float extractionLevelPerc, out int expectedExtractCount)
        {
            if (terrainAmount <= DssConst.ExtractionSpeedDying_Value)
            {
                level = ExtractionLevel.Very_low;
                extractionLevelPerc = DssConst.ExtractionSpeedDying_Percentage;
            }
            if (terrainAmount <= DssConst.ExtractionSpeedLow_Value)
            {
                level = ExtractionLevel.Low;
                extractionLevelPerc = DssConst.ExtractionSpeedLow_Percentage;
            }
            else
            {
                level = ExtractionLevel.High;
                extractionLevelPerc = DssConst.ExtractionSpeedHigh_Percentage;
            }

            expectedExtractCount = terrainAmount * DssConst.MineDecay_DepositsPerTick;
        }
    }

    enum ExtractionLevel
    {
        Very_low,
        Low,
        High,
        NUM
    }
}
