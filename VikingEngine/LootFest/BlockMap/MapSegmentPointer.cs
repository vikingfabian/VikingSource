using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.BlockMap
{
    struct SquareItem
    {
        public IntVector2 position;
        public BlockMapSquare square;

        public SquareItem(IntVector2 position, BlockMapSquare square)
        {
            this.position = position;
            this.square = square;
        }
    }

    class MapSegmentPointer
    {
        IntVector2 topLeft;
        public List<SquareItem> entrances = new List<SquareItem>();
        public List<SquareItem> spawnPositions = new List<SquareItem>();
        public List<SquareItem> terrainModels = new List<SquareItem>();
        public List<SquareItem> specialPoints = new List<SquareItem>();
        public List<SquareItem> specialModels = null;// = new List<SquareItem>();
        public List<SquareItem> items = new List<SquareItem>();
        public IntVector2 landMarkIx0 = IntVector2.MinValue, landMarkIx1 = IntVector2.MinValue, landMarkIx2 = IntVector2.MinValue;

        public MapSegmentPointer(IntVector2 topLeft)
        {
            this.topLeft = topLeft;
        }

        public void addSpecial(IntVector2 pos, BlockMapSquare square, AbsLevel lvl)
        {
            switch (square.special)
            {
                case MapBlockSpecialType.SpawnPos:
                    spawnPositions.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.Entrance:
                    entrances.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.TerrainModel:
                    terrainModels.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.SpecialPoint:
                    specialPoints.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.Item:
                    items.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.SpecialModel:
                    if (specialModels == null) specialModels = new List<SquareItem>();
                    specialModels.Add(new SquareItem(pos, square));
                    break;
                case MapBlockSpecialType.Landmark:
                    switch (square.specialIndex)
                    {
                        case 0: landMarkIx0 = pos; break;
                        case 1: landMarkIx1 = pos; break;
                        case 2: landMarkIx2 = pos; break;
                    }
                    break;
            }
        }

        public SquareItem getSpecialPointIndex(int index)
        {
            foreach (var m in specialPoints)
            {
                if (m.square.specialIndex == index)
                    return m;
            }

            throw new Exception("getSpecialPointIndex");
        }

        public IntVector2 getEntranceSqPos(Dir4 dir, byte index = 0)
        {
            byte specDir = (byte)dir;
            foreach (var m in entrances)
            {
                if (m.square.specialDir == specDir && m.square.specialIndex == index)
                {
                    return m.position;
                }
            }
            throw new Exception("Missing entrance " + dir.ToString() + index.ToString());
            //return IntVector2.NegativeOne;
        }
    }
}
