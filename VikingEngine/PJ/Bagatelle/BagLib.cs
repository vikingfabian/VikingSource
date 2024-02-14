using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.PJ.Bagatelle
{
    static class BagLib
    {
        public static readonly RandomObjects<PowerUpType> PowerUpChance = new RandomObjects<PowerUpType>(
            new ObjectCommonessPair<PowerUpType>(100, PowerUpType.SplitBall),
            new ObjectCommonessPair<PowerUpType>(100, PowerUpType.MoneySphere),
            new ObjectCommonessPair<PowerUpType>(100, PowerUpType.LargeBump)
        );
        
        public static readonly SpriteSize CoinSprite = new SpriteSize(28, 32);
        public static readonly SpriteSize PegSprite = new SpriteSize(40, 64);
        public static readonly SpriteSize SnakeHeadSprite = new SpriteSize(32, 64);
        public static readonly SpriteSize Coin5And10Sprite = new SpriteSize(46, 64);
        public static readonly SpriteSize Coin20Sprite = new SpriteSize(50, 64);
        public static readonly SpriteSize BumpRefillSprite = new SpriteSize(36, 64);


        public const int BallCount = 5;

        public static readonly int MatchCount =
            PlatformSettings.DevBuild ? 3
            : 3;//NO TOUCHY

        public const ImageLayers PointEffectLayer = ImageLayers.Lay3;
        
        public const ImageLayers BallLayer = ImageLayers.Lay4;
        public const ImageLayers BallParticlesLayer = ImageLayers.Lay5;

        public const ImageLayers MovingItemsLayer = ImageLayers.Lay7;
        public const ImageLayers PegsLayer = ImageLayers.Lay8;
        public const ImageLayers PickupsLayer = ImageLayers.Lay9;
        public const ImageLayers BallTraceLayer = ImageLayers.Background1;

        public static ulong seed;
    }

    enum GameObjectType
    {
        UNKNOWN,
        Peg,
        Coin,
        CoinOutLine,

        BigCoin5,
        BigCoin10,
        BigCoin20,
        BumpRefill,

        CoinPeg,
        Spikes,
        Snake,
        PowerUpBox,
    }

    //PowerUp: split (två sen fyra), stendrop, fallskärm, fireboost (burstar linjärt med hög skada), laser skott, raket upp, penga sphär, större bump radie
    enum PowerUpType
    {
        SplitBall,
        MoneySphere,
        LargeBump,
        NUM_NON
    }
}
