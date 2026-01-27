using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Xna.Framework;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;

namespace VikingEngine.CardDesign.CardGraphics
{
    class CardFace : Graphics.RenderTargetImage
    {
        FieldUnit card;

        public const float IconSize = 16 * TextureSizeMultiply;
        const int TextureSizeMultiply = 4;
        public static readonly Vector2 IconSzV2 = new Vector2(IconSize );
        static readonly Vector2 CardBgSize = new Vector2(2, 3) * 32 * TextureSizeMultiply;
        const float CardOutLineSpace = 32 * TextureSizeMultiply;
        public static readonly Vector2 FullTargetSize = CardBgSize + new Vector2(CardOutLineSpace * 2);

        public CardFace(FieldUnit card) 
            :base(Vector2.Zero, FullTargetSize, ImageLayers.Lay0)
        { 
            this.card = card;
            generateTexture();
        }

        public void generateTexture()
        {
            var images = new List<Graphics.AbsDraw>(8);
            Vector2 topLeft = new Vector2(CardOutLineSpace);
            Graphics.Image bg = new Graphics.Image(SpriteName.CardFront, topLeft, CardBgSize, ImageLayers.Background5, false, false);
            images.Add(bg);

            Vector2 imageCenter = topLeft + CardBgSize * new Vector2(0.5f, 0.27f);
            Vector2 imageSize = new Vector2(CardBgSize.X * 0.6f);
            Graphics.Image image = new Graphics.Image(card.image, imageCenter, imageSize, ImageLayers.Lay1, true, false);
            images.Add(image);

            card.cost.ToCard(images, VectorExt.AddY( topLeft, -IconSize * 0.5f), CardBgSize.X);

            card.unitProperties.ToCard(images, VectorExt.AddY( topLeft, CardBgSize.Y * 0.44f), CardBgSize.X);

            DrawImagesToTarget(images, true);
        }
    }
}
