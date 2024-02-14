using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.PickUp
{
    abstract class AbsHeroPickUp : AbsPickUp
    {
        public bool isUnlockItem = false;
        protected float RotationSpeed = 0.002f;
        public int amount = 1;
        NetworkShare netShare = GO.NetworkShare.Full;
        PlayerCharacter.AbsHero target = null;


        public AbsHeroPickUp(GoArgs args)
            : base(args)
        {
            //if (!args.LocalMember)
            //{
            //    image.Position = ReadPosition(args.reader);
            //}
        }

        public override void netWriteGameObject(System.IO.BinaryWriter w)
        {
            base.netWriteGameObject(w);
            //WritePosition(image.Position, w);
        }
        //public AbsHeroPickUp(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    image.Position = ReadPosition(r);
        //}

        public void Throw(Rotation1D dir)
        {
            if (physics != null)
            {
                physics.SpeedY = 0.004f;
                Velocity = new VikingEngine.Velocity( dir,0.006f);
                physics.WakeUp();
            }
        }

        public override void setImageDirFromSpeed()
        { }


        public override void AsynchGOUpdate(GO.UpdateArgs args)
        {
            const float MaxSlideDistance = 6;




            if (localMember)
            {
                if (this is PickUp.HealUp)
                {
                    lib.DoNothing();
                }
               target = GetClosestHero(true);

               //Vector3 mypos = image.Position;
               //Vector3 tPos = target.Position;
               //Vector3 diff = tPos - mypos;
               //float l = diff.Length();
                float dist = distanceToObject(target);
                if (target != null && distanceToObject(target) > MaxSlideDistance)
                {
                    target = null;
                }

            }
            base.AsynchGOUpdate(args);
        }
        public override void Time_Update(UpdateArgs args)
        {
            if (localMember && autoMoveTowardsHero && pickUpBlockTime <= 0f)
            {
                var sTarget = target;
                if (sTarget != null && heroAbleToPickUp(sTarget))
                {
                    const float MoveVelocity =  0.025f;
                    
                    moveTowardsObject2(sTarget, MoveVelocity);
                }
            }
            if (rotating)
            {
                
                rotation.Radians += RotationSpeed * args.time; setImageDirFromRotation();
            }
            if (Health <= 0)
            {
                DeleteMe();
            }
            base.Time_Update(args);
            UpdateBound();

            if (WorldPos.BlockHasMaterial())
            {
                image.position.Y += 0.1f;
                UpdateWorldPos();
            }
        }

        virtual protected bool rotating { get { return true; } }


        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            if (Alive && heroPickUp((PlayerCharacter.AbsHero)character))
            {
                DeleteMe();
            }
            return true;
        }

        public override void InteractVersion2_interactEvent(PlayerCharacter.AbsHero hero, bool start)
        {
            hero.PickUpCollect(this, true, true);
            
            DeleteMe();
        }

        virtual protected bool heroAbleToPickUp(PlayerCharacter.AbsHero hero)
        {
            return hero.PickUpCollect(this, false, false);
        }

        virtual protected bool heroPickUp(PlayerCharacter.AbsHero hero)
        {
            return hero.PickUpCollect(this, false, true);
        }



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

        //abstract public PickUpType pickUpType { get; }
        
        //public override GameObjectType Type
        //{
        //    get { return pickUpType; }
        //}

        virtual protected bool autoMoveTowardsHero { get { return true; } }

        override protected void timedRemovalEvent() { netShare.DeleteByHost = false; netShare.DeleteByClient = false; }
        override protected void checkPickup() 
        { 
            checkHeroCollision(true, false, null); 
        }

        virtual public bool HelpfulLooterTarget { get { return false; } }

        public override void onObjectSwapOut(AbsUpdateObj original, AbsUpdateObj replacedWith)
        {
            if (target == original)
            {
                target = replacedWith as PlayerCharacter.AbsHero;
            }
        }
    }
}
