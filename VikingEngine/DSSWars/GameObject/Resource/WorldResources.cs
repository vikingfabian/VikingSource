using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    class WorldResources
    {
        SpottedArray<ResourceChunk> resourceRegister = new SpottedArray<ResourceChunk>(4096);
        SpottedArrayCounter_Resource registerCounter;

        TerrainContent terrainContent = new TerrainContent();

        public WorldResources() 
        { 
            registerCounter = new SpottedArrayCounter_Resource(resourceRegister);
        }

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(resourceRegister.Array.Length);
            for (int i = 0; i < resourceRegister.Array.Length; i++)
            {
                resourceRegister.Array[i].writeGameState(w);
            }
        }
        public void readGameState(System.IO.BinaryReader r, int subversion)
        {
            int length = r.ReadInt32();
            resourceRegister = new SpottedArray<ResourceChunk>(length);
            registerCounter = new SpottedArrayCounter_Resource(resourceRegister);
            for (int i = 0; i < length; i++)
            {
                ResourceChunk chunk = new ResourceChunk();
                chunk.readGameState(r, subversion);
                resourceRegister.Array[i] = chunk;
            }
        }
            //int addNew(ItemResource resource)
            //{
            //    ResourceChunk newChunk = ResourceChunk.Empty;
            //    newChunk.Add(resource);

            //    var index = registerCounter.Add(newChunk);
            //    return index;
            //}

        public void addItem(ItemResource resource, ref int collIndex)
        {
            if (collIndex < 0)
            {
                ResourceChunk newChunk = ResourceChunk.Empty;
                newChunk.Add(resource);

                collIndex = registerCounter.Add(newChunk);
            }
            else
            {
                var chunk = resourceRegister.Array[collIndex];
                chunk.Add(resource);
                resourceRegister.Array[collIndex] = chunk;
            }
        }

        public ResourceChunk get(int index)
        { 
            return resourceRegister.Array[index];
        }

        public void update(int index, ref ResourceChunk resourceChunk)
        {
            resourceRegister.Array[index] = resourceChunk;
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
                else if (subtile.mainTerrain == TerrainMainType.Building)
                {
                    terrainContent.asyncCityProduce(loop.Position, subtile);
                }
            }

            
        }

    }
}
