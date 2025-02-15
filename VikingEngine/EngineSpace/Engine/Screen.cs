using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Engine
{
    static class Screen
    {
        /* Properties */
        public static VectorRect Area { get { return new VectorRect(Vector2.Zero, RenderingResolution.Vec); } }
        public static Vector2 CenterScreen { get { return new Vector2(RenderingResolution.X * PublicConstants.Half, RenderingResolution.Y * PublicConstants.Half); } }
        public static int Width { get { return RenderingResolution.X; } }
        public static int Height { get { return RenderingResolution.Y; } }

        public static int MinWidthHeight { get { return Math.Min(RenderingResolution.X, RenderingResolution.Y); } }
        public static bool PortraitOrientation { get { return RenderingResolution.Y > RenderingResolution.X; } }

        public static float IconSize;
        public static Vector2 IconSizeV2;
        public static float BorderWidth;
        public static float TextSize;
        public static Vector2 TextSizeV2;
        public static float SmallIconSize;
        public static Vector2 SmallIconSizeV2;

        public static float RegularFontSize = 1f;
        public static Vector2 TextIconFitSize;
        public static Vector2 TextSmallIconFitSize;
        public static Vector2 TextIconExactSize;
        public static Vector2 TextSmallIconExactSize;
        public static Vector2 TextBreadScale;
        public static Vector2 TextTitleScale;

        public static float TextBreadHeight;
        public static float TextTitleHeight;

        public static float TextSizeMultiplier { get { return textSizeMultiplier; } }

        /* Fields */
        public static GraphicsAdapter Monitor = Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter;
#if PCGAME
        //public static System.Windows.Forms.Screen FormScreen = System.Windows.Forms.Screen.PrimaryScreen;
#endif
        public static IntVector2 PcTargetResolution = new IntVector2(1280, 720);
        public static bool PcTargetFullScreen = false;
        public static int RenderScalePerc = 100;
        public static RecordingPresets UseRecordingPreset = RecordingPresets.NumNon;
        public static float RenderScaleF;

        public static VectorRect SafeArea;
        public static VectorRect MousePushEdge, MousePushEdgeMax;
        public static IntVector2 RenderingResolution;
        public static Vector2 ResolutionVec;
        public static IntVector2 MonitorTargetResolution;
        public static IntVector2 MonitorCenter;
        public static Rectangle MonitorTargetRect;

        public static bool ScreenIsReady = false;
        static float textSizeMultiplier = 1;

        public static int oversizeWidthPerc = 0;
        public static int oversizeHeightPerc = 0;

        /* Novelty methods */
        public static void ApplyScreenSettings()
        {

            Vector2 SafeBorderPerc;            

            SafeBorderPerc =
#if XBOX
                new Vector2(0.045f, 0.06f);
#else
                new Vector2(0.009f, 0.01f);// 0.04f;
#endif

            IntVector2 monitorResolution;

#if PCGAME
            monitorResolution = new IntVector2(Monitor.CurrentDisplayMode.Width, Monitor.CurrentDisplayMode.Height);
            //monitorResolution = new IntVector2(FormScreen.Bounds.Width, FormScreen.Bounds.Height);//monitor.CurrentDisplayMode.Width, monitor.CurrentDisplayMode.Height);

            if (UseRecordingPreset == RecordingPresets.NumNon)
            {

                double renderScaleW = RenderScalePerc / 100.0;
                double renderScaleH = renderScaleW;
                if (!PcTargetFullScreen)
                {
                    renderScaleH = Math.Min(renderScaleH, 0.94);
                    renderScaleW = Math.Min(renderScaleW, 0.99);
                }


                RenderingResolution.X = Convert.ToInt32(monitorResolution.X * renderScaleW);
                RenderingResolution.Y = Convert.ToInt32(monitorResolution.Y * renderScaleH);

                if (PcTargetFullScreen)
                {
                    MonitorTargetResolution = monitorResolution;
                    RenderScaleF = (float)renderScaleH;
                }
                else
                {
                    MonitorTargetResolution = RenderingResolution;
                    RenderScaleF = 1f;
                }

                applyBorderLessWindow(PcTargetFullScreen);
            }
            else
            { //Recording presets
                RenderScaleF = 1f;

                IntVector2 preSetResolution = RecordingPresetsResolution(UseRecordingPreset);
                bool fullScreen = monitorResolution.X <= preSetResolution.X || monitorResolution.Y <= preSetResolution.Y;

                if (fullScreen)
                {
                    MonitorTargetResolution = monitorResolution;
                }
                else
                {
                    MonitorTargetResolution = preSetResolution;
                }
                RenderingResolution = MonitorTargetResolution;

                applyBorderLessWindow(fullScreen);
            }

            if (!PcTargetFullScreen)
            {
                if (oversizeWidthPerc != 0)
                {
                    var perc = oversizeWidthPerc / 100.0;
                    MonitorTargetResolution.X = MathExt.MultiplyInt(perc, MonitorTargetResolution.X);
                    RenderingResolution.X = MathExt.MultiplyInt(perc, RenderingResolution.X);
                }
                if (oversizeHeightPerc != 0)
                {
                    var perc = oversizeHeightPerc / 100.0;
                    MonitorTargetResolution.Y = MathExt.MultiplyInt(perc, MonitorTargetResolution.Y);
                    RenderingResolution.Y = MathExt.MultiplyInt(perc, RenderingResolution.Y);
                }
            }
#else
            
            monitorResolution = new IntVector2(Monitor.CurrentDisplayMode.Width, Monitor.CurrentDisplayMode.Height);
            MonitorTargetResolution = monitorResolution;
            RenderingResolution = MonitorTargetResolution;

            RenderScaleF = 1f;
#endif

            MonitorTargetRect = new Rectangle(0, 0, MonitorTargetResolution.X, MonitorTargetResolution.Y);

            Engine.Draw.ApplyScreenResolution();
           
            SafeArea = new VectorRect(Vector2.Zero, RenderingResolution.Vec);

            Vector2 safeEdge = VectorExt.Round(RenderingResolution.Vec * SafeBorderPerc);
            SafeArea.AddXRadius(-safeEdge.X);
            SafeArea.AddYRadius(-safeEdge.Y);

            MousePushEdge = new VectorRect(Vector2.Zero, RenderingResolution.Vec);
            MousePushEdge.AddRadius(-2);
            MousePushEdgeMax = MousePushEdge;
            MousePushEdgeMax.AddRadius(10);

            ResolutionVec = RenderingResolution.Vec;
            MonitorCenter = MonitorTargetResolution / 2;
            
            RefreshUiSize();
            
            ScreenIsReady = true;
            if (Ref.gamestate != null) Ref.gamestate.OnResolutionChange();
        }

        public static void SetupSplitScreen(int numPlayers, bool horizontalSplit)
        {
            Engine.Draw.horizontalSplit = horizontalSplit;
            int screenIx = 0;
            for (int i = 0; i < numPlayers; ++i)
            {
                Engine.PlayerData p = Engine.XGuide.GetPlayer(i);

                p.IsActive = true;

                Engine.XGuide.GetPlayer(i).view.SetDrawArea(numPlayers, screenIx++, false, null);
            }
        }
    
        public static void RefreshUiSize()
        {
            IconSize = (float)Math.Round(MinWidthHeight * 0.05 * Ref.gamesett.UiScale);
            SmallIconSize = (int)(IconSize * 0.6f);
            IconSizeV2 = new Vector2(IconSize);
            SmallIconSizeV2 = new Vector2(SmallIconSize);
            BorderWidth = (int)(IconSize * 0.12f);

            TextSize = MinWidthHeight * 0.0006f * Ref.gamesett.UiScale;
            TextSizeV2 = new Vector2(TextSize);

            TextIconExactSize = new Vector2(IconSize / RegularFontSize);
            TextSmallIconExactSize = new Vector2(SmallIconSize / RegularFontSize);

            TextIconFitSize = TextIconExactSize * 0.8f;
            TextSmallIconFitSize = TextSmallIconExactSize * 0.8f;

            TextBreadHeight = MathExt.Round(SmallIconSize * 0.6f);
            TextTitleHeight = MathExt.Round(TextBreadHeight * 1.2f);

            TextBreadScale = Graphics.AbsText.HeightToScale(TextBreadHeight, LoadedFont.Regular);
            TextTitleScale = Graphics.AbsText.HeightToScale(TextTitleHeight, LoadedFont.Bold);
        }

        static void applyBorderLessWindow(bool fullScreen)
        {
            //Ref.main.Window.IsBorderless = fullScreen;

            Engine.Draw.graphicsDeviceManager.HardwareModeSwitch = false;
            Engine.Draw.graphicsDeviceManager.IsFullScreen = fullScreen;
            
            if (!fullScreen)
            {
#if PCGAME
                //var loc = FormScreen.WorkingArea.Location;

                IntVector2 center = new IntVector2(
                    (Monitor.CurrentDisplayMode.Width - MonitorTargetResolution.X) / 2, 
                    (Monitor.CurrentDisplayMode.Height - MonitorTargetResolution.Y) / 2);
                Ref.main.Window.Position = new Point(center.X, center.Y);
#endif
            }
        }

        public static List<IntVector2> SupportedResolutions()
        {
            var resolutions = new List<IntVector2>();
            
            foreach (DisplayMode mode in Monitor.SupportedDisplayModes)
            {
                //mode.whatever (and use any of avaliable information)
                IntVector2 res = new IntVector2(mode.Width, mode.Height);
                if (!resolutions.Contains(res))
                {
                    resolutions.Add(res);
                }
            }

            return resolutions;
        }

        public static List<int> ResoutionPercOptions()
        {
            var result = new List<int>
            {
                100,
                67,
                50,
            };

            if (Monitor.CurrentDisplayMode.Height > 2000)
            {
                result.Add(25);
            }

            return result;
        }

        public static IntVector2 RecordingPresetsResolution(RecordingPresets type)
        {
            switch (type)
            {
                case RecordingPresets.YouTube720p:
                    return new IntVector2(1280, 720);
                default://case RecordingPresets.YouTube1080p:
                    return new IntVector2(1920,1080);
                case RecordingPresets.YouTube1440p:
                    return new IntVector2(2560,1440);
                case RecordingPresets.YouTube2160p:
                    return new IntVector2(3840,2160);
                //default:
                //    throw new NotImplementedException();
            }
        }

        public static int MonitorCount
        {
            get
            {
//#if PCGAME
//                return System.Windows.Forms.Screen.AllScreens.Length;
//#else
                return 1;
//#endif
            }
        }

        public static int OnMonitorIndex
        {
            get
            {
//#if PCGAME
//                return arraylib.IndexFromValue(System.Windows.Forms.Screen.AllScreens, Engine.Screen.FormScreen);
//#else
                return 0;
//#endif
            }

        }

        public static bool IsHighDefinition()
        {
            return RenderingResolution.X >= 1200;
        }
    }

    enum RecordingPresets
    {
        YouTube720p,
        YouTube1080p,
        YouTube1440p,
        YouTube2160p,
        NumNon,
    }
}
