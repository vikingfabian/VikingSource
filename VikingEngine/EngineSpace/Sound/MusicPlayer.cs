using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DataStream;
using VikingEngine.DSSWars.Data;
using VikingEngine.EngineSpace.Sound;

namespace VikingEngine.Sound
{
    class MusicPlayer
    {
        public static bool MediaPlayerError = false;

        static SongData currentMedia = null;

        public IntervalF LoopTimesRange = new IntervalF(2, 3);
        public IntervalF DelayBetweenSongs_minutes = new IntervalF(5, 8);
        public float currentDelay = 0;
        PcgRandom random = new PcgRandom();
        
        public static float SongVolumeAdjust = 1f;
        List<SongData> playList;
        int shuffleSongsLeftToPlay = 0;
        PlaySongState playSongState = PlaySongState.Stopped;
        //Song nextSong;
        public SongData nextSongData; 
        public SongData currentSong;
        Time playTime;
        public bool playingFromPlayList = true;

        public bool keepPlaying = true;
        public bool useDelay = true;

        NVorbisPlayer player;
        public bool randomPlayList = true;

        public MusicPlayer()
        {
            player = new NVorbisPlayer
            {
                Volume = 1f,
                IsRepeating = false
            };

            if (Ref.music != null)
            {
                throw new Exception("Two music players");
            }
            //lib.DoNothing();
        }

        /// <summary>
        /// Skips a part of the delay
        /// </summary>
        public void OnGameEvent()
        {
            if (currentDelay <= TimeExt.MinutesToMS(3))
            {
                currentDelay = 0;
            }
        }

        public void OnGameStart()
        {
            nextRandomSong();
            currentDelay = new IntervalF(10000, 20000).GetRandom();
        }

        public void debugNext() 
        {
            nextRandomSong();
            currentDelay = 0;
        }

        public void nextRandomSong()
        {
            if (playList == null) return;

            currentDelay = TimeExt.MinutesToMS(DelayBetweenSongs_minutes.GetRandom());
            keepPlaying = true;
            playSongState = PlaySongState.LoadingSong;
            if (shuffleSongsLeftToPlay <= 0)
            {
                resetPlayList();
            }
            shuffleSongsLeftToPlay--;

            int rndIx;

            if (randomPlayList)
            {
                int loops = 0;
                do
                {
                    rndIx = random.Int(playList.Count);
                    if (++loops > playList.Count * 8)
                    {
                        resetPlayList();
                        loops = 0;
                    }
                } while (playList[rndIx].played);
            }
            else
            {
                for (rndIx = 0; rndIx < playList.Count; rndIx++)
                {
                    if (!playList[rndIx].played)
                    {
                        break;
                    }
                }
            }

            SongData songdata = playList[rndIx];
            songdata.played = true;
            nextSongData = songdata;

            playingFromPlayList = true;
            //new LoadAndPlaySong(this, songdata, false);
            SongLoaded(songdata);
            //return songdata;
        }

        public void stop(bool fade)
        {
            keepPlaying = false;
            if (fade)
            {
                playSongState = PlaySongState.FadeOut;
            }
            else
            {
                StopMusic();
            }
        }

        public void SetPlaylist(List<SongData> playList, bool startPlaying, bool random = true)
        {
            randomPlayList = random;
            this.playList = playList;
            shuffleSongsLeftToPlay = playList.Count;
            if (startPlaying)
            {
                nextRandomSong();
            }
        }

