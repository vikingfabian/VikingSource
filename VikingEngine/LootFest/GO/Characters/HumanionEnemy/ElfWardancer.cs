using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Characters.HumanionEnemy
{
    class ElfWardancer : AbsElf
    {
        public ElfWardancer(GoArgs args)
            : base(args, VoxelModelName.elf_wardancer, ElfScaleRange)
        {
            aggresivity = HumanoidEnemyAgressivity.Berserk_4;
            attackRate.Seconds = 0.5f;
            preAttackTime = 300;
            LockedAfterAttackTime = 100;
            elfSword();

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        //public ElfWardancer(System.IO.BinaryReader r)
        //    : base(r)
        //{

        //}
       

        const float BerserkWalkingSpeed = WalkingSpeed * 1.6f;
        override protected float walkingSpeed
        {
            get { return BerserkWalkingSpeed; }
        }
        public override GameObjectType Type
        {
            get { return GameObjectType.ElfWardancer; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.ElfWardancer; }
        }
        public static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pure_pea_green,
            Data.MaterialType.dark_warm_brown,
            Data.MaterialType.pure_red_orange
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }

}

