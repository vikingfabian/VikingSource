using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest
{
    static class hqLib
    {
        public const int MaxPlayers = 4;

        public const int BloodRageForKill = 2;
        public const int BloodRageForDamage = 1;


        public static BattleDice BattleDie;
        public static BattleDice DodgeDie;
        public static BattleDice ArmorDie;
        public static BattleDice HeavyArmorDie;

        public const int AddPetsToPlayerCount = 2;

        public static readonly Damage WallHitDamage = Damage.BellDamage(1);
        
        public static readonly QuestName[] AvailableMissions = new QuestName[]
            {
                QuestName.CyclopsBoss,

                QuestName.StoryGoblinFoodSteal,
                QuestName.StoryGoblinFoodBoss,
                QuestName.NagaBoss,
                QuestName.StoryGoblinCounter,
                QuestName.StoryGoblinCastle,
            };

        public static void Init()
        {
            BattleDie = new BattleDice(BattleDiceType.Attack, LanguageLib.BattleDie);
            {
                BattleDie.sides = new List<BattleDiceSide>
                {
                    new BattleDiceSide(0.20f,  BattleDiceResult.Hit1),
                    new BattleDiceSide(0.20f,  BattleDiceResult.Hit2),
                    //new BattleDiceSide(0.14f,  BattleDiceResult.Hit3),
                    new BattleDiceSide(0.10f,  BattleDiceResult.CriticalHit),

                    //Genomsnitt 0.70 hits 
                    new BattleDiceSide(0.20f,  BattleDiceResult.Surge1),
                    new BattleDiceSide(0.05f,  BattleDiceResult.Surge2),
                };
                BattleDie.AddNoneResult();
            }

            DodgeDie = new BattleDice(BattleDiceType.Dodge, LanguageLib.DefenceDodge);
            {
                DodgeDie.sides = new List<BattleDiceSide>
                {
                    new BattleDiceSide(0.1f,  BattleDiceResult.Avoid),
                    new BattleDiceSide(0.1f,  BattleDiceResult.AvoidRanged),
                    new BattleDiceSide(0.3f,  BattleDiceResult.Block1),
                };
                DodgeDie.AddNoneResult();
                DodgeDie.value = 3f;
                DodgeDie.icon = SpriteName.cmdArmorDodge;
            }

            ArmorDie = new BattleDice(BattleDiceType.Armor, LanguageLib.DefenceArmor);
            {
                ArmorDie.sides = new List<BattleDiceSide>
                {
                    new BattleDiceSide(0.3f,  BattleDiceResult.Block1),
                    new BattleDiceSide(0.1f,  BattleDiceResult.Block2),
                };
                ArmorDie.AddNoneResult();
                ArmorDie.value = 0.5f;
                ArmorDie.icon = SpriteName.cmdArmorLight;
            }

            HeavyArmorDie = new BattleDice(BattleDiceType.HeavyArmor, LanguageLib.DefenceHeavyArmor);
            {
                HeavyArmorDie.sides = new List<BattleDiceSide>(2);
                HeavyArmorDie.sides.Add(new BattleDiceSide(0.5f, BattleDiceResult.Block2));
                HeavyArmorDie.sides.Insert(0, new BattleDiceSide(HeavyArmorDie.noneChance(), BattleDiceResult.Block1));
                HeavyArmorDie.value = 1.5f;
                HeavyArmorDie.icon = SpriteName.cmdArmorHeavy;
            }
        }

        public static string QuestTitle(QuestName quest)
        {
            switch (quest)
            {
                case QuestName.CyclopsBoss: return "Cyclops (Tutorial)";
                case QuestName.StoryGoblinFoodSteal: return "Protect food (Story)";
                case QuestName.StoryGoblinFoodBoss: return "Food boss (Story)";
                case QuestName.StoryGoblinCounter: return "Goblin counter (Story)";
                case QuestName.StoryGoblinCastle: return "Uncles' Castle (Story)";
                case QuestName.NagaBoss: return "Naga (Side mission)";

                default:
                    return quest.ToString();
            }
        }

        //public static void
    }

    enum LootLevel
    {
        Level1,
        Level2,
        Level3,
        NUM_NONE
    }

    enum DoomChestLevel
    {
        Bronze,
        Silver,
        Gold,
        NUM
    }

    enum QuestName
    {
        None,
        Custom,

        TutorialPractice,
        TutorialExam,

        StoryGoblinFoodSteal,
        StoryGoblinFoodBoss,
        StoryGoblinCounter,
        StoryGoblinCastle,
        NagaBoss,
        CyclopsBoss,
        testchamber,
    }
}
