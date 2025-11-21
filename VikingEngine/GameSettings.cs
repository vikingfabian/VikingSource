using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Music;
using VikingEngine.PJ.Strategy;


namespace VikingEngine
{
    /// <summary>
    /// Setup for the game, will load at startup in Windows
    /// </summary>
    class GameSettings
    {
        public static FileCheck FileCheck;

        const int Version = 30;
        const string FileName = "technicalsettings";
        const string FileEnd = ".set";

        DataStream.FilePath path = new DataStream.FilePath(null, FileName, FileEnd, true, true);

        public int ChunkLoadRadius = LootFest.Map.World.StandardOpenRadius;
        public ThreeOptions MapLoadingSpeed = ThreeOptions.Medium;
        static readonly int[] FrameRateOptions = new int[] { 30, 60, 75, 100, 120, 144, 165, 240, 360 };
        public int FrameRate = 60;
        public int DetailLevel = 1;
        public bool AutoJoinToCoopLevel = true;
        public int VibrationLevel = 100;
        public const int MaxBlood = 100;
        public int Blood = 100;
        public float UiScale = 1f;
        public float IngameMenuWidth = 1f;
        public bool customCursor = false;
        public float reversedStereoValue = 1f;
        public bool dyslexiaFont = false;
        public Network.BannedPeers bannedPeers = new Network.BannedPeers();
        public bool graphicsHasChanged = false;
        public bool settingsHasChanged = false;
        public bool shaderHasChanged = false;
        public LanguageType language = LanguageType.NONE;
        public InputMap controllerMap;
        public InputMap keyboardMap;
        public bool ModelLightShaderEffect = true;
        public bool modelShadow = true;

        public bool farViewDistance = true;
        //public bool modelShadow_Soft = true;
        public bool waterFoam = true;
        public float modelBrightness = 1f;
        public ShadowResolution shadowResolution = ShadowResolution.Medium_2048;

        public bool ParticlesEffect = true;
        public bool panOnZoom = true;
        public int controlLayout = 0;

        public float scrollWheelSensitivity_menu = 1;
        public float scrollWheelSensitivity_game = 1;
        public float keyPanSpeed = 1f;


        float MasterVolume = 0.5f;
        float MusicMasterVolume = 0f;//1f;
        float SoundVolume = Engine.Sound.SoundStandardVolume;
        float AmbientVolume = Engine.Sound.SoundStandardVolume;
        float BattleMelodyVolume = 1f;
        bool lowLatencyGarbageCollecting = true;
        public float SoundVol() { return SoundVolume * MasterVolume; }
        public float AmbientVol() { return AmbientVolume * MasterVolume; }
        public float BattleMelodyVol() { return BattleMelodyVolume; }
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
            w.Write(Engine.Screen.WindowScalePerc);
            Engine.Screen.PcTargetResolution.write(w);
            w.Write((byte)Engine.Screen.PcDisplayMode);//Engine.Screen.PcTargetFullScreen);
            w.Write((byte)Engine.Screen.UseRecordingPreset);
            w.Write(MusicMasterVolume);
            w.Write(SoundVolume);
            w.Write((byte)VibrationLevel);
            w.Write(UiScale);
            w.Write(IngameMenuWidth);
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

            w.Write(panOnZoom);
            w.Write(controlLayout);
            w.Write(scrollWheelSensitivity_menu);
            w.Write(scrollWheelSensitivity_game);
            w.Write(keyPanSpeed);
            w.Write(BattleMelodyVolume);
            w.Write(ParticlesEffect);

            w.Write(lowLatencyGarbageCollecting);

            w.Write((byte)shadowResolution);
            w.Write(modelShadow);
            w.Write(waterFoam);
            w.Write(modelBrightness);

            w.Write(FrameRate);
            w.Write(farViewDistance);

            w.Write(customCursor);

            Debug.WriteCheck(w);
        }

        public void readEmbeddedSettingsAndVersion(System.IO.BinaryReader r)
        {
            int version = r.ReadInt32();
            readSettings(r, version);
        }

