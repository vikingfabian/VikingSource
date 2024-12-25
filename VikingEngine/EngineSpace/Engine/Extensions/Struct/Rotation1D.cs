using System;
using Microsoft.Xna.Framework;

namespace VikingEngine
{   
    public struct Rotation1D
    {        
        public float radians;
        public float Radians
        {
            get { return radians; }
            set
            {
                radians = value;
                checkBounds();
            }
        }
        public float Degrees
        {
            get { return MathHelper.ToDegrees(radians); }
            set
            {
                Radians = MathHelper.ToRadians(value);
            }
        }
        public Rotation1D(float radians)
        {
            this.radians = radians;
            checkBounds();
        }

        public Rotation1D(double radians)
        {
            this.radians = (float)radians;
            checkBounds();
        }

        public Rotation1D(byte byteDir)
        {
            radians = RadiansPerByte * byteDir;
            checkBounds();
        }
        public Rotation1D(Dir8 dir)
        {
            radians = (int)dir * MathHelper.PiOver4;
        }

        public Rotation1D Add(Rotation1D add)
        {
            radians += add.radians;
            checkBounds();
            return this;
        }

        public void AddDegrees(float add)
        {
            radians += MathHelper.ToRadians(add);
            checkBounds();
        }

        const float RadiansPerByte = MathHelper.TwoPi / byte.MaxValue;
        const float BytePerRadian = byte.MaxValue / MathHelper.TwoPi;
        public static byte RadiansToByte(float radians)
        {
            return (byte)(radians * BytePerRadian);
        }
        public static float ByteToRadians(byte value)
        {
            return RadiansPerByte * value;
        }

        public byte ByteDir
        {
            get { return (byte)(radians * BytePerRadian); }
            set { radians = RadiansPerByte * value; }
        }
        public void AddByte(byte add)
        {
            radians += add * RadiansPerByte;
            checkBounds();
        }
        public Rotation1D Add(float add)
        {
            radians += add;
            checkBounds();
            return this;
        }
        public void invert()
        {
            radians += MathHelper.Pi;
            checkBounds();
        }

        public Rotation1D getInvert()
        {
            Rotation1D inv = this;
            inv.invert();
            return inv;
        } 
        //public static Rotation1D operator =(float value)
        //{
        //    radians = value;
        //    checkBounds();
        //}
        //public implicit operator float()
        //{
        //    #if 
        //}

        public static implicit operator Rotation1D(float value)
        {
            return new Rotation1D(value);
        }
        public static implicit operator Rotation1D(Dir4 value)
        {
            return new Rotation1D((float)value * MathHelper.PiOver2);
        }

