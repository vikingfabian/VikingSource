using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class HeroPhysics : AbsPhysics
    {
        Graphics.Mesh collisionBound;
        

        bool jumping = false;
        public bool Jumping
        { get { return jumping; } }
        int jumpingTime = 0;
        protected bool physicsStatusFalling = false;
        override public bool PhysicsStatusFalling
        {
            get { return physicsStatusFalling; }
        }

        public HeroPhysics(GameObjects.AbsUpdateObj parent)
            :base(parent)
        {
            obsticles = new LF2.ObsticleBounds(3, 3);
            Gravity = -0.004f;

            collisionBound =  new Graphics.Mesh(LoadedMesh.cube_repeating, Vector3.Zero, new Graphics.TextureEffect(TextureEffectType.Flat, SpriteName.InterfaceBorderDot),
                        Vector3.One * 1.2f, Vector3.Zero);
        }
        public void Jump()
        {
            System.Diagnostics.Debug.WriteLine("JUMP!");
            jumping = true;
            parent.Velocity.Y = 0.7f;
            jumpingTime = 0;
            physicsStatusFalling = true;
        }
        override public GroundWithSlopesData GetGroundY()
        {
            const float HeroYAdj = 1;
            Vector3 heroPos = parent.Position;
            heroPos.Y -= HeroYAdj;

            GroundWithSlopesData data = groundYwithSlopes2(heroPos);


            physicsStatusFalling = data.slopeY < heroPos.Y || parent.Velocity.Y > 0.1f;

            if (physicsStatusFalling)
            {
                parent.Velocity.Y += Gravity * Ref.DeltaTimeMs;
                float newYpos = heroPos.Y + parent.Velocity.Y;

                if (newYpos > data.slopeY)
                    data.slopeY = newYpos;
            }
            else
            {

                parent.Velocity.Y = 0;
                
            }

            data.BasicY += HeroYAdj;
            data.slopeY += HeroYAdj;

            return data;
        }


        public override void CollectObsticles()
        {
            if (parent.Position.Y < CheckMaxHeight)
            {
                obsticles.Collect(parent.WorldPosition);
            }
        }
        public override void UpdatePosFromParent()
        {
            // 
        }

       
    }
}
