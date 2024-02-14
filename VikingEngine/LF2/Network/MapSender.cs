using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.DataLib;

namespace VikingEngine.LF2
{
    class MapSender : OneTimeQueTrigger2
    {
        public const int SendRate = 1000;
        
        public MapSender()
            :base(true)
        {
            //expectedTime = SaveLoad.FilesInStorageDir(Map.WorldOverview.WorldDirName).Count * SendRate;
        }
        public override void StartQuedProcess(bool saveThread)
        {
            IntVector2 pos = IntVector2.Zero;

            for (pos.Y = 0; pos.Y < Map.WorldPosition.WorldChunksY; pos.Y++)
            {
                for (pos.X = 0; pos.X < Map.WorldPosition.WorldChunksX; pos.X++)
                {
                    if (LfRef.worldOverView.GetChunkData(pos).changed)
                    {
                        LfRef.gamestate.NetAddChunkRequest(pos);
                        System.Threading.Thread.Sleep(SendRate);
                    }
                }
            }
            LfRef.gamestate.LocalHostingPlayer.Print("Map sending complete");
            Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_SendMapComplete, Network.PacketReliability.ReliableLasy, LootfestLib.LocalHostIx);
                
            
        }
    }
}
