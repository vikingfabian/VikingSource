using System;
using System.IO;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.Engine
{
    static class Sound
    {
        static Song currentSong = null;

        public const float SterioUnitReduction = 0.05f;
        public const float SterioMaxDist = 64;

        public static bool MuteSound = false;
        public static bool MediaPlayerError = false;
        
        static float stackVolume = SoundStandardVolume;

        public const float MusicStandardVol = 0.16f;
        public const float SoundStandardVolume = 1f;

        static float musicVol = MusicStandardVol;
        public static float SoundVolume = SoundStandardVolume;
        public static float MusicVolume
        {
            get { return musicVol; }
            set
            {
                musicVol = value;
            }
        }

        static List<SoundEffectInstance> loopedSounds = new List<SoundEffectInstance>();

        public static SoundEffect PlaySound(LoadedSound whichSound, float volume, Pan pan, float pitch)
        {
            SoundEffect fx = LoadContent.Sound(whichSound);
            fx.Play(Bound.Max(volume * SoundVolume, 1), Bound.Set(pitch, -1, 1), pan.Value);
            return fx;
        }
        public static SoundEffect PlaySound(LoadedSound whichSound, float volume)
        {
            SoundEffect fx = LoadContent.Sound(whichSound);
            fx.Play(volume * SoundVolume, 0, 0);
            return fx;
            //return null;
        }

        public static SoundEffect PlaySound(SoundEffect fx, float volume, Pan pan, float pitch)
        {
            fx.Play(Bound.Max(volume * SoundVolume, 1), Bound.Set(pitch, -1, 1), pan.Value);
            return fx;
        }
        public static SoundEffect PlaySound(SoundEffect fx, float volume)
        {
            fx.Play(volume * SoundVolume, 0, 0);
            return fx;
            //return null;
        }

        //public static SoundEffectInstance PlayLoopedSound(LoadedSound whichSound, float vol)
        //{

        //    SoundEffect fx = LoadContent.Sound(whichSound);
        //    SoundEffectInstance instance = fx.CreateInstance();
        //    instance.Volume = vol;
        //    instance.IsLooped = true;
        //    instance.Play();

        //    return instance;
        //}

        //public static SoundEffectInstance PlayLoopedSound(SoundEffect fx, float vol)
        //{
        //    SoundEffectInstance instance = fx.CreateInstance();
        //    instance.Volume = vol;
        //    instance.IsLooped = true;
        //    instance.Play();

        //    return instance;
        //}
        public static void SetInstancePan(SoundEffectInstance ins, Vector2 screenPosition)
        {
            ins.Pan = Pan.PositionToPan(screenPosition.X, 0, Engine.Screen.Width).Value;
        }

        public static void PlayInstance(SoundEffectInstance fx, float volume, Pan pan, bool looped = false, float pitch = 0)
        {
            fx.IsLooped = looped;
            fx.Volume = SoundVolume * volume;
            fx.Pitch = pitch;
            fx.Pan = pan.Value;

            if (looped)
            {
                loopedSounds.Add(fx);
            }

            fx.Play();
        }

        public static void StopInstace(SoundEffectInstance sound)
        {
            sound.Stop();
            if (sound.IsLooped)
            {
                loopedSounds.Remove(sound);
            }
        }

        public static void StopAllLoopedSounds()
        {
            foreach (SoundEffectInstance instance in loopedSounds)
            {
                instance.Stop();
            }
            loopedSounds.Clear();            
        }

        public static void PauseAllLoopedSounds(bool pause)
        {
            if (pause)
            {
                foreach (SoundEffectInstance instance in loopedSounds)
                {
                    instance.Pause();
                }
            }
            else
            {
                foreach (SoundEffectInstance instance in loopedSounds)
                {
                    instance.Resume();
                }
            }
        }

        public static float Volume
        {
            get { return musicVol; }
            set
            {
                musicVol = value;
                MediaPlayer.Volume = musicVol;
            }
        }

        public static void PlayStereoSound(LoadedSound whichSound, float volume, Vector2 pos)
        {
            if (!MuteSound)
            {
                pos.X -= Ref.draw.Camera.LookTarget.X;
                pos.Y -= Ref.draw.Camera.LookTarget.Z;

                float distance = pos.Length();
                if (distance < SterioMaxDist)
                {
                    volume = volume * (1 - (SterioUnitReduction * distance));
                    volume *= stackVolume;

                    if (volume > 0)
                    {
                        volume *= stackVolume;

                        SoundEffect fx = LoadContent.Sound(whichSound);
                        fx.Play(volume, 0,
                            Pan.PositionToPan(distance, -SterioMaxDist, SterioMaxDist).Value);
                        stackVolume *= 0.6f;
                    }
                   
                }
            }
        }

        public static void Update()
        {
            stackVolume = SoundVolume;
        }
       
        public static void PlayPauseCustomMusic()
        {
            switch (MediaPlayer.State)
            {
                case MediaState.Paused:
                    MediaPlayer.Resume();
                    break;
                case MediaState.Playing:
                    MediaPlayer.Pause();
                    break;
                case MediaState.Stopped:
                    Microsoft.Xna.Framework.Media.MediaLibrary library = new MediaLibrary();
                    SongCollection songs = library.Songs;
                    MediaPlayer.IsShuffled = true;
                    if (songs.Count > 0)
                    {
                        MediaPlayer.Play(songs);
                    }
                    break;
            }
        }

        public static void PlayPauseMusic()
        {
            switch (MediaPlayer.State)
            {
                case MediaState.Paused:
                    MediaPlayer.Resume();
                    break;
                case MediaState.Playing:
                    MediaPlayer.Pause();
                    break;
            }
        }

        public static void CustomMusicNext(bool forward)
        {
            if (MediaPlayer.State != MediaState.Playing)
            {
                PlayPauseCustomMusic();
            }
            //must be sure we have any songs to play
            if (MediaPlayer.State == MediaState.Playing)
            {
                if (forward)
                {
                    MediaPlayer.MoveNext();
                }
                else
                {
                    MediaPlayer.MovePrevious();
                }
                
            }
        }
        //public static int PlayMusic(LoadedSong whichSong, bool loop)
        //{
        //    currentSong = LoadContent.Music(whichSong);
        //    return PlayMusic(currentSong, loop);
        //}

        public static bool IsPlaying
        {
            get { return Microsoft.Xna.Framework.Media.MediaPlayer.State == Microsoft.Xna.Framework.Media.MediaState.Playing; }
        }

        public static int PlayMusic(Song s, bool loop)
        {
            if (s != null)
            {
                try
                {
                    MediaPlayer.Stop();
                    currentSong = s;
                    MediaPlayer.Play(s);
                    MediaPlayer.IsRepeating = loop;
                    return (int)s.Duration.TotalMilliseconds;
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                    MediaPlayerError = true;
                }
            }
            return 0;
        }
        public static void StopMusic()
        {
            MediaPlayer.Stop();
        }
    }
}