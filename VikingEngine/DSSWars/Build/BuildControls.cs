using Microsoft.Xna.Framework.Graphics;
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
                Build.BuildAndExpandType.RapeSeedFarm,
                Build.BuildAndExpandType.HempFarm,

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
                    //var conflictingOrder = player.orders.orderOnSubTile(selectedSubTile.subTilePos);
                    //if (conflictingOrder == null)
                    //{
                        //create build order
                        player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, selectedSubTile.city, selectedSubTile.subTilePos, placeBuildingType), true);
                    //}
                    //else
                    //{ 
                        
                    //}
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

                                
                                if (subTile.MayBuild()
                                    &&
                                    !player.orders.orderConflictingSubTile(subTileLoop.Position))
                                {
                                    player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, city, subTileLoop.Position, placeBuildingType), false);
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

            List< BuildAndExpandType> available = player.tutorial == null ? BuildLib.AvailableBuildTypes : player.tutorial.AvailableBuildTypes();

            foreach (var opt in available)
            {
                var button = new RichboxButton(new List<AbsRichBoxMember> {
                    
                    new RichBoxImage(BuildLib.BuildOptions[(int)opt].sprite),

                },
                new RbAction1Arg<BuildAndExpandType>(buildingTypeClick, opt, SoundLib.menu),
                new RbAction1Arg<BuildAndExpandType>((BuildAndExpandType type) =>
                {
                    RichBoxContent content = new RichBoxContent();

                    var build = BuildLib.BuildOptions[(int)type];
                    content.h2( TextLib.LargeFirstLetter(build.Label()) ).overrideColor = HudLib.TitleColor_TypeName;
                    build.blueprint.toMenu(content, city);

                    content.Add(new RichBoxSeperationLine());
                    HudLib.Description(content, build.Description());

                    content.newLine();
                    switch (type)
                    { 
                        case BuildAndExpandType.Nobelhouse:
                            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.NobelHouseAddDiplomacy * 3600);

                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticAddTime));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
                            content.newLine();

                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.WarsDiplomaticPoint));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.NobelHouseAddMaxDiplomacy)));
                            content.newLine();

                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
                            content.newLine();

                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.rtsUpkeepTime));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Hud_Upkeep, DssLib.NobleHouseUpkeep)));

                            break;
                        case BuildAndExpandType.WheatFarm:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize -1))));
                            
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format( DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(DssConst.DefaultItemRawFoodAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.LinenFarm:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(TerrainContent.FarmCulture_ReadySize.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.RapeSeedFarm:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(DssConst.DefaultItemFuelAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Fuel));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Fuel));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.HempFarm:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(DssConst.HempFuelAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Fuel));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Fuel));
                            content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            content.Add(new RichBoxText(DssConst.HempFuelAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case BuildAndExpandType.HenPen:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.HenReady - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce + DssConst.WorkTime_PickUpResource))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText((DssConst.HenRawFoodAmout * 2).ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.PigPen:
                            content.h2(DssRef.todoLang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_GrowTime, string.Format(DssRef.todoLang.Hud_Time_Minutes, TerrainContent.PigReady - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_WorkTime, string.Format(DssRef.todoLang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.todoLang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(DssConst.PigRawFoodAmout.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));
                            content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            content.Add(new RichBoxText(DssConst.PigRawFoodAmout.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case BuildAndExpandType.Brewery:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            content.space();
                            ResourceLib.CraftBeer.toMenu(content, city, false);
                            break;

                        case BuildAndExpandType.Cook:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;
                            
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            content.space();
                            ResourceLib.CraftFood1.toMenu(content, city, false);

                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            content.space();
                            ResourceLib.CraftFood2.toMenu(content, city, false);

                            break;

                        case BuildAndExpandType.Carpenter:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            foreach (var m in ResourceLib.CarpenterCraftTypes)
                            {
                                content.newLine();
                                content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                                content.space();
                                ResourceLib.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                                bp1.toMenu(content, city, false);
                            }
                            
                            break;

                        case BuildAndExpandType.WorkBench:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            foreach (var m in ResourceLib.BenchCraftTypes)
                            {
                                content.newLine();
                                content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                                content.space();
                                ResourceLib.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                                bp1.toMenu(content, city, false);
                            }

                            break;

                        case BuildAndExpandType.Smith:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            foreach (var m in ResourceLib.SmithCraftTypes)
                            {
                                content.newLine();
                                content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                                content.space();
                                ResourceLib.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                                bp1.toMenu(content, city, false);
                            }
                            break;

                        case BuildAndExpandType.CoalPit:
                            content.h2(DssRef.todoLang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            content.space();
                            ResourceLib.CraftCharcoal.toMenu(content, city, false);
                            break;

                    }

                    
                    content.Add(new RichBoxSeperationLine());
                    content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                    build.blueprint.listResources(content, city);

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
                    new RichBoxText(DssRef.lang.Hud_EndSessionIcon),
                    new RichBoxSpace(),
                    },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.None, SoundLib.menuBack));
                button.setGroupSelectionColor(HudLib.RbSettings, false);
                content.Add(button);
                content.space();
            }

            int orderLength = 0;
            foreach (var m in player.orders.orders)
            {
                if (m.GetWorkOrder(city) != null)
                {
                    orderLength++;
                }
            }
            content.newParagraph();
            content.Button(DssRef.lang.Build_AutoPlace, new RbAction(() =>
            {
                autoPlaceBuilding(city, 1);
            }, SoundLib.menuBuy), null, buildMode == SelectTileResult.Build);
            content.space();
            content.Button(string.Format(DssRef.lang.Hud_XTimes, 4), new RbAction(() =>
            {
                autoPlaceBuilding(city, 4);
            }, SoundLib.menuBuy), null, buildMode == SelectTileResult.Build);

            content.newLine();
            content.Button(DssRef.lang.Build_ClearOrders, new RbAction(() =>
            {
                player.orders.clearAll(city);
            }, SoundLib.menuBack), null, orderLength > 0);


            content.newParagraph();
            content.text(string.Format(DssRef.lang.Build_OrderQue, orderLength)).overrideColor = HudLib.InfoYellow_Light;

           
            content.Add(new RichBoxSeperationLine());

            //--Automation
            content.h2(DssRef.lang.Automation_Title).overrideColor = HudLib.TitleColor_Label;

            content.newLine();
            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_AutoBuild_Work),
                }, city.AutoBuildWorkProperty));
            content.newLine();

            content.Add(new RichboxCheckbox(new List<AbsRichBoxMember>
                {
                    new RichBoxText( DssRef.lang.CityOption_AutoBuild_Farm),
                }, city.AutoBuildFarmProperty));
            

            if (city.AutoBuildFarmProperty(0, false, false))
            {
                content.newLine();

                foreach (var opt in AutoBuildOptions)
                {
                    var build = BuildLib.BuildOptions[(int)opt];

                    var optButton = new RichboxButton(new List<AbsRichBoxMember> {
                        new RichBoxImage(build.sprite),
                        new RichBoxSpace(),
                        new RichBoxText(build.Label())
                    }, new RbAction(() =>
                    {
                        city.autoExpandFarmType = opt;
                    }, SoundLib.menu));
                    optButton.setGroupSelectionColor(HudLib.RbSettings, opt == city.autoExpandFarmType);
                    content.Add(optButton);
                    content.space();
                }
            }

            content.newParagraph();
            
            city.workTemplate.autoBuild.toHud(player, content, DssRef.lang.Work_OrderPrioTitle, SpriteName.MenuPixelIconSettings, SpriteName.NO_IMAGE, WorkPriorityType.autoBuild, player.faction, city);
        }

        void modeClick(SelectTileResult set)
        { 
            buildMode = set;
        }

        void buildingTypeClick(BuildAndExpandType type)
        {
            buildMode = SelectTileResult.Build;
            placeBuildingType = type;
            player.mapControls.setObjectMenuFocus(false);
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
