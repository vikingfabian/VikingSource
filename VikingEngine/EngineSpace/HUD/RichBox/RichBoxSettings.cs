﻿using System;
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
        public TextFormat buttonDisabled;
        public SpriteName checkOn, checkOff;

        public float breadIconHeight, titleIconHeight;

        public RichBoxSettings(TextFormat breadText, TextFormat button, float iconHeight, float TitleSizeUp = 1.2f)
        {
            this.breadText = breadText;
            this.button = button;

            buttonDisabled = button;
            buttonDisabled.BgColor = ColorExt.Mix(Color.DarkGray, button.BgColor, 0.6f);
            buttonDisabled.Color = ColorExt.Mix(Color.DarkGray, button.Color, 0.6f);

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

        public void refreshIconSz(float scaleUp = 1.2f)
        {
            breadIconHeight = breadText.size * scaleUp;

            titleIconHeight = breadIconHeight;
        }
    }
}
