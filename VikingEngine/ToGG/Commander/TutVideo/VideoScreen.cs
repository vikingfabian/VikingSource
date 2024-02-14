using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework;

namespace VikingEngine.ToGG.TutVideo
{
    class VideoScreen
    {
        public Vector2 tileStart, tilesz;
        public RenderTargetDrawContainer drawContainer = null;//Is a target image, rendering the menu content
        Graphics.Image border;

        public VideoScreen()
        {
            Vector2 size = Vector2.One;
            size.Y = Engine.Screen.Height * 0.28f;
            IntVector2 res720p = Engine.Screen.RecordingPresetsResolution(Engine.RecordingPresets.YouTube720p);
            size.X = (int)(size.Y / res720p.Y * res720p.X);

            VectorRect area = new VectorRect( VectorExt.AddX( Engine.Screen.SafeArea.RightTop, -size.X), size);

            drawContainer = new RenderTargetDrawContainer(area.Position, area.Size, ImageLayers.Background3, new List<AbsDraw>());

            VectorRect borderArea = area;

            borderArea.AddWidth(Engine.Screen.IconSize * 0.5f);
            borderArea.AddHeight(Engine.Screen.IconSize * 0.5f);

            border = new Image(SpriteName.WhiteArea, borderArea.Position, borderArea.Size, ImageLayers.AbsoluteBottomLayer);
            border.LayerBelow(drawContainer);
            border.Color = Color.Black;
        }

        public void createGrid(Vector2 startOffset_percentSz, float viewHoriTilesCount)
        {
            tilesz = new Vector2((int)(drawContainer.Size.Y / viewHoriTilesCount));
            tileStart = tilesz * startOffset_percentSz;

            IntVector2 tileCount = new IntVector2((int)Math.Ceiling((drawContainer.Size.X - tileStart.X)/ tilesz.X), (int)Math.Ceiling(viewHoriTilesCount));

            ForXYLoop loop = new ForXYLoop(tileCount);
            while (loop.Next())
            {
                Graphics.Image tile = new Image(SpriteName.cmdTutVideo_BoardTile, tileStart + loop.Position.Vec * tilesz, tilesz, ImageLayers.Bottom5, false, false);
                drawContainer.AddImage(tile);
            }
        }

        public Vector2 placeUnit(IntVector2 tile, bool friendly)
        {
            Vector2 pos = tileStart + tile.Vec * tilesz + tilesz * 0.5f;
            Vector2 sz = tilesz * 1.2f;

            Graphics.Image unit = new Image(
                friendly ? SpriteName.cmdTutVideo_FriendUnit : SpriteName.cmdTutVideo_EnemyUnit,
                pos, sz, ImageLayers.Lay4, true, false);
            drawContainer.AddImage(unit);

            return pos;
        }

        public void DeleteMe()
        {
            drawContainer.DeleteMe();
            border.DeleteMe();
        }
    }
}
