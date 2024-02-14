using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class PowerUpBox : AbsGameObject
    {
        public PowerUpBox(Vector2 pos, int networkId, BagatellePlayState state)
            : base(networkId, state)
        {
            image = new Graphics.Image(SpriteName.birdQuestionBox, pos,
               new Vector2(state.CoinScale * 1.4f), ImageLayers.AbsoluteBottomLayer, true);
            setPickUpLayer();
            bound = new Physics.RectangleBound(image.Position, image.Size * 0.5f);

            solidBound = false; pickupType = true;
            createShadow();
        }

        public override void PickUpEvent(AbsGameObject collectingItem, LocalGamer gamer)
        {
            Ball b = collectingItem.GetBall();
            if (b != null && b.alive)
            {
                b.onPowerUpPickUp(this);
                DeleteMe(true);

                beginNetWriteItemStatus(null);
            }
        }

        public override void netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
            base.netReadItemStatus(affectingItem, r);
            DeleteMe(false);
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            PickUpEvent(ball, gamer);
        }

        public override void update()
        {
            base.update();
            updateShadow();
        }
    }
}
