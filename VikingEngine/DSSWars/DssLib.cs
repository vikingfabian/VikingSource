using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Graphics;
using VikingEngine.LootFest;


namespace VikingEngine.DSSWars
{
    static class DssLib
    {
        public const bool UseLocalTrading = false;

        public const VoxelModelName WorkerModel = VoxelModelName.war_gnome;

        public static readonly string ContentDir = "DSS" + DataStream.FilePath.Dir;
        public static readonly string StoryContentDir = ContentDir + "Story" + DataStream.FilePath.Dir;

#region DEBUG
        public static readonly bool RandomSeed = PlatformSettings.DebugOptions ? true :
            true;//DO NOT CHANGE
        
        public static readonly bool DebugSquareInfo = PlatformSettings.DebugOptions ? false :
            false;//DO NOT CHANGE 
        public static readonly bool UpdateBorders = PlatformSettings.DebugOptions ? true :
            true;//DO NOT CHANGE

        public const bool AllowDoubleTime = false;
        #endregion
        
        public const int UserHeraldicWidth = 16;
        public const int MaxLocalPlayers = 4;
        public const int RtsMaxFactions = 2000;

        public static readonly RotationQuarterion FaceCameraRotation = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(0, 1.2f, 0f));
        public static readonly RotationQuarterion FaceForwardRotation = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(0, MathExt.TauOver4, 0f));
        public static readonly RotationQuarterion OverviewIconRotation = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(-0.4f, 0f, 0f));
        public static readonly RotationQuarterion BannerRotation = new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(-0.3f, 0f, 0f));


        public const int MercenaryPurchaseCount = 150;

        public const int DefaultMaxCommand = 2;
        public const double DefaultCommandPerSecond = 1.0 / 60.0;
        public const double NobleHouseAddCommand = 1.0 / 120.0;
        public const double NobleHouseAddMaxCommand = 0.5;

        //public const int DefaultMaxDiplomacy = 4;
        //public const double DefaultDiplomacyPerSecond = 1.0 / 60.0;
        //public const double NobelHouseAddDiplomacy = 1.0 / 240.0;
        //public const double NobelHouseAddMaxDiplomacy = 0.25;

        public static readonly SoldierConscriptProfile SoldierProfile_Farmer = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.SharpStick,
                armorLevel = Resource.ItemResourceType.PaddedArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_Swordsman = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.IronArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_Dwarf = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.FullPlateArmor,
                training = TrainingLevel.Skillful,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_Sailor = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.IronArmor,
                training = TrainingLevel.Skillful,
                specialization = SpecializationType.Sea,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_Pikeman = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Pike,
                armorLevel = Resource.ItemResourceType.FullPlateArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.AntiCavalry,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_Knight = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.KnightsLance,
                armorLevel = Resource.ItemResourceType.FullPlateArmor,
                training = TrainingLevel.Skillful,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_FootKnight = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.TwoHandSword,
                armorLevel = Resource.ItemResourceType.FullPlateArmor,
                training = TrainingLevel.Skillful,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_StandardArcher = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.LongBow,
                armorLevel = Resource.ItemResourceType.PaddedArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_StandardBallista = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Ballista,
                armorLevel = Resource.ItemResourceType.PaddedArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.Siege,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_CrossbowMan = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Crossbow,
                armorLevel = Resource.ItemResourceType.IronArmor,
                training = TrainingLevel.Basic,
                specialization = SpecializationType.Traditional,
            }
        };

        public static readonly SoldierConscriptProfile SoldierProfile_HonorGuard = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.FullPlateArmor,
                training = TrainingLevel.Professional,
                specialization = SpecializationType.HonorGuard,
            }
        };
        public static readonly SoldierConscriptProfile SoldierProfile_GreenSoldier = new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.IronArmor,
                training = TrainingLevel.Professional,
                specialization = SpecializationType.Green,
            }
        };
        public static readonly SoldierConscriptProfile SoldierProfile_Viking= new SoldierConscriptProfile()
        {
            conscript = new ConscriptProfile()
            {
                weapon = Resource.ItemResourceType.Sword,
                armorLevel = Resource.ItemResourceType.IronArmor,
                training = TrainingLevel.Skillful,
                specialization = SpecializationType.Viking,
            }
        };

        public const int TruceTimeSec = 180;

       
        public const int NobleHouseCost = 4000;
        public const float NobleHouseUpkeep = 10;

        public const float BattleConflictRadius = 2f;
        public const int BattleChainConflictRadius = 3;
        public const float CityDominationRadius = BattleConflictRadius + 1.5f;


        //public static readonly UnitType[] AvailableUnitTypes = new UnitType[]
        //    {
        //        UnitType.Folkman,
        //        UnitType.Soldier,
        //        UnitType.Sailor,
        //        UnitType.Knight,
        //        UnitType.Archer,
        //        UnitType.Ballista,
        //    };

        public const int GroupDefaultCost = 340;
        public const int GroupDefaultCultureCostReduction = 20;
        public const int GroupMinCost = 20;
        public const float SoldierDefaultUpkeep = 1f;
        public static float SoldierDefaultEnergyUpkeep = DssConst.ManDefaultEnergyCost;
        public static float GroupDefaultUpkeep = SoldierDefaultUpkeep * DssConst.SoldierGroup_DefaultCount;
        public const int DefalutRecruitTrainingTimeSec = 3 * 60;
                

        public const float WeeklyArmyActionPoints = 0.05f; 
        public const float ArmyMoveDoubleTimeCostToFatigue = 0.1f;
        public const float ArmyWeeklyRest = 0.002f;
        public const float ArmyWeeklyCityTraining = 0.010f;
        public const float ArmyMaxCityTraining = -0.20f;

        //public const int SmallCityStartWorkForce = AbsSoldierData.GroupDefaultCount * 4;
        //public const int LargeCityStartWorkForce = AbsSoldierData.GroupDefaultCount * 6;
        //public const int HeadCityStartWorkForce = AbsSoldierData.GroupDefaultCount * 10;

        

        public static readonly int NobelHouseWorkForceReqiurement = DssConst.HeadCityStartMaxWorkForce;

        public const float ShipBuildTimeSec = 5f;
        public const float ShipExitTimeSec = 3f;
        public const float BattleMaxQueTimeMs = 2000;

        #region OVERVIEW_LAYERS

        public const float BattleIconYpos = ArmyIconMaxYpos + 0.1f;
        public const float ArmyIconMaxYpos = ArmyIconMinYpos + 0.4f;
        public const float ArmyIconMinYpos = CityIconYpos + 0.3f;
        public const float CityIconYpos = OverviewMapYpos + 0.1f;
        public const float OverviewMapYpos = 0.4f;
