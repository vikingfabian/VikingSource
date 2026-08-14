using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class CustomMapStorage : WorldDataStorage
    {

        public string customName = null;
        public string autoName = "None";
        public override void saveMap(WorldData data)
        {
            this.worldData = data;
            var filePath = customMapfilePath(true);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            new DataStream.WriteBinaryIO(filePath, data.writeMapFile, null);
        }

        public DataStream.FilePath customMapfilePath(bool save)
        {
            string folder;
            //if (save)
            //{
            folder = "World Map";
            //}
            //else
            //{
            //    folder = DssLib.ContentDir + "Map";
            //}

            return new DataStream.FilePath(folder,
                Name, ".map", save, false);
        }

        public string Name { get { return customName != null ? customName : autoName; } }
    }
}
