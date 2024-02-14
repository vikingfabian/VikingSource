using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.MoneyRoll
{
    static class mrollRef
    {
        public static MoneyRollState state;
        public static Map map;

        public static void ClearMem()
        {
            state = null;
            map = null;
        }
    }

    class MoneyRollState : AbsPJGameState
    {
        public MoneyRollState()
            :base(true)
        {
            mrollRef.state = this;

            Ref.draw.ClrColor = Color.DarkGray;
            new Map();
        }
    }

    class Map
    {
        public List<MapSquare> squares = new List<MapSquare>();
        Vector2 squareSz;

        public Map()
        {
            mrollRef.map = this;

            const int SquareH = 8;
            const int SquareW = 14;

            float spacing = Engine.Screen.BorderWidth;
            squareSz = new Vector2((Engine.Screen.SafeArea.Height / SquareH) - spacing);

            Vector2 startSquareSz = VectorExt.Add( squareSz * 2f, spacing);
            VectorRect startArea = new VectorRect(Engine.Screen.SafeArea.Position, startSquareSz);

            squares.Add(new MapSquare(MapSquareType.START, startArea));

            VectorRect nextPos = new VectorRect(
                startArea.Right + spacing,
                startArea.Y, 
                squareSz.X, 
                squareSz.Y);

            for (int i = 2; i < SquareW; ++i)
            {
                squares.Add(new MapSquare(MapSquareType.Empty, nextPos));

                if (i != SquareW - 1)
                {
                    nextPos.nextAreaX(1, spacing);
                }
            }

            nextPos.nextAreaY(1, spacing);
            for (int i = 1; i < SquareH; ++i)
            {
                squares.Add(new MapSquare(MapSquareType.Empty, nextPos));
                if (i != SquareH - 1)
                {
                    nextPos.nextAreaY(1, spacing);
                }
            }

            nextPos.nextAreaX(-1, spacing);
            for (int i = 1; i < SquareW; ++i)
            {
                squares.Add(new MapSquare(MapSquareType.Empty, nextPos));

                if (i != SquareW - 1)
                {
                    nextPos.nextAreaX(-1, spacing);
                }
            }

            nextPos.nextAreaY(-1, spacing);
            for (int i = 1; i < SquareH -2; ++i)
            {
                squares.Add(new MapSquare(MapSquareType.Empty, nextPos));
                if (i != SquareH - 1)
                {
                    nextPos.nextAreaY(-1, spacing);
                }
            }
        }

        
    }

    class MapSquare
    {
        MapSquareType type;
        VectorRect area;
        Graphics.Image bg;

        public MapSquare(MapSquareType type, VectorRect area)
        {
            this.type = type;
            this.area = area;

            bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.Background7);
            bg.Color = type == MapSquareType.START ? Color.Yellow : Color.Blue;

            Graphics.Text2 debugNum = new Graphics.Text2((mrollRef.map.squares.Count + 1).ToString(), LoadedFont.Console,
                area.Position, Engine.Screen.SmallIconSize, Color.White, ImageLayers.Background7_Front);
        }
    }

    enum MapSquareType
    {
        START,
        Empty,
    }

}
