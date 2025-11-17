using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.Characters.Monsters;
using VikingEngine.PJ;
using VikingEngine.Timer;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Players.PlayerControls
{

    class Tutorial
    {
         

        enum TutorialMission
        {
            CollectResources,
            
            Linen,
            //SharpStickWork,
            ProduceWeaponsArmor,
            CasualBuildBarracks,
            CasualRecruitSoldier,
            ConscriptArmy,
            CollectFood,
            //RecruitGuard,
            
            MoveArmy,
            AttackBarbarian,
            Diplomatics,

            EndTutorial,

            //Start of Advisor
            TagFoodCity,
            LogisticsUpgrade,
            EducateBurner,
            SendFood,

            FindWoodCity,
            FletcherPractice,
            FletcherPracticeWait,
            ProduceBow,
            BuildDefences,
            RecruitGuard,

            FindIronCity,
            ProduceSword,
            ProduceMail,

            EndAdvisor,
        }

        int tutorialLength = -1;
        List2<TutorialMission> missions;

        const int CollectWoodStoneAmount = 30;
        const int CollectLinenAmount = 15;
        static int CollectWeaponArmorAmount = DssConst.SoldierGroup_DefaultCount * 2;
        const int CollectGuardResources = 10;
        static int ProduceIronAmount = 40;

        bool collectResources_zoomIn = false;
        bool collectResources_zoomIn_sound = false;
        bool collectResources_selectCity = false;
        bool collectResources_selectCity_sound = false;
        bool collectResources_selectTab = false;
        bool collectResources_selectTab_sound = false;
        bool collectResources_collectwood = false;
        bool collectResources_collectstone = false;

        bool CasualBuildBarracks_selectCity = false;
        bool CasualBuildBarracks_selectCity_sound = false;
        bool CasualBuildBarracks_selectTab = false;
        bool CasualBuildBarracks_selectTab_sound = false;
        bool CasualBuildBarracks_build = false;
        

        bool casualRecruit_selectTab = false;
        bool casualRecruit_selectTab_sound = false;
        bool casualRecruit_recruit = false;
        
        bool linen_selectTab = false;
        bool linen_build = false;
        bool linen_collect = false;

        bool weaponsArmor_selectTab = false;
        bool weaponsArmor_selectTab_Sound = false;
        bool weaponsArmor_selectSubTab = false;
        bool weaponsArmor_selectSubTab_Sound = false;
        bool weaponsArmor_setWeaponPrio = false;
        bool weaponsArmor_setArmorPrio = false;
        bool weaponsArmor_produceArmor = false;
        bool weaponsArmor_produceWeapons = false;

        bool conscriptArmy_build = false;
        bool conscriptArmy_selectTab = false;
        bool conscriptArmy_createArmy = false;

        bool CollectFood_selecttab = false;
        bool CollectFood_foodblueprint = false;
        bool CollectFood_buildfoodproduction = false;
        bool CollectFood_buildfuelproduction = false;
        bool CollectFood_builcook = false;
        bool CollectFood_selectStockPile = false;
        bool CollectFood_increasefoodbuffer = false;
        bool CollectFood_reachfoodamount = false;

        bool moveArmy_ZoomOut = false;
        bool moveArmy_ZoomOut_sound = false;
        bool moveArmy_SelectMove = false;

        Army barbarianArmy = null;
        bool attackBarbarian_win = false;

        bool diplomatics_ZoomOut = false;
        bool diplomatics_ZoomOut_sound = false;
        bool diplomatics_goodRelation = false;


        //ADVISOR

        
        //TagCity
        TwoBools tagCity_selectCity_sound = TwoBools.False;
        TwoBools tagCity_tagTab_sound = TwoBools.False;
        TwoBools tagCity_foodTag_sound = TwoBools.False;
        TwoBools tagCity_subTabTag_sound = TwoBools.False;
        TwoBools tagCity_rawFoodToHud_sound = TwoBools.False;
        TwoBools tagCity_fuelToHud_sound = TwoBools.False;
        TwoBools tagCity_foodToHud_sound = TwoBools.False;

        //LogisticsUpgrade,
        //bool logisticsUpgrade_selectTab = false;
        TwoBools logisticsUpgrade_collectFood_sound = TwoBools.False;
        TwoBools logisticsUpgrade_build_sound = TwoBools.False;

        //EducateBurner,
        TwoBools educateBurner_buildSchool = TwoBools.False;
        TwoBools educateBurner_buildCoalPit = TwoBools.False;
        TwoBools educateBurner_schoolTab = TwoBools.False;
        TwoBools educateBurner_educateFuel = TwoBools.False;
        
        
        //SendFood,
        TwoBools sendFood_buildPostal = TwoBools.False;
        TwoBools sendFood_selectTab = TwoBools.False;
        TwoBools sendFood_postalQueue = TwoBools.False;

        //FindWoodCity,
        TwoBools findWoodCity_selectCity_sound = TwoBools.False;
        TwoBools findWoodCity_selectTab_sound = TwoBools.False;
        TwoBools findWoodCity_bowTag_sound = TwoBools.False;
        
        //FletcherPractice
        TwoBools fletcherPractice_buildWorkBench_sound = TwoBools.False;
        TwoBools fletcherPractice_resourceTab_sound = TwoBools.False;
        TwoBools fletcherPractice_setSlingerTo3_sound = TwoBools.False;
        //TwoBools fletcherPractice_setJavelinTo3_sound = TwoBools.False;
        TwoBools fletcherPractice_setBowTo4_sound = TwoBools.False;

        //TwoBools fletcherPractice_fletcherLevel2_sound = TwoBools.False;

        //FletcherPracticeWait
        TwoBools fletcherPracticeWait_experienceTab_sound = TwoBools.False;
        TwoBools fletcherPracticeWait_skillToHud_sound = TwoBools.False;
        TwoBools fletcherPracticeWait_progressToHud_sound = TwoBools.False;
        TwoBools fletcherPracticeWait_fletcherLevel2_sound = TwoBools.False;

        //ProduceBow,
        bool produceBow_buildLogistics = false;
        TwoBools produceBow_buildCarpenter = TwoBools.False;
        TwoBools produceBow_blackmarketTab = TwoBools.False;
        TwoBools produceBow_buyIron = TwoBools.False;
        TwoBools produceBow_produceBow = TwoBools.False;

        //BuildDefences,
        bool buildDefences_selectBuildTab = false;
        bool buildDefences_selectBuildTab_sound = false;
        bool buildDefences_buildPalisade = false;
        bool buildDefences_moveGuard = false;

        //RecruitGuard,
        //bool recruitGuard_zoomIn = false;
        //bool recruitGuard_zoomIn_sound = false;
        //bool recruitGuard_selectCity = false;
        //bool recruitGuard_selectCity_sound = false;
        TwoBools recruitGuard_buildBarracks = TwoBools.False;
        bool recruitGuard_selectConscriptTab = false;
        bool recruitGuard_selectConscriptTab_sound = false;
        bool recruitGuard_selectGuardTab = false;
        bool recruitGuard_selectGuardTab_sound = false;
        bool recruitGuard_createGuard = false;

        //    FindIronCity,
        TwoBools findIronCity_selectCity_sound = TwoBools.False;
        TwoBools findIronCity_Tag_sound = TwoBools.False;
        TwoBools findIronCity_buildSmelter_sound = TwoBools.False;
        //TwoBools findIronCity_increasePriority_sound = TwoBools.False;
        TwoBools findIronCity_produceIron_sound = TwoBools.False;

        //    ProduceSword,
        TwoBools produceSword_level2smith_sound = TwoBools.False;
        TwoBools produceSword_buildSmith_sound = TwoBools.False;
        TwoBools produceSword_swordPriority_sound = TwoBools.False;
        TwoBools produceSword_produceSword_sound = TwoBools.False;

        //    ProduceMail,
        TwoBools produceMail_level2Armorer_sound = TwoBools.False;
        TwoBools produceMail_buildArmorer_sound = TwoBools.False;
        TwoBools produceMail_mailPriority_sound = TwoBools.False;
        TwoBools produceMail_produceMail_sound = TwoBools.False;

        //    EndAdvisor,

        //Maybe projectile progress


        Rectangle2 cityarea;

        //        (hide tavern)
        //-look at the food blueprint
        //-build something that produces raw food
        //-build something that produces fuel
        //-build a food crafting station
        //-increase the food buffer limit
        //-reach a stockpile of X food
        //*The workers will move to the city hall for food

        LocalPlayer player;
        //TutorialMission tutorialMission = 0;
        Interface.TutorialDisplay display;

        public List<MenuTab> cityTabs;
        const int ReachFoodBuffer = City.DefaultFoodBuffer + 100;

        public List<BuildAndExpandType> AvailableBuildTypes()
        {
            var list = new List<BuildAndExpandType>(){
                BuildAndExpandType.WorkerHut,
                BuildAndExpandType.ServiceHouse_Small,
                BuildAndExpandType.SoldierBarracks,
                BuildAndExpandType.Palisade,
      
                //BuildAndExpandType.Brewery,
                //BuildAndExpandType.Cook,
                BuildAndExpandType.CoalPit,
                BuildAndExpandType.WorkBench,
                //BuildAndExpandType.Smith,

                //BuildAndExpandType.PigPen,
                BuildAndExpandType.HenPen,
                BuildAndExpandType.WheatFarm,
                BuildAndExpandType.LinenFarm,
            };

            if (missions.sel >= TutorialMission.CollectFood)
            {
                list.Insert(4, BuildAndExpandType.Cook);
                list.Add(BuildAndExpandType.RapeSeedFarm);
            }



            return list;
        }

        

        void initMissions()
        {
            if (player.profile.casualControls)
            {
                missions = new List2<TutorialMission>
                {
                    TutorialMission.CasualBuildBarracks,
                    TutorialMission.CasualRecruitSoldier,
                    TutorialMission.MoveArmy,
                    TutorialMission.AttackBarbarian,
                    TutorialMission.Diplomatics,
                    TutorialMission.EndTutorial,
                };
            }
            else
            {
                missions = new List2<TutorialMission>
                {
                    TutorialMission.CollectResources,
                    TutorialMission.Linen,
                    TutorialMission.ProduceWeaponsArmor,
                    TutorialMission.ConscriptArmy,
                    TutorialMission.CollectFood,
                    TutorialMission.MoveArmy,
                    TutorialMission.AttackBarbarian,
                    TutorialMission.Diplomatics,
                    TutorialMission.EndTutorial,
                };

                tutorialLength = missions.Count;

                missions.AddRange(new List<TutorialMission>
                {
                    //Start of Advisor
                    TutorialMission.TagFoodCity,
                    TutorialMission.LogisticsUpgrade,
                    TutorialMission.EducateBurner,
                    TutorialMission.SendFood,

                    TutorialMission.FindWoodCity,
                    TutorialMission.FletcherPractice,
                    TutorialMission.FletcherPracticeWait,
                    TutorialMission.ProduceBow,
                    //TutorialMission.BuildDefences,
                    TutorialMission.RecruitGuard,

                    TutorialMission.FindIronCity,
                    TutorialMission.ProduceSword,
                    TutorialMission.ProduceMail,

                    TutorialMission.EndAdvisor,
                });

            }
            missions.selectFirst();
        }

        public Tutorial(LocalPlayer player)
        {
            DssRef.storage.runTutorial = true;
            cityarea = new Rectangle2();

            this.player = player;
            player.hud.minimapProperty(null, true, false);
            display = new Interface.TutorialDisplay(player);
            initMissions();
            

            //Setup resources and map
            var cityCounter = player.faction.cities.counter();
            while (cityCounter.Next())
            {
                cityCounter.sel.res_wood.amount = 0;
                cityCounter.sel.res_sharpstick.amount = CollectWeaponArmorAmount - 6;//30;
                cityCounter.sel.res_paddedArmor.amount = CollectWeaponArmorAmount - 6;

                //if (DssRef.storage.runTutorial_1short_2normal == 1)
                //{
                //    cityCounter.sel.res_Palisade.amount = 50;
                //    cityCounter.sel.createStartupBarracks();
                //}


                CityStructure.WorkInstance.setupTutorialMap(cityCounter.sel);

                if (cityarea.X == 0)
                {
                    cityarea.pos = cityCounter.sel.tilePos;
                    cityarea.size = IntVector2.One;
                }
                else
                {
                    cityarea.includeTile(cityCounter.sel.tilePos);
                }
            }

            player.faction.workTemplate.craft_sharpstick.value = 0;
            player.faction.workTemplate.craft_bow.value = 0;
            player.faction.workTemplate.craft_paddedarmor.value = 0;
            player.faction.refreshCityWork();
            
            refreshLimits();
            //new TimedAction0ArgTrigger(song, 3000);
        }

        //public void song()
        //{
        //    Ref.music.PlaySong(Data.Music.Tutorial, true);
        //}

        void refreshLimits()
        {
            player.gameControls.map.setCameraBounds(missions.sel < TutorialMission.MoveArmy, cityarea);

            cityTabs = new List<MenuTab>{ MenuTab.Info, MenuTab.Resources };

            //if (tutorialMission >= TutorialMission.ProduceWeaponsArmor)
            //{
            //    cityTabs.Add(MenuTab.Work);
            //}
            if (missions.sel >= TutorialMission.Linen)
            {
                cityTabs.Add(MenuTab.Build);
            }
            if (missions.sel >= TutorialMission.ConscriptArmy)
            {
                cityTabs.Add(MenuTab.Conscript);
            }
            if (missions.sel >= TutorialMission.CollectFood)
            {
                cityTabs.Add(MenuTab.BlackMarket);
            }

            player.hud.messages.blockFoodWarning(missions.sel < TutorialMission.CollectFood);
        }

        public bool DisplayResourseSubTabs()
        { 
            return missions.sel >= TutorialMission.ProduceWeaponsArmor;
        }

        public bool DisplayCompressedBuildTab()
        {
            return missions.sel <= TutorialMission.Linen;
        }
        

        public void tutorial_ToHud(RichBoxContent content)
        {
            int missionIndex = missions.selIndex;
            if (missions.sel < TutorialMission.EndTutorial)
            {
                content.h1(DssRef.lang.Tutorial_MissionsTitle);
            }
            else
            {
                missionIndex -= tutorialLength;
                content.h1(DssRef.todoLang.Tutorial_AdvisorMission);
            }

            content.h2(string.Format(DssRef.lang.Tutorial_MissionX, missionIndex + 1), HudLib.InfoYellow_Light);

            switch (missions.sel)
            {


                case TutorialMission.CollectResources:
                    
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(collectResources_selectCity)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialCity));
                    content.Add(new RbText("/"));
                    content.Add(new RbImage(SpriteName.WarsCityHall));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_SelectACity));

                    content.iconicontext(HudLib.CheckImage(collectResources_zoomIn), SpriteName.WarsWorker, DssRef.lang.Tutorial_ZoomInWorkers);
                    content.iconicontext(HudLib.CheckImage(collectResources_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    content.iconicontext(HudLib.CheckImage(collectResources_collectwood), SpriteName.WarsResource_Wood, string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.lang.Resource_TypeName_Wood));
                    content.iconicontext(HudLib.CheckImage(collectResources_collectstone), SpriteName.WarsResource_Stone, string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.lang.Resource_TypeName_Stone));
                    break;

                case TutorialMission.CasualBuildBarracks:
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(CasualBuildBarracks_selectCity)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialCity));
                    content.Add(new RbText("/"));
                    content.Add(new RbImage(SpriteName.WarsCityHall));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_SelectACity));

                    content.iconicontext(HudLib.CheckImage(CasualBuildBarracks_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.iconicontext(HudLib.CheckImage(CasualBuildBarracks_build), SpriteName.WarsBuild_Barracks, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, DssRef.lang.BuildingType_Barracks));
                    break;

                case TutorialMission.CasualRecruitSoldier:
                    content.iconicontext(HudLib.CheckImage(casualRecruit_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Recruit));
                    content.iconicontext(HudLib.CheckImage(casualRecruit_recruit), SpriteName.WarsUnitIcon_Folkman, DssRef.lang.Tutorial_CasualRecruitSoldiers);
                    break;
               
                case TutorialMission.Linen:
                    content.iconicontext(HudLib.CheckImage(linen_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.iconicontext(HudLib.CheckImage(linen_build), SpriteName.WarsBuild_LinenFarms, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.LinenFarm].Label()));
                    //content.icontext(HudLib.CheckImage(linen_armorWork), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_LightArmor));
                    content.iconicontext(HudLib.CheckImage(linen_collect), SpriteName.WarsResource_LinenCloth, string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectLinenAmount, DssRef.lang.Resource_TypeName_Linen));
                    break;

                case TutorialMission.ProduceWeaponsArmor:
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(weaponsArmor_selectSubTab)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsHammer));
                    var tabImg = new RbImage(SpriteName.WarsHudSubTabSelected);
                    content.Add(new RbOverlapImage(tabImg, SpriteName.WarsResource_Sword, Vector2.Zero, 0.8f));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Work)));
                    
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_setWeaponPrio), ResourceLib.Icon(ItemResourceType.SharpStick), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_SharpStick));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_setArmorPrio), ResourceLib.Icon(ItemResourceType.PaddedArmor), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_PaddedArmor));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_produceWeapons), ResourceLib.Icon(ItemResourceType.SharpStick), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_SharpStick));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_produceArmor), ResourceLib.Icon(ItemResourceType.PaddedArmor), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_PaddedArmor));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Tutorial_HighPriority, HudLib.InfoYellow_Light));
                    
                    break;


               

                case TutorialMission.ConscriptArmy:
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_build), SpriteName.WarsBuild_Barracks, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.SoldierBarracks].Label()));
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_createArmy), SpriteName.WarsUnitIcon_Folkman, string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.Resource_TypeName_PaddedArmor));
                    break;

                case TutorialMission.CollectFood:
                    {
                        content.iconicontext(HudLib.CheckImage(CollectFood_selecttab), SpriteName.MenuPixelIconManual,
                            string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources) + ". " + string.Format(DssRef.lang.Tutorial_Select_SubTab, DssRef.lang.Resource_Tab_Overview));
                        content.iconicontext(HudLib.CheckImage(CollectFood_foodblueprint), SpriteName.WarsBluePrint, DssRef.lang.Tutorial_LookAtFoodBlueprint);//-look at the food blueprint
                        content.iconicontext(HudLib.CheckImage(CollectFood_buildfoodproduction), SpriteName.WarsResource_RawFood, string.Format(DssRef.lang.Tutorial_BuildSomething, DssRef.lang.Resource_TypeName_RawFood));//-build something that produces raw food
                        content.iconicontext(HudLib.CheckImage(CollectFood_buildfuelproduction), SpriteName.WarsResource_Fuel, string.Format(DssRef.lang.Tutorial_BuildSomething, DssRef.lang.Resource_TypeName_Fuel));//-build something that produces fuel
                        content.iconicontext(HudLib.CheckImage(CollectFood_builcook), SpriteName.WarsBuild_Cook, string.Format(DssRef.lang.Tutorial_BuildCraft, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station

                        content.iconicontext(HudLib.CheckImage(CollectFood_selectStockPile), SpriteName.WarsStockpileAdd,
                            string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources) + ". " + string.Format(DssRef.lang.Tutorial_Select_SubTab, DssRef.lang.Resource_Tab_Stockpile));//-build a food crafting station

                        content.iconicontext(HudLib.CheckImage(CollectFood_increasefoodbuffer), SpriteName.WarsResource_Food, string.Format(DssRef.lang.Tutorial_IncreaseBufferLimit, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station
                        content.iconicontext(HudLib.CheckImage(CollectFood_reachfoodamount), SpriteName.WarsStockpileStop, string.Format(DssRef.lang.Tutorial_CollectItemStockpile, ReachFoodBuffer, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station

                        content.newLine();
                        HudLib.BulletPoint(content);
                        var info0 = new RbText(DssRef.lang.Tutorial_CollectFood_Info0);
                        info0.overrideColor = HudLib.InfoYellow_Light;
                        content.Add(info0);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        var info1 = new RbText(DssRef.lang.Tutorial_CollectFood_Info1);
                        info1.overrideColor = HudLib.InfoYellow_Light;
                        content.Add(info1);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.Tutorial_CollectFood_Info2, HudLib.InfoYellow_Light));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        City city = player.gameControls.map.selection.obj as City;
                        CraftResourceLib.Food1.toMenu(content, city, false, false, city != null, false);
                    }
                    break;
                
                case TutorialMission.MoveArmy:
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(moveArmy_ZoomOut)));
                    
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_ZoomOutOverview));

                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(moveArmy_SelectMove)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialArmy));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_Mission_MoveArmy));
                    break;

                case TutorialMission.AttackBarbarian:
                    content.iconicontext(HudLib.CheckImage(attackBarbarian_win), SpriteName.WarsRelationWar, string.Format( DssRef.lang.Tutorial_AttackAndDestroyX, DssRef.lang.FactionName_Barbarian));
                    
                    break;

                case TutorialMission.Diplomatics:
                    content.iconicontext(HudLib.CheckImage(diplomatics_ZoomOut), SpriteName.WarsDiplomaticPoint,  DssRef.lang.Tutorial_ZoomOutDiplomacy);
                    content.iconicontext(HudLib.CheckImage(diplomatics_goodRelation), SpriteName.WarsRelationGood, DssRef.lang.Tutorial_ImproveRelations);
                    break;
                                    
                case TutorialMission.TagFoodCity:
                    //TwoBools tagCity_selectCity_sound = TwoBools.False;
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(tagCity_selectCity_sound.Value1)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialCity));
                    content.Add(new RbText("/"));
                    content.Add(new RbImage(SpriteName.WarsCityHall));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_SelectACity + ". Not your capital."));
                   
                    //TwoBools tagCity_tagTab_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(tagCity_tagTab_sound.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Tag));
                    
                    //TwoBools tagCity_foodTag_sound = TwoBools.False;
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(tagCity_foodTag_sound.Value1)));
                    content.space();
                    content.Add(new RbText("Add tag:"));
                    content.space();
                    content.Add(new RbImage(SpriteName.warsFolder_carton));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsResource_Food));

                    //TwoBools tagCity_subTabTag_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(tagCity_subTabTag_sound.Value1), SpriteName.WarsHudSubTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.HudPins));
                    //TwoBools tagCity_rawFoodToHud_sound = TwoBools.False;
                    //TwoBools tagCity_fuelToHud_sound = TwoBools.False;
                    //TwoBools tagCity_foodToHud_sound = TwoBools.False;
                    addResourcePin(tagCity_rawFoodToHud_sound.Value1, ItemResourceType.RawFood_Group);
                    addResourcePin(tagCity_fuelToHud_sound.Value1, ItemResourceType.Fuel_G);
                    addResourcePin(tagCity_foodToHud_sound.Value1, ItemResourceType.Food_G);

                    

                    break;
                case TutorialMission.LogisticsUpgrade:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Food, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));
                    ////LogisticsUpgrade,
                    //content.iconicontext(HudLib.CheckImage(logisticsUpgrade_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    //TwoBools logisticsUpgrade_collectFood_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(logisticsUpgrade_collectFood_sound.Value1), SpriteName.WarsStockpileStop, string.Format(DssRef.lang.Tutorial_CollectItemStockpile, DssConst.Logistics1FoodStorage, DssRef.lang.Resource_TypeName_Food));
                    //TwoBools logisticsUpgrade_build_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(logisticsUpgrade_build_sound.Value1), SpriteName.WarsBuild_Logistics, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Logistics].Label()));
                    break;
                case TutorialMission.EducateBurner:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Food, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));
                    ////EducateBurner,
                    //TwoBools educateBurner_buildSchool = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(educateBurner_buildCoalPit.Value1), SpriteName.WarsBuild_CoalPit, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.CoalPit].Label()));
                    content.iconicontext(HudLib.CheckImage(educateBurner_buildSchool.Value1), SpriteName.WarsBuild_School, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.School].Label()));
                    
                    //TwoBools educateBurner_educateFuel = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(educateBurner_schoolTab.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Progress) + ". " + string.Format(DssRef.lang.Tutorial_Select_SubTab, DssRef.lang.BuildingType_School_Tab));
                    
                    content.iconicontext(HudLib.CheckImage(educateBurner_educateFuel.Value1), SpriteName.WarsBuild_School, "Produce:");
                    LangLib.ExperienceType(XP.WorkExperienceType.CraftFuel, out var xpName, out var xpIcon);
                    content.Add(new RbImage(xpIcon));
                    content.Add(new RbText(xpName));
                    HudLib.BulletSeperationPoint(content);
                    content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                    content.Add(new RbText(LangLib.ExperienceLevel(XP.ExperienceLevel.Practitioner_2)));

                    break;
                case TutorialMission.SendFood:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Food, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));

                    ////SendFood,
                    //TwoBools sendFood_buildPostal = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(sendFood_buildPostal.Value1), SpriteName.WarsBuild_Postal, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Postal].Label()));
                    //TwoBools sendFood_selectTab = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(sendFood_selectTab.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Delivery));
                    //TwoBools sendFood_postalQueue = TwoBools.False;
                    content.iconiconicontext(HudLib.CheckImage(sendFood_postalQueue.Value1), SpriteName.WarsBuild_Postal, SpriteName.WarsResource_Food, string.Format( "Send {0}", LangLib.Item(ItemResourceType.Food_G)));
                    break;

                case TutorialMission.FindWoodCity:

                    ////FindWoodCity,
                    //TwoBools findWoodCity_selectCity_sound = TwoBools.False;
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(findWoodCity_selectCity_sound.Value1)));
                    content.Add(new RbImage(SpriteName.WarsBuild_TreeSoft));
                    content.Add(new RbText("Find your city with the most trees"));

                    //TwoBools findWoodCity_selectTab_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(findWoodCity_selectTab_sound.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Tag));
                    //TwoBools findWoodCity_woodTag_sound = TwoBools.False;
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(findWoodCity_bowTag_sound.Value1)));
                    content.space();
                    content.Add(new RbText("Add tag:"));
                    content.space();
                    content.Add(new RbImage(SpriteName.warsFolder_carton));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsResource_Bow));
                    break;
                case TutorialMission.FletcherPractice:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Bow, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));
                    ////FletcherPractice
                    //TwoBools fletcherPractice_buildWorkBench_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(fletcherPractice_buildWorkBench_sound.Value1), SpriteName.WarsBuild_WorkBench, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.WorkBench].Label()));

                    content.iconicontext(HudLib.CheckImage(fletcherPractice_resourceTab_sound.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));

                    //TwoBools fletcherPractice_setSlingerTo3_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(fletcherPractice_setSlingerTo3_sound.Value1), ResourceLib.Icon(ItemResourceType.SlingShot), string.Format("Set {0} priority to {1}", DssRef.lang.Resource_TypeName_SlingShot, 3));
                    //TwoBools fletcherPractice_setJavelinTo3_sound = TwoBools.False;
                    //content.iconicontext(HudLib.CheckImage(fletcherPractice_setJavelinTo3_sound.Value1), ResourceLib.Icon(ItemResourceType.ThrowingSpear), string.Format("Set {0} priority to {1}", DssRef.lang.Resource_TypeName_ThrowingSpear, 3));
                    //TwoBools fletcherPractice_setBowTo4_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(fletcherPractice_setBowTo4_sound.Value1), ResourceLib.Icon(ItemResourceType.Bow), string.Format("Set {0} priority to {1}", DssRef.lang.Resource_TypeName_Bow, 4));

                    break;


                case TutorialMission.FletcherPracticeWait:
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Bow, Vector2.Zero));
                        content.Add(new RbText(DssRef.lang.UnitType_City));
                        ////FletcherPracticeWait
                        //TwoBools fletcherPracticeWait_experienceTab_sound = TwoBools.False;
                        content.iconicontext(HudLib.CheckImage(fletcherPracticeWait_experienceTab_sound.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Progress));
                        //TwoBools fletcherPracticeWait_progressToHud_sound = TwoBools.False;
                        LangLib.Technology(XP.TechnologyTreeType.catapult, out SpriteName catapultIcon, out string catapultCaption);
                        content.iconicontext(HudLib.CheckImage(fletcherPracticeWait_progressToHud_sound.Value1), SpriteName.HudPinIcon, "Add pin:");
                        content.Add(new RbImage(catapultIcon));
                        content.Add(new RbText(catapultCaption));
                        //TwoBools fletcherPracticeWait_skillToHud_sound = TwoBools.False;
                        LangLib.ExperienceType(XP.WorkExperienceType.Fletcher, out string fletcherCaption, out SpriteName fletcherIcon);
                        content.iconicontext(HudLib.CheckImage(fletcherPracticeWait_skillToHud_sound.Value1), SpriteName.HudPinIcon, "Add pin:");
                        content.space();
                        content.Add(new RbText(DssRef.lang.Experience_Title));
                        HudLib.BulletSeperationPoint(content);
                        content.Add(new RbImage(fletcherIcon));
                        content.Add(new RbText(fletcherCaption));

                        //TwoBools fletcherPracticeWait_fletcherLevel2_sound = TwoBools.False;
                        content.newLine();
                        content.Add(new RbImage(HudLib.CheckImage(fletcherPracticeWait_fletcherLevel2_sound.Value1)));
                        content.space();
                        content.Add(new RbText("Wait for a worker to reach:"));
                        content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                        content.Add(new RbText(LangLib.ExperienceLevel(XP.ExperienceLevel.Practitioner_2)));
                        HudLib.BulletSeperationPoint(content);
                        content.Add(new RbImage(fletcherIcon));
                        content.Add(new RbText(fletcherCaption));

                        content.newParagraph();
                        City city = player.gameControls.map.selection.obj as City;
                        content.Add(new RbImage(SpriteName.WarsBluePrint));
                        content.space();
                        CraftResourceLib.Slingshot.toMenu(content, city, false, false, city != null, false);

                        if (city != null)
                        {
                            var sligshot = city.GetGroupedResource(ItemResourceType.SlingShot);
                            content.newLine();
                            content.Add(new RbImage(SpriteName.WarsResource_Slingshot));
                            content.space();
                            content.Add(new RbImage(sligshot.amount >= sligshot.goalBuffer ? SpriteName.WarsStockpileStop : SpriteName.WarsStockpileAdd));
                            content.hspace();
                            content.Add(new RbText(DssRef.lang.Resource_Tab_Stockpile + ":"));
                            content.space();
                            content.Add(new RbText(string.Format("{0}/{1}", sligshot.amount, sligshot.goalBuffer), HudLib.InfoYellow_Light));
                        }
                    }
                    break;
                case TutorialMission.ProduceBow:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Bow, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));
                    ////ProduceBow,
                    //bool produceBow_buildLogistics = false;
                    content.iconicontext(HudLib.CheckImage(produceBow_buildLogistics), SpriteName.WarsBuild_Logistics, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Logistics].Label()));

                    //TwoBools produceBow_buildCarpenter = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(produceBow_buildCarpenter.Value1), SpriteName.WarsBuild_Carpenter, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Carpenter].Label()));

                    content.iconicontext(HudLib.CheckImage(produceBow_blackmarketTab.Value1 || produceBow_buyIron.Value1), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Hud_BlackMarket));

                    content.iconicontext(HudLib.CheckImage(produceBow_buyIron.Value1), SpriteName.WarsResource_Iron, string.Format(DssRef.lang.HudAction_BuyItem, DssRef.lang.Resource_TypeName_Iron));
                    //TwoBools produceBow_produceBow = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(produceBow_produceBow.Value1), ResourceLib.Icon(ItemResourceType.Bow), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectGuardResources, DssRef.lang.Resource_TypeName_Bow));
                    break;
                
                case TutorialMission.BuildDefences:
                    content.iconicontext(HudLib.CheckImage(buildDefences_selectBuildTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.iconicontext(HudLib.CheckImage(buildDefences_buildPalisade), SpriteName.WarsBuild_Palisade, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Palisade].Label()));
                    content.iconicontext(HudLib.CheckImage(buildDefences_moveGuard), SpriteName.WarsGuardPostIcon, DssRef.lang.Tutorial_GuardToWall);

                    break;

                case TutorialMission.RecruitGuard:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbOverlapImage(new RbImage(SpriteName.warsFolder_carton), SpriteName.WarsResource_Bow, Vector2.Zero));
                    content.Add(new RbText(DssRef.lang.UnitType_City));
                    //content.newLine();
                    //content.Add(new RbImage(HudLib.CheckImage(recruitGuard_selectCity)));
                    //content.space();
                    //content.Add(new RbImage(SpriteName.WarsTutorialCity));
                    //content.Add(new RbText("/"));
                    //content.Add(new RbImage(SpriteName.WarsCityHall));
                    //content.space();
                    //content.Add(new RbText(DssRef.lang.Tutorial_SelectACity));
                    ////content.icontext(HudLib.CheckImage(recruitGuard_selectCity), DssRef.lang.Tutorial_SelectACity);
                    //content.iconicontext(HudLib.CheckImage(recruitGuard_zoomIn), SpriteName.WarsWorker, DssRef.lang.Tutorial_ZoomInWorkers);
                    content.iconicontext(HudLib.CheckImage(recruitGuard_buildBarracks.Value1), SpriteName.WarsBuild_ArcherBarracks, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.ArcherBarracks].Label()));

                    content.iconicontext(HudLib.CheckImage(recruitGuard_selectConscriptTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.iconicontext(HudLib.CheckImage(recruitGuard_selectGuardTab), SpriteName.WarsGuard, string.Format(DssRef.lang.Tutorial_OpenGuardSubTab, DssRef.lang.Conscript_Soldiers_GuardType));
                    content.iconicontext(HudLib.CheckImage(recruitGuard_createGuard), SpriteName.WarsUnitIcon_Archer, string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_Bow, DssRef.lang.Resource_TypeName_PaddedArmor));

                    break;

                case TutorialMission.FindIronCity:
                    ////    FindIronCity,
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(findIronCity_selectCity_sound.Value1)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsResource_IronOre));
                    content.space();
                    content.Add(new RbText(string.Format("Select a city with {0}", DssRef.lang.Resource_TypeName_IronOre)));
                    content.newLine();
                    
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsBogIron));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_BogIron));

                    content.newLine();
                    content.space(2);
                    content.Add(new RbText( DssRef.lang.Hud_RequirementOr));
                    
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_Iron));
                    content.Add(new RbImage(SpriteName.WarsWorkMine));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ResourceMine, DssRef.lang.Resource_TypeName_Iron)));


                    content.newParagraph();
                    //TwoBools findIronCity_Tag_sound = TwoBools.False;
                    addResourcePin(findIronCity_Tag_sound.Value1, ItemResourceType.Iron_G);
                    //TwoBools findIronCity_buildSmelter_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(findIronCity_buildSmelter_sound.Value1), SpriteName.WarsBuild_Smelter, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Smelter].Label()));
                    //TwoBools findIronCity_produceIron_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(findIronCity_produceIron_sound.Value1), ResourceLib.Icon(ItemResourceType.Iron_G), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, ProduceIronAmount, DssRef.lang.Resource_TypeName_Iron));

                    break;
                case TutorialMission.ProduceSword:

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.HudPinIcon));
                    content.Add(new RbImage(SpriteName.WarsResource_Iron));
                    content.Add(new RbText(DssRef.lang.UnitType_City));


                    LangLib.ExperienceType(XP.WorkExperienceType.CraftMetal, out string smithCaption, out SpriteName smithIcon);
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(produceSword_level2smith_sound.Value1)));
                    content.space();
                    content.Add(new RbText("Wait for a worker to reach:"));
                    content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                    content.Add(new RbText(LangLib.ExperienceLevel(XP.ExperienceLevel.Practitioner_2)));
                    HudLib.BulletSeperationPoint(content);
                    content.Add(new RbImage(smithIcon));
                    content.Add(new RbText(smithCaption));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format("Practice on {0}, or use a {1}", DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.BuildingType_School), HudLib.InfoYellow_Light));
                    content.newParagraph();

                   
                    ////    ProduceSword,
                    //TwoBools produceSword_buildSmith_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(produceSword_buildSmith_sound.Value1), SpriteName.WarsBuild_Smith, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Smith].Label()));
                    //TwoBools produceSword_swordPriority_sound = TwoBools.False;
                    content.iconiconicontext(HudLib.CheckImage(produceSword_swordPriority_sound.Value1), SpriteName.WarsHammer, ResourceLib.Icon(ItemResourceType.ShortSword), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_ShortSword));
                    //TwoBools produceSword_produceSword_sound = TwoBools.False;
                    content.iconiconicontext(HudLib.CheckImage(produceSword_produceSword_sound.Value1), SpriteName.WarsStockpileAdd, ResourceLib.Icon(ItemResourceType.ShortSword), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_ShortSword));
                    break;
                case TutorialMission.ProduceMail:
                    
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.HudPinIcon));
                    content.Add(new RbImage(SpriteName.WarsResource_Iron));
                    content.Add(new RbText(DssRef.lang.UnitType_City));


                    LangLib.ExperienceType(XP.WorkExperienceType.CraftArmor, out string armorerCaption, out SpriteName armorerIcon);
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(produceMail_level2Armorer_sound.Value1)));
                    content.space();
                    content.Add(new RbText("Wait for a worker to reach:"));
                    content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                    content.Add(new RbText(LangLib.ExperienceLevel(XP.ExperienceLevel.Practitioner_2)));
                    HudLib.BulletSeperationPoint(content);
                    content.Add(new RbImage(armorerIcon));
                    content.Add(new RbText(armorerCaption));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format("Practice on {0}, or use a {1}", DssRef.lang.Resource_TypeName_PaddedArmor, DssRef.lang.BuildingType_School), HudLib.InfoYellow_Light));
                    content.newParagraph();

                    //TwoBools produceMail_buildArmorer_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(produceMail_buildArmorer_sound.Value1), SpriteName.WarsBuild_Armory, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Armory].Label()));
                    //TwoBools produceMail_mailPriority_sound = TwoBools.False;
                    content.iconiconicontext(HudLib.CheckImage(produceMail_mailPriority_sound.Value1), SpriteName.WarsHammer, ResourceLib.Icon(ItemResourceType.IronArmor), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_IronArmor));
                    //TwoBools produceMail_produceMail_sound = TwoBools.False;
                    content.iconicontext(HudLib.CheckImage(produceMail_produceMail_sound.Value1), ResourceLib.Icon(ItemResourceType.IronArmor), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_IronArmor));

                    break;

            }

            void addResourcePin(bool complete, ItemResourceType resourceType)
            {
                content.newLine();
                content.Add(new RbImage(HudLib.CheckImage(complete)));
                content.space();
                content.Add(new RbText("Add pin:"));
                content.space();
                content.Add(new RbImage(SpriteName.HudPinIcon));
                content.hspace();
                content.Add(new RbImage(ResourceLib.Icon(resourceType)));
                content.space();
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Resource, LangLib.Item(resourceType))));
            }
            //content.newParagraph();
            //content.icontext(player.gameControls.input.mouseSelect.Icon, DssRef.lang.Tutorial_SelectInput);            
            //content.icontext(player.gameControls.input.inputSource.IsController? player.gameControls.input.cameraTiltZoom.Icon : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
            //if (missions.sel == TutorialMission.MoveArmy ||
            //    missions.sel == TutorialMission.AttackBarbarian ||
            //    missions.sel == TutorialMission.BuildDefences)
            //{
            //    content.icontext(player.gameControls.input.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
            //}
        }

        public void update(ref bool mouseOverHud)
        {
#if DEBUG
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.T))
            {
                onMissionSuccess();
            }
