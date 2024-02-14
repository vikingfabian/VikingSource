using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.RPG
{
    class RpgPlay : AbsPJGameState
    {
        static readonly IntVector2 RoomSquareCount = new IntVector2(15, 9);
        public VectorRect gamePlayArea;
        List<LocalGamer> gamers;

        public RpgPlay(List<GamerData> joinedGamers)
            : base(true)
        {
            draw.ClrColor = Color.Black;
            set1080pScreenArea();

            float leftSideBorderW = (int)(activeScreenSafeArea.Width * 0.1f);
            gamePlayArea = activeScreenSafeArea;
            gamePlayArea.AddToLeftSide(-leftSideBorderW);

            Vector2 squareSz = new Vector2(gamePlayArea.Width / RoomSquareCount.X);

            ForXYLoop loop = new ForXYLoop(RoomSquareCount);
            while (loop.Next())
            {
                VectorRect squareArea = new VectorRect(
                    gamePlayArea.Position + loop.Position.Vec * squareSz,
                    squareSz);
                Graphics.Image square = new Graphics.Image(SpriteName.WhiteArea,
                    squareArea.Center, squareSz * 0.94f, ImageLayers.Background5, true);
                square.Color = Color.Gray;
            }


            IntVector2 startPos = new IntVector2(1, 1);
            gamers = new List<LocalGamer>(joinedGamers.Count);
            foreach (var m in joinedGamers)
            {
                var g = new LocalGamer(m, startPos);
                gamers.Add(g);
                startPos.Y++;
            }
        }
        
    }

    class LocalGamer
    {
        GamerData gamerdata;

        public LocalGamer(GamerData data, IntVector2 startPos)
        {
            this.gamerdata = data;
        }

    }
}
