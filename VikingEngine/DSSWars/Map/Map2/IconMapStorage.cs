using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DebugExtensions;
using VikingEngine.Graphics;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars.Map.Map2
{
    class IconMapStorage : IStreamIOCallback
    {        

        public const string MapSavesRoot = "Icon maps";
        public const string MapSavesLocal = "User";
        public const string MapSavesWorkshop = "Workshop";
        const string FileEnd = ".icm";

        public FilePath SavePath(string saveFileName, out FilePath path, out FilePath iconPath)
        {
            saveFileName = FilePath.SanitizeFileName(saveFileName);
            path = new FilePath(MapSavesRoot + FilePath.Dir + MapSavesLocal + FilePath.Dir + saveFileName, saveFileName, FileEnd, true, false);
            iconPath = new FilePath(MapSavesRoot + FilePath.Dir + MapSavesLocal + FilePath.Dir + saveFileName, "icon", ".png", true, false);
            return path;
        }

        public void save(IconWorldDataMeta meta, IconWorldData data, Texture2D texture, IStreamIOCallback callbackObj)
        {
            FilePath path, iconPath;
            SavePath(meta.name, out path, out iconPath);

            var iconTexture = Ref.draw.ShrinkTexture(texture, 256, 256);
            new Timer.AsynchActionTrigger(() =>
            {
                try
                {
                    System.IO.Directory.CreateDirectory(path.CompleteDirectory);

                    StreamLib.saveTextureAsPNG(iconTexture, iconPath);

                    DataStream.BeginReadWrite.BinaryIO(true, path, data.writeIcon, null, callbackObj, false);

                    Ref.update.AddSyncAction(() => { callbackObj.SaveComplete(true, 0, true, null); });
                }
                catch (Exception ex)
                {
                    BlueScreen.ThreadException = ex;
                    callbackObj.SaveComplete(true, 0, false, null);
                }
            }, true);           
        }

        public void SaveComplete(bool save, int player, bool completed, byte[] value)
        { 
            
        }
    }
}