        public void readSettings(System.IO.BinaryReader r, int version)
        {
            if (version <= Version)
            {
                Engine.Screen.WindowScalePerc = r.ReadInt32();
                Engine.Screen.PcTargetResolution.read(r);
                //Engine.Screen.PcTargetFullScreen = r.ReadBoolean();
                if (version >= 28)
                {
                    Engine.Screen.PcDisplayMode = (WindowDisplayMode)r.ReadByte();
                }
                else
                {
                    var PcTargetFullScreen = r.ReadBoolean();
                }
                Engine.Screen.UseRecordingPreset = (Engine.RecordingPresets)r.ReadByte();

                MusicMasterVolume = r.ReadSingle();
                if (version == 23)
                {
                    MusicMasterVolume = 1f;
                }
                SoundVolume = r.ReadSingle();
                VibrationLevel = r.ReadByte();
                //Engine.Screen.WindowScalePerc = r.ReadInt32();
                //Engine.Screen.PcTargetResolution.read(r);
                
                //Engine.Screen.UseRecordingPreset = (Engine.RecordingPresets)r.ReadByte();

                UiScale = r.ReadSingle();
                if (UiScale < 0.5f)
                {
                    UiScale = 1f;
                }

                if (version >= 28)
                {
                    IngameMenuWidth = r.ReadSingle();
                }

                language = (LanguageType)r.ReadByte();

                dyslexiaFont = r.ReadBoolean();

                controllerMap.read(r);
                keyboardMap.read(r);


                bannedPeers.read(r, version);

                ModelLightShaderEffect = r.ReadBoolean();

                MasterVolume = r.ReadSingle();
                AmbientVolume = r.ReadSingle();

                MapLoadingSpeed = (ThreeOptions)r.ReadByte();
                Blood = r.ReadInt32();

                panOnZoom = r.ReadBoolean();
                controlLayout = r.ReadInt32();
                scrollWheelSensitivity_menu = r.ReadSingle();
                scrollWheelSensitivity_game = r.ReadSingle();
                if (version >= 22)
                {
                    keyPanSpeed = r.ReadSingle();
                }


                BattleMelodyVolume = r.ReadSingle();

                ParticlesEffect = r.ReadBoolean();


                if (version >= 23)
                {
                    lowLatencyGarbageCollecting = r.ReadBoolean();
                }


                if (version >= 25)
                {
                    shadowResolution = (ShadowResolution)r.ReadByte();
                    modelShadow = r.ReadBoolean();
                    waterFoam = r.ReadBoolean();
                    modelBrightness = r.ReadSingle();
                }

                if (version >= 26)
                {
                    FrameRate = r.ReadInt32();
                }

                if (version >= 27)
                {
                    farViewDistance = r.ReadBoolean();
                }

                if (version >= 29)
                {
                    customCursor = r.ReadBoolean();
                }

                Debug.ReadCheck(r);

                Engine.Update.SetFrameRate(FrameRate);
                setSoundLevelsOnError();
                //MusicMasterVolume = 0;
            }
        }

        public void setSoundLevelsOnError()
        {
            if (VikingEngine.Sound.SoundManager.SoundInitializeSuccess == false)
            {
                MasterVolume = 0;
                MusicMasterVolume = 0;
            }
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(Version);
            writeSettings(w);
        }

        public void read(System.IO.BinaryReader r)
        {
            FileCheck fileCheck = new FileCheck();
            try
            {
                int version = r.ReadInt32();
                fileCheck.start(version, Version);

                if (version >= 21)
                {
                    readSettings(r, version);
                }

                fileCheck.end();
            }
            catch (Exception e)
            {
                fileCheck.exception = e;
                new GameSettings();
                //Debug.LogError("Loading game settings error, " + e.Message);
            }

            FileCheck = fileCheck;
        }

//        public void oldread(System.IO.BinaryReader r, int version)
//        {
            

//            if (version >= 2)
//            {
//                Engine.Screen.WindowScalePerc = r.ReadInt32();
//            }
//            Engine.Screen.PcTargetResolution.read(r);
//            Engine.Screen.PcTargetFullScreen = r.ReadBoolean();

//            ChunkLoadRadius = r.ReadInt32();


//            int ReadFrameRate = r.ReadInt32();
//#if !PJ
//            FrameRate = ReadFrameRate;
//#endif

//            DetailLevel = r.ReadInt32();

//            MusicMasterVolume = r.ReadSingle();
//            SoundVolume = r.ReadSingle();

//            if (version >= 1)
//            {
//                AutoJoinToCoopLevel = r.ReadBoolean();
//            }
//            if (version >= 3)
//            {
//                Engine.Screen.UseRecordingPreset = (Engine.RecordingPresets)r.ReadByte();
//            }
//            if (version >= 4 && version < 9)
//            {
//                string screenName = StreamLib.ReadString_safe(r);
//            }
//            if (version >= 7)
//            {
//                bannedPeers.read(r, version);
//            }
//        }

