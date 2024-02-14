using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    class MessageBlock : AbsMessage
    {
        Image background;

        public MessageBlock(Vector2 pos, float width, SpriteName backgroundTile, Color backgroundCol, string text, TextFormat format, ImageLayers layer)
            : this(pos, width, backgroundTile, backgroundCol, text, format, SpriteName.NO_IMAGE, layer)
        { }

        public MessageBlock(Vector2 pos, float width, SpriteName backgroundTile, Color backgroundCol, string text, TextFormat format, SpriteName icon, ImageLayers layer)
        {
            const float IconSize = 32;

            float textW = width;
            if (icon != SpriteName.NO_IMAGE)
            {
                textW -= IconSize * 1.5f;
            }

            this.textBox = new TextBoxSimple(format.Font, new Vector2(pos.X + 8, pos.Y), format.Scale, Align.Zero, text,
                format.Color, layer -1, textW);
            background = new Image(backgroundTile, pos, new Vector2(width,
                this.textBox.MeasureText().Y), layer);
            background.Color = backgroundCol;
            
            area = new VectorRect(background.Position, background.Size);
            images = new List<AbsDraw2D> { this.textBox, background };

            if (icon != SpriteName.NO_IMAGE)
            {
                this.textBox.Xpos += IconSize;
                Image iconImage = new Image(icon, new Vector2(pos.X + 4, pos.Y + 4), VectorExt.V2(IconSize), ImageLayers.AbsoluteTopLayer);
                iconImage.PaintLayer = background.PaintLayer - PublicConstants.LayerMinDiff;
                images.Add(iconImage);
            }
        }

    }

    
    
}
