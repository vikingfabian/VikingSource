using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameObject.Worker;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.Graphics;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars.Players
{
    class MapControls
    {
        const float CamMaxRotation = 0.5f;
        const float CamStartRotation = MathHelper.PiOver2;
        IntervalF ZoomRange = MapDetailLayerManager.FullZoomRange;

        FloatInBound camRotation = new FloatInBound(CamStartRotation, new IntervalF(CamStartRotation - CamMaxRotation, CamStartRotation + CamMaxRotation), false);

        LocalPlayer player;

        Ray ray;
        Plane groundPlane = new Plane(Vector3.UnitY, 0);

        BoundingBox subTileBoundingBox = new BoundingBox();
        static readonly Vector3 SubTileBoxSz = new Vector3(WorldData.SubTileWidth, WorldData.SubTileWidth * 3f, WorldData.SubTileWidth);

        SafeCollectAsynchList<AbsMapObject> nearMapObjects = new SafeCollectAsynchList<AbsMapObject>(8);
        SafeCollectAsynchList<AbsSoldierUnit> nearDetailUnits = new SafeCollectAsynchList<AbsSoldierUnit>(64);

        public Vector3 playerPointerPos = Vector3.Zero, mousePosition = Vector3.Zero;
        VectorRect selectWpRectangle = VectorRect.Zero;


        public IntVector2 tilePosition, subTilePosition;
        public bool onNewTile = false;

        public Graphics.TopViewCamera camera;
        Graphics.Image controllerPointer;

        public Selection hover;
        public Selection selection;
        bool controllerInput;
        public bool unlockEdgePush = false;

        public AbsGameObject cameraFocus = null;
        Graphics.RectangleLines selectRectangle = null;

        public MapControls(LocalPlayer player)
        {
            this.player = player;

            camera = new TopViewCamera(MapDetailLayerManager.StartZoom, 
                new Vector2(MathHelper.PiOver2, Map.MapDetailLayerManager.NormalCamAngle),
                player.playerData.view.DrawAreaF.Width, player.playerData.view.DrawAreaF.Height);
            camera.FarPlane = 800;
            camera.positionChaseLengthPercentage = 0.9f;
            camera.FieldOfView = 20;
            camera.UseTerrainCollisions = false;
            camera.zoomChaseLengthPercentage = 0.5f;

            hover = new Selection(player, true);
            selection = new Selection(player, false);
            
            player.playerData.view.Camera = camera;

            Graphics.TopViewCamera lightcamera = camera.Clone();
            lightcamera.FieldOfView = 5;
            //lightcamera.TiltX = MathHelper.PiOver2;
            player.playerData.view.LightCamera= lightcamera;

            controllerInput = player.input.inputSource.IsController;

            if (controllerInput)
            {
                controllerPointer = new Image(SpriteName.cmdPointer, player.playerData.view.DrawAreaF.Center, Engine.Screen.SmallIconSizeV2, ImageLayers.Lay1, true);
            }
        }

        public void setZoomRange(bool tutorial)
        {
            ZoomRange = tutorial? MapDetailLayerManager.TutorialZoomRange : MapDetailLayerManager.FullZoomRange;
        }

        public Vector2 XPointerPos()
        {
            return controllerPointer.position;
        }

        public void setCameraPos(IntVector2 tile)
        {
            playerPointerPos = WP.ToWorldPos(tile);
            camera.LookTarget = playerPointerPos;
        }

        public void update(bool mouseOverHud)
        {
            //if (controllerInput)
            //{
            //    mousePosition = screenPosToWorldPos(controllerPointer.position);

            //    IntVector2 prevTile = tilePosition;
            //    tilePosition = WP.ToTilePos(mousePosition);
            //    onNewTile = prevTile != tilePosition;

            //    if (focusedObjectMenuState())
            //    {
            //        player.hud.displays.updateMove(out bool bRefresh);
            //        player.hud.needRefresh |= bRefresh;

            //        if (player.input.ControllerFocus.DownEvent)
            //        {
            //            setObjectMenuFocus(false);
            //        }
            //    }
            //    else
            //    {
            //        if (selection.obj != null)
            //        {
            //            if (player.input.ControllerFocus.DownEvent)
            //            {
            //                setObjectMenuFocus(true);
            //            }
            //        }

            //        panInput();
            //        //Find closest object
            //        hover.begin(true);
            //        {
            //            controllerHoverUpdate();
            //        }
            //        hover.end();
            //    }

            //    if (player.input.ControllerCancel.DownEvent)
            //    {
            //        player.hud.displays.clearMoveSelection();
            //        player.clearSelection();
            //    }

            //    checkSelectionAlive();
            //    selection.end();
            //    rectangleSelectUpdate();
            //    selection.begin(false);

            //    updateSeletionGui();
            //}
            //else
            {
                if (mouseOverHud)
                {
                    hover.clear();
                }
                else
                {
                    if (controllerInput)
                    {
                        mousePosition = screenPosToWorldPos(controllerPointer.position);
                    }
                    else
                    {
                        mousePosition = screenPosToWorldPos(Input.Mouse.Position);
                    }
                    IntVector2 prevTile = tilePosition;
                    tilePosition = WP.ToTilePos(mousePosition);
                    onNewTile = prevTile != tilePosition;

                    if (focusedObjectMenuState() || player.hud.menuFocus)
                    {
                        player.hud.displays.updateMove(out bool bRefresh);
                        player.hud.needRefresh |= bRefresh;

                        if (player.input.ControllerFocus.DownEvent)
                        {
                            if (player.hud.menuFocus)
                            {
                                setHeadMenuFocus(false);
                            }
                            else
                            {
                                setObjectMenuFocus(false);
                            }
                        }
                    }
                    else
                    {
                        if (controllerInput)
                        {
                            panInput();
                        }

                        if (selectRectangle == null)
                        {
                            if (controllerInput)
                            {
                                if (player.input.ControllerFocus.DownEvent)
                                {
                                    if (selection.obj != null)
                                    {
                                        setObjectMenuFocus(true);
                                    }
                                    else if (hover.obj == null)
                                    {
                                        setHeadMenuFocus(true);
                                    }
                                }

                                
                            }
                            //mouseHoverUpdate();
                            hover.begin(true);
                            {
                                if (controllerInput)
                                {
                                    controllerHoverUpdate();
                                }
                                else
                                {
                                    mouseHoverUpdate();
                                }
                            }
                            subTileHoverUpdate();
                            //mouseHoverUpdate();
                        }
                        hover.end();
                    }
                }

                if (controllerInput && player.input.ControllerCancel.DownEvent)
                {
                    player.hud.displays.clearMoveSelection();
                    player.clearSelection();
                }

                checkSelectionAlive();
                selection.end();

                rectangleSelectUpdate();

                selection.begin(false);

                updateSeletionGui();

                if (!controllerInput)
                {
                    panInput();
                }
            }

            zoomInput();
            cameraFocusUpdate();
            updateCamera();
        }


        Vector2 pointerPos()
        {
            if (controllerInput)
            {
                return controllerPointer.position;
            }
            else
            {
                return Input.Mouse.Position;
            }
        }


        void rectangleSelectUpdate()
        {
            if (selectRectangle == null)
            {
                bool select;
                if (controllerInput)
                {
                    select = player.input.Select.DownEvent && hover.obj == null;
                }
                else
                {
                    select = Input.Keyboard.Ctrl && Input.Mouse.ButtonDownEvent(MouseButton.Left);
                }
                if (select)
                {
                    //rectangleStart = mousePosition;
                    //rectangleEnd = rectangleStart;

                    selectWpRectangle.Position = VectorExt.V3XZtoV2(mousePosition);
                    selectWpRectangle.Size =Vector2.Zero;

                    selectRectangle = new RectangleLines(new VectorRect(pointerPos(), Vector2.Zero), 2, 0, HudLib.GUILayer);
                }
            }
            else
            {
                //rectangleEnd = mousePosition;
                selectWpRectangle.SetRightBottom( VectorExt.V3XZtoV2(mousePosition), true);
                selectRectangle.rectangle.Position = player.playerData.view.From3DToScreenPos(VectorExt.V3FromXZ(selectWpRectangle.Position, 0));
                selectRectangle.rectangle.SetRightBottom(pointerPos(), true);
                selectRectangle.rectangle.RemoveNegativeSize();
                selectRectangle.Refresh();

                var wpRectangle_normalized = selectWpRectangle;
                wpRectangle_normalized.RemoveNegativeSize();

                if (player.drawUnitsView.current.type == MapDetailLayerType.TerrainOverview2)
                {
                    var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsMultiselectMapObjects(WP.ToTilePos(wpRectangle_normalized.Position), WP.ToTilePos(wpRectangle_normalized.RightBottom), player.faction);
                    
                    for (int i = nearMapObjects.Count - 1; i >= 0; i--)
                    {
                        if (!wpRectangle_normalized.IntersectPoint(VectorExt.V3XZtoV2(nearMapObjects[i].position)))
                        {
                            nearMapObjects.RemoveAt(i);
                        }
                    }

                    if (hover.obj == null || hover.obj.gameobjectType() != GameObjectType.ObjectCollection)
                    {
                        hover.obj = new MapObjectCollection(player.faction);
                    }

                    if (nearMapObjects.Count > 0)
                    {
                        lib.DoNothing();
                    }

                    hover.obj.GetCollection().set(nearMapObjects);
                }


                bool keyUp;
                if (controllerInput)
                {
                    keyUp = !player.input.Select.IsDown;
                }
                else
                {
                    keyUp = !Input.Mouse.IsButtonDown(MouseButton.Left);
                }

                if (keyUp)
                {
                    selectRectangle.DeleteMe();
                    selectRectangle = null;
                    //select

                    if (hover.obj != null && 
                        hover.obj.gameobjectType() == GameObjectType.ObjectCollection
                        )
                    {
                        var coll = hover.obj.GetCollection();
                        if (coll.objects.Count > 0)
                        {
                            SoundLib.click.Play();

                            if (coll.objects.Count == 1)
                            {
                                selection.obj = coll.objects[0];
                                player.armyControls = new ArmyControls(player, coll.objects);
                            }
                            else
                            {
                                selection.obj = coll;
                                player.armyControls = new ArmyControls(player, coll.objects);
                            }
                        }
                    }
                }
            }
        }

        Vector3 screenPosToWorldPos(Vector2 screenPos)
        {
            
            ray = camera.CastRay(screenPos, player.playerData.view.Viewport);

            //Place cubes and find the exact spot of the subtile
            bool hasValue;
            Vector3 result = this.camera.CastRayInto3DPlane(ray, groundPlane, out hasValue);
            subTilePosition = WP.ToSubTilePos(mousePosition);

            IntVector2 subTilePositionInLoop= IntVector2.Zero;
            //IntVector2 closest = IntVector2.NegativeOne;
            //float closestDist = float.MaxValue;

            //foreach (Graphics.Mesh mesh in debugmeshes)
            //{
            //    mesh.position = Vector3.Zero;
            //}
            //int currentMesh = 0;
            
            SubTile subTile;
            for (int y = 6; y >= -1; --y)
            {
                subTilePositionInLoop.Y = subTilePosition.Y + y;
                for (int x = -1; x <= 1; ++x)
                {
                    subTilePositionInLoop.X = subTilePosition.X + x;

                    Vector3 min = WP.SubtileToWorldPosXZ(subTilePositionInLoop);
                    if (DssRef.world.subTileGrid.TryGet(subTilePositionInLoop, out subTile))
                    {
                        min.Y = subTile.groundY - SubTileBoxSz.Y;

                        subTileBoundingBox.Min = min;
                        subTileBoundingBox.Max = min + SubTileBoxSz;

                        float? distance = ray.Intersects(subTileBoundingBox);
                        if (distance.HasValue) 
                        {
                            subTilePosition = subTilePositionInLoop;

                            goto exitLoop;
                        }
                    }
                }
            }

            exitLoop:

            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }

            //if (closest.X >= 0)
            //{
            //    subTilePosition = closest;
            //}

            return result;
        }

        public void asynchUpdate_depricated()
        {
            if (nearMapObjects.ReadyForAsynchProcessing() &&
                nearDetailUnits.ReadyForAsynchProcessing())
            {
                nearMapObjects.processList.Clear();
                nearDetailUnits.processList.Clear();

                const int MapObjRadius = 5;
                const float DetailUnitRadius = 2f; 

                var factions = DssRef.world.factions.counter();

                while (factions.Next())
                {
                    var armies = factions.sel.armies.counter();
                    while (armies.Next())
                    {
                        if (checkDistance(armies.sel))
                        {
                            var groups = armies.sel.groups.counter();
                            while (groups.Next())
                            {
                                var soldiers = groups.sel.soldiers.counter();
                                while (soldiers.Next())
                                {
                                    if (soldiers.sel.model != null &&
                                        VectorExt.PlaneXZLength(soldiers.sel.position - mousePosition) < DetailUnitRadius)
                                    {
                                        nearDetailUnits.processList.Add(soldiers.sel);
                                    }
                                }
                            }
                        }
                    }
                }

                var cities = DssRef.world.cities;

                foreach (var m in cities)
                {
                    checkDistance(m);
                }

                nearMapObjects.onAsynchProcessComplete();
                nearDetailUnits.onAsynchProcessComplete();

                bool checkDistance(AbsMapObject obj)
                {
                    if (tilePosition.SideLength(obj.tilePos) <= MapObjRadius)
                    {
                        nearMapObjects.processList.Add(obj);
                        return true;
                    }

                    return false;
                }
            }
        }

        bool lookingForAttackTarget()
        {
            return selection.obj != null && selection.obj.gameobjectType() == GameObjectType.Army;
        }

        void subTileHoverUpdate()
        {
            if (player.drawUnitsView.current.type == MapDetailLayerType.UnitDetail1)
            {
                hover.subTile.update(subTilePosition, player);
            }
            else
            {
                hover.subTile.hasSelection = false;
            }
        }

        void mouseHoverUpdate()
        {

            if (player.drawUnitsView.current.type == MapDetailLayerType.TerrainOverview2)
            {
                AbsMapObject intersectObj = null;
                var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects(tilePosition, false);
                foreach (var m in nearMapObjects)
                {
                    if (m.rayCollision(ray))
                    {
                        intersectObj = m;

                        if (
                            (m.faction == player.faction && m.gameobjectType() == GameObjectType.Army) ||
                            lookingForAttackTarget()
                            )
                        {
                            break;
                        }
                    }
                }

                hover.obj = intersectObj;
            }
            else if (player.drawUnitsView.current.type == MapDetailLayerType.UnitDetail1)
            {
                detailHoverUpdate();
            }
        }

        public bool armyMayAttackHoverObj()
        {
            return player.mapControls.hover.obj != null &&
                 player.mapControls.hover.obj.GetFaction() != player.faction;
        }

        void controllerHoverUpdate()
        {
            if (player.drawUnitsView.current.type == MapDetailLayerType.TerrainOverview2)
            {
                const float FriendlyPriorityDistAdd = 0.25f;
                float maxDistance_enemy;
                float maxDistance_friend;
                if (player.armyControls != null)
                {
                    maxDistance_enemy = 1.5f;
                    maxDistance_friend = 0.5f;
                }
                else
                {
                    maxDistance_enemy = 1f;
                    maxDistance_friend = 1.5f;
                }

                var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects(tilePosition, true);
                AbsMapObject closestObj= null;
                float closest = float.MaxValue;
                foreach (var m in nearMapObjects)
                {
                    var dist= VectorExt.PlaneXZLength(m.position - mousePosition);
                    bool enemy = m.faction != player.faction;
                    float maxDistance = enemy ? maxDistance_enemy : maxDistance_friend;

                    if (dist <= maxDistance)
                    {
                        if (dist < closest || 
                            (
                                closestObj.faction != player.faction && 
                                dist < closest + FriendlyPriorityDistAdd && 
                                !lookingForAttackTarget()
                            )
                            )
                        {
                            closest = dist;
                            closestObj = m;
                        }
                    }
                }

                if (closestObj != null)
                {
                    hover.obj = closestObj;
                }
            }
            else if (player.drawUnitsView.current.type == MapDetailLayerType.UnitDetail1)
            {
                //var nearDetailUnits = DssRef.world.unitCollAreaGrid.MapControlsNearDetailUnits(tilePosition);

                //foreach (var m in nearDetailUnits)
                //{

                //}
                detailHoverUpdate();
            }
        }

        void detailHoverUpdate()
        {
            var nearDetailUnits = DssRef.world.unitCollAreaGrid.MapControlsNearDetailUnits(tilePosition);

            BoundingSphere bound = new BoundingSphere(Vector3.Zero, 0f);

            foreach (var m in nearDetailUnits)
            {
                bound.Center = m.position;
                bound.Radius = m.radius * 2f;
                float? distance = ray.Intersects(bound);
                if (distance.HasValue)
                { //intersects
                    hover.obj = m;
                    break;
                }
            }

            bound.Radius = DssVar.Worker_StandardBoundRadius;
            var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects_Workers(tilePosition, false);//DssRef.world.unitCollAreaGrid.MapControlsWorkerCities(tilePosition);
            foreach (var m in nearMapObjects)
            {
                switch (m.gameobjectType())
                {
                    case GameObjectType.City:
                        var city = m.GetCity();
                        if (city != null && city.workerUnits != null)
                        {
                            foreach (var worker in city.workerUnits)
                            {
                                bound.Center = worker.WorldPos();
                                float? distance = ray.Intersects(bound);
                                if (distance.HasValue)
                                { //intersects
                                    hover.obj = worker;
                                    break;
                                }
                            }
                        }
                        break;
                    case GameObjectType.Army:
                        var army = m.GetArmy();
                        if (army.workerUnits != null)
                        {
                            foreach (var worker in army.workerUnits)
                            {
                                bound.Center = worker.WorldPos();
                                float? distance = ray.Intersects(bound);
                                if (distance.HasValue)
                                { //intersects
                                    hover.obj = worker;
                                    break;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public void onSelect()
        {
            selection.obj = hover.obj;
            
        }

        public void onTileSelect(SelectedSubTile selectedSubTile, bool sameMapObject)//City city, SelectTileResult tileResult)
        {
            //if (selection.obj != null && selection.obj.gameobjectType() == GameObjectType.City)
            //{
            //    if (player.cityTab == Display.MenuTab.Build)
            //    { 
            //        player.BuildControls.onTileSelect(selectedSubTile);
            //    }
            //    //    && 
            //    //    player.BuildControls.buildMode == SelectTileResult.Build)
            //    //{ 
            //    //    var mayBuild = selectedSubTile.MayBuild(player);
            //    //    if (mayBuild == MayBuildResult.Yes || mayBuild == MayBuildResult.Yes_ChangeCity)
            //    //    { 
            //    //        //create build order
            //    //        player.addOrder(new BuildOrder(10, selectedSubTile.city, selectedSubTile.subTilePos, player.BuildControls.placeBuildingType)
            //    //    }
            //    //}
            //}
            if (selection.obj != selectedSubTile.city)
            {
                selection.obj = selectedSubTile.city;
                if (!sameMapObject)
                {
                    SoundLib.select_city.Play();
                }
            }

            switch (selectedSubTile.selectTileResult)
            {
                case SelectTileResult.Conscript:
                    {
                        player.cityTab = Display.MenuTab.Conscript;
                        selectedSubTile.city.selectedConscript = selectedSubTile.city.conscriptIxFromSubTile(selectedSubTile.subTilePos);



                        //int id = conv.IntVector2ToInt(selectedSubTile.subTilePos);
                        //for (int i = 0; i < selectedSubTile.city.conscriptBuildings.Count; ++i)
                        //{
                        //    if (selectedSubTile.city.conscriptBuildings[i].idAndPosition == id)
                        //    {
                        //        selectedSubTile.city.selectedConscript = i; break;
                        //    }
                        //}
                    }
                    break;
                case SelectTileResult.Recruitment:
                case SelectTileResult.Postal:
                    {
                        player.cityTab = Display.MenuTab.Delivery;
                        selectedSubTile.city.selectedDelivery = selectedSubTile.city.deliveryIxFromSubTile(selectedSubTile.subTilePos);

                        //int id = conv.IntVector2ToInt(selectedSubTile.subTilePos);
                        //for (int i = 0; i < selectedSubTile.city.deliveryServices.Count; ++i)
                        //{
                        //    if (selectedSubTile.city.deliveryServices[i].idAndPosition == id)
                        //    {
                        //        selectedSubTile.city.selectedDelivery = i; break;
                        //    }
                        //}

                        //setObjectMenuFocus(true);
                    }
                    break;
                //case SelectTileResult.:
                //    player.cityTab = Display.MenuTab.Delivery;
                //    break;
            }
            
            //switch (tileResult)
            //{
            //    case SelectTileResult.CityHall:
            //        player.cityTab = Display.MenuTab.Recruit;
            //        break;
            //    case SelectTileResult.Resources:
            //        player.cityTab = Display.MenuTab.Resources;
            //        break;
            //}
        }

        public bool focusedObjectMenuState()
        {
            return selection.obj != null &&
                controllerInput &&
                selection.menuFocus;
        }

        public void setHeadMenuFocus(bool set)
        {
            player.hud.setHeadMenuFocus(set);
            //player.hud.displays.headDisplay.viewOutLine(set);

            //if (set)
            //{
            //    //playerPointerPos = selection.obj.WorldPos();
            //    player.hud.displays.beginMove(1);
            //}
            //else
            //{
            //    player.hud.displays.clearMoveSelection();
            //}


            //controllerPointer.Visible = !set;

            //player.hud.needRefresh = true;

        }
        
        public void setObjectMenuFocus(bool set)
        {
            //if (!set )//&& selection.obj.gameobjectType() == GameObjectType.City)
            //{
            //    return;
            //}
            if (controllerInput)
            {

                if (set && !selection.obj.CanMenuFocus())
                {
                    return;
                }

                selection.menuFocus = set;
                player.hud.displays.objectDisplay.viewOutLine(set);

                if (set)
                {
                    //playerPointerPos = selection.obj.WorldPos();
                    player.hud.displays.beginMove(1);
                }
                else
                {
                    player.hud.displays.clearMoveSelection();
                }


                controllerPointer.Visible = !set;

                player.hud.needRefresh = true;
            }
        }

        public bool clearSelection()
        {
            bool bClear = selection.clear();
            
            player.hud.displays.clearState();
            if (controllerInput)
            {
                controllerPointer.Visible = true;
            }
            return bClear;
        }

        void checkSelectionAlive()
        {
            if (selection.obj != null && selection.obj.aliveAndBelongTo(player.faction) == false)
            { 
                player.clearSelection();
            }
        }

        void updateSeletionGui()
        {
            bool viewTile = hover.subTile.viewSelection(true);//hover.obj == null);

            if (!viewTile && hover.obj != null && hover.obj != selection.obj)
            {
                //hover.frameModel.Visible = true;
                hover.obj.selectionFrame(true, hover);

                //hover.frameModel.Color = hover.obj.GetFaction() == player.faction? Color.White : Color.LightGray;

                updateSelectionGui(hover);
            }
            else
            {
                hover.ClearSelectionModels();                
            }

            if (selection.obj != null)
            {
                //selection.frameModel.Visible = true;
                selection.obj.selectionFrame(false, selection);

                updateSelectionGui(selection);
            }
            else
            {
                //selection.guiModels.DeleteAll();
                selection.ClearSelectionModels();//.frameModel.Visible = false;
            }
        }

        void updateSelectionGui(Selection selection)
        {
            if (selection.isNew || DssRef.time.oneSecond)
            {
                //Shows path dots
                selection.guiModels.DeleteAll();
                selection.obj.selectionGui(player, selection.guiModels);
            }
        }

        private void zoomInput()
        {
            //if (StartupSettings.Trailer)
            //{
            //    if (Input.Keyboard.Ctrl)
            //    {
            //        camera.targetZoom += 0.005f*camera.targetZoom;
            //    }
            //}

            var newZoom = VikingEngine.Bound.Set(
                camera.CurrentZoom + player.input.ZoomValue * 0.005f * camera.CurrentZoom, ZoomRange);//10 12
            if (newZoom != camera.CurrentZoom)
            {
                camera.CurrentZoom = newZoom;
                if (!controllerInput)
                {
                    camera.positionFromRotation();
                    camera.RecalculateMatrices();
                    var mousePosition2 = screenPosToWorldPos(Input.Mouse.Position);
                    Vector3 diff = mousePosition2 - mousePosition;
                    panCamera(diff);
                }
            }

            const float XBuffer = 0.6f;
            const float RotationSpeed = 0.004f;
            if (Math.Abs(player.input.cameraTiltZoom.direction.X) > XBuffer)
            {
                camRotation.Value += RotationSpeed * player.input.cameraTiltZoom.directionAndTime.X;
            }
            else
            {
                //Rotate back
                float diff = CamStartRotation - camRotation.Value;
                if (Math.Abs(diff) > 0.01f)
                {
                    float dir = lib.ToLeftRight(diff);
                    float rotAdd = VikingEngine.Bound.MaxAbs(RotationSpeed * dir * Ref.DeltaTimeMs, diff);
                    camRotation.Value += rotAdd;
                }
            }

            camera.TiltX = camRotation.Value;
        }
        private void panInput()
        {
            if (player.diplomacyMap!= null && player.diplomacyMap.hasSelection())
            {
                return;
            }

            float panSpeed;

            if (controllerInput)
            {
                panSpeed = 0.0003f * camera.targetZoom;
            }
            else
            {
                panSpeed = 0.0006f * camera.targetZoom;
            }
            //if (StartupSettings.Trailer)
            //{
            //    panSpeed *= 0.2f;
            //}

            //playerPointerPos += VectorExt.V2toV3XZ(player.input.move.directionAndTime * panSpeed);
            panCamera(VectorExt.V2toV3XZ(-player.input.move.directionAndTime * panSpeed));
            if (!player.hud.hudMouseOver() && !controllerInput)
            {
                if (hasMouseMapPanInput())
                {
                    //bool hasValue;
                    Vector3 prevMousePosition = screenPosToWorldPos(Input.Mouse.Position - Input.Mouse.MoveDistance);
                   
                    Vector3 diff = mousePosition - prevMousePosition;

                    panCamera(diff);

                    return;
                }



                if (DssRef.state.localPlayers.Count == 1)
                {
                    if (!player.input.DragPan.IsDown &&
                        !player.input.Select.IsDown &&
                        Input.Mouse.HasEdgePush())
                    {
                        panCamera(VectorExt.V2toV3XZ(-Input.Mouse.EdgePush() * Ref.DeltaTimeMs * panSpeed));

                    }
                }
            }


            DssRef.world.WorldBound(ref playerPointerPos.X, ref playerPointerPos.Z);
            playerPointerPos.Y = DssRef.world.GetTile(playerPointerPos).GroundY() + 0.5f;
        }

        void cameraFocusUpdate()
        {
            if (cameraFocus != null)
            {   
                Vector3 goal = cameraFocus.WorldPos();
                goal.Y = 0;
                goal.Z += 0.5f;
                Vector3 diff = goal - camera.LookTarget;
                if (VectorExt.HasValue(diff))
                {
                    float panSpeed = 0.003f * Ref.DeltaTimeMs * camera.targetZoom;
                    
                    if (panSpeed >= diff.Length())
                    {
                        camera.LookTarget = goal;
                    }
                    else
                    {
                        diff.Normalize();
                        Vector3 move = diff * panSpeed;
                        camera.LookTarget += move;
                    }
                    
                    playerPointerPos = camera.LookTarget;
                } 
            }
        }

        public void focusMap(bool focus)
        {
            controllerPointer.Visible = focus;
        }

        void panCamera(Vector3 pan)
        {
            pan.Y = 0;
            if (VectorExt.HasValue(pan))
            {
                cameraFocus = null;

                camera.LookTarget -= pan;
                camera.setLookTargetXBound(DssRef.world.unitBounds.Position.X, DssRef.world.unitBounds.Right);
                camera.setLookTargetZBound(DssRef.world.unitBounds.Position.Y, DssRef.world.unitBounds.Bottom);

                playerPointerPos = camera.LookTarget;
            }
        }

        bool hasMouseMapPanInput()
        {
            return player.input.inputSource.HasMouse &&
                selectRectangle == null &&
                ((player.input.Select.IsDown && NoSelection()) || player.input.DragPan.IsDown) &&
                Input.Mouse.bMoveInput;
        }

        private void updateCamera()
        {
            Vector3 camTarget = playerPointerPos;
            camTarget.Y = 0.6f;

            if ((camTarget - camera.LookTarget).Length() < 0.5f)
            {
                camera.LookTarget = camTarget;
            }
            else
            {
                camera.GoalLookTarget = camTarget;
            }

            camera.Time_Update(Ref.DeltaTimeMs);
        }

        public GameObjectType SelectionType
        {
            get
            {
                if (selection.obj != null)
                {
                    return selection.obj.gameobjectType();
                }
                else
                {
                    return GameObjectType.NUM_NON;
                }
            }
        }

        public AbsGameObject FocusObject()
        {
            if (selection.obj != null)
            {
                return selection.obj;
            }
            else
            {
                return hover.obj;
            }
        }

        public bool NoSelection() { return selection.obj == null; }
    }

   
}