        private void resetPlayList()
        {
            if (playList != null)
            {
                for (int i = 0; i < playList.Count; ++i)
                {
                    var data = playList[i];
                    data.played = false;
                }
                shuffleSongsLeftToPlay = playList.Count;
            }
        }
        public void PlaySong(SongData songdata)
        {
            PlaySong(songdata, false);
        }
        public void PlaySong(SongData songdata, bool isAsynch, bool autoplay = true)
        {
            currentDelay = 0; // TimeExt.MinutesToMS(DelayBetweenSongs_minutes.GetRandom());

            if (PlatformSettings.PlayMusic)
            {
                if (autoplay)
                {
                    playSongState = PlaySongState.LoadingSong;
                    playingFromPlayList = false;
                }
                keepPlaying = autoplay;
                //new LoadAndPlaySong(this, songdata, isAsynch);
                SongLoaded(songdata);
            }
        }

        public string GetSongName()
        {
            if (currentSong != nextSongData)
            {
                return nextSongData.name;
            }
            return currentSong.name;
        }

        public void Update()
        {
            if (PlatformSettings.PlayMusic)
            {
                switch (playSongState)
                {
                    case PlaySongState.Delay:
                        currentDelay -= Ref.DeltaTimeMs;

                        if (currentDelay < 0)
                        {
                            beginNextSong();
                        }
                        break;

                    case PlaySongState.Playing:
                        if (playList != null)
                        {
                            if (playTime.CountDown())
                            {
                                nextRandomSong();
                            }
                        }
                        break;
                    case PlaySongState.FadeOut:
                        //MediaPlayer.Volume -= fadeSoundSpeed * Ref.DeltaTimeMs;
                        player.Volume -= fadeSoundSpeed * Ref.DeltaTimeMs;
                        if (currentSong != null)
                        {
                            if (player.Volume <= 0 || !player.IsPlaying || (!currentSong.seamlessLoop && playTime.TimeOut))
                            {
                                onSongComplete();
                            }
                        }
                        else
                        {
                            onSongComplete();
                        }
                        break;
                    case PlaySongState.FadeIn:
                        player.Volume += fadeSoundSpeed * Ref.DeltaTimeMs;
                        if (player.Volume >= currentVolume)
                        {
                            player.Volume = currentVolume;
                            playSongState = PlaySongState.Playing;
                        }

                        break;
                }
            }
        }

        void onSongComplete()
        {
            if (useDelay)
            {
                StopMusic();
                playSongState = PlaySongState.Delay;
            }
            else
            {
                beginNextSong();
            }
        }

        void beginNextSong()
        {
            if (keepPlaying)
            {
                if (Ref.gamesett.MusicVol() <= 0)
                {
                    playSongState = PlaySongState.Delay;
                    currentDelay = TimeExt.MinutesToMS(DelayBetweenSongs_minutes.GetRandom());
                }
                else
                {
                    currentSong = nextSongData;
                    playTime.MilliSeconds = PlayMusic(nextSongData);

                    if (currentSong.seamlessLoop)
                    {
                        playTime.MilliSeconds *= LoopTimesRange.GetRandom(random);
                        playSongState = PlaySongState.FadeIn;
                    }
                    else
                    {
                        player.Volume = currentVolume;
                        playSongState = PlaySongState.Playing;
                    }
                }
            }
            else
            {
                playSongState = PlaySongState.Stopped;
            }
        }

        float fadeSoundSpeed 
        {
          get { return  0.001f * currentVolume; }
        }

        float currentVolume
        {
            get
            {
                if (currentSong == null)
                    return 0f;
                return Bound.Max( currentSong.volume * Ref.gamesett.MusicVol(), 1f);
            }
        }

        public void SongLoaded(SongData songData/*, Song song*/)
        {
            //nextSong = song;
            nextSongData = songData;

            if (keepPlaying)
            {
                playSongState = PlaySongState.FadeOut;
            }
        }

        public void PlayLoaded()
        {
            keepPlaying = true;
            beginNextSong();
        }

        public void RefreshVolume()
        {
            if (currentSong != null)
            {
                player.Volume = currentVolume;
            }
            else
            {
                player.Volume = Ref.gamesett.MusicVol() * SongVolumeAdjust;
            }
        }

        public bool hasMusicQue()
        {
            return playList != null && playList.Count > 1;
        }

