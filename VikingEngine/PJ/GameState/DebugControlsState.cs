using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.PJ.GameState
{
    class DebugControlsState : Engine.GameState
    {
        Graphics.TextBoxSimple activeControllers, keyInputs;
        Timer.Basic refreshTimer = new Timer.Basic(1000, true);

        public DebugControlsState()
            : base()
        {
            Ref.draw.ClrColor = Color.Blue;

            Graphics.TextG printScreen = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.SafeArea.Position, new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero,
                "[F12] Print screen", Color.Yellow, ImageLayers.Lay1);

            Graphics.TextG exit = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.SafeArea.RightTop, new Vector2(Engine.Screen.TextSize), new Graphics.Align(Vector2.UnitX),
                "Exit [ESC]", Color.Yellow, ImageLayers.Lay1);

            Vector2 pos = Engine.Screen.SafeArea.Position;
            pos.Y += printScreen.MeasureText().Y * 2f;

            activeControllers = new Graphics.TextBoxSimple(LoadedFont.Regular, pos, new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero, "", Color.White, ImageLayers.Lay1, Engine.Screen.Height * 0.5f);

            keyInputs = new Graphics.TextBoxSimple(LoadedFont.Regular, pos, new Vector2(Engine.Screen.TextSize), Graphics.Align.Zero, "", Color.White, ImageLayers.Lay1, Engine.Screen.Height * 0.5f);
            keyInputs.Xpos = Engine.Screen.SafeArea.Right - keyInputs.lineWidth;

            refreshTimer.SetZeroTimeLeft();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            //if (refreshTimer.Update())
            //{
            //    activeControllers.TextString = "DEVICES:" + Environment.NewLine + Input.SharpDXInput.DevicesDebugString(); 
            //}

            string keysText = "KEYS: " + Environment.NewLine;
            for (Keys k = 0; k <= Keys.OemClear; ++k)
            {
                if (Input.Keyboard.IsKeyDown(k))
                {
                    keysText += k.ToString() + " ";
                }
            }

            keysText += Environment.NewLine + "BUTTONS:" + Environment.NewLine;

            foreach (var m in Input.XInput.controllers)
            {
                var buttons = m.listAllButtonDown();
                if (buttons.Count > 0)
                {
                    keysText += m.ToString();
                    foreach (var b in buttons)
                    {
                        keysText += " [" + b.ToString() + "]";
                    }
                    keysText += Environment.NewLine;
                }
            }

            //foreach (var m in Input.SharpDXInput.controllers)
            //{
            //    var buttons = m.listAllButtonDown();
            //    if (buttons.Count > 0)
            //    {
            //        keysText += m.ToString();
            //        foreach (var b in buttons)
            //        {
            //            keysText += " [" + b.ToString() + "]";
            //        }
            //        keysText += Environment.NewLine;
            //    }
            //}

            keyInputs.TextString = keysText;

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Input.InputLib.Init(Ref.main);

                new LobbyState();
            }
        }
    }
}
