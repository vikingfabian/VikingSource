﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.LootFest.Players;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Resource;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        //
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public static readonly Color TitleColor_Action = Color.LightBlue;
        public static readonly Color TitleColor_Attack = Color.Red;
        public static readonly Color TitleColor_Name = Color.LightYellow;
        public static readonly Color TitleColor_TypeName = Color.LightGray;
        public static readonly Color TitleColor_Label = new Color(0, 128, 153);
        public static readonly Color TitleColor_Label_Dark = new Color(0, 63, 76);
        public static readonly Color AvailableColor = Color.LightGreen;
        public static readonly Color NotAvailableColor = Color.Red;

        public static readonly Color TextColor_Relation = Color.LightBlue;

        public static readonly Color OffStandardOrange = new Color(200, 128, 0);
        public static readonly Color InfoYellow_Dark = new Color(160, 128, 0);
        public static readonly Color InfoYellow_Light = new Color(255, 255, 0);
        public static readonly Color InfoYellow_BG = new Color(40, 32, 0);
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
            RbSettings.checkOn = SpriteName.warsCheckYes;
            RbSettings.checkOff = SpriteName.warsCheckNo;
            //RbSettings.tabSelected.BgColor = new Color(126, 56, 23);
            //RbSettings.tabSelected.Color = new Color(222, 156, 125);
            //RbSettings.tabNotSelected.BgColor = new Color(114, 73, 53);
            //RbSettings.tabNotSelected.Color = RbSettings.tabSelected.Color;
            RbSettings.tabSelected.BgColor = new Color(53, 158, 209);//new Color(121,110,233);
            RbSettings.tabSelected.Color = new Color(3, 0, 46);
            RbSettings.tabNotSelected.BgColor = new Color(36, 107, 142); //new Color(99,96,146);
            RbSettings.tabNotSelected.Color = RbSettings.tabSelected.Color;

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

        public static void ResourceCost(RichBoxContent content, ResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = ResourceLib.PayIcon(resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
                content.space(0.5f);
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                ResourceLib.Name(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add( new RichBoxText(text, ResourceCostColor(hasResource >= needResource)));
        }

        public static void ResourceCost(RichBoxContent content, ItemResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = ResourceLib.Icon( resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RichBoxImage(icon));
                content.space(0.5f);
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                LangLib.Item(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add(new RichBoxText(text, ResourceCostColor(hasResource >= needResource)));
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

        public static RichBoxText ItemCount(RichBoxContent content, string item, string count)
        {
            string text = string.Format(DssRef.lang.Language_ItemCountPresentation, item, count);
            return content.text(text);
        }

        public static Color ResourceCostColor(bool hasEnough)
        { 
            return hasEnough ? AvailableColor : NotAvailableColor;
        }

        public static SpriteName CheckImage(bool value)
        { 
            return value? SpriteName.warsCheckYes : SpriteName.warsCheckNo;
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
            var followFactionButton = new RichboxButton(new List<AbsRichBoxMember> { new RichBoxImage(followFaction ? SpriteName.WarsFollowFactionYes : SpriteName.WarsFollowFactionNo) },
                        action, new RbAction2Arg<bool, double>( player.followFactionTooltip, followFaction, currentFactionValue));//new RbAction2Arg<ItemResourceType, City>(faction.tradeFollowFactionClick, resource, city));
            if (!followFaction)
            {
                followFactionButton.overrideBgColor = OffStandardOrange;
            }
            content.Add(followFactionButton);
            content.space();
        }

        public static void InfoButton(RichBoxContent content, AbsRbAction enterAction)
        {
            var text = new RichBoxText(DssRef.lang.Info_ButtonIcon);
            text.overrideColor = InfoYellow_Light;

            var button = new RichboxButton(new List<AbsRichBoxMember> { 
                new RichBoxSpace(0.5f),
                text,
                new RichBoxSpace(0.5f),
            },
            null, enterAction, true, InfoYellow_Dark);
            content.Add(button);
        }

        public static void PerSecondInfo(Players.LocalPlayer player, RichBoxContent content, bool minuteAverage)
        {
            InfoButton(content, new RbAction1Arg<bool>(player.perSecondTooltip, minuteAverage));
        }

        public static void Description(RichBoxContent content, string description)
        {
            content.text("\"" + description + "\"").overrideColor = InfoYellow_Light;
        }

        public static void Label(RichBoxContent content, string text)
        {
            content.text(text + ":").overrideColor = TitleColor_Label;
        }

        public static void CloseButton(RichBoxContent content, AbsRbAction click)
        {
            RichBoxText x = new RichBoxText(DssRef.lang.Hud_EndSessionIcon);
            x.overrideColor = Color.White;

           var button = new RichboxButton(new List<AbsRichBoxMember>
                    { new RichBoxSpace(), x,new RichBoxSpace(), },
                    click);
            button.overrideBgColor = Color.DarkRed;

            content.Add(button);
        }

        public static RichBoxImage BulletPoint(RichBoxContent content)
        {
            var dot = new RichBoxImage(SpriteName.warsBulletPoint, 0.8f, 0f, 0.3f);
            //dot.color = Color.DarkGray;
            content.Add(dot);
            return dot;
        }
    }
}
