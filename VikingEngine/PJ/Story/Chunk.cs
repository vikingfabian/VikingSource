using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace VikingEngine.PJ.Story
{
    class Chunk
    {
        public const int Size = 64;
        public const int ChunkMaxPos = Size - 1;
        public IntVector2 mapGrindex;
        Grid2D<int> tileGrid;

        Graphics.GeneratedObjColor model;

        public Chunk(IntVector2 mapGrindex)
        {
            this.mapGrindex = mapGrindex;

            tileGrid = new Grid2D<int>(Size);
            tileGrid.LoopBegin();
            while (tileGrid.LoopNext())
            {
                if (lib.IsEven(tileGrid.LoopPosition.X + tileGrid.LoopPosition.Y))
                {
                    tileGrid.LoopValueSet(1);
                }
            }
        }

        public void write(BinaryWriter w)
        {
            var loop = tileGrid.LoopInstance();
            while (loop.Next())
            {
                w.Write(tileGrid.Get(loop.Position));
            }
        }
        public void read(BinaryReader r, int version)
        {
            var loop = tileGrid.LoopInstance();
            while (loop.Next())
            {
                tileGrid.Set(loop.Position, r.ReadInt32());
            }
        }

        public void generateMeshData()
        {
            List<Graphics.PolygonColor> polygons = new List<Graphics.PolygonColor>();

            var loop = tileGrid.LoopInstance();

            while (loop.Next())
            {
                if (tileGrid.Get(loop.Position) > 0)
                {
                    Graphics.PolygonColor poly = new Graphics.PolygonColor();

                    //size *= 0.5f;

                    Vector3 topLeft = storyLib.ToV3(loop.Position.Vec - new Vector2(0.5f));

                    //Top left
                    poly.V1nw.Position = topLeft;
                    //Top right
                    poly.V0sw.Position = topLeft;
                    poly.V0sw.Position.X += 1f;
                    //Bottom left
                    poly.V3ne.Position = topLeft;
                    poly.V3ne.Position.Y += 1f;
                    //Bottom right
                    poly.V2se.Position = topLeft;
                    poly.V2se.Position.X += 1f;
                    poly.V2se.Position.Y += 1f;

                    Color color = Color.Orange;
                    poly.V0sw.Color = color;
                    poly.V1nw.Color = color;
                    poly.V2se.Color = color;
                    poly.V3ne.Color = color;

                    polygons.Add(poly);
                }
            }

            model = new Graphics.GeneratedObjColor(new Graphics.PolygonsAndTrianglesColor(
                polygons, null), LoadedTexture.WhiteArea, false);

            model.position = storyLib.ToV3((mapGrindex * Size).Vec);
        }

        public void createModel()
        {
            model.AddToRender();
        }

        public void set(IntVector2 gridPos, int value)
        {
            tileGrid.Set(toLocalGridPos(gridPos), value);
        }

        public int get(IntVector2 gridPos)
        {
            return tileGrid.Get(toLocalGridPos(gridPos));
        }

        IntVector2 toLocalGridPos(IntVector2 worldGridPos)
        {
            if (worldGridPos.X < 0)
            {
                worldGridPos.X = Size + worldGridPos.X % Size;
            }
            else
            {
                worldGridPos.X = worldGridPos.X % Size;
            }

            if (worldGridPos.Y < 0)
            {
                worldGridPos.Y = Size + worldGridPos.Y % Size;
            }
            else
            {
                worldGridPos.Y = worldGridPos.Y % Size;
            }

            return worldGridPos;
        }

        public void refresh()
        {
            if (model != null)
            {
                model.DeleteMe();
            }
            generateMeshData();
            createModel();
        }
    }
}
