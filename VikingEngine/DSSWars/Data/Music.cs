using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars.Data
{
    static class Music
    {
        const string Artist_MartinGronlund = "Martin Grönlund";
        const string Artist_CosmeLeites = "Cosme Leites";
        const string Artist_SoundImage = "by Eric Matyas, www.soundimage.org";

        static readonly string FilePath = DssLib.ContentDir + "Music" + DataStream.FilePath.Dir;
        static readonly string SoundOrgPath = FilePath + "soundimage org" + DataStream.FilePath.Dir;

        public static readonly SongData Intro = new SongData(FilePath + "dramatic-opener-nonloop", "Dramatic Opener", null, false, 0.5f);
        public static readonly SongData CardGameIntro = new SongData(FilePath + "CCGmusic_mastered", "CCG music", Artist_MartinGronlund, false, 0.5f);
        public static readonly SongData Nightmare = new SongData(FilePath + "epic-warfare-nonloop", "Epic Warfare", null, false, 1f);
        public static readonly SongData DoomStory = new SongData(FilePath + "shadow-hunter-nonloop", "Shadow Hunter", null, false, 1f);
        public static readonly SongData Victory = new SongData(FilePath + "we-are-heroes", "We are heroes", null, false, 0.8f);
        public static readonly SongData Fail = new SongData(FilePath + "Sadness in blue", "Sadness in blue", null, false, 0.5f);

        public static readonly SongData AncientSpace = new Sound.SongData(FilePath + "ancient space", "Ancient Space", null, false, 0.22f);
        public static readonly SongData AncientGameMenu = new Sound.SongData(SoundOrgPath + "Ancient-Game-Menu_looping", "Ancient Game Menu", Artist_SoundImage, true, 0.25f);
        static bool firstMenuEnter = true;

        public static new List<Sound.SongData> MenuPlayList(out bool random)
        {
            var result = new List<Sound.SongData>(4);

            random = !firstMenuEnter;
            if (firstMenuEnter)
            {
                firstMenuEnter = false;
                result.Add(Intro);
            }
            result.Add(CardGameIntro);
            result.Add(AncientSpace);
            result.Add(AncientGameMenu);
            
            return result;
        }

        public static new List<Sound.SongData> PlayList()
        {
           

            var result = new List<Sound.SongData>
            {
                //new Sound.SongData(FilePath + "BBaaB_loop", "BBaaB", true, 0.22f),
                new Sound.SongData(FilePath + "DSS War Industry -Theme music", "DSS Theme music", Artist_CosmeLeites, true, 0.3f),
                new Sound.SongData(FilePath + "Gargoyle_loop", "Gargoyle", Artist_MartinGronlund, true, 0.3f),

                //new Sound.SongData(FilePath + "RM 10 - Incubation", "Incubation", false, 0.3f),
                new Sound.SongData(FilePath + "RM 2 - Arcane Benevolence","Arcane Benevolence", Artist_MartinGronlund, false, 0.22f),
                new Sound.SongData(FilePath + "RM 3 - Left in Autumn","Left in Autumn", Artist_MartinGronlund, false, 0.11f),
                new Sound.SongData(FilePath + "RM 4 - Warhogs", "Warhogs", Artist_MartinGronlund, false, 0.2f),
                //new Sound.SongData(FilePath + "RM 5 - Suddenly Empty","Suddenly Empty", false, 0.15f),
                new Sound.SongData(FilePath + "RM 6 - Auderesne","Auderesne", Artist_MartinGronlund, false, 0.2f),
                new Sound.SongData(FilePath + "RM 7 - For Eternity","For Eternity", Artist_MartinGronlund, false, 0.18f),
                new Sound.SongData(FilePath + "RM 8 - Asynchronous Flanking","Asynchronous Flanking", Artist_MartinGronlund, false, 0.13f),
                new Sound.SongData(FilePath + "RM 9 - Weeping Bedlam","Weeping Bedlam", Artist_MartinGronlund, false, 0.18f),

                new Sound.SongData(FilePath + "digital battleground","Digital Battleground", null, false, 0.2f),
                new Sound.SongData(FilePath + "echoes of valor","Echoes of Valor", null, false, 0.18f),
                new Sound.SongData(FilePath + "Pixelated Battlefields","Pixelated Battlefields", null, false, 0.18f),

                AncientSpace,
                new Sound.SongData(FilePath + "Dreamscape Adventures","Dreamscape Adventures", null, false, 0.2f),
                new Sound.SongData(FilePath + "Shadows of Conflict","Shadows of Conflict", null, false, 0.2f),
                new Sound.SongData(FilePath + "Veil of Time","Veil of Time", null, false, 0.3f),


                new Sound.SongData(FilePath + "What Lurks Below","What Lurks Below", null, false, 0.3f),
                new Sound.SongData(FilePath + "Arcadia","Arcadia", null, false, 0.3f),
                new Sound.SongData(FilePath + "Elysian_Dreamscape","Elysian Dreamscape", null, false, 0.4f),
                new Sound.SongData(FilePath + "Epic Shadows of the Fallen","Epic Shadows of the Fallen", null, false, 0.5f),
                new Sound.SongData(FilePath + "Guitar Shadows of the Fallen","Guitar Shadows of the Fallen", null, false, 0.6f),
                new Sound.SongData(FilePath + "Legends of Valor","Legends of Valor", null, false, 0.4f),
                new Sound.SongData(FilePath + "A History of Cubes - Gameplay Edit","A History of Cubes - Gameplay Edit", "Rymdreglage", false, 4f),

                new Sound.SongData(FilePath +  "37 (longer)", "37 - longer", "justmike", false, 3f),
               

                //new Sound.SongData(FilePath + "Endless Plains", "Endless Plains", true, 0.4f),
                //new Sound.SongData(FilePath + "MissingCardinals", "Missing Cardinals", false, 0.8f),
                //new Sound.SongData(FilePath + "Mysterious Grotto", "Mysterious Grotto", true, 1f),
                //new Sound.SongData(SoundOrgPath + "Ancient-Game-Menu_looping", "Ancient Game Menu", Artist_SoundImage, true, 0.25f),
                AncientGameMenu,
                new Sound.SongData(SoundOrgPath + "City-Beneath-the-Waves", "City Beneath the Waves", Artist_SoundImage, false, 0.29f),
                new Sound.SongData(SoundOrgPath + "Crossing-the-Tundra_v001_Looping", "Crossing the Tundra v001", Artist_SoundImage, true, 0.28f),
                new Sound.SongData(SoundOrgPath + "Cumulonimbus", "Cumulonimbus", Artist_SoundImage, false, 0.25f),
                new Sound.SongData(SoundOrgPath + "Dragon-Mystery_Looping", "Dragon Mystery_Looping", Artist_SoundImage, true, 0.23f),
                new Sound.SongData(SoundOrgPath + "Dreamy-Game-Intro_Looping", "Dreamy Game Intro_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Eerie-Techno-Game-Open_Looping", "Eerie Techno Game Open_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Fantascape_Looping", "Fantascape_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Flurries", "Flurries", Artist_SoundImage, false, 0.3f),
                new Sound.SongData(SoundOrgPath + "Gliding", "Gliding", Artist_SoundImage, false, 0.25f),
                new Sound.SongData(SoundOrgPath + "History-Piano", "History Piano", Artist_SoundImage, false, 0.25f),
                new Sound.SongData(SoundOrgPath + "Key-West-Sunset_Looping", "Key West Sunset_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Kingdom-Quest_LoFi_looping", "Kingdom Quest_LoFi_looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Moonlight-Flying_Looping", "Moonlight Flying_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Mysterious-Deep", "Mysterious Deep", Artist_SoundImage, false, 0.25f),
                new Sound.SongData(SoundOrgPath + "Of-Legends-and-Fables-3_looping", "Of Legends and Fables 3", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Once-Upon-a-Time-in-the-Kingdom_looping", "Once Upon a Time in the Kingdom", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Our-Mountain_v003_Looping", "Our Mountain_v003_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Pond-at-Twilight_Looping", "Pond at Twilight_Looping", Artist_SoundImage, true, 0.22f),
                new Sound.SongData(SoundOrgPath + "Secret-Journey_Looping", "Secret Journey_Looping", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "Spooky-Enchantment_looping", "Spooky Enchantment", Artist_SoundImage, true, 0.25f),
                new Sound.SongData(SoundOrgPath + "The-Spunky-Princess_looping", "The Spunky Princess", Artist_SoundImage, true, 0.25f),

            };

            return result;
        }

        public static List<Sound.SongData> SongList_RemoveMuted()
        {
            Dictionary<int, Sound.SongData> hashedList = new Dictionary<int, Sound.SongData>();

            var result = PlayList();

            foreach (var item in result)
            { 
                hashedList.Add(item.Hash(), item);
            }

            hashedList.Remove(3);

            foreach (var item in DssRef.storage.mutedSongs)
            {
                hashedList.Remove(item);
            }

            result.Clear();
            foreach (var kv in hashedList)
            {
                result.Add(kv.Value);
            }

            return result;
        }

        public static new List<Sound.SongData> OtherSongs()
        {
            return new List<Sound.SongData>
            {
                Intro,
                CardGameIntro,
                Nightmare,
                DoomStory,
                Victory,
                Fail,
                //IAmYourDoom,
                //Tutorial,
            };
        }
    } 
}
