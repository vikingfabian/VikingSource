using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    //kanske kunna kolla framför sig om terrängen plötsligt ändrar höjd och använda det som collision
    class CharacterPhysicsSimple : AbsPhysics
    {
        public float forwardSearch = 2;
        public const float CharacterStandardGravity = -0.0002f;

        public CharacterPhysicsSimple(GO.AbsUpdateObj parent)
            : base(parent)
        {
            Gravity = CharacterStandardGravity;
        }

        public override void PushForce(Vector2 force)
        {
            Velocity v = new Velocity(force);
            
            parent.Position = addVelocity(ref v, parent.Position, 1);
        }
        
        /// <returns>new position</returns>
        public override Vector3 UpdateMovement()
        {
            return addVelocity(ref parent.Velocity, parent.Position, Ref.DeltaTimeMs);
            
        }

        Vector3 addVelocity(ref Velocity v, Vector3 pos, float time)
        {
            if (parent is VikingEngine.LootFest.GO.NPC.Miner)
            {
                lib.DoNothing();
            }


            Vector3 result = v.Update(time, pos);

            Map.WorldPosition wp = new Map.WorldPosition(result);
            float groundY = wp.GetClosestFreeYFloat();

            float ydiff = groundY - result.Y;

            onGround = Math.Abs(ydiff) <= 0.1f;
            if (!onGround)
            {
                if (ydiff > 0) //buried inside ground
                {
                    
                    //if (ydiff > 2)
                    //{//to high difference, undo movement and send collision
                    //    parent.HandleObsticle(true, null);
                    //    return pos;
                    //}
                    //else 
                        if (Math.Abs(ydiff) > 1.1f || parent.Velocity.Y < -0.01f)
                    {
                        result.Y = groundY;
                    }
                    else
                    { //slowly float upward
                        result.Y += 0.008f * time * ydiff;
                    }
                    parent.Velocity.Y = 0;
                }
                else //floating above ground, should fall
                {
                    parent.Velocity.Y += Gravity * time;
                }
            }

            if (forwardSearch > 0 && !parent.Velocity.ZeroPlaneSpeed)
            {
                Velocity vcheck = parent.Velocity;
                vcheck.Value.Y = 0;
                vcheck.SetLength(forwardSearch);
                Vector3 check = result;
                check.Y = groundY;
                check += vcheck.Value;
                float y = new Map.WorldPosition(check).GetClosestFreeYFloat();
                ydiff = y - check.Y;
                if (ydiff > 2) parent.HandleObsticle(true, null); //wall
                else if (ydiff < -2) parent.HandleObsticle(false, null); //pit
            }

            return result;
        }

        public override bool PhysicsStatusFalling
        {
            get
            {
                return parent.Velocity.Y != 0;
            }
        }

        public override GroundWithSlopesData GetGroundY()
        {
            throw new NotImplementedException();
        }

        public override void CollectObsticles()
        {
            throw new NotImplementedException();
        }
    }
}
