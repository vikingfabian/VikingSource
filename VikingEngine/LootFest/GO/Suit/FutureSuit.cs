using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.GO.WeaponAttack;

namespace VikingEngine.LootFest.GO
{
    class FutureSuit: AbsSuit
    {
        const float SwordScale = 0.15f;
        AbsFutureSuitGun gun;

        public FutureSuit(Players.AbsPlayer user)
            : base(user, VoxelModelName.sword_base)
        {
            
            gun = new MashineGun(player);
           
        }

        public override Players.HatType[] availableHatTypes()
        {
            return new Players.HatType[]
            {
                Players.HatType.Future1, Players.HatType.FutureMask1, Players.HatType.FutureMask2, Players.HatType.GasMask,
            };
        }

        public override void Update(Players.InputMap controller)
        {
            base.Update(controller);
        }

        protected override Time PrimaryAttack(out float attackAnimFrameTime, bool localUse)
        {
            if (localUse)
            {
                gun.Fire(player);
            }
            attackAnimFrameTime = 0;

            return new Time(gun.attackRate);
        }

        protected override bool spendMainAttackAmmo()
        {
            if (gun.usesAmmo && gun.Ammo-- <= 0)
            {
                gun.DeleteMe();
                gun = new SmallSideArm(player);
            }

            ((Players.Player)player).statusDisplay.primaryAttackHUD.UpdateAmount(gun.Ammo, gun.MaxAmmo);

            return base.spendMainAttackAmmo();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            gun.DeleteMe();
        }

        protected override void UseSpecial()
        {
            //throwing spear
            new WeaponAttack.Future.HandGranade(player.hero);
        }

        protected override LoadedSound mainAttackSound
        {
            get
            {
                return gun.sound;
            }
        }

        public override SuitType Type
        {
            get { return SuitType.FutureSuit; }
        }
        public override SpriteName PrimaryIcon
        {
            get { return gun.icon; }
        }
        public override SpriteName SpecialAttackIcon
        {
            get { return SpriteName.LfHandgranade; }
        }

        public override int MaxMainAmmo
        {
            get
            {
                return 200;
            }
        }
    }
}

