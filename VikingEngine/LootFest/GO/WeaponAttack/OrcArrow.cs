using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.WeaponAttack
{
    class OrcArrow : EnemyGravityProjectile
    {
        public OrcArrow(GoArgs args, Vector3 target)
            : base(args, new DamageData(1f, WeaponUserType.Enemy, NetworkId.Empty), args.startPos, target, LootFest.ObjSingleBound.QuickBoundingBox(0.6f))
        {
            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        protected override VoxelModelName VoxelObjName
        {
            get { return VoxelModelName.orc_arrow; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.OrcArrow; }
        }

        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return OrchArrowDamageCols;
            }
        }
    }

}
