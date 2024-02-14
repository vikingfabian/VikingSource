using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace VikingEngine.PJ
{
    static class SoundManager
    {
        public static SoundLevel SoundLevel = PJ.SoundLevel.Medium;
        public static SoundLevel MusicLevel = PJ.SoundLevel.Medium;

        public static readonly float[] LevelVolume = new float[]
        {
            1.4f,//High,
            0.7f,//Medium,
            0.35f,//Low,
            0f,//Off,
        };

        static readonly IntervalF EnemySoundFreq = new IntervalF(6, 16);
        public static float EnemySoundTimer = 0;

        public static void PlaySound(LoadedSound sound)
        {
            PlaySound(sound, Pan.Center);
        }

        public static void NextMusicLevel()
        {
            if (MusicLevel == PJ.SoundLevel.Off)
            {
                MusicLevel = PJ.SoundLevel.Medium;
                if (MediaPlayer.State == MediaState.Paused)
                    MediaPlayer.Resume();
            }
            else
            {
                MusicLevel = PJ.SoundLevel.Off;
                if (MediaPlayer.State == MediaState.Playing)
                    MediaPlayer.Pause();
            }
            //Engine.Sound.PlayPauseMusic();
            //MusicLevel++;
            //if (MusicLevel >= PJ.SoundLevel.NUM)
            //{
            //    MusicLevel = 0;
            //}
        }

        public static void PlaySound(LoadedSound sound, Pan pan)
        {
            float volume = 1;
            int variations = 1;
            float randomPitch = 0;
            float pitchAdd = 0;

            switch (sound)
            {
                case LoadedSound.NON:
                    return;
                case LoadedSound.MenuMove:
                    volume = 0.08f;
                    randomPitch = 0.02f;
                    break;
                case LoadedSound.MenuSelect:
                    volume = 0.07f;
                    break;
                case LoadedSound.Coin1:
                    variations = 3;
                    volume = 0.5f;
                    break;

                case LoadedSound.shieldcrash:
                    volume = 3f;
                    break;
                case LoadedSound.smack:
                    volume = 1f;
                    break;
                case LoadedSound.SmackEchoes:
                    volume = 1.8f;
                    break;
                case LoadedSound.smallSmack:
                    sound = LoadedSound.smack;
                    volume = 0.5f;
                    pitchAdd = 0.2f;
                    break;
                case LoadedSound.flap:
                    volume = 0.4f;
                    randomPitch = 0.05f;
                    break;
                case LoadedSound.minefire:
                    volume = 0.25f;
                    randomPitch = 0.05f;
                    break;
                case LoadedSound.flowerfire:
                    volume = 1.6f;
                    randomPitch = 0.05f;
                    break;

                //case LoadedSound.barrel_explo:
                //    volume = 2f;
                //    break;
                case LoadedSound.birdToasty:
                    volume = 4f;
                    break;
            }
            Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), volume * LevelVolume[(int)SoundLevel], pan, Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }
    }

    enum SoundLevel
    {
        High,
        Medium,
        Low,
        Off,
        NUM
    }
}

