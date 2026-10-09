using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.Core.BlackBolts.Data
{
    class MapStorage: IStreamIOCallback
    {

        public List<PlaceObjectData> restorePoint = new List<PlaceObjectData>(1024);

        public MapStorage()
        {
            BlackRef.storage = this;
        }

        public DataStream.FilePath Path(bool playerStorage)
        {
            string folder = "BlackBolt";
            if (playerStorage)
            {
                folder = Ref.steam.UserCloudPath + DataStream.FilePath.Dir + folder;
            }
            return new DataStream.FilePath(folder,
                BlackRef.missionSetup.missionName, ".bbs", playerStorage);
        }

        public void Save()
        {
            RichBoxContent content = new RichBoxContent();
            content.h2("Saving...", HudLib.TitleColor_Head);
            BlackRef.playScene.messages.Add(content);

            var path = Path(true);

            try
            {
                System.IO.Directory.CreateDirectory(path.CompleteDirectory);
            }
            catch (Exception ex)
            {
                IOLib.fileCheck_gamestorage.createFolderFail = false;
                IOLib.fileCheck_gamestorage.exception = ex;
                return;
            }

            BlackRef.mapData.CreateStorePoint();
            DataStream.BeginReadWrite.BinaryIO(true, path, write, null, this, true);
        }
        public void Load(bool playerStorage)
        {
            var path = Path(playerStorage);
            DataStream.BeginReadWrite.BinaryIO(false, path, null, read, playerStorage? this : null, playerStorage);       
        }

        void write(System.IO.BinaryWriter w)
        {
            const int Version = 1;
            w.Write(Version);

            w.Write(restorePoint.Count);
            foreach (var item in restorePoint)
            {
                item.write(w);
            }

            Debug.WriteCheck(w);
        }
        void read(BinaryReader r)
        {            
            int version = r.ReadInt32();

            int count = r.ReadInt32();
            List<PlaceObjectData> items = new List<PlaceObjectData>(count);
            for (int i = 0; i < count; ++i)
            {
                PlaceObjectData objectData = new PlaceObjectData();
                objectData.read(r, version);
                items.Add(objectData);
            }

            Debug.ReadCheck(r);


            restorePoint = items;
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {

            if (save)
            {
                RichBoxContent content = new RichBoxContent();
                content.h2("Save complete", HudLib.TitleColor_Head);
                BlackRef.playScene.messages.Add(content);
            }
            else
            {
                BlackRef.mapData.ClearMap();
                BlackRef.mapData.RestoreMap();
            }
        }
    }
}
