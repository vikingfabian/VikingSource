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
        bool inUse = false;
        Rectangle2 bounds;
        VectorRect MousePushEdge, MousePushEdgeMax;
        public IDirectionalMap inGameMap, inMenuMap;
        public bool centerlockAndHide = false;
        bool hide = false;
        Graphics.Image customMousePointer = null;

        public MouseInstance()
        {
            bounds = Engine.Screen.Area.Rectangle2;
            isMouse = true;
        }

        public MouseInstance(PlayerData playerData, IDirectionalMap inGameMap = null, IDirectionalMap inMenuMap = null)
        {
            SetPlayer(playerData);
            isMouse = inGameMap == null;
            this.inGameMap = inGameMap;
            this.inMenuMap = inMenuMap;
        }

        public void SetPlayer(PlayerData playerData)
        {
            bounds.Rect = playerData.view.DrawArea;
            MousePushEdge = new VectorRect(bounds);
            MousePushEdge.AddRadius(-4);
            MousePushEdgeMax = MousePushEdge;
            MousePushEdgeMax.AddRadius(10);
            inUse = true;
        }


        public void Update()
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
                    Position += inGameMap.directionAndTime;
                }

            }

            if (MainGame.GameIsActive)
            {
                if (centerlockAndHide)
                {
                    SetPosition(Engine.Screen.MonitorCenter);
                }
            }
        }

        public void Draw()
        {
            if (customMousePointer != null && RenderMouseCursor())
            {
                customMousePointer.position = Input.Mouse.Position;
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
            }
            else
            {
                if (visible && customMousePointer == null)
                {
                    customMousePointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, true, false);
                }

                if (customMousePointer != null)
                {
                    customMousePointer.Visible = visible;
                }
            }
        }

        public Vector2 EdgePush()
        {
            Vector2 result = Vector2.Zero;
            //if (Engine.Screen.Area.IntersectPoint(Position))
            //{
                if (Position.X < MousePushEdge.X &&
                    Position.X > MousePushEdgeMax.X)
                {
                    result.X = -1;
                }
                else if (Position.X >MousePushEdge.Right &&
                    Position.X < MousePushEdgeMax.Right)
                {
                    result.X = 1;
                }

                if (Position.Y < MousePushEdge.Y &&
                    Position.Y > MousePushEdgeMax.Y)
                {
                    result.Y = -1;
                }
                else if (Position.Y > MousePushEdge.Bottom &&
                    Position.Y < MousePushEdgeMax.Bottom)
                {
                    result.Y = 1;
                }
            //}
            return result;
        }


        public bool HasEdgePush()
        {
            return !Mouse.MenuMode && Ref.gamesett.lockMouseToWindow && Ref.gamesett.edgePush != MouseEdgePush.None && !MousePushEdge.IntersectPoint(Position);
        }

        public bool bMoveInput
        {
            get
            {
                return MoveDistance.X != 0 || MoveDistance.Y != 0;//currentMouseState.X != previousMouseState.X || currentMouseState.Y != previousMouseState.Y;
            }
        }
        //public void refreshCursor()
        //{
        //    customMousePointer = null;

        //    if (Ref.gamesett.customCursor)
        //    {
        //        customMousePointer = new Graphics.Image(SpriteName.cmdPointer, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.AbsoluteTopLayer, true, false);
        //    }
        //    RefreshMouseVisible();// = !Ref.gamesett.customMouse;
        //}
    } 
}
