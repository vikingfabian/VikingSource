using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    class PreviewMarker
    {
        BrickBox box;
        List<Graphics.Image> markers;
        List<RowOfBricks> columns;

        public PreviewMarker(BrickBox box, List<FallingBlockMember> bricks)
        {
            this.box = box;
            markers = new List<Graphics.Image>(bricks.Count);
            columns = new List<RowOfBricks>(3);

           // update(bricks);
        }

        public void update(List<FallingBlockMember> bricks)
        {
            columns.Clear();

            foreach (var m in bricks)
            {
                addToColum(m);
            }


            arraylib.DeleteAndClearArray(markers);

            foreach (var m in columns)
            {
                if (m.columnX >= 0 && m.columnX < BrickBox.BrickCountSz.X)
                {
                    bool center = m.highToLowList[0].offset.X == 0;
                    IntVector2 topPos = box.getTopFreePos(m.highToLowList[0].brick.gridPos);

                    foreach (var brick in m.highToLowList)
                    {
                        var centerPos = box.worldPos(topPos);
                        Graphics.Image previewBrick = new Graphics.Image(brick.brick.previewSprite,
                            centerPos, new Vector2(m3Ref.TileWidth), m3Lib.LayerMarkEndPos, true);
                        previewBrick.Opacity = 0.2f;
                        markers.Add(previewBrick);

                        if (center)
                        {
                            Graphics.Image outline = new Graphics.Image(SpriteName.WhiteArea_LFtiles,
                                previewBrick.position, previewBrick.size, ImageLayers.AbsoluteBottomLayer, true);
                            outline.LayerBelow(previewBrick);
                            outline.Opacity = 0.5f;//center ? 0.6f : 0.1f;
                            markers.Add(outline);
                        }
                        topPos.Y--;
                        center = false;
                    }
                }
            }
        }

        void addToColum(FallingBlockMember m)
        {
            foreach (var column in columns)
            {
                if (column.columnX == m.brick.gridPos.X)
                {
                    column.add(m);
                    return;
                }
            }

            columns.Add(new RowOfBricks(m));
        }



        public void DeleteMe()
        {
            arraylib.DeleteAndClearArray(markers);
        }

        class RowOfBricks
        {
            public int columnX;
            public List<FallingBlockMember> highToLowList = new List<FallingBlockMember>(3);

            public RowOfBricks(FallingBlockMember m)
            {
                highToLowList = new List<FallingBlockMember> { m };
                columnX = m.brick.gridPos.X;
            }

            public void add(FallingBlockMember m)
            {
                for (int i = 0; i < highToLowList.Count; ++i)
                {
                    if (m.brick.gridPos.Y > highToLowList[i].brick.gridPos.Y)
                    {
                        highToLowList.Insert(i, m);
                        return;
                    }
                }

                highToLowList.Add(m);
            }
        }
    }
}
