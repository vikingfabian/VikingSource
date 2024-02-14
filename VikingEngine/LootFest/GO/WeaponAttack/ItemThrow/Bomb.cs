using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.WeaponAttack.ItemThrow
{
    class Bomb : AbsBomb
    {
        Gadgets.GoodsType type;
        public Bomb(GoArgs args, GO.Characters.AbsCharacter h, Gadgets.GoodsType type)
            : base(args, h, 0.008f)
        {
            this.type = type;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
            else
            {
                type = (Gadgets.GoodsType)args.reader.ReadByte();
            }
        }

        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);
            writer.Write((byte)type);
        }

        //public Bomb(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    type = (Gadgets.GoodsType)r.ReadByte();
        //}

        public override GameObjectType Type
        {
            get { return GameObjectType.Bomb; }
        }

        static readonly GO.WeaponAttack.DamageData FireExplosionDamage = new GO.WeaponAttack.DamageData(LfLib.HeroNormalAttack, GO.WeaponAttack.WeaponUserType.NON, NetworkId.Empty, 
            Magic.MagicElement.NoMagic, SpecialDamage.NONE, WeaponPush.Normal, Rotation1D.D0);
        static readonly GO.WeaponAttack.DamageData PoisionDamage = new GO.WeaponAttack.DamageData(LfLib.HeroWeakAttack, GO.WeaponAttack.WeaponUserType.NON, NetworkId.Empty,
            Magic.MagicElement.Poision, SpecialDamage.NONE, WeaponPush.NON, Rotation1D.D0);
        static readonly IntervalF FireExplosionRadius = new IntervalF(6, 7);
        static readonly GO.WeaponAttack.DamageData HolyExplosionDamage = new GO.WeaponAttack.DamageData(LfLib.HeroStrongAttack, GO.WeaponAttack.WeaponUserType.Player, NetworkId.Empty, 
            Magic.MagicElement.NoMagic, SpecialDamage.NONE, WeaponPush.Large, Rotation1D.D0);

        protected override void Explode(UpdateArgs args)
        {
            
             switch (type)
             {
                 case Gadgets.GoodsType.Fire_bomb:
                     if (localMember)
                        new GO.WeaponAttack.Explosion(new GoArgs(image.position), args.localMembersCounter, FireExplosionDamage, FireExplosionRadius.GetRandom(), Data.MaterialType.lava, true, true, callBackObj);
                     break;
                 case Gadgets.GoodsType.Holy_bomb:
                     if (localMember)
                         new GO.WeaponAttack.Explosion(new GoArgs(image.position), args.localMembersCounter, HolyExplosionDamage, 12, Data.MaterialType.lava, false, true, callBackObj);
                     break;
                 case Gadgets.GoodsType.Fluffy_bomb:
                     if (localMember)
                     {
                         Music.SoundManager.PlaySound(LoadedSound.shieldcrash, image.position);
                         WorldPos = new Map.WorldPosition(image.position);
                         //new GO.Characters.Critter( GameObjectType.CritterSheep, WorldPos);//.SheepBomb(image.Position);
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
                             Ref.rnd.Vector3_Sq(image.position, 1), speed));
                     }

                     startDir.Radians += MathHelper.PiOver4;
                     for (int i = 0; i < 4; i++)
                     {
                         startDir.Radians += MathHelper.PiOver2;
                        // new WeaponAttack.BombLightSpark(image.Position, startDir, callBackObj);
                     }
                     break;
                 case Gadgets.GoodsType.Evil_bomb:
                     if (localMember)
                     {
                         for (int i = 0; i < 3; i++)
                         {
                             //new Characters.EvilSpider(image.Position);
                         }
                     }
                     break;
                 case Gadgets.GoodsType.Poision_bomb:

                     const float Radius = 9;
                     List<Graphics.ParticleInitData> particles = new List<Graphics.ParticleInitData>();
                     for (int i = 0; i < 32; i++)
                     {
                         particles.Add(new Graphics.ParticleInitData(Ref.rnd.Vector3_Sq(image.position, Radius)));
                     }
                     Engine.ParticleHandler.AddParticles(Graphics.ParticleSystemType.Poision, particles);

                     CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickBoundingBox(Radius);
                     CollisionAndDefaultBound.UpdatePosition2(this);
                     characterCollCheck(args.localMembersCounter);
                     break;

             }
        }

        protected override bool handleCharacterColl(AbsUpdateObj character, GO.Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
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
