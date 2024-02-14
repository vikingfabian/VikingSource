using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Display
{
    class SpriteText3D :AbsUpdateable
    {
        Time beginFade;
        float fadeSpeed;
        List<Graphics.Mesh> letters;

        public SpriteText3D(string text, Vector3 center, float height, Color color)
            :base(false)
        {
            letters = new List<Graphics.Mesh>(text.Length);
            float letterSpacing = SpriteText.LetterWidthScale * height;
            float totalWidth = Table.TotalWidth(text.Length, SpriteText.LetterWidthScale * height, 0f);
            center.X -= (totalWidth - letterSpacing) * 0.5f;

            Vector3 sz = new Vector3(height);
            foreach (var c in text)
            {
                Graphics.Mesh l = new Graphics.Mesh(LoadedMesh.plane, center, sz, Graphics.TextureEffectType.Flat,
                    SpriteText.CharTile(c), color);
                letters.Add(l);

                center.X += letterSpacing;
            }
        }
        
        public override void Time_Update(float time_ms)
        {
            if (beginFade.CountDown())
            {
                letters[0].Opacity -= fadeSpeed * time_ms;

                if (letters[0].Opacity <= 0)
                {
                    DeleteMe();
                    AddToOrRemoveFromUpdateList(false);
                }
                else
                {
                    for (int i = 1; i < letters.Count; ++i)
                    {
                        letters[i].Opacity = letters[0].Opacity;
                    }
                }
            }
        }

        public void fadeOut(float startTime, float fadeTime)
        {
            beginFade.MilliSeconds = startTime;
            fadeSpeed = letters[0].Opacity / fadeTime;
            this.AddToUpdateList();
        }

        public override void DeleteMe()
        {   
            arraylib.DeleteAndClearArray(letters);
        }
    }
}
