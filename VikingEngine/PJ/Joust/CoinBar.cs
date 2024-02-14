using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust
{
    class CoinBar : AbsUpdateable
    {
        const float ViewTimeSec = 3f;

        Graphics.Image[] coins;
        float viewTime = 0f;
        Gamer parent;

        float spacing;
        Vector2 startPos;

        public CoinBar(Gamer parent, Vector2 parentScale, int coinCount)
            : base(true)
        {
            this.parent = parent;
            Vector2 heartScale = parentScale * 0.12f;

            coins = new Graphics.Image[JoustLib.CoinsToHealthUp];

            for (int i = 0; i < JoustLib.CoinsToHealthUp; ++i)
            {
                coins[i] = new Graphics.Image(SpriteName.birdCoin1, Vector2.Zero, heartScale, JoustLib.LayerHealthBar, true);
                if (i >= coinCount)
                {
                    coins[i].Color = Color.Black;
                }
            }

            spacing = heartScale.X;
            startPos.X = -spacing * (JoustLib.CoinsToHealthUp - 1) * 0.5f;
            startPos.Y = -parentScale.Y * 0.6f;
            updatePos();
        }
        public override void Time_Update(float time)
        {
            viewTime += Ref.DeltaTimeSec;
            if (viewTime >= ViewTimeSec)
            {
                DeleteMe();
            }
            else
            {
                updatePos();
            }
        }

        void updatePos()
        {
            Vector2 pos = parent.Position + startPos;
            for (int i = 0; i < JoustLib.CoinsToHealthUp; ++i)
            {
                coins[i].Position = pos;
                pos.X += spacing;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            for (int i = 0; i < JoustLib.CoinsToHealthUp; ++i)
            {
                coins[i].DeleteMe();
            }
        }
    }
}
