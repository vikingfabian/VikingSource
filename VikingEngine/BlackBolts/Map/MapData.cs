using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.LootFest.GO;
using VikingEngine.PJ.Tanks;

namespace VikingEngine.Core.BlackBolts.Map
{
    class MapData
    {
        Tile wallTile = new Tile() { tileType = TileType.Wall };

        public IntVector2 Size;

        public SpottedArray<Worker> creatureList = new SpottedArray<Worker>(1024);
        public SpottedArray<Belt> staticObjectList = new SpottedArray<Belt>(1024);
        public Grid2D_L<Tile> tileGrid;

        public MapData(IntVector2 size)
        {
            BlackRef.mapData = this;
            Size = size;
            tileGrid = new Grid2D_L<Tile>(size);
            for (int i = 0; i < tileGrid.array.Length; i++)
            {
                tileGrid.array[i] = new Tile();
            }
        }

        public void AddObject(Worker go)
        {
            int ix=  creatureList.Add(go);
            go.pointer = new ObjectPointer(){ listType = ObjectListType.Creature, hasValue = true, objIndex =ix};
            tileGrid.GetRef(go.currentPos.tilePos).pCreature = go.pointer;
        }

        public void AddObject(Belt go)
        {
            int ix = staticObjectList.Add(go);
            go.pointer = new ObjectPointer() { listType = ObjectListType.Static, hasValue = true, objIndex = ix };
            tileGrid.GetRef(go.currentPos.tilePos).pMachine = go.pointer;
        }

        public Tile GetTile(IntVector2 pos)
        {
            if (tileGrid.TryGet(pos, out Tile tile))
                return tile;
            else
                return wallTile;
        } 
    }

    

    class Tile
    {
        public TileType tileType = TileType.Floor;
        public ObjectPointer pCreature = ObjectPointer.Empty;
        public ObjectPointer pMachine = ObjectPointer.Empty;
        public List<ObjectPointer> nextPosList = new List<ObjectPointer>(4);

        public bool IsEmpty()
        { 
            return pCreature.hasValue == false && pMachine.hasValue == false;
        }
    }

    

    enum TileType
    {
        Floor,
        Wall,
        
    }
}
