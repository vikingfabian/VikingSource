using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars;


namespace VikingEngine
{
    /// <summary>
    /// Setup for the game, will load at startup in Windows
    /// </summary>
    class GameSettings
    {
        const int Version = 13;
        const string FileName = "technicalsettings";
        const string FileEnd = ".set";

        DataStream.FilePath path = new DataStream.FilePath(null, FileName, FileEnd, true, true);

        public int ChunkLoadRadius = LootFest.Map.World.StandardOpenRadius;
        public int FrameRate = 60;
        public int DetailLevel = 1;
        public bool AutoJoinToCoopLevel = true;
        public int VibrationLevel = 100;
        public float UiScale = 1f;
        public bool dyslexiaFont = false;
        public Network.BannedPeers bannedPeers = new Network.BannedPeers();
        public bool graphicsHasChanged = false;
        public LanguageType language = LanguageType.NONE;
        public InputMap controllerMap;
        public InputMap keyboardMap;
        public bool ModelLightShaderEffect = false;

        public GameSettings()
        {
            controllerMap = new InputMap(false);
            controllerMap.setInputSource(Input.InputSourceType.XController, 0);
            keyboardMap = new InputMap(true);
            keyboardMap.setInputSource(Input.InputSourceType.KeyboardMouse, 0);
            Ref.gamesett = this;
        }

        public void Save()
        {
            DataStream.BeginReadWrite.BinaryIO(true, path, write, read, null, true);
        }
        public void Load()
        {
            DataStream.BeginReadWrite.BinaryIO(false, path, write, read, null, false);
        }

        public void writeEmbeddedSettingsAndVersion(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            writeSettings(w);
        }

        public void writeSettings(System.IO.BinaryWriter w)
        {
            w.Write(Engine.Screen.RenderScalePerc);
            Engine.Screen.PcTargetResolution.write(w);
            w.Write(Engine.Screen.PcTargetFullScreen);
            w.Write((byte)Engine.Screen.UseRecordingPreset);
            w.Write(Sound.MusicPlayer.MasterVolume);
            w.Write(Engine.Sound.SoundVolume);
            w.Write((byte)VibrationLevel);
            w.Write(UiScale);
            w.Write((byte)language);
            w.Write(dyslexiaFont);
            controllerMap.write(w);
            keyboardMap.write(w);

            bannedPeers.write(w);
            w.Write(ModelLightShaderEffect);
        }

        public void readEmbeddedSettingsAndVersion(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            readSettings(r, version);
        }

