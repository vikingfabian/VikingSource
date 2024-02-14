using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.DSSWars.Profile
{
    class ColorButtonGroup : OptionButtonGroup
    {
        public static HUD.ButtonGuiSettings ButtonGuiSettings;

        public ColorButtonGroup(VectorRect paintArea, ProfileData profile)
        {
            ButtonGuiSettings = new HUD.ButtonGuiSettings(Color.White, 4f, Color.White, Color.Red);

            Vector2 nextPos = VectorExt.AddX(paintArea.RightTop, Engine.Screen.SmallIconSize);

            createButton(ProfileColorType.Main);
            createButton(ProfileColorType.Detail1);
            createButton(ProfileColorType.Detail2);

            select(0);

            void createButton(ProfileColorType type)
            {
                Graphics.TextG name = new Graphics.TextG(LoadedFont.Regular, 
                    nextPos,
                    Engine.Screen.TextSizeV2,
                    Align.Zero,
                     ProfileSettingsState.ProfileColorName(type), Color.White, ImageLayers.Lay2);
                nextPos.Y += Engine.Screen.TextBreadHeight + ButtonGuiSettings.highlightThickness *2;
                VectorRect area = new VectorRect(nextPos, Engine.Screen.IconSizeV2);

                if (type == ProfileColorType.Main)
                {
                    area.Width *= 2f;
                }

                buttons.Add(new ColorButton(area, type));

                nextPos.Y = area.Bottom + Engine.Screen.BorderWidth + ButtonGuiSettings.highlightThickness * 4;
            }

            refreshColors(profile);
        }

        public void refreshColors(ProfileData profile)
        {
            foreach (var m in buttons)
            {
                ((ColorButton)m).refeshColor(profile);
            }
        }
    }

    class ColorButton : AbsOptionButton
    {
        public ColorButton(VectorRect area, ProfileColorType opt)
            : base(ColorButtonGroup.ButtonGuiSettings, opt)
        {
            this.area = area;

            baseImage = new Graphics.Image(SpriteName.WhiteArea,
                area.Position, area.Size, ImageLayers.Lay0);

            createHighlight();
        }

        public void refeshColor(ProfileData profile)
        {
            baseImage.Color = profile.getColor((ProfileColorType)option);
        }
    }
}
