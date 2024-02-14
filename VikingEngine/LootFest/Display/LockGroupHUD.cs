using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Map;
using VikingEngine.LootFest.BlockMap;

namespace VikingEngine.LootFest.Display
{
    class LockGroupHUD : AbsInteractDisplay
    {
        List<Graphics.Image> backgrounds;
        List<Graphics.Image> colllectedIcons;

        public LockGroupHUD(LockGroup lockGroup)
        {
            backgrounds = new List<Graphics.Image>(lockGroup.connectedGameObjects);
            colllectedIcons = new List<Graphics.Image>(lockGroup.connectedGameObjects);
        }

        override public void refresh(Players.Player p, object sender)
        {
            LockGroup lockGroup = sender as LockGroup;
            DeleteMe();
            Vector2 pos = PlayerStatusDisplay.BottomLeftPos(p.localPData);
            pos.Y += Engine.Screen.IconSize * 2f;
            Vector2 size = new Vector2(Engine.Screen.IconSize);
            pos += size * 0.5f;

            for (int i = 0; i < lockGroup.connectedGameObjects; ++i)
            {
                Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, pos, size, AbsHUD2.HUDLayer, true);
                bg.Color = Color.Black;
                bg.Xpos += size.X * 1.2f * i;
                backgrounds.Add(bg);
                if (i < lockGroup.collectedGameObjects)
                {
                    Graphics.Image icon = new Graphics.Image(SpriteName.WhiteArea, bg.Position, size * 0.8f, AbsHUD2.HUDLayer - 1, true);
                    colllectedIcons.Add(icon);
                }
            }

            time.Seconds = 4f;
        }

        override public void DeleteMe()
        {
            foreach (var img in backgrounds)
            {
                img.DeleteMe();
            }
            foreach (var img in colllectedIcons)
            {
                img.DeleteMe();
            }
            
        }
    }
}
