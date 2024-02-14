using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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

            using (FileStream fileStream = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
            {
                data.CopyTo(fileStream);
                fileStream.Dispose();
            }
            onComplete();
        }

        public override void read(FilePath file, MemoryStream data, Action onComplete)
        {
            string path = file.CompletePath(false);

            if (file.Content || File.Exists(path))
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    fileStream.CopyTo(data);
                    fileStream.Dispose();
                }
            }

            onComplete();
        }
    }
}
