using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;

namespace VikingEngine.SteamWrapping
{
    class SteamWriter : DataStream.MemoryStreamHandler, IUpdateable
    {
        PacketReliability relyability;

        public SendPacketTo To;
        public ulong SpecificGamerID;

        public SteamWriter(PacketReliability relyability, bool addToTrigger, 
            SendPacketTo To, ulong SpecificGamerID)
        {
            this.To = To;
            this.SpecificGamerID = SpecificGamerID;
            this.relyability = relyability;
            if (addToTrigger)
            { Ref.update.AddToOrRemoveFromUpdate(this, true); }
        }

        public SteamWriter()
        { }

        public void EndWrite_Asynch()
        {
            Ref.update.AddSyncAction(new SyncAction1Arg<float>(Time_Update, 0));
        }

        public System.IO.BinaryWriter writeHead(PacketType type, int? sender)
        {
            byte senderout = sender == null ? byte.MinValue : (byte)sender.Value;

            System.IO.BinaryWriter w = this.GetWriter();
            w.Write(senderout);
            w.Write((byte)type);

            return w;
        }

        public void Time_Update(float time)
        {
#if PCGAME
            if (Ref.steam.isNetworkInitialized)
            {
                Ref.steam.P2PManager.Send(this.ByteArray(), relyability, To, SpecificGamerID);
            }
#endif
        }
        public UpdateType UpdateType { get { return VikingEngine.UpdateType.OneTimeTrigger; } }

        public int SpottedArrayMemberIndex { get { return -1; } set { } }
        public bool SpottedArrayUseIndex { get { return false; } }
        public bool RunDuringPause { get { return true; } }
    }
}