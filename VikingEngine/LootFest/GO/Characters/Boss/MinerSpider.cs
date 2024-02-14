using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.Characters.Monster3;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class MinerSpider : Spider
    {
        public MinerSpider(GoArgs args)
            : base(args)
        {
            Health = 3;
            if (args.LocalMember)
            {
                followTimeRange = new IntervalF(8f, 10f);
                alwaysFullAttension();
                SetAsManaged();

                var rider = new SkeletonBoneThrower(new GoArgs(args.startPos, 0));
                rider.alwaysFullAttension();
                rider.MountAnimal(this);
                rider.SetAsManaged();
            }
        }

        protected override void createSpiderImage()
        {
            createImage(VoxelModelName.miner_spider, 5f, 0, new Graphics.AnimationsSettings(6, 0.8f, 1));
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            base.handleDamage(damage, local);
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        public static readonly Effects.BouncingBlockColors MinerSpiderDamageColors = new Effects.BouncingBlockColors(
           Data.MaterialType.gray_80, 
            Data.MaterialType.gray_90);
        public override Effects.BouncingBlockColors DamageColors
        {
            get
            {
                return MinerSpiderDamageColors;
            }
        }

        public override MountType MountType
        {
            get
            {
                return GO.MountType.Mount;
            }
        }

        protected override Vector3 mountSaddlePos()
        {
            return calcSaddlePos(0.6f, 3f, 1.2f);
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.MinerSpider; }
        }

        const float MinerSpiderWalkingSpeed = 0.018f;
        const float CasualWalkSpeed = MinerSpiderWalkingSpeed * 0.5f;

        override protected float casualWalkSpeed
        {
            get { return CasualWalkSpeed; }
        }
        protected override float walkingSpeed
        {
            get { return MinerSpiderWalkingSpeed; }
        }
    }
}
