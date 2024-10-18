using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
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
            SharpStickWork,
            Linen,
            ConscriptArmy,
            CollectFood,
            MoveArmy,
            Diplomatics,
            End,
        }

        const int CollectWoodStoneAmount = 30;
        const int CollectLinenAmount = 4 * 30;

        bool collectResources_zoomIn = false;
        bool collectResources_selectCity = false;
        bool collectResources_selectTab = false;
        bool collectResources_collectwood = false;
        bool collectResources_collectstone = false;

        bool sharpStickWork_selectTab = false;
        bool sharpStickWork_setPrio = false;

        bool linen_selectTab = false;
        bool linen_build = false;
        bool linen_armorWork = false;
        bool linen_collect = false;

        bool conscriptArmy_build = false;
        bool conscriptArmy_selectTab = false;
        bool conscriptArmy_createArmy = false;

        bool CollectFood_selecttab = false;
        bool CollectFood_foodblueprint = false;
        bool CollectFood_buildfoodproduction = false;
        bool CollectFood_buildfuelproduction = false;
        bool CollectFood_builcook = false;
        bool CollectFood_increasefoodbuffer = false;
        bool CollectFood_reachfoodamount = false;

        bool moveArmy_ZoomOut = false;
        //bool moveArmy_Select = false;
        bool moveArmy_SelectMove = false;

        bool diplomatics_ZoomOut = false;
        bool diplomatics_goodRelation = false;

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

        public Tutorial(LocalPlayer player)
        {
            this.player = player;
            display = new Display.TutorialDisplay(player);

            //Setup resources and map
            var cityCounter = player.faction.cities.counter();
            while (cityCounter.Next())
            {
                cityCounter.sel.res_wood.amount = 0;
                CityStructure.Singleton.setupTutorialMap(cityCounter.sel);
            }

            player.faction.workTemplate.craft_sharpstick.value = 0;
            player.faction.workTemplate.craft_lightarmor.value = 0;
            player.faction.refreshCityWork();

            refreshLimits();
        }

        void refreshLimits()
        {
            player.mapControls.setZoomRange(tutorialMission < TutorialMission.Diplomatics);

            cityTabs = new List<MenuTab>{ MenuTab.Info, MenuTab.Resources };

            if (tutorialMission >= TutorialMission.SharpStickWork)
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
                case TutorialMission.SharpStickWork:
                    content.icontext(HudLib.CheckImage(sharpStickWork_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Work));
                    content.icontext(HudLib.CheckImage(sharpStickWork_setPrio), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_SharpStick));
                    break;
                case TutorialMission.Linen:
                    content.icontext(HudLib.CheckImage(linen_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Build));
                    content.icontext(HudLib.CheckImage(linen_build), string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.LinenFarm].Label()));
                    content.icontext(HudLib.CheckImage(linen_armorWork), string.Format(DssRef.lang.Tutorial_IncreasePriorityOnX, DssRef.lang.Resource_TypeName_LightArmor));
                    content.icontext(HudLib.CheckImage(linen_collect), string.Format(DssRef.lang.Tutorial_CollectXAmountOfY, CollectLinenAmount, DssRef.lang.Resource_TypeName_SkinAndLinen));
                    break;
               
                case TutorialMission.ConscriptArmy:
                    content.icontext(HudLib.CheckImage(conscriptArmy_build), string.Format(DssRef.lang.Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Barracks].Label()));
                    content.icontext(HudLib.CheckImage(conscriptArmy_selectTab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.Conscription_Title));
                    content.icontext(HudLib.CheckImage(conscriptArmy_createArmy), string.Format(DssRef.lang.Tutorial_CreateSoldiers, DssRef.lang.Resource_TypeName_SharpStick, DssRef.lang.Resource_TypeName_LightArmor));
                    break;

                case TutorialMission.CollectFood:
                    string BuildSomething = "Build something that produces {0}";
                    string BuildCraft = "Build a crafting station for: {0}";
                    string IncreaseBufferLimit = "Increase buffer limit for: {0}";
                    string CollectFoodStockpile = "Reach a stockpile of {0} food";
                    string LookAtFoodBlueprint = "Look at the food blueprint";
                    string CollectFood_Info1 = "The workers will walk to the city hall for food";
                    string CollectFood_Info2 = "The army sends tross workers to collect food";

                    content.icontext(HudLib.CheckImage(CollectFood_selecttab), string.Format(DssRef.lang.Tutorial_SelectTabX, DssRef.lang.MenuTab_Resources));
                    content.icontext(HudLib.CheckImage(CollectFood_foodblueprint), LookAtFoodBlueprint);//-look at the food blueprint
                    content.icontext(HudLib.CheckImage(CollectFood_buildfoodproduction), string.Format(BuildSomething, DssRef.lang.Resource_TypeName_RawFood));//-build something that produces raw food
                    content.icontext(HudLib.CheckImage(CollectFood_buildfuelproduction), string.Format(BuildSomething, DssRef.lang.Resource_TypeName_Fuel));//-build something that produces fuel
                    content.icontext(HudLib.CheckImage(CollectFood_builcook), string.Format(BuildCraft, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station
                    content.icontext(HudLib.CheckImage(CollectFood_increasefoodbuffer), string.Format(IncreaseBufferLimit, DssRef.lang.Resource_TypeName_Food));//-build a food crafting station
                    content.icontext(HudLib.CheckImage(CollectFood_reachfoodamount), string.Format(CollectFoodStockpile, ReachFoodBuffer));//-build a food crafting station

                    content.newLine();
                    content.BulletPoint();
                    var info1 = new RichBoxText(CollectFood_Info1);
                    info1.overrideColor = HudLib.InfoYellow_Light;
                    content.Add(info1);

                    content.newLine();
                    content.BulletPoint();
                    var info2 = new RichBoxText(CollectFood_Info2);
                    info1.overrideColor = HudLib.InfoYellow_Light;
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
                    if (!collectResources_selectCity)
                    {
                        if (player.mapControls.selection.obj is City)
                        {   
                            collectResources_selectCity = true;
                            onPartSuccess();
                        }
                    }
                    if (!collectResources_zoomIn)
                    {
                        if (player.drawUnitsView.current.DrawDetailLayer)
                        {
                            collectResources_zoomIn = true;
                            onPartSuccess();
                        }
                    }
                    if (!collectResources_selectTab)
                    {
                        if (player.cityTab == Display.MenuTab.Resources)
                        {
                            collectResources_selectTab = true;
                            onPartSuccess();
                        }
                    }
                    if (!collectResources_collectwood)
                    {
                        if (player.mapControls.selection.obj is City)
                        {
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(GameObject.Resource.ItemResourceType.Wood_Group).amount >= CollectWoodStoneAmount)
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
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(GameObject.Resource.ItemResourceType.Stone_G).amount >= CollectWoodStoneAmount)
                            {
                                collectResources_collectstone = true;
                                onPartSuccess();
                            }
                        }
                    }
                    break;

                case TutorialMission.SharpStickWork:
                    if (!sharpStickWork_selectTab)
                    {
                        if (player.cityTab == Display.MenuTab.Work)
                        {
                            sharpStickWork_selectTab = true;
                            onPartSuccess();
                        }
                    }
                    if (!sharpStickWork_setPrio)
                    {
                        if (player.mapControls.selection.obj is City && 
                            player.mapControls.selection.obj.GetCity().workTemplate.craft_sharpstick.value > 0)
                        {
                            sharpStickWork_setPrio = true;
                            onPartSuccess();
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
                    if (!linen_armorWork)
                    {
                        if (player.mapControls.selection.obj is City &&
                            player.mapControls.selection.obj.GetCity().workTemplate.craft_lightarmor.value > 0)
                        {
                            linen_armorWork = true;

                            onPartSuccess();
                        }
                    }
                    if (!linen_collect)
                    {
                        if (player.mapControls.selection.obj is City)
                        {
                            if (player.mapControls.selection.obj.GetCity().GetGroupedResource(GameObject.Resource.ItemResourceType.SkinLinen_Group).amount >= CollectLinenAmount)
                            {
                                linen_collect = true;
                                onPartSuccess();
                            }
                        }
                    }
                    
                    break;
                case TutorialMission.ConscriptArmy:
                    if (!conscriptArmy_build)
                    {   
                        foreach (var order in player.orders.orders)
                        {
                            if (order is BuildOrder && ((BuildOrder)order).buildingType == Build.BuildAndExpandType.Barracks)
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
                        foreach (var order in player.orders.orders)
                        {
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
                        foreach (var order in player.orders.orders)
                        {
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
                        foreach (var order in player.orders.orders)
                        {
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
                    if (!moveArmy_ZoomOut)
                    {
                        if (player.drawUnitsView.current.DrawNormal)
                        {
                            moveArmy_ZoomOut = true;
                            onPartSuccess();
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
                    if (!diplomatics_ZoomOut)
                    {
                        if (player.drawUnitsView.current.DrawOverview)
                        {
                            diplomatics_ZoomOut = true;
                            onPartSuccess();
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

        void onPartSuccess()
        {
            SoundLib.trophy.Play();
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
                case TutorialMission.SharpStickWork:
                    missionComplete = sharpStickWork_setPrio;
                    break;
                case TutorialMission.Linen:
                    missionComplete = linen_selectTab &&
                        linen_build &&
                        linen_armorWork &&
                        linen_collect;
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
            player.mapControls.setZoomRange(false);
            

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
        }
    }
}
