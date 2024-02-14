using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2 
{
    //class AutoAssignButtonMessage
    //{
    //}
    class AutoAssignButtonMessage : VikingEngine.HUD.AbsMessage
    {
        //static readonly Vector2 Size = new Vector2(96, 32);

        public AutoAssignButtonMessage(Vector2 pos, SpriteName button, GameObjects.Gadgets.IGadget gadget)
        {
            Graphics.Image background = new Graphics.Image(button, pos, LoadTiles.EquipButtonSize, ImageLayers.Background4);
            area = new VectorRect(pos, LoadTiles.EquipButtonSize);


            GadgetImage image = new GadgetImage(ImageLayers.Background3, gadget, false);
            image.Position = background.Position + new Vector2(GadgetImage.Edge, GadgetImage.Edge);

            images = new List<Graphics.AbsDraw2D> { background, image };

        }
    }
}