#endif
            switch (missions.sel)
            {
                case TutorialMission.CasualBuildBarracks:
                    if (player.gameControls.map.selection.obj is City)
                    {
                        if (!CasualBuildBarracks_selectCity)
                        {
                            CasualBuildBarracks_selectCity = true;
                            onPartSuccess(CasualBuildBarracks_selectCity_sound);
                            CasualBuildBarracks_selectCity_sound = true;
                        }
                    }
                    else
                    {
                        if (CasualBuildBarracks_selectCity)
                        {
                            CasualBuildBarracks_selectCity = false;
                            display.refresh = true;
                        }
                    }

                    if (player.cityTab == Interface.MenuTab.Casual_Build)
                    {
                        if (!CasualBuildBarracks_selectTab)
                        {
                            CasualBuildBarracks_selectTab = true;
                            onPartSuccess(CasualBuildBarracks_selectTab_sound);
                            CasualBuildBarracks_selectTab_sound = true;
                        }
                    }
                    else
                    {
                        if (CasualBuildBarracks_selectTab)
                        {
                            CasualBuildBarracks_selectTab = false;
                            display.refresh = true;
                        }
                    }

                    if (!CasualBuildBarracks_build)
                    {
                        if (player.gameControls.map.selection.obj is City)
                        {
                            if (player.gameControls.map.selection.obj.GetCity().getCount(Casual.CasualBuildType.Barracks) > 0)
                            {
                                CasualBuildBarracks_build = true;
                                onPartSuccess(true);
                            }
                        }
                    }

                    break;

                case TutorialMission.CasualRecruitSoldier:
                    

                    if (player.cityTab == Interface.MenuTab.Casual_Recruit)
                    {
                        if (!casualRecruit_selectTab)
                        {
                            casualRecruit_selectTab = true;
                            onPartSuccess(casualRecruit_selectTab_sound);
                            casualRecruit_selectTab_sound = true;
                        }
                    }
                    else
                    {
                        if (casualRecruit_selectTab)
                        {
                            casualRecruit_selectTab = false;
                            display.refresh = true;
                        }
                    }

                    if (!casualRecruit_recruit && player.faction.armies.Count > 0)
                    {
                        casualRecruit_recruit = true;
                        onPartSuccess(true);
                    }

                    break;



                case TutorialMission.CollectResources:

                    if (player.gameControls.map.selection.obj is City)
                    {
                        if (!collectResources_selectCity)
                        {
                            collectResources_selectCity = true;
                            onPartSuccess(collectResources_selectCity_sound);
                            collectResources_selectCity_sound = true;
                        }
                    }
                    else
                    {
                        if (collectResources_selectCity)
                        {
                            collectResources_selectCity = false;
                            display.refresh = true;
                        }
                    }

                    if (player.mapLayersManager.current.DrawDetailLayer)
                    {
                        if (!collectResources_zoomIn)
                        {
                            collectResources_zoomIn = true;
                            onPartSuccess(collectResources_zoomIn_sound);
                            collectResources_zoomIn_sound = true;
                        }
                    }
                    else
                    {
                        if (collectResources_zoomIn)
                        {
                            collectResources_zoomIn = false;
                            display.refresh = true;
                        }
                    }
                    

                    
                    if (player.cityTab == Interface.MenuTab.Resources)
                    {
                        if (!collectResources_selectTab)
                        {
                            collectResources_selectTab = true;
                            onPartSuccess(collectResources_selectTab_sound);
                            collectResources_selectTab_sound = true;
                        }
                    }
                    else
                    {
                        if (collectResources_selectTab)
                        {
                            collectResources_selectTab = false;
                            display.refresh = true;
                        }
                    }
                    
                    if (!collectResources_collectwood)
                    {
                        if (player.gameControls.map.selection.obj is City)
                        {
                            if (player.gameControls.map.selection.obj.GetCity().GetGroupedResource(ItemResourceType.Wood_Group).amount >= CollectWoodStoneAmount)
                            {
                                player.faction.workTemplate.move.value = 2;
                                player.faction.workTemplate.wood.value = 2;
                                player.faction.workTemplate.stone.value = 4;

                                player.faction.refreshCityWork();


                                collectResources_collectwood = true;
                                onPartSuccess();
                            }
                        }
                    }
                    if (!collectResources_collectstone)
                    {
                        if (player.gameControls.map.selection.obj is City)
                        {
                            if (player.gameControls.map.selection.obj.GetCity().GetGroupedResource(ItemResourceType.Stone_G).amount >= CollectWoodStoneAmount)
                            {
                                collectResources_collectstone = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;

                
                case TutorialMission.Linen:

                    ((PlayState)DssRef.state).speedUpGrowing();

                    if (!linen_selectTab)
                    {
                        if (player.cityTab == Interface.MenuTab.Build)
                        {
                            linen_selectTab = true;
                            onPartSuccess();
                        }
                    }

                    if (!linen_build)
                    {
                        lock (player.orders.orders)
                        {
                            foreach (var order in player.orders.orders)
                            {
                                if (order is BuildOrder && ((BuildOrder)order).buildingType == Build.BuildAndExpandType.LinenFarm)
                                {
                                    linen_build = true;
                                    onPartSuccess();
                                    break;
                                }
                            }
                        }
                    }
                   
                    if (!linen_collect)
                    {
                        if (player.gameControls.map.selection.obj is City)
                        {
                            if (player.gameControls.map.selection.obj.GetCity().GetGroupedResource(ItemResourceType.SkinLinen_Group).amount >= CollectLinenAmount)
                            {
                                linen_collect = true;
                                onPartSuccess();
                            }
                        }
                    }
                    
                    break;

               
                case TutorialMission.ProduceWeaponsArmor:
                    //if (!weaponsArmor_selectTab)
                    //{
                    if (player.cityTab == Interface.MenuTab.Resources)
                    {
                        if (!weaponsArmor_selectTab)
                        {
                            weaponsArmor_selectTab = true;
                            onPartSuccess_goback(ref weaponsArmor_selectTab_Sound);
                        }
                    }
                    else
                    {
                        if (weaponsArmor_selectTab)
                        {
                            weaponsArmor_selectTab = false;
                            display.refresh = true;
                        }
                    }
                    //}
                    //else
                    //{ 

                    //}
                    if (player.resourcesSubTab == ResourcesSubTab.Work_Weapons)
                    {
                        if (!weaponsArmor_selectSubTab)
                        {
                            weaponsArmor_selectSubTab = true;
                            onPartSuccess_goback(ref weaponsArmor_selectSubTab_Sound);
                        }
                    }
                    else if (!weaponsArmor_setWeaponPrio)
                    {
                        if (weaponsArmor_selectSubTab)
                        {
                            weaponsArmor_selectSubTab = false;
                            display.refresh = true;
                        }
                    }


                    if (!weaponsArmor_setWeaponPrio)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().workTemplate.craft_sharpstick.value > 0)
                        {
                            weaponsArmor_setWeaponPrio = true;
                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_setArmorPrio)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().workTemplate.craft_paddedarmor.value > 0)
                        {
                            weaponsArmor_setArmorPrio = true;
                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_produceWeapons)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().res_sharpstick.amount >= CollectWeaponArmorAmount)
                        {
                            weaponsArmor_produceWeapons = true;

                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_produceArmor)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().res_paddedArmor.amount >= CollectWeaponArmorAmount)
                        {
                            weaponsArmor_produceArmor = true;

                            onPartSuccess();
                        }
                    }
                    break;

              

               

                case TutorialMission.ConscriptArmy:
                    if (!conscriptArmy_build)
                    {
                        lock (player.orders.orders)
                        {
                            foreach (var order in player.orders.orders)
                            {
                                if (order is BuildOrder && ((BuildOrder)order).buildingType == Build.BuildAndExpandType.SoldierBarracks)
                                {
                                    conscriptArmy_build = true;
                                    onPartSuccess();
                                    break;
                                }
                            }
                        }
                    }
                    if (!conscriptArmy_selectTab)
                    {
                        if (player.cityTab == Interface.MenuTab.Conscript)
                        {
                            conscriptArmy_selectTab = true;

                            onPartSuccess();
                        }
                    }
                    if (!conscriptArmy_createArmy)
                    {
                        var armyC = player.faction.armies.counter();

                        while (armyC.Next())
                        {
                            if (armyC.sel.groups.Count >= 2)
                            {
                                conscriptArmy_createArmy = true;
                                onPartSuccess();
                                break;
                            }
                        }
                    }
                    
                    break;

                case TutorialMission.CollectFood:
                    //bool CollectFood_increasefoodbuffer = false;
                    //bool CollectFood_reachfoodamount = false;
                    if (!CollectFood_selecttab)
                    {
                        if (player.cityTab == Interface.MenuTab.Resources)
                        {
                            CollectFood_selecttab = true;

                            onPartSuccess();
                        }
                    }
                    if (!CollectFood_foodblueprint)
                    {
                        //if (player.hud.tooltip.tooltip_id == Tooltip.Food_BlueprintId &&
                        //    player.hud.tooltip.tooltip_id_timestampsec >= 2)
                        if (player.hud.objMenu.menu != null && player.hud.objMenu.menu.HasToolTip(Tooltip.Food_BlueprintId))
                        {
                            CollectFood_foodblueprint = true;

                            onPartSuccess();
                        }
                    }
                    if (!CollectFood_buildfoodproduction)
                    {
                        lock (player.orders.orders)
                        {
                            for (int i = player.orders.orders.Count - 1; i >= 0; --i)//each (var order in player.orders.orders)
                            {
                                var order = player.orders.orders[i];
                                if (order is BuildOrder)
                                {
                                    switch (((BuildOrder)order).buildingType)
                                    {
                                        case Build.BuildAndExpandType.HenPen:
                                        case Build.BuildAndExpandType.PigPen:
                                        case Build.BuildAndExpandType.WheatFarm:
                                            CollectFood_buildfoodproduction = true;
                                            onPartSuccess();
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (!CollectFood_buildfuelproduction)
                    {
                        lock (player.orders.orders)
                        {
                            for (int i = player.orders.orders.Count - 1; i >= 0; --i)//each (var order in player.orders.orders)
                            {
                                var order = player.orders.orders[i];
                                if (order is BuildOrder)
                                {
                                    switch (((BuildOrder)order).buildingType)
                                    {
                                        case Build.BuildAndExpandType.CoalPit:
                                        case Build.BuildAndExpandType.RapeSeedFarm:
                                            CollectFood_buildfuelproduction = true;
                                            onPartSuccess();
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if (!CollectFood_builcook)
                    {
                        lock (player.orders.orders)
                        {
                            for (int i = player.orders.orders.Count - 1; i >= 0; --i)//each (var order in player.orders.orders)
                            {
                                var order = player.orders.orders[i];
                                if (order is BuildOrder)
                                {
                                    switch (((BuildOrder)order).buildingType)
                                    {
                                        case Build.BuildAndExpandType.Cook:
                                            CollectFood_builcook = true;
                                            onPartSuccess();
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (!CollectFood_selectStockPile)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.cityTab == Interface.MenuTab.Resources &&
                            player.resourcesSubTab == ResourcesSubTab.Stockpile_Resources)
                        {
                            CollectFood_selectStockPile = true;

                            onPartSuccess();
                        }
                    }

                    if (!CollectFood_increasefoodbuffer)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().res_food.goalBuffer > City.DefaultFoodBuffer)
                        {
                            CollectFood_increasefoodbuffer = true;

                            onPartSuccess();
                        }
                    }

                    if (!CollectFood_reachfoodamount)
                    {
                        if (player.gameControls.map.selection.obj is City &&
                            player.gameControls.map.selection.obj.GetCity().res_food.amount >= ReachFoodBuffer)
                        {
                            CollectFood_reachfoodamount = true;

                            onPartSuccess();
                        }
                    }

                    break;
              
                case TutorialMission.MoveArmy:

                    if (player.mapLayersManager.current.DrawMid)
                    {
                        if (!moveArmy_ZoomOut)
                        {
                            moveArmy_ZoomOut = true;
                            onPartSuccess(moveArmy_ZoomOut_sound);
                            moveArmy_ZoomOut_sound = true;
                        }
                    }
                    else
                    {
                        if (moveArmy_ZoomOut)
                        {
                            moveArmy_ZoomOut = false;
                            display.refresh = true;
                        }
                    }
                    

                    if (!moveArmy_SelectMove)
                    {
                        var armyC = player.faction.armies.counter();

                        while (armyC.Next())
                        {
                            if (armyC.sel.objective == ArmyObjective.MoveTo ||
                                armyC.sel.objective == ArmyObjective.Attack)
                            {
                                moveArmy_SelectMove = true;
                                onPartSuccess();
                                break;
                            }
                        }
                    }
                    break;

                case TutorialMission.AttackBarbarian:                    
                    
                    if (!attackBarbarian_win && 
                        barbarianArmy != null && barbarianArmy.defeated())
                    {
                        attackBarbarian_win = true;

                        onPartSuccess();
                    }
                    
                    break;

                case TutorialMission.Diplomatics:

                    if (player.mapLayersManager.current.DrawFar)
                    {
                        if (!diplomatics_ZoomOut)
                        {
                            diplomatics_ZoomOut = true;
                            onPartSuccess(diplomatics_ZoomOut_sound);
                            diplomatics_ZoomOut_sound = true;
                        }
                    }
                    else
                    {
                        if (diplomatics_ZoomOut)
                        {
                            diplomatics_ZoomOut = false;
                            display.refresh = true;
                        }
                    }
                    
                    if (!diplomatics_goodRelation)
                    {
                        foreach (var rel in player.faction.diplomaticRelations)
                        {
                            if (rel != null)
                            {
                                if (rel.Relation >= RelationType.RelationType2_Good)
                                {
                                    diplomatics_goodRelation = true;
                                    onPartSuccess();
                                    break;
                                }
                            }
                        }                       
                    }
                    break;

                case TutorialMission.TagFoodCity:
                    ////TagCity
                    //TwoBools tagCity_selectCity_sound = TwoBools.False;
                    if (player.gameControls.map.selection.obj is City &&
                        player.gameControls.map.selection.obj != player.faction.mainCity)
                    {
                        if (!tagCity_selectCity_sound.Value1)
                        {
                            tagCity_selectCity_sound.Value1 = true;
                            onPartSuccess(tagCity_selectCity_sound.Value2);
                            tagCity_selectCity_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_selectCity_sound.Value1)
                        {
                            tagCity_selectCity_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }

                    if (tagCity_selectCity_sound.Value1 &&
                        player.cityTab == Interface.MenuTab.Tag)
                    {
                        if (!tagCity_tagTab_sound.Value1)
                        {
                            tagCity_tagTab_sound.Value1 = true;
                            onPartSuccess(tagCity_tagTab_sound.Value2);
                            tagCity_tagTab_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_tagTab_sound.Value1)
                        {
                            tagCity_tagTab_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }

                    //TwoBools tagCity_foodTag_sound = TwoBools.False;
                    if (tagCity_selectCity_sound.Value1 &&
                        lib.EqualToAny(player.gameControls.map.selection.obj.GetCity().tagArt, Data.CityTagArt.ItemResourceTypeFood,  Data.CityTagArt.ItemResourceTypeRawFood))
                    {
                        if (!tagCity_foodTag_sound.Value1)
                        {
                            tagCity_foodTag_sound.Value1 = true;
                            onPartSuccess(tagCity_foodTag_sound.Value2);
                            tagCity_foodTag_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_foodTag_sound.Value1)
                        {
                            tagCity_foodTag_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }
                    //TwoBools tagCity_subTabTag_sound = TwoBools.False;
                    if (tagCity_selectCity_sound.Value1 &&
                        player.cityTab == Interface.MenuTab.Tag &&
                        player.tagSubTab == TagSubTab.HudPin)
                    {
                        if (!tagCity_subTabTag_sound.Value1)
                        {
                            tagCity_subTabTag_sound.Value1 = true;
                            onPartSuccess(tagCity_subTabTag_sound.Value2);
                            tagCity_subTabTag_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_subTabTag_sound.Value1)
                        {
                            tagCity_subTabTag_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }
                    //TwoBools tagCity_rawFoodToHud_sound = TwoBools.False;
                    if (tagCity_selectCity_sound.Value1 &&
                        player.hud.HasPin(new HudPin(ItemResourceType.RawFood_Group)))
                    {
                        if (!tagCity_rawFoodToHud_sound.Value1)
                        {
                            tagCity_rawFoodToHud_sound.Value1 = true;
                            onPartSuccess(tagCity_rawFoodToHud_sound.Value2);
                            tagCity_rawFoodToHud_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_rawFoodToHud_sound.Value1)
                        {
                            tagCity_rawFoodToHud_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }
                    //TwoBools tagCity_fuelToHud_sound = TwoBools.False;
                    if (tagCity_selectCity_sound.Value1 &&
                        player.hud.HasPin(new HudPin(ItemResourceType.Fuel_G)))
                    {
                        if (!tagCity_fuelToHud_sound.Value1)
                        {
                            tagCity_fuelToHud_sound.Value1 = true;
                            onPartSuccess(tagCity_fuelToHud_sound.Value2);
                            tagCity_fuelToHud_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_fuelToHud_sound.Value1)
                        {
                            tagCity_fuelToHud_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }
                    //TwoBools tagCity_foodToHud_sound = TwoBools.False;
                    if (tagCity_selectCity_sound.Value1 &&
                        player.hud.HasPin(new HudPin(ItemResourceType.Food_G)))
                    {
                        if (!tagCity_foodToHud_sound.Value1)
                        {
                            tagCity_foodToHud_sound.Value1 = true;
                            onPartSuccess(tagCity_foodToHud_sound.Value2);
                            tagCity_foodToHud_sound.Value2 = true;
                        }
                    }
                    else
                    {
                        if (tagCity_rawFoodToHud_sound.Value1)
                        {
                            tagCity_rawFoodToHud_sound.Value1 = false;
                            display.refresh = true;
                        }
                    }
                    break;

                case TutorialMission.LogisticsUpgrade:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            //TwoBools logisticsUpgrade_collectFood_sound = TwoBools.False;
                            if (city.GetGroupedResource(ItemResourceType.Food_G).amount >= DssConst.Logistics1FoodStorage)
                            {
                                if (!logisticsUpgrade_collectFood_sound.Value1)
                                {
                                    logisticsUpgrade_collectFood_sound.Value1 = true;
                                    onPartSuccess(logisticsUpgrade_collectFood_sound.Value2);
                                    logisticsUpgrade_collectFood_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (logisticsUpgrade_collectFood_sound.Value1)
                                {
                                    logisticsUpgrade_collectFood_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }


                            if (!logisticsUpgrade_build_sound.Value1 && hasBuildOrder(BuildAndExpandType.Logistics))
                            {
                                logisticsUpgrade_build_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;
                case TutorialMission.EducateBurner:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////EducateBurner,
                            //TwoBools educateBurner_buildSchool = TwoBools.False;
                            if (!educateBurner_buildSchool.Value1 && hasBuildOrder(BuildAndExpandType.School))
                            {
                                educateBurner_buildSchool.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools educateBurner_buildCoalPit = TwoBools.False;
                            if (!educateBurner_buildCoalPit.Value1 && hasBuildOrder(BuildAndExpandType.CoalPit))
                            {
                                educateBurner_buildCoalPit.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools educateBurner_schoolTab = TwoBools.False;
                            if (player.cityTab == MenuTab.Progress && player.progressSubTab == ProgressSubTab.Schools)
                            {
                                if (!educateBurner_schoolTab.Value1)
                                {
                                    educateBurner_schoolTab.Value1 = true;
                                    onPartSuccess(educateBurner_schoolTab.Value2);
                                    educateBurner_schoolTab.Value2 = true;
                                }
                            }
                            else
                            {
                                if (educateBurner_schoolTab.Value1)
                                {
                                    educateBurner_schoolTab.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools educateBurner_educateFuel = TwoBools.False;
                            if (!educateBurner_educateFuel.Value1 &&
                                city.cityExperienceLevels.levels_CraftFuel.Max() >= XP.ExperienceLevel.Practitioner_2)
                            {
                                educateBurner_educateFuel.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;
                case TutorialMission.SendFood:
                    ////SendFood,
                    
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            //TwoBools sendFood_buildPostal = TwoBools.False;
                            if (!sendFood_buildPostal.Value1 && 
                                hasBuildOrder(BuildAndExpandType.Postal))
                            {
                                sendFood_buildPostal.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools sendFood_selectTab = TwoBools.False;
                            if (player.cityTab == MenuTab.Delivery && lib.EqualToAny( player.deliverySupTab, ItemResourceType.RESOURCES, ItemResourceType.NUM))
                            {
                                if (!sendFood_selectTab.Value1)
                                {
                                    sendFood_selectTab.Value1 = true;
                                    onPartSuccess(sendFood_selectTab.Value2);
                                    sendFood_selectTab.Value2 = true;
                                }
                            }
                            else
                            {
                                if (sendFood_selectTab.Value1)
                                {
                                    sendFood_selectTab.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools sendFood_postalQueue = TwoBools.False;
                            if (!sendFood_postalQueue.Value1 &&
                                sendFood_selectTab.Value1)
                            {
                                if (arraylib.InBound(city.deliveryServices, city.selectedDelivery))
                                {
                                    var deliverStatus = city.deliveryServices[city.selectedDelivery];
                                    if (deliverStatus.inProgress.type == ItemResourceType.Food_G)
                                    {
                                        sendFood_postalQueue.Value1 = true;
                                        onPartSuccess();
                                    }
                                }
                            }
                        }

                    }
                    break;

                case TutorialMission.FindWoodCity:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            int mostWood = 0;
                            City mostWoodCity = null;
                            var citiesC = player.faction.cities.counter();
                            while (citiesC.Next())
                            {
                                if (citiesC.sel.terrainStructure.resourceCount_wood > mostWood)
                                {
                                    mostWood = citiesC.sel.terrainStructure.resourceCount_wood;
                                    mostWoodCity = citiesC.sel;
                                }
                            }

                            ////FindWoodCity,
                            //TwoBools findWoodCity_selectCity_sound = TwoBools.False;
                            if (city == mostWoodCity)
                            {
                                if (!findWoodCity_selectCity_sound.Value1)
                                {
                                    findWoodCity_selectCity_sound.Value1 = true;
                                    onPartSuccess(findWoodCity_selectCity_sound.Value2);
                                    findWoodCity_selectCity_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (findWoodCity_selectCity_sound.Value1)
                                {
                                    findWoodCity_selectCity_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }

                            //TwoBools findWoodCity_selectTab_sound = TwoBools.False;
                            if (player.cityTab == MenuTab.Tag && player.tagSubTab == TagSubTab.Tag)
                            {
                                if (!findWoodCity_selectTab_sound.Value1)
                                {
                                    findWoodCity_selectTab_sound.Value1 = true;
                                    onPartSuccess(findWoodCity_selectTab_sound.Value2);
                                    findWoodCity_selectTab_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (findWoodCity_selectTab_sound.Value1)
                                {
                                    findWoodCity_selectTab_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools findWoodCity_woodTag_sound = TwoBools.False;
                            if (!findWoodCity_bowTag_sound.Value1 &&
                                lib.EqualToAny(city.tagArt, Data.CityTagArt.ItemResourceTypeBow, Data.CityTagArt.ItemResourceTypeLongBow, Data.CityTagArt.ItemResourceTypeMithrilBow))
                            {
                                findWoodCity_bowTag_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                            break;
                case TutorialMission.FletcherPractice:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////FletcherPractice
                            //TwoBools fletcherPractice_buildWorkBench_sound = TwoBools.False;
                            if (!fletcherPractice_buildWorkBench_sound.Value1 &&
                                hasBuildOrder(BuildAndExpandType.WorkBench))
                            {
                                fletcherPractice_buildWorkBench_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools fletcherPractice_resourceTab_sound = TwoBools.False;
                            if (player.cityTab == MenuTab.Resources && player.resourcesSubTab == ResourcesSubTab.Work_Projectile)
                            {
                                if (!fletcherPractice_resourceTab_sound.Value1)
                                {
                                    fletcherPractice_resourceTab_sound.Value1 = true;
                                    onPartSuccess(fletcherPractice_resourceTab_sound.Value2);
                                    fletcherPractice_resourceTab_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (fletcherPractice_resourceTab_sound.Value1)
                                {
                                    fletcherPractice_resourceTab_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools fletcherPractice_setSlingerTo3_sound = TwoBools.False;
                            if (city.workTemplate.craft_slingshot.value == 3)
                            {
                                if (!fletcherPractice_setSlingerTo3_sound.Value1)
                                {
                                    fletcherPractice_setSlingerTo3_sound.Value1 = true;
                                    onPartSuccess(fletcherPractice_setSlingerTo3_sound.Value2);
                                    fletcherPractice_setSlingerTo3_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (fletcherPractice_setSlingerTo3_sound.Value1)
                                {
                                    fletcherPractice_setSlingerTo3_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools fletcherPractice_setJavelinTo3_sound = TwoBools.False;
                            //if (city.workTemplate.craft_throwingspear.value == 3)
                            //{
                            //    if (!fletcherPractice_setJavelinTo3_sound.Value1)
                            //    {
                            //        fletcherPractice_setJavelinTo3_sound.Value1 = true;
                            //        onPartSuccess(fletcherPractice_setJavelinTo3_sound.Value2);
                            //        fletcherPractice_setJavelinTo3_sound.Value2 = true;
                            //    }
                            //}
                            //else
                            //{
                            //    if (fletcherPractice_setJavelinTo3_sound.Value1)
                            //    {
                            //        fletcherPractice_setJavelinTo3_sound.Value1 = false;
                            //        display.refresh = true;
                            //    }
                            //}
                            //TwoBools fletcherPractice_setBowTo4_sound = TwoBools.False;
                            if (city.workTemplate.craft_bow.value == 4)
                            {
                                if (!fletcherPractice_setBowTo4_sound.Value1)
                                {
                                    fletcherPractice_setBowTo4_sound.Value1 = true;
                                    onPartSuccess(fletcherPractice_setBowTo4_sound.Value2);
                                    fletcherPractice_setBowTo4_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (fletcherPractice_setBowTo4_sound.Value1)
                                {
                                    fletcherPractice_setBowTo4_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                        }
                    }
                            

                            break;
                case TutorialMission.FletcherPracticeWait:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////FletcherPracticeWait
                            //TwoBools fletcherPracticeWait_experienceTab_sound = TwoBools.False;
                            if (player.cityTab == MenuTab.Progress)
                            {
                                if (!fletcherPracticeWait_experienceTab_sound.Value1)
                                {
                                    fletcherPracticeWait_experienceTab_sound.Value1 = true;
                                    onPartSuccess(fletcherPracticeWait_experienceTab_sound.Value2);
                                    fletcherPracticeWait_experienceTab_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (fletcherPracticeWait_experienceTab_sound.Value1)
                                {
                                    fletcherPracticeWait_experienceTab_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools fletcherPracticeWait_skillToHud_sound = TwoBools.False;
                            if (!fletcherPracticeWait_skillToHud_sound.Value1 &&
                                player.hud.HasPin(new HudPin(XP.WorkExperienceType.Fletcher)))
                            {
                                fletcherPracticeWait_skillToHud_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools fletcherPracticeWait_progressToHud_sound = TwoBools.False;
                            if (!fletcherPracticeWait_progressToHud_sound.Value1 &&
                            player.hud.HasPin(new HudPin(XP.TechnologyTreeType.catapult)))
                            {
                                fletcherPracticeWait_progressToHud_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools fletcherPracticeWait_fletcherLevel2_sound = TwoBools.False;
                            if (!fletcherPracticeWait_fletcherLevel2_sound.Value1 &&
                                city.cityExperienceLevels.levels_Fletcher.Max() >= XP.ExperienceLevel.Practitioner_2)
                            {
                                fletcherPracticeWait_fletcherLevel2_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;

                case TutorialMission.ProduceBow:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////ProduceBow,
                            //bool produceBow_buildLogistics = false;
                            if (city.buildingStructure.buildingLevel_logistics >= 1 || hasBuildOrder(BuildAndExpandType.Logistics))
                            {
                                if (!produceBow_buildLogistics)
                                {
                                    produceBow_buildLogistics = true;
                                }
                            }
                            else
                            {
                                if (produceBow_buildLogistics)
                                {
                                    produceBow_buildLogistics = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools produceBow_buildCarpenter = TwoBools.False;
                            if (!produceBow_buildCarpenter.Value1 &&
                               (hasBuildOrder(BuildAndExpandType.Carpenter) || city.buildingStructure.Carpenter_count >= 1))
                            {
                                produceBow_buildCarpenter.Value1 = true;
                                onPartSuccess();
                            }

                            if (player.cityTab == MenuTab.BlackMarket ||
                                produceBow_buyIron.Value1)
                            {
                                if (!produceBow_blackmarketTab.Value1)
                                {
                                    produceBow_blackmarketTab.Value1 = true;
                                    onPartSuccess(produceBow_blackmarketTab.Value2);
                                    produceBow_blackmarketTab.Value2 = true;
                                }
                            }
                            else
                            {
                                if (produceBow_blackmarketTab.Value1)
                                {
                                    produceBow_blackmarketTab.Value1 = false;
                                    display.refresh = true;
                                }
                            }

                            //TwoBools produceBow_produceBow = TwoBools.False;
                            if (!produceBow_produceBow.Value1 &&
                                city.GetGroupedResource(ItemResourceType.Bow).amount >= CollectGuardResources)
                            {

                                produceBow_produceBow.Value1 = true;
                                onPartSuccess();                                
                            }
                        }
                    }
                    break;

                case TutorialMission.BuildDefences:

                    if (player.cityTab == Interface.MenuTab.Build)
                    {
                        if (!buildDefences_selectBuildTab)
                        {
                            buildDefences_selectBuildTab = true;
                            onPartSuccess_goback(ref buildDefences_selectBuildTab_sound);
                        }
                    }
                    else
                    {
                        if (buildDefences_selectBuildTab)
                        {
                            buildDefences_selectBuildTab = false;
                            display.refresh = true;
                        }
                    }

                    if (!buildDefences_buildPalisade)
                    {
                        lock (player.orders.orders)
                        {
                            for (int i = player.orders.orders.Count - 1; i >= 0; --i)
                            {
                                var order = player.orders.orders[i];
                                if (order is BuildOrder)
                                {
                                    switch (((BuildOrder)order).buildingType)
                                    {
                                        case Build.BuildAndExpandType.Palisade:

                                            buildDefences_buildPalisade = true;
                                            onPartSuccess();
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }

                    if (!buildDefences_moveGuard)
                    {
                        var citiesC = player.faction.cities.counter();

                        while (citiesC.Next())
                        {
                            var soldierGroupsC = citiesC.sel.groups.counter();
                            while (soldierGroupsC.Next())
                            {
                                var cmd = soldierGroupsC.sel.command;
                                if (cmd != null && cmd.HasCommand(Command.CommandType.EnterPost))
                                {
                                    buildDefences_moveGuard = true;
                                    onPartSuccess();
                                    return;
                                }
                            }
                        }
                    }
                    break;
                case TutorialMission.RecruitGuard:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            bool guardTab = false;

                            //if (player.gameControls.map.selection.obj is City)
                            //{
                            //    if (!recruitGuard_selectCity)
                            //    {
                            //        recruitGuard_selectCity = true;
                            //        onPartSuccess(recruitGuard_selectCity_sound);
                            //        recruitGuard_selectCity = true;
                            //    }

                            //    //if (!recruitGuard_selectGuardTab)
                            //    {
                            //        var city = player.gameControls.map.selection.obj.GetCity();
                            //        if (arraylib.TryGet(city.conscriptBuildings, city.selectedConscript, out BarracksStatus barracks))
                            //        {
                            //            if (barracks.profile.specialization == SpecializationType.CityGuard)
                            //            {
                            //                guardTab = true;
                            //            }
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    if (recruitGuard_selectCity)
                            //    {
                            //        recruitGuard_selectCity = false;
                            //        display.refresh = true;
                            //    }
                            //}

                            //if (player.mapLayersManager.current.DrawDetailLayer)
                            //{
                            //    if (!recruitGuard_zoomIn)
                            //    {
                            //        recruitGuard_zoomIn = true;
                            //        onPartSuccess_goback(ref recruitGuard_zoomIn_sound);
                            //    }
                            //}
                            //else
                            //{
                            //    if (recruitGuard_zoomIn)
                            //    {
                            //        recruitGuard_zoomIn = false;
                            //        display.refresh = true;
                            //    }
                            //}
                            
                            if (!recruitGuard_buildBarracks.Value1 &&
                                    (hasBuildOrder(BuildAndExpandType.ArcherBarracks) || city.buildingStructure.ArcherBarracks_count >= 1))
                            {
                                recruitGuard_buildBarracks.Value1 = true;
                                onPartSuccess();
                            }

                            if (player.cityTab == Interface.MenuTab.Conscript)
                            {
                                if (!recruitGuard_selectConscriptTab)
                                {
                                    recruitGuard_selectConscriptTab = true;
                                    onPartSuccess_goback(ref recruitGuard_selectConscriptTab_sound);
                                }
                            }
                            else
                            {
                                if (recruitGuard_selectConscriptTab)
                                {
                                    recruitGuard_selectConscriptTab = false;
                                    display.refresh = true;
                                }
                            }

                            if (guardTab)
                            {
                                if (!recruitGuard_selectGuardTab)
                                {
                                    recruitGuard_selectGuardTab = true;
                                    onPartSuccess_goback(ref recruitGuard_selectGuardTab_sound);
                                }
                            }
                            else
                            {
                                if (recruitGuard_selectGuardTab)
                                {
                                    recruitGuard_selectGuardTab = false;
                                    display.refresh = true;
                                }
                            }

                            if (!recruitGuard_createGuard)
                            {
                                if (DssRef.stats.guardsRecruited >= 2)
                                {
                                    recruitGuard_createGuard = true;
                                    onPartSuccess();
                                }
                            }
                        } 
                    }
                    break;

                case TutorialMission.FindIronCity:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////    FindIronCity,
                            //TwoBools findIronCity_selectCity_sound = TwoBools.False;
                            if (city.terrainStructure.mineCount_iron + city.terrainStructure.mineCount_bogIron >= 1)
                            {
                                if (!findIronCity_selectCity_sound.Value1)
                                {
                                    findIronCity_selectCity_sound.Value1 = true;
                                    onPartSuccess(findIronCity_selectCity_sound.Value2);
                                    findIronCity_selectCity_sound.Value2 = true;
                                }

                                //TwoBools findIronCity_Tag_sound = TwoBools.False;
                                if (!findIronCity_Tag_sound.Value1 &&
                                    (player.hud.HasPin(new HudPin(ItemResourceType.IronOre_G)) || player.hud.HasPin(new HudPin(ItemResourceType.Iron_G))))
                                {
                                    findIronCity_Tag_sound.Value1 = true;
                                    onPartSuccess();
                                }
                                //TwoBools findIronCity_buildSmelter_sound = TwoBools.False;
                                if (!findIronCity_buildSmelter_sound.Value1 &&
                                    (hasBuildOrder(BuildAndExpandType.Smelter) || city.buildingStructure.Smelter_count >= 1))
                                {
                                    findIronCity_buildSmelter_sound.Value1 = true;
                                    onPartSuccess();
                                }
                                //////TwoBools findIronCity_increasePriority_sound = TwoBools.False;
                                //if (!findIronCity_increasePriority_sound.Value1 &&
                                //    city.workTemplate.craft_iron.value > 1)
                                //{
                                //    findIronCity_increasePriority_sound.Value1 = true;
                                //    onPartSuccess();
                                //}
                            }
                            else
                            {
                                if (findIronCity_selectCity_sound.Value1)
                                {
                                    findIronCity_selectCity_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }

                            //TwoBools findIronCity_produceIron_sound = TwoBools.False;
                            if (!findIronCity_produceIron_sound.Value1 &&
                               city.GetGroupedResource(ItemResourceType.Iron_G).amount >= ProduceIronAmount)
                            {
                                findIronCity_produceIron_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;
                case TutorialMission.ProduceSword:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            //    ProduceSword,
                            if (city.cityExperienceLevels.levels_CraftMetal.Max() >= XP.ExperienceLevel.Practitioner_2)
                            {
                                if (!produceSword_level2smith_sound.Value1)
                                {
                                    produceSword_level2smith_sound.Value1 = true;
                                    onPartSuccess(produceSword_level2smith_sound.Value2);
                                    produceSword_level2smith_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (produceSword_level2smith_sound.Value1)
                                {
                                    produceSword_level2smith_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }


                            //TwoBools produceSword_buildSmith_sound = TwoBools.False;
                            if (!produceSword_buildSmith_sound.Value1 &&
                                    (hasBuildOrder(BuildAndExpandType.Smith) || city.buildingStructure.Smith_count >= 1))
                            {
                                produceSword_buildSmith_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools produceSword_swordPriority_sound = TwoBools.False;
                            if (!produceSword_swordPriority_sound.Value1 &&
                                city.workTemplate.craft_shortsword.value > 1)
                            {
                                produceSword_swordPriority_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools produceSword_produceSword_sound = TwoBools.False;
                            if (!produceSword_produceSword_sound.Value1 &&
                               city.GetGroupedResource(ItemResourceType.ShortSword).amount >= CollectWeaponArmorAmount)
                            {
                                produceSword_produceSword_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;
                case TutorialMission.ProduceMail:
                    {
                        City city = player.gameControls.map.selection.obj?.GetCity();

                        if (city != null)
                        {
                            ////    ProduceMail,
                            //TwoBools produceMail_level2Armorer_sound = TwoBools.False;
                            if (city.cityExperienceLevels.levels_CraftArmor.Max() >= XP.ExperienceLevel.Practitioner_2)
                            {
                                if (!produceMail_level2Armorer_sound.Value1)
                                {
                                    produceMail_level2Armorer_sound.Value1 = true;
                                    onPartSuccess(produceMail_level2Armorer_sound.Value2);
                                    produceMail_level2Armorer_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (produceMail_level2Armorer_sound.Value1)
                                {
                                    produceMail_level2Armorer_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools produceMail_buildArmorer_sound = TwoBools.False;
                            if (!produceMail_buildArmorer_sound.Value1 &&
                                    (hasBuildOrder(BuildAndExpandType.Armory) || city.buildingStructure.Armory_count >= 1))
                            {
                                produceMail_buildArmorer_sound.Value1 = true;
                                onPartSuccess();
                            }
                            //TwoBools produceMail_mailPriority_sound = TwoBools.False;
                            if (city.workTemplate.craft_mailarmor.value > 1)
                            {
                                if (!produceMail_mailPriority_sound.Value1)
                                {
                                    produceMail_mailPriority_sound.Value1 = true;
                                    onPartSuccess(produceMail_mailPriority_sound.Value2);
                                    produceMail_mailPriority_sound.Value2 = true;
                                }
                            }
                            else
                            {
                                if (produceMail_mailPriority_sound.Value1)
                                {
                                    produceMail_mailPriority_sound.Value1 = false;
                                    display.refresh = true;
                                }
                            }
                            //TwoBools produceMail_produceMail_sound = TwoBools.False;
                            if (!produceMail_produceMail_sound.Value1 &&
                               city.GetGroupedResource(ItemResourceType.IronArmor).amount >= CollectWeaponArmorAmount)
                            {
                                produceMail_produceMail_sound.Value1 = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;
                    

            }

            display.update(ref mouseOverHud);
        }


        bool hasBuildOrder(Build.BuildAndExpandType build)
        {
            lock (player.orders.orders)
            {
                for (int i = player.orders.orders.Count - 1; i >= 0; --i)//each (var order in player.orders.orders)
                {
                    var order = player.orders.orders[i];
                    if (order is BuildOrder)
                    {
                        var type = ((BuildOrder)order).buildingType;
                        if (type == build)
                        { 
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        void onPartSuccess_goback(ref bool soundPlayed)
        {
            onPartSuccess(soundPlayed);
            soundPlayed = true;
        }

        void onPartSuccess(bool soundPlayed = false)
        {
            if (!soundPlayed)
            {
                SoundLib.trophy.Play();
            }
            display.refresh = true;

            bool missionComplete = false;

            switch (missions.sel)
            {
                case TutorialMission.CollectResources:
                    missionComplete = collectResources_selectCity &&
                        collectResources_zoomIn &&
                        collectResources_selectTab &&
                        collectResources_collectwood &&
                        collectResources_collectstone;
                    break;

                case TutorialMission.CasualBuildBarracks:
                    missionComplete = CasualBuildBarracks_build;
                    break;
                case TutorialMission.CasualRecruitSoldier:
                    missionComplete = casualRecruit_recruit;
                    break;

                case TutorialMission.Linen:
                    missionComplete = linen_selectTab &&
                        linen_build &&
                        linen_collect;
                    break;

                case TutorialMission.ProduceWeaponsArmor:
                    missionComplete = weaponsArmor_produceWeapons && weaponsArmor_produceArmor;
                    break;

                case TutorialMission.ConscriptArmy:
                    missionComplete = conscriptArmy_createArmy;
                    break;
                case TutorialMission.CollectFood:
                    missionComplete = CollectFood_selecttab &&
                        CollectFood_foodblueprint &&
                        CollectFood_buildfoodproduction &&
                        CollectFood_buildfuelproduction &&
                        CollectFood_builcook &&
                        CollectFood_increasefoodbuffer &&
                        CollectFood_reachfoodamount;
                    break;
                case TutorialMission.MoveArmy:
                    missionComplete = moveArmy_ZoomOut &&
                        moveArmy_SelectMove;
                    break;
                case TutorialMission.AttackBarbarian:
                    missionComplete = attackBarbarian_win;
                    break;
                case TutorialMission.Diplomatics:
                    missionComplete = diplomatics_ZoomOut &&
                        diplomatics_goodRelation;
                    break;

                case TutorialMission.TagFoodCity:
                    missionComplete = tagCity_foodTag_sound.Value1 &&
                        tagCity_rawFoodToHud_sound.Value1 &&
                        tagCity_fuelToHud_sound.Value1 &&
                        tagCity_foodToHud_sound.Value1;
                    break;

                case TutorialMission.LogisticsUpgrade:
                    missionComplete = logisticsUpgrade_build_sound.Value1;
                    break;

                case TutorialMission.EducateBurner:
                    missionComplete = educateBurner_educateFuel.Value1 && educateBurner_buildCoalPit.Value1;
                    break;
                case TutorialMission.SendFood:
                    missionComplete = sendFood_postalQueue.Value1;
                    break;

                case TutorialMission.FindWoodCity:
                    missionComplete = findWoodCity_bowTag_sound.Value1;
                    break;
                case TutorialMission.FletcherPractice:
                    missionComplete = fletcherPractice_buildWorkBench_sound.Value1 &&
                        fletcherPractice_setSlingerTo3_sound.Value1 &&
                        fletcherPractice_setBowTo4_sound.Value1;
                    break;
                case TutorialMission.FletcherPracticeWait:
                    missionComplete = fletcherPracticeWait_fletcherLevel2_sound.Value1;
                    break;
                case TutorialMission.ProduceBow:
                    missionComplete = produceBow_produceBow.Value1;
                    break;
                case TutorialMission.BuildDefences:
                    missionComplete = buildDefences_buildPalisade && buildDefences_moveGuard;
                    break;
                case TutorialMission.RecruitGuard:
                    missionComplete = recruitGuard_createGuard;
                    break;

                case TutorialMission.FindIronCity:
                    missionComplete = findIronCity_Tag_sound.Value1 && findIronCity_buildSmelter_sound.Value1 && findIronCity_produceIron_sound.Value1;
                    break;
                case TutorialMission.ProduceSword:
                    missionComplete = produceSword_produceSword_sound.Value1;
                    break;
                case TutorialMission.ProduceMail:
                    missionComplete = produceMail_produceMail_sound.Value1;
                    break;
            }

            if (missionComplete)
            { 
                onMissionSuccess();
            }
        }

        void onMissionSuccess()
        {
            new TimedAction1ArgTrigger<int>(nextMission, missions.selIndex +1, 1000);            
        }

        void nextMission(int nextIx)
        {
            if (missions.selIndex < nextIx)
            {
                missions.SelectIndex(nextIx);
                display.refresh = true;

                if (missions.sel == TutorialMission.EndTutorial)
                {

                    DssRef.stats.completeTutorial.addOne();

                    player.hud.messages.Add(DssRef.lang.Tutorial_CompleteTitle, DssRef.lang.Tutorial_CompleteMessage);
                    EndTutorial();
                }
                else if (missions.sel < TutorialMission.EndTutorial)
                {
                    if (missions.sel == TutorialMission.AttackBarbarian)
                    {
                        startUnits();
                        spawnBarbarians();
                        player.gameControls.map.setCameraBounds(false, cityarea);
                    }

                    refreshLimits();

                    RichBoxContent content = new RichBoxContent();
                    content.h1(DssRef.lang.Tutorial_MissionComplete_Title).overrideColor = HudLib.InfoYellow_Light;
                    content.text(DssRef.lang.Tutorial_MissionComplete_Unlocks);
                    player.hud.messages.Add(content);
                }
                else if (missions.sel == TutorialMission.EndAdvisor)
                {
                    player.hud.messages.Add(DssRef.todoLang.Tutorial_AdvisorCompleteTitle, DssRef.todoLang.Tutorial_AdvisorCompleteMessage);
                    EndAdvisor();
                }
            }
        }
        
        void spawnBarbarians()
        {
            var city = player.faction.mainCity;

            barbarianArmy = StoryEvent_Barbarians.spawnBarbarians(city, true);
            player.gameControls.map.cameraFocus = barbarianArmy;
           
        }

        public void writeGameState(BinaryWriter w)
        {
            //w.Write(DssRef.storage.shortTutorial);
            w.Write(missions.selIndex);
        }

        public void readGameState(BinaryReader r, int subversion)
        {
            missions.SelectIndex(r.ReadInt32());

            refreshLimits();
        }

        public void EndCurrentTutorialMode()
        {
            if (missions.sel <= TutorialMission.EndTutorial)
            {
                EndTutorial();
                DssRef.stats.skipTutorial.addOne();
            }
            else
            {
                EndAdvisor();
                DssRef.stats.skip_advisor.addOne();
            }
        }

        void EndTutorial()
        {
            missions.SelectItem(TutorialMission.EndTutorial);
            nextMission(missions.selIndex + 1);
            refreshLimits();

            player.gameControls.map.setCameraBounds(false, cityarea);
            bool createStartUnits = missions.sel < TutorialMission.AttackBarbarian;
            
            if (!PlatformSettings.STEAM_DEMO)
            {
                DssRef.storage.Save(null);
            }

            Faction enemyFac = DssRef.world.factions.GetIndex_Safe(DssRef.settings.Faction_Barbarian);
            if (enemyFac != null)
            {
                enemyFac.player.GetAiPlayer().armyAi_enabled = true;
            }

            if (createStartUnits)
            {
                startUnits();
            }
            
            player.hud.messages.blockFoodWarning(false);
            DssRef.state.events.onTutorialEnd();

            if (player.profile.casualControls || PlatformSettings.STEAM_DEMO)
            {
                EndAdvisor();
            }
            else
            { 
                Ref.update.AddSyncAction(new SyncAction(DssRef.state.menuSystem.TutorialCompleteMenu)); 
            }
        }

        public bool AdvisorMode()
        {
            return missions.sel > TutorialMission.EndTutorial;
        }

        public bool TutorialMode()
        {
            return missions.sel < TutorialMission.EndTutorial;
        }

        public void onBuyFromBlackMarket(ItemResourceType resourceType)
        {
            if (missions.sel == TutorialMission.ProduceBow && resourceType == ItemResourceType.Iron_G)
            {
                if (!produceBow_buyIron.Value1)
                {
                    produceBow_buyIron.Value1 = true;
                    onPartSuccess(true);
                }
            }
        }

        public void EndAdvisor()
        {
            player.tutorial = null;

            display.DeleteMe();
            DssRef.storage.runTutorial = false;
            player.hud.minimapProperty(null, true, true);
        }

        void startUnits()
        {
            var factionC = DssRef.world.factions.counter();
            while (factionC.Next())
            {
                factionC.sel.player.createStartupBarracks();
                factionC.sel.player.createStartUnits();
            }
        }
    }
}