        public void readSettings(System.IO.BinaryReader r, int version)
        {
            Engine.Screen.RenderScalePerc = r.ReadInt32();
            Engine.Screen.PcTargetResolution.read(r);
            Engine.Screen.PcTargetFullScreen = r.ReadBoolean();
            Engine.Screen.UseRecordingPreset = (Engine.RecordingPresets)r.ReadByte();

            Sound.MusicPlayer.MasterVolume = r.ReadSingle();
            Engine.Sound.SoundVolume = r.ReadSingle();
            VibrationLevel = r.ReadByte();            
            
            UiScale = r.ReadSingle();
            if (UiScale < 0.5f)
            {
                UiScale = 1f;
            }
            if (version >= 11)
            { 
                language = (LanguageType)r.ReadByte(); 
            }
            dyslexiaFont = r.ReadBoolean();
            if (version >= 12)
            {
                controllerMap.read(r);
                keyboardMap.read(r);
            }

            bannedPeers.read(r, version);

            if (version >= 13)
            { 
                ModelLightShaderEffect = r.ReadBoolean();
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            writeSettings(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            try
            {
                int version = r.ReadInt32();

                if (version >= 8)
                {
                    readSettings(r, version);
                }
                else
                {
                    oldread(r, version);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Loading game settings error, " + e.Message);
            }
            
        }

        public void oldread(System.IO.BinaryReader r, int version)
        {
            if (version >= 2)
            {
                Engine.Screen.RenderScalePerc = r.ReadInt32();
            }
            Engine.Screen.PcTargetResolution.read(r);
            Engine.Screen.PcTargetFullScreen = r.ReadBoolean();

            ChunkLoadRadius = r.ReadInt32();


            int ReadFrameRate = r.ReadInt32();
#if !PJ
            FrameRate = ReadFrameRate;
#endif

            DetailLevel = r.ReadInt32();

            Sound.MusicPlayer.MasterVolume = r.ReadSingle();
            Engine.Sound.SoundVolume = r.ReadSingle();

            if (version >= 1)
            {
                AutoJoinToCoopLevel = r.ReadBoolean();
            }
            if (version >= 3)
            {
                Engine.Screen.UseRecordingPreset = (Engine.RecordingPresets)r.ReadByte();
            }
            if (version >= 4 && version < 9)
            {
                string screenName = SaveLib.ReadString(r);

//#if PCGAME
//                foreach (var m in System.Windows.Forms.Screen.AllScreens)
//                {
//                    if (m.DeviceName == screenName)
//                    {
//                        Engine.Screen.FormScreen = m;
//                        break;
//                    }
//                }
//#endif
            }
            if (version >= 7)
            {
                bannedPeers.read(r, version);
            }
        }

        public bool fullscreenProperty(int index, bool set, bool value)
        {
            if (set)
            {
                Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
                Engine.Screen.PcTargetFullScreen = value;
                Engine.Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
            }
            return Engine.Screen.PcTargetFullScreen;
        }

        public void setRecordingPreset(RecordingPresets rp)
        {
            Engine.Screen.UseRecordingPreset = rp;
            Screen.ApplyScreenSettings();
            graphicsHasChanged = true;
        }

        public IntVector2 resolutionProperty(bool set, IntVector2 res)
        {
            return GetSet.Do<IntVector2>(set, ref Engine.Screen.PcTargetResolution, res);
        }

        public GraphicsAdapter monitorProperty(bool set, GraphicsAdapter val)
        {
            if (set)
            {

                Screen.Monitor = val;
                Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
            }

            return Screen.Monitor;
        }

        public bool modelLightProperty(int index, bool set, bool val)
        {
            if (set)
            {
                ModelLightShaderEffect = val;
#if DSS
                //Graphics.EffectBasicVertexColor.Singleton = null;
                Graphics.EffectBasicVertexColor.Singleton.ObjectShader();
#endif
            }

            return ModelLightShaderEffect;
        }

        public int resolutionPercProperty(bool set, int res)
        {
            if (set)
            {
                Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
                Engine.Screen.RenderScalePerc = res;
                Engine.Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
            }
            return Engine.Screen.RenderScalePerc;
        }

        public void optionsMenu(GuiLayout layout)
        {
            soundOptions(layout);
            new GuiSectionSeparator(layout);
            graphicsOptions(layout);
        }
        public void quickOptionsMenu(GuiLayout layout)
        {
            volumeOptions(layout);
            fullScreenBox(layout);
        }


        public void soundOptions(GuiLayout layout)
        {
            volumeOptions(layout);
            //if (Ref.music != null)
            //{
            //    if (Ref.music.hasMusicQue())
            //    {
            //        new GuiTextButton("Next song", null, Ref.music.nextRandomSong, false, layout);
            //    }
            //    //if (Ref.music.IsPlaying())
            //    //{
            //    //    new GuiDelegateLabel(SongTitleProperty, layout);
            //    //}
            //}
        }

        void volumeOptions(GuiLayout layout)
        {
            if (Ref.music != null)
            {
                new GuiFloatSlider(SpriteName.MenuPixelIconMusicVol, Ref.langOpt.SoundOption_MusicVolume, musicVolProperty, new IntervalF(0, 4), false, layout);
            }
            new GuiFloatSlider(SpriteName.MenuPixelIconSoundVol, Ref.langOpt.SoundOption_SoundVolume, soundVolProperty, new IntervalF(0, 4), false, layout);
        }

        public void graphicsOptions(GuiLayout layout)
        {
            listMonitors(layout);

            var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();

            List<GuiOption<int>> optionsList = new List<GuiOption<int>>();
            foreach (var m in resoutionPercOptions)
            {
                optionsList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_Resolution_PercentageOption, m), m));
            }

            new GuiOptionsList<int>(SpriteName.MenuIconScreenResolution, Ref.langOpt.GraphicsOption_Resolution, optionsList, resolutionPercProperty, layout);
            fullScreenBox(layout);//new GuiCheckbox("Fullscreen", null, Ref.pc_gamesett.fullscreenProperty, layout);

            if (!Screen.PcTargetFullScreen)
            {
                int[] oversizes = new int[] { 150, 175, 200, 250, 300 };
                List<GuiOption<int>> oversizeWidthList = new List<GuiOption<int>>();
                List<GuiOption<int>> oversizeHeightList = new List<GuiOption<int>>();
                oversizeWidthList.Add(new GuiOption<int>(Ref.langOpt.GraphicsOption_Oversize_None, 0));
                oversizeHeightList.Add(new GuiOption<int>(Ref.langOpt.GraphicsOption_Oversize_None, 0));
                foreach (var ov in oversizes)
                {
                    oversizeWidthList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeWidth, ov), ov));
                    oversizeHeightList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeHeight, ov), ov));
                }
                new GuiOptionsList<int>(SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeWidth, oversizeWidthList, oversizeWidthProperty, layout);
                new GuiOptionsList<int>(SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeHeight, oversizeHeightList, oversizeHeightProperty, layout);

            }

            new GuiTextButton(Ref.langOpt.GraphicsOption_RecordingPresets, null, new GuiAction1Arg<Gui>(recordingResolutionOptions, layout.gui), true, layout);

            new GuiFloatSlider(SpriteName.LFIconLetter, Ref.langOpt.GraphicsOption_UiScale, uiScaleProperty, new IntervalF(0.5f, 2f), false, layout);
        }

        void fullScreenBox(GuiLayout layout)
        {
            new GuiCheckbox(Ref.langOpt.GraphicsOption_Fullscreen, null, Ref.gamesett.fullscreenProperty, layout);
        }

        void recordingResolutionOptions(Gui gui)
        {
            GuiLayout layout = new GuiLayout(Ref.langOpt.GraphicsOption_RecordingPresets, gui);
            {
                var monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
                for (RecordingPresets rp = 0; rp < RecordingPresets.NumNon; ++rp)
                {
                    IntVector2 sz = Engine.Screen.RecordingPresetsResolution(rp);
                    if (sz.Y > monitor.CurrentDisplayMode.Height)
                    {
                        break;
                    }
                    else
                    {
                        string name = string.Format(Ref.langOpt.GraphicsOption_YoutubePreset, sz.Y);

                        if (rp == Engine.Screen.UseRecordingPreset)
                        {
                            new GuiIconTextButton(SpriteName.LfCheckYes, name, null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                        else
                        {
                            new GuiTextButton(name, null,
                                new GuiAction1Arg<RecordingPresets>(Ref.gamesett.setRecordingPreset, rp), false, layout);
                        }
                    }
                }
            }
            layout.End();
        }

        public void listMonitors(GuiLayout layout)
        {
#if PCGAME
            //if (System.Windows.Forms.Screen.AllScreens.Length > 1)
            //{
            //    List<GuiOption<System.Windows.Forms.Screen>> options = new List<GuiOption<System.Windows.Forms.Screen>>();
            //    for (int i = 0; i < System.Windows.Forms.Screen.AllScreens.Length; ++i)
            //    {
            //        var screen = System.Windows.Forms.Screen.AllScreens[i];
            //        options.Add(new GuiOption<System.Windows.Forms.Screen>(
            //            (i + 1).ToString() + ": " + screen.Bounds.Width.ToString() + "x" + screen.Bounds.Height.ToString(), screen));
            //    }

            //    new GuiIconOptionsList<System.Windows.Forms.Screen>(SpriteName.MenuIconMultiMonitor, "M", options, monitorProperty2, layout);
            //}
#endif
        }

        int oversizeWidthProperty(bool set, int value)
        {
            if (set)
            {
                Engine.Screen.oversizeWidthPerc = value;
                Screen.ApplyScreenSettings();

                graphicsHasChanged = true;
            }
            return Engine.Screen.oversizeWidthPerc;
        }

        int oversizeHeightProperty(bool set, int value)
        {
            if (set)
            {
                Engine.Screen.oversizeHeightPerc = value;
                Screen.ApplyScreenSettings();

                graphicsHasChanged = true;
            }
            return Engine.Screen.oversizeHeightPerc;
        }

        public float musicVolProperty(bool set, float value)
        {
            if (set && Ref.music !=null) Ref.music.SetVolume(value);
            return Sound.MusicPlayer.MasterVolume;
        }
        public float soundVolProperty(bool set, float value)
        {
            return GetSet.Do<float>(set, ref Engine.Sound.SoundVolume, value);
        }

        public float uiScaleProperty(bool set, float value)
        {
            if (set)
            {
                UiScale = value;

                graphicsHasChanged = true;
                Screen.RefreshUiSize();
            }
            return UiScale;
        }

        public int vibrationProperty(bool set, int value)
        {
            return GetSet.Do<int>(set, ref VibrationLevel, value);
        }

        string SongTitleProperty(bool set, string value)
        {
            return "Playing: \n" + Ref.music.GetSongName();
        }

#if PCGAME
        //public System.Windows.Forms.Screen monitorProperty2(bool set, System.Windows.Forms.Screen val)
        //{
        //    if (set)
        //    {
        //        Screen.FormScreen = val;
        //        Screen.ApplyScreenSettings();
        //    }

        //    return Screen.FormScreen;
        //}


#endif
//        public void setMonitorIndex(int ix)
//        {
//#if PCGAME
//            Screen.FormScreen = System.Windows.Forms.Screen.AllScreens[ix];
//            Screen.ApplyScreenSettings();
//#endif
//        }
    }

    enum LanguageType
    {
        NONE = 0,
        English,
        Chinese,
        Russian,
        Spanish,
        Portuguese,
        German,
        Japanese,
        French,
    }
}
