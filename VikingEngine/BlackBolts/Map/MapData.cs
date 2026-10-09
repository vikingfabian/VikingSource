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

        //List<PlaceObjectData> restorePoint = new List<PlaceObjectData>(1024);

        public MapData(IntVector2 size)
        {
            BlackRef.mapData = this;
            Size = size;
            tileGrid = new Grid2D_L<Tile>(size);
            for (int i = 0; i < tileGrid.array.Length; i++)
            {
                tileGrid.array[i] = new Tile();
            }

            RestoreMap();
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
            BlackRef.storage.restorePoint.Clear();

            var creatureC = creatureList.counter();
            while (creatureC.Next())
            {
                BlackRef.storage.restorePoint.Add(creatureC.sel.placementData);
            }

            var machineC = machineList.counter();
            while (machineC.Next())
            {
                BlackRef.storage.restorePoint.Add(machineC.sel.placementData);
            }

            var spawnC = spawnerList.counter();
            while (spawnC.Next())
            {
                BlackRef.storage.restorePoint.Add(spawnC.sel.placementData);
            }

            ForXYLoop loop = new ForXYLoop(tileGrid.Size);
            while (loop.Next())
            {
                if (tileGrid.GetRef(loop.Position).tileEffect == TileEffect.NoBuildZone)
                {
                    BlackRef.storage.restorePoint.Add(new PlaceObjectData()
                    {
                        mapPlacement = new MapPlacement(loop.Position, Dir4.N),
                        component = new Mission.ToolSetupComponent(FactoryObjectType.NoBuildZone),
                    });
                }
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
            foreach (var p in BlackRef.storage.restorePoint)
            {
                ObjectBuilder.Create(p, false, false);
                if (p.locked)
                {
                    tileGrid.Get(p.mapPlacement.tilePos).isLocked = true;
                }
            }

            BlackRef.storage.restorePoint.Clear();
        }
    }

    

    class Tile
    {
        public bool isLocked = false;
        public TileType tileType = TileType.Floor;
        public TileEffect tileEffect = TileEffect.None;
        public ObjectPointer pCreature = ObjectPointer.Empty;
        public ObjectPointer pMachine = ObjectPointer.Empty;
        public ObjectPointer pResource = ObjectPointer.Empty;
        public Fluid fluid = Fluid.Empty;
        public List<ObjectPointer> nextPosList = new List<ObjectPointer>(4);

        public bool IsEmpty()
        { 
            return pCreature.hasValue == false && pMachine.hasValue == false && tileEffect == TileEffect.None;
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

        public void ClearTile()
        {
            
            if (pCreature.hasValue)
            {
                var obj = pCreature.GetCreature();
                //result = obj.placementData;
                obj.DeleteMe();
                pCreature.hasValue = false;
            }
            if (pMachine.hasValue)
            {
                var obj = pMachine.GetMachine();
                if (obj != null)
                {
                    //result = obj.placementData;
                    obj.DeleteMe();
                    DestructionLaws.RemoveMachineMapPointers(obj);
                }
                pMachine.hasValue = false;
            }

            if (pResource.hasValue)
            {
                var obj =pResource.GetSolidResource();
                obj.DeleteMe();
                pResource.hasValue = false;
            }

            fluid.clear();



            tileEffect = TileEffect.None;
             isLocked = false;

            //return result;
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
