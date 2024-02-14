//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.Engine;
//using Game1.Graphics;
//using Game1.LootFest.GameObjects.WeaponAttack;

//namespace Game1.LootFest.GameObjects.Toys
//{
    
//    class ToyProjectile : WeaponAttack.AbsGravityProjectile
//    {
//        const float Scale = 0.6f;
//        static readonly LootFest.ObjSingleBound Bound = LootFest.ObjSingleBound.QuickBoundingBox(0.3f);
//        static int numBulletPacket = 0;
//        float clientDelay;

//        public ToyProjectile(float damage, Vector3 startPos, Vector3 speed, Vector3 parentSpeed, ByteVector2 parentIndex)
//            : base(new WeaponAttack.DamageData(damage, WeaponAttack.WeaponUserType.Toy, parentIndex),
//                startPos, speed + parentSpeed, Bound)
//        {
//            //Music.SoundManager.PlaySound(LoadedSound.rc_shoot, startPos);
           
//            //physics.Gravity = Gravity;
//            NetworkShareObject();
//           // givesDamage.Damage = 5;
//        }

        
//        public ToyProjectile(System.IO.BinaryReader System.IO.BinaryReader)
//            : base(r)
//        {
//            clientDelay = numBulletPacket * 300 + 1;
//            image.Visible = false;
            
//            numBulletPacket++;
//        }
//        protected override void clientSpeed(float speed)
//        {
//            base.clientSpeed(speed);
//        }
//        public override int UnderType
//        {
//            get
//            {
//                return (int)WeaponAttack.WeaponUtype.ToyProjectile;
//            }
//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            if (localMember)
//            {
//                base.Time_Update(args);
//            }
//            else
//            {
//                numBulletPacket = 0;

//                if (clientDelay > 0)
//                {
//                    clientDelay -= args.time;
//                    if (clientDelay <= 0)
//                    {
//                        image.Visible = true;
//                        //Music.SoundManager.PlaySound(LoadedSound.rc_shoot, image.Position);
//                    }
//                }
//                else
//                    base.Time_Update(args);
//            }
            
//        }
//        //public override void ClientTimeUpdate(float time, List<AbsUpdateObj> args.localMembersCounter, List<AbsUpdateObj> active)
//        //{

//        //    numBulletPacket = 0;

//        //    if (clientDelay > 0)
//        //    {
//        //        clientDelay -= time;
//        //        if (clientDelay <= 0)
//        //        {
//        //            image.Visible = true;
//        //            Music.SoundManager.PlaySound(LoadedSound.rc_shoot, image.Position, 0);
//        //        }
//        //    }
//        //    else
//        //        base.ClientTimeUpdate(time, args.localMembersCounter, active);
//        //}
//        protected override VoxelModelName VoxelObjName
//        {
//            get { return VoxelModelName.rc_ball; }
//        }
//        protected override float ImageScale
//        {
//            get { return Scale; }
//        }
//        const float Gravity = LootFest.AbsPhysics.StandardGravity * 0.002f;
//        protected override float gravity
//        {
//            get
//            {
//                return Gravity;
//            }
//        }
//        protected override bool removeAfterCharColl
//        {
//            get
//            {
//                return true;
//            }
//        }
//        protected override WeaponTrophyType weaponTrophyType
//        {
//            get { return WeaponTrophyType.Other; }
//        }
//    }
//}