        public bool IsPlaying()
        {
            return playSongState == PlaySongState.Playing && player.Volume > 0;
        }

        public bool IsPlayingNowOrSoon()
        {
            return playSongState == PlaySongState.Playing || 
                (playSongState == PlaySongState.Delay && currentDelay <= 1000);
        }

        public PlaySongState PlaySongState { get { return playSongState; } }

        public int PlayMusic(SongData song)
        {
            if (!string.IsNullOrEmpty( song.filePath))
            {
                try
                {
                    StopMusic();
                    //MediaPlayer.Stop();

                    //currentMedia = s;
                    currentMedia = song;
                    player.IsRepeating = song.seamlessLoop;
                    //FilePath path = new FilePath(
                    string path = Engine.LoadContent.Content.RootDirectory + FilePath.Dir + song.filePath + ".ogg";


                    int ms = (int)player.Play(path).TotalMilliseconds;


                    //MediaPlayer.Play(s);

                    return ms;//(int)player.Duration.TotalMilliseconds;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    MediaPlayerError = true;
                }
            }
            return 0;
        }

        public void Dispose()
        { 
             player.Stop();
            player.Dispose();
        }

        public void StopMusic()
        {
            
            player.Stop();

            //currentMedia?.Dispose();
            //currentMedia = null;
        }
    }

    class LoadAndPlaySong : StorageTask
    {
        static bool MusicBan = false;
        SongData songData;
        Song song;
        MusicPlayer callBackObj;
        public LoadAndPlaySong(MusicPlayer callBackObj, SongData songData, bool fromAsynchContentLoad)
            : base()//true, false)
        {
            if (MusicBan)
            {
                return;
            }
            this.songData = songData;
            this.callBackObj = callBackObj;
            storagePriority = true;

            if (fromAsynchContentLoad)
            {
                runQuedStorageTask();//quedEvent();
                addToSyncedUpdate();//this.AddToUpdateList();//.AddToUpdateList();
            }
            else
            {
                beginStorageTask();//start();
            }
        }
        public override void runQuedStorageTask()
        {
            base.runQuedStorageTask();
            try
            {
                song = Engine.LoadContent.Content.Load<Song>(songData.filePath);
            }
            catch (Exception e)
            {
                MusicBan = true;
            }
        }

        public override void runSyncAction()
        {
            base.runSyncAction();
            onStorageComplete();
        }

        public override void onStorageComplete()
        {
            base.onStorageComplete();
            //callBackObj.SongLoaded(songData, song);
        }
    }

    class SongData
    {
        //public bool play = true;
        public string filePath;
        public bool seamlessLoop;
        public float volume;
        public bool played;
        public string name;
        public string artist;
        //Song storedSong;

        public SongData(string filePath, bool seamlessLoop, float volume)
            : this(filePath, null, null, seamlessLoop, volume)
        { }

        public SongData(string filePath, string name, string artist, bool seamlessLoop, float volume)
        {
            this.filePath = filePath;
            this.name = name;
            this.artist = artist;
            this.seamlessLoop = seamlessLoop;
            this.volume = volume;
            played = false;
        }

        public void LoadAndStore()
        {
            //storedSong = Engine.LoadContent.Content.Load<Song>(filePath);
        }

        public int Hash()
        {
            return name.GetDeterministicHashCode();
        }

        public void PlayStored()
        {
            if (PlatformSettings.PlayMusic)
            {
                MusicPlayer.SongVolumeAdjust = volume;
                MediaPlayer.Volume = MusicPlayer.SongVolumeAdjust * Ref.gamesett.MusicVol();
                //MusicPlayer.PlayMusic(storedSong, seamlessLoop);
            }
        }

    }

    enum PlaySongState
    {
        Stopped,
        Delay,
        Playing,
        LoadingSong,
        FadeOut,
        FadeIn,

        UnLockSong,
    }
}
