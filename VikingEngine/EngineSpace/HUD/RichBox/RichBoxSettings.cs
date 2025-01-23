using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.HUD.RichBox
{
    struct RichBoxSettings
    {
        public TextFormat breadText;
        public TextFormat head1, head2;
        public TextFormat button;
        public TextFormat buttonSecondary;
        public TextFormat buttonDisabled;

        public TextFormat tabSelected;
        public TextFormat tabNotSelected;


        public SpriteName checkOn, checkOff;
        public SpriteName optionOn, optionOff;

        public float breadIconHeight, titleIconHeight;

        public NineSplitSettings artButtonTex;
        public NineSplitSettings artCheckButtonTex;
        public NineSplitSettings artOptionButtonTex;
        public NineSplitSettings artTabTex;


        public RichBoxSettings(TextFormat breadText, TextFormat button, float iconHeight, float TitleSizeUp = 1.2f)
        {
            this.breadText = breadText;
            this.button = button;

            buttonDisabled = button;
            buttonDisabled.BgColor = ColorExt.Mix(Color.DarkGray, button.BgColor, 0.6f);
            buttonDisabled.Color = ColorExt.Mix(Color.DarkGray, button.Color, 0.6f);

            buttonSecondary = button;
            buttonSecondary.BgColor = ColorExt.Mix(Color.LightGray, button.BgColor, 0.5f);

            tabSelected = button;
            tabNotSelected = buttonSecondary;

            head2 = breadText;
            head2.size *= TitleSizeUp;
            head1 = head2;
            head1.size *= TitleSizeUp;

            this.breadIconHeight = iconHeight;
            this.titleIconHeight = iconHeight * TitleSizeUp;

            checkOn = SpriteName.cmdHudCheckOn;
            checkOff = SpriteName.cmdHudCheckOff;
        }

        public RichBoxSettings(GuiStyle guiStyle)
            : this()
        {
            breadText = guiStyle.textFormat;
            breadText.size = Graphics.AbsText.ScaleToHeight(breadText.size, breadText.Font);

            head1 = breadText;
            refreshIconSz();
        }

        public void scaleUp(float scaleUp = 1.2f)
        {
            breadText.size *= scaleUp;
            head1.size *= scaleUp;
            head2.size *= scaleUp;

            breadIconHeight = breadText.size * scaleUp;
            titleIconHeight = breadIconHeight;
        }
        public void refreshIconSz(float scaleUp = 1.2f)
        {
            breadIconHeight = breadText.size * scaleUp;
            titleIconHeight = breadIconHeight;
        }
    }
}
