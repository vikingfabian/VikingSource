using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
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
                //todo check toggle
                var mayBuild = selectedSubTile.MayBuild(player, out bool upgrade);
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {
                    if (mayBuild == MayBuildResult.Yes_ChangeCity)
                    {
                        player.mapSelect(selectedSubTile.city);
                    }


                    if (selectedSubTile.city.availableBuildQueue(player) && placeBuildingOption().blueprint.meetsRequirements(selectedSubTile.city))
                    {
                        player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, selectedSubTile.city, selectedSubTile.subTilePos, placeBuildingType, upgrade), ActionOnConflict.Toggle);
                    }
                    else
                    {
                        //Remove current orders
                        player.orders.orderConflictingSubTile(selectedSubTile.subTilePos, true);
                    }
                }
            }
            else if (buildMode == SelectTileResult.Demolish)
            {
                if (selectedSubTile.MayDemolish(player))
                {
                    player.orders.addOrder(new DemolishOrder(WorkTemplate.MaxPrio, true, selectedSubTile.city, selectedSubTile.subTilePos), ActionOnConflict.Toggle);
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


                                if (subTile.MayBuild(placeBuildingType, out bool upgrade)
                                    &&
                                    !player.orders.orderConflictingSubTile(subTileLoop.Position, false))
                                {
                                    player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, city, subTileLoop.Position, placeBuildingType, upgrade),  ActionOnConflict.Cancel);
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

            List< BuildAndExpandType> available = player.tutorial == null ? BuildLib.AvailableBuildTypes(city) : player.tutorial.AvailableBuildTypes();

            foreach (var opt in available)
            {
                var build = BuildLib.BuildOptions[(int)opt];
                var buildCount = city.buildingStructure.getCount(opt);

                var buttonIcon = new RichBoxImage(build.sprite);
                var buttonContent = new List<AbsRichBoxMember> {
                    buttonIcon,                    
                };
                if (buildCount > 0)
                {
                    buttonContent.Add(new RichBoxOverlapText(buttonIcon,buildCount.ToString(), new Vector2(1.1f, 1.1f), 1.0f, new Vector2(1,1f), Color.White));
                }
                var button = new RichboxButton(buttonContent,
                new RbAction1Arg<BuildAndExpandType>(buildingTypeClick, opt, SoundLib.menu),
                new RbAction1Arg<BuildAndExpandType>((BuildAndExpandType type) =>
                {
                    RichBoxContent content = new RichBoxContent();

                    var build = BuildLib.BuildOptions[(int)type];
                    content.h2(TextLib.LargeFirstLetter(build.Label())).overrideColor = HudLib.TitleColor_TypeName;
                    build.blueprint.toMenu(content, city);

                    content.Add(new RichBoxSeperationLine());
                    HudLib.Description(content, build.Description());

                    content.newLine();
                    switch (type)
                    {
                        case BuildAndExpandType.WaterResovoir:
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.WarsResource_WaterAdd));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Resource_MaxAmount, TextLib.PlusMinus(DssConst.WaterResovoirWaterAdd))));

                            content.newParagraph();
                            HudLib.Label(content, DssRef.lang.Hud_ThisCity);
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Resource_CurrentAmount, city.res_water.amount)));
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Resource_MaxAmount, city.maxWaterTotal)));
                            content.newLine();
                            content.Add(new RichBoxImage(SpriteName.WarsResource_WaterAdd));
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.OneDecimal( city.waterAddPerSec))));
                            break;

                        case BuildAndExpandType.WoodCutter:
                            HudLib.Label(content, DssRef.todoLang.BuildHud_AreaAffectTitle);

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildingType_WoodCutter_AreaAffect, DssConst.WoodCutter_WoodBonus)));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_BonusRadius, DssConst.WoodCutter_BonusRadius)));
                            break;

                        case BuildAndExpandType.StoneCutter:
                            HudLib.Label(content, DssRef.todoLang.BuildHud_AreaAffectTitle);

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildingType_StoneCutter_AreaAffect, DssConst.StoneCutter_StoneBonus)));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.BuildHud_BonusRadius, DssConst.StoneCutter_BonusRadius)));
                            break;

                        case BuildAndExpandType.Storehouse:
                        case BuildAndExpandType.Tavern:
                            HudLib.Description(content, DssRef.lang.Info_FoodAndDeliveryLocation);
                            break;

                        case BuildAndExpandType.Logistics:
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.birdUnLock));
                            if (city.CanBuildLogistics(2))
                            {
                                content.Add(new RichBoxText(string.Format(DssRef.lang.XP_UnlockBuildQueue, DssRef.lang.Hud_NoLimit)));
                            }
                            else
                            {
                                content.Add(new RichBoxText(string.Format(DssRef.lang.XP_UnlockBuildQueue, City.LevelToMaxBuildQueue(1))));
                            }

                            foreach (var building in BuildLib.LogisticsUnlockBuildings)
                            {
                                var opt = BuildLib.BuildOptions[(int)building];
                                content.newLine();
                                HudLib.BulletPoint(content);
                                content.Add(new RichBoxText(DssRef.lang.XP_UnlockBuilding));
                                content.Add(new RichBoxImage(opt.sprite));
                                content.Add(new RichBoxText(opt.Label()));
                            }
                            content.newParagraph();

                            HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxImage(SpriteName.WarsResource_Food));
                            content.space();
                            var reqText = new RichBoxText(string.Format(DssRef.lang.Requirements_XItemStorageOfY, DssRef.lang.Resource_TypeName_Food, City.Logistics1FoodStorage));
                            reqText.overrideColor = city.CanBuildLogistics(1) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                            content.Add(reqText);
                            break;

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
                            farmHud(false, new ItemResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount), ItemResource.Empty);
                            break;
                        case BuildAndExpandType.WheatFarmUpgraded:
                            farmHud(true, new ItemResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount), ItemResource.Empty);
                            //float plantTime = type == BuildAndExpandType.WheatFarm? DssConst.WorkTime_Plant : DssConst.WorkTime_Plant_Upgraded;

                            //content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, plantTime + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            //content.space();
                            //content.Add(new RichBoxText(DssConst.WheatFoodAmount.ToString()));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.LinenFarm:
                            farmHud(false, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.LinenHarvestAmount), ItemResource.Empty);
                            //content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            //content.space();
                            //content.Add(new RichBoxText(TerrainContent.FarmCulture_ReadySize.ToString()));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.LinenFarmUpgraded:
                            farmHud(true, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.LinenHarvestAmount), ItemResource.Empty);
                            break;

                        case BuildAndExpandType.RapeSeedFarm:
                            farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.RapeSeedFuelAmount), ItemResource.Empty);
                            //content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            //content.space();
                            //content.Add(new RichBoxText(DssConst.RapeSeedFuelAmount.ToString()));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Fuel));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Fuel));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;
                        case BuildAndExpandType.RapeSeedFarmUpgraded:
                            farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.RapeSeedFuelAmount), ItemResource.Empty);
                            break;

                        case BuildAndExpandType.HempFarm:
                            farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount));
                            //content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_Plant + DssConst.WorkTime_GatherFoil_FarmCulture))));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                            //content.newLine();
                            //HudLib.BulletPoint(content);
                            //content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            //content.space();
                            //content.Add(new RichBoxText(DssConst.HempLinenAndFuelAmount.ToString()));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_Fuel));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Fuel));
                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            //content.Add(new RichBoxText(DssConst.HempLinenAndFuelAmount.ToString()));
                            //content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            //content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case BuildAndExpandType.HempFarmUpgraded:
                            farmHud(true, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount));
                            break;

                        case BuildAndExpandType.HenPen:
                            content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.HenReady - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce + DssConst.WorkTime_PickUpResource))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText((DssConst.HenRawFoodAmout + DssConst.EggRawFoodAmout).ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));

                            //content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            break;

                        case BuildAndExpandType.PigPen:
                            content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.PigReady - 1))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce))));

                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                            content.space();
                            content.Add(new RichBoxText(DssConst.PigRawFoodAmout.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_RawFood));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_RawFood));
                            content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            content.Add(new RichBoxText(DssConst.PigSkinAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));
                            break;

                        case BuildAndExpandType.Brewery:
                            mayCraftList(content, CraftResourceLib.Beer);
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //content.space();
                            //CraftResourceLib.Beer.toMenu(content, city, false);
                            break;

                        case BuildAndExpandType.Cook:
                            mayCraftList(content, CraftResourceLib.Food1);
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            //content.newLine();
                            //content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //content.space();
                            //CraftResourceLib.Food1.toMenu(content, city, false);

                            //content.newLine();
                            //content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //content.space();
                            //CraftResourceLib.Food2.toMenu(content, city, false);

                            break;

                        case BuildAndExpandType.Carpenter:
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            //foreach (var m in CraftBuildingLib.CarpenterCraftTypes)
                            //{
                            //    content.newLine();
                            //    content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //    content.space();
                            //    ItemPropertyColl.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                            //    bp1.toMenu(content, city, false);
                            //}
                            mayCraftList(content, city, CraftBuildingLib.CarpenterCraftTypes);

                            break;

                        case BuildAndExpandType.WorkBench:
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            //foreach (var m in CraftBuildingLib.BenchCraftTypes)
                            //{
                            //    content.newLine();
                            //    content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //    content.space();
                            //    ItemPropertyColl.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                            //    bp1.toMenu(content, city, false);
                            //}
                            mayCraftList(content, city, CraftBuildingLib.BenchCraftTypes);
                            break;

                        case BuildAndExpandType.Smelter:
                            mayCraftList(content, city, CraftBuildingLib.SmelterCraftTypes);
                            break;

                        case BuildAndExpandType.Foundry:
                            mayCraftList(content, city, CraftBuildingLib.FoundryCraftTypes);
                            break;

                        case BuildAndExpandType.Armory:
                            mayCraftList(content, city, CraftBuildingLib.ArmoryCraftTypes);
                            break;

                        case BuildAndExpandType.Smith:
                            mayCraftList(content, city, CraftBuildingLib.SmithCraftTypes);
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

                            //foreach (var m in CraftBuildingLib.SmithCraftTypes)
                            //{
                            //    content.newLine();
                            //    content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //    content.space();
                            //    ItemPropertyColl.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                            //    bp1.toMenu(content, city, false);
                            //}
                            break;

                        case BuildAndExpandType.CoalPit:
                            mayCraftList(content, CraftResourceLib.Charcoal);
                            //content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;
                            //content.newLine();
                            //content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                            //content.space();
                            //CraftResourceLib.Charcoal.toMenu(content, city, false);
                            break;

                        case BuildAndExpandType.Postal:
                        case BuildAndExpandType.Recruitment:
                            deliveryHud(1);
                            break;

                        case BuildAndExpandType.PostalLevel2:
                        case BuildAndExpandType.RecruitmentLevel2:
                            deliveryHud(2);
                            break;

                        case BuildAndExpandType.PostalLevel3:
                        case BuildAndExpandType.RecruitmentLevel3:
                            deliveryHud(3);
                            break;

                        case BuildAndExpandType.Bank:
                            content.h2(DssRef.lang.XP_UnlockBuilding).overrideColor = HudLib.TitleColor_Label;
                            List<BuildAndExpandType> unlocks = new List<BuildAndExpandType>()
                            {
                                BuildAndExpandType.CoinMinter,
                            };

                            if (!DssRef.storage.centralGold)
                            {
                                unlocks.Add(BuildAndExpandType.GoldDeliveryLvl1);
                            }
                            
                            foreach (var building in unlocks)
                            {
                                var opt = BuildLib.BuildOptions[(int)building];
                                content.newLine();
                                HudLib.BulletPoint(content);
                                content.Add(new RichBoxText(DssRef.lang.XP_UnlockBuilding));
                                content.Add(new RichBoxImage(opt.sprite));
                                content.space();
                                content.Add(new RichBoxText(opt.Label()));
                            }
                            content.newParagraph();

                            content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
                            content.newLine();
                            HudLib.BulletPoint(content);                            
                            content.Add(new RichBoxText(string.Format(DssRef.lang.Economy_TaxIncome, TextLib.PlusMinus(MathExt.PercentageInteger(DssConst.BankTaxIncreasePercUnits)))));
                            content.text(DssRef.todoLang.Hud_EffectDoesNotStack).overrideColor = HudLib.InfoYellow_Light;
                            break;

                    }


                    content.Add(new RichBoxSeperationLine());
                    content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
                    build.blueprint.listResources(content, city);
                    if (type == BuildAndExpandType.Logistics)
                    {
                        bool reachedBuffer = false;
                        city.res_food.toMenu(content, ItemResourceType.Food_G, false, ref reachedBuffer);
                    }

                    if (build.blueprint.levelRequirement > XP.ExperienceLevel.Beginner_1)
                    {
                        content.newLine();

                        HudLib.Experience(content, build.blueprint.experienceType, city.GetTopSkill(build.blueprint.experienceType));
                    }

                    player.hud.tooltip.create(player, content, true);


                    void deliveryHud(int level)
                    {
                        int maxAmount;
                        float speedBonus;

                        switch (level)
                        {
                            default:
                                maxAmount = DssConst.CityDeliveryChunkSize_Level1;
                                speedBonus = 0;
                                break;
                            case 2:
                                maxAmount = DssConst.CityDeliveryChunkSize_Level2;
                                speedBonus = DssConst.DeliveryLevel2TimeReducePerc;
                                break;
                            case 3:
                                maxAmount = DssConst.CityDeliveryChunkSize_Level3;
                                speedBonus = DssConst.DeliveryLevel3TimeReducePerc;
                                break;
                        }

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.Delivery_SendChunk, maxAmount)));
                        if (speedBonus > 0)
                        {
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RichBoxText(string.Format(DssRef.todoLang.Delivery_SpeedBonus, speedBonus)));
                        }
                    }
                    void farmHud(bool upgrade, ItemResource produce1, ItemResource produce2)
                    {
                        float plantTime = upgrade ? DssConst.WorkTime_Plant_Upgraded : DssConst.WorkTime_Plant;

                        content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        var workTimeText = new RichBoxText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, plantTime + DssConst.WorkTime_GatherFoil_FarmCulture)));
                        if (upgrade)
                        {
                            workTimeText.overrideColor = HudLib.AvailableColor;
                        }
                        content.Add(workTimeText);

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                        content.Add(new RichBoxImage(SpriteName.WarsResource_Water));
                        content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Water));

                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RichBoxText(DssRef.lang.BuildHud_Produce));
                        content.space();
                        content.Add(new RichBoxText(produce1.amount.ToString()));
                        content.Add(new RichBoxImage(ResourceLib.Icon(produce1.type)));//SpriteName.WarsResource_RawFood));
                        content.Add(new RichBoxText(LangLib.Item(produce1.type)));//DssRef.lang.Resource_TypeName_RawFood));
                        if (produce2.amount > 0)
                        {
                            content.Add(new RichBoxImage(SpriteName.pjNumPlus));
                            content.Add(new RichBoxText(DssConst.HempLinenAndFuelAmount.ToString()));
                            content.Add(new RichBoxImage(SpriteName.WarsResource_LinenCloth));
                            content.Add(new RichBoxText(DssRef.lang.Resource_TypeName_Linen));
                        }
                    }
                }, opt)
                {

                });

                bool availableBuild = true;
                if (opt == BuildAndExpandType.Logistics)
                {
                    availableBuild = city.CanBuildLogistics(1);
                }

                if (availableBuild)
                {
                    button.setGroupSelectionColor(HudLib.RbSettings, buildMode == SelectTileResult.Build && placeBuildingType == opt);
                }
                else
                {
                    button.enabled = false;
                }
                content.Add(button);
                content.space();

               
            }
            content.Add(new RichBoxScale(1));

            content.newParagraph();

            BuildOption buildOpt = null;

            content.Add(new RichboxButton(new List<AbsRichBoxMember>
            {
                new RichBoxText(DssRef.lang.Build_DestroyBuilding)
            }, new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.Demolish, SoundLib.menu)));

            content.space();

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

                if (buildMode == SelectTileResult.Build)
                {
                    buildOpt = BuildLib.BuildOptions[(int)placeBuildingType];
                }
            }

            int orderLength = 0;
            foreach (var m in player.orders.orders)
            {
                if (m.GetWorkType(city) != OrderType.NONE)
                {
                    orderLength++;
                }
            }
            content.newParagraph();
            autoBuildButton(DssRef.lang.Build_AutoPlace, 1);
            if (buildOpt != null && !buildOpt.uniqueBuilding)
            {
                content.space();
                autoBuildButton(string.Format(DssRef.lang.Hud_XTimes, 4), 4);
            }
            //content.Button(DssRef.lang.Build_AutoPlace, new RbAction(() =>
            //{
            //    autoPlaceBuilding(city, 1);
            //}, SoundLib.menuBuy), null, buildMode == SelectTileResult.Build);
            //content.space();
            //content.Button(string.Format(DssRef.lang.Hud_XTimes, 4), new RbAction(() =>
            //{
            //    autoPlaceBuilding(city, 4);
            //}, SoundLib.menuBuy), null, buildMode == SelectTileResult.Build);

            content.newLine();
            content.Button(DssRef.lang.Build_ClearOrders, new RbAction(() =>
            {
                player.orders.clearAll(city);
            }, SoundLib.menuBack), null, orderLength > 0);

            if (city.buildingStructure.buildingLevel_logistics == 1)
            {
                content.space();
                var upgradeText = new RichBoxText(string.Format(DssRef.lang.XP_UpgradeBuildingX, DssRef.lang.BuildingType_Logistics));
                
                content.Add(new RichboxButton(new List<AbsRichBoxMember>() { upgradeText }, new RbAction(city.upgradeLogistics, SoundLib.menuBuy), new RbAction(()=>
                {
                    RichBoxContent content = new RichBoxContent();
                    HudLib.Label(content, DssRef.lang.XP_Upgrade);
                    content.newLine();
                    CraftBuildingLib.CraftLogistics.toMenu(content, city);

                    content.newParagraph();
                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                    content.newLine();
                    content.text(string.Format(DssRef.lang.BuildingType_Logistics_NationSizeRequirement, DssConst.Logistics2_PopulationRequirement)).overrideColor = city.faction.totalWorkForce>= DssConst.Logistics2_PopulationRequirement? HudLib.AvailableColor : HudLib.NotAvailableColor;

                    content.newParagraph();
                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn);
                    content.newLine();
                    content.icontext(SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers + ": " + TextLib.LargeNumber(city.faction.totalWorkForce));

                    player.hud.tooltip.create(player, content, true);
                }), CraftBuildingLib.CraftLogisticsLevel2.hasResources(city) && city.CanBuildLogistics(2)));
            }

            content.newParagraph();
            content.text(string.Format(DssRef.lang.Build_OrderQue, orderLength)).overrideColor = HudLib.InfoYellow_Light;

            if (city.buildingStructure.buildingLevel_logistics > 0)
            {
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
        
            void autoBuildButton(string caption, int count)
            {
                int max = city.MaxBuildQueue();

                if (max >= count)
                {
                    int current = player.orders.buildQueue(city);

                    content.Button(caption, new RbAction(() =>
                    {
                        autoPlaceBuilding(city, count);
                    }, SoundLib.menuBuy), null, buildOpt != null && (count <= max - current) );
                }
            }

            
        }

        void mayCraftList(RichBoxContent content, City city, ItemResourceType[] types)
        {
            content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

            foreach (var m in types)
            {
                content.newLine();
                content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
                content.space();
                ItemPropertyColl.Blueprint(m, out CraftBlueprint bp1, out CraftBlueprint bp2);
                //bp1.toMenu(content, city, false);
                bp1.resultTypeToMenu(content);
            }
        }
        void mayCraftList(RichBoxContent content, CraftBlueprint bp1)
        {
            content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

           
            content.newLine();
            content.Add(new RichBoxImage(SpriteName.WarsBluePrint));
            content.space();
            //bp1.toMenu(content, city, false);
            bp1.resultTypeToMenu(content);            
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
