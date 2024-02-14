using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Graphics;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars
{
    /// <summary>
    /// The first state for semi-rts, will load all content
    /// </summary>
    class IntroState : Engine.GameState
    {
        
        bool isReset;
        Graphics.TextG pressStartText;

        bool loadingContentComplete = false;
        bool loadingDataComplete = false;
        //bool bStorageReady = false;

        public IntroState(bool isReset)
            : base()
        {
            this.isReset = isReset;

            Ref.draw.ClrColor = Color.Black;

            pressStartText = new Graphics.TextG(
                   LoadedFont.Regular, new Vector2(Engine.Screen.Width * 0.5f, Engine.Screen.Height * 0.85f), new Vector2(Engine.Screen.TextSize * 2f),
                   Align.CenterAll, "Loading...", Color.White, ImageLayers.Lay4);


            if (isReset)
            {
                //bStorageReady = true;
                loadingDataComplete = true;
                loadingContentComplete = true;
            }
            else
            {
                new Display.Translation.Translation();

                //Texture2D introImageTex = Engine.LoadContent.Content.Load<Texture2D>(DssLib.ContentDir + "IntroScene");
                //ImageAdvanced introImage = new ImageAdvanced(SpriteName.NO_IMAGE, Engine.Screen.CenterScreen,
                //   new Vector2(Engine.Screen.Width, (float)Engine.Screen.Width / introImageTex.Width * introImageTex.Height),
                //   ImageLayers.Background9, true);
                //introImage.Texture = introImageTex;
                //introImage.SetFullTextureSource();
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

                Map.Tile.Init();
            
                //new Network.Session();
            }
        }

        
        void asynchContentLoading()
        {
            //Engine.LoadContent.LoadTextures(new List<LoadedTexture> { LoadedTexture.LF3Tiles, LoadedTexture.WhiteArea, });
            //Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, VikingEngine.LootFest.LfLib.ContentFolder + "Lf3Tiles2");
            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");
            new SpriteSheet();
            LootFest.Data.Block.Init();
            ProfileData.Init();
            FlagDesign.Init();
            new GameObject.AllUnits();
            new Models().loadContent();

            //new GameObject.AllUnits();
            //string ModelsDir = "Model" + DataStream.FilePath.Dir;
            //Engine.LoadContent.LoadTexture(LoadedTexture.rtsTiles, RTSlib.ContentDir + "rtsTiles");
            //new LoadTiles();
            //Engine.LoadContent.LoadTexture(LoadedTexture., RtsDir + "rtsTiles");


            //Engine.LoadContent.LoadMesh(LoadedMesh.move_arrow, ModelsDir + "move_arrow");
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
            //Engine.LoadContent.LoadSound(LoadedSound.buy, DssLib.ContentDir + "buy");
            //Engine.LoadContent.LoadSound(LoadedSound.HitMelee, DssLib.ContentDir + "Melee");
            //Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, DssLib.ContentDir + "Button_Clicked");
            //Engine.LoadContent.LoadSound(LoadedSound.MenuMove, DssLib.ContentDir + "Cursor_Moved");
            ////Engine.LoadContent.LoadSound(LoadedSound.UnitHighlight, RTSlib.ContentDir + "menumove");
            //Engine.LoadContent.LoadSound(LoadedSound.MenuBack, DssLib.ContentDir + "Returning");

            ////Engine.LoadContent.LoadSound(LoadedSound.SelectUnit, RTSlib.ContentDir + "hi_100ms");
            ////Engine.LoadContent.LoadSound(LoadedSound.UnselectUnit, RTSlib.ContentDir + "lo_100ms");

            //Engine.LoadContent.LoadSound(LoadedSound.MenuHi100MS, DssLib.ContentDir + "hi_100ms");
            //Engine.LoadContent.LoadSound(LoadedSound.MenuLo100MS, DssLib.ContentDir + "lo_100ms");
            //Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo, DssLib.ContentDir + "out_of_ammo");
            //Engine.LoadContent.LoadSound(LoadedSound.Heal, DssLib.ContentDir + "Heal");

            VikingEngine.HUD.Gui.LoadContent();

            //Display.AbsBubbleMessage.Init();

            DataStream.FilePath.CreateStorageFolder(LootFest.Editor.DesignerStorage.UserVoxelObjFolder);


            loadingContentComplete = true;
            //new Timer.Action0ArgTrigger(createStartButton);
        }

        void asynchStorageLoading()
        {
            new Players.GameStorage().Load();
            loadingDataComplete = true;
        }


        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            DssRef.models?.sychLoading();


            if (loadingContentComplete && loadingDataComplete)
            {
#if PCGAME
                Engine.Screen.ApplyScreenSettings();
#endif
                    
                Ref.main.criticalContentIsLoaded = true;

                new GameState.ExitGamePlay();
                //TestFileSaving();
            }
        }

    }
}
