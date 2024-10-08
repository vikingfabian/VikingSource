﻿using System;
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
using VikingEngine.Timer;
using VikingEngine.ToGG;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    
    class Tutorial
    {
        //# tutorial
        //    zoom in on a city
        //    select resource tab, collect x wood and x stone

        //new tab, increase sharp stick work

        //new tab, build linen
        //collect x linnen
        //increase light armor work

        //new tab conscript
        //build conscript
        //que up two armies with light armor and stick

        //zoom out
        //select move army

        //zoom out to diplomatic view,
        //create good relations w neighbor

        //>end
        //-add start soldiers(and conscripts)
        enum TutorialMission
        {
            CollectResources,
            SharpStickWork,
            Linen,
            ConscriptArmy,
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

        bool moveArmy_ZoomOut = false;
        //bool moveArmy_Select = false;
        bool moveArmy_SelectMove = false;

        bool diplomatics_ZoomOut = false;
        bool diplomatics_goodRelation = false;


        LocalPlayer player;
        TutorialMission tutorialMission = 0;
        Display.TutorialDisplay display;

        public List<MenuTab> cityTabs; //=  { MenuTab.Info, MenuTab.Resources, MenuTab.BlackMarket, MenuTab.Work, MenuTab.Build, MenuTab.Delivery, MenuTab.Conscript };

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

        public void tutorial_ToHud(RichBoxContent content)
        {
            string Tutorial_MissionX = "Mission {0}";
            string Tutorial_CollectXAmountOfY = "Collect {0} {1}";
            string Tutorial_SelectTabX = "Select tab: {0}";
            string Tutorial_IncreasePriorityOnX = "Increase the priority on: {0}";
            string Tutorial_PlaceBuildOrder = "Place build order: {0}";
            string Tutorial_ZoomInput = "Zoom";

            content.h1(DssRef.lang.Tutorial_MissionsTitle).overrideColor = HudLib.TitleColor_Label;
            content.h2(string.Format(Tutorial_MissionX, ((int)tutorialMission) +1)).overrideColor = HudLib.InfoYellow_Light;

            switch (tutorialMission)
            {
                case TutorialMission.CollectResources:
                    content.icontext(HudLib.CheckImage(collectResources_selectCity), "Select a city");
                    content.icontext(HudLib.CheckImage(collectResources_zoomIn), "Zoom in to see the workers");
                    content.icontext(HudLib.CheckImage(collectResources_selectTab), string.Format(Tutorial_SelectTabX, DssRef.todoLang.MenuTab_Resources));
                    content.icontext(HudLib.CheckImage(collectResources_collectwood), string.Format(Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.todoLang.Resource_TypeName_Wood));
                    content.icontext(HudLib.CheckImage(collectResources_collectstone), string.Format(Tutorial_CollectXAmountOfY, CollectWoodStoneAmount, DssRef.todoLang.Resource_TypeName_Stone));
                    break;
                case TutorialMission.SharpStickWork:
                    content.icontext(HudLib.CheckImage(sharpStickWork_selectTab), string.Format(Tutorial_SelectTabX, DssRef.todoLang.MenuTab_Work));
                    content.icontext(HudLib.CheckImage(sharpStickWork_setPrio), string.Format(Tutorial_IncreasePriorityOnX, DssRef.todoLang.Resource_TypeName_SharpStick));
                    break;
                case TutorialMission.Linen:
                    content.icontext(HudLib.CheckImage(linen_selectTab), string.Format(Tutorial_SelectTabX, DssRef.todoLang.MenuTab_Build));
                    content.icontext(HudLib.CheckImage(linen_build), string.Format(Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.LinenFarm].Label()));
                    content.icontext(HudLib.CheckImage(linen_armorWork), string.Format(Tutorial_IncreasePriorityOnX, DssRef.todoLang.Resource_TypeName_LightArmor));
                    content.icontext(HudLib.CheckImage(linen_collect), string.Format(Tutorial_CollectXAmountOfY, CollectLinenAmount, DssRef.todoLang.Resource_TypeName_SkinAndLinen));
                    break;
                case TutorialMission.ConscriptArmy:
                    content.icontext(HudLib.CheckImage(conscriptArmy_build), string.Format(Tutorial_PlaceBuildOrder, Build.BuildLib.BuildOptions[(int)Build.BuildAndExpandType.Barracks].Label()));
                    content.icontext(HudLib.CheckImage(conscriptArmy_selectTab), string.Format(Tutorial_SelectTabX, DssRef.todoLang.Conscription_Title));
                    content.icontext(HudLib.CheckImage(conscriptArmy_createArmy), string.Format("Create two soldier groups with the equipment: {0} and {1}", DssRef.todoLang.Resource_TypeName_SharpStick, DssRef.todoLang.Resource_TypeName_LightArmor));
                    break;
                case TutorialMission.MoveArmy:
                    content.icontext(HudLib.CheckImage(moveArmy_ZoomOut), "Zoom out, to map overview");
                    content.icontext(HudLib.CheckImage(moveArmy_SelectMove), DssRef.lang.Tutorial_Mission_MoveArmy);
                    break;
                case TutorialMission.Diplomatics:
                    content.icontext(HudLib.CheckImage(diplomatics_ZoomOut), "Zoom out, to diplomacy view");
                    content.icontext(HudLib.CheckImage(diplomatics_goodRelation), "Improve your relations with a neihbor faction");
                    break;

            }

            content.newParagraph();
            content.icontext(player.input.Select.Icon, DssRef.lang.Tutorial_SelectInput);            
            content.icontext(player.input.inputSource.IsController? player.input.cameraTiltZoom.Icon : SpriteName.MouseScroll, Tutorial_ZoomInput);
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
                case TutorialMission.MoveArmy:
                    if (!moveArmy_ZoomOut)
                    {
                        if (player.drawUnitsView.current.DrawOverview)
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
                        if (player.drawUnitsView.current.DrawFullOverview)
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
                    content.h1("Mission complete!").overrideColor = HudLib.InfoYellow_Light;
                    content.text("New controls has been unlocked");
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