using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection.Metadata.Ecma335;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.DSSWars.Players.Orders;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.EngineSpace.Graphics.In3D;
using VikingEngine.Graphics;
using VikingEngine.Physics;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars.Players
{
    class MapControls
    {
        int currentTiltYAngleOption = 0;
        const float TiltYUpAngle = -0.2f;

        const float CamMaxRotation = 0.7f;
        const float CamStartRotation = MathHelper.PiOver2;
        IntervalF ZoomRange = MapLayerManager.FullZoomRange;
        VectorRect panBounds;
        FloatInBound camRotation = new FloatInBound(CamStartRotation, new IntervalF(CamStartRotation - CamMaxRotation, CamStartRotation + CamMaxRotation), false);
        float camRotationKeyDownTime = 0;
        float lastRotationDir;

        LocalPlayer player;

        Ray ray;
        Plane groundPlane = new Plane(Vector3.UnitY, 0);

        BoundingBox subTileBoundingBox = new BoundingBox();
        static readonly Vector3 SubTileBoxSz = new Vector3(WorldData.SubTileWidth, WorldData.SubTileWidth * 3f, WorldData.SubTileWidth);

        SafeCollectAsynchList<AbsMapObject> nearMapObjects = new SafeCollectAsynchList<AbsMapObject>(8);
        SafeCollectAsynchList<AbsSoldierUnit> nearDetailUnits = new SafeCollectAsynchList<AbsSoldierUnit>(64);

        public Vector3 playerPointerPos = Vector3.Zero, pointerPosWP = Vector3.Zero;
        ScreenToSpaceRectangleBound rectangleBound;
        Graphics.RectangleLines rectangleLines = null;
        float multiSelectMoveLenght = 0;
        float multiSelectHoldTime = 0;


        public IntVector2 tilePosition, subTilePosition;
        public bool onNewTile = false;

        public Graphics.TopViewCamera camera;
        public Graphics.Image controllerPointer;

        public Selection hover;
        public Selection selection;
        bool controllerInput;
        public bool unlockEdgePush = false;

        public AbsGameObject cameraFocus = null;
        
        float targetZoom;
        bool panDownInput = false;
        TerrainTypeSearch TerrainTypeSearch = new TerrainTypeSearch();

        public MapControls(LocalPlayer player)
        {
            this.player = player;

            targetZoom = MapLayerManager.StartZoom;
            camera = new TopViewCamera(MapLayerManager.StartZoom, 
                new Vector2(MathHelper.PiOver2, Map.MapLayerManager.NormalCamAngle),
                player.playerData.view.DrawAreaF.Width, player.playerData.view.DrawAreaF.Height);
            camera.FarPlane = 800;
            camera.positionChaseLengthPercentage = 0.9f;
            camera.FieldOfView = 20;
            camera.UseTerrainCollisions = false;
            camera.zoomChaseLengthPercentage = 0.5f;
            panBounds = DssRef.world.unitBounds;
            hover = new Selection(player, true);
            selection = new Selection(player, false);
            
            player.playerData.view.Camera = camera;

            Graphics.TopViewCamera lightcamera = camera.Clone();
            lightcamera.FieldOfView = 5;
            //lightcamera.TiltX = MathHelper.PiOver2;
            player.playerData.view.LightCamera= lightcamera;

            controllerInput = player.gameControls.input.inputSource.IsController;

            rectangleBound = new ScreenToSpaceRectangleBound(player.playerData.view, Map.Settings.Height.DeepWaterHeight-1, Map.Settings.Height.MaxHeight +1);

            if (controllerInput)
            {
                controllerPointer = new Image(SpriteName.cmdPointer, player.playerData.view.DrawAreaF.PercentToPosition(0.6f, 0.5f), Engine.Screen.SmallIconSizeV2, ImageLayers.Lay1, true);

            }
        }

        public void terrainSearchClick(SubTile terrain)
        {
           Vector3 pos = TerrainTypeSearch.FindNext(selection.obj.GetCity(), terrain);
            cameraFocus = new EmptyPoint(pos);
        }

        public void battleModeCamBound()
        {
            ZoomRange = MapLayerManager.MidToDetailZoomRange;
        }

        public void setCameraBounds(bool tutorial, Rectangle2 cityArea)
        {
            ZoomRange = tutorial? MapLayerManager.MidToDetailZoomRange : MapLayerManager.FullZoomRange;

            if (tutorial)
            {
                cityArea.AddRadius(5);
                panBounds = new VectorRect(cityArea);
            }
            else
            {
                panBounds = DssRef.world.unitBounds;
            }
        }

        public Vector2 XPointerPos()
        {
            return controllerPointer.position;
        }

        

        public bool overridingDrag()
        { 
            return rectangleLines != null || panDownInput;
        }

        public void focusedUpdate()
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
            //{
            //    if (mouseOverHud)
            //    {
            //        hover.clear();
            //    }
            //    else
            //    {
            //if (controllerInput)
            //{
            //    mousePosition = screenPosToWorldPos(controllerPointer.position);
            //}
            //else
            //{
            updatePointer();

            //if (focusedObjectMenuState() || player.hud.menuFocus)
            //{
            //    //player.hud.displays.updateMove(out bool bRefresh);
            //    //player.hud.needRefresh |= bRefresh;

            //    if (player.gameControls.input.ControllerFocus.DownEvent)
            //    {
            //        if (player.hud.menuFocus)
            //        {
            //            setHeadMenuFocus(false);
            //        }
            //        else
            //        {
            //            setObjectMenuFocus(false);
            //        }
            //    }
            //}
            //else
            //{
            //    if (controllerInput)
            //    {
            //        keypPanInput();
            //    }

            if (rectangleLines == null)
            {
                //if (controllerInput)
                //{
                //    if (player.gameControls.input.ControllerFocus.DownEvent)
                //    {
                //        if (selection.obj != null)
                //        {
                //            setObjectMenuFocus(true);
                //        }
                //        else if (hover.obj == null)
                //        {
                //            setHeadMenuFocus(true);
                //        }
                //    }


                //}
                //mouseHoverUpdate();
                hover.begin(true);
                {
                    //if (controllerInput)
                    //{
                    //    controllerHoverUpdate();
                    //}
                    //else
                    //{
                    mouseHoverUpdate();
                    //}
                }
                subTileHoverUpdate();
                //mouseHoverUpdate();
            }
            hover.end();
                        
            //}
            //}

            //if (controllerInput && player.gameControls.input.ControllerCancel.DownEvent)
            //{
            //    //player.hud.displays.clearMoveSelection();
            //    player.gameControls.clearSelection();
            //}


            checkSelectionAlive();

            if (player.gameControls.InBuildOrdersMode())
            {
                cancelRectangleSelect();
                updateCitySelectionFromTile();
            }
            else
            {
                selection.end();
                rectangleSelectUpdate();
                selection.begin(false);
            }

            updateSeletionGui(true);

            mousePanInput();
            zoomInput();
            rotateCameraInput();

            if (player.gameControls.input.PinAndPing.DownEvent)
            {
                player.createPin();
            }

            

        }

        void updateCitySelectionFromTile()
        {
            if (onNewTile)
            {
                var newCity = DssRef.world.tileGrid.Get(tilePosition).City();
                if (newCity != selection.obj && newCity.factionIndex == player.faction.myIndex)
                {
                    selection.obj = newCity;
                    player.hud.needRefresh = true;
                    //SoundLib.select_city.Play();
                }
            }
        }

        private void updatePointer()
        {
            pointerPosWP = screenPosToWorldPos(pointerPos());
            IntVector2 prevTile = tilePosition;
            tilePosition = DssRef.world.tileBounds.KeepTilePointInArea(WP.ToTilePos(pointerPosWP));
            onNewTile = prevTile != tilePosition;
            if (onNewTile)
            {
                player.hud.needRefresh = true;
            }
        }

        public void mapControlsUpdate()
        {
            updatePointer();

            mousePanInput();
            zoomInput();
            rotateCameraInput();

            
        }

        public void leftFocusUpdate()
        {
            hover.clear();
            updateSeletionGui(false);
        }

        public void passiveUpdate()
        {
            if (player.gameControls.map.selection.obj != null &&
                player.gameControls.map.selection.obj.isDeleted)
            {
                player.gameControls.map.clearSelection();
            }
            keypPanInput();
            cameraFocusUpdate();
            updateCamera();
            if (!player.gameControls.input.mousePan.IsDown)
            {
                panDownInput = false;
            }
        }


        public Vector2 pointerPos()
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

        public bool HasRectangleSelect()
        { 
            return rectangleLines != null;
        }

        public bool RectangleSelect_ToolipAboveMouse()
        { 
            return rectangleBound.currentPointerPos.Y + 10 < rectangleBound.pointerDownPos.Y;
        }

        public void cancelRectangleSelect()
        { 
            if (rectangleLines != null)
            {
                rectangleLines.DeleteMe();
                rectangleLines = null;
            }
        }

        void rectangleSelectUpdate()
        {
            if (player.mapLayersManager.current.DrawFar)
            {
                cancelRectangleSelect();
                return;
            }


            if (rectangleLines == null)
            {   
                //bool select;
                //if (controllerInput)
                //{
                //    select = player.gameControls.input.ControllerSelect.DownEvent;//&& hover.obj == null;
                //}
                //else
                //{
                    //select = player.gameControls.input.mouseSelect.DownEvent;
                
                if (player.gameControls.input.mouseSelect.DownEvent)
                {
                    multiSelectMoveLenght = 0;
                    multiSelectHoldTime = 0;
                    rectangleBound.begin(pointerPos(), pointerPosWP);
                    rectangleLines = new RectangleLines(rectangleBound.vectorRect, 2, 0, HudLib.GUILayer);
                }
            }
            else
            {
                if (controllerInput)
                {
                    multiSelectMoveLenght += movePanLength.Length();
                }
                else
                {
                    multiSelectMoveLenght += Input.Mouse.MoveDistance.Length();
                }
                multiSelectHoldTime += Ref.DeltaTimeMs;

                //Must start dragging to start multiselect
                if (multiSelectMoveLenght > 10 || multiSelectHoldTime >= Input.InputLib.ButtonMaxClickTimeMs)
                {
                    rectangleBound.update(pointerPos());

                    rectangleLines.Refresh(rectangleBound.vectorRect);
                    rectangleBound.outerBound(out Vector3 topLeft, out Vector3 bottomRight);

                    switch (player.mapLayersManager.current.type)
                    {
                        case MapDetailLayerType.TerrainOverview2:
                            {

                                if (rectangleBound.vectorRect.SideLength() > 1f)
                                {
                                    var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsMultiselectMapObjects(WP.ToTilePos(topLeft), WP.ToTilePos(bottomRight), player.faction.myIndex);

                                    if (Input.Keyboard.Ctrl)
                                    {
                                        lib.DoNothing();
                                    }

                                    if (hover.obj == null || hover.obj.gameobjectType() != GameObjectType.ObjectCollection)
                                    {
                                        hover.obj = new ArmyCollection(player.faction);
                                    }

                                    for (int i = nearMapObjects.Count - 1; i >= 0; i--)
                                    {
                                        if (!nearMapObjects[i].rectangleCollision(rectangleBound))
                                        {
                                            nearMapObjects.RemoveAt(i);
                                        }
                                    }

                                    hover.obj.GetMapCollection().set(nearMapObjects);
                                }
                                else
                                {
                                    hover.obj = null;
                                }
                                
                            }
                            break;

                        case MapDetailLayerType.UnitDetail1:
                            {
                                var nearDetailUnits = DssRef.world.unitCollAreaGrid.MapControlsNearGroups_Rectangle(
                                    WP.ToTilePos(topLeft), WP.ToTilePos(bottomRight), player.faction, rectangleBound);

                                if (hover.obj == null || hover.obj.gameobjectType() != GameObjectType.DetailCollection)
                                {
                                    hover.obj = new DetailObjectCollection(player.faction);
                                }

                                hover.obj.GetDetailCollection().set(nearDetailUnits);
                            }
                            break;
                    }
                }

                bool keyUp;
               
                keyUp = !player.gameControls.input.mouseSelect.IsDown;
                

                if (keyUp)
                {
                    rectangleLines.DeleteMe();
                    rectangleLines = null;
                    //select

                    if (hover.obj != null && hover.obj.IsCollection() && hover.obj.CollectionCount() > 0)
                    {
                        switch (hover.obj.gameobjectType())
                        {
                            case GameObjectType.ObjectCollection:
                                {
                                    var coll = hover.obj.GetMapCollection();

                                    SoundLib.click.Play();

                                    if (coll.objects.Count == 1)
                                    {
                                        selection.obj = coll.objects[0].army;
                                    }
                                    else
                                    {
                                        selection.obj = coll;                                        
                                    }
                                    player.gameControls.army = new ArmyControls(player, coll);
                                }
                                break;


                            case GameObjectType.DetailCollection:
                                {
                                    var coll = hover.obj.GetDetailCollection();

                                    SoundLib.click.Play();

                                    if (coll.armyGroups.Count > 0 && coll.guardGroups.Count > 0)
                                    {
                                        new PopMenu( player, coll);
                                    }
                                    else
                                    {
                                        if (coll.CollectionCount() == 1)
                                        {
                                            selection.obj =coll.first();
                                        }
                                        else
                                        {
                                            selection.obj = coll; //TODO if (coll.objects.Count == 1)
                                        }
                                        player.gameControls.soldier = new SoldierControls(coll.armyGroups.Count > 0 ? coll.armyGroups : coll.guardGroups);
                                    }
                                    //}
                                }
                                break;
                        }
                   
                    }
                }
            }
        }

        public void selectCollection(List<SoldierGroup> coll)
        {
            var collObj = new DetailObjectCollection(player.faction); //TODO if (coll.objects.Count == 1)
            collObj.set(coll);
            selection.obj = collObj;
            player.gameControls.soldier = new SoldierControls(coll);
        }

         public  Vector3 screenPosToWorldPos(Vector2 screenPos)
        {
            
            ray = camera.CastRay(screenPos, player.playerData.view.Viewport);

            //Place cubes and find the exact spot of the subtile
            bool hasValue;
            Vector3 result = this.camera.CastRayInto3DPlane(ray, groundPlane, out hasValue);
            subTilePosition = WP.ToSubTilePos(pointerPosWP);

            IntVector2 subTilePositionInLoop= IntVector2.Zero;
            

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
                                        VectorExt.PlaneXZLength(soldiers.sel.position - pointerPosWP) < DetailUnitRadius)
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
            if (player.mapLayersManager.current.type == MapDetailLayerType.UnitDetail1)
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

            if (player.mapLayersManager.current.type == MapDetailLayerType.TerrainOverview2)
            {
                AbsMapObject intersectObj = null;

                intersectObj = player.rayCollisionWithPin(ray);

                if (intersectObj != null)
                {
                    hover.obj = intersectObj;
                    return;
                }

                var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects(tilePosition, false);
                foreach (var m in nearMapObjects)
                {
                    if (m.rayCollision(ray))
                    {
                        intersectObj = m;

                        if (
                            (m.factionIndex == player.faction.myIndex && m.gameobjectType() == GameObjectType.Army) ||
                            lookingForAttackTarget()
                            )
                        {
                            break;
                        }
                    }
                }

                if (intersectObj != null)
                {
                    hover.obj = intersectObj;
                    return;
                }

                if (controllerInput)
                {
                    controllerNearHoverUpdate(nearMapObjects, ref intersectObj);
                }

                hover.obj = intersectObj;
            }
            else if (player.mapLayersManager.current.type == MapDetailLayerType.UnitDetail1)
            {
                detailHoverUpdate();
            }
        }

        void controllerNearHoverUpdate(List<AbsMapObject> nearMapObjects, ref AbsMapObject intersectObj)
        {
            const float FriendlyPriorityDistAdd = 0.25f;
            float maxDistance_enemy;
            float maxDistance_friend;
            if (player.gameControls.army != null)
            {
                maxDistance_enemy = 1.5f;
                maxDistance_friend = 0.5f;
            }
            else
            {
                maxDistance_enemy = 1f;
                maxDistance_friend = 1.5f;
            }

            //var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects(tilePosition, true);
            AbsMapObject closestObj = null;
            float closest = float.MaxValue;
            foreach (AbsMapObject m in nearMapObjects)
            {
                var dist = VectorExt.PlaneXZLength(m.position - pointerPosWP);
                bool enemy = m.factionIndex != player.faction.myIndex;
                float maxDistance = enemy ? maxDistance_enemy : maxDistance_friend;

                if (dist <= maxDistance)
                {
                    if (dist < closest ||
                        (
                            closestObj.factionIndex != player.faction.myIndex &&
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
                intersectObj = closestObj;
            }
        }

        
        void controllerHoverUpdate()
        {
            if (player.mapLayersManager.current.type == MapDetailLayerType.TerrainOverview2)
            {
                const float FriendlyPriorityDistAdd = 0.25f;
                float maxDistance_enemy;
                float maxDistance_friend;
                if (player.gameControls.army != null)
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
                    var dist= VectorExt.PlaneXZLength(m.position - pointerPosWP);
                    bool enemy = m.factionIndex != player.faction.myIndex;
                    float maxDistance = enemy ? maxDistance_enemy : maxDistance_friend;

                    if (dist <= maxDistance)
                    {
                        if (dist < closest || 
                            (
                                closestObj.factionIndex != player.faction.myIndex && 
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
            else if (player.mapLayersManager.current.type == MapDetailLayerType.UnitDetail1)
            {
                //var nearDetailUnits = DssRef.world.unitCollAreaGrid.MapControlsNearDetailUnits(tilePosition);

                //foreach (var m in nearDetailUnits)
                //{

                //}
                detailHoverUpdate();
            }
        }
        public bool armyMayAttackHoverObj()
        {
            return hover.obj != null &&
                 hover.obj.GetFaction() != player.faction;
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
                        var army = m.GetAbsArmy();
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
                        player.cityTab = Interface.MenuTab.Conscript;
                        selectedSubTile.city.selectedConscript = selectedSubTile.city.conscriptIxFromSubTile(selectedSubTile.subTilePos);

                    }
                    break;
                case SelectTileResult.Wall:
                    {
                        player.cityTab = Interface.MenuTab.Defence;
                        selectedSubTile.city.selectedDefenceBuilding = selectedSubTile.city.defenceIxFromSubTile(selectedSubTile.subTilePos);

                    }
                    break;
                case SelectTileResult.Recruitment:
                case SelectTileResult.Postal:
                case SelectTileResult.GoldDeliver:
                    {
                        player.cityTab = Interface.MenuTab.Delivery;
                        selectedSubTile.city.selectedDelivery = selectedSubTile.city.deliveryIxFromSubTile(selectedSubTile.subTilePos);
                    }
                    break;

                case SelectTileResult.School:
                    {
                        player.cityTab = Interface.MenuTab.Progress;
                        player.progressSubTab = Interface.ProgressSubTab.Schools;
                        selectedSubTile.city.selectedSchool = selectedSubTile.city.SchoolIxFromSubTile(selectedSubTile.subTilePos);
                    }
                    break;

                case SelectTileResult.ResearchCenter:
                case SelectTileResult.BookPress:
                    {
                        player.cityTab = Interface.MenuTab.Progress;
                        player.progressSubTab = Interface.ProgressSubTab.Research;
                        selectedSubTile.city.selectedResearchBuilding = selectedSubTile.city.ResearchIxFromSubTile(selectedSubTile.subTilePos);
                    }
                    break;
            }
        }

        public bool focusedObjectMenuState()
        {
            return selection.obj != null &&
                controllerInput &&
                selection.obj.gameobjectType() == GameObjectType.City;
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
        
        //public void setObjectMenuFocus(bool set)
        //{
        //    //if (!set )//&& selection.obj.gameobjectType() == GameObjectType.City)
        //    //{
        //    //    return;
        //    //}
        //    if (controllerInput)
        //    {

        //        if (set && !selection.obj.CanMenuFocus())
        //        {
        //            return;
        //        }

        //        selection.menuFocus = set;
        //        //player.hud.displays.objectDisplay.viewOutLine(set);

        //        if (set)
        //        {
                    
        //            //player.hud.displays.beginMove(1);
        //        }
        //        else
        //        {
        //            //player.hud.displays.clearMoveSelection();
        //        }


        //        controllerPointer.Visible = !set;

        //        player.hud.needRefresh = true;
        //    }
        //}

        public bool clearSelection()
        {
            bool bClear = selection.clear();
            
            player.hud.objMenu.menu?.clearState();
            if (controllerInput)
            {
                controllerPointer.Visible = true;
            }
            return bClear;
        }

        void checkSelectionAlive()
        {
            if (selection.obj != null && selection.obj.aliveAndBelongTo(player.faction) == false && !DssRef.difficulty.GodPowers())
            { 
                player.gameControls.clearSelection();
            }
        }

        void updateSeletionGui(bool focus)
        {
            if (focus)
            {
                bool viewTile = hover.subTile.viewSelection(true);

                if (!viewTile && hover.obj != null && hover.obj != selection.obj)
                {
                    hover.obj.selectionFrame(player, true, hover);

                    updateSelectionGui(hover);
                }
                else
                {
                    hover.ClearSelectionModels();
                }
            }

            if (selection.obj != null)
            {                
                selection.obj.selectionFrame(player, false, selection);

                updateSelectionGui(selection);
            }
            else
            {
                
                selection.ClearSelectionModels();
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
            if (player.gameControls.input.inputSource.IsController &&
                player.gameControls.input.inputSource.Controller.IsButtonDown(Buttons.LeftTrigger))
            { return; }

            float zoominput = player.gameControls.input.ZoomValue();

            targetZoom = VikingEngine.Bound.Set(
                targetZoom + zoominput * 0.005f * Bound.Min(targetZoom, 0.5f), ZoomRange);

            if (targetZoom != camera.CurrentZoom)
            {
                player.hud.miniMap?.OnMapZoom(zoominput, player);

                float zdiff = targetZoom - camera.CurrentZoom;
                if (Math.Abs(zdiff) > 2)
                {
                    camera.CurrentZoom += zdiff * 0.4f / Ref.UpdateTimes60FPS;
                }
                else
                {
                    camera.CurrentZoom = targetZoom;
                }
                if (!controllerInput)
                {
                    camera.positionFromRotation();
                    camera.RecalculateMatrices();
                    if (Ref.gamesett.panOnZoom)
                    {
                        var mousePosition2 = screenPosToWorldPos(Input.Mouse.Position);
                        Vector3 diff = mousePosition2 - pointerPosWP;
                        panCamera(VectorExt.V3XZtoV2( diff), true);
                    }
                }
            }
        }

        float? targetRotation = null;
        void rotateCameraInput()
        {
            const float XBuffer = 0.6f;
            const float RotationSpeed = 0.00006f;
            const float TargetRotationSpeed = 0.005f;

            if (Math.Abs(player.gameControls.input.cameraTiltZoom.direction.X) > XBuffer)
            {
                lastRotationDir = player.gameControls.input.cameraTiltZoom.directionAndTime.X;
                camRotationKeyDownTime += Ref.DeltaTimeMs;
                camRotation.Value += RotationSpeed * Ref.DeltaTimeMs * lastRotationDir;
            }
            else
            {
                
                if (camRotationKeyDownTime > 0)
                {
                    bool bTap = camRotationKeyDownTime < VikingEngine.Input.InputLib.ButtonMaxClickTimeMs;
                    if (bTap)
                    {//Target next rotation point
                        if (lastRotationDir > 0)
                        { //Right
                            if (camRotation.Value < CamStartRotation)
                            {
                                targetRotation = CamStartRotation;
                            }
                            else
                            {
                                targetRotation = camRotation.Bounds.Max;
                            }
                        }
                        else
                        {
                            if (camRotation.Value > CamStartRotation)
                            {
                                targetRotation = CamStartRotation;
                            }
                            else
                            {
                                targetRotation = camRotation.Bounds.Min;
                            }
                        }
                    }
                    else
                    {
                        targetRotation = null;
                    }
                    camRotationKeyDownTime = 0;
                }

               

                if (targetRotation != null)
                {
                    float diff = targetRotation.Value - camRotation.Value;
                    
                    float dir = lib.ToLeftRight(diff);

                    float speed = TargetRotationSpeed * Ref.DeltaTimeMs;

                    if (speed > Math.Abs(diff))
                    {
                        camRotation.Value = targetRotation.Value;
                        targetRotation = null;
                    }
                    else
                    {
                        float rotAdd =speed * dir;
                        camRotation.Value += rotAdd;
                    }
                }
            }

            camera.TiltX = camRotation.Value;

            if (player.gameControls.input.inputSource.HasKeyBoard)
            {
                if (player.gameControls.input.cameraTiltUp.DownEvent)
                {
                    toggleCameraTiltUp();
                }
            }
            else if (player.gameControls.input.inputSource.IsController)
            {
                controllerCameraUp();
            }
        }

        public void toggleCameraTiltUp()
        {
            currentTiltYAngleOption++;
            if (currentTiltYAngleOption >= 3)
            {
                currentTiltYAngleOption = -1;
            }

            player.mapLayersManager.TiltYAdd = currentTiltYAngleOption * TiltYUpAngle;
        }

        void controllerCameraUp()
        {
            if (player.gameControls.input.inputSource.Controller.IsButtonDown(Buttons.LeftTrigger))
            {
                player.mapLayersManager.TiltYAdd = Bound.Set(player.mapLayersManager.TiltYAdd + player.gameControls.input.inputSource.Controller.JoyStickValue(ThumbStickType.Right).DirAndTime.Y * 0.0012f,
                    3 * TiltYUpAngle, -1 * TiltYUpAngle);
            }
        }

        float PanSpeed()
        {
            const float MinZoomAffect = 1.5f;

            if (controllerInput)
            {
                return Ref.gamesett.keyPanSpeed * 0.0003f * Bound.Min(targetZoom, MinZoomAffect);
            }
            else
            {
                return Ref.gamesett.keyPanSpeed * 0.0006f * Bound.Min(targetZoom, MinZoomAffect);
            }
        }

        Vector2 movePanLength = Vector2.Zero;

        private void keypPanInput()
        {
            if (player.gameControls.controllerPointer != null)
            {
                return;
            }
            if (player.gameControls.diplomacy != null && player.gameControls.diplomacy.hasSelection())
            {
                return;
            }

            movePanLength = -player.gameControls.input.move.directionAndTime * PanSpeed();
            panCamera(movePanLength, true);
        }

        void mousePanInput()
        {
            //if (!player.hud.hudMouseOver() && !controllerInput)
            //{
            //
            //
            if (!controllerInput)
            {
                if (player.gameControls.input.mousePan.DownEvent)
                {
                    panDownInput = true;
                }

                if (panDownInput && hasMouseMapPanInput())
                {
                    //bool hasValue;
                    Vector3 prevMousePosition = screenPosToWorldPos(Input.Mouse.Position - Input.Mouse.MoveDistance);

                    Vector3 diff = pointerPosWP - prevMousePosition;

                    panCamera(VectorExt.V3XZtoV2(diff), false);

                    return;
                }

                if (DssRef.state.localPlayers.Count == 1)
                {
                    if (!player.gameControls.input.mousePan.IsDown &&
                        //!player.gameControls.input.ControllerSelect.IsDown &&
                        Input.Mouse.HasEdgePush())
                    {
                        panCamera(-Input.Mouse.EdgePush() * Ref.DeltaTimeMs * PanSpeed(), true);

                    }
                }
            }
            //}
        }

        /// <summary>
        /// Chase selected object
        /// </summary>
        void cameraFocusUpdate()
        {
            if (cameraFocus != null)
            {   
                Vector3 goal = cameraFocus.WorldPos();
                
                goal.Y = 0;
                goal.Z += 0.5f;
                Vector3 diff = goal - camera.LookTarget;
                diff.Y = 0;
                if (VectorExt.HasValue(diff))
                {
                    float panSpeed = 0.003f * Ref.DeltaTimeMs * camera.targetZoom;
                    float length = diff.Length();
                    if (panSpeed >= length)
                    {
                        camera.LookTarget = goal;
                    }
                    else
                    {
                        
                        diff.Normalize();
                        Vector3 move = diff * panSpeed;
                        if (!Debug.CorruptValue(move))
                        {
                            camera.LookTarget += move;
                        }
                    }
                    
                    playerPointerPos = camera.LookTarget;
                } 
            }
        }

        public void focusMap(bool focus)
        {
            controllerPointer.Visible = focus;
        }

        void panCamera(Vector2 pan, bool followCamRotation)
        {
            //pan.Y = 0;
            if (VectorExt.HasValue(pan))
            {
                if (followCamRotation)
                {
                    pan = VectorExt.RotateVector(pan, camera.Tilt.X - CamStartRotation);
                }

                camera.MoveLookTargetXZ( - pan);
                onPan();
            }
        }

        public void setCameraPosition(Vector2 worldXZ)
        {
            camera.LookTargetXZ = worldXZ;
            onPan();
        }

        public void setCameraPos(IntVector2 tile)
        {
            playerPointerPos = WP.ToWorldPos(tile);
            camera.LookTarget = playerPointerPos;
        }

        void onPan()
        {
            cameraFocus = null;

            camera.setLookTargetXBound(panBounds.Position.X, panBounds.Right);
            camera.setLookTargetZBound(panBounds.Position.Y, panBounds.Bottom);

            playerPointerPos = camera.LookTarget;

            DssRef.world.WorldBound(ref playerPointerPos.X, ref playerPointerPos.Z);
            playerPointerPos.Y = DssRef.world.GetTile(playerPointerPos).GroundY() + 0.5f;
        }

        public void loadCamPos()
        {
            playerPointerPos = camera.LookTarget;
            camRotation.Value = camera.TiltX;
        }

        bool hasMouseMapPanInput()
        {
            return player.gameControls.input.inputSource.HasMouse &&
                rectangleLines == null &&
                player.gameControls.input.mousePan.IsDown &&
                Input.Mouse.bMoveInput;
        }

        private void updateCamera()
        {
            Vector3 camTarget = playerPointerPos;
            camTarget.Y = 0.1f;

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
                    return GameObjectType.NUM;
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

        public GameObjectType FocusObjectType()
        {
            if (player.gameControls.diplomacy != null)
            {
                return GameObjectType.Faction;
            }

            var obj = FocusObject();
            return obj != null ? obj.gameobjectType() : GameObjectType.NONE;
        }

        public bool NoSelection() { return selection.obj == null; }
    }

   
}
