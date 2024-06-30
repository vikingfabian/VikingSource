﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class WorldResources
    {
        SpottedArray<ResourceChunk> resourceRegister = new SpottedArray<ResourceChunk>();
        SpottedArrayCounter_Resource registerCounter;

        TerrainContent terrainContent = new TerrainContent();

        public WorldResources() 
        { 
            registerCounter = new SpottedArrayCounter_Resource(resourceRegister);
        }


        public int addNew(ItemResource resource)
        {
            ResourceChunk newChunk = ResourceChunk.Empty;
            newChunk.Add(resource);

            var index = registerCounter.Add(newChunk);
            return index;
        }

        public ResourceChunk get(int index)
        { 
            return registerCounter.array.Array[index];
        }

        public void update(int index, ref ResourceChunk resourceChunk)
        {
            registerCounter.array.Array[index] = resourceChunk;
        }

        public void asyncUpdate()
        { 
            ForXYLoop loop = new ForXYLoop(DssRef.world.subTileGrid.Size);

            while (loop.Next())
            {
               var subtile = DssRef.world.subTileGrid.Get(loop.Position);

                if (subtile.mainTerrain == Map.TerrainMainType.Foil)
                {
                    terrainContent.asyncFoilGroth(loop.Position, subtile);
                }
            }

            
        }

    }
}
