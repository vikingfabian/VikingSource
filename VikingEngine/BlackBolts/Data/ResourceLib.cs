using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.PJ;
using VikingEngine.Timer;

namespace VikingEngine.Core.BlackBolts.Data
{
    

    struct ResourceProperties
    {
        public ObjectDebugLevel debugLevel;

        public bool isSolid;
        public bool isProffession;

        /// <summary>
        /// Reacts to being walked over
        /// </summary>
        public bool stompEffect;

        public CategoryAndType fireConvert;
        public CategoryAndType cutConvert;

        
    }
    static class ResourceLib
    {
        static ResourceProperties[] resourceProperties;

        public static ResourceProperties Get(ResourceType resourceType)
        {
            return resourceProperties[(int)resourceType];
        }

        public static void Init()
        {
            resourceProperties = new ResourceProperties[(int)ResourceType.NUM_NONE];

            resourceProperties[(int)ResourceType.Box] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };

            resourceProperties[(int)ResourceType.Flesh] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Flesh),
                fireConvert = new CategoryAndType(ResourceType.Grilled_meat),
            };
            resourceProperties[(int)ResourceType.Bone] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Grilled_meat] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Dragon_egg] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(FactoryObjectType.Dragon),
            };
            resourceProperties[(int)ResourceType.Void_egg] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(FactoryObjectType.VoidDemon),
            };
            resourceProperties[(int)ResourceType.Magic_crystal] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
                
            };
            resourceProperties[(int)ResourceType.Fire_crystal] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Void_cube] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Poop] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
                stompEffect = true,
            };
            resourceProperties[(int)ResourceType.Burned_shit] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
                stompEffect = true,
            };
            resourceProperties[(int)ResourceType.Old_shoe] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Feather] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Chicken_egg] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Flesh),
                fireConvert = new CategoryAndType(ResourceType.Grilled_meat),
            };
            resourceProperties[(int)ResourceType.Chicken] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Job_knight] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
            resourceProperties[(int)ResourceType.Job_worker] = new ResourceProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
                isSolid = true,
                cutConvert = new CategoryAndType(ResourceType.Poop),
                fireConvert = new CategoryAndType(ResourceType.Burned_shit),
            };
        }
    }

    enum ObjectDebugLevel
    { 
        Incomplete,
        Develop,
        Retail,
    }

}