        public bool farViewDistanceProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                if (farViewDistance != value)
                {
                    farViewDistance = value;
                    if (DssRef.state != null)
                    {
                        foreach (var p in DssRef.state.localPlayers)
                        {
                            p.mapLayersManager.refreshLayers();
                        }
                    }
                    settingsHasChanged = true;
                }
            }
            return farViewDistance;
        }
        //public bool fullscreenProperty(object tag, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        if (value)
        //        {
        //            Engine.Screen.WindowScalePerc = 100;
        //        }
        //        Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
        //        Engine.Screen.PcTargetFullScreen = value;
        //        Engine.Screen.ApplyScreenSettings();
        //        graphicsHasChanged = true;
        //        settingsHasChanged = true;
        //    }
        //    return Engine.Screen.PcTargetFullScreen;
        //}
        public bool CustomCursorProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                customCursor = value;
                Ref.draw.refreshCursor();
                settingsHasChanged = true;
            }
            return customCursor;
        }

        public bool AddSomePixelsProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                Engine.Screen.bRecordingPresetAddPixels = value;
                Engine.Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
                settingsHasChanged = true;
            }
            return Engine.Screen.bRecordingPresetAddPixels;
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

        public bool modelLightProperty(object tag, bool set, bool val)
        {
            if (set)
            {
                ModelLightShaderEffect = val;
#if DSS
                Graphics.EffectBasicVertexColor.Singleton.ObjectShader();
#endif
                settingsHasChanged = true;
            }

            return ModelLightShaderEffect;
        }

        public bool shadowProperty(object tag, bool set, bool val)
        {
            if (set)
            {
                modelShadow = val;
                settingsHasChanged = true;
            }

            return modelShadow;
        }

        public bool waterFoamProperty(object tag, bool set, bool val)
        {
            if (set)
            {
                waterFoam = val;
                settingsHasChanged = true;
            }

            return waterFoam;
        }

        public float brightnessProperty(bool set, float value)
        {
            if (set)
            {
                modelBrightness = value;
                settingsHasChanged = true;
            }
            return modelBrightness;
        }
        public bool particlesProperty(object tag, bool set, bool val)
        {
            if (set)
            {
                ParticlesEffect = val;
                settingsHasChanged = true;
            }

            return ParticlesEffect;
        }

        //public void optionsMenu(GuiLayout layout)
        //{
        //    soundOptions(layout);
        //    new GuiSectionSeparator(layout);
        //    graphicsOptions(layout);
        //}
        

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

            new GuiCheckbox(Ref.langOpt.ReversedStereo, null, ReversedStereoProperty, layout);
        }

        public void volumeOptions(RichBoxContent content)
        {
            if (VikingEngine.Sound.SoundManager.SoundInitializeSuccess)
            {

                content.newLine();
                content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
                content.space();
                content.Add(new RbText(DssRef.lang.Settings_MasterVolume));
                content.space();
                content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), masterVolProperty, true));

                if (Ref.music != null)
                {
                    //content.newLine();
                    //content.Add(new RbText(DssRef.lang.MusicIsBroken, HudLib.InfoYellow_Light));
                    content.newLine();
                    content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                    content.Add(new RbImage(SpriteName.MenuPixelIconMusicVol));
                    content.space();
                    content.Add(new RbText(Ref.langOpt.SoundOption_MusicVolume));
                    content.space();
                    content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), musicVolProperty, true));
                }

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
                content.space();
                content.Add(new RbText(DssRef.lang.Settings_AmbienceVolume));
                content.space();
                content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), ambientVolProperty, true));

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                content.Add(new RbImage(SpriteName.MenuPixelIconMusicVol));
                content.space();
                content.Add(new RbText(DssRef.lang.Settings_BattleMelody));
                content.space();
                content.Add(new RbDragButton(new DragButtonSettings(0, 2, 0.1f), BattleMelodyVolProperty, true));

                content.newLine();
                content.Add(new RbImage(SpriteName.WarsHudIconChildArrow));
                content.Add(new RbImage(SpriteName.MenuPixelIconSoundVol));
                content.space();
                content.Add(new RbText(Ref.langOpt.SoundOption_SoundVolume));
                content.space();
                content.Add(new RbDragButton(new DragButtonSettings(0, 4, 0.1f), soundVolProperty, true));
            }
            else
            {
                content.newLine();
                content.Add(new RbImage(SpriteName.cmdWarningTriangle));
                content.space();
                content.Add(new RbText(DssRef.lang.Error_SoundInitFailure, HudLib.InfoYellow_Light));
                content.space();
                content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("!") },
                        new RbAction(() => { BlueScreen.ThreadException = VikingEngine.Sound.SoundManager.SoundInitializeException; }),
                        new RbTooltip_Text(VikingEngine.Sound.SoundManager.SoundInitializeException.Message)));
            }
        }

        public void monitorOptions(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();
            DropDownBuilder dropdown = new DropDownBuilder("resolution%");
            {
                foreach (var m in resoutionPercOptions)
                {
                    dropdown.AddOption(string.Format(Ref.langOpt.GraphicsOption_Resolution_PercentageOption, m),
                        Engine.Screen.UseRecordingPreset == RecordingPresets.NumNon &&
                        m == Engine.Screen.WindowScalePerc,
                        m == 100,
                        new RbAction1Arg<int>(setResolutionPercProperty, m), null);
                }

                dropdown.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_Resolution, menu);
            }

            content.newLine();
            DropDownBuilder winmodeDropdown = new DropDownBuilder("windowMode");
            {
                addMode(WindowDisplayMode.Windowed, Ref.langOpt.DisplayMode_Windowed);
                addMode(WindowDisplayMode.BorderlessFullscreen, Ref.langOpt.DisplayMode_BorderlessFullscreen);
                addMode(WindowDisplayMode.HardwareFullscreen, Ref.langOpt.GraphicsOption_Fullscreen);

                void addMode(WindowDisplayMode mode, string caption)
                {
                    winmodeDropdown.AddOption(caption, mode == Screen.PcDisplayMode, mode == WindowDisplayMode.BorderlessFullscreen,
                        new RbAction1Arg<WindowDisplayMode>(SetDisplayMode, mode), null);
                }

                winmodeDropdown.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.DisplayMode, menu);
            }
                        
                //content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.GraphicsOption_Fullscreen) }, Ref.gamesett.fullscreenProperty));

            if (Screen.PcDisplayMode != WindowDisplayMode.HardwareFullscreen)//if (!Screen.PcTargetFullScreen)
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

                OversizeWidth.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeWidth, menu);
                OversizeHeight.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeHeight, menu);
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
                RecordPreset.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_RecordingPresets, menu);
            }

            if (Engine.Screen.UseRecordingPreset != RecordingPresets.NumNon)
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> {
                    new RbText(Ref.langOpt.GraphicsOption_RecordingPresets + ":", HudLib.TitleColor_Label_Dark),
                    new RbSpace(0.5f),
                    new RbText(string.Format( Ref.langOpt.GraphicsOption_RecordingPresets_AddXPixels, Screen.RecordingPresetAddPixelsCount))
                    }, AddSomePixelsProperty));
            }

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbImage(SpriteName.cmdPointer), new RbSpace(0.5f), new RbText(Ref.langOpt.GameSettings_RenderedMouseCursor) },
                CustomCursorProperty));

            content.newLine();
            //content.Add(new RbImage(SpriteName.LFIconLetter));
            content.Add(new RbText( Ref.langOpt.GraphicsOption_UiScale));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.5f, 2f, 0.1f), uiScaleProperty, true));
            content.space();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Apply) },
                new RbAction(Ref.gamestate.OnResolutionChange)));

            content.newLine();
            content.Add(new RbText(Ref.langOpt.GraphicsOption_IngameMenuWidth));
            content.space();
            content.Add(new RbDragButton(new DragButtonSettings(0.8f, 1.6f, 0.1f), IngameMenuWProperty, true));

            
            //new GuiFloatSlider(SpriteName.LFIconLetter, Ref.langOpt.GraphicsOption_UiScale, uiScaleProperty, new IntervalF(0.5f, 2f), false, layout);
        }

        public void SetDisplayMode(WindowDisplayMode mode)
        {
            if (mode != WindowDisplayMode.Windowed)
            {
                Engine.Screen.WindowScalePerc = 100;
            }
            Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
            Screen.PcDisplayMode = mode;
            Engine.Screen.ApplyScreenSettings();
            graphicsHasChanged = true;
            settingsHasChanged = true;

        }

