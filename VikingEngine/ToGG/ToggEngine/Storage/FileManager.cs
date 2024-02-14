using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.ToGG.Data
{
    class FileManager : VikingEngine.DataStream.IStreamIOCallback
    {
        const string MapFileEnd = ".map";
        const string SaveStateFileEnd = ".sst";


        public bool lockedInSaving = false;
        public string fileName = newMapName();
        VikingEngine.DataStream.IStreamIOCallback parentCallback;
        public DataStream.ReadToMemory data;
        //public HeroQuest.Data.QuestSetup quest;

        public FileManager(VikingEngine.DataStream.IStreamIOCallback parentCallback)
        {
            this.parentCallback = parentCallback;
        }

        public DataStream.FilePath mapfilePath(bool toStorage)
        {
            var result = MapfolderPath(toStorage);
            result.FileName = fileName;

            return result;
        }

        public static DataStream.FilePath MapfolderPath(bool toStorage)
        {
            return new DataStream.FilePath(toStorage ? toggLib.MapsSaveFolder : toggLib.ContentMapsFolder,
                null, MapFileEnd, toStorage, false);
        }

        public DataStream.FilePath statefilePath()
        {
            return new DataStream.FilePath(toggLib.SaveStateFolder,
                "state0", SaveStateFileEnd, true, false);
        }

        public void saveMap()
        {
            lockedInSaving = true;
            var filePath = mapfilePath(true);
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            new DataStream.WriteBinaryIO(filePath, writeMap, this);
        }

        public void saveState()
        {
            lockedInSaving = true;
            var filePath = statefilePath();
            System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            new DataStream.WriteBinaryIO(filePath, writeState, this);
        }

        public void loadState()
        {
            lockedInSaving = true;

            //toggRef.board.clearOutMap();

            new DataStream.ReadBinaryIO(statefilePath(), readState, this);
        }

        public void loadFile()
        {
            //this.quest = quest;

            if (HeroQuest.hqRef.setup.quest == HeroQuest.QuestName.None)
            {
                throw new Exception();
            }
            else if (HeroQuest.hqRef.setup.quest == HeroQuest.QuestName.Custom)
            {
                loadFile(HeroQuest.hqRef.setup.customName, true);
            }
            else
            {
                loadFile(HeroQuest.hqRef.setup.quest.ToString(), false);
            }
        }

        public void loadFile(string fileName, bool storage)
        {
            lockedInSaving = true;
            this.fileName = fileName;
            data = new DataStream.ReadToMemory(mapfilePath(storage), this);
            //lockedInSaving = false;
        }

        public void loadMapFromMemory()
        {
            toggRef.board.clearOutMap();
            System.IO.BinaryReader r = data.memory.GetReader();
            readMap(r);

            toggRef.board.refresh();
        }

        static string newMapName()
        {
            return "map" + Ref.rnd.Int(10000).ToString();
        }

        public void SaveComplete(bool save, int player, bool successful, byte[] value)
        {
            lockedInSaving = false;

            if (successful)
            {
               
                if (!save && toggRef.InEditor)
                {
                    loadMapFromMemory();
                    //toggRef.board.refresh();
                }

                if (parentCallback != null)
                {
                    parentCallback.SaveComplete(save, player, successful, value);
                }
            }
        }

        public void writeMapToMemory()
        {
            if (data == null)
            {
                data = new DataStream.ReadToMemory();
            }
            var w = data.writeToMemory();
            writeMap(w);
        }

        void writeMap(System.IO.BinaryWriter w)
        {
            DataStream.FileVersion.Write(w,
                ToggEngine.StorageLib.MapReleaseVersion,
                ToggEngine.StorageLib.MapDevSubVersion);

            write(w, false);
        }

        void writeState(System.IO.BinaryWriter w)
        {
            DataStream.FileVersion.Write(w,
                ToggEngine.StorageLib.MapReleaseVersion,
                ToggEngine.StorageLib.MapDevSubVersion);

            write(w, true);
        }

        void write(System.IO.BinaryWriter w, bool isSaveState)
        {
            toggRef.board.Write(w, isSaveState);
            toggRef.absPlayers.WriteUnitSetup(w);
        }

        const bool ImportOldMaps = true;

        public void readMap(System.IO.BinaryReader r)
        {
            if (ImportOldMaps)
            {

                toggRef.board.ReadOldVersion(r);
            }
            else
            { 
                DataStream.FileVersion version = new DataStream.FileVersion(r);

                read(r, version);

                toggRef.gamestate.NewMapPrep();
                toggRef.board.onLoadComplete();
            }
            //toggRef.gamestate.MapLoadComplete();
        }

        public void readState(System.IO.BinaryReader r)
        {
            DataStream.FileVersion version = new DataStream.FileVersion(r);
            version.isSaveState = true;

            read(r, version);

            toggRef.board.onLoadComplete();
        }

        public void read(System.IO.BinaryReader r, DataStream.FileVersion version)
        {            
            toggRef.board.Read(r, version);
            toggRef.absPlayers.ReadUnitSetup(r, version);
        }

    }
}
