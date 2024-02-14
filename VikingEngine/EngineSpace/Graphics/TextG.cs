using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Text;
using System.Reflection;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// By ref type of visual text, prefered when the text string will change often
    /// </summary>
    class TextG : AbsTextLine
    {
        /* Properties */
        public override string TextString
        {
            get
            {
                if (StringBuilder == null) return "NaN";
                return StringBuilder.ToString();
            }
            set
            {
                StringBuilder = new StringBuilder(value);
                updateCenter();
            }
        }

        /* Fields */
        public StringBuilder StringBuilder;

        /* Constructors */
        public TextG(LoadedFont font, Vector2 pos, Vector2 scale, Align objCenter,
            string text, Color color, ImageLayers layer, bool addToRender)
            : base(font, pos, scale, color, layer, addToRender)
        {
            StringBuilder = new StringBuilder(text);
            SetCenterRelative(objCenter.Center);
        }

        public TextG(LoadedFont font, Vector2 pos, Vector2 scale,
            Align objCenter, string text, Color color, ImageLayers layer)
            : this(font, pos, scale, objCenter, text, color, layer, true)
        { }

        public TextG()
            : this(LoadedFont.Regular, Vector2.Zero, Vector2.One, Align.Zero,
            TextLib.EmptyString, Color.White, ImageLayers.Lay1, true)
        { }

        /* Methods */
        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                if (outline != TextOutlineType.NoBorder)
                {
                    Color col = Color.Multiply(outlineColor, Opacity);
                    float layer = PaintLayer + PublicConstants.LayerMinDiff;

                    if (outline == TextOutlineType.Border8Dir)
                    {
                        foreach (var dir in BorderRender8Directions)
                        {
                            Ref.draw.spriteBatch.DrawString(Engine.LoadContent.Font(Font),
                                StringBuilder,
                                position + dir * outlineThickness,
                                col,
                                rotation,
                                textDrawOrigin,
                                size,
                                SpriteEffects.None,
                                layer);
                        }
                    }
                    else if (outline == TextOutlineType.ShadowSouthEast)
                    {
                        Ref.draw.spriteBatch.DrawString(Engine.LoadContent.Font(Font),
                            StringBuilder,
                            position + Vector2.One * outlineThickness,
                            col,
                            rotation,
                            textDrawOrigin,
                            size,
                            SpriteEffects.None,
                            layer);
                    }
                }

                Ref.draw.spriteBatch.DrawString(Engine.LoadContent.Font(Font),
                    StringBuilder,
                    position,
                    DrawColor(),
                    rotation,
                    textDrawOrigin,
                    size,
                    SpriteEffects.None,
                    PaintLayer);
            }
        }
        public override AbsDraw CloneMe()
        {
            TextG clone = new TextG(Font, position, size, Align.Zero, TextString, Color, ImageLayers.AbsoluteTopLayer);            
            clone.PaintLayer = PaintLayer;
            
            return clone;
        }
        public override void AddChar(char c)
        {
            StringBuilder.Append(c);
        }
    }
}
