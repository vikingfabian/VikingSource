//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LootFest.Music
//{
//    static class MusicLib
//    {
//        public static VolumeLevel MusicLevel = VolumeLevel.Low;
//        public static VolumeLevel SoundLevel = VolumeLevel.Low;
//        public static bool MusicMute
//        { get { return MusicLevel == VolumeLevel.Off; } }

//        static readonly float[] musicLevels = new float[] { Engine.Sound.MusicStandardVol, 0.06f, 0 };
//        static readonly float[] soundLevels = new float[] { 0.3f, 0.1f, 0 };
        
        

//        //public static void ToMenu(HUD.File file)
//        //{
//        //    List<string> musikLevels = new List<string>{ "High", "Low", "Off" };

//        //    file.AddTextOptionList("music", (int)Players.Link.LinkMusikLevel, (int)MusicLevel, musikLevels);
//        //    file.AddTextOptionList("sound",(int)Players.Link.LinkSoundLevel, (int)SoundLevel, musikLevels);
//        //}
//        public static void ChangeLevel(int link, bool musik)
//        {
//            if (musik)
//            {
//                MusicLevel = (VolumeLevel)link;
//            }
//            else
//            {
//                SoundLevel = (VolumeLevel)link;

//            }
//            UpdateVolume();
//        }
//        public static void UpdateVolume()
//        {
//            if (Ref.music != null)
//            { Ref.music.SetVolume( musicLevels[(int)MusicLevel]); }

//            Engine.Sound.SoundVolume = soundLevels[(int)SoundLevel];

//        }

//        static string LevelToText(VolumeLevel level)
//        {
//            switch (level)
//            {
//                default:
//                    return "high";
//                case VolumeLevel.Low:
//                     return "low";
//                case VolumeLevel.Off:
//                    return "off";
//            }
//        }
//    }
//    enum VolumeLevel
//    {
//        High,
//        Low,
//        Off,
//        NUM
//    }
//}
