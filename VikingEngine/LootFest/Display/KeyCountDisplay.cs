using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Display
{
    class KeyCountDisplay : AbsInteractDisplay
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup();

        public KeyCountDisplay(Players.Player p, IntVector2 count)
        {
            refresh(p, count);
        }

        /// <param name="sender">intvector2 - count/need count</param>
        public override void refresh(Players.Player player, object sender)
        {
            IntVector2 count = (IntVector2)sender; 

            Vector2 pos = PlayerStatusDisplay.BottomLeftPos(player.localPData);
            pos.Y += Engine.Screen.IconSize * 2f;
            Vector2 size = new Vector2(Engine.Screen.IconSize * 1.3f);

            count.Y = Bound.Min(count.Y, count.X);

            for (int i = 0; i < count.Y; ++i)
            {
                Graphics.Image keyIcon = new Graphics.Image(i < count.X? SpriteName.LFHudKey : SpriteName.LFHudKeyEmpty, 
                    pos, size, AbsHUD2.HUDLayer, true);
                //keyIcon.Color = Color.Black;
                keyIcon.Xpos += size.X * 0.8f * i;
                images.Add(keyIcon);
            }

            time.Seconds = 10f;
        }

        public override void DeleteMe()
        {
            images.DeleteAll();
        }

    }
}
