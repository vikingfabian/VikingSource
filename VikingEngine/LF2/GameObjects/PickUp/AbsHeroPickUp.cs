using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.PickUp
{
    abstract class AbsHeroPickUp : AbsPickUp
    {
        NetworkShare netShare = GameObjects.NetworkShare.Full;
        Characters.Hero target = null;


        public AbsHeroPickUp(Vector3 position)
            : base(position)
        {

        }

        public override void ObjToNetPacket(System.IO.BinaryWriter w)
        {
            base.ObjToNetPacket(w);
            WritePosition(image.position, w);
        }
        public AbsHeroPickUp(System.IO.BinaryReader r)
            : base(r)
        {
            image.position = ReadPosition(r);
        }

        public void Throw(Rotation1D dir)
        {
            if (physics != null)
            {
                physics.SpeedY = 0.004f;
                Velocity = new VikingEngine.Velocity( dir,0.006f);
                physics.WakeUp();
            }
        }

        protected override void setImageDirFromSpeed()
        { }


        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            if (localMember)
            {
                if (pickUpBlockTime <= 0)
                {
                    //List<Characters.Hero> heroes = LfRef.gamestate.AllHeroes();
                    for (int i = 0; i < LfRef.AllHeroes.Count; ++i)//foreach (Characters.Hero hero in LfRef.AllHeroes)
                    {
                        Characters.Hero hero = LfRef.AllHeroes[i];
                        if (distanceToObject(hero) <= 6)
                        {
                            target = hero;
                            break;
                        }
                    }
                }
            }
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember)
            {
                if (target != null)
                {
                    const float MoveVelocity =  0.025f;
                    //const float MinDist = 0.1f;
                    moveTowardsObject2(target, MoveVelocity);
                    //moveImage(Velocity, args.time);
                    //float diffY = target.Y - image.Position.Y;
                    //if (Math.Abs(diffY) > MinDist)
                    //{
                    //    image.Position.Y += MoveVelocity * args.time * lib.FloatToDir(diffY);
                    //}
                }
            }
            if (rotating)
            {
                const float RotationSpeed = 0.002f;
                rotation.Radians += RotationSpeed * args.time; setImageDirFromRotation();
            }
            base.Time_Update(args);
            UpdateBound();
        }

        virtual protected bool rotating { get { return true; } }


        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
#if WINDOWS
            Debug.DebugLib.CrashIfThreaded();
#endif
            heroPickUp((Characters.Hero)character);
            DeleteMe();
            
            return true;
        }

        

        virtual protected void heroPickUp(Characters.Hero hero)
        {
            hero.PickUpCollect(item);
        }




        virtual protected Gadgets.IGadget item
        { get { throw new NotImplementedException(); } }

        protected override NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return NetworkClientRotationUpdateType.NoRotation;
            }
        }
        override public NetworkShare NetworkShareSettings
        {
            get
            {
                if (localMember)
                {
                    netShare.Update = (physics != null && !physics.Sleeping) || target != null;
                    return netShare;
                }
                else
                {
                    return netShare;
                }
            }
        }


        override protected void timedRemovalEvent() { netShare.DeleteByHost = false; netShare.DeleteByClient = false; }
        override protected void checkPickup() { checkHeroCollision(true); } 
    }
}
