﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.Timer;

namespace VikingEngine.DSSWars.Display.CutScene
{
    
    class EndScene : AbsCutScene
    {
        int state_0black_1in_2ready = 0;
        Graphics.Image blackout;
        Texture2D bgTex = null;
        Graphics.ImageAdvanced bgImage = null;
        bool victory, bossVictory;
        EndSceneDisplay display;

        DoomEpilogue doomEpilouge;

        public EndScene(bool victory, bool bossVictory)
            : base()
        {
            this.victory = victory;
            this.bossVictory = bossVictory;
            VectorRect area = Screen.Area;
            area.AddRadius(5);
            blackout = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.CutSceneBgLayer+1);
            blackout.Color = victory ? new Color(16, 16, 32) : new Color(3, 9, 8);
            blackout.Opacity = 0;

            new Timer.AsynchActionTrigger(load_asynch, true);
        }

        void load_asynch()
        {
            string image = victory ? "success" : "fail";

            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + image);
        }

        override public void Time_Update(float time)
        {
            if (doomEpilouge != null)
            {
                if (doomEpilouge.update())
                {
                    doomEpilouge = null;
                }
                return;
            }

            switch (state_0black_1in_2ready)
            {
                case 0:                    
                    blackout.Opacity += 0.6f * Ref.DeltaTimeSec;
                    if (blackout.Opacity >= 1)
                    {
                        Ref.music.PlaySong(victory ? Data.Music.Victory : Data.Music.Fail, false);
                        state_0black_1in_2ready++;
                    }
                    break;
                case 1:
                    if (bgImage == null)
                    {
                        if (bgTex != null)
                        {
                            float w = Screen.SafeArea.Width;
                            float h = w / bgTex.Width * bgTex.Height;

                            if (h > Screen.SafeArea.Height)
                            { 
                                h = Screen.SafeArea.Height;
                                w = h / bgTex.Height * bgTex.Width;
                            }

                            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                                Screen.CenterScreen - new Vector2(w, h) * 0.5f, new Vector2(w, h), HudLib.CutSceneBgLayer-1, false);
                            bgImage.Texture = bgTex;
                            bgImage.SetFullTextureSource();
                            bgImage.Opacity = 0f;
                        }
                    }
                    else
                    {
                        bgImage.Opacity += 1.2f * Ref.DeltaTimeSec;
                        if (bgImage.Opacity >= 1f)
                        { 
                            state_0black_1in_2ready++;
                            new TimedAction0ArgTrigger(initDisplay, 500);
                        }
                    }
                    break;
                
                case 2:
                    if (display!= null )
                    {
                        display.update();
                    }                    
                    break;
            }

        }

        void watchEpilogue()
        {
            doomEpilouge = new DoomEpilogue();
        }

        void initDisplay()
        {
            display = new EndSceneDisplay(victory, bossVictory, watchEpilogue);
        }

        public override void Close()
        {
            blackout.DeleteMe();
            bgImage.DeleteMe();
            display.DeleteMe();
            base.Close();
        }
    }
}
