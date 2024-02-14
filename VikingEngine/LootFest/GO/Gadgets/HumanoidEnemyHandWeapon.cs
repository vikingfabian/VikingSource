using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.LootFest.GO.WeaponAttack;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Gadgets
{
    class HumanoidEnemyHandWeapon : Process.ILoadImage
    {
        Graphics.VoxelModel originalWeaponMesh;
        public WeaponAttack.HandWeaponAttackSettings settings;
        WeaponAttack.DamageData wepDamage;
        public WeaponAttack.HandWeaponAttack2 attack = null;
        Vector3 preAttackAdjOffset = new Vector3(0, -0.4f, -1f);
        Effects.BouncingBlockColors wepDamageColors;

        public HumanoidEnemyHandWeapon(VoxelModelName model, WeaponAttack.HandWeaponAttackSettings settings, 
            Vector3 preAttackAdjOffset, Effects.BouncingBlockColors wepDamageColors)
        {
            this.preAttackAdjOffset = preAttackAdjOffset;
            wepDamage = new WeaponAttack.DamageData(1, WeaponAttack.WeaponUserType.Enemy, NetworkId.Empty);

            this.settings = settings;
            originalWeaponMesh = LfRef.Images.StandardModel_Sword;
            new Process.LoadImage(this, model, Vector3.Zero);
            this.wepDamageColors = wepDamageColors;
        }

        public void SetCustomImage(Graphics.VoxelModel original, int link)
        {
            this.originalWeaponMesh = original;
        }

        public void PreAttack(GO.Characters.AbsHumanoidEnemy parent)
        {
            settings.damage.UserIndex = parent.ObjOwnerAndId;
            attack = new WeaponAttack.HandWeaponAttack2(settings, originalWeaponMesh, parent, true);
            attack.BeginPreAttack(preAttackAdjOffset);
            attack.damageColors = wepDamageColors;
        }

        /// <returns>Attack time</returns>
        public float Attack()
        {
            //Ref.sound.Play(LoadedSound.FastSwing, originalWeaponMesh.Position);
            //Engine.Sound.PlaySound(LoadedSound.FastSwing, 1);
            if (attack != null)
            {
                attack.EndPreAttack();
                attack = null;
            }
            return settings.attackTime;
        }

        public void Clear()
        {
            if (attack != null)
                attack.DeleteMe();
        }
    }
}
