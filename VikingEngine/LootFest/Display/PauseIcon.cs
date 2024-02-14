using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LootFest.Display
{
    class PauseIcon : AbsUpdateable
    {
        Image pauseIcon, pauseIconBg;
        Vector2 goalPos, startPos;
        float moveLength;
        float moveSpeed;

        bool moveIn = true;
        

        public PauseIcon(VectorRect screenSafeArea)
            : base(true)
        {
            Vector2 sz = new Vector2(Engine.Screen.IconSize * 3f);
            goalPos = screenSafeArea.RightBottom - sz * 0.5f;

            moveLength = sz.X * 1f;
            moveSpeed = moveLength * 6f;

            startPos = goalPos;
            startPos.X += moveLength;

            pauseIconBg = new Image(SpriteName.birdWhiteSoftBox, startPos, sz, ImageLayers.Background6, true);
            pauseIcon = new Image(SpriteName.pauseSymbol, pauseIconBg.Position, sz * 0.8f, ImageLayers.AbsoluteTopLayer, true);
            pauseIcon.Color = Color.Black;
            pauseIcon.LayerAbove(pauseIconBg);

            pauseIconBg.Opacity = 0f;
            pauseIcon.Opacity = 0f;
        }

        public override void Time_Update(float time_ms)
        {
            float diffX = pauseIconBg.Xpos - startPos.X;
            float opacity = Math.Abs(diffX) / moveLength;

            pauseIconBg.Opacity = opacity;
            pauseIcon.Opacity = opacity;

            if (moveIn)
            {
                if (pauseIconBg.Xpos > goalPos.X)
                {
                    pauseIconBg.Xpos -= Ref.DeltaTimeSec * moveSpeed;
                }

                if (pauseIconBg.Xpos < goalPos.X)
                {
                    pauseIconBg.Xpos = goalPos.X;
                }
            }
            else
            {
                if (pauseIconBg.Xpos < startPos.X)
                {
                    pauseIconBg.Xpos += Ref.DeltaTimeSec * moveSpeed;
                }
                else
                {
                    DeleteMe();
                }
            }

            pauseIcon.Position = pauseIconBg.Position;
        }

        public void BeginDelete()
        {
            moveIn = false;
        }

        public override void DeleteMe()
        {
            pauseIcon.DeleteMe();
            pauseIconBg.DeleteMe();
            base.DeleteMe();
        }

    }
}
