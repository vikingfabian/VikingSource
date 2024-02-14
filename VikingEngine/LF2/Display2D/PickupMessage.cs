using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
    class PickupMessage : VikingEngine.HUD.AbsMessage
    {
        static readonly Vector2 Size = new Vector2(GadgetImage.IconSize * 3, GadgetImage.IconSize);
        
        public PickupMessage(Vector2 pos, GameObjects.Gadgets.IGadget gadget)
        {
            Graphics.Image  background = new Graphics.Image(SpriteName.LFMenuLootMessage, pos, Size, ImageLayers.Background4);
            area = new VectorRect(pos, Size);

            GadgetImage image = new GadgetImage(ImageLayers.Background3, gadget, true);
            image.Position = background.Position + new Vector2(GadgetImage.IconSize - GadgetImage.Edge, -GadgetImage.Edge);
            images = new List<Graphics.AbsDraw2D> { background, image };
                
        }
    }
}
