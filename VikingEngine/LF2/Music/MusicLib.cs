using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Music
{
    static class MusicLib
    {
        public static VolumeLevel MusicLevel = VolumeLevel.Low;
        public static VolumeLevel SoundLevel = VolumeLevel.Low;
        public static bool MusicMute
        { get { return MusicLevel == VolumeLevel.Off; } }

        static readonly float[] musicLevels = new float[] { Engine.Sound.MusicStandardVol, 0.06f, 0 };
        static readonly float[] soundLevels = new float[] { 0.3f, 0.1f, 0 };
        
        

        public static void ToMenu(HUD.File file)
        {
            List<string> musikLevels = new List<string>{ "High", "Low", "Off" };

            file.AddTextOptionList(LanguageManager.Wrapper.GameMenuMusicLevel(), (int)Players.Link.LinkMusikLevel, (int)MusicLevel, musikLevels);
            file.AddTextOptionList(LanguageManager.Wrapper.GameMenuSoundLevel(),(int)Players.Link.LinkSoundLevel, (int)SoundLevel, musikLevels);
        }
        public static void ChangeLevel(int link, bool musik)
        {
            if (musik)
            {
                MusicLevel = (VolumeLevel)link;
            }
            else
            {
                SoundLevel = (VolumeLevel)link;

            }
            UpdateVolume();
        }
        public static void UpdateVolume()
        {

            Engine.Sound.Volume = musicLevels[(int)MusicLevel];

            Engine.Sound.SoundVolume = soundLevels[(int)SoundLevel];

        }

        static string LevelToText(VolumeLevel level)
        {
            switch (level)
            {
                default:
                    return LanguageManager.Wrapper.GameMenuSoundHigh();
                case VolumeLevel.Low:
                     return LanguageManager.Wrapper.GameMenuSoundLow();
                case VolumeLevel.Off:
                    return LanguageManager.Wrapper.GameMenuSoundOff();
            }
        }
    }
    enum VolumeLevel
    {
        High,
        Low,
        Off,
        NUM
    }
}
