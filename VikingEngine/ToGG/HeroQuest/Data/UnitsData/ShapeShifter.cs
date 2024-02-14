using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.HeroQuest.Data.UnitsData
{
    class ShapeShifter : HqUnitData
    {
        public ShapeShifter()
            : base()
        {   
            hero = new HeroData(3, 10, HeroClass.ShapeShifter, KillMarkVisuals.GraveStone);

            setShape(Shape.Human);
        }

        void setShape(Shape shape)
        {
            clear();

            switch (shape)
            {
                case Shape.Human:
                    move = 3;
                    startHealth = 8;
                    //defence.set(hqLib.ArmorDie);

                    modelSettings.image = SpriteName.hqHero_Dwarf;
                    modelSettings.modelScale = 0.9f;
                    modelSettings.shadowOffset = -0.04f;
                    modelSettings.facingRight = true;

                    hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,

                            HeroStrategyType.LynxShape,
                            HeroStrategyType.BearShape,
                            HeroStrategyType.RabbitShape,
                            HeroStrategyType.UltimateWerewolf,

                            HeroStrategyType.Rest,        
                        });
                    break;

                case Shape.Rabbit:
                    move = 5;
                    startHealth = 4;
                    //defence.set(hqLib.ArmorDie);

                    modelSettings.image = SpriteName.hamsterPinkWingDown;
                    modelSettings.modelScale = 0.9f;
                    modelSettings.shadowOffset = -0.04f;
                    modelSettings.facingRight = true;

                    hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,
                            HeroStrategyType.Rest,
                        });
                    break;

                case Shape.Lynx:
                    move = 4;
                    startHealth = 7;
                    defence.set(hqLib.DodgeDie);

                    modelSettings.image = SpriteName.cmdUnitElf_LightCavalry;
                    modelSettings.modelScale = 0.9f;
                    modelSettings.shadowOffset = -0.04f;
                    modelSettings.facingRight = true;

                    hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,
                            HeroStrategyType.Rest,
                        });
                    break;

                case Shape.Bear:
                    move = 2;
                    startHealth = 18;
                    defence.set(hqLib.HeavyArmorDie, 2);

                    modelSettings.image = SpriteName.hqCyclops;
                    modelSettings.modelScale = 1.2f;
                    modelSettings.shadowOffset = -0.04f;
                    modelSettings.facingRight = true;

                    hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,
                            HeroStrategyType.Rest,
                        });
                    break;

                case Shape.Werewolf:
                    move = 3;
                    startHealth = 8;
                    defence.set(hqLib.ArmorDie);

                    modelSettings.image = SpriteName.cmdUnitOrc_HeavyWolfRider;
                    modelSettings.modelScale = 1.1f;
                    modelSettings.shadowOffset = -0.04f;
                    modelSettings.facingRight = true;

                    hero.availableStrategies = new StrategyCardDeck(
                        new List<HeroStrategyType>
                        {
                            HeroStrategyType.Advance,
                            HeroStrategyType.Run,
                            HeroStrategyType.Rest,
                        });
                    break;
            }
        }

        void clear()
        {
            defence.clear();
        }

        public override HqUnitType defaultPet()
        {
            //falk
            //får som tauntar o soakar skada
            return HqUnitType.PetDog; 
        }

        public override string heroSelectDesc(out string[] abilities, out HeroDifficulty difficulty)
        {
            difficulty = HeroDifficulty.Complex3;
            abilities = new string[]
                {
                    "Melee specialist",
                    "Multiple roles",
                };
            return "Some lucky people can turn into a massive beast, you turned into your parents";
        }

        public override HqUnitType Type => HqUnitType.ShapeShifter;

        public override string Name => "ShapeShifter";

        enum Shape
        {
            Human,
            Rabbit,
            Lynx,
            Bear,
            Werewolf,
        }
    }
}
