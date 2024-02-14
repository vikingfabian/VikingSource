using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    class TextBoxSimple : TextS
    {
        /* Fields */
        public float lineWidth = 1;

        /* Constructors */
        public TextBoxSimple()
            : base()
        { }
        
        public TextBoxSimple(LoadedFont font, Vector2 pos, Vector2 scale,
            Align objCenter, string text, Color color, ImageLayers layer, float _lineWidth, bool addToRender)
            : base(font, pos, scale, objCenter, text, color, layer, addToRender)
        {
            lineWidth = _lineWidth;
            updateText();
        }

        public TextBoxSimple(LoadedFont font, Vector2 pos, Vector2 scale,
            Align objCenter, string text, Color color, ImageLayers layer, float _lineWidth)
            : this(font, pos, scale, objCenter, text, color, layer, _lineWidth, true)
        { }

        public override AbsDraw CloneMe()
        {
            var clone = new TextBoxSimple();
            this.copyAllDataFrom(clone);
            
            return clone;
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);

            TextBoxSimple c = (TextBoxSimple)master;
            c.lineWidth = this.lineWidth;
            c.textString = this.textString;
        }

        /* Family methods */
        public override string TextString
        {
            get
            {
                return textString;
            }
            set
            {
                base.TextString = value;
                updateText();
            }
        }

        public override void Draw(int cameraIndex)
        {
            base.Draw(cameraIndex);
        }

        public override void AddChar(char c)
        {
            if (textString == " ")
            {
                textString = Convert.ToString(c);
            }
            else base.AddChar(c);
        }

        /* Novelty methods */
        void updateText()
        {
            this.textString = TextLib.SplitToMultiLine2(base.TextString, Font, size.X, lineWidth);
        }
    }
}
