using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;
using VikingEngine.Engine;
using Microsoft.Xna.Framework.Media;
using VikingEngine.Input;
using VikingEngine.SteamWrapping;

namespace VikingEngine.PJ
{
    class StartScreen : Engine.GameState//, DataStream.IStreamIOCallback
    {
        //public static int SignedInPLayer;
        bool isReset;
        bool contentLoadingComplete = false;
        //bool storageLoadingComplete = false;

        public StartScreen(bool isReset)
            : base()
        {
            this.isReset = isReset;
            
            new Timer.AsynchActionTrigger(asynchContentLoading, false);
            if (isReset)
            {
                //storageLoadingComplete = true;
            }
            else
            {
#if XBOX
                Ref.xbox.onGameStartup();
#endif

                
            }
        }

        void asynchContentLoading()
        {
            if (PlatformSettings.BlueScreen)
            {
                try
                {
                    tryAsynchContentLoading();
                }
                catch (Exception e)
                {
                    new DebugExtensions.TheadedCrash(e);
                }
            }
            else
            {
                tryAsynchContentLoading();
            }
        }

        void tryAsynchContentLoading()
        {
            if (!isReset)
            {
                Engine.LoadContent.LoadSteamVersion();
                Engine.LoadContent.LoadTexture(LoadedTexture.BirdJoustBG, PjLib.ContentFolder + "joust_bg");
                Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");

                Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
                Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");

                Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, PjLib.ContentFolder + "Button_Clicked");
                Engine.LoadContent.LoadSound(LoadedSound.MenuBack, PjLib.ContentFolder + "Returning");
                Engine.LoadContent.LoadSound(LoadedSound.Coin1, PjLib.ContentFolder + "coin1");
                Engine.LoadContent.LoadSound(LoadedSound.Coin2, PjLib.ContentFolder + "coin2");
                Engine.LoadContent.LoadSound(LoadedSound.Coin3, PjLib.ContentFolder + "coin3");
                Engine.LoadContent.LoadSound(LoadedSound.shieldcrash, PjLib.ContentFolder + "shieldcrash");
                Engine.LoadContent.LoadSound(LoadedSound.smack, PjLib.ContentFolder + "smack");
                Engine.LoadContent.LoadSound(LoadedSound.SmackEchoes, PjLib.ContentFolder + "SmackEchoes");

                Engine.LoadContent.LoadSound(LoadedSound.flap, PjLib.ContentFolder + "flap");
                Engine.LoadContent.LoadSound(LoadedSound.flowerfire, PjLib.ContentFolder + "flowerfire");
                Engine.LoadContent.LoadSound(LoadedSound.minefire, PjLib.ContentFolder + "minefire");
                Engine.LoadContent.LoadSound(LoadedSound.MenuNotAllowed, PjLib.ContentFolder + "Not_Allowed");

                Engine.LoadContent.LoadSound(LoadedSound.birdToasty, PjLib.ContentFolder + "toasty");
                Engine.LoadContent.LoadSound(LoadedSound.violin_pluck, PjLib.ContentFolder + "violin_pluck");
                Engine.LoadContent.LoadSound(LoadedSound.bass_pluck, PjLib.ContentFolder + "bass_pluck");
                Engine.LoadContent.LoadSound(LoadedSound.birdTimesUp, PjLib.ContentFolder + "times_up");
                Engine.LoadContent.LoadSound(LoadedSound.wolfScare, PjLib.ContentFolder + "jump_scare");
                Engine.LoadContent.LoadSound(LoadedSound.bassdrop, PjLib.ContentFolder + "Bassbomb");
                
                Ref.music = new Sound.MusicPlayer();
                PjRef.JoustSong = new Sound.SongData(PjLib.ContentFolder + "Hemisphere Three", "Standard Joust", false, 1f);
                PjRef.JoustSong.LoadAndStore();
                PjRef.LobbySong = new Sound.SongData(PjLib.ContentFolder + "Elevating", "Lobby", true, 0.3f);
                PjRef.LobbySong.LoadAndStore();


                VikingEngine.HUD.Gui.LoadContent();

                new LoadBaseTextures();
                new SpriteSheet();
                new Network.Session();
                PjRef.Init();

                new Storage();
                new PjEngine.Achievements();
            }

            Ref.main.criticalContentIsLoaded = true;
            contentLoadingComplete = true;
        }
        

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            
            if (contentLoadingComplete)
            {
                start();
            }
        }

        //public void SaveComplete(bool save, int player, bool completed, byte[] value)
        //{
        //    storageLoadingComplete = true;
        //}



        void start()
        {
#if PCGAME
            Engine.Screen.ApplyScreenSettings();
#endif
            //Engine.XGuide.GetPlayer(SignedInPLayer).IsActive = true;
            //Engine.XGuide.LocalHostIndex = SignedInPLayer;

            //if (PlatformSettings.DevBuild)
            {
                //new MiniGolf.MinigolfState();
                //    //new SpaceWar.SpacePlayState();
                //    //new GameState.WolfScare();
                //    //new Strategy.Editor();
                //    //new Story.LoadMapState();//Story.StoryPlayState();
                //    new Bagatelle.BagatellePlayState(null, 0);
            }
            
            new LobbyState();
        }

    }
}
