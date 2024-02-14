using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf
{
    class Coin : AbsItem
    {
        public Graphics.Image shadow;

        public Coin(Vector2 pos, CoinValue coinValue, bool groundLayer)
            :base()
        {
            if (groundLayer)
            {
                startLayer();
            }

            float scale;
            SpriteName sprite;
            switch (coinValue)
            {
                default:
                    scale = GolfRef.gamestate.CoinScale;
                    sprite = SpriteName.birdCoin1;
                    break;
                case CoinValue.Value10:
                    scale = GolfRef.gamestate.CoinScale * 2f;
                    sprite = SpriteName.birdCoinValue10;
                    break;
            }

            image = new Graphics.Image(sprite, pos,
                new Vector2(scale), GolfLib.ItemsLayer, true);

            bound = new Physics.CircleBound(image.Position, image.Size1D * 0.5f);

            if (PlatformSettings.ViewCollisionBounds)
            {
                boundImage = new Graphics.Image(SpriteName.WhiteArea, bound.Center,
                    bound.HalfSize * 2f, ImageLayers.AbsoluteTopLayer, true);
                boundImage.Color = Color.Red;
                boundImage.Opacity = 0.5f;
            }

            endLayer();
            GolfRef.objects.Add(this);

            if (!groundLayer)
            {
                shadow = image.CloneImage();
                shadow.ChangePaintLayer(+1);
                shadow.ColorAndAlpha(Color.Black, Draw.ShadowOpacity);
                setPosition(pos);
            }
        }

        public void setPosition(Vector2 pos)
        {
            image.position = pos;
            bound.update(pos);

            if (shadow != null)
            {
                shadow.position = VectorExt.Add(pos, GolfRef.gamestate.CoinScale * 0.4f);
            }
        }

        public override void onPickup(Ball b, Physics.Collision2D collision)
        {
            GolfRef.sounds.coin.Play(image.position);

            b.gamer.addScore(b, this, 1);
            VikingEngine.PJ.Particles.CoinParticle.Create(image.Position, image.Size1D);
            DeleteMe();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            shadow?.DeleteMe();
        }

        override public ObjectType Type { get { return ObjectType.Coin; } }
    }

    
}
