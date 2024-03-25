using Microsoft.Xna.Framework;
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
using VikingEngine.ToGG.ToggEngine.QueAction;

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
    }
    class ProfileEditorHudPart: RichboxGuiPart
    {
        public PaintFlagState state;

        public ProfileEditorHudPart(PaintFlagHud gui, PaintFlagState state)
            : base(gui)
        {
            this.state = state;
            refresh();
        }

        public void refresh()
        {
            beginRefresh();
            content.text(DssRef.lang.ProfileEditor_Description);
            content.newLine();
            content.Add(new RichBoxImage(SpriteName.KeyAlt));
            content.Add(new RichBoxText(DssRef.lang.ProfileEditor_Bucket));

            content.newParagraph();

            content.h2(DssRef.lang.ProfileEditor_FlagColorsTitle);
            content.newLine();
            flagcolor(ProfileColorType.Main);
            flagcolor(ProfileColorType.Detail1);
            flagcolor(ProfileColorType.Detail2);

            content.newParagraph();

            content.h2(DssRef.lang.ProfileEditor_PeopleColorsTitle);
            content.newLine();
            peoplecolor(ProfileColorType.Hair);
            peoplecolor(ProfileColorType.Skin);

            content.newParagraph();

            content.h2(DssRef.lang.ProfileEditor_MoveImage);
            content.newLine();
            content.Button(DssRef.lang.ProfileEditor_MoveImageUp, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.NegativeY), null, true);
            content.newLine();
            content.Button(DssRef.lang.ProfileEditor_MoveImageLeft, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Left), null, true);
            content.space();
            content.Button(DssRef.lang.ProfileEditor_MoveImageRight, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.Right), null, true);
            content.newLine();
            content.Button(DssRef.lang.ProfileEditor_MoveImageDown, new RbAction1Arg<IntVector2>(state.moveOption, IntVector2.PositiveY), null, true);
            
            content.newParagraph();
            
            if (PlatformSettings.DevBuild)
            {
                content.Button("*Print array*", new RbAction(debugPrintArray), null, true);
                content.newLine();
            }
            content.Button(DssRef.lang.ProfileEditor_DiscardAndExit, new RbAction(state.discardAndExit), null, true);
            content.newLine();
            content.Button(DssRef.lang.ProfileEditor_SaveAndExit, new RbAction(state.saveAndExit), null, true);
            content.newLine();
            
            endRefresh(Engine.Screen.SafeArea.Position, true);

            //interaction?.DeleteMe();
            //interaction = new RbInteraction(content, HudLib.HeadDisplayContentLayer,
            //    input.Select);
        }

        void flagcolor(ProfileColorType colorType)
        {
            content.text(PaintFlagState.ProfileColorName(colorType));
            content.newLine();
            //content.Add(new RichboxButton(
            //    new List<AbsRichBoxMember>
            //    {
            //        new RichBoxImage(SpriteName.IconColorPick),
            //    },
            //    new RbAction1Arg<ProfileColorType>(state.changeColor, colorType), null, true));
            //content.space();
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
            //content.Add(new RichboxButton(
            //    new List<AbsRichBoxMember>
            //    {
            //        new RichBoxImage(SpriteName.IconColorPick),
            //        color
            //    },
            //    new RbAction1Arg<ProfileColorType>(state.changeColor, colorType), null, true));

            content.newLine();
        }

        void debugPrintArray()
        {
            state.profile.PrintFlagColors();
            state.file.dataGrid.Print();
            
        }

        void selectColorType(ProfileColorType colorType)
        {
            state.setColorType(colorType);
            
            refresh();
        }

       
    }


}
