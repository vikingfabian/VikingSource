using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.HUD
{
    class UiStyleGuide
    {
        public TextFormat breadText;
        public TextFormat titleText;

        public float iconSz, smallIconSz;
        public Vector2 iconSzV2, smallIconSzV2;

        public RichBoxSettings richBoxSettings;

        public float iconToTextHeightPerc = 0.8f;

        public void quickSetup(
            LoadedFont titleFont, Color titleCol, 
            LoadedFont breadFont, Color breadCol)
        {
            titleText.Font = titleFont;
            titleText.Color = titleCol;

            breadText.Font = breadFont;
            breadText.Color = breadCol;

            applyScreenSettings();
            appyGameSettings();
        }

        public void applyScreenSettings()
        {
            breadText.size = Engine.Screen.TextBreadHeight;
            titleText.size = Engine.Screen.TextTitleHeight;

            iconSz = Engine.Screen.IconSize;
            smallIconSz = Engine.Screen.IconSize;
        }

        public void appyGameSettings()
        {
            breadText.size = MathExt.Round(breadText.size * Ref.gamesett.UiScale);
            titleText.size = MathExt.Round(titleText.size * Ref.gamesett.UiScale);

            iconSz = MathExt.Round(iconSz * Ref.gamesett.UiScale);
            iconSzV2 = new Vector2(iconSz);

            smallIconSz = MathExt.Round(smallIconSz * Ref.gamesett.UiScale);
            smallIconSzV2 = new Vector2(smallIconSz);

            //TODO dyslexia

            richBoxSettings.breadText = breadText;
            richBoxSettings.head1 = titleText;
            richBoxSettings.refreshIconSz();
        }
    }
}
