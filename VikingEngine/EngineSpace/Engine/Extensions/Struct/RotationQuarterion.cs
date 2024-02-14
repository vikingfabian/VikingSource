using Microsoft.Xna.Framework;
using System;

namespace VikingEngine
{
    public struct RotationQuarterion
    {
        public static readonly RotationQuarterion Identity = new RotationQuarterion(Quaternion.Identity);
        public static RotationQuarterion Random
        {
            get
            {
                return new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(Ref.rnd.Rotation(), Ref.rnd.Rotation(), Ref.rnd.Rotation()));
            }
        }
        Quaternion quadRotation;
        public Quaternion QuadRotation
        {
            get { return quadRotation; }
            set
            {
                quadRotation = value;
                NewValueEvent();
            }
        }
        IRotationCallBack callBackObj;
        public IRotationCallBack CallBackObj { set { callBackObj = value; } }

        //Translate the vector along the axis
        public Vector3 TranslateAlongAxis(Vector3 translation, Vector3 startPosition)
        {
            Matrix calcPos = Matrix.CreateFromQuaternion(quadRotation);
            startPosition.X +=
                calcPos.Left.X * translation.X +
                calcPos.Up.X * translation.Y +
                calcPos.Backward.X * translation.Z;
            startPosition.Y +=
                calcPos.Left.Y * translation.X +
                calcPos.Up.Y * translation.Y +
                calcPos.Backward.Y * translation.Z;
            startPosition.Z +=
                calcPos.Left.Z * translation.X +
                calcPos.Up.Z * translation.Y +
                calcPos.Backward.Z * translation.Z;
            return startPosition;

        }
        public void FromDirection(Vector3 dir)
        {
            quadRotation = Quaternion.CreateFromYawPitchRoll(dir.X, dir.Y, dir.Z);
        }
        public RotationQuarterion(Quaternion rotation)
        {
            quadRotation = rotation;
            callBackObj = null;
        }

        public void WriteStream(System.IO.BinaryWriter w)
        {
            w.Write(quadRotation.X);
            w.Write(quadRotation.Y);
            w.Write(quadRotation.Z);
            w.Write(quadRotation.W);

        }

        public void ReadStream(System.IO.BinaryReader r)
        {
            quadRotation.X = r.ReadSingle();
            quadRotation.Y = r.ReadSingle();
            quadRotation.Z = r.ReadSingle();
            quadRotation.W = r.ReadSingle();

        }

        public void RotateWorldX(float rot) //Will rotate the object around the world coordinates
        {
            quadRotation = Quaternion.Concatenate(quadRotation, Quaternion.CreateFromYawPitchRoll(rot, 0, 0));
            quadRotation.Normalize();
            NewValueEvent();
        }
        public void RotateWorldY(float rot) //Will rotate the object around the world coordinates
        {
            quadRotation = Quaternion.Concatenate(quadRotation, Quaternion.CreateFromYawPitchRoll(0, rot, 0));
            quadRotation.Normalize();
            NewValueEvent();
        }
        public void RotateWorldZ(float rot) //Will rotate the object around the world coordinates
        {
            quadRotation = Quaternion.Concatenate(quadRotation, Quaternion.CreateFromYawPitchRoll(0, 0, rot));
            quadRotation.Normalize();
            NewValueEvent();
        }

        public void RotateWorld(Vector3 rot) //Will rotate the object around the world coordinates
        {
            quadRotation = Quaternion.Concatenate(quadRotation, Quaternion.CreateFromYawPitchRoll(rot.X, rot.Y, rot.Z));
            quadRotation.Normalize();
            NewValueEvent();
        }
        public void RotateAxis(Vector3 rot) //Will rotate the object around its own coordinates
        {
            quadRotation *= Quaternion.CreateFromYawPitchRoll(rot.X, rot.Y, rot.Z);
            quadRotation.Normalize();
            NewValueEvent();
        }

        public static RotationQuarterion FromWorldRotation(Vector3 rot)
        {
            return new RotationQuarterion(Quaternion.CreateFromYawPitchRoll(rot.X, rot.Y, rot.Z));
        }

        /// <summary>
        /// Y is expected to be up
        /// </summary>
        public void PlaneRotation(float rotation)
        {
            QuadRotation = Quaternion.CreateFromYawPitchRoll(MathHelper.TwoPi - rotation, 0, 0);
        }

        public Vector3 RotationV3
        {
            get { return lib.QuaternionToEuler(quadRotation); }
            set
            {
                quadRotation = Quaternion.CreateFromYawPitchRoll(value.X, value.Y, value.Z);
                quadRotation.Normalize();
                NewValueEvent();
            }

        }
        public float Xradians
        {
            get { return RotationV3.X; }
            set { Vector3 rot = RotationV3; rot.X = value; RotationV3 = rot; }
        }
        public float Yradians
        {
            get { return RotationV3.Y; }
            set { Vector3 rot = RotationV3; rot.Y = value; RotationV3 = rot; }
        }
        public float Zradians
        {
            get { return RotationV3.Z; }
            set { Vector3 rot = RotationV3; rot.Z = value; RotationV3 = rot; }
        }

        public float Xdegrees
        {
            get { return MathHelper.ToDegrees(Xradians); }
            set { Xradians = MathHelper.ToRadians(value); }
        }
        public float Ydegrees
        {
            get { return MathHelper.ToDegrees(Yradians); }
            set { Yradians = MathHelper.ToRadians(value); }
        }
        public float Zdegrees
        {
            get { return MathHelper.ToDegrees(Zradians); }
            set { Zradians = MathHelper.ToRadians(value); }
        }

        void NewValueEvent()
        {
            if (callBackObj != null)
            {
                callBackObj.RotationCallBack();
            }
        }

        public void PointAlongVector(Vector3 u)
        {
            float xzLen = (float)(Math.Sqrt(Math.Pow(u.X, 2) + Math.Pow(u.Z, 2)));
            RotationV3 = new Vector3(-(float)Math.Atan2(u.X, -u.Z), (float)Math.Atan2(u.Y, xzLen) + MathHelper.Pi, 0);
        }
    }
}
