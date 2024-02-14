using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.EnvironmentObj
{
    class MiningSpot : AbsDestuctableEnvironment
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(Color.DarkGray, new Vector3(3, 2, 3));

        public MiningSpot(GoArgs args)
            : base(args)
        {
            WorldPos = args.startWp;
            if (args.LocalMember)
            {
                WorldPos.SetFromTopBlock(1);
            }

            modelScale = 3f;
            image = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.mining_pile, modelScale, 0, false);
            LfRef.modelLoad.PreLoadImage(VoxelModelName.mithril_ingot, false, 0, false);
            image.position = WorldPos.PositionV3;
            image.position.Y -= 0.1f;

            CollisionAndDefaultBound = new Bounds.ObjectBound(Bounds.BoundShape.BoundingBox, image.position, 
                new Vector3(0.5f, 0.8f, 0.5f) * modelScale, Vector3.Zero);

            Health = 3;

            if (args.LocalMember)
            {
                NetworkShareObject();
            }
        }

        protected override bool willReceiveDamage(WeaponAttack.DamageData damage)
        {
            Music.SoundManager.WeaponClink(image.position);

            if (damage.Special != WeaponAttack.SpecialDamage.PickAxe)
            {
                return false;
            }
            return base.willReceiveDamage(damage);
        }

        protected override void handleDamage(WeaponAttack.DamageData damage, bool local)
        {
            if (damage.Special == WeaponAttack.SpecialDamage.PickAxe)
            {
                damage.Damage = 1f;
            }
            base.handleDamage(damage, local);
        }

        protected override void onTookDamage(WeaponAttack.DamageData damage, bool local)
        {
            Effects.EffectLib.DamageBlocks(6, image, DamageColors);
            if (Alive)
            {
                new Effects.DamageFlash(image, immortalityTime.MilliSeconds);
            }
        }

        protected override void DeathEvent(bool local, WeaponAttack.DamageData damage)
        {
            base.DeathEvent(local, damage);

            if (local)
            {
                new GO.PickUp.MiningMithril(new GoArgs(image.position + Vector3.Up * 2f));
                SoundLib.SmallSuccessSound.PlayFlat();
                //LoadedSound.CraftSuccessful
            }
        }

        public override void AsynchGOUpdate(UpdateArgs args)
        {
            //base.AsynchGOUpdate(args);
            SolidBodyCheck(args.allMembersCounter);
        }

        public override GameObjectType Type
        {
            get { return GameObjectType.MiningSpot; }
        }

        public override NetworkShare NetworkShareSettings
        {
            get
            {
                return LootFest.GO.NetworkShare.NoUpdate;
            }
        }

        public override void DeleteMe(bool local)
        {
            base.DeleteMe(local);
        }

        static readonly Effects.BouncingBlockColors DamageColorsLvl1 = new Effects.BouncingBlockColors(
            Data.MaterialType.stones_gray, 
            Data.MaterialType.mossy_stones_gray);
        public override Effects.BouncingBlockColors DamageColors
        { get { return DamageColorsLvl1; } }
    }
}
