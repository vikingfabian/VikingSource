using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Resource
{
    class WorldResources
    {
        SpottedArray<ResourceChunk> resourceRegister = new SpottedArray<ResourceChunk>(4096);
        SpottedArrayCounter_Resource registerCounter;

        TerrainContent terrainContent = new TerrainContent();
        public ConcurrentQueue<EditSubTile> editSubTiles = new ConcurrentQueue<EditSubTile>();
        public ConcurrentQueue<AbsRbAction> editSubTilesActionQueue = new ConcurrentQueue<AbsRbAction>();
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
                resourceRegister.adjustMinimumLength(collIndex +1);

                var chunk = resourceRegister.Array[collIndex];
                chunk.Add(resource);
                resourceRegister.Array[collIndex] = chunk;
            }
        }

        public ResourceChunk get(int index)
        {
            if (index < resourceRegister.Array.Length)
            {
                return resourceRegister.Array[index];
            }
            return ResourceChunk.Empty;
        }

        public void update(int index, ref ResourceChunk resourceChunk)
        {
            resourceRegister.Array[index] = resourceChunk;
        }

        public void asyncGrowUpdate()
        {           

            ForXYLoop loop = new ForXYLoop(DssRef.world.subTileGrid.Size);

            while (loop.Next())
            {
                while (editSubTiles.TryDequeue(out var edit))
                {
                    edit.ExecuteEdit();
                }

                ref var subtile = ref DssRef.world.subTileGrid.GetRef(loop.Position);

                if (subtile.mainTerrain == TerrainMainType.Foil)
                {
                    terrainContent.asyncFoilGroth(loop.Position, ref subtile);
                }
                else if (subtile.mainTerrain == TerrainMainType.Building)
                {
                    terrainContent.asyncCityProduce(loop.Position, ref subtile);
                }
            }
        }

        public void asyncEditTiles()
        {
            while (editSubTiles.TryDequeue(out var edit))
            {
                edit.ExecuteEdit();
            }

            while (editSubTilesActionQueue.TryDequeue(out var action))
            {
                action.actionTrigger();
            }
        }

    }
}
