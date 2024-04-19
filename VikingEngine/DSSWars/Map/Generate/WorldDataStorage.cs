using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Valve.Steamworks;

namespace VikingEngine.DSSWars
{
    class WorldDataStorage: DataStream.IStreamIOCallback
    {
        public WorldData worldData;
        public bool loadComplete = false;
        public bool started = false;
        DataStream.ReadBinaryIO readTask;
        public void saveMap(WorldData data)
        {
            this.worldData = data;
            var filePath = mapfilePath(data.metaData.mapSize, data.metaData.saveIndex, true);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            new DataStream.WriteBinaryIO(filePath, data.writeMapFile, null);
        }

        public void loadMap(MapSize size, int mapIndex)
        {
            this.worldData = new WorldData();
            this.worldData.metaData = new Data.WorldMetaData(0, size, mapIndex);
            //this.worldData.mapSize = size;
            //this.worldData.saveIndex = mapIndex;
            DssRef.world = this.worldData;
            var filePath = mapfilePath(worldData.metaData.mapSize, worldData.metaData.saveIndex, false);
            readTask = new DataStream.ReadBinaryIO(filePath, worldData.readMapFile, this);
        }

        public DataStream.FilePath mapfilePath(MapSize size, int mapIndex, bool save)
        {
            string folder;
            if (save)
            {
                folder = "DssMapSave";
            }
            else
            {
                folder = DssLib.ContentDir + "Map";
            }

            return new DataStream.FilePath(folder,
                "map_sz" + ((int)size).ToString() + "_i" + mapIndex.ToString(), ".map", save, false);
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        {
            if (completed && !save)
            {
                loadComplete = true;
            }
        }

        public bool LoadingStarted => readTask != null && readTask.hasStartedTask;
    }
}
