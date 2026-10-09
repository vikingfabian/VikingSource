using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.Core.BlackBolts.Data
{
    internal class ObjectTypeLib
    {
    }

    enum ObjectCategory
    {
        NONE,
        FactoryObject,
        Resource,
        Effect,
        NUM
    }
    struct CategoryAndType
    {
        public ObjectCategory category;
        public ResourceType resource;
        public FactoryObjectType factoryobject;
        public EffectType effect;

        public CategoryAndType(ResourceType resource)
        {
            category = ObjectCategory.Resource;
            this.resource = resource;
        }
        public CategoryAndType(FactoryObjectType factoryobject)
        {
            category = ObjectCategory.FactoryObject;
            this.factoryobject = factoryobject;
        }
    }

    enum FactoryObjectType
    {
        CreatureSpawner,
        GoblinWorker,
        BlackKnight,
        NightDemon,
        Dragon,
        WhiteKnight,

        Belt_dispencer,
        Belt,
        Spin_plate,
        Floor_drop,
        Table,
        //Floor_pick,
        Dispencer,
        Delivery_point,
        Garbage_disposal,
        Stone_pillar,

        IOunit,
        NUM_NONE
    }

    enum ObjectListType
    {
        Unknown,
        Creature,
        Machine,
        SolidResource,
    }

    enum ResourceType
    {
        Box,
        Flesh,
        Bone,
        Grilled_meat,
        Dragon_egg,
        Night_egg,
        Magic_crystal,
        Fire_crystal,
        Void_cube,
        Poop,
        Burned_shit,
        Rubble,
        Old_shoe,
        Feather,
        Chicken_egg,
        Chicken,
        Job_knight,
        Job_worker,

        FluidPoopStain,
        FluidBlood,
        NUM_NONE,

        Any,
        Heat,
        Cutting,
    }

    enum EffectType
    { 
        Fire,
        Explosion,
        VoidWarp,
    }

    enum TargetTag
    { 
        Good,
        Evil,
        Creature,
        //WorkingMachine,
        Hindering,
    }

    enum DestroyType
    { 
        Default,
        Void,
        Fire,
    }
}
