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

        GoblinWorker,
        GoblinKnight,
        VoidDemon,
        Dragon,
        WhiteKnight,
        
        Belt,
        Spin_plate,
        Table,
        //Floor_pick,
        Dispencer,
        Delivery_point,

        Garbage_disposal,

        Floor_drop,
        Belt_dispencer,

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
        Void_egg,
        Magic_crystal,
        Fire_crystal,
        Void_cube,
        Poop,
        Burned_shit,
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
        WorkingMachine,
    }
}
