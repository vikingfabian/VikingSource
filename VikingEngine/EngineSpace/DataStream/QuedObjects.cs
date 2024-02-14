using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//xna

namespace VikingEngine.DataStream
{
    //abstract class DataStreamQuedObj : IQuedObject
    //{
    //    protected FilePath file;
    //    public DataStreamQuedObj(FilePath file)
    //    {
    //        this.file = file;
    //    }
    //    protected void start()
    //    { Engine.Storage.AddToSaveQue(StartQuedProcess, true); }
    //    abstract public void runQuedTask(MultiThreadType threadType);
    //}

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

        protected override void runQuedStorageTask()
        {
            List<string> files = DataStreamHandler.GetTimeMarkedStoragePaths(filePath);
            foreach (string s in files)
            {
                DataStreamHandler.RemoveFile(s);
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

        //public OpenAndSendFile(FilePath path, Network.PacketType packetType, WriteBinaryStream prefix, 
        //    Network.SendPacketTo to, ulong toSpecific, Network.PacketReliability rely)
        //    : this(path, packetType, prefix, to, toSpecific, rely, null)
        //{ }
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
        protected override void runQuedStorageTask()
        {
            data = DataStreamHandler.Read(path);
            //if (data != null)
            //{
            //    bMainThreadTask;
            //}
            //else
            //{
            //    Debug.LogError("OpenAndSendFile2, empty file");
            //    if (failureEvent != null)
            //    {
            //        new Timer.Action0ArgTrigger(failureEvent);
            //    }
            //    return false;
            //}

        }
        public override void onStorageComplete()
        {
            if (data != null && data.Length > 0)
            {
                System.IO.BinaryWriter w = Ref.netSession.BeginWritingPacket(packetType, Network.SendPacketTo.All/*to*/, toSpecific, rely,
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

    //class CorruptFile : DataStreamQuedObj
    //{
    //    bool leaveBackup;
    //    public CorruptFile(FilePath file, bool leaveBackup)
    //        : base(file)
    //    {
    //        this.leaveBackup = leaveBackup;
    //        start();
    //    }
    //    public override void runQuedTask(MultiThreadType threadType)
    //    {
    //        if (leaveBackup)
    //            file.NumVersionsStacking = Bound.SetMinVal(file.NumVersionsStacking, 2);
    //        else
    //            file.NumVersionsStacking = 1;

    //        DataStreamHandler.Write(file, new byte[] { });
    //    }
    //}
}
