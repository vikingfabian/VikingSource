using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    abstract class AbsSpriteText
    {
        float letterSideEdge, letterWidthScale;

        Vector2 relCenter;
        ImageLayers layer;
        Vector2 pivot;
        public Graphics.Image[] letters;
        public Vector2 size;
        bool addToRender;
        public Color color;

        public AbsSpriteText(float letterWidthScale,
            string textString, Vector2 position, float height, ImageLayers layer, Vector2 relCenter, Color color, bool addToRender)
        {
            letterSideEdge = (1f - letterWidthScale) * 0.5f; 
            //const float LetterWidthScale = 1f - LetterSideEdge * 2;
            //this.letterSideEdge = letterSideEdge;
            this.letterWidthScale = letterWidthScale;
            pivot = position;
            this.layer = layer;
            this.relCenter = relCenter;
            this.addToRender = addToRender;
            this.color = color;
            size = new Vector2(0, height);
            Text(textString);
        }

        public void Text(string textString)
        {
            this.Text(textString, this.color);
        }

        public void Text(string textString, Color color)
        {
            if (letters != null)
            {
                DeleteMe();
            }

            if (letters == null || letters.Length != textString.Length)
            {
                letters = new Graphics.Image[textString.Length];
            }
            size.X = 0;

            for (int i = 0; i < textString.Length; ++i)
            {
                SpriteName tile = charTile(textString[i]);
                
                var texSource = DataLib.SpriteCollection.Get(tile);
                letters[i] = new Graphics.Image(tile, Vector2.Zero,
                    new Vector2(size.Y / texSource.Source.Height * texSource.Source.Width, size.Y), layer, false, addToRender);
                letters[i].Color = color;
                size.X += letters[i].Width * letterWidthScale;
            }

            RelativeCenter(relCenter);
        }

        protected abstract SpriteName charTile(char c);

        public void RelativeCenter(Vector2 relCenter)
        {
            Vector2 pos = pivot - relCenter * size;
            foreach (var img in letters)
            {
                float edge = img.Width * letterSideEdge;
                pos.X -= edge;
                img.Position = pos;
                pos.X += img.Width - edge;
            }
        }

        public void SetPosition(Vector2 pos)
        {
            this.pivot = pos;
            RelativeCenter(relCenter);
        }

        public void SetY(float y)
        {
            this.pivot.Y = y;
            RelativeCenter(relCenter);
        }

        public void AddTo(List<Graphics.AbsDraw> drawList)
        {
            foreach (var img in letters)
            {
                drawList.Add(img);
            }
        }

        public void DeleteMe()
        {
            foreach (var img in letters)
            {
                img.DeleteMe();
            }
        }

        public void SetOpacity(float opacity)
        {
            foreach (var img in letters)
            {
                img.Opacity = opacity;
            }
        }

        public void SetColor(Color col)
        {
            this.color = col;
            foreach (var img in letters)
            {
                img.Color = this.color;
            }
        }


        public void SetVisible(bool visible)
        {
            foreach (var img in letters)
            {
                img.Visible = visible;
            }
        }

        public bool GetVisible()
        {
            return letters.Length > 0 && letters[0].Visible; 
        }

        public float Right
        {
            get
            {
                return arraylib.Last(letters).Right - letterSideEdge;
            }
        }
    }
}
