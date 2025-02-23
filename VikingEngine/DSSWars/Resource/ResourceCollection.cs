﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Resource
{
    class ResourceCollection : List<ResourceChunk>
    {
        public void Add(ItemResource resource)
        {
            foreach (var chunk in this)
            {
                if (chunk.count < ResourceChunk.ChunkSize)
                {
                    chunk.Add(resource);
                    return;
                }
            }

            // If all chunks are full or there are no chunks, create a new one
            var newChunk = new ResourceChunk();
            newChunk.Add(resource);
            Add(newChunk);
        }

        public void Add(List<ResourceChunk> collection)
        {
            AddRange(collection);
        }

        public bool HasResources(ItemResourceType resourceType, int minQuality, int count)
        {
            int totalCount = 0;
            foreach (var chunk in this)
            {
                totalCount += chunk.CountResources(resourceType, minQuality);
                if (totalCount >= count)
                    return true;
            }
            return false;
        }

        public bool SpendResources(ItemResourceType resourceType, int minQuality, int count)
        {
            if (!HasResources(resourceType, minQuality, count))
                return false;

            foreach (var chunk in this)
            {
                while (chunk.RemoveResource(resourceType, minQuality))
                {
                    if (--count <= 0)
                    {
                        return true;
                    }
                }
            }

            throw new Exception("SpendResources fail");
        }
    }
}
