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

        public SpottedArray<Worker> worldObjects = new SpottedArray<Worker>(1024);
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
            int ix=  worldObjects.Add(go);
            go.pointer = new ObjectPointer(){ hasValue = true, objIndex =ix};
            tileGrid.GetRef(go.currentPos.tilePos).gameobject = go.pointer;
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
        public ObjectPointer gameobject = ObjectPointer.Empty;
        public List<ObjectPointer> nextPosList = new List<ObjectPointer>(4);

        public bool IsEmpty()
        { 
            return gameobject.hasValue == false;
        }
    }

    struct ObjectPointer
    {
        public static readonly ObjectPointer Empty = new ObjectPointer();
        public bool hasValue;
        public int objIndex;

        public Worker Get()
        {
            if (hasValue)
            {
                return BlackRef.mapData.worldObjects.GetIndex_Safe(objIndex);
            }
            return null;
        }
    }

    enum TileType
    {
        Floor,
        Wall,
        
    }
}
