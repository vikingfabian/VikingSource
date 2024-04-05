using Microsoft.Xna.Framework;
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
    abstract class AbsCutScene
    {
        public AbsCutScene()
        {
            DssRef.state.cutScene = this;
        }

        abstract public void Time_Update(float time);
    }

    class EndScene : AbsCutScene
    {
        int state_0black_1in_2ready = 0;
        Graphics.Image blackout;
        Texture2D bgTex = null;
        Graphics.ImageAdvanced bgImage = null;
        bool success;
        EndSceneDisplay display;
        public EndScene(bool success)
            : base()
        {
            this.success = success;
            VectorRect area = Screen.Area;
            area.AddRadius(5);
            blackout = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.CutSceneBgLayer);
            blackout.Color = new Color(16, 16, 32);
            blackout.Opacity = 0;

            

            new Timer.AsynchActionTrigger(load_asynch, true);
        }

        void load_asynch()
        {
            string image = success ? "success" : "fail";

            bgTex = Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + image);
            //new Timer.Action0ArgTrigger(loadingComplete);
        }

        override public void Time_Update(float time)
        {
            switch (state_0black_1in_2ready)
            {
                case 0:
                    blackout.Opacity += 1.2f * Ref.DeltaTimeSec;
                    if (blackout.Opacity >= 1)
                    {
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
                            float x = Screen.SafeArea.X;

                            float y = Screen.CenterScreen.Y - h * 0.5f;

                            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                                new Vector2(x, y), new Vector2(w, h), HudLib.CutSceneBgLayer, false);
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
                
            }

        }

        void initDisplay()
        {
            display = new EndSceneDisplay();
        }
    }
}
