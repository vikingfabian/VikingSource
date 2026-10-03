using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Graphics;
using VikingEngine.PJ.CarBall;
using VikingEngine.ToGG;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.Core.BlackBolts.Player
{
    class CameraControl
    {
        const float PanSpeed = 0.012f;

        static IntervalF ZoomBound;
        static readonly IntervalF ZoomToViewRadiusY = new IntervalF(1, 4);

        public TopViewCamera camera;

        public bool spectateMode = false;
        public IntVector2 tilePos;
        Vector3 pointerPos3D;
        public bool inCamCheck = false;

        public bool mouseDownOnMapPan = false;

        

        public CameraControl()
        {

            ZoomBound = new IntervalF(20, 80);

            const float OverviewCamAngle = 0.65f;
            camera = new TopViewCamera(ZoomBound.Center, new Vector2(MathHelper.PiOver2, OverviewCamAngle));
            camera.FarPlane = 800;
            camera.FieldOfView = 20;
            camera.positionChaseLengthPercentage = 0.16f;

            Ref.draw.Camera = camera;
            Ref.draw.ClrColor = Color.Black;
        }

        public void panCamera(Vector3 pan)
        {
            pan.Y = 0;
            if (VectorExt.HasValue(pan))
            {
                spectateMode = false;

                Vector2 center1 = Ref.draw.Camera.From3DToScreenPos(camera.LookTarget,
                    Engine.XGuide.LocalHost.view.Viewport);//player.pData.view.Viewport);

                camera.LookTarget -= pan;
                //setCamBounds();

                Vector2 center2 = Ref.draw.Camera.From3DToScreenPos(camera.LookTarget,
                    Engine.XGuide.LocalHost.view.Viewport);//player.pData.view.Viewport);
                //toggRef.absPlayers.OnMapPan(center1 - center2);
            }
        }

        public void zoom(float scrollValue)
        {
            if (scrollValue != 0)
            {
                camera.CurrentZoom = Bound.Set(camera.CurrentZoom - scrollValue, ZoomBound);
                //setCamBounds();
            }
        }

        void setCamBounds()
        {
            //tan(angle) * zoom = radius

            float camRadius = (float)(Math.Tan(MathHelper.ToRadians(camera.FieldOfView * 0.5f)) * camera.targetZoom);

            float minX = toggRef.board.camBounds.X + camRadius;
            float maxX = toggRef.board.camBounds.Right - camRadius;

            if (maxX < minX)
            {
                minX = toggRef.board.camBounds.HalfSize().X;
                maxX = minX;
            }

            float minY = toggRef.board.camBounds.Y + camRadius;
            float maxY = toggRef.board.camBounds.Bottom - camRadius;

            if (maxY < minY)
            {
                minY = toggRef.board.camBounds.HalfSize().Y;
                maxY = minY;
            }

            camera.setLookTargetXBound(minX, maxX);
            camera.setLookTargetZBound(minY, maxY);
        }

        //public void spectate(IntVector2 targetPos, bool inCamCheck = false)
        //{
        //    spectateMode = true;

        //    this.targetPos = targetPos;
        //    this.inCamCheck = inCamCheck;
        //}

        public void update(Player player, bool overHud)
        {
            if (!overHud)
            {
                panCamera(toggLib.ToV3(-player.inputMap.movement.directionAndTime * PanSpeed));

                float scrollValue = Input.Mouse.ScrollValue * 0.02f;
                zoom(scrollValue);

                Vector3 mapPos = Ref.draw.Camera.ScreenPosTo3D(Input.Mouse.Position, out bool foundScreenPos);
                if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
                {
                    mouseDownOnMapPan = true;
                }
                if (Input.Mouse.IsButtonDown(MouseButton.Right) && mouseDownOnMapPan)
                {
                    //unlockEdgePush = false;
                    Vector3 diff = mapPos - pointerPos3D;

                    panCamera(diff);
                }
                else
                {
                    mouseDownOnMapPan = false;
                    setSelectionPos(player, toggLib.ToV2(mapPos));
                }

            }
            camera.Time_Update(Ref.DeltaTimeMs);
        }

        void setSelectionPos(Player player, Vector2 newSelPos)
        {
            pointerPos3D.X = newSelPos.X;
            pointerPos3D.Z = newSelPos.Y;

            IntVector2 tile = new IntVector2(newSelPos.X, newSelPos.Y);
            if (tile != tilePos)
            {
                tilePos = tile;
                player.onNewTile(tile);
            }
            
        }
        
    }
}
