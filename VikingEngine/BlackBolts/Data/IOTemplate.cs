using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO;
using VikingEngine.Core.BlackBolts.Map;
using VikingEngine.Voxels;

namespace VikingEngine.Core.BlackBolts.Data
{
    struct IO_port
    {
        public bool input;
        public MapPlacement localPlacement, mapPlacement;
        public ResourceType resourceType;
        public int amount;
        public int collected;
        public bool isTabledispence;

        public IO_port(bool input, MapPlacement localPlacement, ResourceType resource, int amount)
        {
            this.input = input;
            this.localPlacement = localPlacement;
            mapPlacement = localPlacement;
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

        public IntVector2 ForwardPos()
        {
            return isTabledispence ? mapPlacement.tilePos : mapPlacement.ForwardPos().tilePos;
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

        public IOTemplate GetTemplate()
        {
            foreach (var m in BlackRef.playScene.missionSetup.ioUnits)
            {
                if (this.Equals(m.id))
                {
                    return m;
                }
            }

            throw new Exception();
        }
    }

    class IOTemplate
    {
        public Graphics.VoxelModel model;
        public int frame = 0;
        public float scale = 1.9f;
        public MachineId id;
        public string name;
        public IntVector2 tilesize;
        public List<IO_port> ports = new List<IO_port>(4);

        public void buildModel()
        {
            if (model == null)
            {
                ModelBuilder modelBuilder = new ModelBuilder();
                VoxelObjGridDataAnimHD grid = new VoxelObjGridDataAnimHD(
                    new List<VoxelObjGridDataHD> { BlackRef.models.rawModels[LootFest.VoxelModelName.bb_io_base].Frame(0).Clone() });
                foreach (var port in ports)
                {
                    IntVector3 offset = IntVector3.Zero;
                    int frame = (int)port.localPlacement.direction;
                    if (!port.input)
                    {
                        frame += 4;
                    }
                    offset.X += 16;
                    offset.XZ -= port.localPlacement.tilePos * 16;

                    grid.Frame(0).Merge(BlackRef.models.rawModels[LootFest.VoxelModelName.bb_io_mark].Frame(frame),
                        true, true, offset);
                }

                var centerAdjust = grid.Frame(0).BottomCenterAdj();
                modelBuilder.buildVerticeDataHD_ColorNormal(grid.Frames, centerAdjust);
                model = modelBuilder.modelFromVertices();
            }
        }

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
