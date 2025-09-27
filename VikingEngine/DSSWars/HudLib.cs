using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Valve.Steamworks;
using VikingEngine.DataLib;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.XP;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;

namespace VikingEngine.DSSWars
{
    static class HudLib
    {
        public const SpriteName AvailableIcon = SpriteName.unitEmoteThumbUp;
        public const SpriteName NotAvailableIcon = SpriteName.unitEmoteThumbDown;


        public const float MenuEdgeSize = 8;
        public const float HeadDisplayBgOpacity = 0.9f;
        public static float HeadDisplayWidth, HeadDisplayEdge;

        public const float WarHudIcons_DefaultScale = 0.8f;

        public static readonly Color TitleColor_Head = new Color(104, 149, 219);
        public static readonly Color TitleColor_Head2 = ColorExt.ChangeBrighness(TitleColor_Head, -20);
        public static readonly Color TitleColor_Action = Color.LightBlue;
        public static readonly Color TitleColor_Attack = Color.Red;
        public static readonly Color TitleColor_Name = Color.LightYellow;
        public static readonly Color TitleColor_TypeName = Color.LightGray;
        public static readonly Color TitleColor_TypeName_Dark = new Color(50, 50, 50);
        public static readonly Color TitleColor_Label = new Color(0, 128, 153);
        public static readonly Color TitleColor_Label_Dark = new Color(0, 63, 76);
        public static readonly Color AvailableColor = Color.LightGreen;
        public static readonly Color AvailableColor_Dark = Color.DarkGreen;
        //     Salmon color (R:250,G:128,B:114,A:255).
        public static readonly Color NotAvailableColor = new Color(250, 180, 180);
        public static readonly Color NotAvailableColor_Dark = Color.DarkRed;

        public static readonly Color TextColor_Relation = Color.LightBlue;

        public static readonly Color SecondaryTextColor = new Color(95, 105, 130);//new Color(66, 77, 81);
        public static readonly Color SubOptionTextColor = new Color(20, 20, 30);

        public static readonly Color OffStandardOrange = new Color(200, 128, 0);
        public static readonly Color InfoYellow_Dark = new Color(160, 128, 0);
        public static readonly Color InfoYellow_Light = new Color(255, 255, 150);
        public static readonly Color InfoYellow_BG = new Color(40, 32, 0);
        public const ImageLayers StoryContentLayer = ImageLayers.Lay1_Front;
        public const ImageLayers StoryBgLayer = ImageLayers.Lay1_Back;

        public const ImageLayers CutContentLayer = ImageLayers.Lay1;
        public const ImageLayers CutSceneBgLayer = ImageLayers.Lay2;

        public const ImageLayers MapToolTipLayer = ImageLayers.Lay3;

        public const ImageLayers GUILayer = ImageLayers.Lay4;

        public static readonly Color GodPower_ColorBg = new Color(80, 0, 80);//(R:139,G:0,B:139,A:255)
        public static readonly Color GodPower_Color = Color.LightPink;



        public const ImageLayers PopMenuLayer = ImageLayers.Lay5_Back;

        public const ImageLayers HeadDisplayContentLayer = ImageLayers.Lay6;
        public const ImageLayers HeadDisplayLayer = ImageLayers.Lay7;

        public const ImageLayers DiplomacyDisplayLayer = ImageLayers.Lay8;
        public const ImageLayers IngameUiLayer = ImageLayers.Lay9;

        public static NineSplitSettings HudMenuBackground;
        public static NineSplitSettings HudTutorialBackground;
        public static NineSplitSettings HudMenuScollBackground;
        public static NineSplitSettings MessageBackground;
        public static float MessageDisplayWidth;

        public static NineSplitSettings HudMenuScollButton;
        public static RichBoxSettings RbSettings;
        public static RichBoxSettings RbSettingsLarge;
        public static RichBoxSettings RbSettings_Head;
        public static RichBoxSettings RbSettings_HeadOptions;
        public static RichBoxSettings RbOnGuiSettings;
        public static RichBoxSettings TooltipSettings;
        public static RichBoxSettings TutorialRbSettings;

        public static RichboxGuiSettings richboxGui;
        public static RichboxGuiSettings cutsceneGui;

        public static HUD.NineSplitSettings PopMenuButtonTexture;

        public static readonly Color MenuMoreOptionsArrowCol = new Color(131, 63, 17);

