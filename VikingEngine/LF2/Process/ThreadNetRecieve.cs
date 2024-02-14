using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//

namespace VikingEngine.LF2.Process
{
    class ThreadNetRecieve : IQuedObject
    {
        System.IO.BinaryReader r;  
        Network.PacketType type;
        Network.AbsNetworkPeer sender;

        public ThreadNetRecieve(Network.PacketType type, System.IO.BinaryReader r, Network.AbsNetworkPeer sender)
        {
            this.r = new  System.IO.BinaryReader(new System.IO.MemoryStream(r.ReadBytes(r.Length - r.Position)));
            this.type = type;
            this.sender = sender;
            Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public void StartQuedProcess(bool saveThread)
        {
            switch (type)
            {
                case Network.PacketType.LF2_AcceptVoxelObjPacket:
                    int fileIndex = r.ReadUInt16();
                    string name = Editor.VoxelDesigner.CustomObjectFromIndex(fileIndex);
                    if (name == null)
                        return;
                    new DataStream.OpenAndSendFile(Editor.VoxelDesigner.CustomVoxelObjPath(name),
                    //new DataLib.OpenAndSendFile2(Editor.VoxelDesigner.CustomVoxelObjPath(name),
                        Network.PacketType.LF2_SendAnimDesignCreation, new DataLib.NamePrefix(name).WriteStream, Network.SendPacketTo.OneSpecific, sender.Id,
                        Network.PacketReliability.Reliable);
                    new Process.PrintMessageTrigger(sender.Gamertag + " recieved \"" + name + "\"");
                    break;
                case Network.PacketType.LF2_SendAnimDesignCreation:
                    //recieve a present containing an animated voxelobj
                    throw new Exception("Ska inte vara trådad");

                    // Uncomment when exception is removed.

                    //string voxName = SaveLib.ReadString(r);
                    //Editor.VoxelDesigner.CreateVoxelObjFolder();
                    //if (DataLib.SaveLoad.FileExistInStorageDir(Editor.VoxelDesigner.UserVoxelObjFolder, voxName))
                    //{
                    //    int version = 2;
                    //    while (DataLib.SaveLoad.FileExistInStorageDir(Editor.VoxelDesigner.UserVoxelObjFolder, voxName + version.ToString()))
                    //    {
                    //        version++;
                    //    }
                    //    voxName += version.ToString();
                    //}

                    //new DataStream.WriteByteArray(Editor.VoxelDesigner.CustomVoxelObjPath(voxName),
                    //    r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position)), null);
                    ////DataLib.SaveLoad.SaveByteArray(Editor.VoxelDesigner.CustomVoxelObjPath(voxName), 
                    ////    r.ReadBytes((int)(r.BaseStream.Length - r.BaseStream.Position)));
                    //LfRef.gamestate.LocalHostingPlayerPrint("Recieved AnimDesign Object: " + voxName);
                    //break;
            }
        }
    }
}
