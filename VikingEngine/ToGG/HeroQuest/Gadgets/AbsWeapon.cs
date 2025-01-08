using System;
using System.Collections.Generic;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.Data;

namespace VikingEngine.ToGG.HeroQuest.Gadgets
{
    abstract class AbsWeapon : AbsItem
    {
        public const string MixedMeleeProjecSpriteName = "Mixed range weapon";

        public WeaponStats stats;
        public List<AbsSurgeOption> surgeOptions = null;
        protected List<AbsWeaponProperty> properties = null;

        public AbsWeapon(WeaponType weaponType)
            : base(ItemMainType.Weapon, (int)weaponType)
        { }

        public override string Description => throw new NotImplementedException();

        public override List<AbsRichBoxMember> DescriptionAdvanced()
        {            
            List<AbsRichBoxMember> richbox = new List<AbsRichBoxMember>(8);

            string title;

            switch (stats.weaponRange())
            {
                case WeaponRange.MixedMeleeProjectile:
                    title = MixedMeleeProjecSpriteName;
                    break;
                case WeaponRange.Melee:
                    title = "Melee weapon";
                    break;
                case WeaponRange.Projectile:
                    title = "Projectile weapon";
                    break;
                default:
                    title = TextLib.Error;
                    break;
            }
            //if (stats.meleeStrength > 0 && stats.projectileStrength > 0)
            //{
            //    title = MixedMeleeProjecSpriteName;
            //}
            //else if (stats.meleeStrength > 0)
            //{
            //    title = "Melee weapon";
            //}
            //else
            //{
            //    title = "Projectile weapon";
            //}

            richbox.Add(new RbText(title));
            richbox.Add(new RbNewLine());

            int strength = lib.LargestValue(stats.meleeStrength, stats.projectileStrength);
            
            for (int i = 0; i < strength; ++i)
            {
                richbox.Add(new RbImage(SpriteName.cmdDiceAttack));
            }

            if (!IsMelee)
            {
                richbox.Add(new RbNewLine());
                richbox.Add(new RbText("Range " + stats.projectileRange.ToString()));
            }

            if (surgeOptions != null)
            {
                foreach (var surge in surgeOptions)
                {
                    richbox.Add(new RbNewLine());
                    surge.addConvertIcons(richbox);
                    richbox.Add(new RbText(surge.Name));
                }
            }

            return richbox;
        }

        public AttackSettings attackSettings(bool melee)
        {
            return new AttackSettings(stats.Strength(melee), this);
        }

        public override EquipSlots Equip => EquipSlots.MainHand;

        protected bool IsMelee => stats.meleeStrength > stats.projectileStrength;
    }
    
    class StartingBow : AbsWeapon
    {
        public StartingBow()
            :base(WeaponType.StartingBow)
        {
            stats.projectileStrength = 3;
            stats.projectileRange = 4;

            surgeOptions = new List<AbsSurgeOption>
            {
                new PierceSurgeOption(2, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdStartBow; 

        public override string Name => "Soldiers bow";
    }

    class StartingThrowDaggers : AbsWeapon
    {
        //+damage på distance 2
        //bleed eller cripple
        public StartingThrowDaggers()
            : base(WeaponType.StartingThrowDaggers)
        {
            stats.projectileStrength = 2;
            stats.projectileRange = 3;
            stats.meleeStrength = stats.projectileStrength;

            surgeOptions = new List<AbsSurgeOption>
            {
                new Data.Condition.BleedSurgeOption(1, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdThiefDaggersTier1;

        public override string Name => "Thief daggers";
    }

    class ThrowDaggersTier2 : AbsWeapon
    {
        public ThrowDaggersTier2()
            : base(WeaponType.ThrowDaggersTier2)
        {
            stats.projectileStrength = 3;
            stats.projectileRange = 3;
            stats.meleeStrength = stats.projectileStrength;

            surgeOptions = new List<AbsSurgeOption>
            {
                new Data.Condition.BleedSurgeOption(1, 2)
            };
        }

        public override SpriteName Icon => SpriteName.cmdThiefDaggersTier2;

        public override string Name => "Guild daggers";
    }

    class ThrowDaggersPoisionous : AbsWeapon
    {
        public ThrowDaggersPoisionous()
            : base(WeaponType.ThrowDaggersPoisionous)
        {
            stats.projectileStrength = 2;
            stats.projectileRange = 4;
            stats.meleeStrength = stats.projectileStrength;

            surgeOptions = new List<AbsSurgeOption>
            {
                new Data.Condition.PoisionSurgeOption(1, 3)
            };
        }

        public override SpriteName Icon => SpriteName.cmdPosionousDarts;

        public override string Name => "Poisionous darts";
    }

    class ElfBow : AbsWeapon
    {
        public ElfBow()
            :base(WeaponType.ElfBow)
        {
            stats.projectileStrength = 4;
            stats.projectileRange = 5;

            surgeOptions = new List<AbsSurgeOption>
            {
                new PierceSurgeOption(1, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdElfBow; 

        public override string Name => "Elf bow";
    }

    class StartingSword : AbsWeapon
    {
        public StartingSword()
            :base(WeaponType.StartingSword)
        {
            stats.meleeStrength = 4;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(2, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdStartSword;

        public override string Name => "Soldiers sword";
    }

    class KnightSword : AbsWeapon
    {
        public KnightSword()
            :base( WeaponType.KnightSword)
        {
            stats.meleeStrength = 5;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(1, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdKnightSword;

        public override string Name => "Knight sword";
    }

    class BerserkerAxe : AbsWeapon
    {
        public BerserkerAxe()
            :base(WeaponType.BerserkerAxe)
        {
            stats.meleeStrength = 4;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(1, 1),
                new RageSurgeOption(1, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdBerserkerAxe;

        public override string Name => "Berserker axe";
    }

    class Spear : AbsWeapon
    {
        public Spear()
            :base(WeaponType.Spear)
        {
            stats.meleeStrength = 4;
            stats.projectileStrength = 4;
            stats.projectileRange = 2;

            surgeOptions = new List<AbsSurgeOption>
            {
                new PierceSurgeOption(1, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdSpear;

        public override string Name => "Spear";
    }

    class Whip : AbsWeapon
    {
        public Whip()
            :base(WeaponType.Whip)
        {
            stats.meleeStrength = 3;
            stats.projectileStrength = 2;
            stats.projectileRange = 3;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(2, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdWhip;

        public override string Name => "Whip";

        public override EquipSlots Equip => EquipSlots.AnyHand;
    }

    class BaseDagger : AbsWeapon
    {
        public BaseDagger()
            :base(WeaponType.BaseDagger)
        {
            stats.meleeStrength = 2;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(3, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdStartDagger;

        public override string Name => "Dagger";

        public override EquipSlots Equip => EquipSlots.AnyHand;
    }

    class ArcherDagger : AbsWeapon
    {
        public ArcherDagger()
            :base(WeaponType.ArcherDagger)
        {
            stats.meleeStrength = 3;

            surgeOptions = new List<AbsSurgeOption>
            {
                new DamageSurgeOption(2, 1)
            };
        }

        public override SpriteName Icon => SpriteName.cmdArcherDagger;

        public override string Name => "Short sword";

        public override EquipSlots Equip => EquipSlots.AnyHand;
    }

    enum WeaponType
    {
        StartingBow,
        ElfBow,
        StartingSword,
        KnightSword,
        BerserkerAxe,
        Spear,
        Whip,
        BaseDagger,
        ArcherDagger,
        StartingThrowDaggers,
        ThrowDaggersTier2,
        ThrowDaggersPoisionous,
    }
}
