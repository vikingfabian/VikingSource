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
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        public const float MenuEdgeSize = 8;
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public static readonly Color TitleColor_Head = new Color(104, 149, 219);
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
        public static readonly Color InfoYellow_Light = new Color(255, 255, 150);
        public static readonly Color InfoYellow_BG = new Color(40, 32, 0);
        public const ImageLayers StoryContentLayer = ImageLayers.Lay1_Front;
        public const ImageLayers StoryBgLayer = ImageLayers.Lay1_Back;

        public const ImageLayers CutContentLayer = ImageLayers.Lay2;
        public const ImageLayers CutSceneBgLayer = ImageLayers.Lay3;

        public const ImageLayers GUILayer = ImageLayers.Lay4;

        public const ImageLayers HeadDisplayContentLayer = ImageLayers.Lay6;
        public const ImageLayers HeadDisplayLayer = ImageLayers.Lay7;

        public const ImageLayers DiplomacyDisplayLayer = ImageLayers.Lay8;

        public static NineSplitSettings HudMenuBackground;
        public static NineSplitSettings HudMenuScollBackground;
        public static NineSplitSettings MessageBackground;
        public static float MessageDisplayWidth;

        public static NineSplitSettings HudMenuScollButton;
        public static RichBoxSettings RbSettings;
        public static RichBoxSettings RbSettings_Head;
        public static RichBoxSettings RbSettings_HeadOptions;
        public static RichBoxSettings RbOnGuiSettings;
        public static RichBoxSettings TooltipSettings;

        public static RichboxGuiSettings richboxGui;
        public static RichboxGuiSettings cutsceneGui;

        public static readonly Color MenuMoreOptionsArrowCol = new Color(131, 63, 17);

        public static void Init()
        {
            const float TextToIconSz = 1.2f;

            HudMenuBackground = new HUD.NineSplitSettings(SpriteName.WarsHudMenuBg, 1, 8, 1f, true, true);

            HudMenuScollBackground = new HUD.NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 8, 1f, true, true);
            HudMenuScollButton = new HUD.NineSplitSettings(SpriteName.WarsHudScrollerSlider, 1, 8, 1f, true, true);

            MessageBackground = new HUD.NineSplitSettings(SpriteName.WarsHudMessageBg, 1, 8, 1f, true, true);

            RbSettings = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.Black, Color.CornflowerBlue),
                Engine.Screen.TextBreadHeight * TextToIconSz, 1.1f);
            RbSettings.head1.Font = LoadedFont.Bold;
            RbSettings.head1.Color = Color.LightGray;
            RbSettings.checkOn = SpriteName.WarsHudCheckYes;
            RbSettings.checkOff = SpriteName.WarsHudCheckNo;
            RbSettings.optionOn = SpriteName.WarsHudOptionYes;
            RbSettings.optionOff = SpriteName.WarsHudOptionNo;

            RbSettings.tabSelected.BgColor = new Color(104, 149, 219);//new Color(121,110,233);
            RbSettings.tabSelected.Color = new Color(3, 0, 46);
            RbSettings.tabNotSelected.BgColor = new Color(36, 107, 142); //new Color(99,96,146);
            RbSettings.tabNotSelected.Color = RbSettings.tabSelected.Color;

            RbSettings.artPrimaryButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudPrimaryButton, 1, 8, 1f, true, true)
            {
                disableTexture = SpriteName.WarsHudPrimaryButtonDisabled
            };
            RbSettings.artSecondaryButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudSecondaryButton, 1, 8, 1f, true, true)
            {
                disableTexture = SpriteName.WarsHudSecondaryButtonDisabled
            };
            RbSettings.artOutlineButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudHeadBarOutlineButton, 1, 8, 1f, true, true);
            RbSettings.artHoverAreaTex = new HUD.NineSplitSettings(SpriteName.WarsHudHoverArea, 1, 8, 1f, true, true);

            RbSettings.dragButtonTex = new ThreeSplitSettings(SpriteName.WarsHudDragButton, 1, 15);

            RbSettings.artCheckButtonTex = new NineSplitSettings(SpriteName.WarsHudOptionSelected, 1, 8, 1f, true, true);
            RbSettings.artOptionButtonTex = new NineSplitSettings(SpriteName.WarsHudOptionSelected, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudOptionNotSelected,
            };

            RbSettings.artToggleButtonTex = new NineSplitSettings(SpriteName.WarsHudOptionSelected, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudToggleNotSelected,
            };

            RbSettings.artDropDownButtonTex = new NineSplitSettings(SpriteName.WarsHudRoundButton, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudRoundButtonNotSelected,
            };

            RbSettings.artTabTex = new NineSplitSettings(SpriteName.WarsHudTabSelected, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudTabNotSelected,
            };

            RbSettings.artSubTabTex = new NineSplitSettings(SpriteName.WarsHudSubTabSelected, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudSubTabNotSelected,
            };

            RbOnGuiSettings = RbSettings;
            RbOnGuiSettings.scaleUp(1.4f);

            TooltipSettings = RbSettings;
            TooltipSettings.windowBackground = new NineSplitSettings(SpriteName.cmdHudBorderTooltip, 1, 8, 1f, true, true);

            RbSettings_Head = RbSettings;
            RbSettings_Head.artOptionButtonTex = new NineSplitSettings(SpriteName.WarsHudHeadBarTabSelected, 1, 8, 1f, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudHeadBarTabNotSelected,
            };
            RbSettings_Head.artPrimaryButtonTex = new NineSplitSettings(SpriteName.WarsHudHeadBarButton, 1, 8, 1f, true, true);

            RbSettings_HeadOptions = RbSettings_Head;
            RbSettings_HeadOptions.artOptionButtonTex = RbSettings.artOptionButtonTex;

            HeadDisplayWidth = (int)( Engine.Screen.IconSize * 7);
            HeadDisplayEdge = Engine.Screen.BorderWidth;
            MessageDisplayWidth = (int)(Engine.Screen.IconSize * 6);

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
                content.Add(new RbImage(icon));
                content.space(0.5f);
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                ResourceLib.Name(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add( new RbText(text, ResourceCostColor(hasResource >= needResource)));
        }

        public static void ResourceCost(RichBoxContent content, ItemResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = ResourceLib.Icon( resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(icon));
                content.space(0.5f);
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                LangLib.Item(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add(new RbText(text, ResourceCostColor(hasResource >= needResource)));
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

        public static RbText ItemCount(RichBoxContent content, string item, string count)
        {
            string text = string.Format(DssRef.lang.Language_ItemCountPresentation, item, count);
            return content.text(text);
        }

        public static void Experience(RichBoxContent content, XP.WorkExperienceType exp, XP.ExperienceLevel level)
        {
            LangLib.ExperienceType(exp, out string expName, out SpriteName expIcon);
            content.Add(new RbImage(expIcon));
            content.space();
            var typeNameText = new RbText(expName + ":");
            typeNameText.overrideColor = HudLib.TitleColor_TypeName;
            content.Add(typeNameText);

            //var level = city.GetTopSkill(exp);
            content.space();
            content.Add(new RbImage(LangLib.ExperienceLevelIcon(level)));
            content.Add(new RbText(LangLib.ExperienceLevel(level)));
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

        public static string TimeSpan_LongText(TimeSpan time)
        {
            string result = string.Format(DssRef.lang.Hud_Time_Seconds, time.Seconds);
            if (time.TotalMinutes >= 1)
            {
                result = string.Format(DssRef.lang.Hud_Time_Minutes, time.Minutes) + ", " + result;
            }
            if (time.TotalHours >= 1)
            {
                result = string.Format(DssRef.todoLang.Hud_Time_Hours, (int)time.TotalHours) + ", " + result;
            }
            return result;
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
            SpriteName sprite;
            //RbButtonStyle buttonStyle;

            if (followFaction)
            {
                sprite = SpriteName.WarsFollowFactionYes;
                //buttonStyle = RbButtonStyle.OptionSelected;
            }
            else
            {
                sprite = SpriteName.WarsFollowFactionNo;
                //buttonStyle = RbButtonStyle.OptionNotSelected;
            }


            var followFactionButton = new ArtToggle(followFaction, new List<AbsRichBoxMember> { new RbImage(sprite) },
                        action, //new RbAction2Arg<bool, double>( player.followFactionTooltip, followFaction, currentFactionValue));
                        new RbTooltip(followFactionTooltip, new FollowFactionTooltipArgs() { follows = followFaction , currentFactionValue = currentFactionValue}));
            //if (!followFaction)
            //{
            //    followFactionButton.overrideBgColor = OffStandardOrange;
            //}
            content.Add(followFactionButton);
            content.space();


            
            void followFactionTooltip(RichBoxContent content, object tag)//bool follows, double currentFactionValue)
            {
                FollowFactionTooltipArgs args = (FollowFactionTooltipArgs)tag;

                content.h2(DssRef.lang.Hud_ToggleFollowFaction).overrideColor = HudLib.TitleColor_Action;
                content.newParagraph();

                string current;
                if (args.follows)
                {
                    current = DssRef.lang.Hud_FollowFaction_Yes;
                }
                else
                {
                    current = string.Format(DssRef.lang.Hud_FollowFaction_No, currentFactionValue);
                }
                content.text(current).overrideColor = HudLib.InfoYellow_Light;

                //hud.tooltip.create(this, content, true);
            }

            
        }
        struct FollowFactionTooltipArgs
        {
            public bool follows; public double currentFactionValue;
        }

        public static void InfoButton(List<AbsRichBoxMember> content, AbsRbAction enterAction)
        {
            var text = new RbText(DssRef.lang.Info_ButtonIcon);
            text.overrideColor = InfoYellow_Light;

            var button = new ArtImageButton(new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudInfoIcon)
            },
            null, enterAction, true);
            content.Add(button);
        }

        public static void PerSecondInfo(Players.LocalPlayer player, RichBoxContent content, bool minuteAverage)
        {
            InfoButton(content, new RbTooltip(perSecondTooltip, minuteAverage));
        }
        static void perSecondTooltip(RichBoxContent content, object tag)//bool minuteAverage)
        {
            bool minuteAverage = (bool)tag;
            //RichBoxContent content = new RichBoxContent();
            content.text(DssRef.lang.Info_PerSecond);
            if (minuteAverage)
            {
                content.text(DssRef.lang.Info_MinuteAverage);
            }

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
            RbText x = new RbText(DssRef.lang.Hud_EndSessionIcon);
            x.overrideColor = Color.White;

           var button = new RbButton(new List<AbsRichBoxMember>
                    { new RbSpace(), x,new RbSpace(), },
                    click);
            button.overrideBgColor = Color.DarkRed;

            content.Add(button);
        }

        public static RbImage BulletPoint(RichBoxContent content)
        {
            var dot = new RbImage(SpriteName.warsBulletPoint, 0.8f, 0f, 0.3f);
            //dot.color = Color.DarkGray;
            content.Add(dot);
            return dot;
        }

        public static void CityResource(RichBoxContent content, City city, ItemResourceType type)
        {
            bool buffer = false;
            city.GetGroupedResource(type).toMenu(content, type, city.foodSafeGuardIsActive(type), ref buffer);
        }

        public static Color? NegativeRed(int value)
        {
            if (value < 0)
            {
                return Color.Red;
            }
            else
            {
                return null;
            }
        }
    }
}
