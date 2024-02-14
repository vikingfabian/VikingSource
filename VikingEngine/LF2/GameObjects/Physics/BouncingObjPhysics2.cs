using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
    class BouncingObjPhysics2 : AbsPhysics
    {
        int numBounces = 0;
        public int MaxBounces = 2;
        public float Bounciness = 0.5f;
        bool sleeping = false;
        public const float BallGravity = -0.001f;
        public BouncingObjPhysics2(GameObjects.AbsUpdateObj parent)
            : base(parent)
        {
            Gravity = BallGravity;
            obsticles = new LF2.ObsticleBounds(2, 3);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!sleeping)
            {
                parent.Velocity.Y += Gravity;
                Vector3 pos = parent.Position;
                Vector3 oldPos = pos;
                
                pos += parent.Velocity.Value * time;

                CollectObsticles();
                Physics.Collision3D coll = ObsticleCollision();

                
                if (coll != null)
                {
                    Vector3 collDir = parent.CollisionBound.IntersectionDepthAndDir(coll);
                    
                    if (Math.Abs(collDir.X) > Math.Abs(collDir.Z))
                    {
                        if (!lib.SameDir(parent.Velocity.PlaneX, collDir.X))
                            parent.Velocity.PlaneX *= -Bounciness;
                    }
                    else
                    {
                        if (!lib.SameDir(parent.Velocity.PlaneY, collDir.Z))
                            parent.Velocity.PlaneY *= -Bounciness;
                    }
                    
                    parent.Position -= collDir;
                }

                float groundY = GetGroundY().BasicY;
                if (pos.Y <= groundY)
                {
                    if (Math.Abs(groundY - pos.Y) > 2)
                    {
                        parent.Velocity.Y = 0;
                        sleeping = true;
                        return;//do move into the ground
                    }
                    else
                    {
                        pos.Y = groundY;
                        if (parent.Velocity.Y < -0.01f && numBounces < MaxBounces)
                        {
                            parent.Velocity.Y = -parent.Velocity.Y * Bounciness;
                            ++numBounces;
                        }
                        else
                        {
                            parent.Velocity.Y = 0;
                            sleeping = true;
                        }
                    }
                }


                parent.Position = pos;
            }
        }

        public override GroundWithSlopesData GetGroundY()
        {
            Vector3 pos = parent.Position;
            pos.Y += 1f;
            Map.WorldPosition wp = new Map.WorldPosition(pos);

            return new GroundWithSlopesData(LfRef.chunks.GetScreen(wp).GetGroundY(wp));
        }

        public override void CollectObsticles()
        {
            obsticles.Collect(parent.WorldPosition);
        }

        public override void WakeUp()
        {
            sleeping = false;
            numBounces = 0;
        }
        public override bool Sleeping
        {
            get
            {
                return sleeping;
            }
        }
    }
}
