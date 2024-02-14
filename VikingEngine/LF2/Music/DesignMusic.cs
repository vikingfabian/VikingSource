using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Music
{
    #if CMODE
    static class MusicManager
    {
        static CirkleCounterUp currentSong;
        static List<byte> songList;
        static int loadingTime = 0;

        public static void Init()
        {
            songList = new List<byte> { 0 };
            currentSong = new CirkleCounterUp((byte)SongName.NUM - 1);
            for (byte i = 1; i < (byte)SongName.NUM; i++)
            {
                songList.Insert(Ref.rnd.Int(i), i);
            }
            // LasyCheck();

            const string ThemeSong = 

                "my_first_creation";

            Engine.LoadContent.LoadAndPlaySongThreaded(ThemeSong);

        }
        public static void NextSong()
        {
            Engine.Sound.StopMusic();
            loadingTime = 4;
        }
        public static void LasyCheck()
        {
            if (PlatformSettings.PlayMusic)
            {
                loadingTime--;
                if (loadingTime <= 0)
                {
                    loadingTime =
                        2000;
                    if (!Engine.Sound.IsPlaying && !MusicLib.MusicMute)
                    {
                        if (songList != null)
                        {
                            currentSong.Next();
                            string songName = ((SongName)songList[currentSong.Value]).ToString();
                            
                            Engine.LoadContent.LoadAndPlaySongThreaded(songName);
                            if (Engine.StateHandler.CurrentState.Type == Engine.GameStateType.InGame)
                            {
                                songName = songName.Replace('_', ' ');
                                bool max =

                                    songList[currentSong.Value] < (byte)SongName.Brute_Force;

                                LfRef.gamestate.LocalHostingPlayer.ViewSongName(songName,
                                    max ?
                                    "Max Björkegren" : "Snild Dolkow");
                                Engine.Sound.SongFileVolumeAdjust = max ? 1 : 0.5f;
                            }
                        }
                    }
                }
            }
        }
        public static void ZombieHorde()
        {
            loadingTime -= 500;
        }
    }
    enum SongName : byte
    {
        //MAX
        lazy_town,

        
        Loot,
        my_beard_is_gone,
        rise,
        sandy_dungeon,
        gather_mah_stuff,
        Hammering,
        How_does_this_work,
        mina_nudlar_aer_kallast,
        Looks_pretty_dark_to_be_mid_day,
        hero_ready_to_be_demised,

        //SNILK
        Brute_Force,
        Carmacks_Reverse,
        From_Pixels_Born,
        Monads,
        Radioactive_Crypto_Attack,
        RedBlack_Trees,
        Segfault,
        Undefined_Behavior,
        x13,
        Coding_Blind,
        

        NUM,

        my_first_creation,

       
    }
#endif
}
