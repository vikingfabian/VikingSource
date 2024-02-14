using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.HUD;
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
