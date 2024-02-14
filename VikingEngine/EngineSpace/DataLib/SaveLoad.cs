using System;
using System.Collections.Generic;
using System.IO;

namespace VikingEngine.DataLib
{
    interface ISaveByteArrayObj
    {
        byte[] ByteArraySaveData { get; set; }
    }

    class SaveLoad
    {
        public static bool CorruptFiles = false;
        static int usingStorage = 0;

        public static void Init()
        {

        }

        public static string CurrentUser;

        static string lastItem;
        public static void SetStreamIsOpen(bool open, string user)
        {
            //StreamIsOpen = open;
            if (open)
            {
                usingStorage++;

                //if (PublicConstants.DEBUGMODE && lastItem == user)
                //    throw new Exception();
                lastItem = user;
            }
            else
            {
                lastItem = null;
                usingStorage--;
                if (usingStorage < 0) usingStorage = 0;
            }
            //System.Diagnostics.Debug.WriteLine(user + ": " + (open ? "Open Stream" : "Close Stream"));
            CurrentUser = user;
        }
        public static void ResetStreamIsOpen() { usingStorage = 0; }

        public static bool GetUsingStorage
        {
            get { return usingStorage > 0; }
        }

        public static void CreateTextFile(string path, List<string> lines)
        {
            if (!GetUsingStorage)
            {
                string user = "CreateTextFile: " + path;
                SetStreamIsOpen(true, user);

                FileStream fs = new FileStream(path, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);

                for (int i = 0; i < lines.Count; i++)
                {
                    sw.WriteLine(lines[i]);
                }

                sw.Dispose();
                fs.Dispose();
                SetStreamIsOpen(false, user);
            }
        }

        public static void SaveByteArray(string filePath, byte[] data)
        {
            Stream stream;

                stream = File.Create(filePath);

            stream.Write(data, 0, data.Length);
            stream.Dispose();
        }
        public static byte[] LoadByteArray(string filePath, bool fromStorage)
        {
            //Stream s;
            byte[] data;
            SetStreamIsOpen(true, filePath);
            FileStream stream;

            stream = File.Open(filePath, FileMode.Open);


            if (stream.Length == 0)
            {
                Debug.LogError("LoadByteArray, " + filePath);
                data = new byte[0];
            }
            else
            {
                data = new byte[(int)stream.Length];
                stream.Read(data, 0, (int)stream.Length);
            }            
            stream.Dispose();
            SetStreamIsOpen(false, filePath);

            if (data.Length == 0 && fromStorage)
            {
                //remove corrupt file
                RemoveFile(filePath);
            }
            
            return data;
        }


        public static List<string> LoadTextFile(string filePath)
        {
            List<string> lines = new List<string>();
            try
            {
                StreamReader re = File.OpenText(filePath);
                string input = null;
                while ((input = re.ReadLine()) != null)
                {
                    lines.Add(input);
                }
                re.Dispose();
            }
            catch (Exception e)
            {
                Debug.LogError("LoadTextFile: " + e.Message);
            }
            return lines;
        }

        virtual public void InitLoad()
        { }

        public List<string> LoadTextFile(string folder, string name)
        {
            if (folder != TextLib.EmptyString) { folder += "/"; }
            return LoadTextFile("Content/Data/" + folder + name + ".txt");
        }

        
        public static string[] FilesInContentDir(string folder)
        {
            string dir = Engine.LoadContent.Content.RootDirectory + DataStream.FilePath.Dir + folder + DataStream.FilePath.Dir;
            // = new List<string>();
            ICollection<string> FileList = Directory.GetFiles(dir);
            string[] files = new string[FileList.Count];
            string name = TextLib.EmptyString;

            int ix = 0;
            foreach (string filename in FileList)
            {
                name = filename.Remove(0, dir.Length);
                files[ix++] = name.Remove(name.Length - 4, 4);
            }
            return files;
        }
        public static List<string> FilesInStorageDir(string folder)
        {
            return FilesInStorageDir(folder, TextLib.EmptyString);
        }

