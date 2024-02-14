using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    struct GuiStyle
    {
        public const float LargeButtonScaleUp = 1.3f;
        public static readonly Color StandardMidColor = new Color(48, 48, 48);

        public SpriteName background;

        public Color ShineColor;// = new Color(128, 128, 128);
        public Color SliderColor;// = new Color(92, 92, 92);//new Color(82,82,82);
        public Color Mid_ButtonCol;// = new Color(48, 48, 48);
        public Color Dark_LabelColor;// = new Color(32, 32, 32);
        public Color BackgroundColor;// = new Color(32, 32, 32);
        public Color Tint;// = new Color(0, 1, 0);

        public TextFormat textFormat, tooltipFormat, textFormatDebug;
        public float memberHeight;
        public float memberSpacing;
        public float layoutWidth;
        public float percentageOfViewToEdgeWhenScrolling;

        public float textEdgeSpace;

        public int fadeTimeMS;// = 100;

        public float tooltipDelayMS;
        public Color tooltipBgCol;
        public bool headBar;
        public float headBarHeight;
        public SpriteName headReturnIcon, headCloseIcon;
        public Color headBarColor, headBarTextColor;

        public Sound.SoundContainerBase openSound;
        public Sound.SoundContainerBase closeSound;


        public GuiStyle(float width, float memberSpacing, SpriteName selectionTile)
        {
            int alpha = 255;
            int bgAlpha = 255;
            Tint = new Color(1f, 1f, 0f);
            ShineColor = new Color(128, 128, 128, alpha);
            SliderColor = new Color(92, 92, 92, alpha);
            Mid_ButtonCol = new Color(48, 48, 48, alpha);
            Dark_LabelColor = new Color(32, 32, 32, alpha);
            BackgroundColor = new Color(24, 24, 24, bgAlpha);
            //Tint = new Color(0, 1, 0);

            fadeTimeMS = 100;

            openSound = null; closeSound = null;
            //openSound = new Sound.SoundSettings(LoadedSound.MenuHi100MS, 0.1f);
            //closeSound = new Sound.SoundSettings(LoadedSound.MenuLo100MS, 0.1f);

            background = SpriteName.WhiteArea;

            this.layoutWidth = width;
            this.memberSpacing = memberSpacing;


            memberHeight = Engine.Screen.IconSize * 0.8f;//Engine.Screen.Height * 0.04f; //0.06f;

            textEdgeSpace = memberHeight * 0.1f;
            percentageOfViewToEdgeWhenScrolling = 0.25f;

            textFormat = new TextFormat(LoadedFont.Regular, Engine.Screen.TextSize, Color.White, ColorExt.Empty);
            textFormatDebug = textFormat;
            textFormatDebug.Font = LoadedFont.Console;

            tooltipDelayMS = 140;
            tooltipFormat = textFormat;
            tooltipBgCol = Dark_LabelColor;

            headBar = true;
            headBarHeight = 1.1f * memberHeight;
            headReturnIcon = SpriteName.MenuIconResume;
            headCloseIcon = SpriteName.LfCheckNo;

            headBarColor = BackgroundColor;
            headBarTextColor = Tint;

        }
    }
}
