using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.ToGG.HeroQuest.Display
{
    class DoomTrack
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup();
        List<Graphics.Image> skulls = new List<Graphics.Image>();
        public VectorRect bgArea;
        Vector2 chestSz;

        public Graphics.Image nextClock;
        public Vector2 nextSkullPos;

        public DoomTrack()
        {
            const int FrameCorner = 11;
            
            DoomData doom = hqRef.setup.conditions.doom;

            Vector2 iconSz = Engine.Screen.SmallIconSizeV2;
            chestSz = iconSz * 0.5f;
            float spacing = Engine.Screen.BorderWidth;

            float typeSpacing = Engine.Screen.IconSize;

            float skullsTotWidth = Table.TotalWidth(doom.skulls.maxValue, iconSz.X, spacing);
            float width = skullsTotWidth;
            
            if (ViewTime)
            {
                float timeTotWidth = Table.TotalWidth(DoomData.TurnsToSkull, iconSz.X, spacing);
                width += timeTotWidth + typeSpacing;
            }

            float edge = (FrameCorner + 2) * HudLib.BorderScale;

            Vector2 pos = Engine.Screen.SafeArea.CenterTop;
            pos.X -= width * 0.5f;

            Vector2 topLeft = pos;
            topLeft.X -=  edge;
            pos.Y += edge;

            pos += iconSz * 0.5f;

            if (ViewTime)
            {
                for (int i = 0; i < DoomData.TurnsToSkull; ++i)
                {
                    Graphics.Image clock = new Graphics.Image(i < doom.turn ? SpriteName.DoomClockIcon : SpriteName.DoomClockIconGray,
                        pos, iconSz, HudLib.DungeonMasterLayer, true);
                    images.Add(clock);

                    if (i == doom.turn)
                    {
                        nextClock = clock;
                    }
                    pos.X += iconSz.X + spacing;
                }

                pos.X += typeSpacing - spacing;
            }

            

            for (int i = 0; i < doom.skulls.maxValue; ++i)
            {
                Graphics.Image skull = new Graphics.Image(
                    i < doom.skulls.Value ? SpriteName.DoomSkull : SpriteName.DoomSkullGray,
                    pos, iconSz, HudLib.DungeonMasterLayer, true);
                images.Add(skull);
                skulls.Add(skull);

                if (i == doom.skulls.Value)
                {
                    nextSkullPos = skull.Position;
                }

                pos.X += iconSz.X + spacing;
            }
            
            bgArea = new VectorRect(topLeft, new Vector2(width, iconSz.Y) + new Vector2(edge * 2f));

            addChest(DoomChestLevel.Bronze, SpriteName.BronzeChestOpen, SpriteName.BronzeChestClosed);
            addChest(DoomChestLevel.Silver, SpriteName.SilverChestOpen, SpriteName.SilverChestClosed);
            addChest(DoomChestLevel.Gold, SpriteName.GoldChestOpen, SpriteName.GoldChestClosed);
            
            var bg = new HUD.NineSplitAreaTexture(SpriteName.DoomBarFrame, 1, FrameCorner, bgArea, HudLib.BorderScale, true, 
                HudLib.DungeonMasterLayer + 1, true);

            images.Add(bg);
        }

        void addChest(DoomChestLevel chestLevel, SpriteName open, SpriteName closed)
        {
            float spacing = skulls[1].Xpos - skulls[0].Xpos;
            int onSkull = skulls.Count - hqRef.setup.conditions.doom.GetChest(chestLevel);//chest;

            Vector2 pos = new  Vector2(skulls[onSkull].Xpos - spacing * 0.5f, bgArea.Bottom - chestSz.Y);

            Graphics.Image image = new Graphics.Image(open, pos, chestSz, HudLib.DungeonMasterLayer, true);
            images.Add(image);

            //if (chest > hqRef.setup.conditions.doom.skulls.ValueRemoved)
            if (hqRef.setup.conditions.doom.chestIsOpen(chestLevel) == false)
            {
                image.Color = Color.DarkGray;
                image.SetSpriteName(closed);
            }
        }

        public void DeleteMe()
        {
            images.DeleteAll();
        }

        public static bool ViewTime => hqRef.setup.conditions.DoomClock != Data.DoomClockType.NoClock;
    }
}
