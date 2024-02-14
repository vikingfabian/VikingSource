using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Joust
{
    class HealthBar : AbsUpdateable
    {
        const float ViewTimeSec = 3f;
        
        Graphics.Image[] hearts;            
        float viewTime = 0f;
        Gamer parent;

        float spacing;
        Vector2 startPos;

        public HealthBar(Gamer parent, Vector2 parentScale, int health)
            :base(true)
        {
            this.parent = parent;
            Vector2 heartScale = parentScale * 0.2f;

            hearts = new Graphics.Image[parent.maxHealth];

            for (int i = 0; i < hearts.Length; ++i)
            {
                hearts[i] = new Graphics.Image(SpriteName.birdHeart, Vector2.Zero, heartScale, JoustLib.LayerHealthBar, true);
                hearts[i].PaintLayer += (parent.gamerData.GamerIndex * parent.maxHealth + i) * PublicConstants.LayerMinDiff;
                if (i >= health)
                {
                    hearts[i].Color = Color.Black;
                }
            }

            spacing = heartScale.X;
            startPos.X = -spacing * (parent.maxHealth - 1) * 0.5f;
            startPos.Y = -parentScale.Y * 0.4f;
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
            for (int i = 0; i < hearts.Length; ++i)
            {
                hearts[i].Position = pos;
                pos.X += spacing;
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            for (int i = 0; i < hearts.Length; ++i)
            {
                hearts[i].DeleteMe();
            }
        }
    }
}
