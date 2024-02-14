using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
   
    class GoldenArrow : GravityArrow
    {
        public GoldenArrow(DamageData givesDamage, Vector3 startPos, Vector3 target,
            LF2.AbsObjBound bound, bool reuse, Magic.MagicRingSkill magicBurst, Data.Gadgets.BluePrint bowType, Players.Player player)
            :base(givesDamage, startPos, target, bound, reuse, magicBurst, bowType, player)
        {
            this.givesDamage.Damage *= LootfestLib.GoldenArrowDamageMultiplier;
            NetworkShareObject();
        }
        public GoldenArrow(System.IO.BinaryReader r)
            : base(r)
        {
        }
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            Engine.ParticleHandler.AddParticles(ParticleSystemType.GoldenSparkle, image.position);
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.golden_arrow; }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.GoldenArrow; }
        }
    }

    /// <summary>
    /// Fired by player bow
    /// </summary>
    class GravityArrow : AbsGravityProjectile
    {
        bool reuse;
        Magic.MagicRingSkill magicBurst;
        Players.Player player;

        public GravityArrow(DamageData givesDamage, Vector3 startPos, Vector3 target,
            LF2.AbsObjBound bound, bool reuse, Magic.MagicRingSkill magicBurst, Data.Gadgets.BluePrint bowType, Players.Player player)
            : base(givesDamage, startPos, target, bound, bowTypeToFireSpeed(bowType))
        {
            if (player != null)
            {
                this.player = player;
                callBackObj = player.hero;
            }
            this.magicBurst = magicBurst;
            this.reuse = reuse;
            Music.SoundManager.PlaySound(LoadedSound.Bow, startPos);
            NetworkShareObject();
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)givesDamage.Magic);
        }
        public GravityArrow(System.IO.BinaryReader r)
            : base(r)
        {
            givesDamage.Magic = (Magic.MagicElement)r.ReadByte();
            Music.SoundManager.PlaySound(LoadedSound.Bow, image.position);
            particleSetup();
        }

        public static LF2.ObjSingleBound ArrowBound(Rotation1D dir)
        {
            const float BoundW = 0.2f;
            return new LF2.ObjSingleBound(new LF2.BoundData2( new Physics.Box1axisBound(new VectorVolume(Vector3.Zero, 
                new Vector3(BoundW, BoundW, 1.1f)), dir), Vector3.Zero));
        }

        protected override void HitCharacter(AbsUpdateObj character)
        {
            base.HitCharacter(character);
            if (reuse)
            {
                reuse = false;
                new GameObjects.PickUp.ArrowPickUp(character.Position);
            }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            image.Rotation.RotateAxis(new Vector3(0,0, 0.02f * args.time));
        }

        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            //magic burst
            if (magicBurst != Magic.MagicRingSkill.NO_SKILL)
            {
                Vector3 groundPos = image.position;
                WorldPosition.SetFromGroundY(1);
                groundPos.Y = WorldPosition.WorldGrindex.Y;

                switch (magicBurst)
                {
                    case Magic.MagicRingSkill.Projectile_evil_burst:
                        new Magic.ProjectileEvilBurst(image.position);
                        break;
                    case Magic.MagicRingSkill.Projectile_poision_burst:
                        //two clouds of smoke move out from the center
                        Rotation1D dir = Rotation1D.Random();
                        new Magic.ProjectilePoisionBurst(image.position, dir);
                        dir.Add(Rotation1D.D180);
                        new Magic.ProjectilePoisionBurst(image.position, dir);
                        
                        break;
                    case Magic.MagicRingSkill.Projectile_lightning_burst:
                        Rotation1D startDir = Rotation1D.Random();
                        
                        for (int i = 0; i < 4; i++)
                        {
                            startDir.Radians += MathHelper.PiOver2;
                            new WeaponAttack.BombLightSpark(image.position, startDir, callBackObj);
                        }
                        break;
                    case Magic.MagicRingSkill.Projectile_fire_burst:
                        new GameObjects.WeaponAttack.Explosion(LfRef.gamestate.GameObjCollection.AllMembersUpdateCounter, groundPos,
                            new DamageData(LootfestLib.ProjectileFireBurstDamage, WeaponAttack.WeaponUserType.NON, ByteVector2.Zero, 
                                Gadgets.GoodsType.NONE, Magic.MagicElement.Fire), LootfestLib.ProjectileFireBurstRadius, 
                                Data.MaterialType.black, false, false, callBackObj);
                        break;
                }
            }

            base.HandleColl3D(collData, ObjCollision);
        }

        public override int UnderType
        {
            get { return (int)WeaponUtype.GravityArrow; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Arrow; }
        }
        protected override float ImageScale
        {
            get { return 2f; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Arrow_Slingstone; }
        }
        override protected bool removeAfterCharColl { get { return true; } }

    }
    class SlingStone : AbsGravityProjectile
    {
        
        public SlingStone(DamageData givesDamage, Vector3 startPos, Vector3 target, 
            LF2.AbsObjBound bound)
            : base(givesDamage, startPos, lib.RandomV3(target, 3), bound, bowTypeToFireSpeed( Data.Gadgets.BluePrint.Sling))//0.035f)
        {
            Music.SoundManager.PlaySound(LoadedSound.Bow, startPos);
            NetworkShareObject();
        }
        public SlingStone(System.IO.BinaryReader r)
            : base(r)
        {
            Music.SoundManager.PlaySound(LoadedSound.Bow, image.position);
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.SlingStone; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.slingstone; }
        }
        protected override float ImageScale
        {
            get { return 0.4f; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Arrow_Slingstone; }
        }
        override protected bool removeAfterCharColl { get { return true; } }

        Effects.BouncingBlockColors StoneDamageColors = new Effects.BouncingBlockColors(Data.MaterialType.dark_gray, Data.MaterialType.dark_gray, Data.MaterialType.dark_gray);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return StoneDamageColors;
            }
        }
    }
    
}
