using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG
{
    class IntroState : Engine.GameState
    {
        bool isReset;
        bool loadingContentComplete = false;

        public IntroState(bool isReset)
            : base()
        {
            this.isReset = isReset;

            Graphics.TextG title = new Graphics.TextG(
               LoadedFont.Regular, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.TextSize * 2f),
               new Align(Dir8.S), PlatformSettings.GameTitle, Color.Orange, ImageLayers.Lay4);
            Graphics.TextG company = new TextG(
                LoadedFont.Regular, Engine.Screen.CenterScreen, new Vector2(Engine.Screen.TextSize * 1f),
                new Align(Dir8.N), "vikingfabian.com", Color.Yellow, ImageLayers.Lay1);

            //new Timer.AsynchActionTrigger(asynchContentLoading);
            new StorageTask(asynchContentLoading, true, onContentLoadComplete);

            Engine.Draw.horizontalSplit = !Engine.Screen.IsHighDefinition();
            Engine.ParticleHandler.Init();

            //if (!isReset)
            //{
            //    new Analytics();
            //}

           // host = 1;
        }

        void asynchContentLoading(MultiThreadType type)
        {
            if (!isReset)
            {
                Engine.LoadContent.LoadSteamVersion();

                Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
                new VikingEngine.Engine.LoadBaseTextures();
                new VikingEngine.SpriteSheet();
                string dir = toggLib.ContentFolder;

                VikingEngine.HUD.Gui.LoadContent();                

                Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
                Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
                Engine.LoadContent.LoadMesh(LoadedMesh.sphere, Engine.LoadContent.ModelPath + "sphere");

                //sound
                Engine.LoadContent.LoadSound(LoadedSound.buy, dir + "buy");
                Engine.LoadContent.LoadSound(LoadedSound.Sword1, dir + "sword1");
                Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, dir + "Button_Clicked");
                Engine.LoadContent.LoadSound(LoadedSound.MenuMove, dir + "Cursor_moved");
                Engine.LoadContent.LoadSound(LoadedSound.MenuBack, dir + "Returning");

                new Storage.GameStorage();

                
                new ToggEngine.Data.Achievements();

                MainTerrainProperties.Init();
                new SquareDic();
                VisualTerrainData.Init();
                Commander.BattleLib.Init();
                HeroQuest.hqLib.Init();
                new InputMap(0);
                Ref.music = new Sound.MusicPlayer();
                toggLib.Init();
                HeroQuest.MapGen.MapSpawnLib.Init();
                new HeroQuest.Data.LocalPlayersSetup();
                Ref.gamesett.Load();
            }
            
        }

        void onContentLoadComplete()
        {
            Ref.main.criticalContentIsLoaded = true;
            loadingContentComplete = true;

            Ref.culture = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
            //new DataLib.Language(toggLib.ContentFolder + "Language\\", "English");
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingContentComplete )//&& Ref.language.loadComplete)//loadingContentComplete)
            {
                Engine.Screen.ApplyScreenSettings();
                if (!isReset)
                {
                    new Network.Session();
                }
                Engine.XGuide.LocalHost.IsActive = true;
                
                if (Ref.netSession.GotSessionId)
                {
                    //Join Invite
                    new HeroQuest.Lobby.LobbyState(false, null);
                }
                else
                {
                    new GameState.MainMenuState();
                }
            }
        }
    }
}