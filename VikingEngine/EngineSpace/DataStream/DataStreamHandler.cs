using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Threading.Tasks;


namespace VikingEngine.DataStream
{
    static class DataStreamHandler
    {
        public static void Write(FilePath file, IBinaryIOobj obj)
        {
            Write(file, obj.write);
        }

        public static void Write(FilePath file, WriteBinaryStream write)
        {
            System.IO.MemoryStream s = new System.IO.MemoryStream();
            System.IO.BinaryWriter w = new System.IO.BinaryWriter(s);
            write(w);
            Write(file, s.ToArray());
        }

        public static void Write(FilePath file, byte[] data)
        {

            /* 1.Find old files and list them
             * 2.Create new file
             * 3.Remove old files 
             ------------------------*/

            //1.Find old files and list them
            List<string> oldFiles = null;
            if (file.UseTimeMark)
                oldFiles = GetTimeMarkedStoragePaths(file);

            file.CheckFileLength();
            //2.Create new file
            string path = file.CompletePath(true);
            Stream stream = null;
#if PCGAME
            stream = File.Create(path);
#else
           
#endif
            stream.Write(data, 0, data.Length);
            stream.Dispose();
            simulateXboxLoadingTime();

            if (file.UseTimeMark)
            {
                //3.Remove old files
                for (int i = file.NumVersionsStacking - 1; i < oldFiles.Count; i++)
                {
                    if (oldFiles[i] != path)
                        RemoveFile(oldFiles[i]);
                }
            }
        }


        public static bool TryReadBinaryIO(FilePath file, ReadBinaryStream read)
        {
            if (FileExists(file))
            {
                byte[] data = Read(file);
                if (data != null)
                {
                    System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                    read(new System.IO.BinaryReader(s));
                }
                return true;
            }
            return false;
        }

        public static void ReadBinaryIO(FilePath file, ReadBinaryStream read)
        {
            byte[] data = Read(file);
            if (data != null)
            {
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                read(new System.IO.BinaryReader(s));
            }
        }

        public static System.IO.BinaryReader ReadBinaryIO(FilePath file)
        {
            byte[] data = Read(file);
            if (data != null)
            {
                System.IO.MemoryStream s = new System.IO.MemoryStream(data);
                return new System.IO.BinaryReader(s);
            }
            return null;
        }

        public static byte[] Read(FilePath file)
        {
            //Seek out the versions of the file, load the latest version
            //Automatically find empty/corrupt files, remove and find the next one
            string path;

            if (file.Storage && file.UseTimeMark)
            {
                List<string> files = GetTimeMarkedStoragePaths(file);
                if (files.Count == 0)
                {
                    Debug.LogWarning("Can't find time marked files: " + file.ToString());
                    return null;
                }

                path = files[0];//fel, 0 är inte automatiskt äldst
            }
            else
            {
                path = file.CompletePath(false);
            }

            //            byte[] data = null;
            //            System.IO.Stream stream = null;

            //#if PCGAME
            //            if (File.Exists(path))
            //            {
            //                stream = File.Open(path, FileMode.Open);
            //            }
            //#else
            //            Windows.Storage.StorageFile storageFile;

            //            var task = System.Threading.Tasks.Task.Factory.StartNew(async () =>
            //            {
            //                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //                storageFile = await storageFolder.GetFileAsync(path);
            //                //var ra = await storageFile.OpenAsync(Windows.Storage.FileAccessMode.Read);
            //                stream = await storageFile.OpenStreamForReadAsync();
            //                //stream = randomAccessStream.AsStreamForRead();
            //                //complete = true;
            //            });

            //            try
            //            {
            //                task.Wait();
            //            }
            //            catch (Exception e)
            //            {
            //                Debug.LogError(e.Message);
            //            }
            //#endif


            //            if (stream == null || stream.Length == 0)
            //            {
            //                Debug.LogError("file is corrupt/empty, " + path);
            //            }
            //            else
            //            {
            //                data = new byte[(int)stream.Length];
            //                stream.Read(data, 0, (int)stream.Length);
            //            }

            //            stream?.Dispose();

            byte[] data = null;

            if (File.Exists(path))
            {
                // The using statement ensures the stream is disposed of, even if an exception occurs
                using (var stream = File.Open(path, FileMode.Open))
                {
                    if (stream.Length == 0)
                    {
                        Debug.LogError("file is corrupt/empty, " + path);
                    }
                    else
                    {
                        data = new byte[stream.Length];
                        stream.Read(data, 0, (int)stream.Length);
                    }
                } // The stream is automatically disposed of here, even if an exception is thrown above
            }
            else
            {
                Debug.LogError("File does not exist: " + path);
            }

            return data;
        }


