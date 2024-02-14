using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class FireGoblinBall : AbsGravityProjectile
    {
        static readonly IntervalF StartSideSpeed = new IntervalF(0.01f, 0.012f);

        static readonly DamageData DamageLvl1 = new DamageData(LootfestLib.FireGoblinBall, WeaponUserType.Enemy, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Fire);
        static readonly DamageData DamageLvl2 = new DamageData(LootfestLib.FireGoblinBall * LootfestLib.Level2DamageMultiply, WeaponUserType.Enemy, ByteVector2.Zero, Gadgets.GoodsType.NONE, Magic.MagicElement.Fire);
        int areaLevel;

        public FireGoblinBall(Characters.AbsCharacter parent, int areaLevel)
            : base(DamageData.BasicCollDamage, parent.Position + Vector3.Up * 2,
            Map.WorldPosition.V2toV3(Rotation1D.Random.Direction(StartSideSpeed.GetRandom()), 0.01f), null)
        {
            this.areaLevel = areaLevel;
            basicGoblinBallInit();
            NetworkShareObject();
        }
        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            writer.Write((byte)areaLevel);
        }
        public FireGoblinBall(System.IO.BinaryReader r)
            : base(r)
        {
            areaLevel = r.ReadByte();
            basicGoblinBallInit();
        }

        protected void basicGoblinBallInit()
        {
            givesDamage = areaLevel == 0 ? DamageLvl1 : DamageLvl2;
            CollisionBound = LF2.ObjSingleBound.QuickBoundingBox(ImageScale);
        }

        protected override float ImageScale
        {
            get { return 1f; }
        }
        public override void HandleColl3D(VikingEngine.Physics.Collision3D collData, GameObjects.AbsUpdateObj ObjCollision)
        {
            const float HeightRadius = 2;
            WorldPosition = new Map.WorldPosition(image.position);
            WorldPosition.SetFromGroundY(1);
            if (Math.Abs(WorldPosition.WorldGrindex.Y - image.position.Y) < HeightRadius)
            {
                //place fire
                new Elements.Fire(image.position);
            }
            base.HandleColl3D(collData, ObjCollision);
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.FireGoblinBall; }
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.fire_goblin_ball; }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Other; }
        }
        protected override bool LocalDamageCheck
        {
            get
            {
                return true;
            }
        }
        public override Graphics.LightParticleType LightSourceType
        {
            get
            {
                return Graphics.LightParticleType.Fire;
            }
        }
        
        public override float LightSourceRadius
        {
            get
            {
                return 8;
            }
        }
    }
}
