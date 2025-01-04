using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;

namespace VikingEngine.DSSWars.Players
{

    partial class RemotePlayer
    {
        public const int OverviewSendChunkSize = 8;

        public Grid2D<RemoteTile> remoteTileGrid;


        public void InitData()
        {
            remoteTileGrid = new Grid2D<RemoteTile>(DssRef.world.Size);
        }

        public void Net_HostMapUpdate_async()
        {
            

            if (playerCulling.enterArea.size.HasValue())
            {
                //if (remoteTileGrid.Get(playerCulling.enterArea.pos).tileStatus == 0 ||
                //    remoteTileGrid.Get(playerCulling.enterArea.BottomRight).tileStatus == 0)
                //{ 
                //    //Send 
                //}

                
                if (findMissingTile(out IntVector2 tilePos, false))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_Tile(w, tilePos);
                    }
                    packet.EndWrite_Asynch();
                }
                else if (findMissingTile(out IntVector2 subtilePos, true))
                {
                    var w = Ref.netSession.BeginWritingPacket_Asynch(Network.PacketType.DssWorldSubTiles, Network.PacketReliability.Reliable, out var packet);
                    {
                        DssRef.world.writeNet_SubTile(w, subtilePos);
                    }
                    packet.EndWrite_Asynch();
                }
                
            }

            bool findMissingTile(out IntVector2 tilePos, bool subTile)
            {
                ForXYLoop loop = new ForXYLoop(playerCulling.enterArea);
                while (loop.Next())
                {
                    if (!remoteTileGrid.Get(loop.Position).HasTile(subTile))
                    {
                        tilePos = loop.Position;
                        return true;
                    }
                }

                tilePos = IntVector2.NegativeOne;
                return false;
            }

        }
    }

    struct RemoteTile
    {
        public bool overview, detail;

        public bool HasTile(bool subTile)
        { 
            return subTile? detail : overview;
        }
    }
}
