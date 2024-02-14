using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.Voxels
{
    class MouseToolHUD
    {
        public PaintFillType? selected = null;

        PaintFillType[] tools = new PaintFillType[]
        {
            PaintFillType.Fill,
            PaintFillType.Delete,
            PaintFillType.Select
        };

        HUD.IconButton[] buttons;

        public MouseToolHUD()
        {
            //SpriteName[] icons = new SpriteName[]
            //    {
            //        SpriteName.IconBuildAdd,
            //        SpriteName.IconBuildRemove,
            //        SpriteName.IconBuildSelection,
            //    };
            

            Input.Mouse.Visible = true;

            Vector2 buttonSz = Engine.Screen.IconSizeV2;
            float spacing = Engine.Screen.BorderWidth * 2f;

            float totalW = buttonSz.X * tools.Length + spacing * (tools.Length - 1);

            Vector2 pos = Engine.Screen.CenterScreen;
            pos.X -= totalW * 0.5f;

            buttons = new HUD.IconButton[tools.Length];

            for (int i = 0; i < tools.Length; ++i)
            {
                HUD.IconButton button = new HUD.IconButton(ToolIcon(tools[i]), new VectorRect(pos, buttonSz), ImageLayers.Foreground3, LootFest.LfLib.ButtonGuiSettings);
                pos.X += buttonSz.X + spacing;
                buttons[i] = button;
            }
        }

        public static SpriteName ToolIcon(PaintFillType tool)
        {
            switch (tool)
            {
                case PaintFillType.Fill:
                    return SpriteName.IconBuildAdd;

                case PaintFillType.Delete:
                    return SpriteName.IconBuildRemove;

                case PaintFillType.Select:
                    return SpriteName.IconBuildSelection;
            }

            throw new NotImplementedException();
        }

        public bool update()
        {
            for (int i = 0; i < buttons.Length; ++i)
            {
                if (buttons[i].update())
                {
                    selected = tools[i];

                    return true;
                }
            }

            if (Input.Mouse.ButtonDownEvent(MouseButton.Left) || Input.Mouse.ButtonDownEvent(MouseButton.Right))
            {
                return true;
            }

            return false;
        }
        public void DeleteMe()
        {
            Input.Mouse.Visible = false;

            foreach (var m in buttons)
            {
                m.DeleteMe();
            }
        }
    }
}
