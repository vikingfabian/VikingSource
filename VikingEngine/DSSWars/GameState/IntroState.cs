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
using VikingEngine.Engine;
using VikingEngine.Voxels;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.Sound;

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
            Config.OnStartUp();

            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
                    LoadedTexture.particle3,
                    });
            new SpriteSheet();
            Block.Init();
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
            EffectVertexColorShadow.LoadContent();

            SoundLib.LoadContent();
            Engine.LoadContent.LoadSteamVersion();
            

            VikingEngine.HUD.Gui.LoadContent();

            DataStream.FilePath.CreateStorageFolder(DesignerStorage.VoxelModelFolder);
            DataStream.FilePath.CreateStorageFolder(DesignerStorage.VoxelProjectFolder);
            UserGeneratedContent.UGClib.GameInit();

            bgTex = LobbyState.LoadBg();
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

            DssRef.storage = new Data.GameStorage();
            DssRef.storage.Load();

            DssRef.storage.meta.CreateImportFolders();
            Ref.gamesett.Load();
            new Presentation.Translation().setupLanguage(true);
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
