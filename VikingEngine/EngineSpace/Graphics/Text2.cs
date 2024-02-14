using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Graphics
{
    class Text2 : AbsTextLine
    {
        protected string textString;
        float? multilineWidth;

        public Text2(string text, LoadedFont font, Vector2 pos, float height,
             Color color, ImageLayers layer, float? multilineWidth = null, bool addToRender = true)
            : base(font, pos, Vector2.One, color, layer, addToRender)
        {
            SetHeight(height);
            this.textString = text;
            this.multilineWidth = multilineWidth;

            updateMultiLine();
        }

        public Text2(bool addToRender = true)
            : base(addToRender)
        {
        }

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
                                textString,
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
                            textString,
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

        public Text2 Clone()
        {
            Text2 clone = new Text2(this.inRenderList);
            copyAllDataFrom(clone);
            return clone;
        }

        public override AbsDraw CloneMe()
        {
            return Clone();
        }
        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);

            Text2 c = (Text2)master;
            c.multilineWidth = this.multilineWidth;
            c.textString = this.textString;
        }

        public override void AddChar(char c)
        {
            textString += c;
        }

        public override string TextString
        {
            get
            {
                return textString;
            }
            set
            {
                textString = value;
                updateMultiLine();
                updateCenter();
            }
        }

        public void setLineWidth(float width)
        {
            multilineWidth = width;
            updateMultiLine();
        }

        void updateMultiLine()
        {
            if (multilineWidth != null)
            {
                this.textString = TextLib.SplitToMultiLine2(textString, Font, size.X, multilineWidth.Value);
            }
        }
    }
}
