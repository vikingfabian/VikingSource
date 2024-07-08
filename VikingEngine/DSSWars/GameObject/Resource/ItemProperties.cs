using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    static class ItemPropertyColl
    {
        public static ItemProperties[] items;
        
        public static void Init()
        {
            items = new ItemProperties[(int)ItemResourceType.NUM];

            items[(int)ItemResourceType.HardWood] = new ItemProperties(1f / 20);
            items[(int)ItemResourceType.SoftWood] = new ItemProperties(1f / 30);
            items[(int)ItemResourceType.IronOre] = new ItemProperties(1f / 10);
        }
    }

    class ItemProperties
    {
        /// <summary>
        /// Weight is measured in man-carry, 1 is a standard carry weight for a worker
        /// </summary>
        public float weight;

        public ItemProperties(float weight)
        { 
            this.weight = weight;
        }
    }
}
