using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    class MapControls
    {
        //public Graphics.Image pointer;
        public ToggEngine.Display2D.AbsToolTip tooltip = null;
        Graphics.Image centeredErrorCross;
        List<Graphics.Mesh> squareSelectionFrame = new List<Graphics.Mesh>(8);
        

        public IntVector2 selectionIntV2 = IntVector2.Zero;
        public Vector2 selectionV2 = Vector2.Zero;
        SpriteName selectionSprite = SpriteName.MissingImage;
        public BoardSquareContent selectedTile;
        
        Vector3 pointerPos3D;
        
        public AvailableTilesGUI availableTiles;
        
        //public VikingEngine.Graphics.TopViewCamera camera;
        AbsGenericPlayer player;
        LOSTiles LOSTiles = null;
        public bool isOnNewTile = false;

        public bool prevMouseOverHud = false;
        
        //public bool isMapPan = false;
        public bool mouseDownOnMapPan = false;
        public bool unlockEdgePush = false;

        public MapControls(AbsGenericPlayer player)
        {
            
            this.player = player;
            //this.camera = toggRef.gamestate.camera;
            //pointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, new Vector2(Engine.Screen.IconSize * 2), ImageLayers.Bottom3, true, false);

            centeredErrorCross = new Graphics.Image(SpriteName.RedErrorCross, Vector2.Zero, Engine.Screen.IconSizeV2 * 1f, ImageLayers.Top1, true);
            centeredErrorCross.Visible = false;

            addSelectionFrame(IntVector2.Zero);
        }

        public bool mapGotFocus(bool mouseOverHud)
        {
            return !mouseOverHud && prevMouseOverHud; 
        }

        public bool mapLostFocus(bool mouseOverHud)
        {
            return mouseOverHud && !prevMouseOverHud;
        }

        public void addSelectionFrame(IntVector2 pos)
        {
            var frame = new Graphics.Mesh(LoadedMesh.plane, framePos(pos), Vector3.One,
                Graphics.TextureEffectType.Flat, selectionSprite, Color.White);
            squareSelectionFrame.Add(frame);

            frame.Color = squareSelectionFrame[0].Color;           
        }

        public void selectArea(Rectangle2 area)
        {
            player.mapControls.removeMultiselection();

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                if (loop.Position != selectionIntV2)
                {
                    addSelectionFrame(loop.Position);
                }
            }
        }

        public void removeToolTip()
        {
            if (tooltip != null)
            {
                tooltip.DeleteMe();
                tooltip = null;
            }
            centeredErrorCross.Visible = false;
        }

        public void viewErrorCross(bool view)
        {
            centeredErrorCross.Visible = view;
        }

        public void setSelectionVisible(bool visible)
        {
            foreach (var m in squareSelectionFrame)
            {
                m.Visible = visible;
            } 
        }        

        public void updateMapMovement(bool allowInput, bool cirleCollision = false)
        {
            const float PanSpeed = 0.012f;

            isOnNewTile = false;
            //isMapPan = false;
            zoomInput();

            Debug.CtrlBreak(false);

            if (toggLib.MouseInputMode)
            {
                if (tooltip != null)
                {
                    tooltip.update();
                    centeredErrorCross.position = Input.Mouse.Position;
                }

                bool foundScreenPos;
                Vector3 mapPos = Ref.draw.Camera.ScreenPosTo3D(Input.Mouse.Position, out foundScreenPos);
                if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
                {
                    mouseDownOnMapPan = true;
                }
                if (Input.Mouse.IsButtonDown(MouseButton.Right) && mouseDownOnMapPan)
                {
                    unlockEdgePush = false;
                    Vector3 diff = mapPos - pointerPos3D;

                    toggRef.cam.panCamera(diff);
                }
                else
                {
                    mouseDownOnMapPan = false;
                    setSelectionPos(toggLib.ToV2(mapPos), cirleCollision);
                }
               
                toggRef.cam.panCamera(toggLib.ToV3(-toggRef.inputmap.movement.directionAndTime * PanSpeed));
                
                if (!mouseDownOnMapPan)
                {
                    if (Input.Mouse.HasEdgePush())
                    {
                        if (unlockEdgePush)
                        {
                            toggRef.cam.panCamera(toggLib.ToV3(-Input.Mouse.EdgePush() * Ref.DeltaTimeMs * PanSpeed));
                        }
                    }
                    else
                    {
                        unlockEdgePush = true;
                    }
                }
            }
                       
            if (allowInput)
            {
                if (toggRef.inputmap.lineOfSight.IsDown)
                {
                    if (LOSTiles == null || isOnNewTile)
                    {
                        var prev = LOSTiles;
                        LOSTiles = new LOSTiles(selectionIntV2, prev);
                    }
                }
                else if (LOSTiles != null)
                {
                    LOSTiles.DeleteModels();
                    LOSTiles = null;
                }
            }
        }

        public void setSelectionPos(Vector3 pos)
        {
            setSelectionPos(toggLib.ToV2(pos), false);
        }

        void setSelectionPos(Vector2 newSelPos, bool cirleCollision)
        {
            if (toggRef.board.selectionBounds.IntersectPoint(newSelPos))
            {
                selectionV2 = newSelPos;

                pointerPos3D.X = selectionV2.X;
                pointerPos3D.Z = selectionV2.Y;

                IntVector2 newSquare = new IntVector2(selectionV2);
                if (newSquare != selectionIntV2 || selectedTile == null)
                {
                    bool bSelectNewSquare = true;

                    if (cirleCollision)
                    {
                        bSelectNewSquare = Physics.PhysicsLib2D.PointInsideCirkle(newSelPos, 
                            toggRef.board.toWorldPosXZ_Center(newSquare), Board.MoveCollRadius);
                    }

                    if (bSelectNewSquare)
                    {
                        selectionIntV2 = newSquare;
                        onNewTile();
                    }
                }
            }
        }

        void onNewTile()
        {
            removeMultiselection();
            squareSelectionFrame[0].Position = framePos(selectionIntV2);
            selectedTile = toggRef.board.tileGrid.Get(selectionIntV2);
            player.onNewTile();

            isOnNewTile = true;
        }

        public void removeMultiselection()
        {
            while (squareSelectionFrame.Count > 1)
            {
                arraylib.PullLastMember(squareSelectionFrame).DeleteMe();
            }
        }

        public void SetFramePos(IntVector2 pos)
        {
            squareSelectionFrame[0].Position = framePos(pos);
        }

        Vector3 framePos(IntVector2 pos)
        {
            return toggRef.board.toModelCenter(pos, ToggEngine.Map.SquareModelLib.TerrainY_SelectionFrame);
        }

        public void GroupFrame(AttackTargetGroup group)
        {
            removeMultiselection();

            foreach (var m in group)
            {
                if (m.position != selectionIntV2)
                {
                    addSelectionFrame(m.position);
                }
            }
        }

        public void GroupFrame(List<IntVector2> group)
        {
            removeMultiselection();

            foreach (var m in group)
            {
                if (m != selectionIntV2)
                {
                    addSelectionFrame(m);
                }
            }
        }

        public void zoomInput()
        {
            float scrollValue = Input.Mouse.ScrollValue * 0.008f;

            toggRef.cam.zoom(scrollValue);
            //if (scrollValue != 0)
            //{
            //    camera.CurrentZoom = Bound.Set(camera.CurrentZoom - scrollValue, ZoomBound);
            //    setCamBounds();
            //}
        }

        public void removeAvailableTiles()
        {
            if (availableTiles != null)
            {
                availableTiles.DeleteAll();
                availableTiles = null;
            }
        }

        public void SetAvailableTiles(List<IntVector2> positions, List<IntVector2> targets = null)
        {
            removeAvailableTiles();
            if (arraylib.HasMembers(positions) || 
                arraylib.HasMembers(targets))
            {
                availableTiles = new AvailableTilesGUI(positions, targets);
            }
        }

        public AbsUnit SelectedUnit
        {
            get { return selectedTile == null? null : selectedTile.unit; }
            set { selectedTile.unit = value; }
        }

        public HeroQuest.SpikeTrap SelectedTrap
        {
            get
            {
                if (selectedTile != null)
                {
                    return selectedTile.tileObjects.GetObject(TileObjectType.DamageTrap) as HeroQuest.SpikeTrap;
                }
                return null;
            }
        }

        public AbsTileObject SelectedTileObject(TileObjectType type)
        {
            if (selectedTile != null)
            {
                return selectedTile.tileObjects.GetObject(type);
            }
            return null;
        }

        public void setAvailable(bool available)
        {
            setAvailable(available ? MapSquareAvailableType.Enabled : MapSquareAvailableType.Disabled);//available ? Color.White : Color.Red);
        }

        
        public void setAvailable(MapSquareAvailableType available)
        {           
            Color col;

            switch (available)
            {
                case MapSquareAvailableType.None:
                    selectionSprite = SpriteName.cmdSelectionDotted;
                    col = Color.White;
                    break;
                case MapSquareAvailableType.Disabled:
                    selectionSprite = SpriteName.cmdSelectionDotted;
                    col = new Color(104, 24, 34);
                    break;
                case MapSquareAvailableType.Enabled:
                    selectionSprite = SpriteName.cmdSelectionFull;
                    col = Color.White;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var m in squareSelectionFrame)
            {
                m.SetSpriteName(selectionSprite);
                m.Color = col;
            }
        }

        //public void updateSpectateCamera(IntVector2 targetPos, bool inCamCheck = false)
        //{
        //    //Calc how far the camera must travel to view the edge of the board

        //    if (targetPos.X >= 0)
        //    {
        //        Vector3 targetV3 = toggRef.board.toWorldPos_Center(targetPos, 0f);

        //        if (inCamCheck)
        //        {
        //            Vector2 screenPos = camera.From3DToScreenPos(targetV3, Engine.Draw.defaultViewport);
        //            if (Engine.Screen.SafeArea.IntersectPoint(screenPos))
        //            {
        //                return;
        //            }
        //        }

        //        float zoomPerc = ZoomBound.GetValuePercentPos(camera.targetZoom);
        //        float squaresHWidthInviewY = ZoomToViewRadiusY.GetFromPercent(zoomPerc);
        //        float squaresHWidthInviewX = squaresHWidthInviewY / Engine.Screen.SafeArea.Width * Engine.Screen.Height;

        //        Vector2 targetRadius = new Vector2(
        //            Bound.Min(toggRef.board.HalfSize.X - squaresHWidthInviewX, 0f) + 1f,
        //            Bound.Min(toggRef.board.HalfSize.Y - squaresHWidthInviewY, 0f) + 1f);

        //        const float BoundAdj = -0.5f;
                    
        //        var cameraGoal = new Vector3(
        //            Bound.Set(targetV3.X, toggRef.board.HalfSize.X - targetRadius.X + BoundAdj, toggRef.board.HalfSize.X + targetRadius.X + BoundAdj),
        //            targetV3.Y,
        //            Bound.Set(targetV3.Z, toggRef.board.HalfSize.Y - targetRadius.Y + BoundAdj, toggRef.board.HalfSize.Y + targetRadius.Y + BoundAdj));
        //        camera.moveTowards(cameraGoal);

        //        camera.clearGoalTarget();
        //    }
        //}

        public void setSelection(IntVector2 pos)
        {
            selectionV2.X = pos.X;
            selectionV2.Y = pos.Y;
            onNewTile();
        }

        public void setMapselectionVisible(bool visible)
        {
            squareSelectionFrame[0].Visible = visible;
            //pointer.Visible = visible;
        }


        public void EndTurn()
        {
            setMapselectionVisible(false);
            removeAvailableTiles();

            removeToolTip();
        }
    }
}
