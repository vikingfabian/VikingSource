using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    class BouncingObjPhysics2 : AbsPhysics
    {
        public int numBounces = 0;
        public int maxBounces = 2;
        public float Bounciness = 0.5f;
        bool sleeping = false;
        public const float BallGravity = -0.001f;
        public BouncingObjPhysics2(GO.AbsUpdateObj parent)
            : base(parent)
        {
            Gravity = BallGravity;
            obsticles = new LootFest.ObsticleBounds(2, 3);
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!sleeping)
            {
                if (Ref.TimePassed16ms)
                { parent.Velocity.Y += Gravity; }
                Vector3 pos = parent.Position;
                Vector3 oldPos = pos;
                
                pos += parent.Velocity.Value * time;

                CollectObsticles();
                var coll = ObsticleCollision();

                
                if (coll != null)
                {
                    Vector3 collDir = parent.CollisionAndDefaultBound.IntersectionDepthAndDir(coll);
                    
                    if (Math.Abs(collDir.X) > Math.Abs(collDir.Z))
                    {
                        if (!lib.SameDirection(parent.Velocity.PlaneX, collDir.X))
                            parent.Velocity.PlaneX *= -Bounciness;
                    }
                    else
                    {
                        if (!lib.SameDirection(parent.Velocity.PlaneY, collDir.Z))
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

                        if (parent.Velocity.Y < 0)
                        {
                            parent.onGroundPounce(parent.Velocity.Y);
                        }

                        if (parent.Velocity.Y < -0.01f && numBounces < maxBounces)
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

            return new GroundWithSlopesData(LfRef.chunks.GetScreen(wp).GetClosestFreeY(wp));
        }

        public override void CollectObsticles()
        {
            obsticles.Collect(parent.WorldPos);
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
        public override int MaxBounces
        {
            set
            {
                maxBounces = value;
            }
        }
    }
}
