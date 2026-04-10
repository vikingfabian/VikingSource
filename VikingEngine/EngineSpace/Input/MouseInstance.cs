using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Win32;
using VikingEngine.Engine;
using System.Runtime.CompilerServices;

namespace VikingEngine.Input
{

    class MouseInstance
    {
        public Vector2 Position, PrevPosition, MoveDistance;

        bool isMouse;
        public bool linkToMouse = false; 
        bool inUse = false;
        Rectangle2 bounds;
        public bool inBounds = true;
        
        VectorRect MousePushEdge, MousePushEdgeMax;
        public IDirectionalMap inGameMap, inMenuMap;
        public bool centerlockAndHide = false;
        bool hide = false;
        Graphics.Image customMousePointer = null;
        Vector2 offEdgeMovement = Vector2.Zero;

        /// <summary>
        /// Can be turned off for game mode with no mouse pointer
        /// </summary>
        public bool isActive = true;

        public MouseInstance()
        {
            bounds = Engine.Screen.Area.Rectangle2;
            isMouse = true;
        }

        public MouseInstance(PlayerData playerData, int playerCount, IDirectionalMap inGameMap = null, IDirectionalMap inMenuMap = null)
        {            
            SetPlayer(playerData, playerCount);
            isMouse = inGameMap == null;
            this.inGameMap = inGameMap;
            this.inMenuMap = inMenuMap;
        }

        public void SetPlayer(PlayerData playerData, int playerCount)
        {
            isActive = playerData.inputMap.inputSource.HasMouseInstance;
            bounds.Rect = playerData.view.DrawArea;
            MousePushEdge = new VectorRect(bounds);
            MousePushEdge.AddRadius(-4);
            MousePushEdgeMax = MousePushEdge;
            MousePushEdgeMax.AddRadius(10);
            inUse = true;
            RefreshMouseVisible();

            if (!isMouse)
            {
                SetPosition(bounds.Center);
                if (playerCount == 1)
                { 
                    linkToMouse = true;
                }
            }
        }

        public void Update()
        {
            if (isActive)
            {

                if (isMouse)
                {
                    Position = Mouse.Position;
                    PrevPosition = Mouse.Position;
                    MoveDistance = Mouse.MoveDistance;
                }
                else if (Mouse.MenuMode)
                {
                    //Everyone moves the mouse cursor
                    var pos = Mouse.Position;
                    if (inMenuMap != null)
                    {
                        pos += inMenuMap.directionAndTime;
                        Microsoft.Xna.Framework.Input.Mouse.SetPosition(Convert.ToInt32(pos.X), Convert.ToInt32(pos.Y));
                    }
                }
                else
                {
                    PrevPosition = Position;
                    if (inGameMap != null)
                    {
                        var move = inGameMap.directionAndTime;
                        if (VectorExt.HasValue(move))
                        {
                            Position += move;
                            MoveDistance = move;
                            if (linkToMouse)
                            {
                                Mouse.Instances[0].SetPosition(Position);
                            }
                        }
                        else if (linkToMouse)
                        {
                            Position = Mouse.Instances[0].Position;
                            MoveDistance = Mouse.Instances[0].MoveDistance;
                        }
                    }

                }

                offEdgeMovement = Vector2.Zero;

                if (MainGame.GameIsActive)
                {
                    inBounds = true;
                    if (centerlockAndHide)
                    {
                        SetPosition(Engine.Screen.MonitorCenter);
                    }
                    else if (LockToArea())
                    {
                        bounds.KeepTilePointInArea(Position, out Position, out bool offBounds, out offEdgeMovement);
                        if (offBounds)
                        {
                            SetPosition(Position);
                        }
                    }
                    else
                    {
                        inBounds = bounds.IntersectPoint(Position);
                    }
                }
            }
        }

        public void Draw()
        {
            if (isActive && customMousePointer != null && RenderMouseCursor())
            {
                customMousePointer.position = Position;
                customMousePointer.Draw(0);
            }
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
            if (isMouse)
            {
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(Convert.ToInt32( position.X), Convert.ToInt32(position.Y));
            }
            else
            {
                Position = position;
                if (linkToMouse)
                {
                    Mouse.Instances[0].SetPosition(position);
                }
            }
        }
        public void SetPosition(IntVector2 position)
        {
#if PCGAME
            Position = position.Vec;
            if (isMouse)
            {
                Microsoft.Xna.Framework.Input.Mouse.SetPosition(position.X, position.Y);
            }
            else
            {
                Position = position.Vec;
                if (linkToMouse)
                {
                    Mouse.Instances[0].SetPosition(position);
                }
            }
#endif
        }

        public void RestoreDefault()
        {
            centerlockAndHide = false;
            hide = false;
            RefreshMouseVisible();
        }

        public void Hide()
        {
            hide = true;
            RefreshMouseVisible();
        }

        public void View()
        {
            centerlockAndHide = false;
            hide = false;
            RefreshMouseVisible();
        }
        public void CenterLockAndHide()
        {
            centerlockAndHide = true;
            RefreshMouseVisible();
        }
        public bool RenderMouseCursor()
        {
            if (Mouse.MenuMode)
            {
                return isMouse;
            }
            return !centerlockAndHide && !hide && inUse;
        }
        public void RefreshMouseVisible()
        {
            bool visible = RenderMouseCursor();

            if (isMouse && !Ref.gamesett.customCursor)
            {
                Ref.main.IsMouseVisible = visible;

                if (customMousePointer != null)
                {
                    customMousePointer.Visible = false;
                }
            }
            else
            {
                if (customMousePointer == null)
                {
                    customMousePointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, true, false);
                }

                if (customMousePointer != null)
                {
                    customMousePointer.Visible = visible;
                    if (isMouse)
                    {
                        Ref.main.IsMouseVisible = !visible;
                    }
                }
            }
        }

        public Vector2 EdgePush(float passiveSpeed, float activeSpeed)
        {
            switch (Ref.gamesett.edgePush)
            {
                case MouseEdgePush.Passive:
                    Vector2 result = Vector2.Zero;
                    //if (Engine.Screen.Area.IntersectPoint(Position))
                    //{
                    if (Position.X < MousePushEdge.X &&
                        Position.X > MousePushEdgeMax.X)
                    {
                        result.X = -passiveSpeed;
                    }
                    else if (Position.X > MousePushEdge.Right &&
                        Position.X < MousePushEdgeMax.Right)
                    {
                        result.X = passiveSpeed;
                    }

                    if (Position.Y < MousePushEdge.Y &&
                        Position.Y > MousePushEdgeMax.Y)
                    {
                        result.Y = -passiveSpeed;
                    }
                    else if (Position.Y > MousePushEdge.Bottom &&
                        Position.Y < MousePushEdgeMax.Bottom)
                    {
                        result.Y = passiveSpeed;
                    }
                    //}
                    return result;

                case MouseEdgePush.Active:
                    return offEdgeMovement * activeSpeed;

                default:
                    return Vector2.Zero;
            }

        }


        public bool HasEdgePush()
        {
            return !Mouse.MenuMode && Ref.gamesett.lockMouseToWindow && Ref.gamesett.edgePush != MouseEdgePush.None && !MousePushEdge.IntersectPoint(Position);
        }

        public bool bMoveInput
        {
            get
            {
                return MoveDistance.X != 0 || MoveDistance.Y != 0;
            }
        }

        bool LockToArea()
        {
            return !isMouse || (Ref.gamesett.lockMouseToWindow && !Mouse.MenuMode);
        }
       
    } 
}