        public static bool operator ==(Rotation1D value1, Rotation1D value2)
        {
            return value1.radians == value2.radians;
        }
        public static bool operator !=(Rotation1D value1, Rotation1D value2)
        {
            return value1.radians != value2.radians;
        }
        public override bool Equals(object obj)
        {
            Rotation1D other = (Rotation1D)obj;
            return this.radians == other.radians;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static Rotation1D operator +(Rotation1D value1, float value2)
        {
            value1.radians += value2;
            value1.checkBounds();
            return value1;
        }
        public static Rotation1D operator +(Rotation1D value1, Rotation1D value2)
        {
            value1.radians += value2.radians;
            value1.checkBounds();
            return value1;
        }
        public static Rotation1D operator -(Rotation1D value1, float value2)
        {
            value1.radians -= value2;
            value1.checkBounds();
            return value1;
        }
        public static Rotation1D operator -(Rotation1D value1, Rotation1D value2)
        {
            value1.radians -= value2.radians;
            value1.checkBounds();
            return value1;
        }
        public void checkBounds()
        {
            if (radians < 0) radians += MathHelper.TwoPi;
            else if (radians >= MathHelper.TwoPi) radians -= MathHelper.TwoPi;
        }
        public static float CheckBounds(float radians)
        {
            if (radians < 0) radians += MathHelper.TwoPi;
            else if (radians >= MathHelper.TwoPi) radians -= MathHelper.TwoPi;
            return radians;
        }
        public Vector2 Direction(float length)
        {
            return lib.AngleToV2(radians, length);
        }
        /// <summary>
        /// The difference can be hard to tell be course they start meeting after 180degrees
        /// </summary>
        public float AngleDifference(float otherAngle)
        {
            return this.AngleDifference(new Rotation1D(otherAngle));
        }
        public float AngleDifference(Rotation1D otherAngle)
        {
            return AngleDifference(radians, otherAngle.radians);
        }

        /// <summary>
        /// Calculates the angular difference between two angles, returning a value in the range [-π, π].
        /// </summary>
        /// <param name="angle1">The first angle in radians.</param>
        /// <param name="angle2">The second angle in radians.</param>
        /// <returns>The signed angular difference in radians, within the range [-π, π].</returns>
        public static float AngleDifference(float angle1, float angle2)
        {
            // Calculate the raw angular difference.
            float diff = angle2 - angle1;

            // Normalize the difference to the range [-2π, 2π].
            //diff = diff % MathHelper.TwoPi;

            // Adjust to ensure the difference is within [-π, π].
            if (diff > MathHelper.Pi)
            {
                diff -= MathHelper.TwoPi;
            }
            else if (diff < -MathHelper.Pi)
            {
                diff += MathHelper.TwoPi;
            }

            return diff;
        }


        public static float AngleDifference_Absolute(float angle1, float angle2)
        {
            //float diff = angle2 - angle1;
            //if (diff > MathHelper.Pi) //the difference can't be higher than 180degrees
            //{
            //    diff = Math.Abs(diff - MathHelper.TwoPi);
            //}
            //else if (diff < -MathHelper.Pi) //the difference can't be higher than 180degrees
            //{
            //    diff = diff + MathHelper.TwoPi;
            //}

            return Math.Abs(AngleDifference(angle1, angle2));
        }
        public static float MidAngle(float angle1, float angle2)
        {
            return CheckBounds(angle1 + AngleDifference(angle1, angle2) * PublicConstants.Half);
        }

        public void RotateTowardsAngle(float movePercent, float goalAngle, float minAngleDiff)
        {
            float diff = AngleDifference(goalAngle);
            if (Math.Abs(diff) <= minAngleDiff)
            {
                this.radians = goalAngle;
                checkBounds();
            }
            else
            {
                radians += movePercent * diff;
                checkBounds();
            }
        }

        public static Rotation1D FromDir4(Dir4 dir)
        {
            switch(dir)
            {
                case Dir4.E:
                    return D270;
                case Dir4.N:
                    return D180;
                case Dir4.S:
                    return D0;
                case Dir4.W:
                    return D90;
                default:
                    throw new ArgumentException();
            }
        }

        public static Rotation1D FromDirection(Vector2 direction)
        { return new Rotation1D(lib.V2ToAngle(direction)); }

        public static Rotation1D FromDegrees(float degrees)
        { return new Rotation1D(MathHelper.ToRadians(degrees)); }

        public static readonly Rotation1D D0 = new Rotation1D(0f);
        public static readonly Rotation1D D45 = new Rotation1D(MathHelper.PiOver4);
        public static readonly Rotation1D D90 = new Rotation1D(MathHelper.PiOver2);
        public static readonly Rotation1D D135 = new Rotation1D(MathHelper.PiOver2 + MathHelper.PiOver4);
        public static readonly Rotation1D D180 = new Rotation1D(MathHelper.Pi);
        public static readonly Rotation1D D270 = new Rotation1D(MathHelper.Pi + MathHelper.PiOver2);
        public static Rotation1D Random()
        { return new Rotation1D((float)(Math.PI * 2.0 * Ref.rnd.Double())); }
        public static Rotation1D Random(PcgRandom rnd)
        { return new Rotation1D((float)(Math.PI * 2.0 * rnd.Double())); }

        public void flipX()
        {
            radians = MathExt.FlipAngleX(radians);
            //Vector2 vec = lib.AngleToV2(radians, 1f);
            //vec.X *= -1;
            //radians = lib.V2ToAngle(vec);
        }
        public void flipY()
        {
            radians = MathExt.FlipAngleY(radians);
            //Vector2 vec = lib.AngleToV2(radians, 1f);
            //vec.Y *= -1;
            //radians = lib.V2ToAngle(vec);
        }

        public void flip180()
        {   
            if (radians < MathHelper.Pi)
            {
                radians += MathHelper.Pi;
            }
            else
            {
                radians -= MathHelper.Pi;
            }
        }

        public void setVectorXDir(int dir)
        {
            Vector2 vec = lib.AngleToV2(radians, 1f);
            vec.X = dir * Math.Abs(vec.X);
            radians = lib.V2ToAngle(vec);
        }
        public void setVectorYDir(int dir)
        {
            Vector2 vec = lib.AngleToV2(radians, 1f);
            vec.Y = dir * Math.Abs(vec.Y);
            radians = lib.V2ToAngle(vec);
        }

        public override string ToString()
        {
            return Degrees.ToString() + "Degrees";
        }


    }

}
