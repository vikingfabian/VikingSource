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

        public static readonly SongData Intro = new SongData(FilePath + "dramatic-opener-nonloop", "Dramatic Opener", false, 0.5f);
        public static readonly SongData Nightmare = new SongData(FilePath + "epic-warfare-nonloop", "Epic Warfare", false, 1f);
        public static readonly SongData DoomStory = new SongData(FilePath + "shadow-hunter-nonloop", "Shadow Hunter", false, 1f);
        public static readonly SongData Victory = new SongData(FilePath + "we-are-heroes", "We are heroes", false, 0.8f);
        public static readonly SongData Fail = new SongData(FilePath + "Sadness in blue", "Sadness in blue", false, 0.5f);

        public static readonly SongData IAmYourDoom = new SongData(FilePath + "i am your doom", "I am your doom", false, 0.45f);

        public static readonly SongData Tutorial = new SongData(FilePath + "DSS - The Game Begins", "DSS - The Game Begins", false, 0.3f);
        //public static string FilePath = DssLib.ContentDir + "Music\\";
        public static new List<Sound.SongData> PlayList()
        {
            return new List<Sound.SongData>
            {
                new Sound.SongData(FilePath + "BBaaB_loop", "BBaaB", true, 0.22f),
                new Sound.SongData(FilePath + "Gargoyle_loop", "Gargoyle", true, 0.3f),

                new Sound.SongData(FilePath + "RM 10 - Incubation", "Incubation", false, 0.3f),
                new Sound.SongData(FilePath + "RM 2 - Arcane Benevolence","Arcane Benevolence", false, 0.22f),
                new Sound.SongData(FilePath + "RM 3 - Left in Autumn","Left in Autumn", false, 0.11f),
                new Sound.SongData(FilePath + "RM 4 - Warhogs", "Warhogs", false, 0.2f),
                new Sound.SongData(FilePath + "RM 5 - Suddenly Empty","Suddenly Empty", false, 0.15f),
                new Sound.SongData(FilePath + "RM 6 - Auderesne","Auderesne", false, 0.2f),
                new Sound.SongData(FilePath + "RM 7 - For Eternity","For Eternity", false, 0.18f),
                new Sound.SongData(FilePath + "RM 8 - Asynchronous Flanking","Asynchronous Flanking", false, 0.13f),
                new Sound.SongData(FilePath + "RM 9 - Weeping Bedlam","Weeping Bedlam", false, 0.18f),

                new Sound.SongData(FilePath + "digital battleground","Digital Battleground", false, 0.2f),
                new Sound.SongData(FilePath + "echoes of valor","Echoes of Valor", false, 0.18f),
                new Sound.SongData(FilePath + "Pixelated Battlefields","Pixelated Battlefields", false, 0.18f),

                new Sound.SongData(FilePath + "ancient space","Ancient Space", false, 0.22f),
                new Sound.SongData(FilePath + "Dreamscape Adventures","Dreamscape Adventures", false, 0.2f),
                new Sound.SongData(FilePath + "Shadows of Conflict","Shadows of Conflict", false, 0.2f),
                new Sound.SongData(FilePath + "Veil of Time","Veil of Time", false, 0.3f),


                 new Sound.SongData(FilePath + "What Lurks Below","What Lurks Below", false, 0.3f),
                new Sound.SongData(FilePath + "Arcadia","Arcadia", false, 0.3f),
                new Sound.SongData(FilePath + "Elysian_Dreamscape","Elysian Dreamscape", false, 0.4f),
                new Sound.SongData(FilePath + "Epic Shadows of the Fallen","Epic Shadows of the Fallen", false, 0.5f),
                new Sound.SongData(FilePath + "Guitar Shadows of the Fallen","Guitar Shadows of the Fallen", false, 0.6f),
                new Sound.SongData(FilePath + "Legends of Valor","Legends of Valor", false, 0.4f),

            };
        }

        public static new List<Sound.SongData> OtherSongs()
        {
            return new List<Sound.SongData>
            {
                Intro,
                Nightmare,
                DoomStory,
                Victory,
                Fail,
                IAmYourDoom,
                Tutorial,
            };
        }
    } 
}
