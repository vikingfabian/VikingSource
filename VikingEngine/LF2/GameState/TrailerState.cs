using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.LF2.GameState
{
    class TrailerState : Engine.GameState 
    {
        System.Threading.Thread thread;
        Video video;
        Graphics.VideoImage videoImage = null;

        Graphics.TextS loadingText;
        Graphics.TextS pauseText;

        public TrailerState()
        {
            MediaPlayer.Stop();
            //InGame_trailer2
            loadingText = new Graphics.TextS(LoadedFont.PhoneText, Engine.Screen.CenterScreen, Vector2.One, Graphics.Align.CenterAll, "Loading video...", Color.White, ImageLayers.Background8);

            pauseText = new Graphics.TextS(LoadedFont.PhoneText, Engine.Screen.CenterScreen, Vector2.One, Graphics.Align.CenterAll, "Paused", Color.White, ImageLayers.Foreground1);
            pauseText.Visible = false;

            thread = new System.Threading.Thread(loadThread);
            thread.Name = "Loading trailer";
            thread.Start();

            
        }

        bool loadingCompleteEvent = false;

        void loadThread()
        {
            video = Engine.LoadContent.Content.Load<Video>("InGame_trailer6");
            loadingCompleteEvent = true;
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            if (loadingCompleteEvent)
            {
                loadingCompleteEvent = false;
                videoImage = new Graphics.VideoImage(video, Engine.Screen.SafeArea.Position, Engine.Screen.SafeArea.Size, ImageLayers.Background4, false);
                videoImage.Play();
            }


            //Input
            if (Input.Controller.KeyDownEvent(Buttons.B, Buttons.Back))
            {
                exit();
            }

            if (videoImage != null)
            {
                if (Input.Controller.KeyDownEvent(Buttons.A, Buttons.Start))
                {
                    videoImage.Pause();
                    pauseText.Visible = videoImage.MediaState == MediaState.Paused;
                    videoImage.Color = videoImage.MediaState == MediaState.Paused ? Color.Gray : Color.White;
                }

                if (videoImage.MediaState == MediaState.Stopped)
                {//Movie ended
                    exit();
                }
            }

           

        }

        void exit()
        {
            thread.Abort();
            new MainMenuState();
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            
        }

        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.MainMenu; }
        }
    }
}
