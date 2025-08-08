using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Delivery;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.QueAction;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.GameState.FlagEditor
{
    class PaintFlagHud //: RichboxGui
    {
        RichBoxContent content;
        InputMap input;
        //public ProfileEditorHudPart part;
        public HSLColorArea colorArea;
        RichMenu menu;
        PaintFlagState state;
        bool needRefresh = true;
        const float TabStep = 0.32f;
        public PaintFlagHud(Engine.PlayerData playerData, InputMap input, PaintFlagState state)
            : base()
        {
            this.state = state;
            this.input = input;
            var settings = HudLib.RbSettings;
            float width= Engine.Screen.Width * 0.25f;


            var area = Engine.Screen.SafeArea;
            area.Width = width;

            menu = new RichMenu(settings, area, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, playerData);
            var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);
            //part = new ProfileEditorHudPart(this, state);
            //parts = new List<RichboxGuiPart> { part };

            colorArea = new HSLColorArea(input, state);
        }

        //public override bool update()
        //{
        //    part.update();
        //    return base.update();
        //}
    //}

    //class ProfileEditorHudPart: RichboxGuiPart
    //{
        
        //public PaintFlagState state;

        //public ProfileEditorHudPart(PaintFlagHud gui, PaintFlagState state)
        //    : base(gui)
        //{
        //    this.state = state;
        //    //refresh();
        //}

        public void refresh()
        {
            needRefresh = true;
        }

        public void update()
        {
            bool mouseOver = false;
            menu.updateMouseInput(ref mouseOver);

            if (needRefresh)
            {
                needRefresh = false;
                
                buildContent();
                menu.Refresh(content);
                //beginRefresh();


                //endRefresh(Engine.Screen.SafeArea.Position, true);
            }
        }

        void buildContent()
        {
            content = new RichBoxContent();
            if (state.controllerPickColorState)
            {
                content.h2(PaintFlagState.ProfileColorName(state.selectedColorType)).overrideColor = HudLib.TitleColor_Label;
                content.icontext(SpriteName.LeftStick, DssRef.lang.ProfileEditor_Hue);
                content.icontext(SpriteName.RightStick, DssRef.lang.ProfileEditor_Lightness);
                content.newParagraph();

                colorTypes();
            }
            else
            {
                content.text(DssRef.lang.FlagEditor_Description).overrideColor = HudLib.InfoYellow_Light;
                content.newLine();

                content.icontext(state.VisualInput.FlagDesign_PaintBucket.Icon, DssRef.lang.FlagEditor_Bucket);
                content.icontext(state.VisualInput.FlagDesign_ToggleColor_Next.Icon, DssRef.lang.ProfileEditor_NextColorType);

                if (state.controllerMode)
                {
                    content.icontext(state.VisualInput.Controller_FlagDesign_Colorpicker.Icon, DssRef.lang.ProfileEditor_PickColor);
                }

                content.newParagraph();
                colorTypes();

                content.newParagraph();
                var undoContent = new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Undo) };
                content.Add(new ArtButton( RbButtonStyle.Primary,undoContent, new RbAction(state.undo), null, state.undoHistory.Count > 1));
                content.space();

                var redoContent = new List<AbsRichBoxMember> { new RbText(DssRef.lang.Hud_Redo) };
                content.Add(new ArtButton(RbButtonStyle.Primary, redoContent, new RbAction(state.redo), null, state.redoHistory.Count > 0));
                content.newParagraph();
                if (state.controllerMode)
                {
                    content.icontext(SpriteName.Dpad, DssRef.lang.ProfileEditor_MoveImage);
                }
                else
                {
                    content.h2(DssRef.lang.ProfileEditor_MoveImage).overrideColor = HudLib.TitleColor_Label;
                    content.newLine();
                    content.ArtButton(DssRef.lang.ProfileEditor_MoveImageUp, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.NegativeY), null, true);
                    content.newLine();
                    content.ArtButton(DssRef.lang.ProfileEditor_MoveImageLeft, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Left), null, true);
                    content.space();
                    content.ArtButton(DssRef.lang.ProfileEditor_MoveImageRight, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Right), null, true);
                    content.newLine();
                    content.ArtButton(DssRef.lang.ProfileEditor_MoveImageDown, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.PositiveY), null, true);
                }
                content.newParagraph();

                if (PlatformSettings.DevBuild)
                {
                    content.Button("*Print array*", new RbAction(debugPrintArray), null, true);
                    content.newLine();
                }
                if (state.controllerMode == false)
                {
                    content.ArtButton(DssRef.lang.FlagEditor_ClearAll, new RbAction(state.clearAll), null, true);
                }
                content.newLine();

                var discardButtonContent = new List<AbsRichBoxMember> { new RbText(DssRef.lang.ProfileEditor_DiscardAndExit) };
                if (state.controllerMode)
                {
                    discardButtonContent.Insert(0, new RbImage(SpriteName.ButtonBACK));
                }
                content.Add(new ArtButton(RbButtonStyle.Secondary, discardButtonContent, new RbAction(state.discardAndExit)));
                //content.ArtButton(state.controllerMode ? SpriteName.ButtonBACK : SpriteName.NO_IMAGE, DssRef.lang.ProfileEditor_DiscardAndExit, new RbAction(state.discardAndExit), null, true);
                content.newLine();
                content.ArtButton(state.controllerMode ? SpriteName.ButtonSTART : SpriteName.NO_IMAGE, DssRef.lang.Hud_SaveAndExit, new RbAction(state.saveAndExit), null, true);
                content.newLine();
            }
        }

        private void colorTypes()
        {
            content.h2(DssRef.lang.ProfileEditor_FlagColorsTitle).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            flagcolor(ProfileColorType.Main);
            flagcolor(ProfileColorType.Detail1, ProfileColorType.Detail2);
            //flagcolor(ProfileColorType.Detail2);

            content.newParagraph();

            content.h2(DssRef.lang.ProfileEditor_PeopleColorsTitle).overrideColor = HudLib.TitleColor_Label;
            content.newLine();

            peoplecolor([ProfileColorType.Skin, ProfileColorType.Hair]);
            peoplecolor([ProfileColorType.Tunic, ProfileColorType.Pants, ProfileColorType.Leader]);
            altMainColor();
        }

        void flagcolor(ProfileColorType colorType)
        {
            content.Add(new  RbText( PaintFlagState.ProfileColorName(colorType), HudLib.TitleColor_TypeName));
            content.newLine();
            var color = colorContent(colorType);
            content.Add(new ArtOption(state.selectedColorType == colorType,
                new List<AbsRichBoxMember>
                {
                    new RbImage(SpriteName.EditorToolPencil),
                    color,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType), null, true));

            if (state.selectedColorType == colorType)
            {
                content.Add(new RbImage(SpriteName.LfNpcSpeechArrow));
            }
            else
            { 
                pasteColor(colorType);
            }
            content.newLine();
        }

        void pasteColor(ProfileColorType toColorType)
        {
            
            content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconPaste) },
                new RbAction1Arg<ProfileColorType>(copyPasteColorAction, toColorType), new RbTooltip_Text(DssRef.lang.Editor_CopyPasteSelectedColor)));
        }

        void copyPasteColorAction(ProfileColorType toColorType)
        {
            state.setUndoPoint();
            state.profile.setColor(toColorType, state.profile.getColor(state.selectedColorType));
           state.onColorChange();
        }

        void flagcolor(ProfileColorType colorType1, ProfileColorType colorType2)
        {
            content.Add(new RbText(PaintFlagState.ProfileColorName(colorType1), HudLib.TitleColor_TypeName));

            content.Add(new RbTab(TabStep));

            content.Add(new RbText(PaintFlagState.ProfileColorName(colorType2), HudLib.TitleColor_TypeName));

            content.newLine();
            var color1 = colorContent(colorType1);
            content.Add(new ArtOption(state.selectedColorType == colorType1,
                new List<AbsRichBoxMember>
                {
                    new RbImage(SpriteName.EditorToolPencil),
                    color1,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType1), null, true));

            if (state.selectedColorType == colorType1)
            {
                content.Add(new RbImage(SpriteName.LfNpcSpeechArrow));
            }
            else
            {
                pasteColor(colorType1);
            }

            content.Add(new RbTab(TabStep));
            var color2 = colorContent(colorType2);
            content.Add(new ArtOption(state.selectedColorType == colorType2,
                new List<AbsRichBoxMember>
                {
                    new RbImage(SpriteName.EditorToolPencil),
                    color2,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType2), null, true));

            if (state.selectedColorType == colorType2)
            {
                content.Add(new RbImage(SpriteName.LfNpcSpeechArrow));
            }
            else
            {
                pasteColor(colorType2);
            }

            content.newLine();
        }

        void peoplecolor(ProfileColorType[] colorType)
        {
            

            for (int i = 0; i < colorType.Length; ++i)
            {
                content.Add(new RbText(PaintFlagState.ProfileColorName(colorType[i]), HudLib.TitleColor_TypeName));
                content.Add(new RbTab(TabStep * (i +1)));
            }
            content.newLine();
            for (int i = 0; i < colorType.Length; ++i)
            {
                var color = colorContent(colorType[i]);
                content.Add(new ArtOption(state.selectedColorType == colorType[i],
                    new List<AbsRichBoxMember>
                    {
                    new RbImage(SpriteName.IconColorPick),
                    color,
                    },
                    new RbAction1Arg<ProfileColorType>(selectColorType, colorType[i]), null, true));

                if (state.selectedColorType == colorType[i])
                {
                    content.Add(new RbImage(SpriteName.LfNpcSpeechArrow));
                }
                else
                {
                    pasteColor(colorType[i]);
                }
                content.Add(new RbTab(TabStep * (i + 1)));
            }
            content.newLine();
        }

        void altMainColor()
        {
            content.newParagraph();

            var colorType = ProfileColorType.AltMain;
            content.text(PaintFlagState.ProfileColorName(colorType));
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { 
                new RbText(DssRef.lang.Automation_CheckBoxTitle) }, 
                state.autoAltMainProperty));
            content.space();
            var color = colorContent(colorType);
            content.Add(new ArtOption(state.selectedColorType == colorType,
                new List<AbsRichBoxMember>
                {
                    new RbImage(SpriteName.IconColorPick),
                    color,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType), null, true));

            if (state.selectedColorType == colorType)
            {
                content.Add(new RbImage(SpriteName.LfNpcSpeechArrow));
            }
            else
            {
                pasteColor(colorType);
            }

            content.newLine();
        }

        RbImage colorContent(ProfileColorType colorType)
        {
            var color = new RbImage(SpriteName.WhiteArea, 0.8f);
            color.color = state.profile.getColor(colorType);
            return color;
        }

        void debugPrintArray()
        {
            state.profile.PrintFlagColors();
            state.profile.flagDesign.Print();            
        }

       public void selectColorType(ProfileColorType colorType)
        {
            state.setColorType(colorType);
            
            refresh();
        }

        
    }


}
