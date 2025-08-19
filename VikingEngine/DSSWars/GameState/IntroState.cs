using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Valve.Steamworks;
using VikingEngine.DSSWars.Build;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Map.Settings;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.DSSWars.Resource;
using VikingEngine.DSSWars.Work;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.PJ;
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

        public IntroState(bool isReset)
            : base(isReset)
        {
            //PcgRandom random = new PcgRandom();
            //for (int i = 0; i < 1000000000; i++)
            //{
            //    int result = random.Int(0);
            //    if (result != 0)
            //    {
            //        lib.DoNothing();
            //    }
            //}

        }
        protected override void preLoading()
        {
            if (PlatformSettings.DebugLevel > BuildDebugLevel.Dev)
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
            part++;
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
                    LoadedTexture.particle3,
                    });
            part++;
            new SpriteSheet();
            part++;
            Block.Init();
            part++;
            FlagAndColor.Init();
            part++;
            ItemPropertyColl.Init();
            part++;
            WorkLib.Init();
            part++;
            DssRef.ambience = new Ambience();
            part++;
            DssRef.ambience.contentLoad();
            part++;

            new Models().loadContent();
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
            UserGeneratedContent.UGClib.GameInit();
            part++;
            bgTex = LobbyState.LoadBg();
            part++;

            //DrawGame.LoadContent();
        }

        protected override void asyncLoading_OnRestart(ref int part)
        {
            bgTex = LobbyState.LoadBg();
            part++;
        }

        protected override void asyncLoadIntro()
        {
#if !DEBUG
            introSound = new SoundContainerSingle(SoundLib.SoundDir + "intro_beat", 0.7f);
#endif
        }

        override protected void asyncStorageLoading(ref int part)
        {
            FlagDesign.Init();
            part++;
            DssRef.storage = new Data.GameStorage();
            DssRef.storage.Load();
            part++;
            DssRef.storage.meta.CreateImportFolders();
            part++;
            Ref.gamesett.Load();
            part++;
            new Presentation.Translation().setupLanguage(true);
            part++;

            BuildLib.Init();
            part++;
            CasualBuild.Init();
            part++;
        }


        public override void Time_Update(float time)
        {
            DssRef.models?.sychLoading();

            base.Time_Update(time);        
        }

        protected override void launch()
        {

            Ref.main.criticalContentIsLoaded = true;
            new Achievements();
            new GameStats();

            if (Ref.gamesett.language == LanguageType.NONE)
            {
                new SelectLanguageMenu();
            }
            else
            {
                new LobbyState(bgTex);
            }
        }


        protected override void createDrawManager()
        {
            draw = new Draw2D();
        }
    }
}
