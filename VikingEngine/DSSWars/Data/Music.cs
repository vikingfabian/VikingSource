using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars.Data
{
    static class Music
    {
        static readonly string FilePath = DssLib.ContentDir + "Music" + DataStream.FilePath.Dir;

        public static readonly SongData Intro = new SongData(FilePath + "dramatic-opener-nonloop", false, 0.5f);
        public static readonly SongData Nightmare = new SongData(FilePath + "epic-warfare-nonloop", false, 1f);
        public static readonly SongData DoomStory = new SongData(FilePath + "shadow-hunter-nonloop", false, 1f);
        public static readonly SongData Victory = new SongData(FilePath + "we-are-heroes", false, 0.8f);
        public static readonly SongData Fail = new SongData(FilePath + "sadness-in-blue", false, 0.5f);

    }
}
