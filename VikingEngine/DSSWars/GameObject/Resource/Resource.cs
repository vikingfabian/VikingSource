﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.Resource
{
    struct Resource
    {
        public static readonly Resource Empty = new Resource();

        public ResourceType type;

        /// <summary>
        /// Goes from 0: broken, to 255: masterpiece
        /// </summary>
        public int quality;

        /// <summary>
        /// Tracks time, risk and expences. Based on seconds of work.
        /// </summary>
        public int value;

        public Resource(ResourceType type, int quality, int value)
        {
            this.type = type;
            this.quality = quality;
            this.value = value;
        }
    }    
    
    enum ResourceType
    {
        NONE,

        Goat,
        Ox,
        Milk,
        Cheese,
        Meat,

        RawLeather, 
        Leather,

        WoodShoe,
        LeatherShoe,
        
        SoftWood,
        HardWood,
        Planks,
        Barrel,
        Box,
        Wheel,
        Wagon,
        Coal, 
        Tar,

        Linnen,
        Wool,
        Rope,
        Cloth,
        Clothes,
        Bag,

        //Todo Ore
        Iron,
        Steel,
        Tin,
        Cupper,
        Bronze,
        Silver,
        Gold,
        Mithril,
        
        Bow,
        Arrow,
        Sword,
        Shield,
        
        Padding,
        Gambeson,
        Brigandine,
        PlateArmor,

        StoneBlock,
        Stone,
        Clay,
        Brick,
        Pot,

        Wheat,
        Bread,
        Beer,

        Water,
        SaltWater,
        Poop,

        NUM
    }
}
