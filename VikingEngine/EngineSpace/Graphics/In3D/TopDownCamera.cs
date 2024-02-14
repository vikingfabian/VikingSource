using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class TopDownCamera: AbsCamera
    {
        float currentChaseLength = 0;
        Vector3 upVector = -Vector3.UnitZ;

        public TopDownCamera(float zoom)
            : this(zoom, Engine.Screen.Width, Engine.Screen.Height)
        { }

        public TopDownCamera(float zoom, float screenW, float screenH)
            : base(zoom, Vector2.Zero, screenW, screenH)
        {

        }



        public override void Time_Update(float time_ms)
        {
            base.Time_Update(time_ms);

            //Chase goal
            Vector3 diff = goalLookTarget - lookTarget;
            float l = diff.Length();

            const float BASE_CHASE_SPEED = 0.05f;
            if (l > BASE_CHASE_SPEED)
            {
                if (Ref.TimePassed16ms)
                {
                    currentChaseLength = l * positionChaseLengthPercentage;

                    //if (positionOffset.LengthSquared() > 0.0001f)
                    //{
                    //    // Chase faster while using position offsetting, such as in menu
                    //    currentChaseLength *= 5;
                    //}

                    currentChaseLength = Math.Min(currentChaseLength, maxChaseLength);

                    currentChaseLength += BASE_CHASE_SPEED; // base chase speed
                }

                diff.Normalize();
                diff *= currentChaseLength;
                this.lookTarget += diff;
            }
            else
            {
                this.lookTarget = goalLookTarget;
            }

            if (float.NaN.Equals(lookTarget.Z))
            {
                throw new Exception();
            }
        }

        public override void RecalculateMatrices()
        {
            //base.RecalculateMatrices();

            Vector3 pos = LookTarget;
            pos.Y += targetZoom;

            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(angle), aspectRatio, NearPlane, FarPlane);
            ViewMatrix = Matrix.CreateLookAt(pos, LookTarget, upVector);
            ViewProjection = ViewMatrix * Projection;
            Frustum.Matrix = ViewProjection;
        }

        public override CameraType CamType
        {
            get { return CameraType.TopDown; }
        }
    }
}
