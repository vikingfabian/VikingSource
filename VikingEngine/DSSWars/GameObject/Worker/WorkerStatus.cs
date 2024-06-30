﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject.Worker
{
    struct WorkerStatus
    {
        public WorkType work;

        public float processTimeLengthSec;
        public float processTimeStartStampSec;

        public IntVector2 subTileStart;
        public IntVector2 subTileEnd;

        public ItemResource carry;

        public void WorkComplete(City city)
        {
            SubTile subTile = DssRef.world.subTileGrid.Get(subTileEnd);

            switch (work)
            { 
                case WorkType.Gather:
                    Resource.ItemResourceType resourceType;

                    switch (subTile.GetFoilType())
                    {
                        case TerrainSubFoilType.TreeHard:
                            resourceType = Resource.ItemResourceType.HardWood;
                            break;

                        case TerrainSubFoilType.TreeSoft:
                            resourceType = Resource.ItemResourceType.SoftWood;
                            break;

                        default:
                            resourceType = Resource.ItemResourceType.NONE;
                            break;
                    }

                    if (resourceType != Resource.ItemResourceType.NONE)
                    {
                        subTile.collectionPointer = DssRef.state.resources.addNew(
                            new Resource.ItemResource(
                                resourceType,
                                subTile.terrainQuality,
                                Convert.ToInt32(processTimeLengthSec),
                                subTile.terrainAmount));

                        subTile.SetType(TerrainMainType.Resourses, (int)TerrainResourcesType.Wood, 1);

                        DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                    }
                    work = WorkType.Idle;
                    break;

                case WorkType.PickUp:
                    if (subTile.collectionPointer >= 0)
                    {
                        var chunk = DssRef.state.resources.get(subTile.collectionPointer);
                        carry = chunk.pickUp(1f);
                        DssRef.state.resources.update(subTile.collectionPointer, ref chunk);

                        if (chunk.count <= 0)
                        {
                            subTile.collectionPointer = -1;
                            subTile.mainTerrain = TerrainMainType.DefaultLand;

                            DssRef.world.subTileGrid.Set(subTileEnd, subTile);
                        }
                    }
                    work = WorkType.Idle;
                    break;
                case WorkType.DropOff:
                    carry = ItemResource.Empty;
                    work = WorkType.Idle;
                    break;
            }
        }
    }
}
