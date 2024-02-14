using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest
{
    class HeroPhysics : AbsPhysics
    {
        public bool jumpableGround = false;
        Graphics.Mesh collisionBound;
        
        protected bool physicsStatusFalling = false;
        override public bool PhysicsStatusFalling
        {
            get { return physicsStatusFalling; }
        }

        public HeroPhysics(GO.AbsUpdateObj parent)
            : base(parent)
        {
            obsticles = new LootFest.ObsticleBounds(6, 6);
            Gravity = -0.00014f;

            if (PlatformSettings.ViewCollisionBounds)
            {
                collisionBound = new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, Vector3.One * 1.2f,
                    TextureEffectType.Flat, SpriteName.EditorPencilCube, Color.White);

                    //new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.EditorPencilCube),
                    //        Vector3.One * 1.2f, Vector3.Zero);
            }
        }
        public void Jump(float force)
        {
            //System.Diagnostics.Debug.WriteLine("JUMP!");
            //jumping = true;
            parent.Velocity.Y = 0.024f * force;
            //jumpingTime = 0;
            physicsStatusFalling = true;
            jumpableGround = false;
        }
        override public GroundWithSlopesData GetGroundY()
        {
            //Gravity = -0.00014f;

            const float HeroYAdj = 3.2f;
            Vector3 heroPos = parent.Position;
            heroPos.Y -= HeroYAdj;

            GroundWithSlopesData data = groundYwithSlopes2(heroPos);
            bool falling = data.slopeY < heroPos.Y || parent.Velocity.Y > 0.000001f;

            if (falling)
            {
                 if (Ref.TimePassed16ms)
                { parent.Velocity.Y += Gravity * Engine.Update.Time16ms; }
                float newYpos = heroPos.Y + parent.Velocity.Y * Ref.DeltaTimeMs;

                

                //Debug.Log("hero y: " + heroPos.Y.ToString() + ", slope y: " + data.slopeY.ToString());
                //Debug.Log("slope height " + (heroPos.Y - data.slopeY).ToString());
                //if (heroPos.Y - data.slopeY > 1f)
                //{
                //    jumpableGround = false;
                //}

                if (newYpos > data.slopeY)
                {
                    if (newYpos - data.slopeY > 1f)
                    {
                        jumpableGround = false;
                    }
                    data.slopeY = newYpos;
                }
            }
            else
            {
                if (physicsStatusFalling)
                {
                    parent.onGroundPounce(-Bound.Max(parent.Velocity.Y, 0)); 
                }
                parent.Velocity.Y = 0;
                jumpableGround = true;   
            }
            physicsStatusFalling = falling;

            data.BasicY += HeroYAdj;
            data.slopeY += HeroYAdj;

            return data;
        }

        public override GO.Bounds.BoundCollisionResult ObsticleCollision()
        {
            return base.ObsticleCollision();
        }

        public override void CollectObsticles()
        {
            if (parent.Position.Y < CheckMaxHeight)
            {
                obsticles.Collect(parent.WorldPos);
            }
        }
        public override void UpdatePosFromParent()
        {
            // 
        }

       
    }
}
