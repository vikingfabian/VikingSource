//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LootFest.GO.WeaponAttack
//{
//    class MagicianLightSpark : AbsLightSpark
//    {
//        int bossLvl;
//        const float LifeTime = 2500;
//        public MagicianLightSpark(Vector3 startPos, Rotation1D dir, int bossLvl)
//            : base(startPos, dir, Damage(bossLvl), null)
//        {
//            this.bossLvl = bossLvl;
//            Health = LifeTime;
//            NetworkShareObject();
//        }
//        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
//        {
//            base.ObjToNetPacket(writer);
//            writer.Write((byte)bossLvl);
//        }
//        public MagicianLightSpark(System.IO.BinaryReader r)
//            : base(r)
//        {
//            bossLvl = r.ReadByte();
//            Health = LifeTime;
//            givesDamage = Damage(bossLvl);
//        }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.MagicianLightSpark; }
//        }

//        static DamageData Damage(int bossLvl)
//        {
//            return new DamageData(2, WeaponUserType.Enemy, NetworkId.Empty, Magic.MagicElement.Lightning);
//        }

//        public override WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return WeaponUserType.Enemy;
//            }
//        }
//        protected override bool LocalDamageCheck
//        {
//            get
//            {
//                return true;
//            }
//        }
//    }

//    class BombLightSpark : AbsLightSpark
//    {
//        static readonly DamageData Damage = new DamageData(LfLib.HeroNormalAttack, WeaponUserType.Player, NetworkId.Empty, Magic.MagicElement.Lightning);

//        public BombLightSpark(Vector3 startPos, Rotation1D dir, AbsUpdateObj callBackObj)
//            : base(startPos, dir, Damage, callBackObj)
//        { }

//        public override GameObjectType Type
//        {
//            get { return GameObjectType.BombLightSpark; }
//        }

        
//    }

//    //slider along the ground
//    abstract class AbsLightSpark : AbsWeapon
//    {
//        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(186,230,250), new Vector3(2, 0.4f, 0.4f));
//        const float LifeTime = 1000;

//        public AbsLightSpark(Vector3 startPos, Rotation1D dir, DamageData damage, AbsUpdateObj callBackObj)
//            :base(damage)
//        {
            
//            rotation = dir;
//            lightSparkBasicInit(startPos);
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);
//            GO.AbsUpdateObj.WritePosition(image.Position, w);
//            w.Write(rotation.ByteDir);
//        }

//        public AbsLightSpark(System.IO.BinaryReader r)
//            :base(r)
//        {  
//            Vector3 startPos = GO.AbsUpdateObj.ReadPosition(r);
//            rotation.ByteDir = r.ReadByte();

//            lightSparkBasicInit(startPos);
//        }

//        void lightSparkBasicInit(Vector3 startPos)
//        {
//             WorldPos = new Map.WorldPosition(startPos);
//            const float Scale = 1.6f;
//            const float BoundingScale = 0.35f;
//            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.light_spark, TempImage, Scale, 0, false);
//            image.Position = startPos;
//            Health = LifeTime;

//            Velocity.Set(rotation, 0.02f);
//            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(BoundingScale);
//        }

//        float goalY; 
//        public override void Time_Update(UpdateArgs args)
//        {
//#if !CMODE
//            base.Time_Update(args);

//            const float RotateSpeed = 0.01f;
//            image.Rotation.RotateWorld(Vector3.UnitX * RotateSpeed * args.time);
//            if (UpdateWorldPos())
//            {
//                const float AboveGound = 1.4f;
//                goalY = LfRef.chunks.GetScreen(WorldPos).GetClosestFreeY(WorldPos) + AboveGound;
//            }

//            float diff = goalY - image.Position.Y;
//            image.Position.Y += diff * 0.05f;

//            Health -= args.time;
//            if (Health <= 0)
//            {
//                DeleteMe();
//            }

//            characterCollCheck(args.localMembersCounter);

//            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks, new Graphics.ParticleInitData(image.Position));
//#endif
//        }

//        public override ObjPhysicsType PhysicsType
//        {
//            get
//            {
//                return ObjPhysicsType.NO_PHYSICS;
//            }
//        }
//        protected override bool LocalDamageCheck
//        {
//            get
//            {
//                return true;
//            }
//        }

//        public override NetworkShare  NetworkShareSettings
//        {
//            get 
//            { 
//                 return GO.NetworkShare.OnlyCreation;
//            }
//        }
//        //protected override WeaponTrophyType weaponTrophyType
//        //{
//        //    get { return WeaponTrophyType.Spear; }
//        //}
//        protected override NetworkClientRotationUpdateType NetRotationType
//        {
//            get
//            {
//                return NetworkClientRotationUpdateType.NoRotation;
//            }
//        }

//    }
//}
