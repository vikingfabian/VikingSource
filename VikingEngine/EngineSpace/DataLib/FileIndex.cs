using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DataLib
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public struct FileSortSettings
    {
        public bool sortByDate = true;
        public bool ascending = false;
        
        public FileSortSettings() 
        {}
    }

    public class FileIndex
    {
        public bool projectType = false;

        public class FileEntry
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }

            public object Tag = null;

            public FileEntry(string name, DateTime date)
            {
                Name = name;
                Date = date;
            }
        }

        public List<FileEntry> Files { get; private set; } = new List<FileEntry>();

        public FileIndex(string folder, bool fullPath, string searchPattern, bool storage, FileSortSettings sort)
        {
            LoadFromStorage(folder, fullPath, searchPattern, storage);
            Sort(sort);
        }

        public void LoadFromStorage(string folder, bool fullPath, string searchPattern, bool storage)
        {
            Files.Clear();
            try
            {
                string baseDir;
                if (fullPath)
                {
                    baseDir = folder + DataStream.FilePath.Dir;
                }
                else
                {
                    baseDir = (storage ? DataStream.FilePath.StorageDirectory() : Engine.LoadContent.Content.RootDirectory) + DataStream.FilePath.Dir;

                    if (!string.IsNullOrEmpty(folder))
                        baseDir += folder + DataStream.FilePath.Dir;
                }

                string[] filePaths = string.IsNullOrEmpty(searchPattern)
                    ? Directory.GetFiles(baseDir)
                    : Directory.GetFiles(baseDir, searchPattern);

                foreach (string filepath in filePaths)
                {
                    string name = Path.GetFileNameWithoutExtension(filepath);
                    DateTime date = File.GetLastWriteTime(filepath);
                    Files.Add(new FileEntry(name, date));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("FileIndex.LoadFromStorage: " + e.Message);
            }
        }

        public void Sort(FileSortSettings settings)
        {
            if (settings.sortByDate)
            {
                Files = settings.ascending
                    ? Files.OrderBy(f => f.Date).ToList()
                    : Files.OrderByDescending(f => f.Date).ToList();
            }
            else
            {
                Files = settings.ascending
                    ? Files.OrderBy(f => f.Name).ToList()
                    : Files.OrderByDescending(f => f.Name).ToList();
            }
        }
    }

}