#endregion

      

        public static string MoneyToString(int amount)
        {
            return amount.ToString() + "gold";
        }

        
    }

    enum MapSize { Tiny, Small, Medium, Large, Huge, Epic, NUM }

    enum SoldierTransformType
    { 
        TraningComplete,
        ToShip,
        FromShip,
    }
    enum AiAggressivity
    { 
        /// <summary>
        /// Just randomized
        /// </summary>
        Low,

        /// <summary>
        /// Follows players position, goes aggresive when attacked
        /// </summary>
        Medium,

        /// <summary>
        /// Makes sure to prioritize players as target, ai players protect eachother against players
        /// </summary>
        High,


        NUM
    }

    enum AiConscript
    { 
        Default,
        Orcs,
        Green,
        Viking,
        DragonSlayer,
    }

    enum BossSize
    {
        Small,
        Medium,
        Large,
        Huge,

        NUM
    }

    enum CityCulture
    { 
        LargeFamilies,//
        FertileGround,//
        Archers,//
        Warriors,//
        AnimalBreeder,//
        Miners,//
        Woodcutters,//
        Builders,//
        CrabMentality,// //ingen vill bli expert
        DeepWell,//
        Networker,//
        PitMasters,//

        Stonemason,//.
        Brewmaster,//.
        Weavers,//.
        SiegeEngineer,//.
        Armorsmith,//.
        Noblemen,//.
        Seafaring,//.
        Backtrader,//.
        Lawbiding,//.

        Smelters,//
        BronzeCasters,//
        Apprentices,//

        NUM_NONE
    }
}




