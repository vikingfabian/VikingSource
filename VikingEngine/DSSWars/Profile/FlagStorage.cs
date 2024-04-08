using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.PJ.SpaceWar.SpaceShip;

namespace VikingEngine.DSSWars.Profile
{
    class FlagStorage
    {
        const int ProfilesCount = 16;
        public List<FlagAndColor> flagDesigns;
       

        public FlagStorage()
        {
            flagDesigns = new List2<FlagAndColor>(ProfilesCount);

            for (int i = 0; i < ProfilesCount; ++i)
            {
                flagDesigns.Add(new FlagAndColor(FactionType.Player, i));
            }
        }

        public void Load()
        {
            var filePath = path(-1);

            string[] files = DataStream.DataStreamHandler.SearchFilesInStorageDir(filePath, true);
            
            foreach (string file in files)
            {
                var num = file.Split('_')[1];
                int index = Convert.ToInt32(num) -1;

                filePath.FileName = file;
                DataStream.DataStreamHandler.TryReadBinaryIO(filePath, flagDesigns[index].read);
            }
        }

        public void Save(int index)
        {
            var filePath = path(index);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            DataStream.BeginReadWrite.BinaryIO(true, filePath, flagDesigns[index].write, null, null, true);
        }

        public void old_read(System.IO.BinaryReader r)
        {
            for (int i = 0; i < ProfilesCount; ++i)
            {
                flagDesigns[i].read_old(r);
            }
        }

        DataStream.FilePath path(int index)
        {
            string num = index >= 0? (index + 1).ToString() : string.Empty;
           return new DataStream.FilePath("FlagSave", "flag_" + num, ".set");
        }

    }
}
