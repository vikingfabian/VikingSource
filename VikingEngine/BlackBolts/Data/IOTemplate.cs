using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;

namespace VikingEngine.Core.BlackBolts.Data
{
    struct IO_port
    {
        public bool input;
        public MapPlacement localPlacement, mapPlacement;
        public ResourceType resourceType;
        public int amount;
        public int collected;

        public IO_port(bool input, MapPlacement localPlacement, ResourceType resource, int amount)
        {
            this.input = input;
            this.localPlacement = localPlacement;
            this.resourceType = resource;
            this.amount = amount;
        }

        public void refreshPlacement(MapPlacement parent)
        {
            mapPlacement.tilePos = parent.tilePos + IntVector2.RotateVector_D4(localPlacement.tilePos, (int)parent.direction);
            mapPlacement.direction = lib.Rotate(localPlacement.direction, (int)parent.direction);
        }

        public bool filled()
        {
            return collected >= amount || !input;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int)localPlacement.direction + localPlacement.tilePos.X * 4 + localPlacement.tilePos.Y * 8, 
                (int)resourceType, amount);
        }
    }

    struct MachineId
    {
        public bool hasValue;
        public int inputHash;
        public int outputHash;
        public int mashineHash;

        public bool Equals(MachineId other)
        {
            return inputHash == other.inputHash &&
                   outputHash == other.outputHash &&
                   mashineHash == other.mashineHash;
        }

        public override bool Equals(object? obj)
        {
            return obj is MachineId other && Equals(other);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(inputHash, outputHash, mashineHash);
        }
        public static bool operator ==(MachineId left, MachineId right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(MachineId left, MachineId right)
        {
            return !(left == right);
        }
    }

    class IOTemplate
    {
        public MachineId id;
        public IntVector2 tilesize;
        public List<IO_port> ports = new List<IO_port>(4);

        public void buildId()
        {
            HashBuilder inHash = new HashBuilder();
            HashBuilder outHash = new HashBuilder();

            foreach (var port in ports)
            {
                if (port.input)
                {
                    inHash.Add(port.GetHashCode());
                }
                else
                { 
                    outHash.Add(port.GetHashCode());
                }
            }

            id = new MachineId
            {
                hasValue = true,
                inputHash = inHash.GetHash(),
                outputHash = outHash.GetHash(),
                mashineHash = tilesize.X + tilesize.Y * 4 + ports.Count * 8
            };
        }

    }
}
