//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.Engine;
//using VikingEngine.Graphics;
//using VikingEngine.LootFest;
//using VikingEngine.LootFest.GO.WeaponAttack;

//namespace VikingEngine.LootFest.GO.Magic
//{
//    class MagicianFireBall : FireBall
//    {
//        int level;
//        public MagicianFireBall(int level, Vector3 startPosition, Rotation1D dir)
//            : base(damage(level), startPosition, dir)
//        {
//            this.level = level;
//        }
//        public override void ObjToNetPacket(System.IO.BinaryWriter w)
//        {
//            base.ObjToNetPacket(w);
//            w.Write((byte)level);
//        }
//        public MagicianFireBall(System.IO.BinaryReader r)
//            : base(r)
//        {
//            givesDamage = damage(r.ReadByte());
//        }
//        static WeaponAttack.DamageData damage(int bosslevel)
//        {
//           return new WeaponAttack.DamageData(Characters.Magician.MagicDamage(bosslevel),
//                WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero,
//                Gadgets.GoodsType.NONE, Magic.MagicElement.Fire);
//        }
            
//        public override GameObjectType Type
//        {
//            get { return (int)GameObjectType.MagicianFireball; }
//        }
//        protected override bool LocalDamageCheck
//        {
//            get
//            {
//                return true;
//            }
//        }
//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);
//        }
//    }

//    class FireBall : WeaponAttack.Linear3DProjectile//Weapons.AbsProjectile
//    {
//        const float Speed = 0.03f;
//        const float Scale = 0.5f;
//        static readonly LootFest.ObjSingleBound Bound = new LootFest.ObjSingleBound(
//              new BoundData2(new Physics.StaticBoxBound(
//                    new VectorVolume(Vector3.Zero, Vector3.One * Scale * 0.55f)), new Vector3(0, 0.6f, 0))); //LootFest.ObjSingleBound.QuickBoundingBox(Scale * PublicConstants.HALF);
//        //static readonly Vector3 ImgScale = Vector3.One * Scale / 8;
//        List<ParticleInitData> particles;

//        public FireBall(GO.WeaponAttack.DamageData damage, Vector3 startPosition, Rotation1D dir)
//            : base(damage, startPosition, dir, Bound, Speed)
//        {
//            fireBallBasicInit();
//        }

//        public FireBall(System.IO.BinaryReader r)
//            : base(r, Bound)
//        {
//            fireBallBasicInit();
//        }

//        void fireBallBasicInit()
//        {
//            Velocity.Y = -0.001f; //slowly fall down
//            particles = new List<ParticleInitData>
//            {
//                new  ParticleInitData(), new  ParticleInitData(), new  ParticleInitData(), 
//            };
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            base.Time_Update(args);

//            //particles
//            //Vector3 pos;
//            for (int i = 0; i < particles.Count; i++)
//            {
//                particles[i] = new ParticleInitData(Ref.rnd.Vector3_Sq(image.Position, 0.3f));
//            }
//            Engine.ParticleHandler.AddParticles(ParticleSystemType.Fire, particles);//new ParticleInitData(image.Position));
//        }

//        public override void HandleColl3D(GO.Bounds.BoundCollisionResult collData, AbsUpdateObj ObjCollision)
//        {
            
//            //create a small fire
//            new Elements.Fire(image.Position);

//            base.HandleColl3D(collData, ObjCollision);
//        }
//        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            for (int i = 0; i < 8; i++)
//            {
//                Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Smoke,
//                       new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(image.Position, 1), Vector3.Zero));
//            }
//            return base.handleCharacterColl(character, collisionData);
//        }

//        public override GameObjectType Type
//        {
//            get { return (int)Magic.MagicElement.Fire; }
//        }

//        override protected VoxelObjName VoxelObjName
//        {
//            get { return VoxelObjName.FireBall; }
//        }

//        protected override float ImageScale
//        {
//            get { return Scale; }
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
