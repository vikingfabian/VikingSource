using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsFutureSuitGun
    {
        protected VikingEngine.LootFest.GO.Gadgets.GunModel gunModel;
        public LoadedSound sound;
        public float attackRate;
        public bool usesAmmo = true;
        public int Ammo = 0;
        public int MaxAmmo = 0;
        public SpriteName icon = SpriteName.NO_IMAGE;

        virtual public void Fire(Players.AbsPlayer player)
        {
            gunModel.onFire();
        }

        public void DeleteMe()
        {
            gunModel.DeleteMe();
        }
    }

    class SmallSideArm : AbsFutureSuitGun
    {
        public SmallSideArm(Players.AbsPlayer player)
        {
            sound = LoadedSound.deathpop;
            gunModel = new Gadgets.GunModel(VoxelModelName.gunmodel_sidearm, 3.5f, new Vector3(0.8f, 0.5f, 0.5f), player.hero);
            attackRate = 600;
            usesAmmo = false;
            icon = SpriteName.LfGunSideArm;
        }

        public override void Fire(Players.AbsPlayer player)
        {
            base.Fire(player);
            new WeaponAttack.Future.SideArmBullet(player.hero);
        }
    }

    class MashineGun : AbsFutureSuitGun
    {
        public MashineGun(Players.AbsPlayer player)
        {
            sound = LoadedSound.deathpop;
            gunModel = new Gadgets.GunModel(VoxelModelName.gunmodel_mashine, 3.5f, new Vector3(0.8f, 0.5f, 0.5f), player.hero);
            attackRate = 200;
            Ammo = 200;
            MaxAmmo = 200;
            icon = SpriteName.LfGunMashine;
        }

        public override void Fire(Players.AbsPlayer player)
        {
            base.Fire(player);
            new WeaponAttack.Future.MashineGunBullet(player.hero);
        }
    }
}
