using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Text;
using VikingEngine.Core.BlackBolts.Data;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.GO.Creature;
using VikingEngine.Core.BlackBolts.Laws;
using VikingEngine.LootFest.GO;
using VikingEngine.PJ.Tanks;
using VikingEngine.ToGG.Data.Property;

namespace VikingEngine.Core.BlackBolts.Map
{
    class MapData
    {
        Tile wallTile = new Tile() { tileType = TileType.Wall };

        public IntVector2 Size;

        public SpottedArray<AbsCreature> creatureList = new SpottedArray<AbsCreature>(1024);
        public SpottedArray<AbsMachine> machineList = new SpottedArray<AbsMachine>(1024);
        public SpottedArray<SolidResource> resourceList = new SpottedArray<SolidResource>(1024);
        public SpottedArray<CreatureSpawner> spawnerList = new SpottedArray<CreatureSpawner>(8);
        public Grid2D_L<Tile> tileGrid;

        List<PlaceObjectData> restorePoint = new List<PlaceObjectData>(1024);

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

        public void AddObject(AbsCreature go)
        {
            int ix=  creatureList.Add(go);
            go.pointer = new ObjectPointer(){ listType = ObjectListType.Creature, hasValue = true, objIndex =ix};
            tileGrid.GetRef(go.currentPos.tilePos).pCreature = go.pointer;
        }

        public void AddObject(AbsMachine go)
        {
            int ix = machineList.Add(go);
            go.pointer = new ObjectPointer() { listType = ObjectListType.Machine, hasValue = true, objIndex = ix };
            if (go.tilesize.SideLength() == 1)
            {
                tileGrid.GetRef(go.currentPos.tilePos).pMachine = go.pointer;
            }
            else
            {
                //Rectangle2 area = new Rectangle2(go.placementData.mapPlacement.tilePos, go.tilesize);
                //switch (go.placementData.mapPlacement.direction)
                //{
                //    case Dir4.E:
                //        area.size = area.size.SwapXY();
                //        break;
                //    case Dir4.S:
                //        area.pos -= area.size;
                //        break;
                //    case Dir4.W:
                //        area.size = area.size.SwapXY();
                //        area.pos -= area.size;
                //        break;
                //}

                ForXYLoop loop = new ForXYLoop(MapPlacement.CoverArea(go.placementData.mapPlacement, go.tilesize));
                while(loop.Next())
                {
                    tileGrid.GetRef(loop.Position).pMachine = go.pointer;
                }
            }
        }

        public SolidResource SpawnResource(ResourceType type)
        {
            SolidResource resource = new SolidResource(new PlaceObjectData() { resourceType = type });
            int ix = resourceList.Add(resource);
            resource.pointer = new ObjectPointer() { listType = ObjectListType.SolidResource, hasValue = true, objIndex = ix };

            return resource;
        }

        public Tile GetTile(IntVector2 pos)
        {
            if (tileGrid.TryGet(pos, out Tile tile))
                return tile;
            else
                return wallTile;
        }

        public void CreateStorePoint()
        {
            restorePoint.Clear();

            var creatureC = creatureList.counter();
            while (creatureC.Next())
            {
                restorePoint.Add(creatureC.sel.placementData);
            }

            var machineC = machineList.counter();
            while (machineC.Next())
            {
                restorePoint.Add(machineC.sel.placementData);
            }

            var spawnC = spawnerList.counter();
            while (spawnC.Next())
            {
                restorePoint.Add(spawnC.sel.placementData);
            }
        }

        public void ClearMap()
        {
            foreach (var tile in tileGrid.array)
            {
                tile.ClearTile();
            }

            creatureList.Clear();
            machineList.Clear();
            resourceList.Clear();
            spawnerList.Clear();

        }

        public void RestoreMap()
        {
            foreach (var p in restorePoint)
            {
                ObjectBuilder.Create(p, false);
            }

            restorePoint.Clear();
        }
    }

    

    class Tile
    {
        public TileType tileType = TileType.Floor;
        public TileEffect tileEffect = TileEffect.None;
        public ObjectPointer pCreature = ObjectPointer.Empty;
        public ObjectPointer pMachine = ObjectPointer.Empty;
        public ObjectPointer pResource = ObjectPointer.Empty;
        public Fluid fluid = Fluid.Empty;
        public List<ObjectPointer> nextPosList = new List<ObjectPointer>(4);

        public bool IsEmpty()
        { 
            return pCreature.hasValue == false && pMachine.hasValue == false;
        }

        public bool canPlaceResource(/*out Vector3 offset*/)
        {
            //offset = Vector3.Zero;
            if (tileType == TileType.Floor && (pResource.hasValue == false || pResource.GetSolidResource().hasBeltMove))
            {
                if (pMachine.hasValue)
                {
                    var machine = pMachine.GetMachine();
                    //offset = machine.ResourceOffset();
                    return machine.WalkableTile() || (machine.FactoryObjectType == FactoryObjectType.Table && !machine.pResource.hasValue);
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        public float groundY()
        {
            if (pMachine.hasValue)
            {
                return pMachine.GetMachine().ResourceOffset().Y;
            }
            return 0;
        }

        public PlaceObjectData? ClearTile()
        {
            PlaceObjectData? result = null; 
            if (pCreature.hasValue)
            {
                var obj = pCreature.GetCreature();
                result = obj.placementData;
                obj.DeleteMe();
                pCreature.hasValue = false;
            }
            else if (pMachine.hasValue)
            {
                var obj = pMachine.GetMachine();
                if (obj != null)
                {
                    result = obj.placementData;
                    obj.DeleteMe();
                    DestructionLaws.RemoveMachineMapPointers(obj);
                }
                //pMachine.hasValue = false;
            }

            if (pResource.hasValue)
            {
                var obj =pResource.GetSolidResource();
                obj.DeleteMe();
                pResource.hasValue = false;
            }

            fluid.clear();

            return result;
        }
    }

    

    enum TileType
    {
        Floor,
        Wall,
        
    }

    enum TileEffect
    { 
        None,
        NoBuildZone,
        Spawner,

    }
}
