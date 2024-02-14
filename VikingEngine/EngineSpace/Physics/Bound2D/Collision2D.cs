using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Physics
{
    struct Collision2D
    {
        public static readonly Collision2D NoCollision = new Collision2D();

        public bool IsCollision;
        public Vector2 direction;
        public Vector2 surfaceNormal;
        public Vector2 intersectionSize;
        public float depth;

        public Collision2D(Vector2 surfaceNormal, float depth)
        {
            IsCollision = true;
            direction = Vector2.Zero;
            this.surfaceNormal = surfaceNormal;
            intersectionSize = Vector2.Zero;
            this.depth = depth;
        }

        public Collision2D Invert()
        {
            Collision2D inv = this;
            inv.direction = -direction;
            inv.surfaceNormal = -surfaceNormal;
            return inv;
        }

        public Vector2 reflectAgainstNormal(Vector2 velocity, float elastisy = 1f)
        {
            Rotation1D speedDir = Rotation1D.FromDirection(-velocity);
            Rotation1D normalDir = Rotation1D.FromDirection(surfaceNormal);

            float inAngle = normalDir.AngleDifference(speedDir);
            normalDir.Add(-inAngle);

            velocity = normalDir.Direction(velocity.Length() * elastisy);

            return velocity;
        }

        public Vector2 IntersectVector => direction * depth;
    }
    public class IntersectDetails2D
    {
        public float Depth;
        public Vector2 IntersectionCenter;

        public IntersectDetails2D()
        { }
        public IntersectDetails2D(float depth, Vector2 vertice)
        {
            this.Depth = depth;
            this.IntersectionCenter = vertice;
        }
    }
}
