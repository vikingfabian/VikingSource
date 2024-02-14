using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;

namespace VikingEngine.ToGG.ToggEngine
{
    class Camera
    {
        static IntervalF ZoomBound;
        static readonly IntervalF ZoomToViewRadiusY = new IntervalF(1, 4);

        public TopViewCamera camera;

        public bool spectateMode = false;
        public IntVector2 targetPos;
        public bool inCamCheck = false;

        public Camera()
        {
            toggRef.cam = this;

            ZoomBound = new IntervalF(8, toggRef.InEditor ? 80 : 40);

            const float OverviewCamAngle = 0.65f;
            camera = new TopViewCamera(20, new Vector2(MathHelper.PiOver2, OverviewCamAngle));
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
                setCamBounds();

                Vector2 center2 = Ref.draw.Camera.From3DToScreenPos(camera.LookTarget,
                    Engine.XGuide.LocalHost.view.Viewport);//player.pData.view.Viewport);
                toggRef.absPlayers.OnMapPan(center1 - center2);
            }
        }

        public void zoom(float scrollValue)
        {
            if (scrollValue != 0)
            {
                camera.CurrentZoom = Bound.Set(camera.CurrentZoom - scrollValue, ZoomBound);
                setCamBounds();
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

        public void spectate(IntVector2 targetPos, bool inCamCheck = false)
        {
            spectateMode = true;

            this.targetPos = targetPos;
            this.inCamCheck = inCamCheck;
        }

        public void update()
        {
            if (spectateMode && targetPos.X >= 0)
            {
                Vector3 targetV3 = toggRef.board.toWorldPos_Center(targetPos, 0f);

                if (inCamCheck)
                {
                    Vector2 screenPos = camera.From3DToScreenPos(targetV3, Engine.Draw.defaultViewport);
                    if (Engine.Screen.SafeArea.IntersectPoint(screenPos))
                    {
                        return;
                    }
                }

                float zoomPerc = ZoomBound.GetValuePercentPos(camera.targetZoom);
                float squaresHWidthInviewY = ZoomToViewRadiusY.GetFromPercent(zoomPerc);
                float squaresHWidthInviewX = squaresHWidthInviewY / Engine.Screen.SafeArea.Width * Engine.Screen.Height;

                Vector2 targetRadius = new Vector2(
                    Bound.Min(toggRef.board.HalfSize.X - squaresHWidthInviewX, 0f) + 1f,
                    Bound.Min(toggRef.board.HalfSize.Y - squaresHWidthInviewY, 0f) + 1f);

                const float BoundAdj = -0.5f;

                
                var cameraGoal = new Vector3(
                    Bound.Set(targetV3.X, toggRef.board.HalfSize.X - targetRadius.X + BoundAdj, toggRef.board.HalfSize.X + targetRadius.X + BoundAdj),
                    targetV3.Y,
                    Bound.Set(targetV3.Z, toggRef.board.HalfSize.Y - targetRadius.Y + BoundAdj, toggRef.board.HalfSize.Y + targetRadius.Y + BoundAdj));
                camera.moveTowards(cameraGoal);

                camera.clearGoalTarget();
            }
            else
            {
                camera.currentChaseLength = 0;
            }

            camera.Time_Update(Ref.DeltaTimeMs);

            spectateMode = false;
        }
    }
}
