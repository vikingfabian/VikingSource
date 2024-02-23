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
        public Resource resource1;
        public Resource resource2;
        public Resource resource3;
        public Resource resource4;
        public Resource resource5;
        public Resource resource6;
        public Resource resource7;
        public Resource resource8;

        public void Add(Resource resource)
        {
            if (count >= 8 || resource.type == ResourceType.NONE)
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

        public int CountResources(ResourceType resourceType, int minQuality)
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

        public bool RemoveResource(ResourceType resourceType, int minQuality)
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

        Resource removeAt(int index)
        {
            Resource result = Resource.Empty;

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

        void ShiftResource(ref Resource current, ref Resource next)
        {
            if (current.type == ResourceType.NONE && next.type != ResourceType.NONE)
            {
                current = next;
                next = new Resource();
            }
        }

        private Resource GetResourceAtIndex(int index)
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
                _ => new Resource() // Return a default Resource, assuming ResourceType.NONE is default
            };
        }

        private void SetResourceAtIndex(int index, Resource resource)
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
