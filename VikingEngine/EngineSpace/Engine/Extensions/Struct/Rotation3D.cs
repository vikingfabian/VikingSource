using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine
{
    struct Rotation3D
    {
        public static Rotation3D Zero
        { get { return new Rotation3D(0, 0, 0); } }
        public const int DIMENTIONS = 3;
        //float[] radians;
        float X;
        float Y;
        float Z;

        public Rotation3D(float x, float y, float z)
        {
            X = z; Y = y; Z = z;
        }
        public Rotation3D(Vector3 rotation)
        {
            X = rotation.X; Y = rotation.Y; Z = rotation.Z;
        }

        public void AddTimeVector(Vector3 add, float multi)
        {
            X += add.X * multi;
            Y += add.Y * multi;
            Z += add.Z * multi;

        }

        public override string ToString()
        {
            return "{" + X + "," +
                Y + "," +
                Z + "}";
        }

        public Vector3 Radians
        {
            get
            {
                Vector3 ret = Vector3.Zero;
                ret.X = X;
                ret.Y = Y;
                ret.Z = Z;
                return ret;
            }
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }
        public Vector3 Degrees
        {
            get
            {
                Vector3 ret = Vector3.Zero;
                ret.X = MathHelper.ToDegrees(X);
                ret.Y = MathHelper.ToDegrees(Y);
                ret.Z = MathHelper.ToDegrees(Z);
                return ret;
            }
            set
            {
                X = MathHelper.ToRadians(value.X);
                Y = MathHelper.ToRadians(value.Y);
                Z = MathHelper.ToRadians(value.Z);
            }
        }

        public float Xradians
        {
            get { return X; }
            set { X = setBounds(value); }
        }
        public float Yradians
        {
            get { return Y; }
            set { Y = setBounds(value); }
        }
        public float Zradians
        {
            get { return Z; }
            set { Z = setBounds(value); }
        }


        public float Xdegrees
        {
            get { return MathHelper.ToDegrees(X); }
            set { X = setBounds(MathHelper.ToRadians(value)); }
        }
        public float Ydegrees
        {
            get { return MathHelper.ToDegrees(Y); }
            set { Y = setBounds(MathHelper.ToRadians(value)); }
        }
        public float Zdegrees
        {
            get { return MathHelper.ToDegrees(Z); }
            set { Z = setBounds(MathHelper.ToRadians(value)); }
        }

        public Quaternion QuadRotation
        {
            get
            {
                return Quaternion.CreateFromYawPitchRoll(
                    X,
                    Y,
                    Z);
            }
            set { Radians = lib.QuaternionToEuler(value); }
        }

        float setBounds(float val)
        {
            // return Bound.SetBoundsRollover(val, -MathHelper.TwoPi, MathHelper.TwoPi);
            if (val > MathHelper.TwoPi)
                val -= MathHelper.TwoPi;
            else if (val < -MathHelper.TwoPi)
                val += MathHelper.TwoPi;
            return val;
        }
        //void checkBounds(Dimentions dimention)
        //{
        //    int i = (int)dimention;
        //    if (radians[i] < 0) radians[i] += MathHelper.TwoPi;
        //    else if (radians[i] >= MathHelper.TwoPi) radians[i] -= MathHelper.TwoPi;


        //}
        public Matrix RotationMatrix()
        {
            return Matrix.CreateFromYawPitchRoll(X,
                Y, Z);
        }

        public ByteVector3 LowRes
        {
            get
            {
                return new ByteVector3(
              Rotation1D.RadiansToByte(X),
              Rotation1D.RadiansToByte(Y),
              Rotation1D.RadiansToByte(Z));
            }
            set
            {
                X = Rotation1D.ByteToRadians(value.X);
                Y = Rotation1D.ByteToRadians(value.Y);
                Z = Rotation1D.ByteToRadians(value.Z);
            }
        }
    }
}
