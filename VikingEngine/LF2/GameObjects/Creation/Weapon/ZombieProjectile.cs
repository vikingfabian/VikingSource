using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Net;
using VikingEngine.LootFest.GameObjects.WeaponAttack;

namespace VikingEngine.LootFest.Creation.Weapon
{
    abstract class ZombieProjectile : GameObjects.WeaponAttack.Linear3DProjectile
    {
        Vector3 rotationSpeed = new Vector3(0.2f, 0, 0);
        static readonly LootFest.ObjSingleBound ProjectileBound = LootFest.ObjSingleBound.QuickBoundingBox(0.6f);

        public ZombieProjectile(PacketReader r)//, GameObjects.Weapons.DamageData damage, GameObjects.Weapons.DamageData damageBoosted)
            : base(r, ProjectileBound)
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, image.Position);
        }

        public ZombieProjectile(GameObjects.WeaponAttack.DamageData givesDamage,
            Vector3 startPos, Vector3 target, float aimDiffAngle, float startSpeed, float lifeTime)
            : base(givesDamage, startPos, target, aimDiffAngle, ProjectileBound, startSpeed)
        {
            Music.SoundManager.PlaySound(LoadedSound.Sword1, image.Position);
            this.lifeTime = lifeTime;
        }
        public override void  Time_Update(GameObjects.UpdateArgs args)
        {
 	        base.Time_Update(args);
            rotate();
        }
        //public override void ClientTimeUpdate(float time, List<LootFest.GameObjects.AbsUpdateObj> args.localMembersCounter, List<LootFest.GameObjects.AbsUpdateObj> active)
        //{
        //    base.ClientTimeUpdate(time, args.localMembersCounter, active);
        //    rotate();
        //}

        void rotate()
        {
            image.Rotation.RotateWorld(rotationSpeed);
        }
        protected override GameObjects.NetworkClientRotationUpdateType NetRotationType
        {
            get
            {
                return GameObjects.NetworkClientRotationUpdateType.NoRotation;
            }
        }
        protected override bool LocalDamageCheck
        {
            get
            {
                return true;
            }
        }
        protected override WeaponTrophyType weaponTrophyType
        {
            get { return WeaponTrophyType.Other; }
        }
    }
}
