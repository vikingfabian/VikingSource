using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;
using VikingEngine.LootFest.Players;

namespace VikingEngine.LootFest.GO
{
    class SpearManSuit : AbsSuit
    {
        const float SpearScale = 0.27f;
        const float AttackAngle = -0.1f;
        const float DashAttackTime = 200;
        public const float DashMoveSpeed = 0.1f;
        WeaponAttack.HandWeaponAttackSettings dashAttack;

        public SpearManSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.handspear)
        {
            shield = new WeaponAttack.Shield(user.hero, player.Storage);
            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
               GameObjectType.SpearManAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SpearScale,
                new Vector3(1.8f, HandWeaponAttackSettings.SwordBoundScaleH, 8.5f),//bounds
                new Vector3(3f, 1.4f, 4f), //offset
                380, //att time
                AttackAngle,
                AttackAngle,
                0,
                 new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                     user.hero.ObjOwnerAndId)
                 );

            dashAttack = primaryWeaponAttack;
            dashAttack.scaleToBoundSize.X *= 3f;
            dashAttack.damage.Push = WeaponPush.Huge;
            dashAttack.damage.Special = SpecialDamage.IgnoreShield;
            dashAttack.attackTime += DashAttackTime;
        }

        public override void Update(InputMap controller)
        {
            base.Update(controller);
            shield.update();
            //specialAttackTimer.CountDown();
        }

        protected override void UseSpecial()
        {
            if (player.hero.isMounted)
            {
                new WeaponAttack.ItemThrow.Javelin(player.hero);
            }
            else
            {

                //specialAttackTimer.MilliSeconds = DashAttackTime;
                new WeaponAttack.HandWeaponAttack2(dashAttack, originalWeaponMesh1, player.hero, true);
                Time actionTime = new Time(dashAttack.attackTime + AttackAnimFrameTimeAdd);
                //player.hero.setTimedMainAction(primaryAttackReloadTime);
                player.hero.mainAction = new Players.DashAttackAction(DashAttackTime, actionTime, player.hero);
            }
        }
        public override SuitType Type
        {
            get { return SuitType.SpearMan; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFSpearmanIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFSpearmanIcon2; }
        }
        override public bool SecondaryAttackWorksFromMounts
        {
            get { return true; }
        }

        override protected bool canUseSpecial() { return player.hero.canPerformAction(true); }

        override protected float primaryAttackMovementPerc { get { return 0f; } }

        override public Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[] { 
                Players.HatType.Vendel, Players.HatType.Spartan, Players.HatType.Knight, 
                Players.HatType.Cone1, Players.HatType.Cone2, Players.HatType.Cone3, Players.HatType.Cone4, 
            };
        }
    }
}