        public static void RemoveFile(string path)
        {
            File.Delete(path);
            //simulateXboxLoadingTime();
        }
        

        public static void BeginUserRemoveFile(FilePath file)
        {
            new UserRemoveFile(file);
        }

        public static bool StorageFolderIsEmpty(string directoryPath)
        {
            return SearchFilesInStorageDir(new FilePath(directoryPath, "", ".*"), false).Length == 0; 
        }
        public static bool StorageFolderExists(string directoryPath)
        {

            return Directory.Exists(DataStream.FilePath.StorageDirectory() + FilePath.Dir + directoryPath);//System.IO.Directory.GetCurrentDirectory() + FilePath.Dir + directoryPath);

        }
        /// <summary>
        /// Must be empty or it crashes!
        /// </summary>
        public static void RemoveEmptyFolder(string directoryPath)
        {

            Directory.Delete(DataStream.FilePath.StorageDirectory() + FilePath.Dir + directoryPath);
            simulateXboxLoadingTime();

        }

        public static bool RemoveFolder(string directoryPath, bool containsFolders)
        {
            return RemoveFolder(directoryPath, containsFolders, int.MaxValue);
        }

        /// <summary>
        /// Safely removes a folder by first removing the files in it 
        /// </summary>
        /// <param name="containsFolders">Expected tp contain other folder</param>
        /// <param name="maxFilesRemoval">Method will return after removing this number of files</param>
        /// <returns>Completed the removal</returns>
        public static bool RemoveFolder(string directoryPath, bool containsFolders, int maxFilesRemoval)
        {

                directoryPath = DataStream.FilePath.StorageDirectory() + FilePath.Dir + directoryPath;
                if (Directory.Exists(directoryPath))
                    Directory.Delete(directoryPath, true);
                return true;
        }
        public static List<string> ListStorageFolders(string lookPattern, bool removeCurrentDir)
        {

            List<string> result = new List<string>();
            try
            {
                    //make sure the project dir isnt included
                    AddProjectDirIfMissing(ref lookPattern);
                    result.AddRange(Directory.GetDirectories(lookPattern));
                    if (removeCurrentDir)
                    {
                        int dirLenght = DataStream.FilePath.StorageDirectory().Length + 1;
                        for (int i = 0; i < result.Count; i++)
                        {
                            result[i] = result[i].Remove(0, dirLenght);
                        }
                    }

            }

            catch (Exception e)
            {
                Debug.LogError("ListStorageFolders," + e.Message);
            }
            return result;
        }

        public static void CopyFiles(string fromDir, string toDir, bool removeTimeMark)
        {
            fromDir += FilePath.Dir;
            toDir += FilePath.Dir;
            string[] files = SearchFilesInStorageDir(fromDir);
            foreach (string f in files)
            {
                string name = f.Remove(0, fromDir.Length);
                if (removeTimeMark)
                {
                    var noTimeMark = FileNameWithOutTimeMark(name);
                    name = noTimeMark.String1 + noTimeMark.String2;
                }
                System.IO.File.Copy(f, toDir + name, true);
            }
        }

        static void AddProjectDirIfMissing(ref string dir)
        {

            if (!(dir.Length >= DataStream.FilePath.StorageDirectory().Length &&
                    dir.Remove(DataStream.FilePath.StorageDirectory().Length) == DataStream.FilePath.StorageDirectory()))
            {
                dir = DataStream.FilePath.StorageDirectory() + FilePath.Dir + dir;
            }
        }

        public static string[] SearchFilesInStorageDir(FilePath file, bool removeFileEnd)
        {

            if (Directory.Exists(file.CompleteDirectory))
            {
                var result = Directory.GetFiles(file.CompleteDirectory, file.FileName + "*" + file.FileEnd);
                if (removeFileEnd)
                {
                    for (int i = 0; i < result.Length; ++i)
                    {
                        result[i] = System.IO.Path.GetFileNameWithoutExtension(result[i]);
                    }
                }
                return result;
            }
            return new string[0];
        }

