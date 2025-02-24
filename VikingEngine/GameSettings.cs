using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars;
using VikingEngine.HUD.RichBox;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;


namespace VikingEngine
{
    /// <summary>
    /// Setup for the game, will load at startup in Windows
    /// </summary>
    class GameSettings
    {
        const int Version = 16;
        const string FileName = "technicalsettings";
        const string FileEnd = ".set";

        DataStream.FilePath path = new DataStream.FilePath(null, FileName, FileEnd, true, true);

        public int ChunkLoadRadius = LootFest.Map.World.StandardOpenRadius;
        public ThreeOptions MapLoadingSpeed = ThreeOptions.Medium;
        public int FrameRate = 60;
        public int DetailLevel = 1;
        public bool AutoJoinToCoopLevel = true;
        public int VibrationLevel = 100;
        public const int MaxBlood = 400;
        public int Blood = 100;
        public float UiScale = 1f;
        public float reversedStereoValue = 1f;
        public bool dyslexiaFont = false;
        public Network.BannedPeers bannedPeers = new Network.BannedPeers();
        public bool graphicsHasChanged = false;
        public bool settingsHasChanged = false;
        public LanguageType language = LanguageType.NONE;
        public InputMap controllerMap;
        public InputMap keyboardMap;
        public bool ModelLightShaderEffect = true;

        float MasterVolume = 0.5f;
        float MusicMasterVolume = 1f;
        float SoundVolume = Engine.Sound.SoundStandardVolume;
        float AmbientVolume = Engine.Sound.SoundStandardVolume;

        public float SoundVol() { return SoundVolume * MasterVolume; }
        public float AmbientVol() { return AmbientVolume * MasterVolume; }
        public float MusicVol() { return MusicMasterVolume * MasterVolume; }


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
            w.Write(MusicMasterVolume);
            w.Write(SoundVolume);
            w.Write((byte)VibrationLevel);
            w.Write(UiScale);
            w.Write((byte)language);
            w.Write(dyslexiaFont);
            controllerMap.write(w);
            keyboardMap.write(w);

            bannedPeers.write(w);
            w.Write(ModelLightShaderEffect);

            w.Write(MasterVolume);
            w.Write(AmbientVolume);
            w.Write((byte)MapLoadingSpeed);
            w.Write(Blood);
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

            MusicMasterVolume = r.ReadSingle();
            SoundVolume = r.ReadSingle();
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
            if (version >= 15)
            {
                MasterVolume = r.ReadSingle();
                AmbientVolume = r.ReadSingle();

            }
            if (version >= 16)
            {
                MapLoadingSpeed = (ThreeOptions)r.ReadByte();
                Blood = r.ReadInt32();
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

            MusicMasterVolume = r.ReadSingle();
            SoundVolume = r.ReadSingle();

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
                string screenName = SaveLib.ReadString_safe(r);

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
                settingsHasChanged = true;
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
            if (set)
            {
                Engine.Screen.PcTargetResolution = res;
                settingsHasChanged = true;
            }
            return Engine.Screen.PcTargetResolution;
        }

        public GraphicsAdapter monitorProperty(bool set, GraphicsAdapter val)
        {
            if (set)
            {

                Screen.Monitor = val;
                Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
                settingsHasChanged= true;
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
                settingsHasChanged = true;
            }

            return ModelLightShaderEffect;
        }

        

