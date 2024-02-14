//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
//{
//    class MagicianPoisonBomb : AbsBomb
//    {
//        int level;

//        public MagicianPoisonBomb(GO.Characters.AbsCharacter magician, int level)
//            : base(magician, 0.006f + Ref.rnd.Plus_MinusF(0.002f))
//        {
//            this.level = level;
//            NetworkShareObject();
//        }

//        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
//        {
//            base.ObjToNetPacket(writer);
//            writer.Write((byte)level);
//        }

//        public MagicianPoisonBomb(System.IO.BinaryReader r)
//            : base(r)
//        {
//            level = r.ReadByte();
//        }
//        protected override void Explode(UpdateArgs args)
//        {
//            DamageData damage = new DamageData(GO.Characters.Magician.MagicDamage(level),
//                WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero, Magic.MagicElement.Poision);
//            new PoisionEmitter(image.Position, damage);
//        }
//        public override GameObjectType Type
//        {
//            get { return GameObjectType.MagicianPoisonBomb; }
//        }
//    }

//    class PoisionEmitter : AbsVoxelObj
//    {
//        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(24,255,0), new Vector3(1.4f));
//        const float ImageScale = 0.35f;
//        const float Radius = 3;
//        const float LifeTime = 5000;
        
//        DamageData damage;

//        public PoisionEmitter(Vector3 pos, DamageData damage)
//            :base()
//        {
//            image = LootFest.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelObjName.poision_emitter, TempImage, ImageScale, 1);
//            image.Position = pos;
//            CollisionBound = LootFest.ObjSingleBound.QuickBoundingBox(Radius);
//            Health = LifeTime;
//            this.damage = damage;
//        }

//        public override void Time_Update(UpdateArgs args)
//        {
//            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision, Ref.rnd.Vector3_Sq(image.Position, Radius));
//            image.Scale = Vector3.One * (Health / LifeTime) * ImageScale; //scrink
//            if (Health <= 0)
//                DeleteMe();
//        }

//        public override void AIupdate(GO.UpdateArgs args)
//        {
//            Health -= args.time;
//            characterCollCheck(args.localMembersCounter);
//        }

//        public override WeaponUserType WeaponTargetType
//        {
//            get
//            {
//                return  WeaponAttack.WeaponUserType.Enemy;
//            }
//        }
//        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
//        {
//            new Process.UnthreadedDamage(damage, character);
//            return false;
//        }
//        public override ObjectType Type
//        {
//            get { return ObjectType.WeaponAttack; }
//        }
//        public override NetworkShare NetworkShareSettings
//        {
//            get
//            {
//                return NetworkShare.None;
//            }
//        }
//        public override GameObjectType Type
//        {
//            get { throw new NotImplementedException(); }
//        }
//    }
//}
