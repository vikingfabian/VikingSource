using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class Coin : AbsGameObject
    {
        bool hidden = false;
        AbsGameObject unhideObject = null;
        int unHideTimeCount = 0;
        int value = 1;
        
        GameObjectType coinType;
        SpriteName particleColor = SpriteName.birdCoinParticleYellow;

        public Coin(Vector2 pos, int networkId, BagatellePlayState state, GameObjectType coinType)
            : base(networkId, state)
        {
            const float BigScale = 1.7f;
            SpriteName tile;
            float visualScale = state.CoinScale;
            this.coinType = coinType;
            SpriteSize spriteSz;

            switch (coinType)
            {
                case GameObjectType.CoinOutLine:
                    hidden = true;
                    goto default;
                default:
                    tile = hidden? SpriteName.birdCoinHidden : SpriteName.birdCoin1;
                    spriteSz = BagLib.CoinSprite;
                    break;
                case GameObjectType.BigCoin5:
                    visualScale *= BigScale;
                    tile = SpriteName.birdCoinValue5;
                    value = 5;
                    particleColor = SpriteName.birdCoinParticleBlue;
                    spriteSz = BagLib.Coin5And10Sprite;
                    break;
                case GameObjectType.BigCoin10:
                    visualScale *= BigScale;
                    tile = SpriteName.birdCoinValue10;
                    value = 10;
                    particleColor = SpriteName.birdCoinParticleGreen;
                    spriteSz = BagLib.Coin5And10Sprite;
                    break;
                case GameObjectType.BigCoin20:
                    visualScale = BagLib.Coin20Sprite.relativeSizeTo(BagLib.Coin5And10Sprite) * visualScale * BigScale;
                    tile = SpriteName.birdCoinValue20;
                    value = 20;
                    spriteSz = BagLib.Coin20Sprite;
                    break;
                case GameObjectType.BumpRefill:
                    visualScale *= 1.4f;
                    tile = SpriteName.bagBumpRefill;
                    value = 0;
                    spriteSz = BagLib.BumpRefillSprite;
                    break;
            }

            image = new Graphics.Image(tile, pos,
               new Vector2(spriteSz.toImageSize(visualScale)), BagLib.PickupsLayer, true);
            createShadow();
            setPickUpLayer();

            bound = new Physics.CircleBound(image.Position, visualScale * 0.5f);

            //if (hidden)
            //{
            //    image.Color = ColorExtensions.VeryDarkGray;
            //}

            solidBound = false; pickupType = true;
        }

        public override void update()
        {
            base.update();

            if (hidden && unHideTimeCount > 0)
            {
                if (--unHideTimeCount <= 0)
                {
                    hidden = false;
                    //image.Color = Color.White;
                    image.SetSpriteName(SpriteName.birdCoin1);
                    new Graphics.Motion2d(Graphics.MotionType.SCALE, image, image.Size * 0.5f, Graphics.MotionRepeate.BackNForwardOnce,
                        90, true);
                }
            }

            updateShadow();
        }

        public override void PickUpEvent(AbsGameObject collectingItem, LocalGamer gamer)
        {
            if (hidden)
            {
                unhide(collectingItem);
            }
            else
            {
                onPickUp(collectingItem, gamer);
            }

            beginNetWriteItemStatus(collectingItem);
        }

        void unhide(AbsGameObject collectingItem)
        {
            if (unhideObject == null || unhideObject == collectingItem)
            {
                unhideObject = collectingItem;
                unHideTimeCount = 2;
            }
        }

        void onPickUp(AbsGameObject collectingItem, LocalGamer gamer)
        {
            if (coinType == GameObjectType.BumpRefill)
            {
                if (collectingItem != null && collectingItem.GetBall() != null &&
                    gamer != null)
                {
                    gamer.bumpRefillPickup(collectingItem.GetBall());
                }
            }
            else
            {
                VikingEngine.PJ.Particles.CoinParticle.Create(image.Position, image.Size1D, particleColor);

                if (gamer != null)
                {
                    gamer.collectPoint(value, collectingItem, this);
                }
                if (collectingItem != null && collectingItem.GetBall() != null)
                {
                    collectingItem.GetBall().hitSound();
                }
            }
            DeleteMe(gamer != null);
        }

        protected override void netWriteItemStatus(System.IO.BinaryWriter w)
        {
            base.netWriteItemStatus(w);

            byte status = (byte)(isDeleted ? 0 : 1);
            w.Write(status);
        }

        public override void netReadItemStatus(AbsGameObject affectingItem, System.IO.BinaryReader r)
        {
            base.netReadItemStatus(affectingItem, r);

            byte status = r.ReadByte();

            if (status == 1)
            {
                unhide(null);
            }
            else if (status == 0)
            {
                onPickUp(affectingItem, null);
            }
        }

        public override void OnHitWaveCollision(Ball ball, LocalGamer gamer)
        {
            PickUpEvent(ball, gamer);
        }

        public override GameObjectType Type => coinType;
    }

    
}
