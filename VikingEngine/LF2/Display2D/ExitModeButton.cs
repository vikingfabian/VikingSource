﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.LF2
{
    class ExitModeButton
    {
        Image icon;
        TextG text;

        public ExitModeButton(SpriteName button, VectorRect safeArea)
        {
            icon = new Image(button, safeArea.BottomLeft, new Microsoft.Xna.Framework.Vector2(48), MiniMap.MapBackgroundLayer - 1);
            icon.Ypos -= icon.Height;
            text = new TextG(LoadedFont.Lootfest, icon.Position, new Microsoft.Xna.Framework.Vector2(1.4f), Align.CenterHeight, "Exit", Color.White, MiniMap.MapBackgroundLayer - 1);
            text.Ypos = icon.Center.Y;
            text.Xpos += icon.Width;
        }
        public void DeleteMe()
        {
            icon.DeleteMe();
            text.DeleteMe();
        }
    }
}
