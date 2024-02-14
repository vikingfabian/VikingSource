using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.MapEditor
{
    class SelectionData
    {
        Grid2D<DataStream.MemoryStreamHandler> tiles;
        public SelectionData(Rectangle2 area)
        {
            tiles = new Grid2D<DataStream.MemoryStreamHandler>(area.size);

            ForXYLoop loop = new ForXYLoop(area);
            while (loop.Next())
            {
                IntVector2 relPos = loop.Position - area.pos;

                DataStream.MemoryStreamHandler stream = new DataStream.MemoryStreamHandler();
                var w = stream.GetWriter();
                toggRef.board.tileGrid.Get(loop.Position).writeMemory(w);

                tiles.Set(relPos, stream);
            }
        }

        public void paste(IntVector2 topLeft)
        {
            tiles.LoopBegin();
            while (tiles.LoopNext())
            {
                IntVector2 squarePos = tiles.LoopPosition + topLeft;

                var sq = toggRef.Square(squarePos);
                if (sq != null)
                {
                    sq.readMemory(tiles.LoopValueGet().GetReader(), squarePos, true);
                }
            }
        }
    }
}
