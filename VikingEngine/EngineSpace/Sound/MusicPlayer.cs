using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.Sound
{
    class MusicPlayer
    {
        public IntervalF LoopTimesRange = new IntervalF(2, 3);
        public IntervalF DelayBetweenSongs_minutes = new IntervalF(5, 8);
        public float currentDelay = 0;
        PcgRandom random = new PcgRandom();
        public static float MasterVolume = 1f;
        public static float SongVolumeAdjust = 1f;
        List<SongData> playList;
        int shuffleSongsLeftToPlay = 0;
        PlaySongState playSongState = PlaySongState.Stopped;
        Song nextSong;
        public SongData nextSongData; 
        public SongData currentSong;
        Time playTime;
        public bool playingFromPlayList = true;

        public bool keepPlaying = true;
        public bool useDelay = true;


        public MusicPlayer()
        {
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
            currentDelay = new IntervalF(5000, 8000).GetRandom();
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

            int rndIx;
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
            
            SongData songdata = playList[rndIx];
            songdata.played = true;
            nextSongData = songdata;

            playingFromPlayList = true;
            new LoadAndPlaySong(this, songdata, false);

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
                MediaPlayer.Stop();
            }
        }

        public void SetPlaylist(List<SongData> playList, bool startPlaying)
        {
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
                new LoadAndPlaySong(this, songdata, isAsynch);
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
                        MediaPlayer.Volume -= fadeSoundSpeed * Ref.DeltaTimeMs;
                        if (currentSong != null)
                        {
                            if (MediaPlayer.Volume <= 0 || MediaPlayer.State != MediaState.Playing || (!currentSong.seamlessLoop && playTime.TimeOut))
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
                        MediaPlayer.Volume += fadeSoundSpeed * Ref.DeltaTimeMs;
                        if (MediaPlayer.Volume >= currentVolume)
                        {
                            MediaPlayer.Volume = currentVolume;
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
                if (MasterVolume <= 0)
                {
                    playSongState = PlaySongState.Delay;
                    currentDelay = TimeExt.MinutesToMS(DelayBetweenSongs_minutes.GetRandom());
                }
                else
                {
                    currentSong = nextSongData;
                    playTime.MilliSeconds = Engine.Sound.PlayMusic(nextSong, currentSong.seamlessLoop);

                    if (currentSong.seamlessLoop)
                    {
                        playTime.MilliSeconds *= LoopTimesRange.GetRandom(random);
                        playSongState = PlaySongState.FadeIn;
                    }
                    else
                    {
                        MediaPlayer.Volume = currentVolume;
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
                return currentSong.volume * MasterVolume;
            }
        }

        public void SongLoaded(SongData songData, Song song)
        {
            nextSong = song;
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

        public void SetVolume(float masterVolume)
        {
            MasterVolume = masterVolume;
            if (currentSong != null)
            {
                 MediaPlayer.Volume = currentVolume;
            }
            else
            {
                MediaPlayer.Volume = MasterVolume * SongVolumeAdjust;
            }
        }

        public bool hasMusicQue()
        {
            return playList != null && playList.Count > 1;
        }

        public bool IsPlaying()
        {
            return playSongState != PlaySongState.Stopped;
        }

        public PlaySongState PlaySongState { get { return playSongState; } }
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
            // return true;
        }

        public override void runSyncAction()
        {
            base.runSyncAction();
            onStorageComplete();
        }

        public override void onStorageComplete()
        {
            base.onStorageComplete();
            callBackObj.SongLoaded(songData, song);
        }
    }

    class SongData
    {
        public string filePath;
        public bool seamlessLoop;
        public float volume;
        public bool played;
        public string name;
        Song storedSong;

        public SongData(string filePath, bool seamlessLoop, float volume)
            : this(filePath, null, seamlessLoop, volume)
        { }

        public SongData(string filePath, string name, bool seamlessLoop, float volume)
        {
            this.filePath = filePath;
            this.name = name;
            this.seamlessLoop = seamlessLoop;
            this.volume = volume;
            played = false;
        }

        public void LoadAndStore()
        {
            storedSong = Engine.LoadContent.Content.Load<Song>(filePath);
        }

        public void PlayStored()
        {
            if (PlatformSettings.PlayMusic)
            {
                MusicPlayer.SongVolumeAdjust = volume;
                MediaPlayer.Volume = MusicPlayer.SongVolumeAdjust * MusicPlayer.MasterVolume;
                Engine.Sound.PlayMusic(storedSong, seamlessLoop);
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
