using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack.ItemThrow
{
    class Bomb : AbsBomb
    {
        Gadgets.GoodsType type;
        public Bomb(GameObjects.Characters.AbsCharacter h, Gadgets.GoodsType type)
            : base(h, 0.008f)
        {
            this.type = type;
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)type);
        }

        public Bomb(System.IO.BinaryReader r)
            : base(r)
        {
            type = (Gadgets.GoodsType)r.ReadByte();
        }

        public override int UnderType
        {
            get { return (int)WeaponUtype.Bomb; }
        }

        static readonly GameObjects.WeaponAttack.DamageData FireExplosionDamage = new GameObjects.WeaponAttack.DamageData(LootfestLib.FireBombDamage, GameObjects.WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, 
            Gadgets.GoodsType.NONE, Magic.MagicElement.NoMagic, SpecialDamage.NONE, WeaponPush.Normal, Rotation1D.D0);
        static readonly GameObjects.WeaponAttack.DamageData PoisionDamage = new GameObjects.WeaponAttack.DamageData(LootfestLib.PoisionBombDamage, GameObjects.WeaponAttack.WeaponUserType.NON, ByteVector2.Zero,
            Gadgets.GoodsType.NONE, Magic.MagicElement.Poision, SpecialDamage.NONE, WeaponPush.NON, Rotation1D.D0);
        static readonly IntervalF FireExplosionRadius = new IntervalF(6, 7);
        static readonly GameObjects.WeaponAttack.DamageData HolyExplosionDamage = new GameObjects.WeaponAttack.DamageData(LootfestLib.HolyBombDamage, GameObjects.WeaponAttack.WeaponUserType.Player, ByteVector2.Zero, 
            Gadgets.GoodsType.NONE, Magic.MagicElement.NoMagic, SpecialDamage.NONE, WeaponPush.Large, Rotation1D.D0);

        protected override void Explode(UpdateArgs args)
        {
            
             switch (type)
             {
                 case Gadgets.GoodsType.Fire_bomb:
                     if (localMember)
                        new GameObjects.WeaponAttack.Explosion(args.localMembersCounter, image.position, FireExplosionDamage, FireExplosionRadius.GetRandom(), Data.MaterialType.lava, true, true, callBackObj);
                     break;
                 case Gadgets.GoodsType.Holy_bomb:
                     if (localMember)
                         new GameObjects.WeaponAttack.Explosion(args.localMembersCounter, image.position, HolyExplosionDamage, 12, Data.MaterialType.lava, false, true, callBackObj);
                     break;
                 case Gadgets.GoodsType.Fluffy_bomb:
                     if (localMember)
                     {
                         Music.SoundManager.PlaySound(LoadedSound.shieldcrash, image.position);
                         WorldPosition = new Map.WorldPosition(image.position);
                         new GameObjects.Characters.Critter( Characters.CharacterUtype.CritterSheep, WorldPosition);//.SheepBomb(image.Position);
                     }
                     break;
                 case Gadgets.GoodsType.Lightning_bomb:
                     const float SideWaysSpeed = 4;
                     Vector3 position = image.position;
                     position.Y += 1;
                     Vector3 upspeed = Vector3.Up * 6;
                     for (int i = 0; i < 8; i++)
                     {
                         Vector3 speed = upspeed;
                         speed.X += Ref.rnd.Plus_MinusF(SideWaysSpeed);
                         speed.Z += Ref.rnd.Plus_MinusF(SideWaysSpeed);
                         Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.LightSparks, new Graphics.ParticleInitData(
                             lib.RandomV3(image.position, 1), speed));
                     }

                     startDir.Radians += MathHelper.PiOver4;
                     for (int i = 0; i < 4; i++)
                     {
                         startDir.Radians += MathHelper.PiOver2;
                         new WeaponAttack.BombLightSpark(image.position, startDir, callBackObj);
                     }
                     break;
                 case Gadgets.GoodsType.Evil_bomb:
                     if (localMember)
                     {
                         for (int i = 0; i < 3; i++)
                         {
                             new Characters.EvilSpider(image.position);
                         }
                     }
                     break;
                 case Gadgets.GoodsType.Poision_bomb:

                     const float Radius = 9;
                     List<Graphics.ParticleInitData> particles = new List<Graphics.ParticleInitData>();
                     for (int i = 0; i < 32; i++)
                     {
                         particles.Add(new Graphics.ParticleInitData(lib.RandomV3(image.position, Radius)));
                     }
                     Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision, particles);

                     CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(Radius);
                     CollisionBound.UpdatePosition2(this);
                     characterCollCheck(args.localMembersCounter);
                     break;

             }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, LF2.ObjBoundCollData collisionData)
        {
            const bool EndSearchLoop = false;
            if (type == Gadgets.GoodsType.Poision_bomb)
            {
                character.TakeDamage(PoisionDamage, true);
            }
            else
                throw new NotImplementedException("bomb coll damage");
            return EndSearchLoop;
        }

        public override string ToString()
        {
            return "throwing bomb (" + type.ToString() + ")";
        }
    }
}
