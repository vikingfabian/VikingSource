using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Display
{
    class LevelCollectItemDisplay : AbsInteractDisplay
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup();
        
        public LevelCollectItemDisplay(Players.Player p)
        {
            refresh(p, null);
        }

        public override void refresh(Players.Player player, object sender)
        {
            VikingEngine.LootFest.BlockMap.CollectItem collect = player.hero.Level.collect;

            Vector2 pos = PlayerStatusDisplay.BottomLeftPos(player.localPData);
            pos.Y += Engine.Screen.IconSize * 2f;
            Vector2 size = new Vector2(Engine.Screen.IconSize * 1.3f);

            collect.goalCount = Bound.Min(collect.goalCount, collect.collectedCount);

            for (int i = 0; i < collect.goalCount; ++i)
            {
                Graphics.Image collectIcon = new Graphics.Image(collect.collectDisplayIcon,
                    pos, size, AbsHUD2.HUDLayer, true);

                if (i >= collect.collectedCount)
                {
                    collectIcon.Color = ColorExt.VeryDarkGray;
                }

                collectIcon.Xpos += size.X * 1f * i;
                images.Add(collectIcon);
            }

            time.Seconds = 10f;
        }

        public override void DeleteMe()
        {
            images.DeleteAll();
        }

    }
}
