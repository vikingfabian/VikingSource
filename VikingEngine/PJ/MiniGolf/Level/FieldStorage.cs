using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;

namespace VikingEngine.PJ.MiniGolf
{
    class FieldStorage
    {
       public static readonly LevelName[] RetailLevels = new LevelName[]
       {
            LevelName.BoxInBox1,
            LevelName.BoxInBox2,
            LevelName.ArrowBarrel,
            LevelName.angrymouth,
            LevelName.blob,
            LevelName.endgoal,
            LevelName.et,
            LevelName.fivesquare,
            LevelName.lines,
            LevelName.plus,
            LevelName.snake1,
            LevelName.tarm,

       };

        const int SaveVersion = 1;
        public const string LevelsFolder = "MiniGolf";
        public string saveFileName = RandomName();
        public bool isLoaded = false;

        public string DebugLevel = null;//"ArrowBarrel";

        public void loadLevel(LevelName lvl)
        {
            loadLevel(lvl.ToString(), false);
        }

        public void loadLevel(string map, bool fromStorage)
        {
            isLoaded = false;
            saveFileName = map;
            saveLoad(false, false, fromStorage);
        }

        public void saveAsynch()
        {
            saveLoad(true, true, true);
        }

        void saveLoad(bool save, bool alreadythreaded, bool fromStorage)
        {
            //IStreamIOCallback callback = null;
            if (save)
            {
                fromStorage = true;               
            }
            FilePath filePath = file(fromStorage);
            filePath.FileName = saveFileName;

            if (save)
            {
                System.IO.Directory.CreateDirectory(filePath.CompleteDirectory);
            }

            if (!save && PlatformSettings.DevBuild &&
                DebugLevel != null)
            {
                fromStorage = true;
                filePath = file(fromStorage);
                saveFileName = DebugLevel;
                filePath.FileName = DebugLevel;
            }

            filePath.FileEnd = ".lvl";

            new DataStream.FileIO_2(save, filePath, write, read, !alreadythreaded, true,
                null, null);
            //DataStream.BeginReadWrite.BinaryIO(save, filePath, write, read_asynch, callback, !alreadythreaded);
        }

        public static FilePath file(bool fromStorage)
        {
            FilePath filePath = new FilePath(
                fromStorage ?
                    LevelsFolder :
                    PjLib.ContentFolder + LevelsFolder,
                null,
                null,
                fromStorage,
                false);

            return filePath;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(SaveVersion);

            GolfRef.field.write(w);
        }

        //public void read_asynch(System.IO.BinaryReader r)
        //{
        //    var readerIns = new MemoryStreamHandler().CloneReader(r);
        //    new Timer.Action1ArgTrigger<System.IO.BinaryReader>(read, readerIns);
        //}

        public void read(System.IO.BinaryReader r)
        {
            GolfRef.field.clearMap(false); 

            int version = r.ReadInt32();

            GolfRef.field.read(r, version);

            isLoaded = true;
            GolfRef.gamestate.onMapLoaded();
        }
        
        public static string RandomName()
        {
            return "Field" + Ref.rnd.Int(9999).ToString();
        }
    }

    enum LevelName
    {
        angrymouth,
        blob,
        endgoal,
        et,
        fivesquare,
        lines,
        plus,
        snake1,
        tarm,
        ArrowBarrel,
        BoxInBox1,
        BoxInBox2,

        NumNon,
        Custom,
    }
}