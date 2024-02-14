using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class SnakeHead : AbsGameObject
    {
        ItemMover mover;

        public SnakeHead(Vector2 pos, ItemMover mover, int networkId, int dir, BagatellePlayState state)
            : base(networkId, state)
        {
            this.mover = mover;
            float boundScale = state.PegScale * 1.3f;
            float scale = BagLib.SnakeHeadSprite.toImageSize(boundScale);

            image = new Graphics.Image(SpriteName.bagSnakeHead, pos,
               new Vector2(scale), BagLib.PegsLayer, true);
            bound = new Physics.CircleBound(image.Position, boundScale * 0.5f);

            //image.Color = Color.Green;
            if (dir > 0)
            {
                image.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            elasticity = Peg.PegElasticity;
            createShadow();
        }

        public override void OnCollsion(AbsGameObject otherObj, Physics.Collision2D coll, bool primaryCheck)
        {
            base.OnCollsion(otherObj, coll, primaryCheck);
            takeHit(otherObj);
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            takeHit(ball);
        }

        void takeHit(AbsGameObject collectingItem)
        {
            collectingItem.localGamer.collectPoint(1, collectingItem, this);
            new CoinCirkleEffect(image.Position, 4, state);
            mover.stopMover(true);
            DeleteMe(true);

            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.birdRemoveGameObject, Network.PacketReliability.Reliable);
            writeId(w);
        }

        public override void update()
        {
            base.update();
            updateShadow();
        }
    }
}
