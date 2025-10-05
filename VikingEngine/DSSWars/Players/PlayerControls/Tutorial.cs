using Microsoft.CodeAnalysis.Text;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Event;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
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
            RecruitGuard,
            BuildDefences,
            MoveArmy,
            AttackBarbarian,
            Diplomatics,

            

            End,
        }

        List2<TutorialMission> missions;

        const int CollectWoodStoneAmount = 30;
        const int CollectLinenAmount = 15;
        static int CollectWeaponArmorAmount = DssConst.SoldierGroup_DefaultCount * 2;

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
        //bool casualRecruit_selectCity = false;
        //bool casualRecruit_selectCity_sound = false;
        bool casualRecruit_selectTab = false;
        bool casualRecruit_selectTab_sound = false;
        bool casualRecruit_recruit = false;
        //bool casualRecruit_recruit_sound = false;

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

        bool recruitGuard_zoomIn = false;
        bool recruitGuard_zoomIn_sound = false;
        bool recruitGuard_selectCity = false;
        bool recruitGuard_selectCity_sound = false;
        bool recruitGuard_selectConscriptTab = false;
        bool recruitGuard_selectConscriptTab_sound = false;
        bool recruitGuard_selectGuardTab = false;
        bool recruitGuard_selectGuardTab_sound = false;
        bool recruitGuard_createGuard = false;

        bool buildDefences_selectBuildTab = false;
        bool buildDefences_selectBuildTab_sound = false;
        bool buildDefences_buildPalisade = false;
        bool buildDefences_moveGuard = false;

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
                    TutorialMission.End,
                };
            }
            else
            {
                //if (DssRef.storage.runTutorial_1short_2normal == 1)
                //{
                //    missions = new List2<TutorialMission>
                //{
                //    TutorialMission.RecruitGuard,
                //    TutorialMission.BuildDefences,
                //    TutorialMission.MoveArmy,
                //    TutorialMission.End,
                //};
                //}
                //else
                //{
                    missions = new List2<TutorialMission>
                {
                    //TutorialMission.AttackBarbarian,
                    TutorialMission.CollectResources,
                    TutorialMission.Linen,
                    TutorialMission.ProduceWeaponsArmor,
                    TutorialMission.ConscriptArmy,
                    TutorialMission.CollectFood,
                    TutorialMission.MoveArmy,
                    TutorialMission.AttackBarbarian,
                    TutorialMission.Diplomatics,
                    TutorialMission.End,
                };
                //}
            }
            missions.selectFirst();
        }

        public Tutorial(LocalPlayer player)
        {
            cityarea = new Rectangle2();

            this.player = player;
            display = new Interface.TutorialDisplay(player);
            initMissions();
            

            //Setup resources and map
            var cityCounter = player.faction.cities.counter();
            while (cityCounter.Next())
            {
                cityCounter.sel.res_wood.amount = 0;
                cityCounter.sel.res_sharpstick.amount = CollectWeaponArmorAmount - 6;//30;
                cityCounter.sel.res_paddedArmor.amount = CollectWeaponArmorAmount - 6;

                if (DssRef.storage.runTutorial_1short_2normal == 1)
                {
                    cityCounter.sel.res_Palisade.amount = 50;
                    cityCounter.sel.createStartupBarracks();
                }


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
        

        public void tutorial_ToHud(RichBoxContent content)
        {
            content.h1(DssRef.lang.Tutorial_MissionsTitle);
            content.h2(string.Format(DssRef.lang.Tutorial_MissionX, missions.selIndex +1), HudLib.InfoYellow_Light);

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
                    //content.Add(tabImg);
                    content.Add(new RbOverlapImage(tabImg, SpriteName.WarsResource_Sword, Vector2.Zero, 0.8f));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Work)));
                    
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_setWeaponPrio), ResourceLib.Icon(ItemResourceType.SharpStick), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_SharpStick));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_setArmorPrio), ResourceLib.Icon(ItemResourceType.PaddedArmor), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_PaddedArmor));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_produceWeapons), ResourceLib.Icon(ItemResourceType.SharpStick), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_SharpStick));
                    content.iconicontext(HudLib.CheckImage(weaponsArmor_produceArmor), ResourceLib.Icon(ItemResourceType.PaddedArmor), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_PaddedArmor));
                    break;

                case TutorialMission.RecruitGuard:
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(recruitGuard_selectCity)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialCity));
                    content.Add(new RbText("/"));
                    content.Add(new RbImage(SpriteName.WarsCityHall));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_SelectACity));
                    //content.icontext(HudLib.CheckImage(recruitGuard_selectCity), DssRef.lang.Tutorial_SelectACity);
                    content.iconicontext(HudLib.CheckImage(recruitGuard_zoomIn), SpriteName.WarsWorker, DssRef.lang.Tutorial_ZoomInWorkers);
                    content.iconicontext(HudLib.CheckImage(recruitGuard_selectConscriptTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.iconicontext(HudLib.CheckImage(recruitGuard_selectGuardTab), SpriteName.WarsGuard, string.Format(DssRef.lang.Tutorial_OpenGuardSubTab, DssRef.lang.Conscript_Soldiers_GuardType));
                    content.iconicontext(HudLib.CheckImage(recruitGuard_createGuard), SpriteName.WarsUnitIcon_Folkman, string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.Resource_TypeName_PaddedArmor));
                    
                    break;

                case TutorialMission.BuildDefences:
                    content.iconicontext(HudLib.CheckImage(buildDefences_selectBuildTab),SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.iconicontext(HudLib.CheckImage(buildDefences_buildPalisade), SpriteName.WarsBuild_Palisade, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Palisade].Label()));
                    content.iconicontext(HudLib.CheckImage(buildDefences_moveGuard), SpriteName.WarsGuardPostIcon, DssRef.lang.Tutorial_GuardToWall);
                   
                    break;

                case TutorialMission.ConscriptArmy:
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_build), SpriteName.WarsBuild_Barracks, string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.SoldierBarracks].Label()));
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_selectTab), SpriteName.WarsHudTabSelected, string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.iconicontext(HudLib.CheckImage(conscriptArmy_createArmy), SpriteName.WarsUnitIcon_Folkman, string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.Resource_TypeName_PaddedArmor));
                    break;

                case TutorialMission.CollectFood:
                    content.iconicontext(HudLib.CheckImage(CollectFood_selecttab), SpriteName.MenuPixelIconManual,
                        string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources) + ". " + string.Format(DssRef.lang.Tutorial_Select_SubTab, DssRef.lang.Resource_Tab_Overview)/*string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources)*/);
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
                    var info2 = new RbText(DssRef.lang.Tutorial_CollectFood_Info2);
                    info2.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info2);

                    break;
                
                case TutorialMission.MoveArmy:
                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(moveArmy_ZoomOut)));
                    // content.space();
                    //content.Add(new RbImage(SpriteName.WarsTutorialArmy));
                     content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_ZoomOutOverview));

                    content.newLine();
                    content.Add(new RbImage(HudLib.CheckImage(moveArmy_SelectMove)));
                    content.space();
                    content.Add(new RbImage(SpriteName.WarsTutorialArmy));
                    content.space();
                    content.Add(new RbText(DssRef.lang.Tutorial_Mission_MoveArmy));


                    //content.icontext(HudLib.CheckImage(moveArmy_ZoomOut), DssRef.lang.Tutorial_ZoomOutOverview);
                    //content.icontext(HudLib.CheckImage(moveArmy_SelectMove), DssRef.lang.Tutorial_Mission_MoveArmy);
                    break;

                case TutorialMission.AttackBarbarian:
                    content.iconicontext(HudLib.CheckImage(attackBarbarian_win), SpriteName.WarsRelationWar, string.Format( DssRef.lang.Tutorial_AttackAndDestroyX, DssRef.lang.FactionName_Barbarian));
                    //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.cmdSpyglass), new RbSpace(), new RbText(DssRef.lang.FactionName_Barbarian) },
                    //    new RbAction(() =>
                    //    {
                    //        player.gameControls.map.cameraFocus = barbarianArmy;
                    //    })));
                    break;

                case TutorialMission.Diplomatics:
                    content.iconicontext(HudLib.CheckImage(diplomatics_ZoomOut), SpriteName.WarsDiplomaticPoint,  DssRef.lang.Tutorial_ZoomOutDiplomacy);
                    content.iconicontext(HudLib.CheckImage(diplomatics_goodRelation), SpriteName.WarsRelationGood, DssRef.lang.Tutorial_ImproveRelations);
                    break;

            }

            content.newParagraph();
            content.icontext(player.gameControls.input.mouseSelect.Icon, DssRef.lang.Tutorial_SelectInput);            
            content.icontext(player.gameControls.input.inputSource.IsController? player.gameControls.input.cameraTiltZoom.Icon : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
            if (missions.sel == TutorialMission.MoveArmy ||
                missions.sel == TutorialMission.AttackBarbarian ||
                missions.sel == TutorialMission.BuildDefences)
            {
                content.icontext(player.gameControls.input.mouseOrder.Icon, DssRef.lang.Tutorial_MoveInput);
            }
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

                case TutorialMission.RecruitGuard:
                    
                    bool guardTab = false;

                    if (player.gameControls.map.selection.obj is City)
                    {
                        if (!recruitGuard_selectCity)
                        {
                            recruitGuard_selectCity = true;
                            onPartSuccess(recruitGuard_selectCity_sound);
                            recruitGuard_selectCity = true;
                        }

                        //if (!recruitGuard_selectGuardTab)
                        {
                            var city = player.gameControls.map.selection.obj.GetCity();
                            if (arraylib.TryGet(city.conscriptBuildings, city.selectedConscript, out BarracksStatus barracks))
                            {
                                if (barracks.profile.specialization == SpecializationType.CityGuard)
                                {
                                    guardTab = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (recruitGuard_selectCity)
                        {
                            recruitGuard_selectCity = false;
                            display.refresh = true;
                        }
                    }

                    if (player.mapLayersManager.current.DrawDetailLayer)
                    {
                        if (!recruitGuard_zoomIn)
                        {
                            recruitGuard_zoomIn = true;
                            onPartSuccess_goback(ref recruitGuard_zoomIn_sound);
                        }
                    }
                    else
                    {
                        if (recruitGuard_zoomIn)
                        {
                            recruitGuard_zoomIn = false;
                            display.refresh = true;
                        }
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
            }

            display.update(ref mouseOverHud);
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

                
                //case TutorialMission.SharpStickWork:
                //    missionComplete = weaponsArmor_setWeaponPrio;
                //    break;

                case TutorialMission.ProduceWeaponsArmor:
                    missionComplete = weaponsArmor_produceWeapons && weaponsArmor_produceArmor;
                    break;

                case TutorialMission.RecruitGuard:
                    missionComplete = recruitGuard_createGuard;
                    break;

                case TutorialMission.BuildDefences:
                    missionComplete = buildDefences_buildPalisade && buildDefences_moveGuard;
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

                if (missions.sel >= TutorialMission.End)
                {
                    if (DssRef.storage.runTutorial_1short_2normal == 1)
                    {
                        DssRef.stats.completeShortTutorial.addOne();
                    }
                    else
                    {
                        DssRef.stats.completeTutorial.addOne();
                    }
                    player.hud.messages.Add(DssRef.lang.Tutorial_CompleteTitle, DssRef.lang.Tutorial_CompleteMessage);
                    EndTutorial();
                }
                else
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
            }
        }
        
        void spawnBarbarians()
        {
            var city = player.faction.mainCity;

            StoryEvent_Barbarians.spawnBarbarians(city, true);
           
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

        public void EndTutorial()
        {
            player.gameControls.map.setCameraBounds(false, cityarea);
            bool createStartUnits = DssRef.storage.runTutorial_1short_2normal == 2 && 
                missions.sel < TutorialMission.AttackBarbarian;
            DssRef.storage.runTutorial_1short_2normal = 0;

            if (!PlatformSettings.STEAM_DEMO)
            {
                DssRef.storage.Save(null);
            }

            Faction enemyFac = DssRef.world.factions.GetIndex_Safe(DssRef.settings.Faction_Barbarian);
            enemyFac.player.GetAiPlayer().armyAi_enabled = true;

            player.tutorial = null;
            
            display.DeleteMe();

            if (createStartUnits)
            {
                startUnits();
            }
            
            player.hud.messages.blockFoodWarning(false);
            DssRef.state.events.onTutorialEnd();
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