        public void optionsMenu(GuiLayout layout)
        {
            soundOptions(layout);
            new GuiSectionSeparator(layout);
            graphicsOptions(layout);
        }
        public void optionsMenu(RichBoxContent content, RichMenu menu)
        {
            content.h2("Sound", HudLib.TitleColor_Head);
            volumeOptions(content);

            content.newParagraph();
            content.h2("Monitor settings", HudLib.TitleColor_Head);
            graphicsOptions(content, menu);

            content.newParagraph();
            content.h2("Graphics options", HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Model light effect") },
                modelLightProperty));
            DropDownBuilder mapLoadingDropDown = new DropDownBuilder("mapload");
            {
                for (ThreeOptions opt = 0; opt < ThreeOptions.NUM; opt++)
                {
                    mapLoadingDropDown.AddSubOption(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.ThreeOption(opt)) },
                        opt == MapLoadingSpeed, opt == ThreeOptions.Medium, new RbAction1Arg<ThreeOptions>((ThreeOptions value) =>
                        {
                            MapLoadingSpeed = value;
                            settingsHasChanged = true;
                        }, opt), null);
                }
                mapLoadingDropDown.Build(content, "Map loading speed", menu);
            }
        }
        public void quickOptionsMenu(GuiLayout layout)
        {
            volumeOptions(layout);
            fullScreenBox(layout);
        }


        public void soundOptions(GuiLayout layout)
        {
            volumeOptions(layout);
        }

        bool ReversedStereoProperty(int index, bool _set, bool value)
        {
            if (_set)
            {
                reversedStereoValue = value ? -1 : 1;
                settingsHasChanged = true;
            }
            return reversedStereoValue < 0;
        }

        void volumeOptions(GuiLayout layout)
        {
            if (Ref.music != null)
            {
                new GuiFloatSlider(SpriteName.MenuPixelIconMusicVol, Ref.langOpt.SoundOption_MusicVolume, musicVolProperty, new IntervalF(0, 4), false, layout);
            }
            new GuiFloatSlider(SpriteName.MenuPixelIconSoundVol, Ref.langOpt.SoundOption_SoundVolume, soundVolProperty, new IntervalF(0, 4), false, layout);

            new GuiCheckbox(DssRef.todoLang.ReversedSterio, null, ReversedStereoProperty, layout);
        }

        public void volumeOptions(RichBoxContent content)
        {
            content.newLine();
            content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
            content.space();
            content.Add(new RbText("Master Volume"));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), masterVolProperty, true));

            if (Ref.music != null)
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.MenuPixelIconMusicVol));
                content.space();
                content.Add(new RbText(Ref.langOpt.SoundOption_MusicVolume));
                content.space();
                content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), musicVolProperty, true));
            }

            content.newLine();
            content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
            content.space();
            content.Add(new RbText("Ambience Volume"));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), ambientVolProperty, true));

            content.newLine();
            content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
            content.space();
            content.Add(new RbText(Ref.langOpt.SoundOption_SoundVolume));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), soundVolProperty, true));
        }

        public void graphicsOptions(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();
            DropDownBuilder dropdown = new DropDownBuilder("resolution%");
            {
                foreach (var m in resoutionPercOptions)
                {
                    dropdown.AddOption(string.Format(Ref.langOpt.GraphicsOption_Resolution_PercentageOption, m),
                        Engine.Screen.UseRecordingPreset == RecordingPresets.NumNon &&
                        m == Engine.Screen.RenderScalePerc,
                        m == 100,
                        new RbAction1Arg<int>(setResolutionPercProperty, m), null);
                }

                dropdown.Build(content, Ref.langOpt.GraphicsOption_Resolution, menu);
            }

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.GraphicsOption_Fullscreen) }, Ref.gamesett.fullscreenProperty));

            if (!Screen.PcTargetFullScreen)
            {
                DropDownBuilder OversizeWidth = new DropDownBuilder("OversizeWidth");
                DropDownBuilder OversizeHeight = new DropDownBuilder("OversizeHeight");                

                OversizeWidth.AddOption(Ref.langOpt.GraphicsOption_Oversize_None, Engine.Screen.oversizeWidthPerc == 0, false,
                        new RbAction1Arg<int>(setOversizeWidthProperty, 0), null);
                OversizeHeight.AddOption(Ref.langOpt.GraphicsOption_Oversize_None, Engine.Screen.oversizeHeightPerc == 0, false,
                    new RbAction1Arg<int>(setOversizeHeightProperty, 0), null);

                int[] oversizes = new int[] { 150, 175, 200, 250, 300 };

                foreach (var ov in oversizes)
                {
                    OversizeWidth.AddOption(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeWidth, ov), ov == Engine.Screen.oversizeWidthPerc, false,
                        new RbAction1Arg<int>(setOversizeWidthProperty, ov), null);
                    OversizeHeight.AddOption(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeHeight, ov), ov == Engine.Screen.oversizeHeightPerc, false,
                        new RbAction1Arg<int>(setOversizeHeightProperty, ov), null);
                }

                OversizeWidth.Build(content, Ref.langOpt.GraphicsOption_OversizeWidth, menu);
                OversizeHeight.Build(content, Ref.langOpt.GraphicsOption_OversizeHeight, menu);
            }

            //new GuiTextButton(Ref.langOpt.GraphicsOption_RecordingPresets, null, new GuiAction1Arg<Gui>(recordingResolutionOptions, layout.gui), true, layout);

            DropDownBuilder RecordPreset = new DropDownBuilder("RecordPreset");
            {
                var monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
                for (RecordingPresets rp = 0; rp < RecordingPresets.NumNon; ++rp)
                {
                    IntVector2 sz = Engine.Screen.RecordingPresetsResolution(rp);
                    if (sz.Y > monitor.CurrentDisplayMode.Height)
                    {
                        //Too large for the screen
                        break;
                    }
                    else
                    {
                        string name = string.Format(Ref.langOpt.GraphicsOption_YoutubePreset, sz.Y);
                        RecordPreset.AddOption(name, rp == Engine.Screen.UseRecordingPreset, rp == RecordingPresets.YouTube1080p,
                            new RbAction1Arg<RecordingPresets>(setRecordingPreset, rp), null);
                    }
                }
                RecordPreset.Build(content, Ref.langOpt.GraphicsOption_RecordingPresets, menu);
            }

            content.newLine();
            //content.Add(new RbImage(SpriteName.LFIconLetter));
            content.Add(new RbText( Ref.langOpt.GraphicsOption_UiScale));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.5f, 2f, 0.1f), uiScaleProperty, true));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Apply") },
                new RbAction(Ref.gamestate.OnResolutionChange)));
            //new GuiFloatSlider(SpriteName.LFIconLetter, Ref.langOpt.GraphicsOption_UiScale, uiScaleProperty, new IntervalF(0.5f, 2f), false, layout);
        }

        void setOversizeWidthProperty(int value)
        {
            oversizeWidthProperty(true, value);
        }
        int oversizeWidthProperty(bool set, int value)
        {
            if (set)
            {
                Engine.Screen.oversizeWidthPerc = value;
                Screen.ApplyScreenSettings();

                graphicsHasChanged = true;
                settingsHasChanged = true;
            }
            return Engine.Screen.oversizeWidthPerc;
        }
        void setOversizeHeightProperty(int value)
        {
            oversizeHeightProperty(true, value);
        }
        int oversizeHeightProperty(bool set, int value)
        {
            if (set)
            {
                Engine.Screen.oversizeHeightPerc = value;
                Screen.ApplyScreenSettings();

                graphicsHasChanged = true;
                settingsHasChanged= true;
            }
            return Engine.Screen.oversizeHeightPerc;
        }

        public void setResolutionPercProperty(int res)
        {
            resolutionPercProperty(true, res);  
        }

        public int bloodProperty(bool set, int value)
        {
            if (set)
            {
                Blood = value;
                settingsHasChanged = true;
            }
            return Blood;
        }

        public int resolutionPercProperty(bool set, int res)
        {
            if (set)
            {
                Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
                Engine.Screen.RenderScalePerc = res;
                Engine.Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
                settingsHasChanged = true;
            }
            return Engine.Screen.RenderScalePerc;
        }

        public void graphicsOptions(GuiLayout layout)
        {
            //listMonitors(layout);

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
                        //Too large for the screen
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

        

        public float musicVolProperty(bool set, float value)
        {
            if (set)
            {
                MusicMasterVolume = value;
                Ref.music?.RefreshVolume();
                settingsHasChanged = true;
            }
            return MusicMasterVolume;
        }
        public float soundVolProperty(bool set, float value)
        {
            if (set)
            {
                SoundVolume = value;
                settingsHasChanged = true;
            }
            return SoundVolume;
        }
        public float masterVolProperty(bool set, float value)
        {
            if (set)
            {
                MasterVolume = value;
                settingsHasChanged = true;
            }
            return MasterVolume;
        }
        public float ambientVolProperty(bool set, float value)
        {
            if (set)
            {
                AmbientVolume = value;
                settingsHasChanged = true;
            }
            return AmbientVolume;
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
            if (set)
            {
                VibrationLevel = value;
                settingsHasChanged = true;
            }
            return VibrationLevel;
        }

        //string SongTitleProperty(bool set, string value)
        //{
        //    return "Playing: \n" + Ref.music.GetSongName();
        //}

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

    enum ThreeOptions
    { 
        Low,
        Medium,
        High,
        NUM
    }
}
