using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// By value type of visual text, prefered when the text string will change rarely
    /// </summary>
    class TextS: AbsTextLine
    {
        /* Properties */
        public override string TextString
        {
            get
            {
                return textString;
            }
            set
            {
                textString = value;
                updateCenter();
            }
        }

        /* Fields */
        protected string textString = TextLib.EmptyString;
        
        /* Constructors */
        public TextS(LoadedFont font, Vector2 pos, Vector2 scale, Align objCenter,
            string text, Color color, ImageLayers layer, bool addToRender)
            : base(font, pos, scale, color, layer, addToRender)
        {
            textString = text;
            SetCenterRelative(objCenter.Center);
        }

        public TextS(LoadedFont font, Vector2 pos, Vector2 scale,
            Align objCenter, string text, Color color, ImageLayers layer)
            : this(font, pos, scale, objCenter, text, color, layer, true)
        { }

        public TextS()
            : this(LoadedFont.Regular, Vector2.Zero, Vector2.One, Align.Zero, 
            TextLib.EmptyString, Color.White, ImageLayers.Lay1, true)
        { }

        /* Methods */
        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
#if DEBUG
                if (textString == null)
                    textString = "NULL Exception";
#endif

                Ref.draw.spriteBatch.DrawString(Engine.LoadContent.Font(Font),
                    textString,
                    position,
                    DrawColor(),
                    rotation,
                    textDrawOrigin,
                    size,
                    SpriteEffects.None,
                    PaintLayer);
            }
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);

            TextS c = (TextS)master;
            c.textString = this.textString;
        }

        public override void AddChar(char c)
        {
            textString += c;
        }  
    }
    
}
