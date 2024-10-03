﻿using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars.Build
{
    class BuildControls
    {
        static readonly Build.BuildAndExpandType[] AutoBuildOptions =
           {
                Build.BuildAndExpandType.WheatFarm,
                Build.BuildAndExpandType.LinenFarm,
                Build.BuildAndExpandType.PigPen,
                Build.BuildAndExpandType.HenPen,
            };
        public SelectTileResult buildMode = SelectTileResult.None;
        //public BuildOption placeBuildingType = BuildLib.BuildOptions[0];
        public BuildAndExpandType placeBuildingType = BuildAndExpandType.WorkerHuts;

        LocalPlayer player;

        public BuildControls(LocalPlayer player) 
        { 
            this.player = player;
        }

        public BuildOption placeBuildingOption()
        {
            return BuildLib.BuildOptions[(int)placeBuildingType];
        }

        public void onTileSelect(SelectedSubTile selectedSubTile)
        {   
            if (buildMode == SelectTileResult.Build)
            {
                var mayBuild = selectedSubTile.MayBuild(player);
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {
                    //create build order
                    player.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, selectedSubTile.city, selectedSubTile.subTilePos, placeBuildingType));
                }
            }            
        }

        public void autoPlaceBuilding(City city, int count)
        {
            IntVector2 topleft;
            ForXYLoop subTileLoop;

            for (int radius = 1; radius <= city.cityTileRadius; ++radius)
            {
                int distanceValue = -radius;
                ForXYEdgeLoop cirkleLoop = new ForXYEdgeLoop(Rectangle2.FromCenterTileAndRadius(city.tilePos, radius));

                while (cirkleLoop.Next())
                {
                    if (DssRef.world.tileBounds.IntersectTilePoint(cirkleLoop.Position))
                    {
                        var tile = DssRef.world.tileGrid.Get(cirkleLoop.Position);
                        if (tile.CityIndex == city.parentArrayIndex && tile.IsLand())
                        {
                            topleft = WP.ToSubTilePos_TopLeft(cirkleLoop.Position);
                            subTileLoop = new ForXYLoop(topleft, topleft + WorldData.TileSubDivitions_MaxIndex);

                            while (subTileLoop.Next())
                            {
                                var subTile = DssRef.world.subTileGrid.Get(subTileLoop.Position);

                                if (subTile.MayBuild() &&
                                    !player.orderConflictingSubTile(subTileLoop.Position))
                                {
                                    player.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, city, subTileLoop.Position, placeBuildingType));
                                    if (--count <= 0)
                                    { return; }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void toHud(LocalPlayer player, RichBoxContent content, City city)
        {
            content.newParagraph();

            content.Add(new RichBoxScale(2.1f)); 
            foreach (var opt in BuildLib.AvailableBuildTypes)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> {
                    
                    new RichBoxImage(BuildLib.BuildOptions[(int)opt].sprite),

                },
                new RbAction1Arg<BuildAndExpandType>(buildingTypeClick, opt),
                new RbAction1Arg<BuildAndExpandType>((BuildAndExpandType type) =>
                {
                    RichBoxContent content = new RichBoxContent();

                    content.h2(BuildLib.BuildOptions[(int)type].Label()).overrideColor = HudLib.TitleColor_TypeName;
                    var build = BuildLib.BuildOptions[(int)type];
                    
                    HudLib.Description(content, build.Description());

                    content.newLine();
                    switch (type)
                    { 
                        case BuildAndExpandType.Nobelhouse:
                            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);

                            content.BulletPoint();
                            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
                            content.newLine();

                            content.BulletPoint();
                            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
                            content.newLine();

                            content.BulletPoint();
                            content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
                            content.newLine();

                            break;
                    }

                    build.blueprint.toMenu(content, city);

                    player.hud.tooltip.create(player, content, true);
                }, opt));
                
                button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Build && placeBuildingType == opt);
                content.Add(button);
                content.space();
            }
            content.Add(new RichBoxScale(1));

            content.newParagraph();
                       
            if (buildMode != SelectTileResult.None)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> { 
                    new RichBoxSpace(),
                    new RichBoxText(DssRef.todoLang.Hud_EndSessionIcon),
                    new RichBoxSpace(),
                    },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.None));
                button.setGroupSelectionColor(HudLib.RbSettings, false);
                content.Add(button);
                content.space();
            }

            int orderLength = 0;
            foreach (var m in player.orders)
            {
                if (m.GetWorkOrder(city) != null)
                {
                    orderLength++;
                }
            }
            content.newParagraph();
            content.Button("Auto place", new RbAction(() =>
            {
                autoPlaceBuilding(city, 1);
            }), null, buildMode == SelectTileResult.Build);
            content.space();
            content.Button(string.Format(DssRef.lang.Hud_XTimes, 4), new RbAction(() =>
            {
                autoPlaceBuilding(city, 4);
            }), null, buildMode == SelectTileResult.Build);

            content.newLine();
            content.Button("Clear build orders", new RbAction(() =>
            {
                for (int i = player.orders.Count-1; i>=0; --i)
                {
                    if (player.orders[i].GetWorkOrder(city) != null)
                    {
                        player.orders[i].DeleteMe();
                        player.orders.RemoveAt(i);
                    }
                }
            }), null, orderLength > 0);


            content.newParagraph();
            content.text(string.Format( "Build order que: {0}", orderLength)).overrideColor = HudLib.InfoYellow_Light;

            content.Add(new RichBoxSeperationLine());
            content.h2(DssRef.lang.Automation_Title);

            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.todoLang.CityOption_AutoBuild_Work),
                }, city.AutoBuildWorkProperty));
            content.newLine();

            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.todoLang.CityOption_AutoBuild_Farm),
                }, city.AutoBuildFarmProperty));
            //if (autoBuild)
            //{
            //content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
            //{
            //    new RichBoxText(DssRef.todoLang.CityOption_AutoBuild_Intelligent),
            //}, AutoBuildIntelligentProperty));

            if (city.AutoBuildFarmProperty(0, false, false))
            {
                content.newLine();

                foreach (var opt in AutoBuildOptions)
                {
                    var optButton = new RichboxButton(new List<AbsRichBoxMember> {
                    new RichBoxText(BuildLib.BuildOptions[(int)opt].Label())
                    }, new RbAction(() =>
                    {
                        city.autoExpandFarmType = opt;
                    }));
                    optButton.setGroupSelectionColor(HudLib.RbSettings, opt == city.autoExpandFarmType);
                    content.Add(optButton);
                    content.space();
                }
            }

            content.newParagraph();
            
            city.workTemplate.autoBuild.toHud(player, content, DssRef.todoLang.Work_OrderPrioTitle, WorkPriorityType.autoBuild, player.faction, city);
        }

        void modeClick(SelectTileResult set)
        { 
            buildMode = set;
        }

        void buildingTypeClick(BuildAndExpandType type)
        {
            buildMode = SelectTileResult.Build;
            placeBuildingType = type;
        }

        //void buildingTooltip(BuildAndExpandType type)
        //{
        //    RichBoxContent content = new RichBoxContent();

        //    content.h2(BuildLib.BuildOptions[(int)type].Label()).overrideColor=HudLib.TitleColor_TypeName;
        //    var build = BuildLib.BuildOptions[(int)type];

        //    HudLib.Description(content, build.Description());
        //    //CraftBlueprint blueprint = ResourceLib.Blueprint(index);
        //    build.blueprint.toMenu(content, city);

        //    player.hud.tooltip.create(player, content, true);
        //}
        
    }

    
}
