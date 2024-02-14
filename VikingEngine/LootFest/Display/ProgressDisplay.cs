using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.LootFest.Display
{
    class ProgressDisplay : AbsInteractDisplay
    {
        Graphics.ImageGroup images;

        public ProgressDisplay(Players.Player p)
            : base(p)
        {
            //p.deleteInteractDisplay();
            //p.interactDisplay = this;
            inputToRemove_notTimed = true;
            int BabyCount = PlayerProgress.BabyLocation_Id.Length;
            images = new Graphics.ImageGroup(BabyCount * 3);

            Vector2 center = p.SafeScreenArea.PercentToPosition(new Vector2(0.5f, 0.3f));

            const int GridCellWidth = 6;
            
            int gridCellHeight = (int)Math.Ceiling((double)BabyCount / GridCellWidth);

            float IconSz = Engine.Screen.IconSize * 1.6f;

            float spacing = IconSz * 0.2f;

            float totalW = IconSz * GridCellWidth + spacing * (GridCellWidth - 1);
            float totalH = IconSz * gridCellHeight + spacing * (gridCellHeight - 1);


            Vector2 start = center - new Vector2(totalW * 0.5f, totalH * 0.5f);

            IntVector2 gridPos = IntVector2.Zero;
            for (int i = 0; i <PlayerProgress.BabyLocation_Id.Length; ++i)
            {
                BabyLocation loc = PlayerProgress.BabyLocation_Id[i].Key;

                Vector2 pos = start + gridPos.Vec * (IconSz + spacing);
                VectorRect area = new VectorRect(pos, new Vector2(IconSz));

                Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea,
                    area.Position, area.Size, ImageLayers.Foreground5);
                area.AddRadius(-0.1f * IconSz);

                Graphics.Image bossIcon = new Graphics.Image(
                    PlayerProgress.BabyLocationIcon(loc),
                    area.Position, area.Size, ImageLayers.AbsoluteTopLayer);
                bossIcon.LayerAbove(bg);

                Graphics.Image babyIcon = new Graphics.Image(SpriteName.LfBabyIcon,
                    area.PercentToPosition(new Vector2(0.8f)), new Vector2(IconSz * 0.65f), ImageLayers.AbsoluteTopLayer, true);
                babyIcon.LayerAbove(bossIcon);
                babyIcon.Rotation = -0.2f;

                bool completed = p.Storage.progress.StoredBabyLocations.Get(loc);

                if (completed)
                {
                    bg.Color = ColorExt.VeryDarkGray;
                }
                else
                {
                    bg.Color = Color.Gray;
                    bossIcon.Color = Color.Black;
                    bossIcon.Opacity = 0.5f;
                    babyIcon.Color = Color.Black;
                }

                gridPos.X++;
                if (gridPos.X >= GridCellWidth)
                {
                    gridPos.X = 0;
                    gridPos.Y++;
                }


                images.Add(bg); images.Add(bossIcon); images.Add(babyIcon);
            }
        }

        public override void DeleteMe()
        {
            images.DeleteAll();
        }
    }


}
