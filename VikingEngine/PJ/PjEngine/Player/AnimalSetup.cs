using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ
{
    struct AnimalSetup
    {
        static readonly Vector2 SheepHatOffset = new Vector2(0.17f, -0.2f);
        static readonly Vector2 PigHatOffset = new Vector2(0.09f, -0.22f);
        static readonly Vector2 BirdHatOffset = new Vector2(0.07f, -0.23f);
        static readonly Vector2 FishHatOffset = new Vector2(0.11f, -0.19f);

        static readonly Vector2 CowHatOffset = new Vector2(0.15f, -0.17f);
        static readonly Vector2 CatHatOffset = new Vector2(0.19f, -0.13f);

        static readonly Vector2 HamsterHatOffset = new Vector2(0.15f, -0.13f);

        static readonly Vector2 DogHatOffset = new Vector2(0.15f, -0.22f);
        static readonly Vector2 PugHatOffset = new Vector2(0.15f, -0.17f);

        public AnimalTheme theme;
        public AnimalMainType mainType;

        public Vector2 hatOffset;
        public SpriteName wingUpSprite;
        public SpriteName wingDownSprite;
        public SpriteName deadSprite;
        public SpriteName featherSprite;
        public SpriteName specialFeatherItem;

        public SpriteName carBody, carBodyTurnL, carBodyTurnR, carHead, carHeadBlink;
        
        static AnimalSetup CarSetup(SpriteName head, SpriteName headBlink, SpriteName arms, SpriteName armsL, SpriteName armsR, AnimalTheme theme)
        {
            AnimalSetup result = new AnimalSetup();

            result.carHead = head;
            result.carHeadBlink = headBlink;
            result.carBody = arms;
            result.carBodyTurnL = armsL;
            result.carBodyTurnR = armsR;

            result.theme = theme;
            
            return result;
        }

        static AnimalSetup JoustSetup(AnimalMainType mainType, Vector2 hatOffset, SpriteName wingUpTile, SpriteName wingDownTile, SpriteName deadTile, SpriteName featherTile,
            AnimalTheme theme, SpriteName specialFeatherItem)
        {
            AnimalSetup result = new AnimalSetup();

            result.mainType = mainType;
            result.hatOffset = hatOffset;
            result.wingUpSprite = wingUpTile;
            result.wingDownSprite = wingDownTile;
            result.deadSprite = deadTile;
            result.featherSprite = featherTile;

            result.theme = theme;
            result.specialFeatherItem = specialFeatherItem;

            return result;
        }

        public static AnimalSetup Get(CarAnimal animal)
        {
            AnimalSetup animalSetup;
            switch (animal)
            {   
                case CarAnimal.ChickenPink:
                    animalSetup = CarSetup(SpriteName.cballBirdPinkHead, SpriteName.cballBirdPinkHeadBlink, SpriteName.cballBirdPinkArms,
                        SpriteName.cballBirdPinkArmsL, SpriteName.cballBirdPinkArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.ChickenYellow:
                    animalSetup = CarSetup(SpriteName.cballBirdYellowHead, SpriteName.cballBirdYellowHeadBlink, SpriteName.cballBirdYellowArms,
                        SpriteName.cballBirdYellowArmsL, SpriteName.cballBirdYellowArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.FishGreen:
                    animalSetup = CarSetup(SpriteName.cballFishGreenHead, SpriteName.cballFishGreenHeadBlink, SpriteName.cballFishGreenArms,
                        SpriteName.cballFishGreenArmsL, SpriteName.cballFishGreenArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.FishOrange:
                    animalSetup = CarSetup(SpriteName.cballFishOrangeHead, SpriteName.cballFishOrangeHeadBlink, SpriteName.cballFishOrangeArms,
                        SpriteName.cballFishOrangeArmsL, SpriteName.cballFishOrangeArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.PigOrange:
                    animalSetup = CarSetup(SpriteName.cballPigOrangeHead, SpriteName.cballPigOrangeHeadBlink, SpriteName.cballPigOrangeArms,
                        SpriteName.cballPigOrangeArmsL, SpriteName.cballPigOrangeArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.PigPink:
                    animalSetup = CarSetup(SpriteName.cballPigPinkHead, SpriteName.cballPigPinkHeadBlink, SpriteName.cballPigPinkArms,
                        SpriteName.cballPigPinkArmsL, SpriteName.cballPigPinkArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.SheepBlack:
                    animalSetup = CarSetup(SpriteName.cballSheepBlackHead, SpriteName.cballSheepBlackHeadBlink, SpriteName.cballSheepBlackArms,
                        SpriteName.cballSheepBlackArmsL, SpriteName.cballSheepBlackArmsR, AnimalTheme.NoTheme);
                    break;
                case CarAnimal.SheepWhite:
                    animalSetup = CarSetup(SpriteName.cballSheepWhiteHead, SpriteName.cballSheepWhiteHeadBlink, SpriteName.cballSheepWhiteArms,
                        SpriteName.cballSheepWhiteArmsL, SpriteName.cballSheepWhiteArmsR, AnimalTheme.NoTheme);
                    break;

                default:
                    throw new IndexOutOfRangeException("GetCarAnimal " + animal.ToString());
            }

            return animalSetup;
        }

        public static AnimalSetup Get(JoustAnimal animal)
        {
            AnimalSetup animalSetup;
            switch (animal)
            {
                case JoustAnimal.Bird1:
                    animalSetup = JoustSetup(AnimalMainType.Bird, BirdHatOffset, SpriteName.birdP1WingUp, SpriteName.birdP1WingDown, 
                        SpriteName.birdP1Dead, SpriteName.birdFeather1, 
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Bird2:
                    animalSetup = JoustSetup(AnimalMainType.Bird, BirdHatOffset, SpriteName.birdP2WingUp, SpriteName.birdP2WingDown, 
                        SpriteName.birdP2Dead, SpriteName.birdFeather2,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Bird3:
                    animalSetup = JoustSetup(AnimalMainType.Bird, BirdHatOffset, SpriteName.birdP3WingUp, SpriteName.birdP3WingDown, 
                        SpriteName.birdP3Dead, SpriteName.birdFeather3,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Bird4:
                    animalSetup = JoustSetup(AnimalMainType.Bird, BirdHatOffset, SpriteName.birdP4WingUp, SpriteName.birdP4WingDown, 
                        SpriteName.birdP4Dead, SpriteName.birdFeather4,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Fish1://Green yellow
                    animalSetup = JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishP1WingUp, SpriteName.fishP1WingDown, 
                        SpriteName.fishP1Dead, SpriteName.fishScale1,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.Fish2://Red
                    animalSetup = JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishP2WingUp, SpriteName.fishP2WingDown, 
                        SpriteName.fishP2Dead, SpriteName.fishScale2,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Fish3: //Green orange
                    animalSetup = JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishP3WingUp, SpriteName.fishP3WingDown, 
                        SpriteName.fishP3Dead, SpriteName.fishScale3,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Fish4://Yellow
                    animalSetup = JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishP4WingUp, SpriteName.fishP4WingDown, 
                        SpriteName.fishP4Dead, SpriteName.fishScale4,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.FishBling:
                    animalSetup = JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishBlingWingUp, SpriteName.fishBlingWingDown, 
                        SpriteName.fishBlingDead, SpriteName.fishBlingScale, 
                        AnimalTheme.Bling, SpriteName.fishBlingGlasses);
                    break;

                case JoustAnimal.Pig1:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigP1WingUp, SpriteName.pigP1WingDown, 
                        SpriteName.pigP1Dead, SpriteName.pigBacon,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Pig2:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigP2WingUp, SpriteName.pigP2WingDown, 
                        SpriteName.pigP2Dead, SpriteName.pigBacon,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Pig3:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigP3WingUp, SpriteName.pigP3WingDown, 
                        SpriteName.pigP3Dead, SpriteName.pigBacon,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Pig4:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigP4WingUp, SpriteName.pigP4WingDown, 
                        SpriteName.pigP4Dead, SpriteName.pigBacon,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.PigBling:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigBlingWingUp, SpriteName.pigBlingWingDown, 
                        SpriteName.pigBlingDead, SpriteName.pigBacon,
                        AnimalTheme.Bling, SpriteName.pigBlingGlasses);
                    break;
                case JoustAnimal.PigVoid:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigVoidWingUp, SpriteName.pigVoidWingDown, 
                        SpriteName.pigVoidDead, SpriteName.pigVoidBacon,
                        AnimalTheme.Void, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.SheepWhite:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepWhiteWingUp, SpriteName.sheepWhiteWingDown, 
                        SpriteName.sheepWhiteDead, SpriteName.sheepFurWhite,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Sheep2:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepP2WingUp, SpriteName.sheepP2WingDown, 
                        SpriteName.sheepP2Dead, SpriteName.sheepFur2,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.SheepPink:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepPinkWingUp, SpriteName.sheepPinkWingDown, 
                        SpriteName.sheepPinkDead, SpriteName.sheepFurPink,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Sheep4:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepP4WingUp, SpriteName.sheepP4WingDown, 
                        SpriteName.sheepP4Dead, SpriteName.sheepFur4,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.Goat:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.goatWingUp, SpriteName.goatWingDown, 
                        SpriteName.goatDead, SpriteName.goatFur,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.SheepRainbow:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepRainbowWingUp, SpriteName.sheepRainbowWingDown, 
                        SpriteName.sheepRainbowDead, SpriteName.sheepRainbowFur,
                        AnimalTheme.Rainbow, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.CowSpotty:
                    animalSetup =JoustSetup(AnimalMainType.Other, CowHatOffset, SpriteName.cowSpottyWingUp, SpriteName.cowSpottyWingDown, 
                        SpriteName.cowSpottyDead, SpriteName.cowSpottySkin,
                        AnimalTheme.NoTheme, SpriteName.cowMeat);
                    break;

                case JoustAnimal.CowBrown:
                    animalSetup =JoustSetup(AnimalMainType.Other, CowHatOffset, SpriteName.cowBrownWingUp, SpriteName.cowBrownWingDown, 
                        SpriteName.cowBrownDead, SpriteName.cowBrownSkin,
                        AnimalTheme.NoTheme, SpriteName.cowMeat);
                    break;

                case JoustAnimal.CatRed:
                    animalSetup =JoustSetup(AnimalMainType.Other, CatHatOffset, SpriteName.catRedWingUp, SpriteName.catRedWingDown, SpriteName.catRedDead, SpriteName.catRedAngel,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.CatBlue:
                    animalSetup =JoustSetup(AnimalMainType.Other, CatHatOffset, SpriteName.catBlueWingUp, SpriteName.catBlueWingDown, SpriteName.catBlueDead, SpriteName.catBlueAngel,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.CatRainbow:
                    animalSetup =JoustSetup(AnimalMainType.Other, CatHatOffset, SpriteName.catRainbowWingUp, SpriteName.catRainbowWingDown, SpriteName.catRainbowDead, SpriteName.catRainbowAngel, 
                        AnimalTheme.Rainbow, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.DogBrown:
                    animalSetup =JoustSetup(AnimalMainType.Other, DogHatOffset, SpriteName.dogBrownWingUp, SpriteName.dogBrownWingDown, 
                        SpriteName.dogBrownDead, SpriteName.dogBone,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.Pug:
                    animalSetup =JoustSetup(AnimalMainType.Other, PugHatOffset, SpriteName.pugWingUp, SpriteName.pugWingDown, 
                        SpriteName.pugDead, SpriteName.dogBone,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.PugVoid:
                    animalSetup =JoustSetup(AnimalMainType.Other, PugHatOffset, SpriteName.pugVoidWingUp, SpriteName.pugVoidWingDown, 
                        SpriteName.pugVoidDead, SpriteName.dogBoneVoid,
                         AnimalTheme.Void, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.HamsterOrange:
                    animalSetup =JoustSetup(AnimalMainType.Other, HamsterHatOffset, SpriteName.hamsterOrangeWingUp, SpriteName.hamsterOrangeWingDown,
                        SpriteName.hamsterOrangeDead, SpriteName.hamsterOrangeFur,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.HamsterPink:
                    animalSetup =JoustSetup(AnimalMainType.Other, HamsterHatOffset, SpriteName.hamsterPinkWingUp, SpriteName.hamsterPinkWingDown,
                        SpriteName.hamsterPinkDead, SpriteName.hamsterPinkFur, AnimalTheme.Rainbow, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.BirdZombie:
                    animalSetup =JoustSetup(AnimalMainType.Bird, BirdHatOffset, SpriteName.birdZombWingUp, SpriteName.birdZombWingDown,
                        SpriteName.birdZombDead, SpriteName.birdFeatherZomb,
                        AnimalTheme.Zombie, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.PigZombie:
                    animalSetup =JoustSetup(AnimalMainType.Other, PigHatOffset, SpriteName.pigZombWingUp, SpriteName.pigZombWingDown,
                        SpriteName.pigZombDead, SpriteName.pigBaconZomb,
                        AnimalTheme.Zombie, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.FishZombie:
                    animalSetup =JoustSetup(AnimalMainType.Other, FishHatOffset, SpriteName.fishZombWingUp, SpriteName.fishZombWingDown,
                        SpriteName.fishZombWingUp, SpriteName.fishBoneZomb,                        
                        AnimalTheme.Zombie, SpriteName.NO_IMAGE);
                    break;
                case JoustAnimal.SheepZombie:
                    animalSetup =JoustSetup(AnimalMainType.Sheep, SheepHatOffset, SpriteName.sheepZombWingUp, SpriteName.sheepZombWingDown,
                        SpriteName.sheepZombDead, SpriteName.sheepFurZomb,
                        AnimalTheme.Zombie, SpriteName.NO_IMAGE);
                    break;

                case JoustAnimal.MrW:
                    animalSetup = JoustSetup(AnimalMainType.Other, SheepHatOffset, SpriteName.mrwWingUp, SpriteName.mrwWingDown,
                        SpriteName.mrwWingDead, SpriteName.sheepFurWhite,
                        AnimalTheme.NoTheme, SpriteName.NO_IMAGE);
                    break;

                default:
                    if (animal >= JoustAnimal.MeatPie1 && animal <= JoustAnimal.MeatPie18)
                    {
                        const int PieArtCount = 6;
                        int ix = (int)(animal - JoustAnimal.MeatPie1);
                        ix %= PieArtCount;

                        animalSetup = JoustSetup(AnimalMainType.Other, SheepHatOffset, 
                            Graphics.GraphicsLib.SumSpriteName( SpriteName.meatpie1, ix), 
                            SpriteName.NO_IMAGE,  SpriteName.NO_IMAGE, SpriteName.NO_IMAGE,
                            AnimalTheme.MeatPie, SpriteName.NO_IMAGE);
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("GetJoustAnimal " + animal.ToString());
                    }
                    break;
            }
            return animalSetup;
        }

        public SpriteName PlayerFrame
        {
            get
            {
                switch (theme)
                {
                    default:
                        return SpriteName.birdPlayerFrame;
                    case AnimalTheme.Bling:
                        return SpriteName.birdPlayerFrameBling;
                    case AnimalTheme.Void:
                        return SpriteName.birdPlayerFrameVoid;
                    case AnimalTheme.Rainbow:
                        return SpriteName.birdPlayerFrameRainbow;
                    case AnimalTheme.Zombie:
                        return SpriteName.birdPlayerFrameZomb;
                }
            }
        }
    }

    enum JoustAnimal
    {
        Bird1,
        Bird2,
        Bird3,
        Bird4,
        BirdBling,

        Pig1,
        Pig2,
        Pig3,
        Pig4,
        PigBling,
        PigVoid,

        Fish1,
        Fish2,
        Fish3,
        Fish4,
        FishBling,

        SheepWhite,
        Sheep2,
        SheepPink, //Pink
        Sheep4,
        SheepRainbow,
        Goat,

        DogBrown,
        Pug,
        PugVoid,

        CowSpotty,
        CowBrown,
        
        HamsterOrange,
        HamsterPink,

        CatRed,
        CatBlue,
        CatRainbow,

        BirdZombie,
        PigZombie,
        FishZombie,
        SheepZombie,

        MrW,
        MeatPie1,
        MeatPie2,
        MeatPie3,
        MeatPie4,
        MeatPie5,
        MeatPie6,
        MeatPie7,
        MeatPie8,
        MeatPie9,
        MeatPie10,
        MeatPie11,
        MeatPie12,
        MeatPie13,
        MeatPie14,
        MeatPie15,
        MeatPie16,
        MeatPie17,
        MeatPie18,


        NUM_NON,
    }

    enum CarAnimal
    {
        PigPink,
        PigOrange,
        FishGreen,
        FishOrange,
        ChickenYellow,
        ChickenPink,
        SheepWhite,
        SheepBlack,

        NUM_NON,
    }

    enum AnimalMainType
    {
        Bird,
        Sheep,
        Other,
    }

    enum Hat
    {
        NoHat,
        
        BlingCap,
        King,
        Princess,
        Riddler,
        Shades,
        HeartEyes,
        UniHorn,
        HighRainbow,

        High,
        Viking,
        Fez,
        Pirate,
        Cowboy,
        Indian,
        Vlc,
        Halo,
        RobinHood,
        ChildCap,
        English,
        GooglieEyes,
        SkyMask,
        Bow,
        Bunny,
        Butterix,
        Scotish,
        Frank,

        NUM_NON,
    }

    enum AnimalTheme
    {
        NoTheme,
        Bling,
        Void,
        Rainbow,
        Zombie,
        MeatPie,
    }
}
