using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.Mission
{
    struct ToolSetupComponent
    {
        public FactoryObjectType objectType;

        public ToolSetupComponent(FactoryObjectType objectType)
        {
            this.objectType = objectType;
        }
        public bool Equals(ToolSetupComponent other)
        {
            return objectType == other.objectType;
        }

        public override bool Equals(object obj)
        {
            return obj is ToolSetupComponent other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(objectType);
        }

        public static bool operator ==(ToolSetupComponent left, ToolSetupComponent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ToolSetupComponent left, ToolSetupComponent right)
        {
            return !left.Equals(right);
        }

        public FactoryObjectProperties properties()
        {
            return FactoryObjectLib.Get(objectType);
        }
    }

    abstract class AbsMissionSetup
    {
        public List<ToolSetupComponent> componentList;
        public List<IOTemplate> ioUnits;
    }

    class SandboxSetup : AbsMissionSetup
    {
        public SandboxSetup()
        {
            componentList = new List<ToolSetupComponent>((int)FactoryObjectType.NUM_NONE);

            for (FactoryObjectType fobj = 0; fobj < FactoryObjectType.NUM_NONE; fobj++)
            {
                if (FactoryObjectLib.Get(fobj).debugLevel > ObjectDebugLevel.Incomplete)
                {
                    componentList.Add(new ToolSetupComponent(fobj));
                }
            }

            IOTemplate chickenSeperator = new IOTemplate() { name = "Chicken seperator", tilesize = new IntVector2(2, 1) };
            chickenSeperator.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                ResourceType.Chicken, 1));
            chickenSeperator.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                ResourceType.Feather, 2));
            chickenSeperator.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                ResourceType.Flesh, 1));

            IOTemplate duplicator = new IOTemplate() { name = "Duplicator", tilesize = new IntVector2(2, 1) };
            duplicator.ports.Add(new IO_port(true, new Map.MapPlacement(IntVector2.Zero, Dir4.W),
                ResourceType.Any, 1));
            duplicator.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.N),
                ResourceType.Any, 1));
            duplicator.ports.Add(new IO_port(false, new Map.MapPlacement(IntVector2.Right, Dir4.S),
                ResourceType.Any, 1));

            ioUnits = new List<IOTemplate> { chickenSeperator, duplicator };
            foreach (var io in ioUnits)
            {
                io.buildModel();
                io.buildId();
            }
        }
    }
}