        public static List<string> FilesInStorageDir(string folder, string searchPattern)
        {
            const int FileEndLenght = 4;
            List<string> files = new List<string>();
            try
            {
                    string dir = DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir;// +folder + FilePath.Dir;
                    if (folder != TextLib.EmptyString)
                        dir += folder + DataStream.FilePath.Dir;

                    ICollection<string> FileList =
                        searchPattern == TextLib.EmptyString ?
                        Directory.GetFiles(dir) :
                        Directory.GetFiles(dir, searchPattern);
                    string name = null;
                    foreach (string filename in FileList)
                    {
                        //Console.WriteLine(filename);
                        name = filename.Remove(0, dir.Length);
                        files.Add(name.Remove(name.Length - FileEndLenght, FileEndLenght));
                    }


            }
            catch (Exception e)
            {
                Debug.LogError("FilesInStorageDir, " + e.Message);
                return new List<string>();
            }
            return files;
        }
        public static List<string> FoldersInStorageDir(string folder, string searchPattern)
        {
            List<string> files = new List<string>();
            if (searchPattern == TextLib.EmptyString)
                searchPattern = "*";
            int pathLength = 0;

                folder = DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir + folder;
                files.AddRange(Directory.GetDirectories(folder, searchPattern));
                pathLength = 1;

           pathLength += folder.Length;
           for (int i = 0; i < files.Count; i++)
           {
               files[i] = files[i].Remove(0, pathLength);
           }
            return files;

        }

        /// <param name="fileName"></param>
        public static bool FileExistInStorageDir(string folder, string fileName)
        {
            return FilesInStorageDir(folder).Contains(fileName);
        }
        public static bool FolderExist(string folder)
        {

                return Directory.Exists(DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir + folder);

        }
        public static bool FolderExistAndHaveFilesInit(string folder)
        {
            return FolderExist(folder) && FilesInStorageDir(folder).Count > 0;
        }

        public static void RemoveFile(string path)
        {

                File.Delete(DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir + path);

        }

        public static void RemoveFile2(string completePath)
        {

                File.Delete(completePath);

        }

        public static void RemoveFolder(string path, bool containsFolders)
        {

                path = DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir + path;
                if (Directory.Exists(path))
                    Directory.Delete(path, true);

        }

        public static bool StoragePathExist(string path)
        {

                return File.Exists(path);

        }

        public static List<string> ListStorageFolders(string lookPattern, bool removeCurrentDir)
        {
            
            List<string> result = new List<string>();
            try
            {

                    string dir = DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir;
                    result.AddRange(Directory.GetDirectories(dir, lookPattern));
                    if (removeCurrentDir)
                    {
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i] = result[i].Remove(0, dir.Length);
                        }
                    }

            }

            catch (Exception e)
            {
                Debug.LogError("ListStorageFolders," + e.Message);
            }
            return result;
        }

        public static DateTime DateOnStorageFile(string path)
        {
            if (PlatformSettings.RunningXbox)
            {
                //does not work on xbox
                throw new NotImplementedException();
            }
            else
            {

                return File.GetLastWriteTime(DataStream.FilePath.StorageDirectory() + DataStream.FilePath.Dir + path);
            }
        }

        public static string BytesToString(double size)
        {
            const string Format = "#.#";
            const double KB = 1024;
            const double MB = KB * KB;

            //double size = DirectorySize(folder);
            if (size < (KB * 0.1))
            {
                return ((int)size).ToString() + "B";
            }
            else if (size < (MB * 0.1))
            {
                return (size / KB).ToString(Format) + "kB";
            }
            else
            {
                return (size / MB).ToString(Format) + "MB";
            }
        }

        public static string DirectorySizeLabel(string folder)
        {
            return BytesToString(DirectorySize(folder));
        }

        static double DirectorySize(string folder)
        {
            if (folder == null)
                return 0;

                // 1
                // Get array of all file names.
                string[] a = Directory.GetFiles(folder);

                // 2
                // Calculate total bytes of all files in a loop.
                double b = 0;
                foreach (string name in a)
                {
                    // 3
                    // Use FileInfo to get length of each file.
                    FileInfo info = new FileInfo(name);

                    b += info.Length;
                }
                // 4
                // Return total size
                return b;

        }

        static readonly long MinSaveTime = TimeSpan.FromMinutes(2).Ticks;
        public static string FileTimeMark()
        {
            long ticks = DateTime.Now.Ticks;
            ticks /= MinSaveTime;
            return "_" + ticks.ToString();    
        }

    }

   
}
