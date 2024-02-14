using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO
{
    class EmoSuit : AbsSuit
    {
        const float AttackTime = 600;
        const float ReloadTime = 400;
        const float WeaponScale = 0.25f;
        const float BoundScaleH = HandWeaponAttackSettings.SwordBoundScaleH;
        const float BoundScaleW = 4.4f;
        const float BoundScaleL = 14.6f;
        static readonly Vector3 ScaleToPosDiff = new Vector3(3f, 2f, 3.8f);

        const float DashAttackTime = 110;
        public const float DashMoveSpeed = 0.2f;
        WeaponAttack.HandWeaponAttackSettings dashAttack;

        public EmoSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.emo_sword)
        {
            WeaponAttack.DamageData damage = new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                        user.hero.ObjOwnerAndId, Magic.MagicElement.NUM, SpecialDamage.ShieldBreakAttack, false);

            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.DaneAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                WeaponScale,
                new Vector3(BoundScaleW, BoundScaleH, BoundScaleL),
                ScaleToPosDiff,
                AttackTime,
                -2f, HandWeaponAttackSettings.SwordSwingEndAngle, 0.5f,
                damage
                );
            primaryWeaponAttack.damage.Push = WeaponPush.Large;


            const float DashSwordAngle = -3f;
            dashAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.DaneAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                WeaponScale,
                new Vector3(BoundScaleW * 1.4f, BoundScaleH, BoundScaleL),
                new Vector3(-3f, ScaleToPosDiff.Y, -2f),
                AttackTime,
                DashSwordAngle, DashSwordAngle, 0.0f,
                damage
                );
            dashAttack.damage.Push = WeaponPush.Huge;

        }

        protected override void UseSpecial()
        {
            if (player.hero.isMounted)
            {
                new WeaponAttack.ItemThrow.Javelin(player.hero);
            }
            else
            {
                new WeaponAttack.HandWeaponAttack2(dashAttack, originalWeaponMesh1, player.hero, true);

                Time actionTime = new Time(dashAttack.attackTime + AttackAnimFrameTimeAdd);
                player.hero.mainAction = new Players.DashAttackAction(DashAttackTime, actionTime, player.hero);
            }
        }

        override protected bool canUseSpecial() { return player.hero.canPerformAction(true); }
        
        public override SuitType Type
        {
            get { return SuitType.Emo; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LfEmoIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LfEmoIcon2; }
        }

        override protected float primaryAttackTime { get { return AttackTime; } }
        override protected float primaryReloadTime { get { return ReloadTime; } }
        override protected float primaryAttackMovementPerc { get { return 0f; } }
    }
}
