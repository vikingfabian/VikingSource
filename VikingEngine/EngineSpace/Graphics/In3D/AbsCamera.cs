using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest;
using VikingEngine.EngineSpace.Graphics.DeferredRendering;
using VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights;
using VikingEngine.EngineSpace.Maths;

namespace VikingEngine.Graphics
{
    enum CameraType
    {
        TopView,
        TopDown,
        FirstPerson,
    }

    abstract class AbsCamera : Mesh
    {
        /* Constants */
        public const float StandardFOV = 45;

        /* Statics */
        public static readonly IntervalF PlayerSettingsFOVBounds = new IntervalF(2, 120);
        public static readonly IntervalF ActualFOVBounds = new IntervalF(2, 179);
        private static readonly IntervalF TiltYBounds = new IntervalF(0.05f, MathHelper.Pi - 0.05f);

        private static int nextCamIndex = 0;

        /* Properties */
        public Vector3 LookTarget
        {
            get { return lookTarget; }
            set 
            { 
                lookTarget = value;
                clearGoalTarget(); 
            }
        }

        public void clearGoalTarget()
        {
            goalLookTarget = lookTarget;
        }

        public void setLookTargetXBound(float min, float max)
        {
            if (lookTarget.X < min)
            {
                lookTarget.X = min;
                goalLookTarget.X = min;
            }
            else if (lookTarget.X > max)
            {
                lookTarget.X = max;
                goalLookTarget.X = max;
            }
        }

        public void setLookTargetYBound(float min, float max)
        {
            if (lookTarget.Y < min)
            {
                lookTarget.Y = min;
                goalLookTarget.Y = min;
            }
            else if (lookTarget.Y > max)
            {
                lookTarget.Y = max;
                goalLookTarget.Y = max;
            }
        }

        public void setLookTargetZBound(float min, float max)
        {
            if (lookTarget.Z < min)
            {
                lookTarget.Z = min;
                goalLookTarget.Z = min;
            }
            else if (lookTarget.Z > max)
            {
                lookTarget.Z = max;
                goalLookTarget.Z = max;
            }
        }

        public void MoveLookTargetX(float length, float min, float max)
        {
            lookTarget.X = Bound.Set(lookTarget.X + length, min, max);
            goalLookTarget.X = lookTarget.X;
        }
        public void MoveLookTargetZ(float length, float min, float max)
        {
            lookTarget.Z = Bound.Set(lookTarget.Z + length, min, max);
            goalLookTarget.Z = lookTarget.Z;
        }
        

        public Vector3 GoalLookTarget
        {
            get { return goalLookTarget; }
            set { 
                goalLookTarget = value; 
            }
        }
        public Vector2 Tilt
        {
            get { return tilt; }
            set { tilt = value; lib.AngleInside2Pi(ref tilt.X); lib.AngleInside2Pi(ref tilt.Y); }
        }
        virtual public float TiltX
        {
            get { return tilt.X; }
            set { tilt.X = value; lib.AngleInside2Pi(ref tilt.X); }
        }
        public float TiltY
        {
            get { return tilt.Y; }
            set { tilt.Y = Bound.Set(value, TiltYBounds); }
        }
        public float FieldOfView
        {
            get { return angle; }
            set { angle = Bound.Set(value, ActualFOVBounds.Min, ActualFOVBounds.Max); }
        }
        //public float targetZoom
        //{
        //    get { return targetZoom; }
        //    set
        //    {
        //        targetZoom = value;
        //    }
        //}
        abstract public CameraType CamType { get; }
        protected override bool drawable
        {
            get { return false; }
        }
        public Vector3 Forward { get { Vector3 dir = lookTarget - Position; dir.Normalize(); return dir; } }
        //public override Vector3 Position
        //{
        //    get
        //    {
        //        return base.Position + positionOffset;
        //    }
        //    set
        //    {
        //        base.Position = value - positionOffset;
        //    }
        //}
        public float CurrentZoom { get { return currentZoom; } set { currentZoom = targetZoom = value; } }

        /* Fields */
        public BoundingFrustum Frustum = new BoundingFrustum(Matrix.Identity);
        public Matrix Projection;
        public Matrix ViewMatrix;
        public Matrix ViewProjection;
        public float camShakeValue = 0;
        public float aspectRatio;
        public float NearPlane = 0.3f, FarPlane = 5000f;
        public RotationQuarterion BillBoard2DRotation;
        //public Vector3 positionOffset;

        protected Vector2 tilt = Vector2.Zero;
        public Vector3 goalLookTarget = Vector3.Zero;
        protected Vector3 lookTarget = new Vector3(100, 0, 100);
        public float zoomChaseLengthPercentage = 0.1f;
        public float positionChaseLengthPercentage = VikingEngine.LootFest.Players.Player.HeroCamChaseSpeed;
        public float maxChaseLength = 64f;

        protected float angle = StandardFOV;
        public float targetZoom = 100;
        protected float currentZoom;

