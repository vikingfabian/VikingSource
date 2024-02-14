using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsBarbarian : AbsSuit
    {
        public AbsBarbarian(Players.AbsPlayer user, VoxelModelName loadWeaponModel)
            : base(user, loadWeaponModel)
        { }

        override public Players.BeardType[] availableBeardTypes()
        {
            return new Players.BeardType[] { Players.BeardType.BeardLarge, Players.BeardType.Barbarian1, Players.BeardType.Barbarian2, Players.BeardType.Barbarian3, Players.BeardType.Barbarian4, Players.BeardType.Barbarian5, };
        }
        override public Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[] { 
                Players.HatType.Viking1, Players.HatType.Viking2, Players.HatType.Viking3, Players.HatType.Viking4, 
                Players.HatType.WolfHead,Players.HatType.BearHead,Players.HatType.PoodleHead, 
            };  
        }

        protected override bool canUseSpecial()
        {
            return player.hero.canPerformAction(true);//primaryAttackReloadTime.TimeOut;
        }

        override public float initialJumpForce { get { return 1.3f; } }
        override public float holdJumpMaxTime { get { return 90f; } }
        override public float holdJumpForcePerSec { get { return 0.18f; } }

        override public float MovementAccPerc { get { return StandardMovementAccPerc * 0.9f; } }

        override public float RunningSpeed { get { return StandardRunningSpeed * 1.2f; } }

        //protected override void UseSpecial()
        //{
            
        //}

        //override public bool SecondaryAttackWorksFromMounts
        //{
        //    get { return false; }
        //}
    }
}
