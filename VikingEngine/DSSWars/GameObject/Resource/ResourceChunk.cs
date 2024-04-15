using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    struct ResourceChunk
    {
        public const int ChunkSize = 8;

        public int count;
        public ItemResource resource1;
        public ItemResource resource2;
        public ItemResource resource3;
        public ItemResource resource4;
        public ItemResource resource5;
        public ItemResource resource6;
        public ItemResource resource7;
        public ItemResource resource8;

        public void Add(ItemResource resource)
        {
            if (count >= 8 || resource.type == ItemResourceType.NONE)
            {
                throw new InvalidOperationException("ResourceChunk is full or resource is invalid.");
            }

            switch (count)
            {
                case 0: resource1 = resource; break;
                case 1: resource2 = resource; break;
                case 2: resource3 = resource; break;
                case 3: resource4 = resource; break;
                case 4: resource5 = resource; break;
                case 5: resource6 = resource; break;
                case 6: resource7 = resource; break;
                case 7: resource8 = resource; break;
            }

            ++count;
        }

        public int CountResources(ItemResourceType resourceType, int minQuality)
        {
            int matchingResources = 0;
            if (resource1.type == resourceType && resource1.quality >= minQuality) matchingResources++;
            if (resource2.type == resourceType && resource2.quality >= minQuality) matchingResources++;
            if (resource3.type == resourceType && resource3.quality >= minQuality) matchingResources++;
            if (resource4.type == resourceType && resource4.quality >= minQuality) matchingResources++;
            if (resource5.type == resourceType && resource5.quality >= minQuality) matchingResources++;
            if (resource6.type == resourceType && resource6.quality >= minQuality) matchingResources++;
            if (resource7.type == resourceType && resource7.quality >= minQuality) matchingResources++;
            if (resource8.type == resourceType && resource8.quality >= minQuality) matchingResources++;

            return matchingResources;
        }

        public bool RemoveResource(ItemResourceType resourceType, int minQuality)
        {
            for (int i = count - 1; i >= 0; --i)
            {
                var resource = GetResourceAtIndex(i);//todo, alltid plocka sämst
                if (resource.type == resourceType && resource.quality >= minQuality)
                {
                    removeAt(i); return true;
                }
            }

            return false;
        }

        ItemResource removeAt(int index)
        {
            ItemResource result = ItemResource.Empty;

            switch (index)
            {
                case 0: result = resource1; break;
                case 1: result = resource2; break;
                case 2: result = resource3; break;
                case 3: result = resource4; break;
                case 4: result = resource5; break;
                case 5: result = resource6; break;
                case 6: result = resource7; break;
                case 7: result = resource8; break;
            }

            ShiftResource(ref resource1, ref resource2);
            ShiftResource(ref resource2, ref resource3);
            ShiftResource(ref resource3, ref resource4);

            if (count > 4)
            {
                ShiftResource(ref resource4, ref resource5);
                ShiftResource(ref resource5, ref resource6);
                ShiftResource(ref resource6, ref resource7);
                ShiftResource(ref resource7, ref resource8);
            }
            --count;

            return result;
        }

        void ShiftResource(ref ItemResource current, ref ItemResource next)
        {
            if (current.type == ItemResourceType.NONE && next.type != ItemResourceType.NONE)
            {
                current = next;
                next = new ItemResource();
            }
        }

        private ItemResource GetResourceAtIndex(int index)
        {
            return index switch
            {
                0 => resource1,
                1 => resource2,
                2 => resource3,
                3 => resource4,
                4 => resource5,
                5 => resource6,
                6 => resource7,
                7 => resource8,
                _ => new ItemResource() // Return a default Resource, assuming ResourceType.NONE is default
            };
        }

        private void SetResourceAtIndex(int index, ItemResource resource)
        {
            switch (index)
            {
                case 0: resource1 = resource; break;
                case 1: resource2 = resource; break;
                case 2: resource3 = resource; break;
                case 3: resource4 = resource; break;
                case 4: resource5 = resource; break;
                case 5: resource6 = resource; break;
                case 6: resource7 = resource; break;
                case 7: resource8 = resource; break;
            }
        }


    }
}