public void graphicsOptions(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            content.newLine();
            
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.Settings_ModelShadow) },
                shadowProperty));
            content.space();
            content.Add(new RbImage(SpriteName.MenuIconPerformanceHot));

            if (modelShadow)
            {
                DropDownBuilder shadowMapSizeDropDown = new DropDownBuilder("shadow map sz");
                {
                    for (ShadowResolution resolution = 0; resolution < ShadowResolution.NUM; resolution++)
                    {
                        var dropOpt = shadowMapSizeDropDown.AddOption(ShadowProcessor.Resolution(resolution).ToString(), resolution == shadowResolution,
                            resolution == ShadowResolution.Medium_2048, new RbAction1Arg<ShadowResolution>((ShadowResolution res) =>
                            {
                                shadowResolution = res;
                                settingsHasChanged = true;
                                Ref.draw.OnShaderChange(ShaderChangeType.ShadowMap);
                                menu.CloseDropDown();
                            }, resolution), null);

                        switch (resolution)
                        {
                            case ShadowResolution.Low_1024:
                                dropOpt.iconAfter = SpriteName.MenuIconPerformanceCold;
                                break;
                            case ShadowResolution.VeryHigh_8192:
                                dropOpt.iconAfter = SpriteName.MenuIconPerformanceHot;
                                break;
                        }

                    }
                }
                shadowMapSizeDropDown.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.Settings_ModelShadowMapSize, menu);

                content.newLine();
                HudLib.Label(content, Ref.langOpt.Settings_Brightness); content.space();
                RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f }, new DragButtonSettings(0.2f, 2f, 0.1f),
                    brightnessProperty);
            }
            else
            {
                content.newLine();
                content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_ModelLight) },
                    modelLightProperty));
            }
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.Settings_ModelWaterFoam) },
                waterFoamProperty));

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_Particles) },
                particlesProperty));

            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText(Ref.langOpt.GraphicsOption_FarViewDistance) },
                farViewDistanceProperty));
            content.space();
            content.Add(new RbImage(SpriteName.MenuIconPerformanceHot));

            DropDownBuilder frameRateOptions = new DropDownBuilder("fps");
            {
                foreach (var fps in FrameRateOptions)
                {
                    var dropOpt = frameRateOptions.AddOption(fps.ToString(), fps == FrameRate, fps == 60,
                        new RbAction1Arg<int>((int fps) => { FrameRate = fps; Engine.Update.SetFrameRate(FrameRate); settingsHasChanged = true; menu.CloseDropDown(); }, fps), null);

                    if (fps == FrameRateOptions[0])
                    {
                        dropOpt.iconAfter = SpriteName.MenuIconPerformanceCold;
                    }
                    else if (fps == arraylib.Last(FrameRateOptions))
                    {
                        dropOpt.iconAfter = SpriteName.MenuIconPerformanceHot;
                    }
                }
                frameRateOptions.Build(content, SpriteName.NO_IMAGE, Ref.langOpt.Settings_FrameRate, menu);
            }

            DropDownBuilder mapLoadingDropDown = new DropDownBuilder("mapload");
            {
                for (ThreeOptions opt = 0; opt < ThreeOptions.NUM; opt++)
                {
                    var dropOpt = mapLoadingDropDown.AddOption(Ref.langOpt.ThreeOption(opt),
                        opt == MapLoadingSpeed, opt == ThreeOptions.Medium, new RbAction1Arg<ThreeOptions>((ThreeOptions value) =>
                        {
                            MapLoadingSpeed = value;
                            settingsHasChanged = true;
                            menu.CloseDropDown();

                            DssRef.state?.detailMap?.refreshLoadSpeed();
                        }, opt), null);

                    switch (opt)
                    {
                        case 0:
                            dropOpt.iconAfter = SpriteName.MenuIconPerformanceCold;
                            break;
                        case ThreeOptions.NUM - 1:
                            dropOpt.iconAfter = SpriteName.MenuIconPerformanceHot;
                            break;
                    }
                }
                mapLoadingDropDown.Build(content, SpriteName.NO_IMAGE, DssRef.lang.Settings_MapLoadSpeed, menu);
            }
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
            windowScaleProperty(true, res);  
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

        public int windowScaleProperty(bool set, int res)
        {
            if (set)
            {
                Engine.Screen.UseRecordingPreset = RecordingPresets.NumNon;
                Engine.Screen.WindowScalePerc = res;
                Engine.Screen.ApplyScreenSettings();
                graphicsHasChanged = true;
                settingsHasChanged = true;
            }
            return Engine.Screen.WindowScalePerc;
        }

        //public void graphicsOptions(GuiLayout layout)
        //{
        //    //listMonitors(layout);

        //    var resoutionPercOptions = Engine.Screen.ResoutionPercOptions();

        //    List<GuiOption<int>> optionsList = new List<GuiOption<int>>();
        //    foreach (var m in resoutionPercOptions)
        //    {
        //        optionsList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_Resolution_PercentageOption, m), m));
        //    }

        //    new GuiOptionsList<int>(SpriteName.MenuIconScreenResolution, Ref.langOpt.GraphicsOption_Resolution, optionsList, windowScaleProperty, layout);
        //    fullScreenBox(layout);//new GuiCheckbox("Fullscreen", null, Ref.pc_gamesett.fullscreenProperty, layout);

        //    if (Screen.PcDisplayMode != WindowDisplayMode.HardwareFullscreen)//!Screen.PcTargetFullScreen)
        //    {
        //        int[] oversizes = new int[] { 150, 175, 200, 250, 300 };
        //        List<GuiOption<int>> oversizeWidthList = new List<GuiOption<int>>();
        //        List<GuiOption<int>> oversizeHeightList = new List<GuiOption<int>>();
        //        oversizeWidthList.Add(new GuiOption<int>(Ref.langOpt.GraphicsOption_Oversize_None, 0));
        //        oversizeHeightList.Add(new GuiOption<int>(Ref.langOpt.GraphicsOption_Oversize_None, 0));
        //        foreach (var ov in oversizes)
        //        {
        //            oversizeWidthList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeWidth, ov), ov));
        //            oversizeHeightList.Add(new GuiOption<int>(string.Format(Ref.langOpt.GraphicsOption_PercentageOversizeHeight, ov), ov));
        //        }
        //        new GuiOptionsList<int>(SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeWidth, oversizeWidthList, oversizeWidthProperty, layout);
        //        new GuiOptionsList<int>(SpriteName.NO_IMAGE, Ref.langOpt.GraphicsOption_OversizeHeight, oversizeHeightList, oversizeHeightProperty, layout);

        //    }

        //    new GuiTextButton(Ref.langOpt.GraphicsOption_RecordingPresets, null, new GuiAction1Arg<Gui>(recordingResolutionOptions, layout.gui), true, layout);

        //    new GuiFloatSlider(SpriteName.LFIconLetter, Ref.langOpt.GraphicsOption_UiScale, uiScaleProperty, new IntervalF(0.5f, 2f), false, layout);
        //}

        void fullScreenBox(GuiLayout layout)
        {
            //new GuiCheckbox(Ref.langOpt.GraphicsOption_Fullscreen, null, Ref.gamesett.fullscreenProperty, layout);
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

        public float scrollMenuProperty(bool set, float value)
        {
            if (set)
            {
                scrollWheelSensitivity_menu = value;
                settingsHasChanged = true;
            }
            return scrollWheelSensitivity_menu;
        }
        public float scrollGameProperty(bool set, float value)
        {
            if (set)
            {
                scrollWheelSensitivity_game = value;
                settingsHasChanged = true;
            }
            return scrollWheelSensitivity_game;
        }
        public float panSpeedProperty(bool set, float value)
        {
            if (set)
            {
                keyPanSpeed = value;
                settingsHasChanged = true;
            }
            return keyPanSpeed;
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
                Ref.music?.RefreshVolume();
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

        public float BattleMelodyVolProperty(bool set, float value)
        {
            if (set)
            {
                BattleMelodyVolume = value;
                settingsHasChanged = true;
            }
            return BattleMelodyVolume;
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

        public float IngameMenuWProperty(bool set, float value)
        {
            if (set)
            {
                IngameMenuWidth = value;
                HudLib.Init();
                settingsHasChanged = true;
            }
            return IngameMenuWidth;
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

        public bool panOnZoomProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                panOnZoom = value;
                settingsHasChanged = true;
            }
            return panOnZoom;
        }

        public bool lowGCProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                lowLatencyGarbageCollecting = value;
                settingsHasChanged = true;

                Ref.gamestate.refreshGcLatency();
            }
            return lowLatencyGarbageCollecting;
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
        Turkish,
        Italian,
        Korean,
    }

    enum ThreeOptions
    { 
        Low,
        Medium,
        High,
        NUM
    }
}
