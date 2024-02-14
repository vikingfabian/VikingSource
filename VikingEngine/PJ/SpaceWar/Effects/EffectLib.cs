using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SpaceWar.Effects
{
    static class EffectLib
    {
        public static void SplitModelExplosion(Graphics.Mesh model, IntVector2 splits)
        {
            ForXYLoop loop = new ForXYLoop(splits);
            var tex = model.TextureSource;
            tex.SourceF.Size.X /= 8; //Ska vara splits?
            tex.SourceF.Size.Y /= 8;

            Vector3 pos = model.Position;
            pos.X -= model.Scale.X * 0.5f;
            pos.Z -= model.Scale.Z * 0.5f;

            Vector3 scale = new Vector3( model.Scale.X /  splits.X, 1f, model.Scale.Z / splits.Y);
            pos.X += scale.X * 0.5f;
            pos.Z += scale.Z * 0.5f;

            Vector3 explodeCenter = model.Position;
            explodeCenter.Y -= 0.1f;

            while (loop.Next())
            {
                Vector3 partPos = VectorExt.AddXZ(pos, loop.Position.X * scale.X, loop.Position.Y * scale.Z);
                Vector3 centerDiff = partPos - explodeCenter;

                centerDiff = model.Rotation.TranslateAlongAxis(centerDiff, Vector3.Zero);
                partPos = explodeCenter + centerDiff;

                var part = new Graphics.ParticlePlaneXZ(SpriteName.MissingImage,
                    partPos, VectorExt.V3XZtoV2(scale),
                    
                    VectorExt.SetLength(centerDiff, 
                    Ref.rnd.Plus_MinusPercent(SpaceLib.DriftSpeed * 3f, 0.05f)));

                part.Rotation = model.Rotation;
                part.particleData.setFadeout(Ref.rnd.Float(600, 700), 600);
                part.TextureSource = tex;
                part.TextureSource.SourceF.Position += part.TextureSource.SourceF.Size * loop.Position.Vec;
            }
        }

        public static void BuyItemEffect(Vector3 pos, float radius)
        {
            int count = Ref.rnd.Int(12, 19);
            Vector2 sz = new Vector2(0.25f);

            var dirs = VectorExt.CircleOfDirections(count, Ref.rnd.Rotation(), 1f);

            foreach (var m in dirs)
            {
                var part = new Graphics.ParticlePlaneXZ(SpriteName.WhiteArea_LFtiles, VectorExt.AddXZ(pos, m * radius), sz,
                    VectorExt.V2toV3XZ(m * Ref.rnd.Float(SpaceLib.DriftSpeed * 0.1f, SpaceLib.DriftSpeed * 0.3f)));
                part.Color = GameObjects.ShopSquare.ShopColor;
                part.Opacity = 0.6f;
                part.particleData.setFadeout(Ref.rnd.Float(60, 120), 60);
            }
        }

        public static void CoinEffect(Coin coin)
        {
            int count;
            
            float angleStart = MathExt.TauOver8;
            float lifeTime;

            switch (coin.value)
            {
                default:
                    count = 4;
                    lifeTime = 90;
                    break;

                case CoinValue.Value5:
                case CoinValue.Value10:
                    angleStart = 0;
                    count = 6;
                    lifeTime = 120;
                    break;

                case CoinValue.Value25:
                case CoinValue.Value100:
                    count = 8;
                    lifeTime = 190;
                    break;
            }

            var dirs = VectorExt.CircleOfDirections(count, angleStart, SpaceLib.DriftSpeed * 2f);

            foreach (var m in dirs)
            {
                var part = new Graphics.ParticlePlaneXZ(SpriteName.birdCoinParticleYellow, coin.model.Position,
                    new Vector2(Coin.Scale * 0.6f), VectorExt.V2toV3XZ(m));
                part.particleData.setFadeout(lifeTime, 60);
                part.particleData.velocityPercChange = -0.4f;
            }

            if (coin.value != CoinValue.Value1)
            {
                new Display.SpriteText3D(Coin.GetCoinValue(coin.value).ToString(), VectorExt.AddY(coin.model.Position, -0.1f), Coin.Scale * 0.4f,
                    PjLib.CoinPlusColor).fadeOut(2000, 400);
            }
        }
    }
}
