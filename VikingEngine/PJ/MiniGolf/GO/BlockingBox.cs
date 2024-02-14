using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf.GO
{
    class BlockingBox : AbsItem
    {
        public BlockingBox(IntVector2 square)
            :base()
        {
            startLayer();
            var area = GolfRef.field.toSquareScreenArea(square);
            image = new Graphics.Image(GolfRef.field.visualSetup.brickTex, area.Position, area.Size, GolfLib.ItemsLayer);

            bound = new Physics.RectangleBound(area.Center, area.HalfSize());
            endLayer();

            GolfRef.objects.Add(this);
        }

        public override void onPickup(Ball b, Physics.Collision2D collision)
        {
            //DeleteMe();
            //new Effect.FourSplitEffect(image);
            takeDamage(new DamageOrigin());
        }

        public override void takeDamage(DamageOrigin origin)
        {
            DeleteMe();
            new Effect.FourSplitEffect(image);
            GolfRef.sounds.softExplosion.Play(image.position);
        }

        override public bool BallObsticle { get { return true; } }
        override public ObjectType Type { get { return ObjectType.BlockingBox; } }
    }
}