        public int camIndex;

        /* Constructors */
        public AbsCamera(float zoom, Vector2 tilt, float screenW, float screenH)
            : base()
        {
            camIndex = nextCamIndex++;
            this.targetZoom  = zoom;
            currentZoom = zoom;
            this.tilt = tilt;
            setAspectRatio(screenW, screenH);

            positionFromRotation();
            RecalculateMatrices();
        }

        //virtual public void CopyPositionFrom(AbsCamera camera)
        //{ 
        //    position= camera.position;
        //    currentZoom = camera.currentZoom;
        //}

        public void setAspectRatio(float screenW, float screenH)
        {
            aspectRatio = screenW / screenH;
        }

        /* Methods */
        public Vector2 From3DToScreenPos(Vector3 objectPos, Viewport view)
        {
            Vector3 pos = view.Project(
                Vector3.Zero, Projection,
                ViewMatrix, Matrix.CreateTranslation(objectPos));
            Vector2 ret = Vector2.Zero;
            ret.X = pos.X;
            ret.Y = pos.Y;
            return ret;
        }

        public Vector3 ScreenPosTo3D(Vector2 screenPos, out bool hasValue)
        {
            return ScreenPosTo3D(screenPos, Engine.Draw.graphicsDeviceManager.GraphicsDevice.Viewport, out hasValue);
        }

        public Ray CastRay(Vector2 globalScreenPos, Viewport view)
        {
            //Vector2 localScreenPos = globalScreenPos - new Vector2(view.X, view.Y);
            Vector3 near = new Vector3(globalScreenPos, 0);
            Vector3 far = new Vector3(globalScreenPos, 1);

            near = view.Unproject(near, Projection, ViewMatrix, Matrix.Identity);
            far = view.Unproject(far, Projection, ViewMatrix, Matrix.Identity);

            Vector3 dir = far - near;
            dir.Normalize();

            return new Ray(Position, dir);
        }

        public Vector3 ScreenPosTo3D(Vector2 screenPos, Viewport view, out bool hasValue)
        {
            return CastRayInto3DPlane(screenPos, view, new Plane(Vector3.UnitY, 0), out hasValue);
        }

        public Vector3 CastRayInto3DPlane(Vector2 screenPos, Viewport view, Plane plane, out bool hasValue)
        {
            Ray ray = CastRay(screenPos, view);
            float? distance = ray.Intersects(plane);

            return CastRayInto3DPlane(ray, plane, out hasValue);
        }

        public Vector3 CastRayInto3DPlane(Ray ray, Plane plane, out bool hasValue)
        {
            float? distance = ray.Intersects(plane);

            hasValue = distance.HasValue;
            if (distance.HasValue)
            {
                return ray.Position + ray.Direction * distance.Value;
            }

            return new Vector3(-1);
        }

        virtual public void instantMoveToTarget()
        {
            currentZoom = targetZoom;
        }

        virtual public void RecalculateMatrices()
        {
            Vector3 pos = Position;// + positionOffset;
            Vector3 target = LookTarget;// + positionOffset;
            float near = NearPlane;
            float far = FarPlane;
            //if (Ref.draw is DeferredRenderer)
            //{
            //    pos *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            //    target *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            //    near *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            //    far *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            //}
            if (camShakeValue > 0)
            {
                float addSideLength = Bound.MaxAbs((float)Math.Sin(camShakeValue * 2f) * camShakeValue * 0.004f, 0.6f);
                Vector2 dir = lib.AngleToV2(tilt.X, addSideLength);
                pos.X += dir.X;
                pos.Z += dir.Y;

                target.X += dir.X;
                target.Z += dir.Y;
            }

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(angle), aspectRatio, near, far);
            ViewMatrix = Matrix.CreateLookAt(pos, target, Vector3.Up);
            ViewProjection = ViewMatrix * Projection;
            Frustum.Matrix = ViewProjection;
        }

        public Vector3 lookDir()
        {
            return LookTarget - Position;
        }

        public void updateBillboard()
        {
            BillBoard2DRotation.RotationV3 = new Vector3(MathHelper.PiOver2 - TiltX, MathHelper.PiOver2, 0);
        }
        virtual public void SetPersonVisible(bool visible)
        { }

        public override string ToString()
        {
            return "Camera (" + camIndex.ToString() + "), type: " + CamType.ToString();
        }
        public string Info()
        {
            return "Pos(" + lib.Vec3Text(Position) + ") Tilt(" +
               lib.Vec2Text(Tilt) + ") Target(" + lib.Vec3Text(lookTarget) + ")";
        }
        virtual public void Time_Update(float time_ms)
        {
            camShakeValue -= time_ms;
        }

        virtual public void positionFromRotation()
        { }
        virtual public Vector2 MoveCamera(Vector2 leftPad) { return Vector2.Zero; }
        virtual public void RotateCamera(Vector2 dir)
        { }
    }
}
