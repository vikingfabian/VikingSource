using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        public const string OneDecimalFormat = "{0:0.0}";
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public const ImageLayers CutContentLayer = ImageLayers.Lay2;
        public const ImageLayers CutSceneBgLayer = ImageLayers.Lay3;

        public const ImageLayers HeadDisplayContentLayer = ImageLayers.Lay6;
        public const ImageLayers HeadDisplayLayer = ImageLayers.Lay7;

        public const ImageLayers DiplomacyDisplayLayer = ImageLayers.Lay8;

        public static RichBoxSettings RbSettings;

        public static RichboxGuiSettings richboxGui;
        public static RichboxGuiSettings cutsceneGui;

        public static void Init()
        {
            const float TextToIconSz = 1.2f;

            RbSettings = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.Black, Color.CornflowerBlue),
            Engine.Screen.TextBreadHeight * TextToIconSz, 1.1f);
            RbSettings.head1.Font = LoadedFont.Bold;
            RbSettings.head1.Color = Color.LightGray;

            HeadDisplayWidth = Engine.Screen.IconSize * 6;
            HeadDisplayEdge = Engine.Screen.BorderWidth;

            richboxGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = HeadDisplayBgOpacity,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer= HeadDisplayContentLayer,
                bglayer = HeadDisplayLayer,
                RbSettings= RbSettings,
            };

            cutsceneGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = 0.5f,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer = CutContentLayer -2,
                bglayer = CutSceneBgLayer -2,
                RbSettings = RbSettings,
            };
        }

        public static void ResourceCost(RichBoxContent content, SpriteName icon, string name, int needResource, int hasResource)
        {
            string text = name + ": " + TextLib.Divition_Large(needResource, hasResource);
            //Color? color = null;
            //if (needResource > hasResource)
            //{
            //    color = Color.Red;
            //}
            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
            }
            content.Add( new RichBoxText(text, ResourceCostColor(hasResource >= needResource)));
        }

        public static Color ResourceCostColor(bool hasEnough)
        { 
            return hasEnough ? Color.LightGreen : Color.Red;
        }

        public static SpriteName CheckImage(bool value)
        { 
            return value? SpriteName.cmdHudCheckOn : SpriteName.cmdHudCheckOff;
        }
        
    }
}
