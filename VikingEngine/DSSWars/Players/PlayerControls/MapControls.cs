using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.DSSWars.Players
{
    class MapControls
    {
        const float CamMaxRotation = 0.5f;
        const float CamStartRotation = MathHelper.PiOver2;
        static readonly IntervalF ZoomRange = MapDetailLayerManager.FullZoomRange;

        FloatInBound camRotation = new FloatInBound(CamStartRotation, new IntervalF(CamStartRotation - CamMaxRotation, CamStartRotation + CamMaxRotation), false);

        LocalPlayer player;

        Ray ray;
        Plane groundPlane = new Plane(Vector3.UnitY, 0);
        SafeCollectAsynchList<AbsMapObject> nearMapObjects = new SafeCollectAsynchList<AbsMapObject>(8);
        SafeCollectAsynchList<AbsSoldierUnit> nearDetailUnits = new SafeCollectAsynchList<AbsSoldierUnit>(64);

        public Vector3 playerPointerPos = Vector3.Zero, mousePosition = Vector3.Zero;
        
        public IntVector2 tilePosition, subTilePosition;
        public bool onNewTile = false;

        public Graphics.TopViewCamera camera;
        Graphics.Image controllerPointer;

        public Selection hover;
        public Selection selection;//
        bool controllerInput;
        public bool unlockEdgePush = false;

        public AbsGameObject cameraFocus = null;

        public MapControls(LocalPlayer player)
        {
            this.player = player;

            camera = new TopViewCamera(200, 
                new Vector2(MathHelper.PiOver2, Map.MapDetailLayerManager.NormalCamAngle),
                player.playerData.view.DrawAreaF.Width, player.playerData.view.DrawAreaF.Height);
            camera.FarPlane = 800;
            camera.positionChaseLengthPercentage = 0.9f;
            camera.FieldOfView = 20;
            camera.UseTerrainCollisions = false;
            camera.zoomChaseLengthPercentage = 0.5f;

            hover = new Selection(player);
            selection = new Selection(player);
            
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
            if (controllerInput)
            {
                mousePosition = screenPosToWorldPos(controllerPointer.position);

                IntVector2 prevTile = tilePosition;
                tilePosition = WP.ToTilePos(mousePosition);
                onNewTile = prevTile != tilePosition;
                
                if (focusedObjectMenuState())
                {
                    player.hud.displays.updateMove();

                    if (player.input.ControllerFocus.DownEvent)
                    {
                        setObjectMenuFocus(false);
                    }
                }
                else
                {
                    if (selection.obj != null)
                    {
                        if (player.input.ControllerFocus.DownEvent)
                        {
                            setObjectMenuFocus(true);
                        }
                    }

                    panInput();
                    //Find closest object
                    hover.begin(true);
                    {
                        controllerHoverUpdate();
                    }
                    hover.end();                    
                }

                if (player.input.ControllerCancel.DownEvent)
                {
                    player.hud.displays.clearMoveSelection();
                    player.clearSelection();
                }

                checkSelectionAlive();
                selection.end();
                selection.begin(false);

                updateSeletionGui();
            }
            else
            {
                if (mouseOverHud)
                {
                    hover.clear();

                }
                else
                {
                    mousePosition = screenPosToWorldPos(Input.Mouse.Position);

                    IntVector2 prevTile = tilePosition;
                    tilePosition = WP.ToTilePos(mousePosition);
                    subTilePosition = WP.ToSubTilePos(mousePosition);
                    onNewTile = prevTile != tilePosition;

                    hover.begin(true);
                    {
                        mouseHoverUpdate();
                    }
                    hover.end();
                }

                checkSelectionAlive();
                selection.end();
                selection.begin(false);

                updateSeletionGui();

                panInput();
            }

            zoomInput();
            cameraFocusUpdate();
            updateCamera();
        }

        Vector3 screenPosToWorldPos(Vector2 screenPos)
        {
            ray = camera.CastRay(screenPos, player.playerData.view.Viewport);

            bool hasValue;
            return this.camera.CastRayInto3DPlane(ray, groundPlane, out hasValue);
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

        void mouseHoverUpdate()
        {
            //nearMapObjects.checkForUpdatedList();
            //nearDetailUnits.checkForUpdatedList();

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
                        if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                        {
                            m.debugTagged = true;
                            m.group.debugTagged = true;
                        }
                        break;
                    }
                }
            }
        }

        void controllerHoverUpdate()
        {
            if (player.drawUnitsView.current.type == MapDetailLayerType.TerrainOverview2)
            {
                const float FriendlyPriorityDistAdd = 0.25f;
                float maxDistance = 5;
                if (player.armyControls != null)
                {
                    maxDistance = 1.5f;
                }

                var nearMapObjects = DssRef.world.unitCollAreaGrid.MapControlsNearMapObjects(tilePosition, true);
                AbsMapObject closestObj= null;
                float closest = float.MaxValue;
                foreach (var m in nearMapObjects)
                {
                    var dist= VectorExt.PlaneXZLength(m.position - mousePosition);
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
            }
        }

        public void onSelect()
        {
            selection.obj = hover.obj;
            if (controllerInput)
            {                
                if (focusedObjectMenuState())
                {
                    setObjectMenuFocus(true);
                }
            }
        }

        public bool focusedObjectMenuState()
        {
            return selection.obj != null &&
                controllerInput &&
                (selection.obj.gameobjectType() == GameObjectType.City || selection.menuFocus);
        }

        void setObjectMenuFocus(bool set)
        {
            if (!set && selection.obj.gameobjectType() == GameObjectType.City)
            {
                return;
            }

            selection.menuFocus = set;
            player.hud.displays.objectDisplay.viewOutLine(set);

            if (set)
            {
                playerPointerPos = selection.obj.position;
                player.hud.displays.beginMove(1);
            }
            else
            {
                player.hud.displays.clearMoveSelection();
            }
            controllerPointer.Visible = !set;
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
            if (hover.obj != null && hover.obj != selection.obj)
            {
                hover.frameModel.Visible = true;
                hover.obj.selectionFrame(true, hover);

                hover.frameModel.Color = hover.obj.GetFaction() == player.faction? Color.White : Color.LightGray;

                updateSelectionGui(hover);
            }
            else
            {
                //hover.guiModels.DeleteAll();
                hover.ClearSelectionModels();
            }

            if (selection.obj != null)
            {
                selection.frameModel.Visible = true;
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
                if (hasMouseMapMoveInput())
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

        bool hasMouseMapMoveInput()
        {
            return player.input.inputSource.HasMouse &&
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
