using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.ItemThrow
{
    class MagicianPoisonBomb : AbsBomb
    {
        int level;

        public MagicianPoisonBomb(GameObjects.Characters.AbsCharacter magician, int level)
            : base(magician, 0.006f + Ref.rnd.Plus_MinusF(0.002f))
        {
            this.level = level;
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)level);
        }

        public MagicianPoisonBomb(System.IO.BinaryReader r)
            : base(r)
        {
            level = r.ReadByte();
        }
        protected override void Explode(UpdateArgs args)
        {
            DamageData damage = new DamageData(GameObjects.Characters.Magician.MagicDamage(level),
                WeaponAttack.WeaponUserType.Enemy, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Poision);
            new PoisionEmitter(image.position, damage);
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.MagicianPoisonBomb; }
        }
    }

    class PoisionEmitter : AbsVoxelObj
    {
        static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(24,255,0), new Vector3(1.4f));
        const float ImageScale = 0.35f;
        const float Radius = 3;
        const float LifeTime = 5000;
        
        DamageData damage;

        public PoisionEmitter(Vector3 pos, DamageData damage)
            :base()
        {
            image = LF2.Data.ImageAutoLoad.AutoLoadImgInstace(VoxelModelName.poision_emitter, TempImage, ImageScale, 1);
            image.position = pos;
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(Radius);
            Health = LifeTime;
            this.damage = damage;
        }

        public override void Time_Update(UpdateArgs args)
        {
            Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision, lib.RandomV3(image.position, Radius));
            image.scale = Vector3.One * (Health / LifeTime) * ImageScale; //scrink
            if (Health <= 0)
                DeleteMe();
        }

        public override void AIupdate(GameObjects.UpdateArgs args)
        {
            Health -= args.time;
            characterCollCheck(args.localMembersCounter);
        }

        public override WeaponUserType WeaponTargetType
        {
            get
            {
                return  WeaponAttack.WeaponUserType.Enemy;
            }
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            new Process.UnthreadedDamage(damage, character);
            return false;
        }
        public override ObjectType Type
        {
            get { return ObjectType.WeaponAttack; }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return NetworkShare.None;
            }
        }
        public override int UnderType
        {
            get { throw new NotImplementedException(); }
        }
    }
}
