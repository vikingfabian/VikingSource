using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class CoinPeg: AbsGameObject
    {
        static readonly Color FlashCol = new Color(253,243,182);
        const int Coins = 5;
        int health = Coins;
        int sizeUpTimer = 0;
        float scale;

        public CoinPeg(Vector2 pos, int networkId, BagatellePlayState state)
            :base(networkId, state)
        {
            scale = BagLib.PegSprite.toImageSize(state.CoinPegScale);

            image = new Graphics.Image(SpriteName.bagCoinPeg_off, pos,
               new Vector2(scale), BagLib.PegsLayer, true);
            createShadow();
           // image.Color = Color.Yellow;
            bound = new Physics.CircleBound(image.Position, state.CoinPegScale * 0.5f);

            elasticity = 0.7f;
        }

        public override void update()
        {
            base.update();
            if (sizeUpTimer > 0)
            {
                if (--sizeUpTimer <= 0)
                {
                    image.Size1D = scale;
                    image.SetSpriteName(SpriteName.bagCoinPeg_off);
                }
            }
            updateShadow();
        }

        public override void OnCollsion(AbsGameObject otherObj, Physics.Collision2D coll, bool primaryCheck)
        {
            base.OnCollsion(otherObj, coll, primaryCheck);

            
            takeHit(otherObj, otherObj.localGamer);
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            takeHit(ball, gamer);

            if (pickupType)
            {
                PickUpEvent(ball, gamer);
            }

        }

        void takeHit(AbsGameObject collectingItem, LocalGamer gamer)
        {
            health -= 1;
            onTakeHit(true, collectingItem.GetBall());

            gamer.collectPoint(1, collectingItem, this);

            beginNetWriteItemStatus(collectingItem);
           
        }

        void onTakeHit(bool local, Ball ball)
        {
            if (health <= 0)
            {
                if (local)
                {
                    new CoinCirkleEffect(image.Position, 4, state);
                }
                DeleteMe(true);
            }

            sizeUpTimer = Peg.SizeUpFramesTime;
            image.Size1D = scale * 1.2f;
            image.SetSpriteName(SpriteName.bagCoinPeg_on);

            if (ball != null)
            {
                ball.hitSound();
            }

            new PegFlashAnimation(image, FlashCol);
        }

        protected override void netWriteItemStatus(System.IO.BinaryWriter w)
        {
            base.netWriteItemStatus(w);
            w.Write((byte)health);
        }
        public override void netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
            base.netReadUpdate(r);
            health = r.ReadByte();

            Ball b = null;
            if (affectingItem != null)
            {
                b = affectingItem.GetBall();
            }

            onTakeHit(false, b);
        }

        public override void PickUpEvent(AbsGameObject collectingItem, LocalGamer gamer)
        {
            
        }

        
    }
}