using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2
{
    abstract class AbsPhysics
    {
        protected const float CheckMaxHeight = Map.WorldPosition.ChunkHeight - 4;
        protected GameObjects.AbsUpdateObj parent;

        public const float StandardGravity = -0.0007f;
        public float Gravity = StandardGravity;
        virtual public float SpeedY { get { return parent.Velocity.Y; } set { parent.Velocity.Y = value; } }
        

        protected LF2.ObsticleBounds obsticles;

        public AbsPhysics(GameObjects.AbsUpdateObj parent)
        {
            this.parent = parent;
        }
        
        virtual public void PushForce(Vector2 force)
        {
            parent.PlanePos += force;
        }

        virtual public bool PhysicsStatusFalling
        {
            get { return false; }
        }
        virtual public void Update(float time)
        {
            if (parent.UpdateWorldPos())
            {
                CollectObsticles();
            }
        }
        virtual public void CollectObsticles()
        {
            
        }
        virtual public GroundWithSlopesData GetGroundY()
        {
            throw new NotImplementedException();
        }
        public virtual Physics.Collision3D ObsticleCollision()
        {
            if (parent.Position.Y < CheckMaxHeight)
            {
                AbsObjBound bound = parent.GetGroundInteractBound;
                bound.UpdatePosition2(parent);

                foreach (LF2.CollisionBlock block in obsticles.blocks)
                {
                    Physics.Collision3D intersect = bound.ObsticleIntersect(block.BoundingBox);
                    if (intersect != null)
                    {
                        if (PlatformSettings.ViewCollisionBounds)
                        {
                            new TerrainCollMarker(intersect.OtherBound.CenterScale);
                        }
                        return intersect;
                    }
                }
            }
            return null;
        }

        public virtual Vector3 UpdateMovement()
        {
            throw new NotImplementedException();
        }

        protected IDeleteable force;
        public void AddForce(IDeleteable force)
        {
            removeForce();
            this.force = force;
        }
        public void removeForce()
        {
            lib.SafeDelete(force);
            force = null;
        }
        

        /// <param name="force">Force is a multiplier of a standard jump height</param>
        public void Jump(float force, Graphics.AbsVoxelObj image)
        {
            image.position.Y += 0.4f * force;
            parent.Velocity.Y += 0.03f * force;
        }
       
        virtual public void UpdatePosFromParent()
        { }
        public virtual void WakeUp()
        { }
        virtual public bool Sleeping
        {
            get {  return false; }
        }

        const float MaxSlopeDistance = 1.5f;
        const float MaxSlopeDistanceInv = 1 / MaxSlopeDistance;


        public static GroundWithSlopesData groundYwithSlopes2(Vector3 pos) 
        {
            pos.Y += 1f;
            Map.WorldPosition wp = new Map.WorldPosition(pos);
            wp.SetFromGroundY(0);
            GroundWithSlopesData result = new GroundWithSlopesData(wp.WorldGrindex.Y);

            Map.WorldPosition neighbor;
            IntVector2 posDiff;
            float newSlopeVal;
            float slopeY = 0;
            int numslopes = 0;
            ++wp.WorldGrindex.Y;
            //Look on all sourround directions and see if it is sloping
            for (int dir = 0; dir < 4; ++dir) //all 4 facing dirs
            {
                neighbor = wp;
                posDiff = IntVector2.From4Dirs[dir];
                neighbor.WorldGrindex.X += posDiff.X;
                neighbor.WorldGrindex.Z += posDiff.Y;

                if (LfRef.chunks.Get(neighbor) != 0) 
                {
                    ++neighbor.WorldGrindex.Y;
                    if (LfRef.chunks.Get(neighbor) == 0)
                    {
                        ++neighbor.WorldGrindex.Y;

                        if (LfRef.chunks.GetScreen(neighbor).HasFreeSpaceUp(neighbor, 4))
                        {
                            //Has a block adjacent that is only one 1 in height
                            float percentDiff;
                            if (lib.Dir4IsXdim[dir])
                            {
                                percentDiff = neighbor.WorldGrindex.X - pos.X;
                            }
                            else
                            {
                                percentDiff = neighbor.WorldGrindex.Z - pos.Z;
                            }

                            newSlopeVal = 1 - Math.Abs(percentDiff * MaxSlopeDistanceInv);
                            if (newSlopeVal > slopeY)
                                slopeY = newSlopeVal;

                            ++numslopes;
                        }
                    }
                    
                }
            }

            if (numslopes >= 3)
            {
                slopeY = 1;
            }
            result.slopeY += slopeY;
            return result;
        }

        public static GroundWithSlopesData groundYwithSlopes(Vector3 pos)
        {
            GroundWithSlopesData result = new GroundWithSlopesData();
            pos.Y += 0.5f;
            Map.WorldPosition wp = new Map.WorldPosition(pos);
            result.material = LfRef.chunks.Get(wp);
           
            if (result.material == 0)
            {
                //search down
                wp.WorldGrindex.Y--;
                result.material = LfRef.chunks.Get(wp);
                while (result.material == 0 && wp.WorldGrindex.Y > 0)
                {
                    wp.WorldGrindex.Y--;
                    result.material = LfRef.chunks.Get(wp);
                }
                wp.WorldGrindex.Y++;
            }
            else
            {
                //search up
                wp.WorldGrindex.Y++;
                while (LfRef.chunks.Get(wp) != 0 && wp.WorldGrindex.Y < Map.WorldPosition.ChunkHeight)
                {
                    wp.WorldGrindex.Y++;
                }
            }
            result.BasicY = wp.WorldGrindex.Y;
            //Look on all sourround directions and see if it is sloping
            Vector2 percentPos = new Vector2(
                pos.X % 1, pos.Z % 1);
            Map.WorldPosition neighbor;
            
            //W
            neighbor = wp;
            neighbor.WorldGrindex.X--;
            if (LfRef.chunks.Get(neighbor) != 0)
            {
                neighbor.WorldGrindex.Y++;
                if (LfRef.chunks.Get(neighbor) == 0)
                {
                    result.slopeY = 1 - percentPos.X;
                    result.Xtilt = -1 + percentPos.X;
                }
            }
                
            //E
            neighbor = wp;
            neighbor.WorldGrindex.X++;
            if (LfRef.chunks.Get(neighbor) != 0)
            {
                neighbor.WorldGrindex.Y++;
                if (LfRef.chunks.Get(neighbor) == 0)
                {
                    result.slopeY += percentPos.X;
                    result.Xtilt += percentPos.X;
                }
            }
            //N
            neighbor = wp;
            neighbor.WorldGrindex.Z--;
            if (LfRef.chunks.Get(neighbor) != 0)
            {
                neighbor.WorldGrindex.Y++;
                if (LfRef.chunks.Get(neighbor) == 0)
                {
                    result.slopeY += 1 - percentPos.Y;
                    result.Ztilt = -1 + percentPos.Y;
                }
            }
            //S
            neighbor = wp;
            neighbor.WorldGrindex.Z++;
            if (LfRef.chunks.Get(neighbor) != 0)
            {
                neighbor.WorldGrindex.Y++;
                if (LfRef.chunks.Get(neighbor) == 0)
                {
                    result.slopeY += percentPos.Y;
                    result.Ztilt += percentPos.Y;
                }
            }

            result.slopeY += wp.WorldGrindex.Y;
            return result;
        }

        virtual public bool HasGroundPhysics { get { return true; } }
    }

    struct GroundWithSlopesData
    {
        public float BasicY;
        public float slopeY;
        public float Xtilt;
        public float Ztilt;
        public byte material;

        public GroundWithSlopesData(float Y)
        {
            BasicY = Y;
            this.slopeY = Y;
            Xtilt = 0;
            Ztilt = 0;
            material = 0;
        }
    }

    class NoPhysics : AbsPhysics
    {
        public NoPhysics()
            : base(null)
        { }
        public override void Update(float time)
        {
            //Do nothing
        }
        override public bool HasGroundPhysics { get { return false; } }
    }
    class ProjectilePhysics : AbsPhysics
    {
        public ProjectilePhysics(GameObjects.AbsUpdateObj parent)
            : base(parent)
        { }
        public override void Update(float time)
        {
            //Do nothing
            if (parent.UpdateWorldPos())
            {
                if (LfRef.chunks.Get(parent.WorldPosition) != 0)
                {
                    parent.HandleColl3D(null, null);
                }
            }
        }
    }
    //ska wrappa ihop fysiken för varje object
    //kan göras abstract för att ha olika fysik för olika object
    
    class FlyingObjPhysics : AbsPhysics
    {
        public FlyingObjPhysics(GameObjects.AbsUpdateObj parent)
            : base(parent)
        {
            obsticles = new LF2.ObsticleBounds(2, 2);
        }

        public override void CollectObsticles()
        {
            if (parent.Position.Y < CheckMaxHeight)
            {
                obsticles.Collect(parent.WorldPosition);
            }
        }
        
    }
    class BouncingObjPhysics : AbsPhysics
    {
        public float Bounciness = 1;
        bool sleeping = false;
        public const float BallGravity = -0.00003f;//-0.001f//3,03030303030303E-05
        public BouncingObjPhysics(GameObjects.AbsUpdateObj parent)
            : base(parent)
        {
            Gravity = BallGravity;
        }

        public override void Update(float time)
        {
            base.Update(time);
            if (!sleeping)
            {
                Vector3 pos = parent.Position;
                Vector3 oldPos = pos;
                pos = parent.Velocity.Update(time, pos, Gravity);

                TerrainColl coll = parent.CollisionBound.CollectTerrainObsticles(pos, oldPos, true);
                if (pos.Y <= 0)
                {
                    pos.Y = 0;
                    sleeping = true;
                }
                else if (coll.Collition)
                {
                    Vector3 speed = parent.Velocity.Value;
                    const float CollisionDamp = 1.8f;
                    speed += speed * -coll.CollDir * CollisionDamp * Bounciness;

                    const float TotalBounceDamp = 0.85f;
                    speed *= TotalBounceDamp;

                    const float PosAdjust = 0.08f;
                    const float MinSpeed = 0.002f;
                    if (coll.CollDir.Y != 0 && speed.Length() <= MinSpeed)
                    {
                        speed = Vector3.Zero;
                        sleeping = true;
                    }

                    parent.Velocity.Value = speed;
                    //kanske justera positionen en aning ifrån kollisionen
                    pos += coll.CollDir * PosAdjust;

                    parent.HandleColl3D(null, null);
                }

                parent.Position = pos;
            }
        }

       
        public override void WakeUp()
        {
            sleeping = false;
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
