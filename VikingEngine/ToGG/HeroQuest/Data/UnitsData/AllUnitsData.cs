using System;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class AllUnitsData
    {
        public static readonly HqUnitType[] EditorReadyUnits = new HqUnitType[]
            {
                HqUnitType.Dummy,
                HqUnitType.Decoy,

                HqUnitType.GoblinRunner,
                HqUnitType.GoblinSoldier,
                HqUnitType.GoblinArcher,
                HqUnitType.GoblinWolfRider,
                HqUnitType.GoblinWolfRiderCommander,
                HqUnitType.GoblinBloated,
                HqUnitType.GoblinBoss,

                HqUnitType.GuardDog,
                HqUnitType.OrcGuard,

                HqUnitType.Beastman,
                HqUnitType.Cyclops,
                HqUnitType.HeavyBeastman,

                HqUnitType.Ogre,
                HqUnitType.CannonTroll,

                HqUnitType.DarkPriest,
                HqUnitType.SkeletonArcher,
                HqUnitType.SkeletonPeasant,
                HqUnitType.SkeletonSoldier,

                HqUnitType.Naga,
                HqUnitType.NagaCommander,
                HqUnitType.NagaBoss,

                HqUnitType.CaveSpider,

                HqUnitType.Firelizard,
                HqUnitType.RabidLizard,
                HqUnitType.Bat,
            };

        public AllUnitsData()
        {
            hqRef.unitsdata = this;
        }

        public HqUnitData Get(HqUnitType type)
        {   
            switch (type)
            {
                //HEROES
                case HqUnitType.RecruitHeroBow:
                    return new RecruitHeroBow();

                case HqUnitType.RecruitHeroSword:
                    return new RecruitHeroSword();

                case HqUnitType.ElfHero:
                    return new ElfHero();

                case HqUnitType.KnightHero:
                    return new KnightHero();

                case HqUnitType.KhajaHero:
                    return new KhajaHero();

                case HqUnitType.DwarfHero:
                    return new DwarfHero();

                //PETS
                case HqUnitType.PetCat:
                    return new PetCat();

                case HqUnitType.PetDog:
                    return new PetDog();

                case HqUnitType.PetRat:
                    return new PetRat();

                //GUARDS
                case HqUnitType.KingsGuardSpearman:
                    return new KingsGuardInfantry();

                case HqUnitType.KingsGuardArcher:
                    return new KingsGuardArcher();

                //STATIC UNITS
                case HqUnitType.Dummy:
                    return new Dummy();

                case HqUnitType.Decoy:
                    return new Decoy();

                case HqUnitType.TrapDecoy:
                    return new TrapDecoy();
                                       
                case HqUnitType.ArmoredDecoy:
                    return new ArmoredDecoy();

                //ENEMIES
                case HqUnitType.GoblinRunner:
                    return new GoblinRunner();

                case HqUnitType.GoblinArcher:
                    return new GoblinArcher();

                case HqUnitType.GoblinSoldier:
                    return new GoblinSoldier();

                case HqUnitType.GoblinWolfRider:
                    return new GoblinWolfRider();

                case HqUnitType.GoblinWolfRiderCommander:
                    return new GoblinWolfRiderCommander();

                case HqUnitType.GoblinBloated:
                    return new GoblinBloated();

                case HqUnitType.GoblinBoss:
                    return new GoblinBoss();

                case HqUnitType.Beastman:
                    return new Beastman();

                case HqUnitType.GuardDog:
                    return new GuardDog();

                case HqUnitType.GoblinGuard:
                    return new GoblinGuard();

                case HqUnitType.GoblinKnightBoss:
                    return new GoblinKnightBoss();
                    
                case HqUnitType.HeavyBeastman:
                    return new HeavyBeastman();

                case HqUnitType.Cyclops:
                    return new Cyclops();

                case HqUnitType.Bat:
                    return new Bat();

                case HqUnitType.Ogre:
                    return new Ogre();

                case HqUnitType.OrcGuard:
                    return new OrcGuard();

                case HqUnitType.CannonTroll:
                    return new CannonTroll();

                case HqUnitType.DarkPriest:
                    return new DarkPriest();

                case HqUnitType.SkeletonArcher:
                    return new SkeletonArcher();

                case HqUnitType.SkeletonPeasant:
                    return new SkeletonPeasant();

                case HqUnitType.SkeletonSoldier:
                    return new SkeletonSoldier();

                case HqUnitType.Naga:
                    return new Naga();

                case HqUnitType.NagaBoss:
                    return new NagaBoss();

                case HqUnitType.NagaCommander:
                    return new NagaCommander();

                case HqUnitType.CaveSpider:
                    return new CaveSpider();

                case HqUnitType.RabidLizard:
                    return new RabidLizzard();

                case HqUnitType.Firelizard:
                    return new FireLizard();

                case HqUnitType.FleshGhoul:
                    return new FleshGhoul();
                
                default:
                    throw new NotImplementedException("hq unit type get: " + type.ToString());
            }

        } 
    }

    
}
