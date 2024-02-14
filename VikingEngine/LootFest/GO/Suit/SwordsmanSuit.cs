using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO
{
    class SwordsmanSuit : AbsSuit
    {
        const float SwordScale = 0.15f;

        public SwordsmanSuit(Players.AbsPlayer player)
            : base(player, VoxelModelName.sword_base)
        {
            shield = new WeaponAttack.Shield(player.hero, player.Storage);
            primaryWeaponAttack = new WeaponAttack.HandWeaponAttackSettings(
                GameObjectType.SwordsManAttack, HandWeaponAttackSettings.SwordStartScalePerc,
                SwordScale,
                new Vector3(GO.WeaponAttack.HandWeaponAttackSettings.SwordBoundScaleW,
                HandWeaponAttackSettings.SwordBoundScaleH,
                HandWeaponAttackSettings.SwordBoundScaleL),
                HandWeaponAttackSettings.StandardScaleToPosDiff, 
                SwordAttackTime,
                HandWeaponAttackSettings.SwordSwingStartAngle,
                HandWeaponAttackSettings.SwordSwingEndAngle,
                HandWeaponAttackSettings.SwordSwingPercTime,
                 new WeaponAttack.DamageData(LfLib.HeroNormalAttack, WeaponAttack.WeaponUserType.Player,
                     player.hero.ObjOwnerAndId)
                 );
        }


        public override void Update(Players.InputMap controller)
        {
            base.Update(controller);
            shield.update();
        }

        protected override Time PrimaryAttack(out float attackAnimFrameTime, bool localUse)
        {
            Time result = base.PrimaryAttack(out attackAnimFrameTime, localUse);
            shield.Hide(result.MilliSeconds + 100);
            return result;
        }

        protected override void UseSpecial()
        {
            //throwing spear
            new WeaponAttack.ItemThrow.Javelin(player.hero);
        }
        public override SuitType Type
        {
            get { return SuitType.Swordsman; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return SpriteName.LFSwordsmanIcon1; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LFSwordsmanIcon2; }
        }

        override public Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[] { 
                Players.HatType.Vendel, Players.HatType.Spartan, Players.HatType.Knight,
                Players.HatType.Cone1, Players.HatType.Cone2, Players.HatType.Cone3, Players.HatType.Cone4, 
            };
        }
    }
}
