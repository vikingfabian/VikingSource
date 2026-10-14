using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.Data;

namespace VikingEngine.Core.BlackBolts.Mission
{
    struct ToolSetupComponent
    {
        public FactoryObjectType objectType;

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)objectType);
        }

        public void read(System.IO.BinaryReader r, int version)
        {
            objectType = (FactoryObjectType)r.ReadByte();
        }

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
}
