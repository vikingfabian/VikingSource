using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.Network;
using VikingEngine.Sound;
using VikingEngine.SteamWrapping;
using VikingEngine.Voxels;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// The first state for DSS, will load all content
    /// </summary>
    class IntroState : Engine.LaunchState
    {
        Texture2D bgTex;

        bool bSpriteSheetTexture = false;
        bool joinedSession = false;
        public IntroState(bool isReset)
            : base(isReset)
        {
        }
        protected override void preLoading()
        {
            if (PlatformSettings.DebugLevel > BuildDebugLevel.Dev &&
                !Config.BlockSentry)
            {
                new EngineSpace.DebugExtensions.SentryReport();
            }
            DssVar.UpdateConstants();

            if (Ref.music == null)
            {
                Ref.music = new Sound.MusicPlayer();
            }
            Engine.ParticleHandler.Init();
            new VikingEngine.Engine.LoadBaseTextures();


            new MapSettings();
            Map.Tile.Init();
        }

        override protected void asyncContentLoading(ref int part)
        {            
            part++;
            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            bSpriteSheetTexture = true;
            part++;
            Engine.LoadContent.LoadTexture(LoadedTexture.waterEdge, DssLib.ContentDir + "wave_mask1");
            part++;
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
                    LoadedTexture.particle3,
                    });            
            part++;
            DssRef.ambience = new Ambience();
            part++;
            DssRef.ambience.contentLoad();
            part++;

            new Models().loadContent();
            part++;
            ElephantModelBuilder.Init();
            part++;

            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.sphere, Engine.LoadContent.ModelPath + "sphere");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareDotted, Engine.LoadContent.ModelPath + "SelectSquareDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareSolid, Engine.LoadContent.ModelPath + "SelectSquareSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleDotted, Engine.LoadContent.ModelPath + "SelectCircleDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleSolid, Engine.LoadContent.ModelPath + "SelectCircleSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleThick, Engine.LoadContent.ModelPath + "SelectCircleThick");
            EffectVertexColorShadow.LoadContent();
            part++;

            SoundLib.LoadContent();
            part++;
            Engine.LoadContent.LoadSteamVersion();
            part++;

            VikingEngine.HUD.Gui.LoadContent();
            part++;
            DataStream.FilePath.CreateStorageFolder(DesignerStorage.VoxelModelFolder);
            part++;
            DataStream.FilePath.CreateStorageFolder(DesignerStorage.VoxelProjectFolder);
            part++;
            UserGeneratedContent.UGClib.GameContentInit();
            part++;
            bgTex = MainMenuState.LoadBg();
            part++;
        }

        protected override async void asyncDataProcessLoading()
        {
            ConscriptDataLib.Init();
            dataProcessPart++;
            FlagDesign.Init();
            dataProcessPart++;
            Block.Init();
            dataProcessPart++;
            FlagAndColor.Init();
            dataProcessPart++;

            ItemPropertyColl.Init();
            dataProcessPart++;
            WorkLib.Init();
            dataProcessPart++;
            BuildLib.Init();
            dataProcessPart++;
            

            int loops = 0;
            while (!bSpriteSheetTexture)
            {
                if (++loops > 1000)
                {
                    throw new EndlessLoopException("asyncDataProcessLoading part " + dataProcessPart.ToString());
                }
                await Task.Delay(20);                
            }

            new SpriteSheet();
            dataProcessPart++;
            WaterEdgeBuilder.Init();
            dataProcessPart++;
        }

        protected override void asyncLoading_OnRestart(ref int part)
        {
            bgTex = MainMenuState.LoadBg();
            part++;
        }

        protected override void asyncLoadIntro()
        {

            try
            {
                introSound = new SoundContainerSingle(SoundLib.SoundDir + "intro_beat", 0.5f);
            }
            catch (Exception ex)
            {                
                SoundManager.OnLaunchException(ex);
                
                introSound = null;
            }

        }

        override protected void asyncStorageLoading(ref int part)
        {  
            DssRef.storage = new Data.GameStorage();
            DssRef.storage.Load();
            part++;
            DssRef.storage.meta.CreateImportFolders();
            part++;
            new Presentation.Translation().setupLanguage(true);
            part++;
            CasualBuild.Init();
            part++;
        }

        protected override bool tasksComplete()
        {
            return ElephantModelBuilder.WaitingCount <= 0;
        }


        public override void Time_Update(float time)
        {
            DssRef.models?.sychLoading();

            base.Time_Update(time);        
        }

        protected override void launch()
        {
            DssRef.models.rawModels_temporary = null;
            Ref.main.criticalContentIsLoaded = true;
            new Achievements();
            //new GameStats();
            DssRef.stats.startUp.addOne();

            if (Ref.gamesett.language == LanguageType.NONE)
            {
                new SelectLanguageMenu();
            }
            else
            {
#if DEBUG
                //for (int i = 0; i < 1000000; ++i)
                //{
                //    ForXYEdgeLoopRandomPicker loop = new ForXYEdgeLoopRandomPicker();
                //    for (int radius = 10; radius < 14; ++radius)
                //    {
                //        loop.start(Rectangle2.FromCenterTileAndRadius(new IntVector2(10, 10), radius));
                //        while (loop.Next())
                //        {
                //        }
                //    }
                //}
#endif

                //if (Ref.netSession.InMultiplayerSession)
                //{
                //    lib.DoNothing();
                //}

                string[] args = System.Environment.GetCommandLineArgs();
                foreach (string arg in args)
                {
                    if (arg.ToLower() == "+connect_lobby")
                    { 
                        joinedSession = true;
                    }
                }

                if (joinedSession)
                {
                    new ConnectionState();
                }
                else
                {
                    new MainMenuState(bgTex);
                }
            }
        }
        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            base.NetworkStatusMessage(message);
            if (message == Network.NetworkStatusMessage.Joining_session)
            {
                joinedSession = true;
            }
        }

        protected override void createDrawManager()
        {
            draw = new Draw2D();
        }

    }
}
