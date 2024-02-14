using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.GO.WeaponAttack.Monster;

namespace VikingEngine.LootFest.GO.Characters
{
    class GoblinBerserk : AbsGoblin
    {
        public GoblinBerserk(GoArgs args)
            : base(args, VoxelModelName.goblin_berserk)
        {
            goblinBoneSword();
            aggresivity = HumanoidEnemyAgressivity.Berserk_4;
            attackRate.Seconds = 0.6f;
            preAttackTime = 300;
            LockedAfterAttackTime = 100;
            //goblinBoneSword();

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        //public GoblinBerserk(System.IO.BinaryReader r)
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
            get { return GameObjectType.GoblinBerserk; }
        }
        override public CardType CardCaptureType
        {
            get { return CardType.GoblinBerserk; }
        }
        new static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.pastel_yellow_green,
            Data.MaterialType.gray_85,
            Data.MaterialType.dark_red
            );

        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }

}
