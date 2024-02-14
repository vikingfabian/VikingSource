using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.HeroQuest.Gadgets;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    abstract class AbsRecruitHero : HqUnitData
    {
        bool meleeSpecialist;

        public AbsRecruitHero(bool meleeSpecialist)
           : base()
        {
            this.meleeSpecialist = meleeSpecialist;

            hero = new HeroData(4, 10, HeroClass.Recruit, KillMarkVisuals.GraveStone);

            if (meleeSpecialist)
            {
                move = 3;
                startHealth = 11;

                defence.set(hqLib.ArmorDie);
                modelSettings.image = SpriteName.hqHero_Recruit_Sword;
            }
            else
            {
                move = 4;
                startHealth = 9;

                defence.set(hqLib.DodgeDie);
                modelSettings.image = SpriteName.hqHero_Recruit_Bow;
            }

            modelSettings.modelScale = 0.9f;
            modelSettings.shadowOffset = -0.04f;
            modelSettings.facingRight = true;

            hero.availableStrategies = new StrategyCardDeck(
                new List<HeroStrategyType>
                {
                    HeroStrategyType.Advance,
                    HeroStrategyType.Fight,
                    HeroStrategyType.Run,
                    HeroStrategyType.Rest,
                    HeroStrategyType.UltimateHeroicCry,
                });
        }

        public override void startEquipment(Backpack backpack)
        {
            base.startEquipment(backpack);
            backpack.add(new PheonixPotion(), true);
            //backpack.add(new ThrowingKnife(), true);
            //backpack.add(new StartingSword(), true);
            //backpack.add(new ShieldRound(), true);
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Beginner1;
            abilities = new string[]
                {
                    "Beginner friendly",
                    meleeSpecialist? LanguageLib.MeleeSpecialist : LanguageLib.ProjectileSpecialist,
                    "Extra gadgets",
                };
            return "This young brat is arriving fresh from the hero academy, and hasn't done an honest day of work in his life.";
        }

        

       // public override HqUnitType Type => HqUnitType.RecruitHero;
        
    }

    class RecruitHeroSword : AbsRecruitHero
    {
        public RecruitHeroSword()
            :base(true)
        { }

        public override void startEquipment(Backpack backpack)
        {
            base.startEquipment(backpack);
            //backpack.add(new PheonixPotion(), true);
            //backpack.add(new ThrowingKnife(), true);
            backpack.add(new ThrowingKnife(), true);
            backpack.add(new StartingSword(), true);
            backpack.add(new ShieldRound(), true);
        }

        public override HqUnitType defaultPet()
        {
            return HqUnitType.PetDog;
        }

        public override HqUnitType Type => HqUnitType.RecruitHeroSword;

        public override string Name => "Recruit swordsman";

    }
    class RecruitHeroBow: AbsRecruitHero
    {
        public RecruitHeroBow()
            : base(false)
        { }

        public override void startEquipment(Backpack backpack)
        {
            base.startEquipment(backpack);

            backpack.add(new Gadgets.Decoy(), true);
            backpack.add(new StartingBow(), true);
            backpack.add(new SpecialArrow(ArrowType.Arrow, ArrowSpecialType.Piercing2, 2), false);
        }

        public override HqUnitType defaultPet()
        {
            return HqUnitType.PetCat;
        }

        public override HqUnitType Type => HqUnitType.RecruitHeroBow;

        public override string Name => "Recruit bowman";

    }

    class ElfHero : HqUnitData
    {
        public ElfHero()
            :base()
        {
            move = 4;
            startHealth = 12;

            defence.set(hqLib.DodgeDie);
            hero = new HeroData(4, 10, HeroClass.Archer, KillMarkVisuals.Flower);
            
            modelSettings.image = SpriteName.cmdHero_Archer;
            modelSettings.modelScale = 0.9f;
            modelSettings.shadowOffset = -0.1f;
            modelSettings.facingRight = true;

            hero.availableStrategies = new StrategyCardDeck(
                new List<HeroStrategyType>
                {
                    HeroStrategyType.Advance,
                    HeroStrategyType.AimedShot,
                    HeroStrategyType.LineOfFire,
                    HeroStrategyType.ArrowRain,
                    HeroStrategyType.Run,
                    HeroStrategyType.Rest,
                    HeroStrategyType.UltimatePiercingShot,
                });

            /*
             * ArrowRain follow up
             * "reload" move, push away all adjacent units action, gain charged load2 
             */
        }

        public override void startEquipment(Backpack backpack)
        {
            base.startEquipment(backpack);
            backpack.add(new StartingBow(), true);
            backpack.add(new BaseDagger(), true);
            //backpack.add(new Gadgets.ThrowingKnife(), true);
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Normal2;
            abilities = new string[]
                {
                    LanguageLib.ProjectileSpecialist,
                    "Fast moving",
                };
            return "Only walks down a smelly dungeon to afford an expensive lifestyle, of beauty products and parties.";
        }

        public override HqUnitType defaultPet()
        {
            return HqUnitType.PetCat;
        }

        public override HqUnitType Type => HqUnitType.ElfHero;
        public override string Name => "Elf Hero";
    }

    class KhajaHero : HqUnitData
    {
        public KhajaHero()
            : base()
        {
            move = 4;
            startHealth = 7;

            defence.set(hqLib.DodgeDie);
            hero = new HeroData(5, 10, HeroClass.Thief, KillMarkVisuals.Rose);

            modelSettings.image = SpriteName.hqHero_KhajaF;
            modelSettings.modelScale = 0.9f;
            modelSettings.shadowOffset = -0.1f;
            modelSettings.facingRight = false;
            //leap attack, jump 2-3 squares, damage units passed
            //stunn bomb, stunn/poision area
            //trapper, remove/place trap (damage/immobile trap, invisible)
            //hook shot attack
            //Nån shape attack, kanske alla diagonala targets, eller damage2 in all 8 dir

            //attack from the shadows
            //Overwatch: wep strength damage to all enemies move in sight
            //ultimate: movement *3, loot all action

            // cape som minskar unit alert 


            hero.availableStrategies = new StrategyCardDeck(
                new List<HeroStrategyType>
                {
                    HeroStrategyType.FromTheShadows,
                    HeroStrategyType.Trapper,
                    HeroStrategyType.PoisionBomb,
                    HeroStrategyType.StunBomb,
                    HeroStrategyType.RunAndHide,
                    HeroStrategyType.KnifeDance,
                    HeroStrategyType.LeapAttack,
                    HeroStrategyType.Rest,
                    HeroStrategyType.UltimateLootrun,
                });
        }

        public override void startEquipment(Backpack backpack)
        {
            //base.startEquipment(backpack);
            backpack.add(new StartingThrowDaggers(), true);
            backpack.add(new ApplePotion(), true);

            //backpack.add(new ProtectionRune(), true);
            //backpack.add(new ProtectionRune(), true);
            //backpack.add(new RougeTrapItem(), true);
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Complex3;
            abilities = new string[]
                {
                    "Fast moving",
                    "Low defence",
                    "Rouge skills",
                };
            return "Tell me again, why we bring a thief with us? now all the loot has gone missing again!";
        }

        public override HqUnitType defaultPet()
        {
            //Rat, picks loot, send items adj to rat, (bag of herbs) +4 health resting next to
            return HqUnitType.PetRat;
        }

        override public int QuickBeltSize => 5;
        public override HqUnitType Type => HqUnitType.KhajaHero;
        public override string Name => "Khaja Hero";
    }

    class KnightHero : HqUnitData
    {
        public KnightHero()
            : base()
        {
            move = 3;
            startHealth = 14;

            defence.set(hqLib.ArmorDie);
            hero = new HeroData(3, 8, HeroClass.Knight, KillMarkVisuals.Sword);

            modelSettings.image = SpriteName.cmdHero_Knight;
            modelSettings.modelScale = 0.9f;
            modelSettings.facingRight = true;
            modelSettings.shadowOffset = -0.04f;

            hero.availableStrategies = new StrategyCardDeck(
                new List<HeroStrategyType>
                {
                    HeroStrategyType.Advance,
                    HeroStrategyType.Fight,
                    HeroStrategyType.Swing3,
                    HeroStrategyType.Run,
                    HeroStrategyType.Rest,
                    HeroStrategyType.UltimateEarthShake
                });

            /*
             * Swing3 follow up
             * "flying strike" attack all enemies in a straight line, pierce2, and move through them  
             */
        }

        public override void startEquipment(Backpack backpack)
        {
            base.startEquipment(backpack);
            backpack.add(new StartingSword(), true);
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Normal2;
            abilities = new string[]
                {
                    LanguageLib.MeleeSpecialist,
                    "Good defence",
                    "Well-balanced",
                };
            return "From a culture of fart jokes, and crying openly over one-sided love - comes this feared opponent.";
        }

        public override HqUnitType defaultPet()
        {
            return HqUnitType.PetDog;
        }

        public override HqUnitType Type => HqUnitType.KnightHero;
        public override string Name => "Knight Hero";
    }

    class DwarfHero : HqUnitData
    {
        //Dwarf pet: Stonebelt - knuffar valfria units, ger adj heroes defence?
        //Shape attack: meele två som står direct motsatt sida om dvärgen
        //Dwarf toss: friendlies kan flytta dvärgen som action
        //blitz attack: attacks has follow up and may attack again
        //follow up: will take the place of a destroyed unit
        public DwarfHero()
            : base()
        {
            move = 2;
            startHealth = 14;

            defence.set(hqLib.HeavyArmorDie);
            hero = new HeroData(3, 6, HeroClass.Dwarf, KillMarkVisuals.GraveStone);

            modelSettings.image = SpriteName.hqHero_Dwarf;
            modelSettings.modelScale = 0.8f;
            modelSettings.facingRight = true;
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Complex3;
            abilities = new string[]
                {
                    "Slow moving",
                    "High defence",
                    "High damage",
                };
            return "Through the eons they have fought of invading forces of goblins, titans and deamons - however, their true nemesis will always be the top shelf.";
        }

        public override HqUnitType defaultPet()
        {
            return HqUnitType.PetDog;
        }

        public override HqUnitType Type => HqUnitType.DwarfHero;
        public override string Name => "Dwarf Hero";
    }
}
