using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Network;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.SteamWrapping
{
    class SteamWriter : DataStream.MemoryStreamHandler/*, IUpdateable*/
    {
        PacketReliability relyability;

        public SendPacketTo To;
        public ulong SpecificGamerID;
        PacketType storedtype;
        public SteamWriter()
        { }

        public void init(PacketReliability relyability, bool addToTrigger,
            SendPacketTo To, ulong SpecificGamerID)
        {
            this.To = To;
            this.SpecificGamerID = SpecificGamerID;
            this.relyability = relyability;
            if (addToTrigger)
            {
                //Ref.update.AddToOrRemoveFromUpdate(this, true); 
                Ref.netSession.packetsQueue.Enqueue(this);
            }
        }

        public void CheckPacketLength()
        {
            if (memoryLength > SteamWrapping.SteamP2PManager.SteamPackageByteLimit)
            {
                throw new Exception("Passed steam package limit");
            }
        }

        public void EndWrite_Asynch()
        {
            Ref.netSession.packetsQueue.Enqueue(this);
            //Ref.update.AddSyncAction(new SyncAction1Arg<float>(Time_Update, 0));
        }

        public System.IO.BinaryWriter writeHead(PacketType type, int? sender)
        {
            storedtype = type;
            byte senderout = sender == null ? byte.MinValue : (byte)sender.Value;

            System.IO.BinaryWriter w = this.GetWriter(SteamP2PManager.SteamPackageByteLimit);
            w.Write(senderout);
            w.Write((byte)type);

            return w;
        }

//        virtual public void Time_Update(float time)
//        {
//#if PCGAME
//            if (Ref.steam.isNetworkInitialized)
//            {
//                Ref.steam.P2PManager.Send(this.ByteArray(out long length), (uint)length, relyability, To, new Steamworks.CSteamID(SpecificGamerID));
//            }
//#endif
//        }

        public void send()
        {
            if (Ref.steam.isNetworkInitialized)
            {  
                Ref.steam.P2PManager.Send(this.ByteArray(out long length), (uint)length, relyability, To, new Steamworks.CSteamID(SpecificGamerID));
              
            }
#if DEBUG
            else if (memoryLength > SteamP2PManager.SteamPackageByteLimit)
            {
                throw new Exception("Passed steam package limit: " + storedtype.ToString());
            }
#endif
        }

        //public UpdateType UpdateType { get { return VikingEngine.UpdateType.OneTimeTrigger; } }

        public int SpottedArrayMemberIndex { get { return -1; } set { } }
        public bool SpottedArrayUseIndex { get { return false; } }
        public bool RunDuringPause { get { return true; } }
    }
}