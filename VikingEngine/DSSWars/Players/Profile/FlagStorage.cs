using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.PJ.SpaceWar.SpaceShip;

namespace VikingEngine.DSSWars.Players.Profile
{
    class FlagStorage
    {
        public List<FlagAndColor> flagDesigns;

        public int selected;

        public FlagStorage()
        {
            const int StartCount = 8;
            flagDesigns = new List2<FlagAndColor>(StartCount);

            for (int i = 0; i < StartCount; ++i)
            {
                flagDesigns.Add(new FlagAndColor(FactionType.Player, i, null));
            }
        }

        public FlagAndColor Selected()
        {
            return flagDesigns[selected];
        }

        public void Load()
        {
            var filePath = path(-1);

            string[] files = FileToDiskManager.SearchFilesInStorageDir(filePath, true);
            
            foreach (string file in files)
            {
                var num = file.Split('_')[1];
                int index = Convert.ToInt32(num) -1;

                filePath.FileName = file;

                while (index >= flagDesigns.Count)
                {
                    flagDesigns.Add(new FlagAndColor(FactionType.Player, flagDesigns.Count, null));
                }

                FileToDiskManager.TryReadBinaryIO(filePath, flagDesigns[index].read);
            }
        }

        public void Save(int index)
        {
            var filePath = path(index);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            BeginReadWrite.BinaryIO(true, filePath, flagDesigns[index].write, null, null, true);
        }

        //public void old_read(System.IO.BinaryReader r)
        //{
        //    for (int i = 0; i < 16; ++i)
        //    {
        //        flagDesigns[i].read_old(r);
        //    }
        //}

        FilePath path(int index)
        {
            string num = index >= 0? (index + 1).ToString() : string.Empty;
            return new FilePath(Ref.steam.UserCloudPath + System.IO.Path.AltDirectorySeparatorChar + 
               "FlagSave", "flag_" + num, ".sav");
        }

    }
}
