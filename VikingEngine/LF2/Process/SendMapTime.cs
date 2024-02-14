using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Process
{
    class SendMapTime : OneTimeTrigger, IQuedObject
    {
        Players.Player p;
        float expectedTime;

        public SendMapTime(Players.Player p)
            : base(false)
        { 
            this.p = p;
            Engine.Storage.AddToSaveQue(StartQuedProcess, false);
        }

        public void StartQuedProcess(bool saveThread)
        {
            expectedTime = DataLib.SaveLoad.FilesInStorageDir(Data.WorldsSummaryColl.CurrentWorld.FolderPath).Count * MapSender.SendRate;
            AddToUpdateList(true);
        }

        public override void Time_Update(float time)
        {
            System.IO.BinaryWriter sendMapStart = Ref.netSession.BeginWritingPacket(Network.PacketType.LF2_SendMapStart,
                        Network.PacketReliability.ReliableLasy);
            sendMapStart.Write(expectedTime);

            p.Print("Sending map, estimated time: " + TextLib.TimeToText(expectedTime, false));
        }
    }
}
