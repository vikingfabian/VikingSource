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

            Ref.music = new Sound.MusicPlayer();

            if (isReset)
            {
                //bStorageReady = true;
                loadingDataComplete = true;
                loadingContentComplete = true;
            }
            else
            {
                

                
                Engine.ParticleHandler.Init();
                new VikingEngine.Engine.LoadBaseTextures();
               
                //const string MusicFolder = DssLib.ContentDir + "Music\\";
                //Ref.music.SetPlaylist(new List<Sound.SongData>
                //{
                //    new Sound.SongData(MusicFolder + "BBaaB_loop", true, 1f),
                //    new Sound.SongData(MusicFolder + "Gargoyle_loop", true, 1f),
                //    new Sound.SongData(MusicFolder + "RM 1 - Introversion", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 10 - Incubation", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 2 - Arcane Benevolence", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 3 - Left in Autumn", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 4 - Warhogs", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 5 - Suddenly Empty", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 6 - Auderesne", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 7 - For Eternity", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 8 - Asynchronous Flanking", false, 1f),
                //    new Sound.SongData(MusicFolder + "RM 9 - Weeping Bedlam", false, 1f),
                //    new Sound.SongData(MusicFolder + "YesIAmYourGodHQ", true, 1f),
                //},
                //PlatformSettings.PlayMusic);
                //Ref.music.SetVolume(0.04f);

                new Timer.AsynchActionTrigger(asynchContentLoading);
                new Timer.AsynchActionTrigger(asynchStorageLoading);

                new MapSettings();
                Map.Tile.Init();
            
                //new Network.Session();
            }
        }

        
        void asynchContentLoading()
        {
            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            new SpriteSheet();
            LootFest.Data.Block.Init();
            FlagAndColor.Init();
            
            new GameObject.AllUnits();
            new Models().loadContent();

            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.sphere, Engine.LoadContent.ModelPath + "sphere");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareDotted, Engine.LoadContent.ModelPath + "SelectSquareDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectSquareSolid, Engine.LoadContent.ModelPath + "SelectSquareSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleDotted, Engine.LoadContent.ModelPath + "SelectCircleDotted");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleSolid, Engine.LoadContent.ModelPath + "SelectCircleSolid");
            Engine.LoadContent.LoadMesh(LoadedMesh.SelectCircleThick, Engine.LoadContent.ModelPath + "SelectCircleThick");
            //DSSWars.DrawGame.LoadContent();

            SoundLib.LoadContent();
            Engine.LoadContent.LoadSteamVersion();
            

            VikingEngine.HUD.Gui.LoadContent();

            //Display.AbsBubbleMessage.Init();

            DataStream.FilePath.CreateStorageFolder(LootFest.Editor.DesignerStorage.UserVoxelObjFolder);


            loadingContentComplete = true;
            //new Timer.Action0ArgTrigger(createStartButton);
        }

        void asynchStorageLoading()
        {
            FlagDesign.Init();
            new Data.GameStorage().Load();
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

                    new GameState.ExitGamePlay();
                }
            }
        }

    }
}
