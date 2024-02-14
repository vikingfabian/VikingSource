using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.LF2
{

    //interface IForce : IDeleteable
    //{
    //    void CollisionEvent();
    //}
    static class PhysicsLib
    {
        public const float ProjectileGravity = -0.002f;

        public static Vector3 FireAngle(Vector3 start, Vector3 target, float velocity)
        {
            Vector3 positionDiff = target - start;
            Vector2 positionDiff2DFromSide = new Vector2(new Vector2(positionDiff.X, positionDiff.Z).Length(), positionDiff.Y);

            Rotation1D sideAngle = Rotation1D.FromDirection(positionDiff2DFromSide); //90degree = straight forward
          
            float length = positionDiff.Length();
            const float AddAngleToLengthAdj = 800;
            float maxLength = AddAngleToLengthAdj * velocity;

           //maxlength is 45 degrees
           length = lib.SetMaxFloatVal(length, maxLength);
           sideAngle.Add(MathHelper.PiOver4 * length / maxLength);
           Vector2 sideDir = sideAngle.Direction(velocity);
           //get direction seen from above
           positionDiff.Y = 0;
           positionDiff = lib.SafeNormalizeV3( positionDiff);

           positionDiff.Y = sideDir.Y;
           positionDiff.X *= sideDir.X;
           positionDiff.Z *= sideDir.X;

#if WINDOWS
           if (float.NaN.Equals(positionDiff.Y) || float.NaN.Equals(positionDiff.X))
           {
               throw new NullReferenceException();
           }
#endif
           return positionDiff;
        }
    }
    class ObjBoundCollData
    {
        public Collision3D Intersection;
        //public float Damage;

        public ObjBoundCollData(Collision3D Intersection) //, float Damage)
        {
            this.Intersection = Intersection;
            //this.Damage = Damage;
        }
    }
    
}
