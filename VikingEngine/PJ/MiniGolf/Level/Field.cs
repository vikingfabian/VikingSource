using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MiniGolf
{
    class Field
    {
        const int SquaresW = 32;
        const int SquaresH = 20;
        public const float LowSpeedFrictionMulti = 0.95f;

        public Level.LevelVisualsSetup visualSetup;
        VectorRect topLeftSquare;
        public Vector2 squareSize, squaresStart;
        public float itemsExtremeRadius;
        public VectorRect outerBound;
        
        public IntVector2 cornersCount = new IntVector2(SquaresW, SquaresH) + 1;

        FieldSquare defaultEmptySquare = new FieldSquare();
        public Grid2D<FieldSquare> squares;

        public FieldStorage storage;
        public Dir8 launchAngle = Dir8.N;

        public FieldDataCollection dataCollection;
        Graphics.ImageGroup terrainImages = new Graphics.ImageGroup(32);

        public Field(int matchCount)
        {
            GolfRef.field = this;
            visualSetup = new Level.LevelVisualsSetup(matchCount);
            storage = new FieldStorage();

            outerBound = Engine.Screen.SafeArea;

            squareSize = new Vector2((int)(outerBound.Height / SquaresH));
            itemsExtremeRadius = squareSize.X * 3f;

            outerBound.Width = squareSize.X * SquaresW;
            outerBound.X = Engine.Screen.SafeArea.Right - outerBound.Width;

            squaresStart = outerBound.Position;

            topLeftSquare = new VectorRect(squaresStart, squareSize);

            Ref.draw.ClrColor = Color.Black;

            fieldTexture();

            squares = new Grid2D<FieldSquare>(new IntVector2(SquaresW, SquaresH));
            squares.LoopBegin();
            while (squares.LoopNext())
            {
                squares.LoopValueSet(new FieldSquare());
            }
        }

        void fieldTexture()
        {
            Ref.draw.CurrentRenderLayer = Draw.BackLayer;

            Vector2 textureSz = squareSize * 4f;
            Vector2 topLeft = outerBound.Position - squareSize * 2f;
            Vector2 pos = topLeft;
            do
            {
                pos.X = topLeft.X;
                do
                {
                    new Graphics.Image(visualSetup.groundTex, pos, textureSz, GolfLib.FieldLayer);

                    pos.X += textureSz.X;
                } while (pos.X < Engine.Screen.Area.Right);
                pos.Y += textureSz.Y;
            } while (pos.Y < Engine.Screen.Area.Bottom);

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        void onLoadedData()
        {
            GolfRef.objects.createEdgeCorners();
            dataCollection = new FieldDataCollection();
            new GO.ItemsLayout(dataCollection);    
        }

        public IntVector2 toSquarePos(Vector2 screenPos)
        {
            if (outerBound.IntersectPoint(screenPos))
            {
                Vector2 pos = (screenPos - squaresStart) / squareSize;
                return new IntVector2((int)pos.X, (int)pos.Y);
            }
            return IntVector2.NegativeOne;
        }

        public FieldSquare getSquare(Vector2 pos)
        {
            return squares.Get(toSquarePos(pos));
        }

        public FieldSquare tryGetSquare(Vector2 pos)
        {
            FieldSquare square;
            if (squares.TryGet(toSquarePos(pos), out square))
            {
                return square;
            }

            return defaultEmptySquare;
        }

        public VectorRect toSquareScreenArea(IntVector2 squarePos)
        {
            return topLeftSquare.nextTilePos(squarePos);
            //result.Position.X += squareSize.X * squarePos.X;
            //result.Position.Y += squareSize.Y * squarePos.Y;
            //return result;
        }

        public Vector2 cornerPos(IntVector2 corner)
        {
            return new Vector2(
                corner.X * squareSize.X + squaresStart.X,
                corner.Y * squareSize.Y + squaresStart.Y);
        }
        public Vector2 centerPos(IntVector2 corner)
        {
            return new Vector2(
                (corner.X + 0.5f)* squareSize.X + squaresStart.X,
                (corner.Y + 0.5f) * squareSize.Y + squaresStart.Y);
        }

        public void addTerrain(Rectangle2 area, FieldTerrainType terrain)
        {
            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                addTerrain(loop.Position, terrain);
            }
        }

        public void addTerrain(IntVector2 squarePos, FieldTerrainType terrain)
        {
            squares.Get(squarePos).terrain = terrain;
        }

        public void createTerrainTextures()
        {
            squares.LoopBegin();
            while (squares.LoopNext())
            {
                var sq = squares.LoopValueGet();

                if (sq.terrain == FieldTerrainType.Sand)
                {
                    bool east = false;
                    bool south = false;
                    bool southeast = false;

                    FieldSquare nSq;
                    if (squares.TryGet(VectorExt.AddX(squares.LoopPosition, 1), out nSq))
                    {
                        east = nSq.terrain == FieldTerrainType.Sand;
                    }
                    if (squares.TryGet(VectorExt.AddY(squares.LoopPosition, 1), out nSq))
                    {
                        south = nSq.terrain == FieldTerrainType.Sand;
                    }
                    if (east && south &&
                        squares.TryGet(squares.LoopPosition + IntVector2.One, out nSq))
                    {
                        southeast = nSq.terrain == FieldTerrainType.Sand;
                    }


                    var area = toSquareScreenArea(squares.LoopPosition);
                    placeSand(area.Center);
                    if (east)
                    {
                        placeSand(area.RightCenter);
                    }
                    if (south)
                    {
                        placeSand(area.CenterBottom);
                    }
                    if (southeast)
                    {
                        placeSand(area.RightBottom);
                    }


                }
                else if (sq.terrain == FieldTerrainType.Spikes)
                {
                    Ref.draw.CurrentRenderLayer = Draw.BackLayer;

                    var area = toSquareScreenArea(squares.LoopPosition);
                    Graphics.Image spikes = new Graphics.Image(SpriteName.golfSpikeSquare, area.Position, area.Size, GolfLib.FieldTerrainLayer);

                    Ref.draw.CurrentRenderLayer = Draw.HudLayer;
                }
            }
        }

        void placeSand(Vector2 center)
        {
            Ref.draw.CurrentRenderLayer = Draw.BackLayer;

            TwoSprites tex = arraylib.RandomListMember(visualSetup.sandTex);

            Graphics.Image low = new Graphics.Image(tex.image1, center, squareSize * 2.2f,
                GolfLib.FieldTerrainLayer, true);
            low.ChangePaintLayer(-terrainImages.images.Count);

            Graphics.Image high = new Graphics.Image(tex.image2, low.Position, low.Size,
                GolfLib.FieldTerrainLayer-3, true);
            high.ChangePaintLayer(-terrainImages.images.Count);

            terrainImages.Add(low);
            terrainImages.Add(high);

            Ref.draw.CurrentRenderLayer = Draw.HudLayer;
        }

        public void clearMap(bool clearName)
        {
            GolfRef.objects.clearMap();
            if (clearName)
            {
                GolfRef.field.storage.saveFileName = FieldStorage.RandomName();
            }

            squares.LoopBegin();
            while (squares.LoopNext())
            {
                squares.LoopValueGet().Clear();
            }
            terrainImages.DeleteAll();

            refreshSquareText();
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)GolfRef.objects.edges.Count);
            foreach (var m in GolfRef.objects.edges)
            {
                m.write(w);
            }

            var loop = squares.LoopInstance();
            while (loop.Next())
            {
                squares.Get(loop.Position).write(w);
            }

            w.Write((byte)launchAngle);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            int edgesCount = r.ReadByte();
            for (int i = 0; i < edgesCount; ++i)
            {
                new FieldEdge(r);
            }

            var loop = squares.LoopInstance();
            while (loop.Next())
            {
                squares.Get(loop.Position).read(r, version);
            }

            if (version >= 1)
            {
                launchAngle = (Dir8)r.ReadByte();
            }

            onLoadedData();
            
        }

        public void refreshSquareText()
        {
            if (GolfRef.editor != null)
            {
                squares.LoopBegin();
                while (squares.LoopNext())
                {
                    squares.LoopValueGet().editorText.TextString = squares.LoopValueGet().SquareText();
                }
            }
        }
    }

    
    
}
