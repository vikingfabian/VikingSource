using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VikingEngine;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Graphics;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.LootFest.Players;
using VikingEngine.Sound;
using VikingEngine.ToGG;
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

        public static readonly MapPaintToolShape[] AvailableToolShapes = { MapPaintToolShape.Free, MapPaintToolShape.Line, MapPaintToolShape.LShape, MapPaintToolShape.Area };

        public SelectTileResult buildMode = SelectTileResult.None;
        public BuildAndExpandType placeBuildingType = BuildAndExpandType.WorkerHut;
        public MapPaintToolShape toolShape = MapPaintToolShape.Area;
        LocalPlayer player;
        City city;
        bool blockBuildUpdate = false;

        public BuildControls(LocalPlayer player) 
        { 
            this.player = player;
        }

        public BuildOption placeBuildingOption()
        {
            return BuildLib.BuildOptions[(int)placeBuildingType];
        }

        bool actOnTile(IntVector2 subTilePos, bool commit, out int usesBuildQue, out City city)
        {
            if (buildMode == SelectTileResult.Build)
            {
                usesBuildQue = 1;
                var mayBuild = SelectedSubTile.MayBuild(subTilePos, player, out bool upgrade, out city);
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {

                    if (commit)
                    {
                        //SoundLib.start_build_contruct.Play();

                        if (DssRef.difficulty.GodPowers())
                        {
                            var build = BuildLib.BuildOptions[(int)placeBuildingType];
                            SubTile subTile = DssRef.world.subTileGrid.Get(subTilePos);
                            if (build.execute_async(city, subTilePos, ref subTile, upgrade, false))
                            {
                                EditSubTile edit = new EditSubTile(subTilePos, subTile, true, true, false);
                                edit.Submit();
                            }

                            new GodBuild(subTilePos);
                        }
                        else if (placeBuildingOption().blueprint.meetsRequirements(city))
                        {
                            player.orders.addOrder(player.playerData.localPlayerIndex, new BuildOrder(city.workTemplate.buildOrder.value, true, city, subTilePos, placeBuildingType, upgrade), ActionOnConflict.Toggle);
                        }
                        else
                        {
                            //Remove current orders
                            player.orders.orderConflictingSubTile(subTilePos, true);
                        }
                    }
                    else
                    {
                        if (player.orders.orderConflictingSubTile(subTilePos, false))
                        {
                            usesBuildQue = -1;
                        }
                    }

                    return true;
                }
            }
            else if (buildMode == SelectTileResult.Demolish)
            {
                usesBuildQue = 0;
                if (SelectedSubTile.MayDemolish(subTilePos, player, out city))
                {
                    if (commit)
                    {
                        //SoundLib.start_destroy_contruct.Play();

                        if (DssRef.difficulty.GodPowers())
                        {
                            BuildLib.Demolish(city, subTilePos);
                            new GodBuild(subTilePos);
                        }
                        else
                        {
                            player.orders.addOrder(player.playerData.localPlayerIndex, new DemolishOrder(city.workTemplate.buildOrder.value, true, city, subTilePos), ActionOnConflict.Toggle);
                        }
                    }
                    else
                    {
                        //SoundLib.woodcut.Play();
                    }

                    return true;
                }
            }
            else
            {
                usesBuildQue = 0;
            }

            city = null;
            return false;
        }

        public bool buildKeyDown = false;
        Vector3 keyDownPos;
        IntVector2 startTile, currentTile;
        List<BuildSelection> selection = new List<BuildSelection>();
        LShapeDir lShape;
        class BuildSelection
        {
            public IntVector2 position;
            public bool mayBuild;
            public Mesh model;
            public int usesBuildQue;
            public City City;
        }

        public void updateBuildMode()
        {
            

            if (player.gameControls.input.mouseSelect.DownEvent)
            {
                deleteSelection();
                buildKeyDown = true;
                startTile = player.gameControls.map.hover.subTile.subTilePos;
                keyDownPos = player.gameControls.map.pointerPosWP;
                lShape = LShapeDir.NoSet;
                //actOnTile(player.mapControls.hover.subTile);
            }

            if (buildKeyDown)
            {
                if (player.gameControls.map.hover.subTile.subTilePos != currentTile)
                {
                    //Update paint selection
                    switch (toolShape)
                    {
                        case MapPaintToolShape.Free:
                            if (addToSelection(player.gameControls.map.hover.subTile.subTilePos, true))
                            {
                                refreshSelection();
                            }
                            break;
                        case MapPaintToolShape.Area:
                            {
                                deleteSelection();
                                var area = Rectangle2.FromTwoTilePoints(startTile, player.gameControls.map.hover.subTile.subTilePos);
                                ForXYLoop loop = new ForXYLoop(area);
                                while (loop.Next())
                                {
                                    addToSelection(loop.Position, false);
                                }
                                refreshSelection();
                            }
                            break;
                        case MapPaintToolShape.LShape:
                            {
                                deleteSelection();

                                if (startTile.SideLength(player.gameControls.map.hover.subTile.subTilePos) > 1)
                                {
                                    if (lShape == LShapeDir.NoSet)
                                    {
                                        if (Math.Abs(player.gameControls.map.pointerPosWP.X - keyDownPos.X) > Math.Abs(player.gameControls.map.pointerPosWP.Z - keyDownPos.Z))
                                        {
                                            lShape = LShapeDir.StartX;
                                        }
                                        else
                                        {
                                            lShape = LShapeDir.StartY;
                                        }
                                    }
                                }
                                else
                                {
                                    lShape = LShapeDir.NoSet;
                                }

                                IntVector2 diff = player.gameControls.map.hover.subTile.subTilePos - startTile;
                                IntVector2 dir = new IntVector2(lib.ToLeftRight(diff.X), lib.ToLeftRight(diff.Y));
                                IntVector2 length = new IntVector2(Math.Abs(diff.X), Math.Abs(diff.Y));

                                IntVector2 pos = startTile;

                                //var area = Rectangle2.FromTwoTilePoints(startTile, player.mapControls.hover.subTile.subTilePos);
                                switch (lShape)
                                {
                                    case LShapeDir.NoSet:
                                        {
                                            addToSelection(startTile, false);
                                            addToSelection(player.gameControls.map.hover.subTile.subTilePos, true);
                                        }
                                        break;
                                    case LShapeDir.StartX:
                                        {                                            
                                            for (int xstep = 0; xstep < length.X; xstep++)
                                            {
                                                addToSelection(pos, false);
                                                pos.X += dir.X;
                                            }

                                            for (int ystep = 0; ystep < length.Y; ystep++)
                                            {
                                                addToSelection(pos, true);
                                                pos.Y += dir.Y;
                                            }
                                        }
                                        break;

                                    case LShapeDir.StartY:
                                        {
                                            for (int ystep = 0; ystep < length.Y; ystep++)
                                            {
                                                addToSelection(pos, true);
                                                pos.Y += dir.Y;
                                            }

                                            for (int xstep = 0; xstep < length.X; xstep++)
                                            {
                                                addToSelection(pos, false);
                                                pos.X += dir.X;
                                            }
                                        }
                                        break;
                                }

                                refreshSelection();
                            }
                            break;
                        case MapPaintToolShape.Line:
                            {
                                //How do I make a line that is one tile thick?
                                deleteSelection(); // Clear previous selection

                                IntVector2 start = startTile;
                                IntVector2 end = player.gameControls.map.hover.subTile.subTilePos;

                                int x0 = start.X, y0 = start.Y;
                                int x1 = end.X, y1 = end.Y;

                                int dx = Math.Abs(x1 - x0);
                                int dy = Math.Abs(y1 - y0);
                                int sx = (x0 < x1) ? 1 : -1;
                                int sy = (y0 < y1) ? 1 : -1;
                                int err = dx - dy;

                                IntVector2 pos = start;

                                int loopCount = 0;
                                while (true)
                                {
                                    addToSelection(pos, false); // Add the current position to selection

                                    if (pos.X == x1 && pos.Y == y1) break; // Stop when reaching the endpoint

                                    int e2 = 2 * err;
                                    if (e2 > -dy)
                                    {
                                        err -= dy;
                                        pos.X += sx;
                                    }
                                    if (e2 < dx)
                                    {
                                        err += dx;
                                        pos.Y += sy;
                                    }

                                    if (++loopCount > 1000)
                                    {
                                        throw new EndlessLoopException("MapPaintToolShape.Line");
                                    }
                                }

                                refreshSelection();
                            }
                            break;
                    }
                }
            }

            if (player.gameControls.input.mouseSelect.UpEvent )
            {
               

                bool anySucccess = false;
                int soundIndex = 0;
                SoundContainerBase sound = buildMode == SelectTileResult.Build? SoundLib.start_build_contruct : SoundLib.start_destroy_contruct;
                foreach (var sel in selection)
                {
                    bool success = actOnTile(sel.position, true, out _, out _);

                    if (success)
                    {
                        anySucccess = true;
                        if (soundIndex == 0)
                        {
                            sound.Play();
                        }
                        else if (soundIndex < 2)
                        {
                            sound.PlayDelayed(90 * soundIndex);
                        }
                        soundIndex++;
                    }
                }

                if (!anySucccess)
                {
                    if (blockBuildUpdate)
                    {
                        blockBuildUpdate = false;

                        return;
                    }
                    SoundLib.wrong.Play();
                }

                deleteSelection();
                buildKeyDown = false;
            }

            currentTile = player.gameControls.map.hover.subTile.subTilePos;


            bool addToSelection(IntVector2 subTilePos, bool checkDoublette) 
            {
                if (checkDoublette)
                {
                    foreach (var sel in selection)
                    {
                        if (sel.position == subTilePos)
                        {
                            return false;
                        }
                    }
                }

                bool canAct = actOnTile(subTilePos, false, out int usesBuildQue, out City city);

                var model = SelectedSubTile.CreateOutlineModel(player, false);
                model.Visible = true;
                model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTilePos);
                
                if (!canAct)
                {
                    model.Color = HudLib.NotAvailableColor;
                }

                selection.Add(new BuildSelection() { 
                    position = subTilePos, 
                    mayBuild = canAct,
                    model = model,
                    usesBuildQue = usesBuildQue,
                    City = city,
                });
                return true;
            }

            void refreshSelection()
            {
                //Dictionary<int, int> city_queLength = new Dictionary<int, int>();

                foreach (var sel in selection)
                {
                    if (sel.mayBuild /*&& sel.usesBuildQue != 0*/)
                    {
                        //int availabeQueueLength;
                        //if (!city_queLength.TryGetValue(sel.City.myIndex, out availabeQueueLength))
                        //{
                        //    availabeQueueLength = sel.City.availableBuildQueueLength(player);
                        //    city_queLength.Add(sel.City.myIndex, availabeQueueLength);                           
                        //}

                        sel.model.Color = /*(availabeQueueLength > 0 || sel.usesBuildQue <= 0)  ?*/ Color.White/* : Color.Gray*/;

                        //availabeQueueLength -= sel.usesBuildQue;
                        //city_queLength[sel.City.myIndex] = Bound.Min(availabeQueueLength, 0);
                    }
                }
            }

            void deleteSelection()
            {
                foreach (var sel in selection)
                {
                    sel.model.DeleteMe();
                }
                currentTile = IntVector2.NegativeOne;
                
                selection.Clear();
            }
        }


        public void autoPlaceBuilding(City city, int count)
        {

            BuildAndExpandType buildType = placeBuildingType;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    List<IntVector2> positions = new List<IntVector2>(count);
                    CityStructure structure = new CityStructure();
                    structure.update(city, 1);

                    findBuildPositons_AutoBuilder(positions);
                    if (count > 0)
                    {
                        findBuildPositons_Loop(positions);
                    }

                    foreach (IntVector2 position in positions)
                    {
                        player.orders.addOrder(player.playerData.localPlayerIndex, new BuildOrder(WorkTemplate.MaxPrio, true, city, position, placeBuildingType, false), ActionOnConflict.Cancel, false);
                    }

                    void findBuildPositons_AutoBuilder(List<IntVector2> result)
                    {
                        if (city.buildingStructure.getCount(buildType) > 0)
                        {
                            var prevPos = structure.buildingPosition.getPos(buildType);
                            if (prevPos.X > 0)
                            {
                                findAdjacentFreeSpot(prevPos);
                            }
                        }

                        void findAdjacentFreeSpot(IntVector2 center)
                        {
                            ForXYEdgeLoopRandomPicker Auto_EdgeRandomizer = new ForXYEdgeLoopRandomPicker();
                            for (int r = 1; r <= 2; r++)
                            {
                                Auto_EdgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(center, r));

                                while (Auto_EdgeRandomizer.Next())
                                {
                                    if (structure.MayAutoBuildHere(city, Auto_EdgeRandomizer.Position) &&
                                        !player.orders.orderConflictingSubTile(Auto_EdgeRandomizer.Position, false))
                                    {
                                        result.Add(Auto_EdgeRandomizer.Position);
                                        //player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, city, subTileLoop.Position, placeBuildingType, upgrade), ActionOnConflict.Cancel);
                                        if (--count <= 0)
                                        { return; }
                                    }
                                }
                            }
                        }
                    }


                    void findBuildPositons_Loop(List<IntVector2> result)
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
                                    if (tile.CityIndex == city.myIndex && tile.MayBuild())
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
                                                result.Add(subTileLoop.Position);
                                                //player.orders.addOrder(new BuildOrder(WorkTemplate.MaxPrio, true, city, subTileLoop.Position, placeBuildingType, upgrade), ActionOnConflict.Cancel);
                                                if (--count <= 0)
                                                { return; }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                }
               

            });           
        }

        public List<BuildAndExpandType> availableBuildOptions(City city)
        {
            List<BuildAndExpandType> available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

            if (player.tutorial == null)
            { BuildLib.AvailableBuildTypes(available, city); }
            else
            { available = player.tutorial.AvailableBuildTypes(); }

            List<BuildAndExpandType> availableFiltered = new List<BuildAndExpandType>(available.Count);

            if (player.buildCategoryTab == BuildCategoryTab.Filter)
            {
                foreach (var opt in available)
                {
                    var build = BuildLib.BuildOptions[(int)opt];
                    if (build.Contains(player.buildFilterTag))
                    {
                        availableFiltered.Add(opt);
                    }
                }
            }
            else
            {
                foreach (var opt in available)
                {
                    var build = BuildLib.BuildOptions[(int)opt];
                    if (build.buildCategory == player.buildCategoryTab)
                    {
                        availableFiltered.Add(opt);
                    }
                }
            }
            return availableFiltered;
        }

        //public void toHud_casual(LocalPlayer player, RichBoxContent content, City city)
        //{
        //    List<BuildAndExpandType> available = BuildLib.AvailableBuildTypes_Casual(city);
        //    foreach (var opt in available)
        //    {
                
        //    }
        //}

        public void toHud(LocalPlayer player, RichBoxContent content, City city)
        {
            this.city = city;

            if (player.tutorial != null && player.tutorial.DisplayCompressedBuildTab())
            {
                player.buildCategoryTab = BuildCategoryTab.General;
                content.newParagraph();
            }
            else
            {
                buildTabToHud(player, content);
            }

            if (player.buildCategoryTab == BuildCategoryTab.Automation)
            {
                if (city.buildingStructure.buildingLevel_logistics > 0)
                {
                    content.Add(new RbSeperationLine());

                    //--Automation
                    content.h2(DssRef.lang.Automation_Title).overrideColor = HudLib.TitleColor_Label;

                    content.newLine();
                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember>
                {
                    new RbText( DssRef.lang.CityOption_AutoBuild_Work),
                }, city.AutoBuildWorkProperty));
                    content.newLine();

                    content.Add(new ArtCheckbox(new List<AbsRichBoxMember>
                {
                    new RbText( DssRef.lang.CityOption_AutoBuild_Farm),
                }, city.AutoBuildFarmProperty));


                    if (city.AutoBuildFarmProperty(0, false, false))
                    {
                        content.newLine();

                        foreach (var opt in AutoBuildOptions)
                        {
                            var build = BuildLib.BuildOptions[(int)opt];

                            var optButton = new ArtOption(opt == city.autoExpandFarmType, new List<AbsRichBoxMember> {
                        new RbImage(build.sprite),
                        new RbSpace(),
                        new RbText(build.Label())
                        }, new RbAction(() =>
                        {
                            city.autoExpandFarmType = opt;
                        }, RbSoundType.Option));
                            //optButton.setGroupSelectionColor(HudLib.RbSettings, opt == city.autoExpandFarmType);
                            content.Add(optButton);
                            content.space();
                        }
                    }

                    content.newParagraph();

                    city.workTemplate.autoBuild.toHud(player, content, DssRef.lang.Work_OrderPrioTitle, SpriteName.AutomationGearIcon, SpriteName.NO_IMAGE, WorkPriorityType.autoBuild, player.faction, city);
                }
            }
            else
            {
                buildOptionsToHud(player, content, out BuildOption buildOpt);

                if (player.tutorial == null || !player.tutorial.DisplayCompressedBuildTab())
                {
                    
                        int orderLength = 0;
                        lock (player.orders.orders)
                        {
                            foreach (var m in player.orders.orders)
                            {
                                if (m.GetWorkType(city) != OrderType.NONE)
                                {
                                    orderLength++;
                                }
                            }
                        }
                        content.newParagraph();
                        foreach (MapPaintToolShape shape in AvailableToolShapes)
                        {
                            string caption;
                            SpriteName icon;
                            switch (shape)
                            {
                                default:
                                    caption = DssRef.lang.BuildingToolShape_Free;
                                    icon = SpriteName.ToolPaintShape_Free;
                                    break;
                                case MapPaintToolShape.Line:
                                    caption = DssRef.lang.BuildingToolShape_Line;
                                    icon = SpriteName.ToolPaintShape_Line;
                                    break;
                                case MapPaintToolShape.Area:
                                    caption = DssRef.lang.BuildingToolShape_Area;
                                    icon = SpriteName.ToolPaintShape_Area;
                                    break;
                                case MapPaintToolShape.LShape:
                                    caption = DssRef.lang.BuildingToolShape_LShape;
                                    icon = SpriteName.ToolPaintShape_LShape;
                                    break;
                            }

                            content.Add(new ArtOption(shape == toolShape, new List<AbsRichBoxMember> { new RbImage(icon) },
                                new RbAction1Arg<MapPaintToolShape>((MapPaintToolShape shape) => { toolShape = shape; }, shape, RbSoundType.Option),
                                new RbTooltip_Text(caption)));
                        }


                        content.newParagraph();
                        autoBuildButton(DssRef.lang.Build_AutoPlace, 1);
                        if (buildOpt != null && !buildOpt.uniqueBuilding)
                        {
                            autoBuildButton(string.Format(DssRef.lang.Hud_XTimes, 4), 4);
                        }

                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Build_ClearOrders) },
                            new RbAction(() =>
                            {
                                player.orders.clearAll(city);
                            }, RbSoundType.Back), null, orderLength > 0));
                        content.newLine();
                        content.text(string.Format(DssRef.lang.Build_OrderQue, orderLength), HudLib.InfoYellow_Light);

                        content.newLine();
                        HudLib.Label(content, DssRef.lang.Work_OrderPrioTitle);
                        content.newLine();
                        city.workTemplate.buildOrder.toHud(player, content, DssRef.lang.Build_Order, SpriteName.WarsHammer, SpriteName.warsBuildCategoryHouse, WorkPriorityType.buildOrders,
                            player.faction, city);

                    
                }

                if (player.buildCategoryTab == BuildCategoryTab.Upgrade)
                {

                    if (city.buildingStructure.buildingLevel_logistics == 1)
                    {

                        var upgradeText = new RbText(string.Format(DssRef.lang.XP_UpgradeBuildingX, DssRef.lang.BuildingType_Logistics));
                        content.newParagraph();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember>() {
                            new RbImage(SpriteName.WarsBuild_Logistics),
                            new RbSpace(),
                            upgradeText },
                            new RbAction(city.upgradeLogistics, RbSoundType.Buy), new RbTooltip((RichBoxContent content, object tag) =>
                        {
                            var cityFaction = city.GetFaction();

                            HudLib.Label(content, DssRef.lang.XP_Upgrade);
                            content.newLine();
                            CraftBuildingLib.CraftLogisticsLevel2.toMenu(content, city);

                            content.newParagraph();
                            HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                            content.newLine();
                            content.text(string.Format(DssRef.lang.BuildingType_Logistics_NationSizeRequirement, DssConst.Logistics2_PopulationRequirement)).overrideColor = cityFaction.totalWorkForce >= DssConst.Logistics2_PopulationRequirement ? HudLib.AvailableColor : HudLib.NotAvailableColor;

                            content.newParagraph();
                            HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Gain);
                            content.newLine();
                            HudLib.BulletPoint(content);
                            content.Add(new RbImage(SpriteName.birdUnLock));
                            
                            content.Add(new RbText(string.Format(DssRef.lang.XP_UnlockBuildPrio, City.LevelToMaxBuildPrio(2))));
                            
                            foreach (var building in BuildLib.LogisticsUnlockBuildings_Level2)
                            {
                                var opt = BuildLib.BuildOptions[(int)building];
                                content.newLine();
                                HudLib.BulletPoint(content);
                                content.Add(new RbText(DssRef.lang.XP_UnlockBuilding, HudLib.SecondaryTextColor));
                                content.Add(new RbImage(opt.sprite));
                                content.space();
                                content.Add(new RbText(opt.Label()));
                            }
                            
                            content.newParagraph();
                            HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn);
                            content.newLine();
                            CraftBuildingLib.CraftLogisticsLevel2.listResources(content, city);
                            content.icontext(SpriteName.WarsWorker, DssRef.lang.ResourceType_Workers + ": " + TextLib.LargeNumber(cityFaction.totalWorkForce));
                            
                        }), CraftBuildingLib.CraftLogisticsLevel2.hasResources(city) && city.CanBuildLogistics(2)));
                    }


                    if (city.cityType < CityType.Capital)
                    {
                        content.newLine();
                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                        new RbImage(SpriteName.WarsCityHall),
                        new RbSpace(),
                        new RbText(DssRef.lang.CityHall_Upgrade)
                        }, new RbAction(city.upgradeCityHall), new RbTooltip(city.upgradeCityHallTooltip),
                                city.CanUpgradeCityHall()));
                    }
                }

                void autoBuildButton(string caption, int count)
                {
                    //int max = city.MaxBuildQueue();

                    //if (max >= count)
                    //{
                        int current = player.orders.buildQueue(city);

                        content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(caption) },
                            new RbAction(() =>
                            {
                                autoPlaceBuilding(city, count);
                            }, RbSoundType.Buy), null, buildOpt != null/* && (count <= max - current)*/));
                    //}
                }
            }
            
        }

        void buildTabToHud(LocalPlayer player, RichBoxContent content)
        {
            List<BuildCategoryTab> buildCategories = new List<BuildCategoryTab>
            {

                BuildCategoryTab.General,
                BuildCategoryTab.Advanced,
                BuildCategoryTab.Military,
                BuildCategoryTab.Decor,
                BuildCategoryTab.Upgrade,
                BuildCategoryTab.Filter,

            };

            if (DssRef.difficulty.setting_gameMode == Data.GameModeMainType.Spectator)
            {
                buildCategories.Insert(buildCategories.Count - 1, BuildCategoryTab.GodPower);
            }

            if (city.buildingStructure.buildingLevel_logistics > 0)
            {
                buildCategories.Add(BuildCategoryTab.Automation);
            }

            foreach (var tab in buildCategories)
            {
                string category;
                SpriteName tabIcon;
                switch (tab)
                {
                    case BuildCategoryTab.Filter:
                        tabIcon = SpriteName.warsBuildCategorySearch;
                        category = DssRef.lang.HUD_Filter;
                        break;
                    case BuildCategoryTab.General:
                        tabIcon = SpriteName.warsBuildCategoryHouse;
                        category = DssRef.lang.BuildCategory_General;
                        break;
                    case BuildCategoryTab.Advanced:
                        tabIcon = SpriteName.warsBuildCategoryAdvanced;
                        category = DssRef.lang.Hud_Advanced;
                        break;
                    case BuildCategoryTab.Military:
                        tabIcon = SpriteName.warsBuildCategoryMilitaryWall;
                        category = DssRef.lang.BuildCategory_Military;
                        break;
                    case BuildCategoryTab.Decor:
                        tabIcon = SpriteName.warsBuildCategoryDecorTree;
                        category = DssRef.lang.BuildCategory_Decoration;
                        break;
                    case BuildCategoryTab.Upgrade:
                        tabIcon = SpriteName.warsBuildCategoryUpgrades;
                        category = DssRef.lang.BuildCategory_Upgrade;
                        break;
                    case BuildCategoryTab.GodPower:
                        tabIcon = SpriteName.WarsGodPowerIcon;
                        category = DssRef.lang.GodPower;
                        break;
                    default:
                        tabIcon = SpriteName.warsBuildCategoryAutomation;
                        category = DssRef.lang.Automation_Title;
                        break;

                }
                var tabButton = new ArtButton(tab == player.buildCategoryTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                    new List<AbsRichBoxMember> { new RbImage(tabIcon) },
                    new RbAction1Arg<BuildCategoryTab>((BuildCategoryTab selectTab) => { player.buildCategoryTab = selectTab; }, tab, RbSoundType.Tab),
                    new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_category, category)));
                content.Add(tabButton);
            }
        }


        void buildOptionsToHud(LocalPlayer player, RichBoxContent content, out BuildOption buildOpt)
        {
            bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.Build, out Color focusColor);
            if (viewControllerTabs)
            {
                content.newLine();
                content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                content.space(0.5f);
                content.Add(new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor });
                content.newLine();
            }

            List<BuildAndExpandType> available = availableBuildOptions(city);

            if (player.buildCategoryTab == BuildCategoryTab.Filter)
            {
                content.newLine();
                for (BuildFilterTag tag = 0; tag < BuildFilterTag.NUM_NONE; ++tag)
                {
                    content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(LangLib.Filter(tag)) },
                        new RbAction1Arg<BuildFilterTag>((BuildFilterTag tag) => { player.buildFilterTag = tag; }, tag),
                        null, true, player.buildFilterTag == tag ? Color.White : Color.Gray));
                }
            }
            content.Add(new RichBoxScale(2.1f));
            content.newLine();

            foreach (var opt in available)
            {
                var build = BuildLib.BuildOptions[(int)opt];

                var buildCount = city.buildingStructure.getCount(opt);

                var buttonIcon = new RbImage(build.sprite);
                var buttonContent = new List<AbsRichBoxMember> { buttonIcon };
                if (buildCount > 0)
                {
                    buttonContent.Add(new RbOverlapText(buttonIcon, buildCount.ToString(), new Vector2(1.1f, 1.1f), 1.0f, new Vector2(1, 1f), Color.White));
                }

                var button = new ArtToggle(buildMode == SelectTileResult.Build && placeBuildingType == opt, buttonContent,
                new RbAction1Arg<BuildAndExpandType>(buildingTypeClick, opt, RbSoundType.Option),
                new RbTooltip(buildingTooltip, opt));


                bool availableBuild = true;
                if (opt == BuildAndExpandType.Logistics)
                {
                    availableBuild = city.CanBuildLogistics(1);
                }

                button.enabled = availableBuild;


                content.Add(button);



                //}
            }


            content.Add(new RichBoxScale(1));

            content.newParagraph();

            buildOpt = null;

            content.Add(new ArtToggle(buildMode == SelectTileResult.Demolish, new List<AbsRichBoxMember>
                {
                    new RbText(DssRef.lang.Build_DestroyBuilding)
                }, new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.Demolish, RbSoundType.Option)));

            content.space();

            if (buildMode != SelectTileResult.None)
            {
                var button = new ArtButton(RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_EndSessionIcon),
                    new RbSpace(),
                    },
                    new RbAction1Arg<SelectTileResult>(modeClick, SelectTileResult.None, RbSoundType.Back));
                button.setGroupSelectionColor(HudLib.RbSettings, false);
                content.Add(button);
                content.space();

                if (buildMode == SelectTileResult.Build)
                {
                    buildOpt = BuildLib.BuildOptions[(int)placeBuildingType];
                }
            }
        }

        void buildingTooltip(RichBoxContent content, object tag)
        {
            BuildAndExpandType type = (BuildAndExpandType)tag;

            var build = BuildLib.BuildOptions[(int)type];
            content.h2(TextLib.LargeFirstLetter(build.Label())).overrideColor = HudLib.TitleColor_TypeName;

            //content.newLine();
            build.blueprint.toMenu(content, city, false, true, true);
            if (build.altBlueprint != null)
            {
                content.Add(new RbSeperationLine(HudLib.TitleColor_Head, 0.2f));
                build.altBlueprint.toMenu(content, city, false, false);
            }
            content.Add(new RbSeperationLine(HudLib.TitleColor_Head, 0.2f));
            //content.newLine();
            content.Add(new RbText(DssRef.lang.BuildHud_BuildTime + ":", HudLib.TitleColor_Label));
            content.space();
            content.Add(new RbText(string.Format(DssRef.lang.Hud_Time_Seconds, build.buildTimeSec)));

            content.Add(new RbSeperationLine());
            HudLib.Description(content, build.Description());

            content.newLine();
            switch (type)
            {
                case BuildAndExpandType.ResearchCenter:
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.BuildingType_ResearchCenter_Description, DssConst.TechnologyGain_ResearchCenter)));
                    content.newParagraph();
                    content.Add(new RbText(LangLib.TechnologyExample(), HudLib.InfoYellow_Light));
                    break;

                case BuildAndExpandType.BookPress:
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsUnitLevelBasic));
                    content.space();
                    content.Add(new RbText(string.Format(DssRef.lang.BuildingType_Bookpress_Description, DssRef.lang.BuildingType_ReseachCenter)));
                    content.newParagraph();
                    content.Add(new RbText(LangLib.TechnologyExample(), HudLib.InfoYellow_Light));
                    break;



                case BuildAndExpandType.WaterResovoir:
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
                    content.Add(new RbText(string.Format(DssRef.lang.Resource_MaxAmount, TextLib.PlusMinus(DssConst.WaterResovoirWaterAdd))));

                    content.newParagraph();
                    HudLib.Label(content, DssRef.lang.Hud_ThisCity);
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsResource_Water));
                    content.Add(new RbText(string.Format(DssRef.lang.Resource_CurrentAmount, city.res_water.amount)));
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsResource_Water));
                    content.Add(new RbText(string.Format(DssRef.lang.Resource_MaxAmount, city.maxWaterTotal)));
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsResource_WaterAdd));
                    content.Add(new RbText(string.Format(DssRef.lang.Resource_AddPerSec, TextLib.OneDecimal(city.waterAddPerSec))));
                    break;

                case BuildAndExpandType.WoodCutter:
                    HudLib.Label(content, DssRef.lang.BuildHud_AreaEffectTitle);

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildingType_WoodCutter_AreaAffect, DssConst.WoodCutter_WoodBonus)));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_BonusRadius, DssConst.WoodCutter_BonusRadius)));

                    content.newParagraph();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Hud_Unlock + ": "));
                    content.Add(new RbImage(SpriteName.WarsBuild_TreeSeedlingSoft));
                    content.Add(new RbText(DssRef.lang.Building_TreeSprout_Soft));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Hud_Unlock + ": "));
                    content.Add(new RbImage(SpriteName.WarsBuild_TreeSeedlingHard));
                    content.Add(new RbText(DssRef.lang.Building_TreeSprout_Hard));


                    break;

                case BuildAndExpandType.StoneCutter:
                    HudLib.Label(content, DssRef.lang.BuildHud_AreaEffectTitle);

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildingType_StoneCutter_AreaAffect, DssConst.StoneCutter_StoneBonus)));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_BonusRadius, DssConst.StoneCutter_BonusRadius)));
                    break;

                case BuildAndExpandType.Storehouse:
                case BuildAndExpandType.Tavern:
                    HudLib.Description(content, DssRef.lang.Info_FoodAndDeliveryLocation);
                    break;

                case BuildAndExpandType.Logistics:
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.birdUnLock));
                    if (city.CanBuildLogistics(2))
                    {
                        content.Add(new RbText(string.Format(DssRef.lang.XP_UnlockBuildPrio, DssRef.lang.Hud_NoLimit)));
                    }
                    else
                    {
                        content.Add(new RbText(string.Format(DssRef.lang.XP_UnlockBuildPrio, City.LevelToMaxBuildPrio(1))));
                    }

                    foreach (var building in BuildLib.LogisticsUnlockBuildings)
                    {
                        var opt = BuildLib.BuildOptions[(int)building];
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding, HudLib.SecondaryTextColor));
                        content.Add(new RbImage(opt.sprite));
                        content.space();
                        content.Add(new RbText(opt.Label()));
                    }
                    content.newParagraph();

                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_Food));
                    content.space();
                    var reqText = new RbText(string.Format(DssRef.lang.Requirements_XItemStorageOfY, DssRef.lang.Resource_TypeName_Food, DssConst.Logistics1FoodStorage));
                    reqText.overrideColor = city.CanBuildLogistics(1) ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    content.Add(reqText);
                    break;

                case BuildAndExpandType.Nobelhouse:


                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
                    content.newLine();

                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.Hud_Unlock + ": "));
                    content.Add(new RbImage(SpriteName.WarsBuild_Embassy));
                    content.Add(new RbText(DssRef.lang.BuildingType_Embassy));
                    content.newLine();

                    //HudLib.BulletPoint(content);
                    //content.Add(new RbImage(SpriteName.rtsUpkeepTime));
                    //content.Add(new RbText(string.Format(DssRef.lang.Hud_Upkeep, Resource.Money.CopperToGoldString_Decimal( DssConst.NobleHouseUpkeep_copp))));

                    break;

                case BuildAndExpandType.Embassy:
                    EmbassyDescription(content);

                    //int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.EmbassyAddDiplomacy * 3600);

                    //HudLib.BulletPoint(content);
                    //content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
                    //content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
                    //content.newLine();

                    //HudLib.BulletPoint(content);
                    //content.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
                    //content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.EmbassyAddMaxDiplomacy)));
                    //content.newLine();
                    break;

                case BuildAndExpandType.WheatFarm:
                    farmHud(false, new ItemResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount), ItemResource.Empty);
                    break;
                case BuildAndExpandType.WheatFarmUpgraded:
                    farmHud(true, new ItemResource(ItemResourceType.RawFood_Group, DssConst.WheatFoodAmount), ItemResource.Empty);

                    break;

                case BuildAndExpandType.LinenFarm:
                    farmHud(false, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.LinenHarvestAmount), ItemResource.Empty);

                    break;

                case BuildAndExpandType.LinenFarmUpgraded:
                    farmHud(true, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.LinenHarvestAmount), ItemResource.Empty);
                    break;

                case BuildAndExpandType.RapeSeedFarm:
                    farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.RapeSeedFuelAmount), ItemResource.Empty);

                    break;
                case BuildAndExpandType.RapeSeedFarmUpgraded:
                    farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.RapeSeedFuelAmount), ItemResource.Empty);
                    break;

                case BuildAndExpandType.HempFarm:
                    farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount));
                    break;

                case BuildAndExpandType.HempFarmUpgraded:
                    farmHud(true, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount));
                    break;

                case BuildAndExpandType.HenPen:
                    content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.HenReady - 1))));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce + DssConst.WorkTime_PickUpResource))));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                    content.space();
                    content.Add(new RbText((DssConst.HenRawFoodAmout + DssConst.EggRawFoodAmout).ToString()));
                    content.Add(new RbImage(SpriteName.WarsResource_RawFood));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_RawFood));
                    break;

                case BuildAndExpandType.PigPen:
                    content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.PigReady - 1))));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce))));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                    content.space();
                    content.Add(new RbText(DssConst.PigRawFoodAmout.ToString()));
                    content.Add(new RbImage(SpriteName.WarsResource_RawFood));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_RawFood));
                    content.Add(new RbImage(SpriteName.pjNumPlus));
                    content.Add(new RbText(DssConst.PigSkinAmount.ToString()));
                    content.Add(new RbImage(SpriteName.WarsResource_LinenCloth));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_Linen));
                    break;

                case BuildAndExpandType.Brewery:
                    mayCraftList(content, CraftResourceLib.Beer);
                    break;

                case BuildAndExpandType.Cook:
                    mayCraftList(content, CraftResourceLib.Food1);

                    break;

                case BuildAndExpandType.Carpenter:
                    mayCraftList(content, city, CraftBuildingLib.CarpenterCraftTypes);

                    break;

                case BuildAndExpandType.WorkBench:
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
                    break;

                case BuildAndExpandType.Gunmaker:
                    mayCraftList(content, city, CraftBuildingLib.GunmakerCraftTypes);
                    break;

                case BuildAndExpandType.CoalPit:
                    mayCraftList(content, CraftResourceLib.Charcoal);
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

                    //if (!DssRef.storage.centralGold)
                    //{
                    //    unlocks.Add(BuildAndExpandType.GoldDeliveryLvl1);
                    //}

                    foreach (var building in unlocks)
                    {
                        var opt = BuildLib.BuildOptions[(int)building];
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding));
                        content.Add(new RbImage(opt.sprite));
                        content.space();
                        content.Add(new RbText(opt.Label()));
                    }
                    content.newParagraph();

                    content.h2(DssRef.lang.Hud_PurchaseTitle_Gain).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.Economy_TaxIncome, TextLib.PlusMinus(DssConst.BankTaxIncreasePercUnits_copp * Money.CopperToGold))));
                    content.text(DssRef.lang.Hud_EffectDoesNotStack).overrideColor = HudLib.InfoYellow_Light;
                    break;

            }

            //TAGS
            content.Add(new RbSeperationLine());
            HudLib.Label(content, DssRef.lang.HUD_Tags);
            content.space();
            filterTag(build.filterTag1);
            filterTag(build.filterTag2);
            filterTag(build.filterTag3);

            void filterTag(BuildFilterTag value)
            {
                if (value != BuildFilterTag.NUM_NONE)
                {
                    content.Add(new RbImage(SpriteName.WhiteArea, 0.4f));
                    content.hspace();
                    content.Add(new RbText(LangLib.Filter(value), HudLib.TitleColor_TypeName));
                    content.space(2);
                    //content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText(LangLib.Filter(value), Color.Black) }, null, null, false, Color.Gray));
                }
            }

            //RESOURCES
            buildTooltip_YouOwn(city, content, type);

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
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Delivery_SendChunk, maxAmount)));
                if (speedBonus > 0)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.Delivery_SpeedBonus, speedBonus)));
                }
            }
            void farmHud(bool upgrade, ItemResource produce1, ItemResource produce2)
            {
                float plantTime = upgrade ? DssConst.WorkTime_Plant_Upgraded : DssConst.WorkTime_Plant;

                content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.FarmCulture_ReadySize - 1))));

                content.newLine();
                HudLib.BulletPoint(content);
                var workTimeText = new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, plantTime + DssConst.WorkTime_GatherFoil_FarmCulture)));
                if (upgrade)
                {
                    workTimeText.overrideColor = HudLib.AvailableColor;
                }
                content.Add(workTimeText);

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, DssConst.PlantWaterCost)));
                content.Add(new RbImage(SpriteName.WarsResource_Water));
                content.Add(new RbText(DssRef.lang.Resource_TypeName_Water));

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                content.space();
                content.Add(new RbText(produce1.amount.ToString()));
                content.Add(new RbImage(ResourceLib.Icon(produce1.type)));//SpriteName.WarsResource_RawFood));
                content.Add(new RbText(LangLib.Item(produce1.type)));//DssRef.lang.Resource_TypeName_RawFood));
                if (produce2.amount > 0)
                {
                    content.Add(new RbImage(SpriteName.pjNumPlus));
                    content.Add(new RbText(DssConst.HempLinenAndFuelAmount.ToString()));
                    content.Add(new RbImage(SpriteName.WarsResource_LinenCloth));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_Linen));
                }
            }
        }

        public static void buildTooltip_YouOwn(City city, RichBoxContent content, BuildAndExpandType type)
        {
            var build = BuildLib.BuildOptions[(int)type];

            content.Add(new RbSeperationLine());
            content.h2(DssRef.lang.Hud_PurchaseTitle_CurrentlyOwn, HudLib.TitleColor_Head2);
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
        }

        public static void EmbassyDescription(RichBoxContent content)
        {
            int diplomacydSec = Convert.ToInt32(DssRef.diplomacy.EmbassyAddDiplomacy * 3600);

            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsDiplomaticAddTime));
            content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsAdd, diplomacydSec)));
            content.newLine();

            HudLib.BulletPoint(content);
            content.Add(new RbImage(SpriteName.WarsDiplomaticPoint));
            content.Add(new RbText(string.Format(DssRef.lang.Building_NobleHouse_DiplomacyPointsLimit, DssRef.diplomacy.EmbassyAddMaxDiplomacy)));
            content.newLine();
        }

        void mayCraftList(RichBoxContent content, City city, ItemResourceType[] types)
        {
            content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

            foreach (var m in types)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.WarsBluePrint));
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
            content.Add(new RbImage(SpriteName.WarsBluePrint));
            content.space();
            //bp1.toMenu(content, city, false);
            bp1.resultTypeToMenu(content);            
        }

        void modeClick(SelectTileResult set)
        {
            if (player.gameControls.input.inputSource.IsController)
            {
                blockBuildUpdate = true;
            }
            buildMode = set;
            if (buildMode == SelectTileResult.Demolish)
            {
                player.gameControls.setMenuFocus(false, true);
            }
        }

        public void buildingTypeClick(BuildAndExpandType type)
        {
            if (player.gameControls.input.inputSource.IsController)
            {
                blockBuildUpdate = true;
            }

            buildMode = SelectTileResult.Build;
            placeBuildingType = type;
            player.gameControls.setMenuFocus(false, true);

            
            //player.gameControls.mapControls.setObjectMenuFocus(false);
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
