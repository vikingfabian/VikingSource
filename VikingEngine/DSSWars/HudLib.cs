using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.GameObject.Resource;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        //
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public static readonly Color OffStandardOrange = new Color(200, 128, 0);

        public const ImageLayers StoryContentLayer = ImageLayers.Lay1_Front;
        public const ImageLayers StoryBgLayer = ImageLayers.Lay1_Back;

        public const ImageLayers CutContentLayer = ImageLayers.Lay2;
        public const ImageLayers CutSceneBgLayer = ImageLayers.Lay3;

        public const ImageLayers GUILayer = ImageLayers.Lay4;

        public const ImageLayers HeadDisplayContentLayer = ImageLayers.Lay6;
        public const ImageLayers HeadDisplayLayer = ImageLayers.Lay7;

        public const ImageLayers DiplomacyDisplayLayer = ImageLayers.Lay8;

        public static RichBoxSettings RbSettings;
        public static RichBoxSettings RbOnGuiSettings;

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

            RbOnGuiSettings = RbSettings;
            RbOnGuiSettings.scaleUp(1.4f);

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

        public static void ResourceCost(RichBoxContent content, GameObject.Resource.ResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = GameObject.Resource.ResourceLib.PayIcon(resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                GameObject.Resource.ResourceLib.Name(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add( new RichBoxText(text, ResourceCostColor(hasResource >= needResource)));
        }

        public static void Upkeep(RichBoxContent content, double value)
        {
            string valuestring = TextLib.OneDecimal(value);
            content.icontext(SpriteName.rtsUpkeepTime, string.Format(DssRef.lang.Hud_Upkeep, valuestring));
        }
        public static void Upkeep(RichBoxContent content, int value)
        {
            string valuestring = TextLib.LargeNumber(value);
            content.icontext(SpriteName.rtsUpkeepTime, string.Format( DssRef.lang.Hud_Upkeep, valuestring));  
        }

        public static void ItemCount(RichBoxContent content, SpriteName icon, string item, string count)
        { 
            string text = string.Format( DssRef.lang.Language_ItemCountPresentation, item, count);
            content.icontext(icon, text);
        }

        public static Color ResourceCostColor(bool hasEnough)
        { 
            return hasEnough ? Color.LightGreen : Color.Red;
        }

        public static SpriteName CheckImage(bool value)
        { 
            return value? SpriteName.cmdHudCheckOn : SpriteName.cmdHudCheckOff;
        }

        public static string Date(DateTime date)
        { 
            return string.Format(DssRef.lang.Hud_Date, date.Year, date.Month, date.Day);
        }

        public static string TimeSpan(TimeSpan time) 
        { 
            return string.Format(DssRef.lang.Hud_TimeSpan, (int)time.TotalHours, time.Minutes, time.Seconds);
        }

        public static string InputName(InputSourceType input)
        {
            switch (input)
            {
                case InputSourceType.XController:
                    return DssRef.lang.Input_Source_Controller;
                case InputSourceType.KeyboardMouse:
                case InputSourceType.Keyboard:
                    return DssRef.lang.Input_Source_Keyboard;

                default:
                    return "-";
            }
        }
        public static void FollowFactionButton(bool followFaction, double currentFactionValue, AbsRbAction action, Players.LocalPlayer player, RichBoxContent content)
        {
            var followFactionButton = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxText(followFaction ? "=F" : "!F") },
                        action, new RbAction2Arg<bool, double>( player.followFactionTooltip, followFaction, currentFactionValue));//new RbAction2Arg<ItemResourceType, City>(faction.tradeFollowFactionClick, resource, city));
            if (!followFaction)
            {
                followFactionButton.overrideBgColor = OffStandardOrange;
            }
            content.Add(followFactionButton);
            content.space();
        }

        
    }
}
