using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars
{
    class RyanState : Engine.GameState
    {
        public RyanState()
            :base()
        {
            LoadContent.LoadConsoleFont();

            draw.ClrColor = Color.DarkGreen;
            new TextG(LoadedFont.Console, Engine.Screen.CenterScreen, Vector2.One, Align.CenterAll, "Hi Ryan! (press enter)", 
                Color.White, ImageLayers.Top0_Front);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Enter))
            {
                new IntroState(false);
            }

        }
    }
}
