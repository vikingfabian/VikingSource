using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    class CharacterPhysics : AbsPhysics
    {
        protected bool physicsStatusFalling = false;
        int currentBlockY;

        public CharacterPhysics(GO.AbsUpdateObj parent)
            : base(parent)
        {
            obsticles = new LootFest.ObsticleBounds(2, 3);
        }
        public override GroundWithSlopesData GetGroundY()
        {

            if (physicsStatusFalling)
            {

                if (parent.Position.Y < 2)
                {
                    return new GroundWithSlopesData(2);
                }
                parent.Velocity.Y += Gravity;
                return new GroundWithSlopesData(parent.Position.Y); //+ parent.Velocity.Y);
            }
            else
            {
                parent.Velocity.Y = 0;
                //NYTT
                return new GroundWithSlopesData(currentBlockY);
            }
        }

        public override void CollectObsticles()
        {

            obsticles.Collect(parent.WorldPos);

            currentBlockY = Convert.ToInt32( LfRef.chunks.GetScreen(parent.WorldPos).GetClosestFreeY(parent.WorldPos));

            const byte MinY = 2;
            if (currentBlockY < MinY)
            { currentBlockY = MinY; }

            if (parent.Position.Y > currentBlockY)
            {
                physicsStatusFalling = true;
            }
            else
            {
                physicsStatusFalling = false;
            }

        }


        public override void UpdatePosFromParent()
        {
            currentBlockY = (int)parent.Position.Y;
            physicsStatusFalling = true;
        }

        public override float SpeedY
        {
            get
            {
                return parent.Velocity.Y;
            }
            set
            {
                parent.Velocity.Y = value;
                physicsStatusFalling = true;
            }
        }
    }

   
}
