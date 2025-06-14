using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using VikingEngine.PJ;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.SteamWrapping;
using Valve.Steamworks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// The first state for DSS, will load all content
    /// </summary>
    class IntroState : Engine.GameState
    {        
        bool isReset;
        Graphics.TextG pressStartText;

        bool loadingContentComplete = false;
        bool loadingDataComplete = false;
        //bool bStorageReady = false;
        WaitForCloudSynch  waitForCloudSynch = new WaitForCloudSynch();

        public IntroState(bool isReset)
            : base()
        {
            this.isReset = isReset;

            Ref.draw.ClrColor = Color.Black;

            pressStartText = new Graphics.TextG(
                LoadedFont.Regular, new Vector2(Engine.Screen.Width * 0.5f, Engine.Screen.Height * 0.85f), new Vector2(Engine.Screen.TextSize * 2f),
                Align.CenterAll, "Loading...", Color.White, ImageLayers.Lay4);
                        
            DssVar.UpdateConstants();

            if (isReset)
            {
                loadingDataComplete = true;
                loadingContentComplete = true;
            }
            else
            {
                if (PlatformSettings.DebugLevel > BuildDebugLevel.Dev)
                {
                    new EngineSpace.DebugExtensions.SentryReport();
                }
                if (Ref.music == null)
                {
                    Ref.music = new Sound.MusicPlayer();
                }
                Engine.ParticleHandler.Init();
                new VikingEngine.Engine.LoadBaseTextures();
               
                new Timer.AsynchActionTrigger(asynchContentLoading);
                new Timer.AsynchActionTrigger(asynchStorageLoading);

                new MapSettings();
                Map.Tile.Init();
            }
        }

        
        void asynchContentLoading()
        {
            Config.OnStartUp();

            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
                    LoadedTexture.particle3,
                    });
            new SpriteSheet();
            LootFest.Data.Block.Init();
            FlagAndColor.Init();
            ItemPropertyColl.Init();
            WorkLib.Init();
            DssRef.ambience = new Ambience();
            DssRef.ambience.contentLoad();

            new Models().loadContent();

            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.sphere, Engine.LoadContent.ModelPath + "sphere");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareDotted, Engine.LoadContent.ModelPath + "SelectSquareDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareSolid, Engine.LoadContent.ModelPath + "SelectSquareSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleDotted, Engine.LoadContent.ModelPath + "SelectCircleDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleSolid, Engine.LoadContent.ModelPath + "SelectCircleSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleThick, Engine.LoadContent.ModelPath + "SelectCircleThick");
            DSSWars.DrawGame.LoadContent();

            SoundLib.LoadContent();
            Engine.LoadContent.LoadSteamVersion();
            

            VikingEngine.HUD.Gui.LoadContent();

            //Display.AbsBubbleMessage.Init();

            DataStream.FilePath.CreateStorageFolder(LootFest.Editor.DesignerStorage.UserVoxelObjFolder);
            UserGeneratedContent.UGClib.GameInit();
            

            loadingContentComplete = true;
            //new Timer.Action0ArgTrigger(createStartButton);
        }

        void asynchStorageLoading()
        {
            FlagDesign.Init();

            DssRef.storage = new Data.GameStorage();
            DssRef.storage.Load();

            DssRef.storage.meta.CreateImportFolders();
            Ref.gamesett.Load();
            new Display.Translation.Translation().setupLanguage(true);

            loadingDataComplete = true;
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            DssRef.models?.sychLoading();

            if (waitForCloudSynch.update())
            {
                if (loadingContentComplete && loadingDataComplete)
                {
#if PCGAME
                    Engine.Screen.ApplyScreenSettings();
#endif

                    Ref.main.criticalContentIsLoaded = true;
                    new Achievements();
                    new GameStats();

                    if (Ref.gamesett.language == LanguageType.NONE)
                    {
                        new SelectLanguageMenu();
                    }
                    else
                    {
                        new GameState.ExitGamePlay();
                    }
                }
            }
        }

    }
}
