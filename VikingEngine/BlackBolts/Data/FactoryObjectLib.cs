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

        public ThirtyTwoBit tags;

        public bool isCreature;

        public FactoryObjectProperties AddTag(TargetTag tag)
        {
            tags.Set((int)tag, true);
            return this;
        }
        public FactoryObjectProperties AddTag(TargetTag tag1, TargetTag tag2)
        {
            tags.Set((int)tag1, true);
            tags.Set((int)tag2, true);
            return this;
        }
        public FactoryObjectProperties AddTag(TargetTag tag1, TargetTag tag2, TargetTag tag3)
        {
            tags.Set((int)tag1, true);
            tags.Set((int)tag2, true);
            tags.Set((int)tag3, true);
            return this;
        }
        public bool HasTag(TargetTag tag)
        {
            return tags.Get((int)tag);
        }

        public bool IsEnemyTarget(bool goodSide)
        {
            if (goodSide)
            {
                return tags.Get((int)TargetTag.Hindering) && tags.Get((int)TargetTag.Evil);
            }
            else
            {
                return tags.Get((int)TargetTag.Good);
            }
        }
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


            factoryObjectProperties[(int)FactoryObjectType.CreatureSpawner] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
            };

            factoryObjectProperties[(int)FactoryObjectType.GoblinWorker] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnCreature,
                includeResource = IncludeType.Optional,
                isCreature = true,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering, TargetTag.Creature);
                        
            factoryObjectProperties[(int)FactoryObjectType.NightDemon] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
                isCreature = true,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering, TargetTag.Creature);
            
            factoryObjectProperties[(int)FactoryObjectType.Dragon] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
                isCreature = true,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering, TargetTag.Creature);

            factoryObjectProperties[(int)FactoryObjectType.WhiteKnight] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
                isCreature = true,
            }.AddTag(TargetTag.Good, TargetTag.Hindering, TargetTag.Creature);

            factoryObjectProperties[(int)FactoryObjectType.BlackKnight] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
                isCreature = true,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering, TargetTag.Creature);

            factoryObjectProperties[(int)FactoryObjectType.Belt] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnFloor,
                includeResource = IncludeType.Optional,
            }.AddTag(TargetTag.Evil);

            factoryObjectProperties[(int)FactoryObjectType.Spin_plate] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.LeftRight,
                holdResourceType = HoldResourceType.OnFloor,
                includeResource = IncludeType.Optional,
            }.AddTag(TargetTag.Evil);

            factoryObjectProperties[(int)FactoryObjectType.Table] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.OnMachine,
                includeResource = IncludeType.Optional,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Floor_drop] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.OnMachine,
                includeResource = IncludeType.Optional,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Dispencer] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Belt_dispencer] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Delivery_point] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.MachineSettings,
                includeResource = IncludeType.Required,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Garbage_disposal] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.Stone_pillar] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Retail,
                rotationType = RotationType.None,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);

            factoryObjectProperties[(int)FactoryObjectType.IOunit] = new FactoryObjectProperties()
            {
                debugLevel = ObjectDebugLevel.Incomplete,
                rotationType = RotationType.Dir4,
                holdResourceType = HoldResourceType.NoResource,
                includeResource = IncludeType.NoInclude,
            }.AddTag(TargetTag.Evil, TargetTag.Hindering);
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
