using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    interface IFirstPerson
    {
        Vector3 fpsPosition { get; }
        void setVisibleForFpsCam(bool visible);
    }

    class FirstPersonCamera : AbsCamera
    {
        /* Constants */
        public const float FPStandardFOV =
#if XBOX
            70;
#else
 80;
#endif

        public float PlacementAbove = 1.8f;
        public float PlacementBehind = 0.5f;
        public float PlacementTargetY = -3;

        private const float FocusLength = 20;
        private const float TiltXAjd = MathHelper.Pi + MathHelper.PiOver2;

        /* Properties */
        public override float TiltX
        {
            get { return -cameraRotation.Xradians + TiltXAjd; }
            set { cameraRotation.Xradians = TiltXAjd - value; }
        }
        public override CameraType CamType
        {
            get { return CameraType.FirstPerson; }
        }

        /* Fields */
        public IFirstPerson Person;
        public bool TargetOriented = false;

        private Rotation3D cameraRotation = Rotation3D.Zero;

        /* Constructors */
        public FirstPersonCamera(float zoom, Vector2 tilt, float screenW, float screenH, IFirstPerson person)
            : base(zoom, tilt, screenW, screenH)
        {
            this.Person = person;
            angle = FPStandardFOV;
        }

        /* Methods */
        /// <summary>
        /// Normally with right analog stick
        /// </summary>
        override public void RotateCamera(Vector2 dir)
        {
            const float MinY = -1.35f;
            const float MaxY = 1.26f;
            cameraRotation.Xradians += -dir.X;
            cameraRotation.Yradians = Bound.Set(cameraRotation.Yradians + dir.Y, MinY, MaxY);
        }

        override public Vector2 MoveCamera(Vector2 leftPad)
        {
            return VectorExt.RotateVector(-leftPad, MathHelper.TwoPi - cameraRotation.Xradians);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (Person != null)
            {
                position = Person.fpsPosition;
                position.Y += PlacementAbove;
                if (!TargetOriented || lookTarget == Position)
                {
                    Rotation1D dir = new Rotation1D(MathHelper.TwoPi - cameraRotation.Xradians);
                    Vector2 behind = dir.Direction(PlacementBehind);
                    position.X += behind.X;
                    position.Z += behind.Y;

                    lookTarget = Position + Vector3.Transform(new Vector3(0, 0, FocusLength), cameraRotation.RotationMatrix());
                    lookTarget.Y += PlacementTargetY;
                }
                if (TargetOriented)
                {
                    Vector3 diff = goalLookTarget - lookTarget;
                    diff *= positionChaseLengthPercentage;
                    lookTarget += diff;
                }

                if (float.NaN.Equals(lookTarget.Z))
                {
                    throw new Exception();
                }
            }
        }
        public override void SetPersonVisible(bool visible)
        {
            if (Person != null)
                Person.setVisibleForFpsCam(visible);
        }
        public override string ToString()
        {
            return "FPS cam";
        }
    }
}
