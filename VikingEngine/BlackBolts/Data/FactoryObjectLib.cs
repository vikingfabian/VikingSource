using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.LootFest.BlockMap.Level;

namespace VikingEngine.Core.BlackBolts.Data
{
    struct FactoryObjectProperties
    {
        public ObjectDebugLevel debugLevel;

        public RotationType rotationType;

        public HoldResourceType holdResourceType;

        public IncludeType includeResource;


        public bool isCreature;
    }

    static class FactoryObjectLib
    {
        static FactoryObjectProperties[] factoryObjectProperties;

        public static FactoryObjectProperties Get(FactoryObjectType factoryObjectType)
        {
            return factoryObjectProperties[(int)factoryObjectType];
        }

        public static void init()
        {
            factoryObjectProperties = new FactoryObjectProperties[(int)FactoryObjectType.NUM_NONE];

            factoryObjectProperties[(int)FactoryObjectType.GoblinWorker] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnCreature,
                includeResource = IncludeType.Optional,
                isCreature = true,
            };
            factoryObjectProperties[(int)FactoryObjectType.GoblinKnight] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
            };
            factoryObjectProperties[(int)FactoryObjectType.VoidDemon] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
            };
            factoryObjectProperties[(int)FactoryObjectType.Dragon] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
            };
            factoryObjectProperties[(int)FactoryObjectType.WhiteKnight] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
            };
            factoryObjectProperties[(int)FactoryObjectType.Belt] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnFloor,
                includeResource = IncludeType.Optional,
            };
            factoryObjectProperties[(int)FactoryObjectType.Spin_plate] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.LeftRight,
                holdResourceType = HoldResourceType.OnFloor,
                includeResource = IncludeType.Optional,
            };
            factoryObjectProperties[(int)FactoryObjectType.Table] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.OnMachine,
                includeResource = IncludeType.Optional,
            };
            factoryObjectProperties[(int)FactoryObjectType.Floor_drop] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnMachine,
                includeResource = IncludeType.Optional,
            };
            factoryObjectProperties[(int)FactoryObjectType.Dispencer] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            };
            factoryObjectProperties[(int)FactoryObjectType.Belt_dispencer] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            };
            factoryObjectProperties[(int)FactoryObjectType.Delivery_point] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            };
            factoryObjectProperties[(int)FactoryObjectType.Garbage_disposal] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            };
            factoryObjectProperties[(int)FactoryObjectType.Stone_pillar] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            };
            factoryObjectProperties[(int)FactoryObjectType.IOunit] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            };
        }
    }

    enum RotationType
    { 
        None,
        LeftRight,
        Dir4,
    }

    enum IncludeType
    { 
        NoInclude,
        Optional,
        Required,
    }

    enum HoldResourceType
    { 
        NoResource,
        OnFloor,
        OnMachine,
        MachineSettings,
        OnCreature,
    }
}
