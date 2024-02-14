using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.Match3
{
    class TimeManager
    {
        static readonly Color MinuteCol = Color.LightGreen;
        const int MinutesToSpeedUp = 3;
        
        const int TotalTime = m3Lib.SpeedUpLevels * MinutesToSpeedUp;

        int minutes = TotalTime;
        int seconds = TimeExt.MinuteInSeconds;
        float partSecond = 0;
        int nextSpeedUpMinutes = 0;

        Graphics.Image[] speedArrows;
        Graphics.Image watch;
        Display.SpriteText timeText;
        
        public TimeManager()
        {
            speedArrows = new Graphics.Image[m3Lib.SpeedUpLevels];
            {
                Vector2 sz = Engine.Screen.IconSizeV2 * 0.8f;

                for (int i = 0; i < m3Lib.SpeedUpLevels; ++i)
                {
                    var ar = Table.CellPlacement(VectorExt.AddY(Engine.Screen.SafeArea.CenterTop, sz.Y * 0.5f), 
                        true, i, m3Lib.SpeedUpLevels, sz, Vector2.Zero);

                    Graphics.Image arrow = new Graphics.Image(
                        i == 0 ? SpriteName.m3GameSpeedArrow : SpriteName.m3GameSpeedArrowGray,
                        ar.Position, ar.Size, ImageLayers.Background1);

                    speedArrows[i] = arrow;
                }
            }

            watch = new Graphics.Image(SpriteName.BirdTimesUp1, Engine.Screen.SafeArea.Position, Engine.Screen.IconSizeV2, ImageLayers.Background1);
            {
                timeText = new Display.SpriteText(TotalTime.ToString(), watch.CenterBottom,
                    Engine.Screen.SmallIconSize, ImageLayers.Background0, new Vector2(0.5f, 0), MinuteCol, true);
            }
        }

        public void Update()
        {
            partSecond += Ref.DeltaTimeSec;

            if (PlatformSettings.DevBuild && Input.Keyboard.Ctrl)
            {
                partSecond += 1f;
            }

            if (partSecond >= 1f)
            {
                partSecond -= 1f;

                if (--seconds <= 0)
                {
                    seconds = TimeExt.MinuteInSeconds -1;

                    if (++nextSpeedUpMinutes >= MinutesToSpeedUp)
                    {
                        nextSpeedUpMinutes = 0;
                        //speed up
                        m3Ref.gamestate.addSpeedLevel();
                    }

                    if (--minutes <= 0)
                    {
                        //End 
                        timeText.Text("0", Color.Red);
                        m3Ref.gamestate.onTimeUp();
                        return;
                    }

                    if (minutes > 0)
                    {
                        timeText.Text(minutes.ToString(), MinuteCol);
                    }
                }

                if (minutes <= 1)
                {
                    timeText.Text(seconds.ToString(), Color.Orange);
                }
            }
        }

        public void animateSpeedUp()
        {
            foreach (var m in speedArrows)
            {
                m.SetSpriteName(SpriteName.m3GameSpeedArrowGray);
            }

            for (int i = 0; i <= m3Ref.gamestate.speedLevel; ++i)
            {
                new Graphics.SpriteFlash(speedArrows[i], SpriteName.m3GameSpeedArrow, SpriteName.m3GameSpeedArrowGray,
                    8, 300);
            }
        }
    }
}
