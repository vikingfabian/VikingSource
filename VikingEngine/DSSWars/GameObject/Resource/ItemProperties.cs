﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    static class ItemPropertyColl
    {
        public const int CarryStones = 5;
        public static ItemProperties[] items;
        
        public static void Init()
        {
            items = new ItemProperties[(int)ItemResourceType.NUM];

            items[(int)ItemResourceType.HardWood] = new ItemProperties(1f / 20);
            items[(int)ItemResourceType.SoftWood] = new ItemProperties(1f / 30);
            items[(int)ItemResourceType.Stone] = new ItemProperties(1f / CarryStones);
            items[(int)ItemResourceType.IronOre] = new ItemProperties(1f / 10);
            items[(int)ItemResourceType.Egg] = new ItemProperties(1f / 60);
            items[(int)ItemResourceType.Pig] = new ItemProperties(1f);
            items[(int)ItemResourceType.Hen] = new ItemProperties(1f / 4);
            items[(int)ItemResourceType.Wheat] = new ItemProperties(1f / 10);

        }

        public static int CarryAmount(ItemResourceType item, float maxWeight = 1f)
        {
            int carry = Convert.ToInt32(maxWeight / items[(int)item].weight);
            return carry;
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
