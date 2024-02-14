using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.LootFest.Editor
{
    class EditorPacket
    {
        Network.ReceivedPacket packet;
        IntervalIntV3 volume;
        public Map.WorldPosition minWp, maxWp;

        public EditorPacket(Network.ReceivedPacket packet)
        {
            this.packet = packet;
            volume = Voxels.EditorDrawTools.NetReadVoxelEditVolume(packet);

            var updateArea = volume;
            updateArea.AddRadius(1);
            minWp = new LootFest.Map.WorldPosition(updateArea.Min);
            maxWp = new LootFest.Map.WorldPosition(updateArea.Max);

            LfRef.world.addEditorPacket(this);
        }

        public void read()
        {
            Voxels.EditorDrawTools.NetReadVoxelEdit(packet, volume);

            LootFest.Map.World.ReloadChunkMesh(minWp, maxWp, true);
        }

    }
}
