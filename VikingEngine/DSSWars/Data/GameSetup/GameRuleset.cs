using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Core.DSSWars.Data.GameSetup;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map.MapLib;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Data
{
    struct GameRuleset
    {
        public static readonly IntervalF FoodMultiBound = new IntervalF(0.5f, 10f);
        public static readonly IntervalF WaterMultiBound = new IntervalF(0.2f, 10f);
        public static readonly IntervalF ChildMultiBound = new IntervalF(0.2f, 10f);
        public static readonly IntervalF CraftMultiBound = new IntervalF(0.1f, 4f);
        public static readonly Range TechMultiBound = new Range(1, 10);

        public static readonly GameModeMainType[] AvailableModes = [GameModeMainType.FullStory, GameModeMainType.QuickBoss, /*GameModeMainType.QuickMatch,*/ GameModeMainType.Sandbox, GameModeMainType.Peaceful/*, GameModeMainType.Spectator*/];
        public static readonly TwoInts[] QuickBossOptions_Time_Difficulty = [new TwoInts(3, 100), new TwoInts(5, 50), new TwoInts(8, 25)];

        public MapScale mapSize = new MapScale(MapSizeCategory.Medium);

        public bool module_fogOfWar = false;
        public bool module_foodAndWater = false;
        public bool module_deptAndDeserters = false;
        public bool module_physicalGold = false;
        public bool module_noPauseCommand = false;

        public FactionStartSize factionStartSize = FactionStartSize.OneCity;
        public int QuickBossTimeOption = 1;

        public float setting_foodMulti = 1;
        public float setting_waterMulti = 1;
        public float setting_childMulti = 1;
        public float setting_craftMulti = 1;
        public int setting_techMulti = 1;

        public int setting_techMulti_QuickMatch = 2;

        public int FoodEnergySett;
        public float manFoodUpkeep;
        public float mountFoodUpkeep;

        public CityHallSetup[] CitySizeSetup = new CityHallSetup[(int)CityType.NUM];


        public GameRuleset()
        { }

        public void refreshSettings()
        {
            FoodEnergySett = Convert.ToInt32(DssConst.FoodEnergy * setting_foodMulti);
            manFoodUpkeep = DssConst.ManDefaultEnergyCost / FoodEnergySett;
            mountFoodUpkeep = DssConst.MountDefaultEnergyCost / FoodEnergySett;

            if (!DssRef.storage.metaProgression.unlockedDangerousSettings)
            {
                setting_foodMulti = 1;
                setting_waterMulti = 1;
                setting_childMulti = 1;
                setting_craftMulti = 1;
            }

            for (CityType sz = 0; sz < CityType.NUM; sz++)
            {
                CitySizeSetup[(int)sz] = new CityHallSetup(sz, module_foodAndWater);
            }
        }


        const int Version = 5;
        public void write(System.IO.BinaryWriter w, bool storage)
        {
            if (storage)
            {
                w.Write(Version);
                mapSize.write(w);
            }
            w.Write(module_fogOfWar);
            w.Write(module_foodAndWater);
            w.Write(module_deptAndDeserters);
            w.Write(module_physicalGold);

            if (storage)
            {
                w.Write((byte)factionStartSize);
                w.Write((byte)QuickBossTimeOption);
            }
            w.Write(setting_foodMulti);
            w.Write(setting_waterMulti);
            w.Write(setting_childMulti);
            w.Write(setting_craftMulti);
            w.Write(setting_techMulti);
            w.Write(setting_techMulti_QuickMatch);
        }
        public void read(System.IO.BinaryReader r, bool storage)
        {
            int version = int.MaxValue;

            if (storage)
            {
                version = r.ReadInt32();

                mapSize.read(r);

            }

            if (version < 5)
            {
                return;
            }
            module_fogOfWar = r.ReadBoolean();
            module_foodAndWater = r.ReadBoolean();
            module_deptAndDeserters = r.ReadBoolean();
            module_physicalGold = r.ReadBoolean();

            if (storage)
            {
                factionStartSize = (FactionStartSize)r.ReadByte();

                QuickBossTimeOption = r.ReadByte();
            }

            setting_foodMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, FoodMultiBound);
            setting_waterMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, WaterMultiBound);

            setting_childMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, ChildMultiBound);
            setting_craftMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, CraftMultiBound);

            setting_techMulti = Bound.ResetOffBounds(r.ReadInt32(), 1, TechMultiBound);
            setting_techMulti_QuickMatch = r.ReadInt32();


            refreshSettings();
        }
        public void defaultGameSettings()
        {
            mapSize = new MapScale(MapSizeCategory.Medium);
            module_physicalGold = true;
            factionStartSize = FactionStartSize.Full;
        }
        public void demoSetup()
        {
            mapSize = new MapScale(MapSizeCategory.Medium);
            module_physicalGold = true;
            factionStartSize = FactionStartSize.Full;
        }

        public int TechMultiProperty(object tag, bool set, int value)
        {
            if (set)
            {
                if (DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch)
                {
                    setting_techMulti_QuickMatch = value;
                }
                else
                {
                    setting_techMulti = value;
                }

            }
            return DssRef.difficulty.setting_gameMode == GameModeMainType.QuickMatch ? setting_techMulti_QuickMatch : setting_techMulti;
        }

        public void gameSetupToHud(RichBoxContent content)
        {
            content.icontext(HudLib.CheckImage(module_fogOfWar), DssRef.todoLang.Module_FogOfWar);
            content.icontext(HudLib.CheckImage(module_foodAndWater), DssRef.todoLang.Module_FoodUpkeep);
            content.icontext(HudLib.CheckImage(module_deptAndDeserters), DssRef.todoLang.Module_LossOfUnits);
            content.icontext(HudLib.CheckImage(module_physicalGold), DssRef.todoLang.Module_LocalGold);
            content.icontext(HudLib.CheckImage(module_noPauseCommand), DssRef.todoLang.Module_NoPause);

            content.icontext(SpriteName.WarsResource_Food, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_FoodMultiplier, TextLib.OneDecimal(setting_foodMulti)));
            content.icontext(SpriteName.WarsResource_WaterAdd, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_WaterMultiplier, TextLib.OneDecimal(setting_waterMulti)));
            content.icontext(SpriteName.WarsWorker, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_ChildMultiplier, TextLib.OneDecimal(setting_childMulti)));
            content.icontext(SpriteName.WarsHammer, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_CraftMultiplier, TextLib.OneDecimal(setting_craftMulti)));
            content.icontext(SpriteName.WarsTechnology_Unlocked, string.Format(DssRef.lang.Language_ItemCount_Colon, DssRef.lang.Settings_TechMultiplier, TechMultiProperty(null, false, 0)));

        }


    }
}