        public static string[] SearchFilesInStorageDir(string searchPattern)
        {
           
            string[] result;
            AddProjectDirIfMissing(ref searchPattern);

            result = Directory.GetFiles(searchPattern);
            return result;
        }

        public static bool FileExists(FilePath file)
        {
            if (file.Storage)
            {
                if (file.UseTimeMark)
                {
                    return SearchFilesInStorageDir(file, false).Length > 0;
                }
                else
                {
                    return File.Exists(file.CompletePath(false));
                }
            }
            else
                return DataLib.SaveLoad.FilesInContentDir(file.LocalDirectoryPath).Length > 0;
        }

        static readonly long MinSaveTime = TimeSpan.FromSeconds(1).Ticks;

        const char TimeMarkSignature = '_';
        public static string FileTimeMark()
        {
            long ticks = DateTime.Now.Ticks;
            ticks /= MinSaveTime;
            return TimeMarkSignature + ticks.ToString();
        }

        /// <returns>lastest save first</returns>
        public static List<string> GetTimeMarkedStoragePaths(FilePath file)
        {
            string[] files = SearchFilesInStorageDir(file, false);

            if (files.Length <= 1)
            {
                return new List<string>(files);
            }
            else
            {
                List<KeyValuePair<long, string>> sortedFiles = new List<KeyValuePair<long, string>> 
                    { 
                        new KeyValuePair<long, string>(filePathTimeMark(files[0]), files[0]),
                    };

                for (int i = 1; i < files.Length; i++)
                {
                    long time = filePathTimeMark(files[i]);
                    for (int sfIx = 0; sfIx < sortedFiles.Count; ++sfIx)
                    {
                        if (sortedFiles[sfIx].Key < time)
                        {
                            sortedFiles.Insert(sfIx, new KeyValuePair<long, string>(time, files[i] ));
                            break;
                        }
                    }
                }

                List<string> result = new List<string>(files.Length);
                foreach (KeyValuePair<long, string> kv in sortedFiles)
                {
                    result.Add(kv.Value);
                }

                return result;
            }
        }

        static long filePathTimeMark(string path)
        {
            const int FileEndLength = 4;

            for (int start = path.Length - FileEndLength; start >= 0; --start)
            {
                if (path[start] == TimeMarkSignature)
                {
                    path = path.Remove(0, start + 1);
                    path = path.Remove(path.Length - FileEndLength, FileEndLength);

                    try
                    {
                        return Convert.ToInt64(path);
                    }
                    catch
                    {
                        Debug.LogError("filePathTimeMark is empty: " + path);
                        return 0;
                    }
                }
            }
            return 0;
        }

        public static void DeleteOldFiles(List<string> paths, int keepAmount)
        {
            //senaste har högst nummer
            for (int i = keepAmount + 1; keepAmount < paths.Count; i++)
            {
                RemoveFile(paths[i]);
            }
        }

        /// <returns>Name, File end</returns>
        public static TwoStrings FileNameWithOutTimeMark(string path)
        {
            TwoStrings result_NameAndFileEnd = new TwoStrings();
            const int FiledEndLenght = 4;
            result_NameAndFileEnd.String2 = path.Remove(0, path.Length - FiledEndLenght);

            int letterIx = path.Length - FiledEndLenght;
            int letterIxStart = letterIx;
            while (path[letterIx] != TimeMarkSignature && letterIx > 0)
            {
                letterIx--;
            }
            if (letterIx == 0)
            { 
                //no time mark
                result_NameAndFileEnd.String1 = path.Remove(path.Length - FiledEndLenght, FiledEndLenght);
                return result_NameAndFileEnd;
            }
            

            result_NameAndFileEnd.String1 = path.Remove(letterIx);

            return result_NameAndFileEnd;
        }

        static void simulateXboxLoadingTime()
        {
#if PCGAME
            if (PlatformSettings.SimulateXboxLoadingTime)
            {
                System.Threading.Thread.Sleep(120);
            }
#endif
        }

    }
}
