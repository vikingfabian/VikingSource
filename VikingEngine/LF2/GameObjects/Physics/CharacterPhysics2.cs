using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    class CharacterPhysics2: AbsPhysics
    {
        protected bool physicsStatusFalling = false;
        override public bool PhysicsStatusFalling
        {
            get { return physicsStatusFalling; }
        }

        public CharacterPhysics2(GameObjects.AbsUpdateObj parent)
            :base(parent)
        {
            obsticles = new LF2.ObsticleBounds(2, 3);
        }
        override public GroundWithSlopesData GetGroundY()
        {
            Vector3 pos = parent.Position;
            GroundWithSlopesData data = groundYwithSlopes(pos);
            physicsStatusFalling = data.slopeY < pos.Y - 0.8f;
            
            if (physicsStatusFalling)
            {
                parent.Velocity.Y += Gravity;
                return new GroundWithSlopesData(pos.Y + parent.Velocity.Y);
            }
            else
            {
                parent.Velocity.Y = 0;
                return data;
            }

        }
        public override void Update(float time)
        {
            base.Update(time);
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
        }

       
    }

    class CharacterPhysicsAdvanced : AbsPhysics
    {
        protected bool physicsStatusFalling = false;
        override public bool PhysicsStatusFalling
        {
            get { return physicsStatusFalling; }
        }

        public CharacterPhysicsAdvanced(GameObjects.AbsUpdateObj parent)
            : base(parent)
        {
            obsticles = new LF2.ObsticleBounds(2, 3);
        }
        override public GroundWithSlopesData GetGroundY()
        {
            Vector3 pos = parent.Position;
            GroundWithSlopesData data = groundYwithSlopes2(pos);
            physicsStatusFalling = data.slopeY < pos.Y - 0.8f;

            if (physicsStatusFalling)
            {
                parent.Velocity.Y += Gravity;
                return new GroundWithSlopesData(pos.Y);
            }
            else
            {
                parent.Velocity.Y = 0;
                return data;
            }

        }
        public override void Update(float time)
        {
            base.Update(time);
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
        }


    }
}
