using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    class HealthBar2 : AbsHUD2
    {
        float largeHeartScale;
        float smallHeartScale;

        List<Graphics.Image> hearts;
        Graphics.Motion2d pulsating = null;
        protected ImageLayers layer = HUDLayer;

        public HealthBar2(Vector2 startPos, int maxHealth)
        {

            largeHeartScale = HUDStandardHeight * 0.8f;
            smallHeartScale = HUDStandardHeight * 0.6f;

            float spacing = HUDStandardHeight * 0.7f;

            Area = new VectorRect(startPos.X, startPos.Y, spacing * maxHealth, spacing);

            startPos += new Vector2(spacing * 0.5f);

            hearts = new List<Graphics.Image>(maxHealth);
            for (int i = 0; i < maxHealth; ++i)
            {
                Graphics.Image h = new Graphics.Image(heartIcon, startPos, Vector2.One, layer, true);
                hearts.Add(h);
                startPos.X += spacing;
            }

            UpdateHealth(maxHealth);
        }

        int previousViewHearts = int.MinValue;

        public void UpdateHealth(float health)
        {
            int viewHearts = (int)(health + 0.9f);

            if (viewHearts != previousViewHearts)
            {
                float paintLay = Graphics.GraphicsLib.ToPaintLayer(layer);
                for (int i = 0; i < hearts.Count; ++i)
                {
                    hearts[i].PaintLayer = paintLay - PublicConstants.LayerMinDiff * i;
                    hearts[i].Color = Color.White;

                    if (i < viewHearts)
                    {
                        //hearts[i].Color = Color.White;
                        hearts[i].SetSpriteName(heartIcon);
                        if (i == viewHearts - 1)
                        {
                            hearts[i].Size = new Vector2(largeHeartScale);
                        }
                        else
                        {
                            hearts[i].Size = new Vector2(smallHeartScale);
                            hearts[i].Color = Color.LightGray;
                        }
                    }
                    else
                    {
                        hearts[i].SetSpriteName(SpriteName.LFHudHeartEmpty);
                        hearts[i].Size = new Vector2(smallHeartScale);
                        hearts[i].PaintLayer += 10 * PublicConstants.LayerMinDiff;
                        
                    }
                }

                if (viewHearts == 1)
                {
                    if (pulsating == null)
                    {
                        pulsating = new Graphics.Motion2d(Graphics.MotionType.SCALE, hearts[0],
                            new Vector2(smallHeartScale * 0.5f), Graphics.MotionRepeate.BackNForwardLoop,
                            200, true);
                    }
                }
                else if (pulsating != null)
                {
                    pulsating.DeleteMe();
                    pulsating = null;
                }

                previousViewHearts = viewHearts;
            }
        }

        virtual protected SpriteName heartIcon
        {
            get { return SpriteName.LFHudHeart; }
        }


        public override void DeleteMe()
        {
            foreach (Graphics.Image img in hearts)
            {
                img.DeleteMe();
            }
        }
    }
}
