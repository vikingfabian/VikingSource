using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichMenu
{
    class RichMenuControllerPointer
    {
        public InputMap inputMap;
        public Image pointer;
        public RichMenu menu;
        public float maxInteractDistance;
        public RichMenuControllerPointer(InputMap inputMap)
        {
            this.inputMap = inputMap;
        }

        public void setMenu(RichMenu menu)
        {
            this.menu = menu;

            if (pointer == null)
            {
                maxInteractDistance = Engine.Screen.IconSize;
                pointer = new Image(SpriteName.ColorPickerCircle, Vector2.Zero, Engine.Screen.IconSizeV2, ImageLayers.Lay0, true);
                pointer.Opacity = 0.8f;
            }

            pointer.Layer = menu.layer - 2;

            pointer.position = menu.renderArea.Position;
        }

        public void DeleteMe()
        {
            menu.deleteTooltip();
            pointer?.DeleteMe();
        }
    }

}
