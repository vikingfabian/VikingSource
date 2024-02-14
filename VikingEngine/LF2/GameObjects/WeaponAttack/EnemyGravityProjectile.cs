using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
//

namespace VikingEngine.LF2.GameObjects.WeaponAttack
{
    class HumanoidArrow : EnemyGravityProjectile
    {
        static readonly LF2.ObjSingleBound ProjectileBound = LF2.ObjSingleBound.QuickBoundingBox(0.6f);
        EightBit boost_lvl2;

        public HumanoidArrow(Vector3 startPos, Vector3 target, bool leaderBoost, bool level2, ByteVector2 userIx)
            :base(damage(leaderBoost, level2, userIx), startPos, target, ProjectileBound)
        {
            boost_lvl2 = new EightBit();
            boost_lvl2.Set(0, leaderBoost);
            boost_lvl2.Set(1, level2);
            NetworkShareObject();
        }

        public override void ObjToNetPacket(System.IO.BinaryWriter writer)
        {
            base.ObjToNetPacket(writer);
            
            boost_lvl2.WriteStream(writer);
        }

        public HumanoidArrow(System.IO.BinaryReader r)
            : base(r)
        {
            boost_lvl2 = EightBit.FromStream(r);
            givesDamage = damage(boost_lvl2.Get(0), boost_lvl2.Get(1), ByteVector2.Zero);
            CollisionBound = ProjectileBound;
        }

        static DamageData damage(bool leaderBoost, bool level2, ByteVector2 userIx)
        {
            return Characters.Orc.WeaponDamage(leaderBoost, true, userIx, level2);
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.orc_arrow; }
        }
        public override int UnderType
        {
            get { return (int)WeaponUtype.HumanoidArrow; }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return OrchArrowDamageCols;
            }
        }
    }

    abstract class EnemyGravityProjectile: AbsGravityProjectile
    {
        public EnemyGravityProjectile(DamageData givesDamage, Vector3 startPos, Vector3 target, LF2.ObjSingleBound bound)
            : base(givesDamage, startPos, TargetRandomness(target, 4), bound, 0.04f)
        {
            WorldPosition = new Map.WorldPosition(startPos);
            Music.SoundManager.PlaySound(LoadedSound.EnemyProj1, startPos);
            
        }

        public EnemyGravityProjectile(System.IO.BinaryReader r)
            : base(r)
        { }

        override protected void hitShield(Vector3 shieldPos, Rotation1D heroDir)
        {
            for (int i = 0; i < 5; i++)
            {
                new Effects.BouncingBlock2(shieldPos, Data.MaterialType.wood, 0.16f, heroDir);
            }
        }

        
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.Arrow; }
        }
        protected override float ImageScale
        {
            get { return 2f; }
        }
        protected override bool LocalDamageCheck
        {
            get
            {
                return true;
            }
        }
        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return base.NetworkShareSettings;
            }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Arrow_Slingstone; }
        }
    }
}
