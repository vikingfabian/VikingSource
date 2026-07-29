using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public MapSize mapSize = MapSize.Medium;
        public bool centralGold = true;
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
        }
            
        
        const int Version = 3;
        public void write(System.IO.BinaryWriter w, bool storage)
        {
            if (storage)
            {
                w.Write(Version);
                w.Write((int)mapSize);
            }
            w.Write(centralGold);

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
                mapSize = (MapSize)r.ReadInt32();
            }
            centralGold = r.ReadBoolean();
            if (storage)
            {
                if (version >= 1)
                {
                    factionStartSize = (FactionStartSize)r.ReadByte();
                }
                if (version >= 2)
                {
                    QuickBossTimeOption = r.ReadByte();
                }
            }
            if (version >= 3)
            {
                setting_foodMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, FoodMultiBound);
                setting_waterMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, WaterMultiBound);
                
                setting_childMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, ChildMultiBound);
                setting_craftMulti = Bound.ResetOffBounds(r.ReadSingle(), 1, CraftMultiBound);
               
                setting_techMulti = Bound.ResetOffBounds(r.ReadInt32(), 1, TechMultiBound);
                setting_techMulti_QuickMatch = r.ReadInt32();                   
            }

            refreshSettings();
        }
        public void defaultGameSettings()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
            factionStartSize = FactionStartSize.Full;
        }
        public void demoSetup()
        {
            mapSize = MapSize.Medium;
            centralGold = true;
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

    }
}
