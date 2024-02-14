using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest
{
    struct CardCaptures
    {
        public int BaseCards, SilverCards, GoldCards;

        //public CardCaptures(System.IO.BinaryReader r, int version)
        //    : this()
        //{
        //    Read(r, version);
        //}
        public CardCaptures(int baseCards)
            :this()
        {
            this.BaseCards = baseCards;
        }

        public CardCaptures AddOne()
        {
            BaseCards++;
            return this;
        }

        public void Write(System.IO.BinaryWriter w)
        {
            w.Write((ushort)BaseCards);
            w.Write((ushort)SilverCards);
            w.Write((ushort)GoldCards);
        }

        public void Read(System.IO.BinaryReader r, int version, int cardType)
        {
            int Base = r.ReadUInt16();
            int Silver = r.ReadUInt16();
            int Gold = r.ReadUInt16();

            if (CardAvailableTypeList[cardType] == CardAvailableType.Collectable)
            {
                this.BaseCards = Base;
                this.SilverCards = Silver;
                this.GoldCards = Gold;
            }

        }

        public int TotalCount
        {
            get { return BaseCards + SilverCards + GoldCards; }
        }

        public static readonly CardAvailableType[] CardAvailableTypeList = new CardAvailableType[]
        {
            CardAvailableType.Collectable,//CritterPig = 0,
            CardAvailableType.Collectable,//CritterHen = 1,
            CardAvailableType.Collectable,//CritterSheep = 2,
            CardAvailableType.Collectable,//Frog = 3,
            CardAvailableType.Collectable,//Lizard,
            CardAvailableType.Collectable,//SpitChick,
            CardAvailableType.Collectable,//Hog,
            CardAvailableType.Collectable,//Pitbull,
            CardAvailableType.Collectable,//FatBird1,
            CardAvailableType.Collectable,//FatBird2,
            CardAvailableType.Collectable,//Bee = 10,
            CardAvailableType.Collectable,//Bat1,
            CardAvailableType.Collectable,//Bat2,
            CardAvailableType.Collectable,//Mummy,
            CardAvailableType.Collectable,//Slime,
            CardAvailableType.Collectable,//Crocodile = 15,
            CardAvailableType.Collectable,//GoblinScout,
            CardAvailableType.Collectable,//GoblinLineman,
            CardAvailableType.Collectable,//GoblinBerserk,
            CardAvailableType.Collectable,//OrcSoldier,
            CardAvailableType.Collectable,//OrcArcher = 20,
            CardAvailableType.Collectable,//OrcKnight,
            CardAvailableType.Collectable,//Zombie,
            CardAvailableType.Collectable,//Harpy,
            CardAvailableType.Collectable,//Skeleton,
            CardAvailableType.Collectable,//ZombieLeader = 25,
            CardAvailableType.Collectable,//GoblinKing,
            CardAvailableType.Collectable,//OldHog,
            CardAvailableType.Collectable,//BirdRiderBoss,
            CardAvailableType.Collectable,//StatueBoss1,
            CardAvailableType.Collectable,//BigOrc,
            CardAvailableType.Collectable,//GoblinWolfRider,
            CardAvailableType.Collectable,//SkeletonBoss,
            CardAvailableType.Collectable,//HogBaby,
            CardAvailableType.Collectable,//Ghost,

            CardAvailableType.NotAvailable,//UnderConstruction = 35,

            CardAvailableType.Collectable,//ElfArcher,
            CardAvailableType.Collectable,//ElfWardancer,
            CardAvailableType.Collectable,//ElfKnight,

            CardAvailableType.Collectable,//OrcWolfRider,

            CardAvailableType.StartDeck,//BaseMinionMana1,
            CardAvailableType.StartDeck,//AttackMinionMana1,
            CardAvailableType.StartDeck,//BaseMinionMana2,
            CardAvailableType.StartDeck,//AttackMinionMana2,
            CardAvailableType.StartDeck,//BaseMinionMana3,
            CardAvailableType.StartDeck,//AttackMinionMana3,
            CardAvailableType.StartDeck,//BaseMinionMana4,
            CardAvailableType.StartDeck,//AttackMinionMana4,
            CardAvailableType.StartDeck,//BaseMinionMana5,
            CardAvailableType.StartDeck,//AttackMinionMana5,
            CardAvailableType.StartDeck,//BaseMinionMana10,
            CardAvailableType.StartDeck,//AttackMinionMana10,

            CardAvailableType.StartDeck,//ScrollMoveUp1,
            CardAvailableType.StartDeck,//ScrollMoveDown1,
            CardAvailableType.StartDeck,//ScrollMoveLeft1,
            CardAvailableType.StartDeck,//ScrollMoveRight1,
            CardAvailableType.StartDeck,//ScrollDamage1,
            CardAvailableType.StartDeck,//ScrollDamage4,
            CardAvailableType.StartDeck,//ScrollDamage10,
            CardAvailableType.StartDeck,//ScrollDamageAll1,
            CardAvailableType.StartDeck,//ScrollDamageAll2,
            CardAvailableType.StartDeck,//ScrollDamageAll5,
            CardAvailableType.StartDeck,//ScrollHeal1,
            CardAvailableType.StartDeck,//ScrollHeal4,
            CardAvailableType.StartDeck,//ScrollDoubleHealth,
            CardAvailableType.StartDeck,//ScrollAttackUp1,
            CardAvailableType.StartDeck,//ScrollAttackUp2,
            CardAvailableType.StartDeck,//ScrollDoubleAttack,
        };
    }
    enum CardAvailableType : byte
    {
        StartDeck,
        Collectable,
        Unlock,
        NotAvailable,
    }
    enum CardType
    {
        CritterPig = 0,
        CritterHen = 1,
        CritterSheep = 2,
        Frog = 3,
        Lizard,
        SpitChick,
        Hog,
        Pitbull,
        FatBird1,
        FatBird2,
        Bee = 10,
        Bat1,
        Bat2,
        Mummy,
        Slime,
        Crocodile = 15,
        GoblinScout,
        GoblinLineman,
        GoblinBerserk,
        OrcSoldier,
        OrcArcher = 20,
        OrcKnight,
        Zombie,
        Harpy,
        Skeleton,
        ZombieLeader = 25,
        GoblinKing,
        OldHog,
        BirdRiderBoss,
        StatueBoss1,
        BigOrc,
        GoblinWolfRider,
        SkeletonBoss,
        HogBaby,
        Ghost,

        UnderConstruction = 35,

        ElfArcher,
        ElfWardancer,
        ElfKnight,

        OrcWolfRider,

        BaseMinionMana1,
        AttackMinionMana1,
        BaseMinionMana2,
        AttackMinionMana2,
        BaseMinionMana3,
        AttackMinionMana3,
        BaseMinionMana4,
        AttackMinionMana4,
        BaseMinionMana5,
        AttackMinionMana5,
        BaseMinionMana10,
        AttackMinionMana10,

        ScrollMoveUp1,
        ScrollMoveDown1,
        ScrollMoveLeft1,
        ScrollMoveRight1,
        ScrollDamage1,
        ScrollDamage4,
        ScrollDamage10,
        ScrollDamageAll1,
        ScrollDamageAll2,
        ScrollDamageAll5,
        ScrollHeal1,
        ScrollHeal4,
        ScrollDoubleHealth,
        ScrollAttackUp1,
        ScrollAttackUp2,
        ScrollDoubleAttack,
        NumNon,
    }

    //enum CardCaptureType
    //{
    //    CritterPig,
    //    CritterHen,
    //    CritterSheep,

    //    Frog,
    //    Lizard,
    //    SpitChick,
    //    Hog,
    //    Pitbull,
    //    FatBird1,
    //    FatBird2,
    //    Bee=10,
    //    Bat1,
    //    Bat2,
    //    Mummy,
    //    Slime,
    //    Crocodile=15,

    //    GoblinScout,
    //    GoblinLineman,
    //    GoblinBerserk,

    //    OrcSoldier,
    //    OrcArcher=20,
    //    OrcKnight,

    //    Zombie,
    //    Harpy,
    //    Skeleton,
    //    ZombieLeader=25,

    //    GoblinKing,
    //    OldSwineBoss,
    //    BirdRiderBoss,
    //    StatueBoss1,
    //    BigOrcBoss=30,
    //    StatueBoss2,
    //    FutureBoss,

    //    HogBaby,
    //    Ghost,
    //    Under_Construction=35,
    //    ElfArcher,
    //    ElfWardancer,
    //    ElfKnight,
    //    NUM_NON,
    //}
}
