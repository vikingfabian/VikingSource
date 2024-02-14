using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    static class tankLib
    {
        public static float tileScale;
        public static float tankScale;
        public static float bulletScale;
        public static float MaxTankSpeed;
        public static float MinTankPushSpeed;
        public static float MaxTankPushSpeed;

        public static float TurningTankSpeed;

        public static float BulletSpeed;
        
        public static void Init(float _tileScale)
        {
            tileScale = _tileScale;
            tankScale = MathExt.Round(tileScale * 1.4f);
            bulletScale = MathExt.Round(tankScale * 0.5f);

            MaxTankSpeed = tileScale * 6f;
            MinTankPushSpeed = MaxTankSpeed * 0.05f;
            MaxTankPushSpeed = MaxTankSpeed * 1.5f;
            TurningTankSpeed = MaxTankSpeed * 0.75f;
            BulletSpeed = MaxTankSpeed * 2f;
        }

        public const float MaxTurnSpeed = 0.55f * MathExt.Tau;
        public static readonly Time BulletReloadTime = new Time(3f, TimeUnit.Seconds);

        public const ImageLayers LayerBullet = ImageLayers.Lay4;
        public const ImageLayers LayerTank = ImageLayers.Lay5;

        public const ImageLayers LayerTiles = ImageLayers.Lay6;
    }
}
