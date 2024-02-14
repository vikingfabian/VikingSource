using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.MiniGolf
{
    class FlyingCoins : AbsInGameUpdateable
    {
        const int TailCount = 8;

        Vector2 position;
        Coin headCoin;
        CoinTailPart coinTail;
        Vector2 speedDir;
        Vector2 waveDir;
        double waveAngle = 0;

        Graphics.Image wings;
        Timer.Basic boundsCheck = new Timer.Basic(500, true);
        VectorRect offBoundArea;
        bool startOffCheck = false;

        public FlyingCoins()
            : base(true)
        {
            float r = Engine.Screen.Area.Size.Length();
            Vector2 targetCenter = GolfRef.field.outerBound.Center;
            targetCenter.X += Ref.rnd.Plus_MinusF(GolfRef.field.outerBound.Width * 0.35f);
            targetCenter.Y += Ref.rnd.Plus_MinusF(GolfRef.field.outerBound.Height * 0.35f);

            Rotation1D offsetDir = Rotation1D.Random();
            Vector2 start = targetCenter + offsetDir.Direction(r);
            position = start;

            float speed = GolfRef.gamestate.MaxSpeed * 0.05f;
            speedDir = offsetDir.getInvert().Direction(speed);
            offsetDir.AddDegrees(90);
            waveDir = offsetDir.Direction(GolfRef.gamestate.CoinScale * 0.2f);

            headCoin = new Coin(start, CoinValue.Value10, false);
            wings = new Graphics.Image(SpriteName.pjCoinWings1,
                start, headCoin.image.size * 1.2f, ImageLayers.AbsoluteBottomLayer,
                true);

            wings.LayerAbove(headCoin.image);
            Graphics.AnimationLoop animation = new Graphics.AnimationLoop(wings,
                SpriteName.pjCoinWings1, 4, 90, Graphics.MotionRepeate.Loop);

            int tailCount = TailCount;
            coinTail = new CoinTailPart(ref tailCount, headCoin.image.position);
            offBoundArea = Engine.Screen.Area;
            offBoundArea.AddRadius((TailCount + 2) * CoinTailPart.CoinsSpacing);
        }

        public override void Time_Update(float time_ms)
        {
            float speedUp = startOffCheck? 1f : 2f;
            
            position +=  Ref.DeltaGameTimeSec * speedUp * speedDir;
            waveAngle += 4f * Ref.DeltaGameTimeSec;
            headCoin.setPosition(wavePos(position, waveAngle));
            wings.position = headCoin.image.position;

            coinTail.Update(position, this, waveAngle, CoinTailPart.CoinsSpacing * 1.2f);

            if (boundsCheck.UpdateInGame())
            {
                if (startOffCheck)
                {
                    if (offBoundArea.IntersectPoint(position) == false)
                    {
                        DeleteMe();
                        coinTail.DeleteMe();
                    }
                }
                else
                {
                    if (Engine.Screen.Area.IntersectPoint(position))
                    {
                        startOffCheck = true;
                    }
                }
            }

            if (headCoin.isDeleted)
            {
                coinTail.setDrift();
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();

            wings.DeleteMe();
        }

        public Vector2 wavePos(Vector2 center, double waveAngle)
        {
            return center + waveDir * (float)Math.Sin(waveAngle);
        }
    }

    class CoinTailPart
    {
        public static float CoinsSpacing;
        CoinTailPart child = null;
        Vector2 position;

        Coin coin = null;

        public CoinTailPart(ref int countLeft, Vector2 startPos)
        {
            this.position = startPos;
            coin = new Coin(startPos, CoinValue.Value1, false);
            if (--countLeft > 0)
            {
                child = new CoinTailPart(ref countLeft, startPos);
            }
        }

        public void Update(Vector2 parentPos, FlyingCoins head, double waveAngle, float maxLength)
        {
            waveAngle -= 1.2;
            
            Vector2 diff = parentPos - position;
            if (VectorExt.Length(diff.X, diff.Y) > maxLength)
            {
                position += diff * 0.02f;
            }

            child?.Update(position, head, waveAngle, CoinsSpacing);

            if (coin != null)
            {
                coin.setPosition(head.wavePos(position, waveAngle)); //position);
                if (coin.isDeleted)
                {
                    coin = null;
                }
            }
        }

        public void setDrift()
        {
            if (coin != null)
            {
                Ref.draw.AddToRenderList(coin.image, false, coin.image.inRenderLayer);
                Ref.draw.AddToRenderList(coin.image, true, Draw.ShadowObjLayer);

                coin.shadow.DeleteMe();
                coin.shadow = null;
            }

            child?.setDrift();
        }

        public void DeleteMe()
        {
            coin?.DeleteMe();
            child?.DeleteMe();
        }
    }
}
