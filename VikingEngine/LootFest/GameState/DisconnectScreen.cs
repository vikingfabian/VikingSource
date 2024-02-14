using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GameState
{
    class DisconnectScreen : Engine.GameState
    {
        Time viewTime = new Time(2f, TimeUnit.Seconds);

        public DisconnectScreen()
            : base()
        {
            draw.ClrColor = Color.Black;

            Graphics.TextG text = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen, Engine.Screen.TextSizeV2, Graphics.Align.CenterAll,
                "Disconnected", Color.White, ImageLayers.Foreground1);
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (viewTime.CountDown())
            {
                new GameState.LoadingMap(new Data.WorldData(true));
            }
        }

    }

    class EmptyState : Engine.GameState
    {
        Graphics.TextG text;
        public EmptyState()
            : base()
        {
            draw.ClrColor = Color.Black;

            text = new Graphics.TextG(LoadedFont.Regular, Engine.Screen.CenterScreen, Engine.Screen.TextSizeV2, Graphics.Align.CenterAll,
                "EMPTY", Color.White, ImageLayers.Foreground1);

            
        }

        public override void Time_Update(float time)
        {
            if (UpdateCount == 1)
            {
                LfRef.ClearRAM();
            }
            else if (Input.Keyboard.Ctrl)
            {
                
                text.TextString = "GARBAGE (MB): " + (GC.GetTotalMemory(false) / 1000000.0).ToString();

                GC.Collect();
                //LfRef.storage
            }
        }

    }
}
