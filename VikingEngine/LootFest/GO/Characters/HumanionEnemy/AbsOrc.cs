using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters
{
    abstract class AbsOrc : AbsHumanoidEnemy
    {
        protected static readonly IntervalF OrcScaleRange = new IntervalF(4f, 4.5f);

        public AbsOrc(GoArgs args, VoxelModelName modelName, IntervalF scaleRange)
            : base(args)
        {
            modelScale = scaleRange.GetRandom();
            createImage(modelName, modelScale, new Graphics.AnimationsSettings(11, 0.8f, 6));
            CollisionAndDefaultBound = LootFest.ObjSingleBound.QuickCylinderBoundFromFeetPos(modelScale * 0.22f, modelScale * 0.45f, 0);
            if (args.LocalMember)
            {
                createAiPhys();
                aiPhys.yAdj = 0;
            }
        }

        protected override void shieldHit(WeaponAttack.DamageData damage, bool local)
        {
            base.shieldHit(damage, local);
            aggresivity--;
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.light_pea_green,
            Data.MaterialType.dark_blue,
            Data.MaterialType.dark_yellow_orange);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return DamageColorsLvl1;
            }
        }
        public override MountType MountType
        {
            get { return MountType.Rider; }
        }
    }
}
