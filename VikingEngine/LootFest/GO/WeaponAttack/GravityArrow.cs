using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//xna

namespace VikingEngine.LootFest.GO.WeaponAttack
{
/// <summary>
/// Fired by player bow
/// </summary>
    class GravityArrow : AbsGravityProjectile
    {
        Players.Player player;

        public GravityArrow(DamageData givesDamage, Vector3 startPos, Vector3 target,
            GO.Bounds.ObjectBound bound, Players.Player player)
            : base(GoArgs.Empty, givesDamage, startPos, target, bound, 0.04f)
        {
            if (player != null)
            {
                this.player = player;
                callBackObj = player.hero;
            }
            Music.SoundManager.PlaySound(LoadedSound.Bow, startPos);
            NetworkShareObject();
        }
        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);
            //writer.Write((byte)givesDamage.Magic);
        }
        //public GravityArrow(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    givesDamage.Magic = (Magic.MagicElement)r.ReadByte();
        //    Music.SoundManager.PlaySound(LoadedSound.Bow, image.Position);
        //    particleSetup();
        //}

        public static GO.Bounds.ObjectBound ArrowBound(Rotation1D dir)
        {
            const float BoundW = 0.2f;
            return new GO.Bounds.ObjectBound(new LootFest.BoundData2(new VikingEngine.Physics.Box1axisBound(new VectorVolume(Vector3.Zero,
                new Vector3(BoundW, BoundW, 1.1f)), dir), Vector3.Zero));
        }

    
        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
            
            //float xzLen = (float)(Math.Sqrt(Math.Pow(Velocity.Value.X, 2) + Math.Pow(Velocity.Value.Z, 2)));
            //image.Rotation.RotationV3 = new Vector3(-Velocity.Radians(), (float)Math.Atan2(Velocity.Value.Y, xzLen) + MathHelper.Pi, 0);
            //image.Rotation.PointAlongVector(Velocity.Value);
        }

   

        public override GameObjectType Type
        {
            get { return GameObjectType.GravityArrow; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Arrow; }
        }
        protected override float ImageScale
        {
            get { return 2f; }
        }
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Arrow_Slingstone; }
        //}
        override protected bool removeAfterCharColl { get { return true; } }

    }

    class SlingStone : AbsGravityProjectile
    {
        public SlingStone(DamageData givesDamage, Vector3 startPos, Vector3 target,
            GO.Bounds.ObjectBound bound)
            : base(givesDamage, startPos, Ref.rnd.Vector3_Sq(target, 1), bound, 0.034f)//0.035f)
        {
            Music.SoundManager.PlaySound(LoadedSound.Bow, startPos);
            
            //NetworkShareObject();
        }

        
        //public SlingStone(System.IO.BinaryReader r)
        //    : base(r)
        //{
        //    Music.SoundManager.PlaySound(LoadedSound.Bow, image.Position);
        //}
        public override GameObjectType Type
        {
            get { return GameObjectType.SlingStone; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.slingstone; }
        }
        protected override float ImageScale
        {
            get { return 0.8f; }
        }

        public override void Time_Update(UpdateArgs args)
        {
            base.Time_Update(args);
        }
        protected override bool handleCharacterColl(AbsUpdateObj character, Bounds.BoundCollisionResult collisionData, bool usesMyDamageBound)
        {
            return base.handleCharacterColl(character, collisionData, usesMyDamageBound);
        }
        //protected override WeaponTrophyType weaponTrophyType
        //{
        //    get { return WeaponTrophyType.Arrow_Slingstone; }
        //}
        override protected bool removeAfterCharColl { get { return true; } }

        Effects.BouncingBlockColors StoneDamageColors = new Effects.BouncingBlockColors(Data.MaterialType.white, Data.MaterialType.white, Data.MaterialType.white);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return StoneDamageColors;
            }
        }
    }
    
}
