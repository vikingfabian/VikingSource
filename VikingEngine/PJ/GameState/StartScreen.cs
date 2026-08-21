using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Input;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO.Characters;
using VikingEngine.Sound;
using VikingEngine.SteamWrapping;

namespace VikingEngine.PJ
{
    class StartScreen : Engine.LaunchState//, IStreamIOCallback
    {
        //public static int SignedInPLayer;
        bool isReset;
        bool contentLoadingComplete = false;
        //bool storageLoadingComplete = false;

        public StartScreen(bool isReset)
            : base(isReset)
        {
//            this.isReset = isReset;
            
//            new Timer.AsynchActionTrigger(asynchContentLoading, false);
//            if (isReset)
//            {
//                //storageLoadingComplete = true;
//            }
//            else
//            {
//#if XBOX
//                Ref.xbox.onGameStartup();
//#endif

                
//            }
        }

        protected override void preLoading()
        {
            if (PlatformSettings.DebugLevel > BuildDebugLevel.Dev &&
                !Config.BlockSentry)
            {
                new EngineSpace.DebugExtensions.SentryReport();
            }
            //DssVar.UpdateConstants();

            if (Ref.music == null)
            {
                Ref.music = new Sound.MusicPlayer();
            }
            //Engine.ParticleHandler.Init();
            new VikingEngine.Engine.LoadBaseTextures();

        }

        override protected void asyncContentLoading(ref int part)
        {

            Engine.LoadContent.LoadSteamVersion();
            Engine.LoadContent.LoadTexture(LoadedTexture.BirdJoustBG, PjLib.ContentFolder + "joust_bg");
            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            part++;
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
            part++;
            Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, PjLib.ContentFolder + "Button_Clicked");
            Engine.LoadContent.LoadSound(LoadedSound.MenuBack, PjLib.ContentFolder + "Returning");
            Engine.LoadContent.LoadSound(LoadedSound.Coin1, PjLib.ContentFolder + "coin1");
            Engine.LoadContent.LoadSound(LoadedSound.Coin2, PjLib.ContentFolder + "coin2");
            Engine.LoadContent.LoadSound(LoadedSound.Coin3, PjLib.ContentFolder + "coin3");
            Engine.LoadContent.LoadSound(LoadedSound.shieldcrash, PjLib.ContentFolder + "shieldcrash");
            Engine.LoadContent.LoadSound(LoadedSound.smack, PjLib.ContentFolder + "smack");
            Engine.LoadContent.LoadSound(LoadedSound.SmackEchoes, PjLib.ContentFolder + "SmackEchoes");
            part++;
            Engine.LoadContent.LoadSound(LoadedSound.flap, PjLib.ContentFolder + "flap");
            Engine.LoadContent.LoadSound(LoadedSound.flowerfire, PjLib.ContentFolder + "flowerfire");
            Engine.LoadContent.LoadSound(LoadedSound.minefire, PjLib.ContentFolder + "minefire");
            Engine.LoadContent.LoadSound(LoadedSound.MenuNotAllowed, PjLib.ContentFolder + "Not_Allowed");
            part++;
            Engine.LoadContent.LoadSound(LoadedSound.birdToasty, PjLib.ContentFolder + "toasty");
            Engine.LoadContent.LoadSound(LoadedSound.violin_pluck, PjLib.ContentFolder + "violin_pluck");
            Engine.LoadContent.LoadSound(LoadedSound.bass_pluck, PjLib.ContentFolder + "bass_pluck");
            Engine.LoadContent.LoadSound(LoadedSound.birdTimesUp, PjLib.ContentFolder + "times_up");
            Engine.LoadContent.LoadSound(LoadedSound.wolfScare, PjLib.ContentFolder + "jump_scare");
            Engine.LoadContent.LoadSound(LoadedSound.bassdrop, PjLib.ContentFolder + "Bassbomb");
            part++;

            SoundManager.whip = new SoundContainerSingle(PjLib.SoundFolder + "whip_normal", 0.7f);
            SoundManager.whip_perfect = new SoundContainerSingle(PjLib.SoundFolder + "whip_perfect", 0.7f);
            SoundManager.whip_fail = new SoundContainerSingle(PjLib.SoundFolder + "whip_fail", 0.7f);
            SoundManager.success = new SoundContainerSingle(PjLib.SoundFolder + "Heal", 0.7f);
            SoundManager.scroll_back = new SoundContainerSingle(PjLib.SoundFolder + "scroll_back", 0.8f);
            SoundManager.scroll_forward = new SoundContainerSingle(PjLib.SoundFolder + "scroll_forward", 0.8f);
            SoundManager.option_select = new SoundContainerSingle(PjLib.SoundFolder + "option_select", 0.2f);
            SoundManager.option_deselect = new SoundContainerSingle(PjLib.SoundFolder + "option_deselect", 0.7f);
            SoundManager.soft_buzz_error = new SoundContainerSingle(PjLib.SoundFolder + "soft_buzz_error", 0.1f);


            //Ref.music = new Sound.MusicPlayer();
            PjRef.JoustSong = new Sound.SongData(PjLib.ContentFolder + "Hemisphere Three", "Standard Joust", null, false, 1f);
            PjRef.JoustSong.LoadAndStore();
            PjRef.LobbySong = new Sound.SongData(PjLib.ContentFolder + "Elevating", "Lobby", null, true, 0.3f);
            PjRef.LobbySong.LoadAndStore();
            part++;

            VikingEngine.HUD.Gui.LoadContent();
            part++;
            new LoadBaseTextures();
            new SpriteSheet();
            HudLib.Init();
            part++;

            PjRef.Init();
            part++;
            
            new PjEngine.Achievements();
            part++;
        }

        protected override void asyncDataProcessLoading()
        {
            
        }

        protected override void asyncStorageLoading(ref int part)
        {
            new Storage();
            part++;
        }

        protected override void asyncLoading_OnRestart(ref int part)
        {
            
        }

        protected override void asyncLoadIntro()
        {

            try
            {
                introSound = null;//new SoundContainerSingle(SoundLib.SoundDir + "intro_beat", 0.5f);
            }
            catch (Exception ex)
            {
                //SoundManager.OnLaunchException(ex);

                introSound = null;
            }

        }

        protected override void launch()
        {   
           
#if PCGAME
            Engine.Screen.ApplyScreenSettings();
#endif
            //Engine.XGuide.GetPlayer(SignedInPLayer).IsActive = true;
            //Engine.XGuide.LocalHostIndex = SignedInPLayer;

            //if (PlatformSettings.DevBuild)
#if DEBUG
            {
                new StupidHorse.StupidHorseScene(new List2<GamerData> {
                    new GamerData() { button = new KeyboardButtonMap(
                    Microsoft.Xna.Framework.Input.Keys.Space), joustAnimal = JoustAnimal.Pig1, hat =  Hat.NoHat  },

                    new GamerData() { button = new KeyboardButtonMap(
                    Microsoft.Xna.Framework.Input.Keys.Right), joustAnimal = JoustAnimal.Bird1, hat =  Hat.NoHat  }
                }, 0);
                return;
                //new MiniGolf.MinigolfState();
                //    //new SpaceWar.SpacePlayState();
                //    //new GameState.WolfScare();
                //    //new Strategy.Editor();
                //    //new Story.LoadMapState();//Story.StoryPlayState();
                //    new Bagatelle.BagatellePlayState(null, 0);

            }
#endif
            new LobbyState();
        }

    }
}
