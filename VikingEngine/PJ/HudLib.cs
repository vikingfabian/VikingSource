using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;

namespace VikingEngine.PJ
{
    static class HudLib
    {
        public static ButtonGuiSettings LargeButtonSettings = new ButtonGuiSettings(
            Color.White, Engine.Screen.BorderWidth, Color.White, Color.DarkGray);

        public static ButtonGuiSettings ButtonSettings = new ButtonGuiSettings(
            Color.White, Engine.Screen.BorderWidth, Color.White, Color.DarkGray);

        public const ImageLayers LayPopup = ImageLayers.Foreground0;
        public const ImageLayers LayPopupBg = ImageLayers.Foreground1;

        public const ImageLayers LayMenu = ImageLayers.Foreground2;
        public const ImageLayers LayButtons = ImageLayers.Foreground5;

        public const ImageLayers LayInputDisplay = ImageLayers.Foreground7;


       public static NineSplitSettings HudMenuBackground, HudMenuScollBackground, HudMenuScollButton;

        public static RichBoxSettings RbSettings;

        public static void Init()
        {
            const float TextToIconSz = 1.2f;

            HudMenuBackground = new HUD.NineSplitSettings(SpriteName.pjRichMenuBg, 1, 8, 1f, true, true);
            HudMenuScollBackground = new HUD.NineSplitSettings(SpriteName.WarsHudScrollerBg, 1, 8, 1f, true, true);
            HudMenuScollButton = new HUD.NineSplitSettings(SpriteName.WarsHudScrollerSlider, 1, 8, 1f, true, true);

            float menuTextScale = Engine.Screen.TextBreadHeight * 1.6f;

            RbSettings = new HUD.RichBox.RichBoxSettings(
               new TextFormat(LoadedFont.Regular, menuTextScale, Color.White, ColorExt.Empty),
               new TextFormat(LoadedFont.Regular, menuTextScale, Color.White, Color.DarkGray),
               menuTextScale * TextToIconSz, 1.2f);
            RbSettings.head1.Font = LoadedFont.Bold;
            RbSettings.head2.Font = LoadedFont.Bold;
            RbSettings.head1.Color = Color.LightGray;
            RbSettings.checkOn = SpriteName.pjRichCheckOn;
            RbSettings.checkOff = SpriteName.pjRichCheckOff;
            RbSettings.optionOn = SpriteName.WarsHudOptionYes;
            RbSettings.optionOff = SpriteName.WarsHudOptionNo;

            RbSettings.tabSelected.BgColor = new Color(104, 149, 219);//new Color(121,110,233);
            RbSettings.tabSelected.Color = new Color(3, 0, 46);
            RbSettings.tabNotSelected.BgColor = new Color(36, 107, 142); //new Color(99,96,146);
            RbSettings.tabNotSelected.Color = RbSettings.tabSelected.Color;

            bool smallScreen = Engine.Screen.Height < 800;
            float nineTextureEdge = smallScreen ? 1f : 2f;

            RbSettings.artPrimaryButtonTex = new HUD.NineSplitSettings(SpriteName.pjRichButton, 1, 8, nineTextureEdge, true, true)
            {
                disableTexture = SpriteName.WarsHudPrimaryButtonDisabled
            };
            RbSettings.artCheckButtonTex = new NineSplitSettings(SpriteName.pjRichButton_Round, 1, 8, nineTextureEdge, true, true);
            RbSettings.artOptionButtonTex = new NineSplitSettings(SpriteName.pjRichButton_Round, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.pjRichButton_RoundNotSelected,
            };
            RbSettings.artDropDownButtonTex = new NineSplitSettings(SpriteName.pjRichButton_Round, 1, 8, nineTextureEdge, true, true)
            {
                notSelectedTexture = SpriteName.pjRichButton_RoundNotSelected,
            };

            RbSettings.dragButtonTex = new ThreeSplitSettings(SpriteName.pjRichDragButton, 1, 15);

            DropDownBuilder.DropDownArrow = SpriteName.pjRichDropDownArrow;
            DropDownBuilder.Selected = SpriteName.pjRichListArrowSelected;
            DropDownBuilder.NotSelected = SpriteName.pjRichListArrowNotSelected;
            DropDownBuilder.Default = SpriteName.pjRichListArrowDefault;

            DropDownBuilder.selectedCaptionColor = Color.LightBlue;
        }


        public static bool ExitEndScoreInput()
        {
            if (Input.Keyboard.KeyDownEvent(Keys.Escape) ||
                Input.Keyboard.KeyDownEvent(Keys.Enter))
            {
                return true;
            }

            foreach (var ins in Input.XInput.controllers)
            {
                if (ins.KeyDownEvent(Buttons.Start, Buttons.Back))
                {
                    return true;
                }
            }

            return false;
        }

        public static Graphics.Image DarkBgOverlay(ImageLayers layer)
        {
            var area = Engine.Screen.Area;
            area.AddRadius(4f);
            var bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, layer, false);
            bg.Color = new Color(38, 74, 113);
            bg.Opacity = 0.8f;

            return bg;
        }

        public static void HudInputDisplay(out IButtonMap menuInput, out IButtonMap startInput, out IButtonMap modeInput)
        {
            if (PjRef.HostingPlayerSource.sourceType == InputSourceType.XController)
            {
                menuInput = new XboxButtonMap(Buttons.Back, 0);
                startInput = new XboxButtonMap(Buttons.Start, 0);
                modeInput = new XboxButtonMap(Lobby.ModeDisplay.NextXInput, 0);
            }
            else
            {
                menuInput = new KeyboardButtonMap(Keys.Escape);
                startInput = new KeyboardButtonMap(Keys.Enter);
                modeInput = new KeyboardButtonMap(Lobby.ModeDisplay.NextKeyboardInput);
            }
        }

        public static Vector2 BigButtonsSize => new Vector2(Engine.Screen.IconSize * 2f);
    }
}
