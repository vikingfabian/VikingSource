using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG
{
    struct WeaponStats
    {
        public int 
            meleeStrength, 
            projectileStrength, 
            projectileRange, 
            projectileRange2;

        public SlotType meleeSlot;
        public SlotType projectileSlot;


        public void DefaultWeaponSetup()
        {
            meleeStrength = 1;
        }

        public void Merge(AbsItem otherWeapon, SlotType slot)
        {
            var weapon = otherWeapon as AbsWeapon;
            if (weapon != null)
            {
                Merge(weapon.stats, slot);
            }
        }

        public void Merge(WeaponStats otherWeapon, SlotType slot)
        {
            if (otherWeapon.meleeStrength > this.meleeStrength)
            {
                meleeStrength = otherWeapon.meleeStrength;
                meleeSlot = slot;
            }

            if (otherWeapon.projectileStrength > this.projectileStrength)
            {
                projectileStrength = otherWeapon.projectileStrength;
                projectileRange = otherWeapon.projectileRange;
                projectileSlot = slot;
            }
        }

        public int Strength(bool melee)
        {
            return melee ? meleeStrength : projectileStrength;
        }

        public bool HasProjectileAttack
        {
            get { return projectileStrength > 0 && projectileRange > 1; }
        }

        public void strongestAttack(out int strength, out bool melee)
        {
            if (meleeStrength > projectileStrength)
            {
                strength = meleeStrength;
                melee = true;
            }
            else
            {
                strength = projectileStrength;
                melee = false;
            }
        }

        public WeaponRange weaponRange()
        {
            if (meleeStrength > 0 && projectileStrength > 0)
            {
                return WeaponRange.MixedMeleeProjectile;
            }
            else if (meleeStrength > 0)
            {
                return WeaponRange.Melee;
            }
            else if (projectileStrength > 0)
            {
                return WeaponRange.Projectile;
            }
            else
            {
                return WeaponRange.None;
            }
        }
    }

    enum WeaponRange
    {
        None,
        Melee,
        Projectile,
        MixedMeleeProjectile,
    }
}
