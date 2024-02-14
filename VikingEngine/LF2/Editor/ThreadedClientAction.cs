using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//

namespace VikingEngine.LF2.Editor
{
    class ThreadedClientAction : Network.ThreadedSystem.IO.BinaryReader
    {
        LF2.Map.WPRange updateRange;
        Players.ClientPlayer cp;
        public ThreadedClientAction(System.IO.BinaryReader r, Network.AbsNetworkPeer sender, Players.ClientPlayer cp, Network.PacketType type)
            : base(type, r, sender)
        {
            this.cp = cp;
            start();
        }
        protected override bool ReadPacket(Network.PacketType type, System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
        {
            switch (type)
            {
                case Network.PacketType.LF2_VoxelEditorDottedLine:
                    updateRange = VoxelDesigner.NetworkReadDottedLine(r, cp);
                    break;
                case Network.PacketType.LF2_VoxelEditorDrawRect:
                    updateRange = VoxelDesigner.NetworkReadDrawRect(r, cp);
                    break;
                case Network.PacketType.LF2_VoxelEditorAddTemplate:
                    updateRange = VoxelDesigner.NetworkReadTemplate(r, cp);
                    break;
            }
            
            return true;
        }
        public override void Time_Update(float time)
        {
            VoxelDesigner.UpdateMapArea(updateRange.Min, updateRange.Max);
        }
    }
    
}
