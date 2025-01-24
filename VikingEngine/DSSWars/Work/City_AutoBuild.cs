using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Work;

using VikingEngine.ToGG.MoonFall;
using VikingEngine.DSSWars.Players;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.GameObject
{
    //AUTO BUILD
    partial class City
    {
        static ForXYEdgeLoopRandomPicker Auto_EdgeRandomizer = new ForXYEdgeLoopRandomPicker();
        static List<BuildAndExpandType> AutoBuildList = new List<BuildAndExpandType>(4);
        static RandomObjects_Int AutoBuild_RandomBuild = new RandomObjects_Int();
        static List<BuildAndExpandType> AutoBuild_available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
        //static List<BuildAndExpandType> AutoBuild_available_mustInclude = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

        public bool automateCity = false;
        public AutomationFocus automationFocus = AutomationFocus.NoFocus;

        protected void workAutoBuild(bool fuelSafeGuard, bool rawFoodSafeGuard)
        {
            //EMPTY
            if (checkAutoBuildAvailable())
            {
                AutoBuildList.Clear();

                BuildAndExpandType safeGuardBuild = BuildAndExpandType.NUM_NONE;
                if (fuelSafeGuard && CityStructure.WorkInstance.fuelSpots < 4)
                {
                    ++CityStructure.WorkInstance.fuelSpots;
                    safeGuardBuild = BuildAndExpandType.RapeSeedFarm;
                }
                else if (rawFoodSafeGuard && CityStructure.WorkInstance.foodspots < 4)
                {
                    ++CityStructure.WorkInstance.foodspots;
                    safeGuardBuild = BuildAndExpandType.WheatFarm;
                }

                if (safeGuardBuild != BuildAndExpandType.NUM_NONE)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        AutoBuildList.Add(safeGuardBuild);
                    }
                }
                else if (faction.player.IsAi())
                {
                    var aiPlayer = faction.player.GetAiPlayer();
                    automationFocus = AutomationFocus.NoFocus;

                    bool warCity = aiPlayer.IsWarBorderCity(this, aiPlayer.aggressionLevel < AbsPlayer.AggressionLevel2_RandomAttacks);
                    if (warCity)
                    {
                        automationFocus = AutomationFocus.Military;
                    }
                    commit_automateCityBuilding();

                }
                else if (automateCity)
                {
                    commit_automateCityBuilding();
                }
                else //Player default
                {

                    AutoExpandType(out bool work, out Build.BuildAndExpandType buildType);
                    if (work)
                    {
                        buildType = autoBuild_Farm ? autoExpandFarmType : Build.BuildAndExpandType.NUM_NONE;
                        
                        if (work && workForce.amount >= workForceMax)
                        {
                            buildType = BuildAndExpandType.WorkerHuts;
                        }
                        
                        if (buildType != BuildAndExpandType.NUM_NONE)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                AutoBuildList.Add(buildType);
                            }
                        }
                    }
                }

                int buildCount = lib.SmallestValue(AutoBuildList.Count, CityStructure.WorkInstance.EmptyLand.Count);

                for (int i = 0; i < buildCount; ++i)
                {
                    var buildType = AutoBuildList[i];
                    
                    var pos = CityStructure.WorkInstance.EmptyLand[i];
                    if (this.buildingStructure.getCount(buildType) > 0)
                    {
                        var prevPos = CityStructure.WorkInstance.buildingPosition.getPos(buildType);
                        if (prevPos.X > 0)
                        {
                            findAdjacentFreeSpot(prevPos, ref pos);
                        }
                    }
                    if (BuildLib.BuildOptions[(int)buildType].blueprint.available(this) &&
                        work_isFreeTile(pos))
                    {
                        workQue.Add(new WorkQueMember(WorkType.Build, (int)buildType, 0, pos, workTemplate.autoBuild.value, 0, 0));
                    }
                    
                }
            }

            void findAdjacentFreeSpot(IntVector2 center, ref IntVector2 result)
            {
                for (int r = 1; r <= 2; r++)
                {
                    Auto_EdgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(center, r));

                    while (Auto_EdgeRandomizer.Next())
                    {
                        if (CityStructure.WorkInstance.MayAutoBuildHere(this, Auto_EdgeRandomizer.Position))
                        {
                            result = Auto_EdgeRandomizer.Position;
                            return;
                        }
                    }
                }
            }

            bool checkAutoBuildAvailable()
            {
                if (buildingStructure.buildingLevel_logistics < 2)
                {
                    var p = faction.player.GetLocalPlayer();
                    if (p != null)
                    {
                        return p.orders.buildQueue(this) + 1 < MaxBuildQueue();
                    }
                }
                return true;
            }
        }

        private void commit_automateCityBuilding()
        {
            auto_updateWorkPrio();

            AutoBuild_available.Clear();
            //AutoBuild_available_mustInclude.Clear();
            AutoBuild_RandomBuild.clear();

            BuildLib.AvailableBuildTypes(AutoBuild_available, this);

            int pickCount = lib.SmallestValue(AutoBuild_available.Count, 4);

            switch (automationFocus)
            {
                case AutomationFocus.Grow:
                    auto_addBuildingType(BuildAndExpandType.WorkerHuts);
                    auto_addBuildingType(BuildAndExpandType.WheatFarm);
                    break;
                case AutomationFocus.Export:
                    auto_addBuildingType(BuildAndExpandType.Postal);
                    break;
                case AutomationFocus.Military:
                    auto_addBuildingType(BuildAndExpandType.SoldierBarracks);
                    auto_addBuildingType(BuildAndExpandType.ArcherBarracks);
                    auto_addBuildingType(BuildAndExpandType.WarmashineBarracks);
                    break;
            }

            while (AutoBuild_available.Count > 0 && AutoBuild_RandomBuild.members.Count < 8)
            {
                var buildType = arraylib.RandomListMemberPop(AutoBuild_available);
                auto_addBuildingType(buildType);
            }

            pickCount = lib.SmallestValue(AutoBuild_RandomBuild.members.Count, 2);

            for (int i = 0; i < pickCount; ++i)
            {
                AutoBuildList.Add((BuildAndExpandType)AutoBuild_RandomBuild.PullRandom());
            }
        }

        private void auto_addBuildingType(BuildAndExpandType buildType)
        {
            bool bBuild = true;
            int chance = 100;
            int maxCount = 4;
            int repeat = 1;

            if (BuildLib.BuildOptions[(int)buildType].canAutoBuild)
            {
                switch (buildType)
                {
                    case BuildAndExpandType.WorkerHuts:
                        maxCount = 100;
                        chance = 200;
                        repeat = 4;
                        break;
                    case BuildAndExpandType.SoldierBarracks:
                    case BuildAndExpandType.ArcherBarracks:
                    case BuildAndExpandType.WarmashineBarracks:
                    case BuildAndExpandType.KnightsBarracks:
                    case BuildAndExpandType.GunBarracks:
                    case BuildAndExpandType.CannonBarracks:
                        maxCount = 2;
                        chance = automationFocus == AutomationFocus.Military ? 100 : 5;
                        break;

                    case BuildAndExpandType.CoalPit:
                    case BuildAndExpandType.WorkBench:
                        chance = 200;
                        break;

                    case BuildAndExpandType.WheatFarm:
                    case BuildAndExpandType.LinenFarm:
                    case BuildAndExpandType.HenPen:
                    case BuildAndExpandType.PigPen:
                    case BuildAndExpandType.RapeSeedFarm:
                    case BuildAndExpandType.HempFarm:
                        chance = automationFocus == AutomationFocus.Grow ? 300 : 150;
                        maxCount = 24;
                        break;
                                            
                    case BuildAndExpandType.Postal:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 60;
                            maxCount = 24;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 8;
                        }
                        break;
                    case BuildAndExpandType.Recruitment:
                        if (automationFocus == AutomationFocus.Export)
                        {
                            chance = 200;
                            maxCount = 12;
                        }
                        else
                        {
                            chance = 40;
                            maxCount = 4;
                        }
                        break;

                    case BuildAndExpandType.Foundry:
                        chance = 20;
                        maxCount = 2;
                        break;
                }

                if (bBuild)
                {
                    var opt = BuildLib.BuildOptions[(int)buildType];
                    if (opt.blueprint.hasResources_buildAndUpgrade(this))
                    {
                        int currentCount = this.buildingStructure.getCount(buildType);

                        if (currentCount == 0)
                        {
                            chance /= 4;
                        }

                        if (currentCount < maxCount)
                        {
                            repeat = Ref.rnd.Int(repeat) + 1;
                            for (int i = 0; i < repeat; ++i)
                            {
                                AutoBuild_RandomBuild.AddItem((int)buildType, chance);
                            }
                        }
                    }
                }
            }
        }

        public void AutoExpandType(out bool work, out Build.BuildAndExpandType farm)
        {
            work = autoBuild_Work;

            if (buildingStructure.buildingLevel_logistics == 0)
            {
                farm = Build.BuildAndExpandType.NUM_NONE;
                return;
            }

            farm = autoBuild_Farm ? autoExpandFarmType : Build.BuildAndExpandType.NUM_NONE;
        }

        void auto_updateWorkPrio()
        {
            int weaponPrio = automationFocus== AutomationFocus.Military ? 2 : 1;

            workTemplate.move.set(3);
            workTemplate.wood.set(2);
            workTemplate.stone.set(2);
            workTemplate.craft_fuel.set(4);
            workTemplate.craft_food.set(4);
            workTemplate.craft_beer.set(1);
            workTemplate.craft_coolingfluid.set(1);

             workTemplate.craft_iron.set(1);
            workTemplate.craft_tin.set(1);
            workTemplate.craft_cupper.set(1);
            workTemplate.craft_lead.set(1);
            workTemplate.craft_silver.set(1);

            workTemplate.craft_bronze.set(1);
            workTemplate.craft_castiron.set(1);
            workTemplate.craft_bloomeryiron.set(1);
            workTemplate.craft_steel.set(1);
            workTemplate.craft_mithril.set(1);

            workTemplate.craft_toolkit.set(1);
            workTemplate.craft_wagonlight.set(1);
            workTemplate.craft_wagonheavy.set(1);
            workTemplate.craft_blackpowder.set(1);
            workTemplate.craft_gunpowder.set(1);
            workTemplate.craft_bullet.set(1);

            workTemplate.craft_sharpstick.set(weaponPrio);
            workTemplate.craft_bronzesword.set(weaponPrio);
            workTemplate.craft_shortsword.set(weaponPrio);
            workTemplate.craft_sword.set(weaponPrio);
            workTemplate.craft_longsword.set(weaponPrio);
            workTemplate.craft_handspear.set(weaponPrio);
            workTemplate.craft_mithrilsword.set(weaponPrio);
            workTemplate.craft_warhammer.set(weaponPrio);
            workTemplate.craft_twohandsword.set(weaponPrio);
            workTemplate.craft_knightslance.set(weaponPrio);

            workTemplate.craft_slingshot.set(weaponPrio);
            workTemplate.craft_throwingspear.set(weaponPrio);
            workTemplate.craft_bow.set(weaponPrio);
            workTemplate.craft_longbow.set(weaponPrio);
            workTemplate.craft_crossbow.set(weaponPrio);
            workTemplate.craft_mithrilbow.set(weaponPrio);

            workTemplate.craft_handcannon.set(weaponPrio);
            workTemplate.craft_handculverin.set(weaponPrio);
            workTemplate.craft_rifle.set(weaponPrio);
            workTemplate.craft_blunderbus.set(weaponPrio);

            workTemplate.craft_ballista.set(weaponPrio);
            workTemplate.craft_manuballista.set(weaponPrio);
            workTemplate.craft_catapult.set(weaponPrio);
            workTemplate.craft_batteringram.set(weaponPrio);

            workTemplate.craft_siegecannonbronze.set(weaponPrio);
            workTemplate.craft_mancannonbronze.set(weaponPrio);
            workTemplate.craft_siegecannoniron.set(weaponPrio);
            workTemplate.craft_mancannoniron.set(weaponPrio);

            workTemplate.craft_paddedarmor.set(weaponPrio);
            workTemplate.craft_heavypaddedarmor.set(weaponPrio);
            workTemplate.craft_bronzearmor.set(weaponPrio);
            workTemplate.craft_mailarmor.set(weaponPrio);
            workTemplate.craft_heavymailarmor.set(weaponPrio);
            workTemplate.craft_platearmor.set(weaponPrio);
            workTemplate.craft_fullplatearmor.set(weaponPrio);
            workTemplate.craft_mithrilarmor.set(weaponPrio);

            workTemplate.farm_food.set(4);
            workTemplate.farm_fuel.set(3);
            workTemplate.farm_linen.set(weaponPrio);
            workTemplate.bogiron.set(1);
            workTemplate.mining_iron.set(3);
            workTemplate.mining_tin.set(1);
            workTemplate.mining_cupper.set(1);
            workTemplate.mining_lead.set(1);
            workTemplate.mining_silver.set(2);
            workTemplate.mining_gold.set(2);
            workTemplate.mining_mithril.set(2);
            workTemplate.mining_sulfur.set(1);
            workTemplate.mining_coal.set(1);

            workTemplate.autoBuild.set(1);
        }

        public bool AutomateCityProperty(int index, bool set, bool value)
        {
            if (set)
            {
                automateCity = value;
                (value ? SoundLib.click : SoundLib.back).Play();
            }
            return automateCity;
        }
    }

    enum AutomationFocus
    { 
        NO_AUTO,
        NoFocus,
        Grow,
        Export,
        Military,
        LevelUp,
        //Random,
    }
}
