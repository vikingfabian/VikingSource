using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO
{
    class BasicSuit : AbsSuit
    {
        const float SlingReloadTime = 400;
        const float AttackTime = 260;
        const float SwordScale = 0.15f;

        WeaponAttack.DamageData stoneDamage;

        public BasicSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.stick)
        {
            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.BasicSuitAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SwordScale,
                new Vector3(GO.WeaponAttack.HandWeaponAttackSettings.SwordBoundScaleW,
                 HandWeaponAttackSettings.SwordBoundScaleH,
                 HandWeaponAttackSettings.SwordBoundScaleL),
                HandWeaponAttackSettings.StandardScaleToPosDiff,
                 AttackTime,
                HandWeaponAttackSettings.SwordSwingStartAngle,
                HandWeaponAttackSettings.SwordSwingEndAngle, 
                HandWeaponAttackSettings.SwordSwingPercTime,
                  new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                      NetworkId.Empty)
                  );

            //ska vara LfLib.HeroStunDamage när alla fiender har stunn
            stoneDamage = new WeaponAttack.DamageData(LfLib.HeroStunDamage, WeaponUserType.Player, NetworkId.Empty, Magic.MagicElement.Stunn);
        }

        public override void Update(Players.InputMap controller)
        {
            base.Update(controller);
            updateVisualBow();
        }

        
        protected override void UseSpecial()
        {
            stoneBound.MainBound.center = Vector3.Zero;
            Vector3 target = player.hero.GetBowTarget();

            slingshot(target, true);
            var w = netWriteSpecialAttack();
            SaveLib.WriteVector(w, target);
        }

        GO.Bounds.ObjectBound stoneBound = WeaponAttack.GravityArrow.ArrowBound(Rotation1D.D0);
        void slingshot(Vector3 target, bool local)
        {
            var attack = new GO.WeaponAttack.SlingStone(stoneDamage, player.hero.BowFirePos(), target,
                stoneBound);
            attack.bCollisionCheck = local;

            ViewVisualBow(SlingReloadTime, VoxelModelName.slingshot);
        }

        public override void netReadSpecAttack(Network.ReceivedPacket packet)
        {
            //base.netReadSpecAttack(packet);
            Vector3 target = SaveLib.ReadVector3(packet.r);
            slingshot(target, false);
        }

        
        protected override float primaryAttackTime
        {
            get
            {
                return AttackTime;
            }
        }

        public override SuitType Type
        {
            get { return SuitType.Basic; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFBasicSuitIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFBasicSuitIcon2; }
        }
    }
}
