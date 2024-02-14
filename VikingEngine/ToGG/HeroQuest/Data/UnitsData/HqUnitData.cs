using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest
{
    abstract class HqUnitData : AbsUnitData
    {   
        public HeroData hero = null;
        public DefenceData defence = new DefenceData();
        public List<Gadgets.AbsMonsterWeapon> weapons = null;

        public HqUnitData()
        {   
        }

        public override bool IsProjectileAttackMain()
        {
            if (AoeWeapon(false) != null)
            {
                return true;
            }
            return base.IsProjectileAttackMain();
        }

        public Gadgets.AbsMonsterWeapon AoeWeapon(bool melee)
        {
            if (weapons != null)
            {
                foreach (var m in weapons)
                {
                    if (m.attackArea != null &&
                        melee == m.IsMelee)
                    {
                        return m;
                    }
                }
            }

            return null;
        }

        public ToggEngine.Data.AbsBoardArea AoeAttack(bool melee, out AttackSettings attackSettings)
        {
            attackSettings = null;
            Gadgets.AbsMonsterWeapon weapon = AoeWeapon(melee);
            if (weapon != null)
            {
                return weapon.attackArea;
            }

            if (properties.HasMembers())
            {
                foreach (var m in properties.members)
                {
                    var area = m.AoeAttack(melee, unit, out attackSettings);
                    if (area != null)
                    {
                        return area;
                    }
                }
            }
            return null;
        }

        public override WeaponStats WeaponStats
        {
            get
            {
                if (hero != null && hero.equipment != null)
                {
                    return hero.equipment.weaponStatsMerged();
                }
                else
                {
                    return wep;
                }
            }
        }

        abstract public HqUnitType Type { get; }

        virtual public HqUnitType defaultPet()
        {
            return HqUnitType.Num_None;
        }

        virtual public int QuickBeltSize => 4;

        public override float armorValue()
        {
            return defence.value;
        }

        public override AttackSettings attackSettings(bool melee)
        {
            if (hero != null)
            {
                var weapon = hero.equipment.mainWeapon(melee);
                if (weapon != null)
                {
                   return weapon.attackSettings(melee);
                }
            }
            return base.attackSettings(melee);
        }

        public bool canFillStamina()
        {
            return hero != null && !hero.stamina.IsMax;
        }

        virtual public void startEquipment(Gadgets.Backpack backpack)
        {
            backpack.add(new Gadgets.HealingPotion(), true);
            backpack.add(new Gadgets.StaminaPotion(), true);
        }

        virtual public string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            abilities = null;
            difficulty = HeroDifficulty.Beginner1;
            return "None";
        }

        public bool IsLootCollector()
        {
            return hero != null || properties.HasProperty(UnitPropertyType.LootCollector);
        }

        virtual public float UnitDifficulty => 
            throw new NotImplementedExceptionExt("Unit dif " + Type.ToString(), (int)Type);
    }

    enum HqUnitType
    {        
        Dummy,
        Decoy,
        ElfHero,
        KnightHero,
        PetDog,
        PetCat,

        GoblinArcher,
        Beastman,
        Cyclops,
        HeavyBeastman,

        Firelizard,
        Bat,
        Ogre,
        DarkPriest,
        SkeletonArcher,
        Naga,
        NagaCommander,
        NagaBoss,
        CaveSpider,
        RabidLizard,
        FleshGhoul,
        ArmoredDecoy,
        RecruitHeroSword,
        KhajaHero,
        DwarfHero,
        TrapDecoy,
        PetRat,
        GoblinSoldier,
        GoblinRunner,
        SkeletonPeasant,
        SkeletonSoldier,
        GoblinWolfRider,  
        GoblinWolfRiderCommander,
        GoblinBoss,
        GoblinBloated,
        CannonTroll,
        KingsGuardSpearman,
        KingsGuardArcher,
        GuardDog,
        OrcGuard,
        GoblinKnightBoss,
        GoblinGuard,
        ShapeShifter,
        RecruitHeroBow,
        GreenSlime,
        SmallGreenSlime,

        Num_None,
    }

    enum HeroDifficulty
    {
        Beginner1,
        Normal2,
        Complex3,
        VeryComplex4,
    }
}
