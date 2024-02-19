using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VikingEngine.DataStream
{
    struct FilePath
    {
        public static readonly char Dir = System.IO.Path.DirectorySeparatorChar;//'\\';

        public static readonly string StorageFolderName =
#if PJ
                "PartyJousting";    
#elif CCG
                "PickHero";
#elif DSS
                "DSS_warparty"; 
#elif TOGG
                "TowardsGoldAndGlory"; 
#else
                "Lootfest";
#endif

        public string LocalDirectoryPath;
        public string FileName;
        /// <remark>
        /// set to null if the path is exact (ex: when path is searched)
        /// </remark>
        public string FileEnd; //must include "."
        public bool Storage;
        public bool Content
        {
            get { return !Storage; }
        }
        public int NumVersionsStacking; //normally one
        public bool UseTimeMark;

        public FilePath(string directoryPath)
            : this(directoryPath, null, null, true, 1, true)
        { }

        public FilePath(string directoryPath, string fileName, string fileEnd)
            : this(directoryPath, fileName, fileEnd, true, 1, true)
        { }

        public FilePath(string directoryPath, string fileName, string fileEnd, bool storage)
            : this(directoryPath, fileName, fileEnd, storage, 1, true)
        { }
        public FilePath(string directoryPath, string fileName, string fileEnd, bool storage, bool useVersionNumber)
            : this(directoryPath, fileName, fileEnd, storage, 1, useVersionNumber)
        { }

        public FilePath(string directoryPath, string fileName, string fileEnd, bool storage, int numVersions, bool useVersionNumber)
        {
            this.UseTimeMark = useVersionNumber;
            LocalDirectoryPath = directoryPath; FileName = fileName; FileEnd = fileEnd; Storage = storage; NumVersionsStacking = numVersions;
        }

        public string CompletePath(bool save)
        {
            if (FileEnd == null)
            {
                return FileName;
            }
            else if (UseTimeMark && save)
            {
                string dir = CompleteDirectory;
                if (CompleteDirectory != TextLib.EmptyString)
                    dir += Dir;
                return dir + FileName + DataStreamHandler.FileTimeMark() + FileEnd;
            }
            else
            {
                string dir = CompleteDirectory;
                if (CompleteDirectory != TextLib.EmptyString)
                    dir += Dir;
                return dir + FileName + FileEnd;
            }
        }

        public string CompleteLocalPath(bool save)
        {
            string folder;
            if (TextLib.HasValue(LocalDirectoryPath))
            {
                folder = LocalDirectoryPath + Dir;
            }
            else
            {
                folder = "";
            }
            
            return folder + FileName + FileEnd;
        }

        public string CompleteDirectory
        {
            get
            {
                string result = Storage ? StorageDirectory() : Engine.LoadContent.Content.RootDirectory;

                if (TextLib.HasValue(LocalDirectoryPath))
                {
                    if (TextLib.IsEmpty(result))
                        result = LocalDirectoryPath;
                    else
                        result += Dir + LocalDirectoryPath;
                }

                if (result == "\\")
                    throw new Exception();

                return result;
            }
        }


        

        static string PcStoragePath = null;

        public static string StorageDirectory()
        {
#if PCGAME
            if (PlatformSettings.ReleaseBuild && PlatformSettings.PC_platform)
            {
                if (PcStoragePath == null)
                {
                    PcStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
                        "\\My Games\\" +
                        StorageFolderName;
                }
//#if PJ
//                "PartyJousting";}
//#elif CCG
//                "PickHero";}
//#elif DSS
//                "LootfestWars"; }
//#elif TOGG
//                "TowardsGoldAndGlory"; }
//#else
//                "Lootfest";}
//#endif
                return PcStoragePath;
            }
            else
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
#else
            return TextLib.EmptyString; 
#endif
        }
        
        public static void InitStorage()
        {
            Directory.CreateDirectory(StorageDirectory());
        }

        public static string CreateStorageFolder(string name)
        {
            try
            {
                string newDir = StorageDirectory() + Dir + name;
                Directory.CreateDirectory(newDir);
                return newDir;
            }
            catch (Exception e)
            {
                Debug.LogError("CreateFolder, " + e.Message);
                return "";
            }
        }

        public string[] listFilesInDir()
        {
            if (Storage)
            {
                return DataStreamHandler.SearchFilesInStorageDir(this, true);
            }
            else
            {
                return DataLib.SaveLoad.FilesInContentDir(LocalDirectoryPath);
            }
        }
        
        public void CheckFileLength()
        {
            const int MaxFileLength = 40;
            if (FileName.Length + 4 > MaxFileLength)
                throw new Exception("To long file name: " + this.ToString());
        }
        public override string ToString()
        {
            return FileName + FileEnd;
        }

        public string NameAndEnd()
        {
            return FileName + FileEnd;
        }

        public bool Exists()
        {
            return DataStream.DataStreamHandler.FileExists(this);
        }
    }
    
}
