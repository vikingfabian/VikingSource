using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.DataStream
{
    class UserRemoveFile : StorageTask
    {
       // DataStreamFile file;
        public UserRemoveFile(FilePath file)
            :base()
        {
            this.filePath = file;
            if (!file.Storage)
            {
                throw new Exception("Can't remove content");
            }
            beginStorageTask();//start();
        }

        public override void runQuedStorageTask()
        {
            List<string> files = FileToDiskManager.GetTimeMarkedStoragePaths(filePath);
            foreach (string s in files)
            {
                FileToDiskManager.RemoveFile(s);
            }
        }
    }

    /// <summary>
    /// Open a file and send the steam over net
    /// </summary>
    class OpenAndSendFile : StorageTask//QueAndSynch
    {
        //bool fromStorage;
        byte[] data;
        Network.PacketType packetType;
        WriteBinaryStream prefix;
        //Network.SendPacketTo to;
        ulong toSpecific;
        Network.PacketReliability rely;
        FilePath path;
        Action failureEvent = null;

        public OpenAndSendFile(FilePath path, Network.PacketType packetType, WriteBinaryStream prefix, 
            Network.SendPacketTo to, ulong toSpecific, Network.PacketReliability rely, Action failureEvent)
            : base()
        {
            //this.fromStorage = fromStorage;
            this.path = path;
            this.rely = rely;
            this.toSpecific = toSpecific;
            this.prefix = prefix;
            this.packetType = packetType;

            storagePriority = true;
            beginStorageTask();
            //start();
        }
        public override void runQuedStorageTask()
        {
            data = FileToDiskManager.Read(path);
         
        }
        public override void onStorageComplete()
        {
            if (data != null && data.Length > 0)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(packetType, rely, Network.SendPacketTo.All, toSpecific,
                     null);

                prefix?.Invoke(w);                
                w.Write(data);
            }
            else
            {
                failureEvent?.Invoke();
                Debug.LogError("OpenAndSendFile2, empty file");
            }
        }

    }
}
