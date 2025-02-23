﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;
using VikingEngine.DSSWars.Display;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.MoonFall;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.QueAction;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.DSSWars.Profile
{
    class PaintFlagHud : RichboxGui
    {
        public ProfileEditorHudPart part;
        public HSLColorArea colorArea;
       
        public PaintFlagHud(InputMap input, PaintFlagState state)
            : base()
        { 
            this.input = input;
            settings = HudLib.richboxGui;
            settings.width= Engine.Screen.Width * 0.25f;

            part = new ProfileEditorHudPart(this, state);
            parts = new List<RichboxGuiPart> { part };

            colorArea = new HSLColorArea(input, state);
        }

        public override bool update()
        {
            part.update();
            return base.update();
        }
    }

    class ProfileEditorHudPart: RichboxGuiPart
    {
        bool needRefresh = true;
        public PaintFlagState state;

        public ProfileEditorHudPart(PaintFlagHud gui, PaintFlagState state)
            : base(gui)
        {
            this.state = state;
            //refresh();
        }

        public void refresh()
        {
            needRefresh = true;
        }

        public void update()
        {
            if (needRefresh)
            {
                needRefresh = false;

                beginRefresh();

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
                    var undoContent = new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Hud_Undo) };
                    content.Add(new RichboxButton(undoContent, new RbAction(state.undo), null, state.undoHistory.Count > 1));
                    content.space();

                    var redoContent = new List<AbsRichBoxMember> { new RichBoxText(DssRef.lang.Hud_Redo) };
                    content.Add(new RichboxButton(redoContent, new RbAction(state.redo), null, state.redoHistory.Count > 0));
                    content.newParagraph();
                    if (state.controllerMode)
                    {
                        content.icontext(SpriteName.Dpad, DssRef.lang.ProfileEditor_MoveImage);
                    }
                    else
                    {
                        content.h2(DssRef.lang.ProfileEditor_MoveImage).overrideColor = HudLib.TitleColor_Label;
                        content.newLine();
                        content.Button(DssRef.lang.ProfileEditor_MoveImageUp, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.NegativeY), null, true);
                        content.newLine();
                        content.Button(DssRef.lang.ProfileEditor_MoveImageLeft, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Left), null, true);
                        content.space();
                        content.Button(DssRef.lang.ProfileEditor_MoveImageRight, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Right), null, true);
                        content.newLine();
                        content.Button(DssRef.lang.ProfileEditor_MoveImageDown, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.PositiveY), null, true);
                    }
                    content.newParagraph();

                    if (PlatformSettings.DevBuild)
                    {
                        content.Button("*Print array*", new RbAction(debugPrintArray), null, true);
                        content.newLine();
                    }
                    if (state.controllerMode == false)
                    {
                        content.Button(DssRef.lang.FlagEditor_ClearAll, new RbAction(state.clearAll), null, true);
                    }
                    content.newLine();
                    content.Button(state.controllerMode ? SpriteName.ButtonBACK : SpriteName.NO_IMAGE, DssRef.lang.ProfileEditor_DiscardAndExit, new RbAction(state.discardAndExit), null, true);
                    content.newLine();
                    content.Button(state.controllerMode ? SpriteName.ButtonSTART : SpriteName.NO_IMAGE, DssRef.lang.Hud_SaveAndExit, new RbAction(state.saveAndExit), null, true);
                    content.newLine();
                }
                endRefresh(Engine.Screen.SafeArea.Position, true);
            }
        }

        private void colorTypes()
        {
            content.h2(DssRef.lang.ProfileEditor_FlagColorsTitle).overrideColor = HudLib.TitleColor_Label;
            content.newLine();
            flagcolor(ProfileColorType.Main);
            flagcolor(ProfileColorType.Detail1);
            flagcolor(ProfileColorType.Detail2);

            content.newParagraph();

            content.h2(DssRef.lang.ProfileEditor_PeopleColorsTitle).overrideColor = HudLib.TitleColor_Label;
            content.newLine();

            peoplecolor(ProfileColorType.Skin);
            peoplecolor(ProfileColorType.Hair);
        }

        void flagcolor(ProfileColorType colorType)
        {
            content.text(PaintFlagState.ProfileColorName(colorType));
            content.newLine();
            var color = new RichBoxImage(SpriteName.WhiteArea);
            color.color = state.profile.getColor(colorType);
            content.Add(new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxImage(SpriteName.EditorToolPencil),
                    color,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType), null, true));

            if (state.selectedColorType == colorType)
            {
                content.Add(new RichBoxImage(SpriteName.LfNpcSpeechArrow));
            }
            content.newLine();
        }

        void peoplecolor(ProfileColorType colorType)
        {
            content.text(PaintFlagState.ProfileColorName(colorType));
            content.newLine();
            var color = new RichBoxImage(SpriteName.WhiteArea);
            color.color = state.profile.getColor(colorType);
            content.Add(new RichboxButton(
                new List<AbsRichBoxMember>
                {
                    new RichBoxImage(SpriteName.IconColorPick),
                    color,
                },
                new RbAction1Arg<ProfileColorType>(selectColorType, colorType), null, true));

            if (state.selectedColorType == colorType)
            {
                content.Add(new RichBoxImage(SpriteName.LfNpcSpeechArrow));
            }

            content.newLine();
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
