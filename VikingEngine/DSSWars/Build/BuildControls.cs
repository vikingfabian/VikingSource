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
using VikingEngine.DSSWars.Defence;
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
            Build.BuildAndExpandType.OrchardApple,
            Build.BuildAndExpandType.WheatFarm,
            Build.BuildAndExpandType.LinenFarm,
            Build.BuildAndExpandType.RapeSeedFarm,
            Build.BuildAndExpandType.HempFarm,

            //Build.BuildAndExpandType.PigPen,
            //Build.BuildAndExpandType.HenPen,
        };

        public static readonly MapPaintToolShape[] AvailableToolShapes = { MapPaintToolShape.Free, MapPaintToolShape.Line, MapPaintToolShape.LShape, MapPaintToolShape.Area };

        
        public SelectTileResult buildMode = SelectTileResult.None;
        public BuildAndExpandType placeBuildingType = BuildAndExpandType.OrchardApple;
        bool availableBuildingType = true;
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

        public void checkBuildAvailable(City city)
        {
            this.city = city;
            List<BuildAndExpandType> available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);
            BuildLib.AvailableBuildTypes(available, city, false);
            availableBuildingType = available.Contains(placeBuildingType);
            
        }

        public MayBuildResult adjustMayBuild( MayBuildResult mayBuild)
        {
            if (!availableBuildingType)
            {
                mayBuild = MayBuildResult.No_OutsideRegion;
            }

            return mayBuild;
        }

        bool actOnTile(IntVector2 subTilePos, bool commit, out int usesBuildQue, out City city)
        {
            if (buildMode == SelectTileResult.Build)
            {
                usesBuildQue = 1;
                var mayBuild = adjustMayBuild(SelectedSubTile.MayBuild(subTilePos, player, out bool upgrade, out city));
                
                //if (!availableBuildingType)
                //{
                //    mayBuild = MayBuildResult.No_OutsideRegion;
                //}
                
                if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
                {

                    if (commit)
                    {
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
                            player.orders.addOrder(player.playerData.localPlayerIndex, new BuildOrder(city.workTemplate.Get(WorkPriorityType.buildOrders).value, true, city, subTilePos, placeBuildingType, upgrade), ActionOnConflict.Toggle);
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
                        if (DssRef.difficulty.GodPowers())
                        {
                            BuildLib.Demolish(city, subTilePos);
                            new GodBuild(subTilePos);
                        }
                        else
                        {
                            player.orders.addOrder(player.playerData.localPlayerIndex, new DemolishOrder(city.workTemplate.Get(WorkPriorityType.buildOrders).value, true, city, subTilePos), ActionOnConflict.Toggle);
                        }
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
                    structure.update( DssRef.world, city, 1);

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
                            for (int r = 0; r <= 2; r++)
                            {
                                if (r == 0)
                                {
                                    Auto_EdgeRandomizer.addDir4(center);
                                    Auto_EdgeRandomizer.add(center);
                                }
                                else
                                { 
                                    Auto_EdgeRandomizer.start(Rectangle2.FromCenterTileAndRadius(center, r));
                                }

                                while (Auto_EdgeRandomizer.Next())
                                {
                                    if (city.MayAutoBuildHere(Auto_EdgeRandomizer.Position) &&
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

                        int cityradius = city.cityTileArea.size.SideLength() / 2;
                        for (int radius = 1; radius <= cityradius; ++radius)
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

        public List<BuildAndExpandType> availableBuildOptions(City city, bool viewTabs)
        {
            List<BuildAndExpandType> available = new List<BuildAndExpandType>((int)BuildAndExpandType.NUM_NONE);

            if (player.tutorial == null || player.tutorial.AdvisorMode())
            { BuildLib.AvailableBuildTypes(available, city, false); }
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
                    if (build.buildCategory == player.buildCategoryTab || !viewTabs)
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
            bool viewTabs;

            if (player.tutorial != null && player.tutorial.DisplayCompressedBuildTab())
            {
                viewTabs = false;
                player.buildCategoryTab = BuildCategoryTab.General;
                content.newParagraph();
            }
            else
            {
                viewTabs = true;
                buildTabToHud(content);
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

                    city.workTemplate.Get(WorkPriorityType.autoBuild).toHud(player, content, DssRef.lang.Work_OrderPrioTitle, SpriteName.AutomationGearIcon, SpriteName.NO_IMAGE, WorkPriorityType.autoBuild, player.faction, city, ItemResourceType.NONE);
                    
                }
            }
            else
            {
                BuildOption buildOpt = null;
                if (buildMode != SelectTileResult.None)
                {
                    if (buildMode == SelectTileResult.Build)
                    {
                        buildOpt = BuildLib.BuildOptions[(int)placeBuildingType];
                    }
                }

                upgradeButtons(player, content, city, buildOpt);

                buildOptionsToHud(content, viewTabs);

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
                    autoBuildButton(content, DssRef.lang.Build_AutoPlace, 1, buildOpt);
                    if (buildOpt != null && !buildOpt.uniqueBuilding)
                    {
                        autoBuildButton(content, string.Format(DssRef.lang.Hud_XTimes, 4), 4, buildOpt);
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
                    city.workTemplate.Get(WorkPriorityType.buildOrders).toHud(player, content, DssRef.lang.Build_Order, SpriteName.WarsHammer, SpriteName.warsBuildCategoryHouse, WorkPriorityType.buildOrders,
                        player.faction, city, ItemResourceType.NONE);


                }
                
            }

        }

        private void upgradeButtons(LocalPlayer player, RichBoxContent content, City city, BuildOption buildOpt)
        {
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

           
        }

        void autoBuildButton(RichBoxContent content, string caption, int count, BuildOption buildOpt)
        {
            //int max = city.MaxBuildQueue();

            //if (max >= count)
            //{
            int current = player.orders.buildQueue(city);

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(caption) },
                new RbAction(() =>
                {
                    autoPlaceBuilding(city, count);
                }, RbSoundType.Buy), null, buildOpt != null && buildOpt.blueprint.meetsRequirements(city)));
            //}
        }

        void buildTabToHud(RichBoxContent content)
        {
            List<BuildCategoryTab> buildCategories = new List<BuildCategoryTab>
            {
                BuildCategoryTab.General,
                BuildCategoryTab.Farming,
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
                IconName.BuildCategory(tab, out SpriteName tabIcon, out string category);
                
                var tabButton = new ArtButton(tab == player.buildCategoryTab ? RbButtonStyle.SubTabSelected : RbButtonStyle.SubTabNotSelected,
                    new List<AbsRichBoxMember> { new RbImage(tabIcon) },
                    new RbAction1Arg<BuildCategoryTab>((BuildCategoryTab selectTab) => { player.buildCategoryTab = selectTab; }, tab, RbSoundType.Tab),
                    new RbTooltip_Text(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_category, category)));
                content.Add(tabButton);
            }
        }


        void buildOptionsToHud(RichBoxContent content, bool viewTabs)
        {
            bool viewControllerTabs = player.gameControls.tabFocusColor(Players.PlayerControls.ControllerTabFocus.Build, out Color focusColor);
            if (viewControllerTabs && player.gameControls.input.Controller_TabLeft.IsActive && player.gameControls.input.Controller_TabRight.IsActive)
            {
                content.newLine();
                content.Add(new RbImage(player.gameControls.input.Controller_TabLeft.Icon) { color = focusColor });
                content.space(0.5f);
                content.Add(new RbImage(player.gameControls.input.Controller_TabRight.Icon) { color = focusColor });
                content.newLine();
            }

            List<BuildAndExpandType> available = availableBuildOptions(city, viewTabs);

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
            }


            content.Add(new RichBoxScale(1));

            content.newParagraph();

            //buildOpt = null;

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

                //if (buildMode == SelectTileResult.Build)
                //{
                //    buildOpt = BuildLib.BuildOptions[(int)placeBuildingType];
                //}
            }
        }

        void buildingTooltip(RichBoxContent content, object tag)
        {
            BuildAndExpandType type = (BuildAndExpandType)tag;

            BuildOption build = BuildLib.BuildOptions[(int)type];
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

                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Requirement);
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbImage(SpriteName.WarsResource_Food));
                    content.space();
                    bool canBuild = city.CanBuildLogistics(1);
                    content.Add(new RbImage(canBuild ? HudLib.AvailableIcon : HudLib.NotAvailableIcon));
                    content.hspace();
                    var reqText = new RbText(string.Format(DssRef.lang.Requirements_XItemStorageOfY, DssRef.lang.Resource_TypeName_Food, DssConst.Logistics1FoodStorage));
                    reqText.overrideColor = canBuild ? HudLib.AvailableColor : HudLib.NotAvailableColor;
                    content.Add(reqText);

                    content.newParagraph();
                    HudLib.Label(content, DssRef.lang.Hud_PurchaseTitle_Gain);
                    content.newLine();
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
                    //content.newParagraph();

                    
                    break;

                case BuildAndExpandType.ManorLord:
                    foreach (var building in BuildLib.ManorUnlockBuildings)
                    {
                        var opt = BuildLib.BuildOptions[(int)building];
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(DssRef.lang.XP_UnlockBuilding, HudLib.SecondaryTextColor));
                        content.Add(new RbImage(opt.sprite));
                        content.space();
                        content.Add(new RbText(opt.Label()));
                    }

                    break;

                case BuildAndExpandType.Nobelhouse:


                    HudLib.BulletPoint(content);
                    //content.Add(new RbText(DssRef.lang.Building_NobleHouse_UnlocksKnight));
                    content.Add(new RbText(string.Format(DssRef.todoLang.NobelHouse_HousingCount, DssConst.NobelHouseMenCount)));
                    
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

                case BuildAndExpandType.OrchardApple:
                case BuildAndExpandType.OrchidBanana:
                    farmHud_any(false, new ItemResource(ItemResourceType.Food_G, DssConst.OrchidFoodAmount), ItemResource.Empty, 
                        TerrainContent.OrchardReady - TerrainContent.OrchardWatered, DssConst.WorkTime_PluckOrchards, DssConst.OrchardWaterCost);
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
                    farmHud(true, new ItemResource(ItemResourceType.Fuel_G, DssConst.RapeSeedFuelAmount), ItemResource.Empty);
                    break;

                case BuildAndExpandType.HempFarm:
                    farmHud(false, new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount));
                    break;

                case BuildAndExpandType.HempFarmUpgraded:
                    farmHud(true, new ItemResource(ItemResourceType.SkinLinen_Group, DssConst.HempLinenAndFuelAmount), new ItemResource(ItemResourceType.Fuel_G, DssConst.HempLinenAndFuelAmount));
                    break;

                case BuildAndExpandType.TrapperHut:
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.BuildHud_AreaEffectTitle));

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.todoLang.BuildHud_AreaRadius, DssConst.TrapperHutRadius)));

                    break;

                case BuildAndExpandType.HenPen:
                    //content.h2(DssRef.lang.BuildHud_PerCycle, HudLib.TitleColor_Label);
                    //content.newLine();
                    //HudLib.BulletPoint(content);
                    //content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.HenGrowth.harvestReady - 1))));

                    //content.newLine();
                    //HudLib.BulletPoint(content);
                    //content.Add(new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce + DssConst.WorkTime_PickUpResource))));

                    pen(build, TerrainContent.HenGrowth, ItemResourceType.Hen, false, false);

                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                    content.space();
                    content.Add(new RbText((DssConst.HenRawFoodAmout + DssConst.EggRawFoodAmout).ToString()));
                    content.Add(new RbImage(SpriteName.WarsResource_RawFood));
                    content.Add(new RbText(DssRef.lang.Resource_TypeName_RawFood));
                    break;

                case BuildAndExpandType.PigPen:
                    pen(build, TerrainContent.PigGrowth, ItemResourceType.Pig, false, false);                                    
                    break;

                //content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                //content.newLine();
                //HudLib.BulletPoint(content);
                //content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, TerrainContent.PigGrowth.harvestReady - 1))));

                //content.newLine();
                //HudLib.BulletPoint(content);
                //content.Add(new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce))));

                //content.newLine();
                //HudLib.BulletPoint(content);
                //content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                //content.space();
                //IconName.Item(ItemResourceType.Pig, out SpriteName itemIcon, out string itemName);
                //content.Add(new RbImage(itemIcon));
                //content.hspace();
                //content.Add(new RbText(itemName));    
                //content.Add(new RbText(DssConst.PigRawFoodAmout.ToString()));
                //content.Add(new RbImage(SpriteName.WarsResource_RawFood));
                //content.Add(new RbText(DssRef.lang.Resource_TypeName_RawFood));
                //content.Add(new RbImage(SpriteName.pjNumPlus));
                //content.Add(new RbText(DssConst.PigSkinAmount.ToString()));
                //content.Add(new RbImage(SpriteName.WarsResource_LinenCloth));
                //content.Add(new RbText(DssRef.lang.Resource_TypeName_Linen));

                case BuildAndExpandType.OxenPen:
                    pen(build,TerrainContent.OxenGrowth, ItemResourceType.Oxen, true, false);
                    break;
                case BuildAndExpandType.KineOxenPen:
                    pen(build, TerrainContent.KineOxenGrowth, ItemResourceType.KineOxen, false, true);
                    break;

                case BuildAndExpandType.DogCage:
                    pen(build, TerrainContent.DogGrowth, ItemResourceType.Dog, true, false);
                    break;
                case BuildAndExpandType.HoundCage:
                    pen(build, TerrainContent.HoundGrowth, ItemResourceType.Hound, false, true);
                    break;

                case BuildAndExpandType.PonyPen:
                    pen(build, TerrainContent.PonyGrowth, ItemResourceType.Pony, true, false);
                    break;
                case BuildAndExpandType.HorsePen:
                    pen(build, TerrainContent.HorseGrowth, ItemResourceType.Horse, true, true);
                    break;
                case BuildAndExpandType.WarHorsePen:
                    pen(build, TerrainContent.WarHorseGrowth, ItemResourceType.WarHorse, false, true);
                    break;
                case BuildAndExpandType.DraftHorsePen:
                    pen(build, TerrainContent.DraftHorseGrowth, ItemResourceType.DraftHorse, false, true);
                    break;

                case BuildAndExpandType.WildPigPen:
                    pen(build, TerrainContent.WildPigGrowth, ItemResourceType.WildPig, true, false);
                    break;
                case BuildAndExpandType.WildHogPen:
                    pen(build, TerrainContent.WildHogGrowth, ItemResourceType.WildHog, true, true);
                    break;
                case BuildAndExpandType.WarHogPen:
                    pen(build, TerrainContent.WarHogGrowth, ItemResourceType.WarHog, false, true);
                    break;
                case BuildAndExpandType.StagHogPen:
                    pen(build, TerrainContent.StagHogGrowth, ItemResourceType.StagHog, false, true);
                    break;

                case BuildAndExpandType.WolfCage:
                    pen(build, TerrainContent.WolfGrowth, ItemResourceType.Wolf, true, false);
                    break;
                case BuildAndExpandType.WargCage:
                    pen(build, TerrainContent.WargGrowth, ItemResourceType.Warg, true, true);
                    break;
                case BuildAndExpandType.AlphaWargCage:
                    pen(build, TerrainContent.AlphaWargGrowth, ItemResourceType.AlphaWarg, false, true);
                    break;

                case BuildAndExpandType.WildCatCage:
                    pen(build, TerrainContent.WildCatGrowth, ItemResourceType.WildCat, true, false);
                    break;
                case BuildAndExpandType.LionCage:
                    pen(build, TerrainContent.LionGrowth, ItemResourceType.Lion, true, true);
                    break;
                case BuildAndExpandType.WarLionCage:
                    pen(build, TerrainContent.WarLionGrowth, ItemResourceType.WarLion, false, true);
                    break;

                case BuildAndExpandType.ElephantCage:
                    pen(build, TerrainContent.ElephantGrowth, ItemResourceType.Elephant, true, false);
                    break;
                case BuildAndExpandType.WarElephantCage:
                    pen(build, TerrainContent.WarElephantGrowth, ItemResourceType.WarElephant, true, true);
                    break;
                case BuildAndExpandType.OliphantCage:
                    pen(build, TerrainContent.OliphantGrowth, ItemResourceType.Oliphant, false, true);
                    break;

                case BuildAndExpandType.Brewery:
                    mayCraftList(content, CraftResourceLib.Beer);
                    break;

                case BuildAndExpandType.Cook:
                    mayCraftList(content, CraftResourceLib.Food1);
                    break;

                case BuildAndExpandType.Butcher:
                    //mayCraftList(content, city, CraftList.ButcherAnimalCraftTypes);
                    break;
                case BuildAndExpandType.Pottery:
                    mayCraftList(content, city, CraftList.PotteryCraftTypes);
                    break;
                case BuildAndExpandType.ShieldMaker:
                    mayCraftList(content, city, CraftList.ShieldMakerCraftTypes);
                    break;
                case BuildAndExpandType.Smoker:
                    mayCraftList(content, CraftResourceLib.ConservedFood_Smoked);
                    break;
                case BuildAndExpandType.Dryer:
                    mayCraftList(content, CraftResourceLib.ConservedFood_Dried);
                    break;


                case BuildAndExpandType.Carpenter:
                    mayCraftList(content, city, CraftList.CarpenterCraftTypes);

                    break;

                case BuildAndExpandType.WorkBench:
                    mayCraftList(content, city, CraftList.BenchCraftTypes);
                    break;

                case BuildAndExpandType.Smelter:
                    mayCraftList(content, city, CraftList.SmelterCraftTypes);
                    break;

                case BuildAndExpandType.Foundry:
                    mayCraftList(content, city, CraftList.FoundryCraftTypes);
                    break;

                case BuildAndExpandType.Armory:
                    mayCraftList(content, city, CraftList.ArmoryCraftTypes);
                    break;

                case BuildAndExpandType.Smith:
                    mayCraftList(content, city, CraftList.SmithCraftTypes);
                    break;

                case BuildAndExpandType.Gunmaker:
                    mayCraftList(content, city, CraftList.GunmakerCraftTypes);
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

                case BuildAndExpandType.DirtWall:
                case BuildAndExpandType.DirtTower:
                case BuildAndExpandType.WoodWall:
                case BuildAndExpandType.WoodTower:
                case BuildAndExpandType.StoneWall:
                case BuildAndExpandType.StoneTower:
                case BuildAndExpandType.StoneWallGreen:
                case BuildAndExpandType.StoneWallBlueRoof:
                case BuildAndExpandType.StoneWallWoodHouse:

                    DefenceMenu.WallDefenceToHud(content, (TerrainWallType)build.terrainType.subTerrain, true);
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

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Delivery_MaxDistance, DssConst.DeliveryMaxDistance)));

                if (speedBonus > 0)
                {
                    content.newLine();
                    HudLib.BulletPoint(content);
                    content.Add(new RbText(string.Format(DssRef.lang.Delivery_SpeedBonus, speedBonus)));
                }
            }

            void farmHud(bool upgrade, ItemResource produce1, ItemResource produce2)
            { 
                farmHud_any(upgrade, produce1, produce2, TerrainContent.FarmCulture_ReadySize - 1, DssConst.WorkTime_GatherFoil_FarmCulture, DssConst.PlantWaterCost);
            }

            void farmHud_any(bool upgrade, ItemResource produce1, ItemResource produce2, int plantToReadyTime, float gatherTime, int PlantWaterCost)
            {
                float plantTime = upgrade ? DssConst.WorkTime_Plant_Upgraded : DssConst.WorkTime_Plant;

                content.h2(DssRef.lang.BuildHud_PerCycle).overrideColor = HudLib.TitleColor_Label;
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, plantToReadyTime))));

                content.newLine();
                HudLib.BulletPoint(content);
                var workTimeText = new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, plantTime + gatherTime)));
                if (upgrade)
                {
                    workTimeText.overrideColor = HudLib.AvailableColor;

                    content.Add(new RbImage(HudLib.AvailableIcon));
                    content.hspace();
                }
                content.Add(workTimeText);

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.Language_ItemCountPresentation, DssRef.lang.Hud_PurchaseTitle_Cost, PlantWaterCost)));
                content.Add(new RbImage(SpriteName.WarsResource_Water));
                content.Add(new RbText(DssRef.lang.Resource_TypeName_Water));

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                content.space();
                content.Add(new RbText(produce1.amount.ToString()));
                IconName.Item(produce1.type, out SpriteName itemIcon, out string itemName);
                content.Add(new RbImage(itemIcon));
                content.Add(new RbText(itemName));
                if (produce2.amount > 0)
                {
                    content.Add(new RbImage(SpriteName.pjNumPlus));

                    IconName.Item(produce2.type, out itemIcon, out itemName);
                    content.Add(new RbText(DssConst.HempLinenAndFuelAmount.ToString()));
                    content.Add(new RbImage(itemIcon));
                    content.Add(new RbText(itemName));
                }
            }

            void pen(BuildOption build, AnimalPenGrowth penGrowth, ItemResourceType resourceType, bool canBreedup, bool canBreedDown)
            {
                content.h2(string.Format(DssRef.lang.Hud_Upkeep, string.Empty), HudLib.TitleColor_Label);
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbImage(SpriteName.WarsResource_RawFoodRemove));
                content.hspace();
                content.Add(new RbText(DssRef.lang.Resource_TypeName_RawFood, HudLib.TitleColor_TypeName));
                content.hspace();
                content.Add(new RbText(build.upkeep.amount.ToString()));
                content.newLine();
                content.text(DssRef.todoLang.Hud_Time_ValuePerMinute, HudLib.InfoYellow_Light);

                content.newParagraph();
                content.h2(DssRef.lang.BuildHud_PerCycle, HudLib.TitleColor_Label);
                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.BuildHud_GrowTime, string.Format(DssRef.lang.Hud_Time_Minutes, penGrowth.harvestReady - 1))));

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(string.Format(DssRef.lang.BuildHud_WorkTime, string.Format(DssRef.lang.Hud_Time_Seconds, DssConst.WorkTime_PickUpProduce))));

                content.newLine();
                HudLib.BulletPoint(content);
                content.Add(new RbText(DssRef.lang.BuildHud_Produce));
                content.space();
                IconName.Item(resourceType, out SpriteName itemIcon, out string itemName);
                content.Add(new RbImage(itemIcon));
                content.hspace();
                content.Add(new RbText(itemName));

                if (canBreedup || canBreedDown)
                {
                    content.h2(DssRef.todoLang.Pen_Breeding, HudLib.TitleColor_Label);
                    content.newLine();
                    if (canBreedup)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(string.Format(DssRef.todoLang.Pen_BreedUpChance, conv.ToPercentage(DssConst.BreedingUpChance))));
                    }
                    if (canBreedDown)
                    {
                        content.newLine();
                        HudLib.BulletPoint(content);
                        content.Add(new RbText(string.Format(DssRef.todoLang.Pen_BreedDownChance, conv.ToPercentage(DssConst.BreedingDownChance))));
                    }
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
                city.GetGroupedResource(EntityComponent.CityResoureIndex.food)/*res_food*/.toMenu(content, ItemResourceType.Food_G, ref reachedBuffer);
            }

            if (build.blueprint.levelRequirement > XP.ExperienceLevel.Beginner_1)
            {
                content.newLine();

                HudLib.Experience(content, build.blueprint.experienceType, city.cityExperienceLevels.Get(build.blueprint.experienceType).Max());
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

        void mayCraftList(RichBoxContent content, City city, CraftBlueprint[] types)
        {
            content.h2(DssRef.lang.BuildHud_MayCraft).overrideColor = HudLib.TitleColor_Label;

            foreach (var m in types)
            {
                //content.newLine();
                //content.Add(new RbImage(SpriteName.WarsBluePrint));
                //content.space();
                
                m.toMenu(content, city, false, true, false, false);
                //bp1?.resultTypeToMenu(content);
            }
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
                bp1?.resultTypeToMenu(content);
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
            if (player.gameControls.input.inputSource.ControllerMode)
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
            if (player.gameControls.input.inputSource.ControllerMode)
            {
                blockBuildUpdate = true;
            }

            //buildMode = SelectTileResult.Build;
            //placeBuildingType = type;
            //availableBuildingType = true;
            SetBuildMode(type);
            player.gameControls.setMenuFocus(false, true);

            
            //player.gameControls.mapControls.setObjectMenuFocus(false);
        }
        public void SetBuildMode(BuildAndExpandType type) 
        {
            buildMode = SelectTileResult.Build;
            placeBuildingType = type;
            availableBuildingType = true;
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
