using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataLib;

namespace VikingEngine.DSSWars.UserGeneratedContent
{
    static class UGClib
    {
        public const string UGCFolder = "UserGeneratedContent";
        public static readonly string ModelReplaceFolder = Path.Combine(UGCFolder, "vox_replace");

        public static void GameContentInit()
        {
            DataStream.FilePath.CreateStorageFolder(UGCFolder);
            DataStream.FilePath.CreateStorageFolder(ModelReplaceFolder);
        }
    }
}
