using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Physics;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class FlagPoint : AbsItem
    {
        public FlagPoint(Vector2 pos)
            : base()
        {
            startLayer();
            image = new Graphics.Image(SpriteName.birdCoinValue5, pos,
               new Vector2(GolfRef.gamestate.CoinScale * 1.6f), GolfLib.ItemsLayer, true);

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
        }

        public override void onPickup(Ball b, Collision2D collision)
        {
            //GolfRef.sounds.Coin.Play(image.position, 2f);
            b.FlagCollectSound();
            b.gamer.addScore(b, this, 5);
            DeleteMe();
        }

        override public ObjectType Type { get { return ObjectType.FlagPoint; } }
    }
}
