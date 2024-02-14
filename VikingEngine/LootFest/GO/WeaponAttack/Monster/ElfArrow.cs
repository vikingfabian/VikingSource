using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class ElfArrow: EnemyGravityProjectile
    {
        public ElfArrow(GoArgs args, Vector3 target)
            : base(args, damage(), args.startPos, target, ProjectileBound())
        {
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        public override void netWriteGameObject(System.IO.BinaryWriter writer)
        {
            base.netWriteGameObject(writer);
        }

        static GO.Bounds.ObjectBound ProjectileBound() { return LootFest.ObjSingleBound.QuickBoundingBox(0.6f); }

        static DamageData damage()
        {
            return new DamageData(LfLib.BasicDamage, WeaponUserType.Enemy, NetworkId.Empty);
        }
        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.elf_arrow; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.ElfArrow; }
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_cyan_blue,
            Data.MaterialType.light_yellow,
            Data.MaterialType.CMYK_Blue);

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
    }
}
