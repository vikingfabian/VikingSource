using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.SmashBirds
{
    class Map
    {
        public float tileWidth;
        public float TilePerSec;
        public float Gravity;
        public float JumpForceGravity;
        public float WallStickGravity;
        public float MaxFallSpeed;
        public float DefaultWalkingSpeed;


        IntVector2 tileCount;
        VectorRect baseArea;
        Vector2 tilesTopLeft;
        Vector2 tilesTotalSz;

        public Grid2D<Tile> tileGrid;
        public Map()
        {
            SmashRef.map = this;
            Ref.draw.ClrColor = Color.CornflowerBlue;

            tileCount.Y = 20;
            tileWidth = (int)(Engine.Screen.SafeArea.Height / tileCount.Y);
            tileCount.Y += 2;

            tileCount.X = 41;
            tilesTotalSz = tileCount.Vec * tileWidth;
            
            tilesTopLeft = Engine.Screen.SafeArea.CenterTop;
            tilesTopLeft.X -= tilesTotalSz.X * 0.5f;

            tilesTopLeft = VectorExt.Round(tilesTopLeft);

            baseArea = new VectorRect(tilesTopLeft, new Vector2(tileWidth));

            physicsSetup();

            tileGrid = new Grid2D<Tile>(tileCount);
            
            for (int x = 2; x < tileCount.X - 2; ++x)
            {
                new Tile(new IntVector2(x, tileCount.Y - 2));
                new Tile(new IntVector2(x, tileCount.Y - 1));
            }

            for (int y = 0; y < tileCount.Y; ++y)
            {
                new Tile(new IntVector2(0, y));
                new Tile(new IntVector2(1, y));

                new Tile(new IntVector2(tileCount.X - 2, y));
                new Tile(new IntVector2(tileCount.X - 1, y));
            }

            //Platform
            int platformY = 11;
            for (int x = 9; x < tileCount.X - 9; ++x)
            {
                int center = tileCount.X / 2;
                if (Math.Abs(center - x) > 4)
                {
                    new Tile(new IntVector2(x, platformY));
                    new Tile(new IntVector2(x, platformY + 1));
                }
            }

            //Mid pillar
            new Tile(new IntVector2(tileCount.X / 2, tileCount.Y - 3));
        }

        void physicsSetup()
        {
            TilePerSec = tileWidth / TimeExt.SecondInMs;
            DefaultWalkingSpeed = 10f * TilePerSec;
            Gravity = tileWidth * 0.0012f;
            JumpForceGravity = Gravity * 0.1f;
            WallStickGravity = Gravity * 0.4f;
            MaxFallSpeed = DefaultWalkingSpeed * 3.6f;
        }

        public VectorRect tileArea(IntVector2 pos)
        {
            VectorRect result = baseArea;
            result.Position.X += tileWidth * pos.X;
            result.Position.Y += tileWidth * pos.Y;
            return result;
        }

        public IntVector2 wpToTilePos(Vector2 wp)
        {
            Vector2 pos = (wp - tilesTopLeft) / tileWidth;
            return new IntVector2(pos);
        }
    }

    class Tile
    {
        public Physics.RectangleBound bound;
        public IntVector2 position;

        public Tile(IntVector2 pos)
        {
            this.position = pos;
            VectorRect area = SmashRef.map.tileArea(pos);

            Graphics.Image outline = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size,
                SmashLib.LayMapTile);
            outline.Color = Color.DarkOrange;
            area.AddRadius(-1);

            Graphics.Image color = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size,
                SmashLib.LayMapTile);
            color.LayerAbove(outline);
            color.Color = Color.SandyBrown;

            bound = new Physics.RectangleBound(area.RectangleCentered);

            //ADD TO GRID
            var oldTile = SmashRef.map.tileGrid.Get(pos);

            if (oldTile != null)
            {
                throw new Exception();
            }

            SmashRef.map.tileGrid.Set(pos, this);
        }

        virtual public bool JumpThroughCollision => false;

        virtual public bool DamagePlayer => false;
    }
}
