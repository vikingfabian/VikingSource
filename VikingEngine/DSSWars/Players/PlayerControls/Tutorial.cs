using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
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
            ConscriptArmy,
            CollectFood,
            MoveArmy,
            Diplomatics,
            End,
        }

        const int CollectWoodStoneAmount = 30;
        const int CollectLinenAmount = 30;
        static int CollectWeaponArmorAmount = DssConst.SoldierGroup_DefaultCount * 2;

        bool collectResources_zoomIn = false;
        bool collectResources_zoomIn_sound = false;
        bool collectResources_selectCity = false;
        bool collectResources_selectCity_sound = false;
        bool collectResources_selectTab = false;
        bool collectResources_selectTab_sound = false;
        bool collectResources_collectwood = false;
        bool collectResources_collectstone = false;

        bool linen_selectTab = false;
        bool linen_build = false;
        bool linen_collect = false;

        bool weaponsArmor_selectTab = false;
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
        //bool moveArmy_Select = false;
        bool moveArmy_SelectMove = false;

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
        TutorialMission tutorialMission = 0;
        Display.TutorialDisplay display;

        public List<MenuTab> cityTabs;
        const int ReachFoodBuffer = City.DefaultFoodBuffer + 100;

        public List<BuildAndExpandType> AvailableBuildTypes()
        {
            var list = new List<BuildAndExpandType>(){
                BuildAndExpandType.WorkerHuts,
                BuildAndExpandType.SoldierBarracks,
      
                //BuildAndExpandType.Brewery,
                //BuildAndExpandType.Cook,
                BuildAndExpandType.CoalPit,
                BuildAndExpandType.WorkBench,
                //BuildAndExpandType.Smith,

                BuildAndExpandType.PigPen,
                BuildAndExpandType.HenPen,
                BuildAndExpandType.WheatFarm,
                BuildAndExpandType.LinenFarm,
            };

            if (tutorialMission >= TutorialMission.CollectFood)
            {
                list.Insert(4, BuildAndExpandType.Cook);
            }
            
            return list;
        }

        public Tutorial(LocalPlayer player)
        {
            cityarea = new Rectangle2();

            this.player = player;
            display = new Display.TutorialDisplay(player);

            //Setup resources and map
            var cityCounter = player.faction.cities.counter();
            while (cityCounter.Next())
            {
                cityCounter.sel.res_wood.amount = 0;
                cityCounter.sel.res_sharpstick.amount = DssConst.SoldierGroup_DefaultCount;//30;
                cityCounter.sel.res_paddedArmor.amount = DssConst.SoldierGroup_DefaultCount;

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
            new TimedAction0ArgTrigger(song, 3000);
        }

        public void song()
        {
            Ref.music.PlaySong(Data.Music.Tutorial, true);
        }

        void refreshLimits()
        {
            player.mapControls.setCameraBounds(tutorialMission < TutorialMission.Diplomatics, cityarea);

            cityTabs = new List<MenuTab>{ MenuTab.Info, MenuTab.Resources };

            if (tutorialMission >= TutorialMission.ProduceWeaponsArmor)
            {
                cityTabs.Add(MenuTab.Work);
            }
            if (tutorialMission >= TutorialMission.Linen)
            {
                cityTabs.Add(MenuTab.Build);
            }
            if (tutorialMission >= TutorialMission.ConscriptArmy)
            {
                cityTabs.Add(MenuTab.Conscript);
            }
            if (tutorialMission >= TutorialMission.CollectFood)
            {
                cityTabs.Add(MenuTab.BlackMarket);
            }

            player.hud.messages.blockFoodWarning(tutorialMission < TutorialMission.CollectFood);
        }

        public bool DisplayStockpile()
        { 
            return tutorialMission >= TutorialMission.CollectFood;
        }
        

        public void tutorial_ToHud(RichBoxContent content)
        {
            content.h1(DssRef.lang.Tutorial_MissionsTitle).overrideColor = HudLib.TitleColor_Label;
            content.h2(string.Format(DssRef.lang.Tutorial_MissionX, ((int)tutorialMission) +1)).overrideColor = HudLib.InfoYellow_Light;

            switch (tutorialMission)
            {
                case TutorialMission.CollectResources:
                    content.icontext(HudLib.CheckImage(collectResources_selectCity), DssRef.lang.Tutorial_SelectACity);
                    content.icontext(HudLib.CheckImage(collectResources_zoomIn), DssRef.lang.Tutorial_ZoomInWorkers);
                    content.icontext(HudLib.CheckImage(collectResources_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    content.icontext(HudLib.CheckImage(collectResources_collectwood), string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.lang.Resource_TypeName_Wood));
                    content.icontext(HudLib.CheckImage(collectResources_collectstone), string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.lang.Resource_TypeName_Stone));
                    break;
               
                case TutorialMission.Linen:
                    content.icontext(HudLib.CheckImage(linen_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.icontext(HudLib.CheckImage(linen_build), string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.LinenFarm].Label()));
                    //content.icontext(HudLib.CheckImage(linen_armorWork), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_LightArmor));
                    content.icontext(HudLib.CheckImage(linen_collect), string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectLinenAmount, DssRef.lang.Resource_TypeName_Linen));
                    break;
                                   

                case TutorialMission.ProduceWeaponsArmor:
                    content.icontext(HudLib.CheckImage(weaponsArmor_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Work));
                    content.icontext(HudLib.CheckImage(weaponsArmor_setWeaponPrio), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_SharpStick));
                    content.icontext(HudLib.CheckImage(weaponsArmor_setArmorPrio), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_LightArmor));
                    content.icontext(HudLib.CheckImage(weaponsArmor_produceWeapons), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_SharpStick));
                    content.icontext(HudLib.CheckImage(weaponsArmor_produceArmor), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, CollectWeaponArmorAmount, DssRef.lang.Resource_TypeName_LightArmor));
                    break;

                case TutorialMission.ConscriptArmy:
                    content.icontext(HudLib.CheckImage(conscriptArmy_build), string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.SoldierBarracks].Label()));
                    content.icontext(HudLib.CheckImage(conscriptArmy_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.icontext(HudLib.CheckImage(conscriptArmy_createArmy), string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.Resource_TypeName_LightArmor));
                    break;

                case TutorialMission.CollectFood:
                    content.icontext(HudLib.CheckImage(CollectFood_selecttab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    content.icontext(HudLib.CheckImage(CollectFood_foodblueprint), DssRef.lang.Tutorial_LookAtFoodBlueprint);//-look at the food blueprint
                    content.icontext(HudLib.CheckImage(CollectFood_buildfoodproduction), string.Format(DssRef.lang.Tutorial_BuildSomething, DssRef.lang.Resource_TypeName_RawFood));//-build something that produces raw food
                    content.icontext(HudLib.CheckImage(CollectFood_buildfuelproduction), string.Format(DssRef.lang.Tutorial_BuildSomething, DssRef.lang.Resource_TypeName_Fuel));//-build something that produces fuel
                    content.icontext(HudLib.CheckImage(CollectFood_builcook), string.Format(DssRef.lang.Tutorial_BuildCraft, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station
                    
                    content.icontext(HudLib.CheckImage(CollectFood_selectStockPile), 
                        string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources) + ". " + string.Format(DssRef.lang.Tutorial_Select_SubTab, DssRef.lang.Resource_Tab_Stockpile));//-build a food crafting station

                    content.icontext(HudLib.CheckImage(CollectFood_increasefoodbuffer), string.Format(DssRef.lang.Tutorial_IncreaseBufferLimit, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station
                    content.icontext(HudLib.CheckImage(CollectFood_reachfoodamount), string.Format(DssRef.lang.Tutorial_CollectItemStockpile, ReachFoodBuffer, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station

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
                    content.icontext(HudLib.CheckImage(moveArmy_ZoomOut), DssRef.lang.Tutorial_ZoomOutOverview);
                    content.icontext(HudLib.CheckImage(moveArmy_SelectMove), DssRef.lang.Tutorial_Mission_MoveArmy);
                    break;
                case TutorialMission.Diplomatics:
                    content.icontext(HudLib.CheckImage(diplomatics_ZoomOut),  DssRef.lang.Tutorial_ZoomOutDiplomacy);
                    content.icontext(HudLib.CheckImage(diplomatics_goodRelation), DssRef.lang.Tutorial_ImproveRelations);
                    break;

            }

            content.newParagraph();
            content.icontext(player.input.Select.Icon, DssRef.lang.Tutorial_SelectInput);            
            content.icontext(player.input.inputSource.IsController? player.input.cameraTiltZoom.Icon : SpriteName.MouseScroll, DssRef.lang.Tutorial_ZoomInput);
            if (tutorialMission == TutorialMission.MoveArmy)
            {
                content.icontext(player.input.Execute.Icon, DssRef.lang.Tutorial_MoveInput);
            }
        }

        public void update()
        {
            switch (tutorialMission)
            {
                case TutorialMission.CollectResources:

                    if (player.mapControls.selection.obj is City)
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

                    if (player.drawUnitsView.current.DrawDetailLayer)
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
                    

                    
                    if (player.cityTab == Display.MenuTab.Resources)
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
                        if (player.mapControls.selection.obj is City)
                        {
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(ItemResourceType.Wood_Group).amount >= CollectWoodStoneAmount)
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
                        if (player.mapControls.selection.obj is City)
                        {
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(ItemResourceType.Stone_G).amount >= CollectWoodStoneAmount)
                            {
                                collectResources_collectstone = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;

                
                case TutorialMission.Linen:
                    if (!linen_selectTab)
                    {
                        if (player.cityTab == Display.MenuTab.Build)
                        {
                            linen_selectTab = true;
                            onPartSuccess();
                        }
                    }
                    if (!linen_build)
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
                    //if (!linen_armorWork)
                    //{
                    //    if (player.mapControls.selection.obj is City &&
                    //        player.mapControls.selection.obj.GetCity().workTemplate.craft_lightarmor.value > 0)
                    //    {
                    //        linen_armorWork = true;

                    //        onPartSuccess();
                    //    }
                    //}
                    if (!linen_collect)
                    {
                        if (player.mapControls.selection.obj is City)
                        {
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(ItemResourceType.SkinLinen_Group).amount >= CollectLinenAmount)
                            {
                                linen_collect = true;
                                onPartSuccess();
                            }
                        }
                    }
                    
                    break;

               
                case TutorialMission.ProduceWeaponsArmor:
                    if (!weaponsArmor_selectTab)
                    {
                        if (player.cityTab == Display.MenuTab.Work)
                        {
                            weaponsArmor_selectTab = true;
                            onPartSuccess();
                        }
                    }
                    if (!weaponsArmor_setWeaponPrio)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().workTemplate.craft_sharpstick.value > 0)
                        {
                            weaponsArmor_setWeaponPrio = true;
                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_setArmorPrio)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().workTemplate.craft_paddedarmor.value > 0)
                        {
                            weaponsArmor_setArmorPrio = true;
                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_produceWeapons)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().res_sharpstick.amount >= CollectWeaponArmorAmount)
                        {
                            weaponsArmor_produceWeapons = true;

                            onPartSuccess();
                        }
                    }

                    if (!weaponsArmor_produceArmor)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().res_paddedArmor.amount >= CollectWeaponArmorAmount)
                        {
                            weaponsArmor_produceArmor = true;

                            onPartSuccess();
                        }
                    }
                    break;

                case TutorialMission.ConscriptArmy:
                    if (!conscriptArmy_build)
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
                    if (!conscriptArmy_selectTab)
                    {
                        if (player.cityTab == Display.MenuTab.Conscript)
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
                        if (player.cityTab == Display.MenuTab.Resources)
                        {
                            CollectFood_selecttab = true;

                            onPartSuccess();
                        }
                    }
                    if (!CollectFood_foodblueprint)
                    {
                        if (player.hud.tooltip.tooltip_id == Tooltip.Food_BlueprintId &&
                            player.hud.tooltip.tooltip_id_timesec >= 2)
                        {
                            CollectFood_foodblueprint = true;

                            onPartSuccess();
                        }
                    }
                    if (!CollectFood_buildfoodproduction)
                    {
                        for (int i = player.orders.orders.Count -1; i>=0; --i)//each (var order in player.orders.orders)
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
                    if (!CollectFood_buildfuelproduction)
                    {
                        for (int i = player.orders.orders.Count - 1; i >= 0; --i)//each (var order in player.orders.orders)
                        {
                            var order = player.orders.orders[i];
                            if (order is BuildOrder)
                            {
                                switch (((BuildOrder)order).buildingType)
                                {
                                    case Build.BuildAndExpandType.CoalPit:
                                        CollectFood_buildfuelproduction = true;
                                        onPartSuccess();
                                        break;
                                }
                                break;
                            }
                        }
                    }
                    if (!CollectFood_builcook)
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

                    if (!CollectFood_selectStockPile)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.cityTab == Display.MenuTab.Resources &&
                            player.resourcesSubTab == ResourcesSubTab.Stockpile_Resources)
                        {
                            CollectFood_selectStockPile = true;

                            onPartSuccess();
                        }
                    }

                    if (!CollectFood_increasefoodbuffer)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().res_food.goalBuffer > City.DefaultFoodBuffer)
                        {
                            CollectFood_increasefoodbuffer = true;

                            onPartSuccess();
                        }
                    }

                    if (!CollectFood_reachfoodamount)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().res_food.amount >= ReachFoodBuffer)
                        {
                            CollectFood_reachfoodamount = true;

                            onPartSuccess();
                        }
                    }

                    break;
              
                case TutorialMission.MoveArmy:

                    if (player.drawUnitsView.current.DrawNormal)
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
                case TutorialMission.Diplomatics:

                    if (player.drawUnitsView.current.DrawOverview)
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

            display.update();
        }

        void onPartSuccess(bool soundPlayed = false)
        {
            if (!soundPlayed)
            {
                SoundLib.trophy.Play();
            }
            display.refresh = true;

            bool missionComplete = false;

            switch (tutorialMission)
            {
                case TutorialMission.CollectResources:
                    missionComplete = collectResources_selectCity &&
                        collectResources_zoomIn &&
                        collectResources_selectTab &&
                        collectResources_collectwood &&
                        collectResources_collectstone;
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

                case TutorialMission.ConscriptArmy:
                    missionComplete = conscriptArmy_build &&
                        conscriptArmy_selectTab &&
                        conscriptArmy_createArmy;
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
            new TimedAction1ArgTrigger<TutorialMission>(nextMission, tutorialMission +1, 1000);
            
        }

        void nextMission(TutorialMission next)
        {
            if (tutorialMission < next)
            {
                tutorialMission = next;
                display.refresh = true;

                if (tutorialMission >= TutorialMission.End)
                {
                    player.hud.messages.Add(DssRef.lang.Tutorial_CompleteTitle, DssRef.lang.Tutorial_CompleteMessage);
                    EndTutorial();
                }
                else
                {
                    refreshLimits();

                    RichBoxContent content = new RichBoxContent();
                    content.h1(DssRef.lang.Tutorial_MissionComplete_Title).overrideColor = HudLib.InfoYellow_Light;
                    content.text(DssRef.lang.Tutorial_MissionComplete_Unlocks);
                    player.hud.messages.Add(content);
                }
            }
        }

        public void tutorial_writeGameState(BinaryWriter w)
        {
            w.Write((int)tutorialMission);
        }

        public void tutorial_readGameState(BinaryReader r, int subversion)
        {
            tutorialMission = (TutorialMission)r.ReadInt32();

            refreshLimits();
        }

        public void EndTutorial()
        {
            player.mapControls.setCameraBounds(false, cityarea);
            

            DssRef.storage.runTutorial = false;
            DssRef.storage.Save(null);

            player.tutorial = null;
            
            display.DeleteMe();

            var factionC =  DssRef.world.factions.counter();
            while (factionC.Next())
            {
                factionC.sel.player.createStartupBarracks();
                factionC.sel.player.createStartUnits();

            }

            player.hud.messages.blockFoodWarning(false);
        }
    }
}
