using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.ToGG.HeroQuest.MapGen;

namespace VikingEngine.DSSWars.Display.CutScene
{
    class DoomEpilogue
    {
        int state = 0;
        int imageState_0setup_1in_2view_3out = 0;
        int currentTexture = 0;
        float stateTime = float.MaxValue;
        Texture2D[] textures;
        Graphics.ImageAdvanced bgImage = null;

        Graphics.Image blackout;
        Graphics.TextG title;
        Graphics.TextBoxSimple text;
        float currentImageTimeMs = 0;

        public DoomEpilogue() 
        {
            Ref.music.stop(true);
            
            VectorRect area = Screen.Area;
            area.AddRadius(5);
            blackout = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.StoryBgLayer);
            blackout.Color = new Color(3, 9, 8);
            blackout.Opacity = 0;

            title = new Graphics.TextG( LoadedFont.Bold, VectorExt.AddY(Engine.Screen.CenterScreen, -Engine.Screen.IconSize * 2f), Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterWidth, 
                DssRef.lang.EndScreen_Epilogue_Title, Color.Yellow, HudLib.StoryContentLayer);
            text = new Graphics.TextBoxSimple(LoadedFont.Regular, VectorExt.AddY(Engine.Screen.CenterScreen, 0), Engine.Screen.TextSizeV2 * 2f, Graphics.Align.CenterWidth,
               DssRef.lang.EndScreen_Epilogue_Text, 
                Color.White, HudLib.StoryContentLayer, Engine.Screen.Width * 0.3f);

            title.Opacity = 0;
            title.Visible = false;
            text.Opacity = 0;
            text.Visible = false;

            bgImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE,
                Vector2.Zero, Vector2.Zero, HudLib.StoryContentLayer - 1, false);
            bgImage.Opacity = 0;

            new Timer.AsynchActionTrigger(load_asynch, true);
            
            Ref.music.PlaySong(Data.Music.DoomStory, false, false);
        }

        void load_asynch()
        {
            textures = new Texture2D[] {
                Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + "doom_story1"),
                Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + "doom_story2"),
                Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + "doom_story3"),
                Ref.main.Content.Load<Texture2D>(DssLib.StoryContentDir + "doom_story4"),
                };
        }

        public bool update()
        {
            switch (state)
            { 
                case 0://Black fade in
                    blackout.Opacity += 0.6f * Ref.DeltaTimeSec;

                    if (blackout.Opacity >= 1f && 
                        Ref.music.PlaySongState == Sound.PlaySongState.Stopped)
                    {
                        ++state;
                        stateTime = 500;
                    }
                    break;
                case 4://display images
                    switch (imageState_0setup_1in_2view_3out)
                    {
                        case 0:
                            if (textures != null)
                            {
                                var bgTex = textures[currentTexture];
                                float w = Screen.SafeArea.Width;
                                float h = w / bgTex.Width * bgTex.Height;

                                if (h > Screen.SafeArea.Height)
                                {
                                    h = Screen.SafeArea.Height;
                                    w = h / bgTex.Height * bgTex.Width;
                                }

                                bgImage.size = new Vector2(w, h);
                                bgImage.position = Screen.CenterScreen - new Vector2(w, h) * 0.5f;

                                bgImage.Texture = bgTex;
                                bgImage.SetFullTextureSource();

                                currentImageTimeMs = 0;
                                ++imageState_0setup_1in_2view_3out;
                            }
                            
                            break;
                        case 1:
                            bgImage.Opacity += 2f * Ref.DeltaTimeSec;
                            if (bgImage.Opacity >= 1)
                            {
                                title.Visible = false;
                                text.Visible = false;
                                ++imageState_0setup_1in_2view_3out;
                            }
                            break;

                         case 2:
                            if (currentImageTimeMs >= 6000)
                            {
                                ++imageState_0setup_1in_2view_3out;
                            }
                            break; 
                        
                        case 3:

                            bgImage.Opacity -= 2f * Ref.DeltaTimeSec;
                            if (bgImage.Opacity <= 0)
                            {
                                ++currentTexture;
                                if (currentTexture < textures.Length)
                                {
                                    imageState_0setup_1in_2view_3out = 0;
                                }
                                else
                                {
                                    DeleteMe();
                                    return true;
                                }
                            }
                            break;

                    }
                    break;
            }

            currentImageTimeMs +=  Ref.DeltaTimeMs;
            stateTime -= Ref.DeltaTimeMs;
            if (stateTime <= 0)
            {
                ++state;
                switch (state)
                {                    
                    case 2:
                        Ref.music.PlayLoaded();
                        title.Visible = true;
                        stateTime = 500;
                        break;
                    case 3:
                        text.Visible = true;
                        stateTime = 4000;
                        break;

                    case 4:

                        stateTime = float.MaxValue;
                        break;
                }
            }

            if (title.Visible)
            {
                title.Opacity += 2f * Ref.DeltaTimeSec;                    
            }

            if (text.Visible)
            {
                text.Opacity += 2f * Ref.DeltaTimeSec;
            }

            return false;
        }

        public void DeleteMe()
        {
            blackout.DeleteMe();
            bgImage?.DeleteMe();

            title.DeleteMe();
            text.DeleteMe();
        }
    }
}