        public const string EngineVersionString = "VikingEngine ver: {0}";
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
            RbSettings.head2.Font = LoadedFont.Bold;
            RbSettings.head1.Color = Color.LightGray;
            RbSettings.checkOn = SpriteName.WarsHudCheckYes;
            RbSettings.checkOff = SpriteName.WarsHudCheckNo;
            RbSettings.optionOn = SpriteName.WarsHudOptionYes;
            RbSettings.optionOff = SpriteName.WarsHudOptionNo;

            RbSettings.tabSelected.BgColor = new Color(104, 149, 219);//new Color(121,110,233);
            RbSettings.tabSelected.Color = new Color(3, 0, 46);
            RbSettings.tabNotSelected.BgColor = new Color(36, 107, 142); //new Color(99,96,146);
            RbSettings.tabNotSelected.Color = RbSettings.tabSelected.Color;

            bool smallScreen = Engine.Screen.Height < 800;
            float nineTextureEdge = smallScreen ? 0.5f : 1f;

            RbSettings.artPrimaryButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudPrimaryButton, 1, 8, nineTextureEdge, true, true)
            {
                disableTexture = SpriteName.WarsHudPrimaryButtonDisabled
            };
            RbSettings.artSecondaryButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudSecondaryButton, 1, 8, nineTextureEdge, true, true)
            {
                disableTexture = SpriteName.WarsHudSecondaryButtonDisabled
            };
            RbSettings.artGodPowerButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudGodPowerButton, 1, 8, nineTextureEdge, true, true)
            {
                disableTexture = SpriteName.WarsHudPrimaryButtonDisabled
            };
            RbSettings.artOutlineButtonTex = new HUD.NineSplitSettings(SpriteName.WarsHudOutlineButton, 1, 8, 1f, true, true);
            RbSettings.artHoverAreaTex = new HUD.NineSplitSettings(SpriteName.WarsHudHoverArea, 1, 8, 1f, true, true);

            RbSettings.dragButtonTex = new ThreeSplitSettings(SpriteName.WarsHudDragButton, 1, 15);

            RbSettings.artCheckButtonTex = new NineSplitSettings(SpriteName.WarsHudRoundButton, 1, 8, nineTextureEdge, true, true);
            RbSettings.artOptionButtonTex = new NineSplitSettings(SpriteName.WarsHudRoundButton, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudRoundButtonNotSelected,
            };

            RbSettings.artToggleButtonTex = new NineSplitSettings(SpriteName.WarsHudRoundButton, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudRoundButtonNotSelected,
            };

            RbSettings.artDropDownButtonTex = new NineSplitSettings(SpriteName.WarsHudRoundButton, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudRoundButtonSecondary,
            };

            RbSettings.artTabTex = new NineSplitSettings(SpriteName.WarsHudTabSelected, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudTabNotSelected,
            };

            RbSettings.artSubTabTex = new NineSplitSettings(SpriteName.WarsHudSubTabSelected, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudSubTabNotSelected,
            };

            PopMenuButtonTexture = new HUD.NineSplitSettings(SpriteName.WarsHudPopUpButton, 1, 8, nineTextureEdge, true, true);

            TutorialRbSettings = RbSettings;
            TutorialRbSettings.breadText.Color = Color.Black;
            TutorialRbSettings.head2.Color = TitleColor_Label_Dark;
            TutorialRbSettings.head1.Color = TitleColor_Label_Dark;
            TutorialRbSettings.checkOn = SpriteName.LfCheckYes;
            TutorialRbSettings.checkOff = SpriteName.LfCheckNo;

            HudTutorialBackground = new HUD.NineSplitSettings(SpriteName.WarsHudTutorialBg, 1, 16, 1f, true, true);

            //RbSettingsLarge = RbSettings;
            //RbSettingsLarge.scaleUp(2f);

            RbOnGuiSettings = RbSettings;
            RbOnGuiSettings.scaleUp(1.4f);

            TooltipSettings = RbSettings;
            TooltipSettings.windowBackground = new NineSplitSettings(SpriteName.cmdHudBorderTooltip, 1, 8, 1f, true, true);

            RbSettings_Head = RbSettings;
            RbSettings_Head.artOptionButtonTex = new NineSplitSettings(SpriteName.WarsHudHeadBarTabSelected, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.WarsHudHeadBarTabNotSelected,
            };
            RbSettings_Head.artPrimaryButtonTex = new NineSplitSettings(SpriteName.WarsHudHeadBarButton, 1, 8, nineTextureEdge, true, true);

            RbSettings_HeadOptions = RbSettings_Head;
            RbSettings_HeadOptions.artOptionButtonTex = RbSettings.artOptionButtonTex;

            HeadDisplayWidth = (int)(Engine.Screen.IconSize * 7.4f);
            HeadDisplayEdge = Engine.Screen.BorderWidth;
            MessageDisplayWidth = (int)(Engine.Screen.IconSize * 6);

            richboxGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = HeadDisplayBgOpacity,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer = HeadDisplayContentLayer,
                bglayer = HeadDisplayLayer,
                RbSettings = RbSettings,
            };

            cutsceneGui = new RichboxGuiSettings()
            {
                bgCol = Color.Black,
                bgAlpha = 0.8f,
                edgeWidth = HeadDisplayEdge,
                width = HeadDisplayWidth,
                contentLayer = CutContentLayer - 2,
                bglayer = CutSceneBgLayer - 2,
                RbSettings = RbSettings,
            };
        }

        public static void copyPaste(RichBoxContent content, LocalPlayer player, AbsRbAction copy, AbsRbAction paste)
        {
            player.gameControls.input.Copy.ToRichContent(content);
            content.hspace();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconCopy, WarHudIcons_DefaultScale),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_CopySetup) 
                }, copy));

            content.space();
            player.gameControls.input.Paste.ToRichContent(content);
            content.hspace();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsHudIconPaste, WarHudIcons_DefaultScale),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_Paste)
                }, paste));
        }
        public static void buildingMenuTitle(RichBoxContent content, SpriteName icon, string caption, int id, int index, int buildingCount, Action closeAction, Action<int> nextAction)
        {
            content.Add(new RbBeginTitle(1));

            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconPrevious) }, new RbAction1Arg<int>(nextAction, -1, RbSoundType.Tab),
                new RbTooltip_Text(DssRef.lang.Hud_Previous), buildingCount > 1));
            content.Add(new RbImage(icon));
            content.space();
            content.Add(new RbText(caption, TitleColor_Head));
            content.space();
            content.Add(new RbText(index.ToString(), TitleColor_Name));
            content.space();
            CloseButton(content, new RbAction(closeAction, RbSoundType.Back));
            content.Add(new RbTab(0.8f));
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconNext) }, new RbAction1Arg<int>(nextAction, 1, RbSoundType.Tab),
                new RbTooltip_Text(DssRef.lang.Hud_Next), buildingCount > 1));
            content.newLine();
            content.Add(new RbText(id.ToString(), SecondaryTextColor));
        }

        public static void blueprintButton(City city, LocalPlayer player, RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null, bool roomForAnotherButton = false)
        {

            content.Add(new RbTab(0.65f));//roomForAnotherButton? 0.65f : 0.8f));

            var tooltip = new RbTooltip(blueprintTooltip, new BlueprintTooltipArgs()
            {
                blueprint = blueprint,
                optionalBp = optionalBp,
                city = city.myIndex,
            });

            if (blueprint == CraftResourceLib.Food1)
            {
                tooltip.tagId = Tooltip.Food_BlueprintId;
            }

            content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsBluePrint)
            },
            null, tooltip));

        }
        class BlueprintTooltipArgs
        {
            public CraftBlueprint blueprint;
            public CraftBlueprint optionalBp;
            public int city;
        }

        public static void blueprint(RichBoxContent content, CraftBlueprint blueprint, CraftBlueprint optionalBp = null)
        {

            content.Add(new ArtButton(RbButtonStyle.HoverArea, new List<AbsRichBoxMember> {
                    new RbImage(SpriteName.WarsBluePrint)
                    },
                    null, new RbTooltip(blueprintTooltip, new BlueprintTooltipArgs() { blueprint = blueprint, optionalBp = optionalBp }
                )));
            content.space();
        }
        static void blueprintTooltip(RichBoxContent content, object tag)
        {
            //hover
            BlueprintTooltipArgs args = (BlueprintTooltipArgs)tag;
            var city = DssRef.world.cities[args.city];
            //RichBoxContent content = new RichBoxContent();
            content.h2(DssRef.lang.Blueprint_Title, HudLib.TitleColor_Head);
            args.blueprint.toMenu(content, city);
            if (args.optionalBp != null)
            {
                content.newParagraph();
                args.optionalBp.toMenu(content, city);
            }

            args.blueprint.requirementToHud(content, city, out _);

            content.Add(new RbSeperationLine());
            content.newParagraph();
            content.h2(DssRef.lang.MenuTab_Resources).overrideColor = HudLib.TitleColor_Label;
            args.blueprint.listResources(content, city, args.optionalBp);

            if (args.blueprint.levelRequirement > ExperienceLevel.Beginner_1)
            {
                content.newLine();

                HudLib.Experience(content, args.blueprint.experienceType, city.GetTopSkill(args.blueprint.experienceType));
            }

            //player.hud.tooltip.create(player, content, true, blueprint.tooltipId);
        }

        public static void ResourceCost(RichBoxContent content, ResourceType resource, int needResource, int hasResource)
        {
            SpriteName icon = ResourceLib.PayIcon(resource);

            if (icon != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(icon));
                content.space(0.5f);
            }

            bool hasEnough = hasResource >= needResource;

            content.Add(new RbImage(hasEnough ? AvailableIcon : NotAvailableIcon));

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                ResourceLib.Name(resource), TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add( new RbText(text, ResourceCostColor(hasEnough)));
        }
        public static void ResourceCost(RichBoxContent content, SpriteName resourceIcon, string resourceName, int needResource, int hasResource)
        {      
            if (resourceIcon != SpriteName.NO_IMAGE)
            {
                content.Add(new RbImage(resourceIcon));
                content.space(0.5f);
            }

            string text = string.Format(DssRef.lang.Hud_Purchase_ResourceCostOfAvailable,
                resourceName, TextLib.LargeNumber(needResource), TextLib.LargeNumber(hasResource));

            content.Add(new RbText(text, ResourceCostColor(hasResource >= needResource)));
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
            content.newLine();
            string text = string.Format( DssRef.lang.Language_ItemCountPresentation, item, count);
            content.Add(new RbImage(icon));
            content.space(0.5f);
            content.Add(new RbText(text));
        }

        public static RbText ItemCount(RichBoxContent content, string item, string count)
        {
            
            string text = string.Format(DssRef.lang.Language_ItemCountPresentation, item, count);
            return content.text(text);
        }

        public static void returnButton(RichBoxContent content, RichMenu menu, bool bReturn, Action close)
        {
            if (bReturn)
            {
                content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsHudIconReturn, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_ReturnToPrevious)
                    }, new RbAction(menu.menuBack, RbSoundType.Back)));
            }
            if (close != null)
            {
                content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> {
                    new RbImage( SpriteName.WarsHudIconExit, 0.8f),
                    new RbSpace(),
                    new RbText(DssRef.lang.Hud_Close)
                    }, new RbAction(close, RbSoundType.Back)));
            }
            content.newParagraph();
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
                result = string.Format(DssRef.lang.Hud_Time_Hours, (int)time.TotalHours) + ", " + result;
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

        //public static void listAndEditFlag(RichBoxContent content, int playerNum, LocalPlayerStorage playerData, bool editor, RichMenu menu)
        //{
        //    DropDownBuilder flagOptions = new DropDownBuilder("listflags" + playerNum.ToString());
        //    {
        //        for (int i = 0; i < DssRef.storage.flagStorage.flagDesigns.Count; ++i)
        //        {
        //            flagOptions.AddSubOption(DssRef.storage.flagStorage.flagDesigns[i].RbButton(), i == playerData.flagDesignIndex, false, new RbAction2Arg<int, int>(selectProfileLink, playerNum, i), null);
        //        }
        //        flagOptions.menuCaption = DssRef.storage.flagStorage.flagDesigns[playerData.flagDesignIndex].RbButton();
        //        flagOptions.injectAfter = new List<AbsRichBoxMember>() {
        //                            new ArtButton(editor? RbButtonStyle.Primary : RbButtonStyle.Secondary, new List<AbsRichBoxMember> {
        //                                new RbImage(SpriteName.EditorToolPencil) }, new RbAction1Arg<int>(openProfileEditor, playerData.flagDesignIndex), new RbTooltip_Text(DssRef.lang.Lobby_FlagEdit))
        //                        };
        //        flagOptions.Build(content, SpriteName.NO_IMAGE, null, menu);
        //    }
        //}
        //static void selectProfileLink(int playerNumber, int profile)
        //{
        //    int ix = playerNumber - 1;
        //    LocalPlayerStorage playerData = DssRef.storage.localPlayers[ix];
        //    //playerData.inputSource = InputSource.DefaultPC;
        //    //DssRef.storage.checkPlayerDoublettes(playerNumber - 1);

        //    playerData.flagDesignIndex = profile;

        //    DssRef.storage.checkPlayerDoublettes(ix);

        //    DssRef.storage.Save(null);
        //    refreshSplitScreen();

        //    underMenu.CloseDropDown();
        //}
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
            action.sound = RbSoundType.Option;

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
            click.sound = RbSoundType.Back;
            button.overrideBgColor = Color.DarkRed;

            content.Add(button);
        }

        public static List<AbsRichBoxMember> NextArrow(List<AbsRichBoxMember> content)
        {
            content.Add(new RbSpace(2));
            content.Add(new RbImage(SpriteName.LfMenuMoreMenusArrow, 0.4f, MenuMoreOptionsArrowCol));
            return content;
        }

        public static RbImage BulletPoint(RichBoxContent content)
        {
            var dot = new RbImage(SpriteName.warsBulletPoint, 0.8f, null, 0f, 0.3f);
            //dot.color = Color.DarkGray;
            content.Add(dot);
            return dot;
        }
        public static RbImage BulletSeperationPoint(RichBoxContent content)
        {
            var dot = new RbImage(SpriteName.warsBulletSeperationPoint, 0.8f);
            //dot.color = Color.DarkGray;
            content.Add(dot);
            return dot;
        }
        public static void CityResource(RichBoxContent content, City city, ItemResourceType type)
        {
            bool buffer = false;
            city.GetGroupedResource(type).toMenu(content, type, city.foodSafeGuardIsActive(type), ref buffer);
        }

        public static List<AbsRichBoxMember> AddLockOnDemo(List<AbsRichBoxMember> buttonContent)
        {
            if (PlatformSettings.STEAM_DEMO)
            {
                buttonContent.Insert(0, new RbImage(SpriteName.birdLock, 1.4f));
            }

            return buttonContent;
        }

        public static void WishListButton(RichBoxContent content)
        {
            if (PlatformSettings.STEAM_DEMO && Ref.steam.isInitialized)
            {
                content.newLine();
                var wishlistBtn = new RbButton(new List<AbsRichBoxMember> { new RbTab(0.21f), new RbText(DssRef.lang.LobbyDemoMode_WishlistOn, Color.White), new RbSpace(), new RbImage(SpriteName.SteamIcon) }, new RbAction(() =>
                {
                    SteamAPI.SteamFriends().ActivateGameOverlayToStore(
                    3585100,
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
                }), null, true);
                wishlistBtn.overrideBgColor = Color.Green;
                wishlistBtn.fillWidth = true;
                content.Add(wishlistBtn);
            }
        }

        public static Color? NegativeRed(int value)
        {
            if (value < 0)
            {
                return NotAvailableColor;
            }
            else
            {
                return null;
            }
        }

        public static Color? NegativeRed(long value)
        {
            if (value < 0)
            {
                return NotAvailableColor;
            }
            else
            {
                return null;
            }
        }


        public static void taxInfo(RichBoxContent content, object tag)
        {
            City city = (City)tag;
            CityEconomyData cityEconomy = new CityEconomyData();
            cityEconomy.tax(city, out float taxPerWorker_copp);
            content.text(string.Format(DssRef.lang.Economy_TaxDescription, Resource.Money.CopperToGoldString_Decimal((int)taxPerWorker_copp)));
            content.newParagraph();
            content.text(DssRef.lang.Info_PerSecond);
        }

        public static void servicemenUpkeepInfo(RichBoxContent content, object tag)
        {
            content.text(string.Format(DssRef.lang.Economy_ServicemenUpkeep_Description, Resource.Money.CopperToGoldString_Decimal(DssConst.UpkeepPerServiceMan_copp)));
            content.newParagraph();
            content.text(DssRef.lang.Info_PerSecond);
        }

        public static void guardUpkeepInfo(RichBoxContent content, object tag)
        {
            content.text(string.Format(DssRef.lang.Economy_GuardUpkeep_Description, Resource.Money.CopperToGoldString_Decimal(DssConst.UpkeepPerGuard_copp)));
            content.newParagraph();
            content.text(DssRef.lang.Info_PerSecond);        
        }
    }
}
