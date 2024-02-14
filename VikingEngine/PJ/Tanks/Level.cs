using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Tanks
{
    class Level
    {
        VectorRect tileScreenArea;
        VectorRect topLeftTile;

        public Grid2D<Tile> grid;
        bool needTileRefresh = false;

        public Level()
        {
            tankRef.level = this;

            Ref.draw.ClrColor = Color.LawnGreen;

            float tileScale = MathExt.RoundAndEven(Engine.Screen.SafeArea.Height * 0.05f);
            tileScreenArea = Engine.Screen.SafeArea;
            tileScreenArea.AddRadius(tileScale * 0.5f);
            IntVector2 gridSz = new IntVector2(tileScreenArea.Size / tileScale);
            grid = new Grid2D<Tile>(gridSz);

            tileScreenArea = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, gridSz.Vec * tileScale);
            topLeftTile = new VectorRect(tileScreenArea.Position, new Vector2(tileScale));

            tankLib.Init(tileScale);

            initLevel();

            needTileRefresh = true;
        }

        void initLevel()
        {
            ForXYEdgeLoop edgeLoop = new ForXYEdgeLoop(grid.Size);
            while (edgeLoop.Next())
            {
                addTile(edgeLoop.Position, TileType.Steel);
            }

            IntVector2 center = grid.Size / 2;
            int crossRad = grid.Height / 5;

            IntVector2 pos = center;
            for (pos.X = center.X - crossRad; pos.X <= center.X + crossRad; ++pos.X)
            {
                addTile(pos, TileType.Steel);
            }

            pos = center;
            for (pos.Y = center.Y - crossRad; pos.Y <= center.Y + crossRad; ++pos.Y)
            {
                addTile(pos, TileType.Steel);
            }
        }

        public void update_asynch()
        {
            if (needTileRefresh)
            {
                needTileRefresh = false;

                var loop = grid.LoopInstance();
                while (loop.Next())
                {
                    grid.Get(loop.Position)?.refreshNeighbors();
                }
            }
        }

        void addTile(IntVector2 pos, TileType type)
        {
            if (grid.Get(pos) == null)
            {
                grid.Set(pos, new Tile(pos, type));
            }
        }

        public VectorRect tileArea(IntVector2 pos)
        {
            return topLeftTile.nextTilePos(pos);
        }

        public IntVector2 wpToTilePos(Vector2 wp)
        {
            wp -= topLeftTile.Position;
            return new IntVector2(wp.X / tankLib.tileScale, wp.Y / tankLib.tileScale);
        }
    }

    class Tile
    {
        public IntVector2 position;
        public TileType type;
        Graphics.Image image;
        public Physics.RectangleBound bound;

        public bool neighborN, neighborE, neighborS, neighborW;

        public Tile(IntVector2 pos, TileType type)
        {
            this.position = pos;
            this.type = type;
            VectorRect area = tankRef.level.tileArea(pos);
            image = new Graphics.Image(SpriteName.golfBrickBlue, area.Position, area.Size, tankLib.LayerTiles);
            bound = new Physics.RectangleBound(area.RectangleCentered);
        }

        public void refreshNeighbors()
        {
            neighborN = neighborCheck(VectorExt.AddY(position, -1));
            neighborS = neighborCheck(VectorExt.AddY(position, 1));
            neighborW = neighborCheck(VectorExt.AddX(position, -1));
            neighborE = neighborCheck(VectorExt.AddX(position, 1));


            bool neighborCheck(IntVector2 pos)
            {
                Tile nTile;
                if (tankRef.level.grid.TryGet(pos, out nTile))
                {
                    return nTile != null;
                }

                return false;
            }
        }
    }

    enum TileType
    {
        None,
        Steel,
    }
}
