using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.DataStream
{
    abstract class AbsPlatformStorage
    {
        abstract public void write(FilePath file, System.IO.MemoryStream data, Action onComplete);

        abstract public void read(FilePath file, System.IO.MemoryStream data, Action onComplete);
    }

    class PCStorage : AbsPlatformStorage
    {
        public override void write(FilePath file, MemoryStream data, Action onComplete)
        {
            string path = file.CompletePath(true);
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
            {
                Directory.CreateDirectory(dir);
            }

            using (FileStream fileStream = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
            {
                data.CopyTo(fileStream);
            }
            onComplete();
        }

        public override void read(FilePath file, MemoryStream data, Action onComplete)
        {
            string path = file.CompletePath(false);

            if ((OperatingSystem.IsAndroid() || OperatingSystem.IsIOS()) && file.Content)
            {
                try
                {
                    using Stream assetStream = TitleContainer.OpenStream(FileToDiskManager.NormalizeAndroidAssetPath(path));
                    assetStream.CopyTo(data);
                }
                catch (Exception e)
                {
                    Debug.LogError("PCStorage.read Android asset: " + path + ", " + e.Message);
                }
            }
            else if (file.Content || File.Exists(path))
            {
                using FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                fileStream.CopyTo(data);
            }

            onComplete();
        }
    }
}
