using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.LF2.Music
{
    class MusicDirector : AbsUpdateable
    {
        // -spela songer beroende av location
        //-inte bläddra för mycket
        //-boss musik, är högprio, måste stängas av
        //-låt utan loop:
        //    -förlust musik, vid död
        //    -vinna spelet musik
        //-sänka volumnen vid success
        static readonly Range SongRangeBeforeSilence = new Range(4, 6);
        static readonly IntervalF TimeOfSilence = new IntervalF(lib.SecondsToMS(10), lib.SecondsToMS(20));
        static readonly IntervalF CheckAreaSongRate = new IntervalF(lib.SecondsToMS(90), lib.SecondsToMS(120));
        float checkAreaTime = 0;
        SongType currentType;
        Song nextSong;
        PlaySongState playSongState = 0;
        int songsBeforeSilence = SongRangeBeforeSilence.GetRandom();

        public MusicDirector()
            : base(true)
        { }

        public void BossFight(bool begin)
        {
            if (begin)
            {
                playSong(SongType.BossFight);
            }
            else
            {
                if (currentType == SongType.BossFight)
                {
                    playSong(currentAreaType());

                }
            }
        }
        /// <summary>
        /// Force a new song to play
        /// </summary>
        public void NextSong()
        {
            SongType type = currentType; 
            while (type == currentType)
            {
                if ( lib.RandomBool())
                    type = randomBasicFriendly();
                else 
                    type = randomBasicEvil();
            }
            playSong(type);
        }
        public void SuccessSong()
        {
            playSong(SongType.Success);
        }
        public void GameCompletedSong()
        {
            playSong(SongType.GameCompleted);
        }
        public void DeathEvent()
        {
            playSong(SongType.DeathSong);
        }
        public void RestartFromDeath()
        {
            playSongState = PlaySongState.UnLockSong;
            checkAreaTime = lib.SecondsToMS(10);
        }
        public static void PlayIntroSong()
        {
            Engine.Sound.Volume = Engine.Sound.SoundStandardVolume;
            Engine.LoadAndPlaySong.LoadAndPlay(LootfestLib.MusicFolder + "Into_The_Skies", false);
        }
        void playSong(SongType type)
        {
            if (currentType != type)
            {
                currentType = type;
                checkAreaTime = CheckAreaSongRate.GetRandom();
                if (PlatformSettings.PlayMusic)
                    new LoadAndPlaySong(this, type);
            }
        }

        SongType currentAreaType()
        {
            IntVector2 chunk = LfRef.gamestate.LocalHostingPlayer.hero.ScreenPos;
            Map.ChunkData chunkData = LfRef.worldOverView.GetChunkData(chunk);

            if (chunkData.AreaType == Map.Terrain.AreaType.Empty ||  chunkData.AreaType ==  Map.Terrain.AreaType.FlatEmptyAndMonsterFree)
            {
                return randomBasicFriendly();
            }
            else if ( chunkData.AreaType ==  Map.Terrain.AreaType.Castle ||  
                chunkData.AreaType ==  Map.Terrain.AreaType.EndTomb ||

                chunkData.Environment == Map.Terrain.EnvironmentType.Burned ||  
                chunkData.Environment ==   Map.Terrain.EnvironmentType.Desert ||
                chunkData.Environment == Map.Terrain.EnvironmentType.Swamp
                )
            {
                return randomBasicEvil();                
            }
            else
            {   
                return SongType.InTown;
            }
           // throw new ArgumentOutOfRangeException();
        }

        static SongType randomBasicFriendly()
        {
            Range friendly = new Range(
                    (int)SongType.BasicFriendlySongs_BabyBadgers,
                    (int)SongType.BasicFriendlySongs_OldPhilo);
            return (SongType)friendly.GetRandom();
        }
        static SongType randomBasicEvil()
        {
            Range evil = new Range(
                    (int)SongType.BasicEvilSongs_ClumsyLord,
                    (int)SongType.BasicEvilSongs_KingSleepers);
            return (SongType)evil.GetRandom();
        }
        public void SongLoaded(SongType type, Song song)
        {
            if (type == currentType)
            {
                nextSong = song;
                playSongState = PlaySongState.FadeOut;
            }
        }

        public override void Time_Update(float time)
        {
            float FadeSoundSpeed = 0.001f * Engine.Sound.MusicVolume;
            switch (playSongState)
            {
                default:
                    checkAreaTime -= time;
                    if (checkAreaTime <= 0)
                    {
                        if (!LoopSong(currentType))
                        {
                            SongType type = currentAreaType();
                            playSong(type);
                        }
                        else
                        {
                            checkAreaTime = CheckAreaSongRate.GetRandom();
                        }
                    }
                    break;
                case PlaySongState.FadeOut:
                    MediaPlayer.Volume -= FadeSoundSpeed * time;
                    if (MediaPlayer.Volume <= 0)
                    {
                        if (interruptableSong(currentType) && --songsBeforeSilence <= 0)
                        {// have a moment of silence
                            songsBeforeSilence = SongRangeBeforeSilence.GetRandom();
                            playSongState = PlaySongState.Silence;
                            checkAreaTime = TimeOfSilence.GetRandom();
                        }
                        else
                        {
                            bool loop = LoopSong(currentType);
                            Engine.Sound.PlayMusic(nextSong, loop);
                            if (!loop)
                            {
                                checkAreaTime = (float)nextSong.Duration.TotalMilliseconds;
                            }
                            playSongState = PlaySongState.FadeIn;
                        }
                    }
                    break;
                case PlaySongState.FadeIn:
                    MediaPlayer.Volume += FadeSoundSpeed * time;
                    if (MediaPlayer.Volume >= SongVolume(currentType))
                    {
                        MediaPlayer.Volume = SongVolume(currentType);
                        playSongState = PlaySongState.Playing;
                    }
                    
                    break;
                case PlaySongState.UnLockSong:
                     checkAreaTime -= time;
                     if (checkAreaTime <= 0)
                     {
                         SongType type = currentAreaType();
                         playSong(type);
                     }
                    break;
            }
        }

        static bool interruptableSong(SongType type)
        {
            if (type == SongType.BossFight || type == SongType.GameCompleted || type == SongType.Success)
                return false;

            return true;
        }

        static bool LoopSong(SongType type)
        {
            if (type == SongType.GameCompleted || type ==  SongType.Success || type ==  SongType.DeathSong || type ==  SongType.BasicFriendlySongs_BabyBadgers)
                return false;
            else
                return true;
        }

        static float SongVolume(SongType type)
        {
            float mutiply = 1;
            switch (type)
            {
                case SongType.DeathSong:
                    mutiply = 0.6f;
                    break;
            }
            return Engine.Sound.MusicVolume * mutiply;
        }

        public static string TypeName(SongType type)
        {
            switch (type)
            {
                case SongType.BasicFriendlySongs_BabyBadgers: return "BabyBadgersAndABird";
                case SongType.BasicFriendlySongs_Night: return "Night_HQ";
                case SongType.BasicFriendlySongs_SadPorc: return "Sad_Porcupine";
                case SongType.BasicFriendlySongs_Beetles: return "TheBeetles_HQ";
                case SongType.BasicFriendlySongs_LazyFrog: return "TheLazyFrogHQ";
                case SongType.BasicFriendlySongs_OldPhilo: return "TheOldPhilosopherHQ";

                case SongType.BasicFriendlySongs_Mother: return "Mother_loop";
                case SongType.BasicFriendlySongs_PieSensation: return "Apple Pie Sensation_loop";

                case SongType.BasicEvilSongs_ClumsyLord: return "Clumsy_Lord_HQ";
                case SongType.BasicEvilSongs_DungeonKey: return "DungeonKeepersKey_HQ";
                case SongType.BasicEvilSongs_Gargoyle: return "Gargoyle_HQ";
                case SongType.BasicEvilSongs_Immortals: return "Immortals";
                case SongType.BasicEvilSongs_KingSleepers: return "King_Of_Sleepers_HQ";

                case SongType.BasicEvilSongs_Restart: return "Restart_loop";

                case SongType.InTown:
                    return "Solitary_Comfort_HQ";
                case SongType.BossFight:
                    return "YesIAmYourGodHQ";
                case SongType.DeathSong:
                    return "AftermathHQ";
                case SongType.Success:
                    return "Success_HQ";
                case SongType.GameCompleted:
                    return "YAY";
            }
            throw new NotImplementedException();
        }
    }
    class LoadAndPlaySong : QueAndSynch
    {
        SongType songType;
        Song song;
        MusicDirector callBackObj;
        public LoadAndPlaySong(MusicDirector callBackObj,SongType songType)
            :base(true, false)
        {
            this.songType = songType;
            this.callBackObj = callBackObj;
            start();
        }
        protected override bool quedEvent()
        {
            song = Engine.LoadContent.Content.Load<Song>(LootfestLib.MusicFolder + MusicDirector.TypeName(songType));
           return true;
        }
        public override void Time_Update(float time)
        {
            callBackObj.SongLoaded(songType, song);
        }
    }
    enum PlaySongState
    {
        Silence,
        Playing,
        FadeOut,
        FadeIn,
        UnLockSong,
    }
    enum SongType
    {

        BasicFriendlySongs_BabyBadgers,
        BasicFriendlySongs_Night,
        BasicFriendlySongs_SadPorc,
        BasicFriendlySongs_Beetles,
        BasicFriendlySongs_LazyFrog,
        BasicFriendlySongs_Mother,
        BasicFriendlySongs_PieSensation,
        BasicFriendlySongs_OldPhilo,

        BasicEvilSongs_ClumsyLord,
        BasicEvilSongs_DungeonKey,
        BasicEvilSongs_Gargoyle,
        BasicEvilSongs_Immortals,
        BasicEvilSongs_Restart,
        BasicEvilSongs_KingSleepers,

        InTown,
        BossFight,
        DeathSong,
        Success,
        GameCompleted,
    }
}
