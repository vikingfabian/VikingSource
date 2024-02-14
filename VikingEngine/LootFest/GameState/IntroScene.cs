using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
////xna


namespace VikingEngine.LootFest.GameState
{
    class IntroScene : Engine.GameState
    {
        bool loadingContentComplete = false;
        bool loadingDataComplete = false;
        bool isReset;
        Graphics.ImageAdvanced introImage;

        const int LoadingAnimationDotCount = 7;
        Time startLoadingAnimation = new Time(1f, TimeUnit.Seconds);
        Time nextBumpTimer;
        CirkleCounterUp currentLoadingAnimationBump = new CirkleCounterUp(0, LoadingAnimationDotCount - 1);
        Graphics.Image[] loadingAnimation = null;

        public IntroScene(bool isReset)
            : base()
        {
            //ushort mat = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColorIx(Color.GreenYellow, 1);

            //Color conv = VikingEngine.LootFest.Map.HDvoxel.BlockHD.ToColor(mat);

            this.isReset = isReset;
            
           
            Engine.XGuide.LocalHostIndex = 0;
            new Timer.AsynchActionTrigger(asynchStorageLoading);

            IntVector2 testScreen = new IntVector2(7, 15);
            Map.WorldPosition wp = new Map.WorldPosition(testScreen, true);
            if (wp.ChunkGrindex != testScreen)
            {
                throw new Exception("Wp setting are incorrect, check bit size");
            }
            
            if (isReset)
            {
                LfRef.storage.resetSelectStorageLock();
                loadingContentComplete = true;
            }
            else
            {
                new LoadBaseTextures();
                //new NetManager();
                new NetLobby2();
                draw.ClrColor = Color.Black;
                Texture2D introImageTex = Engine.LoadContent.Content.Load<Texture2D>(LfLib.ContentFolder + "IntroSceneLF");
                introImage = new ImageAdvanced(SpriteName.NO_IMAGE, Engine.Screen.CenterScreen,
                   new Vector2(Engine.Screen.Width, (float)Engine.Screen.Width / introImageTex.Width * introImageTex.Height),
                   ImageLayers.Background9, true);
                introImage.Texture = introImageTex;
                introImage.SetFullTextureSource();


                if (doneLoading)
                {
                    createStartButton();
                }
                else
                {
                    Task.Factory.StartNew(asynchContentLoading);
                    //Thread t = new Thread(asynchContentLoading);
                    //t.Name = "Load content";
                    //t.Start();
                }
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

            loadingContentComplete = true;
            new Timer.Action0ArgTrigger(createStartButton);
        }

        void tryAsynchContentLoading()
        {
            SoundLib.LoadMusic();

            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
                    LoadedTexture.square_particle,
                    });
            Engine.LoadContent.LoadTexture(LoadedTexture.BlockTextures, LfLib.ContentFolder + "lf3blockTex");

            Engine.LoadContent.LoadTexture(LoadedTexture.SpriteSheet, Engine.LoadContent.TexturePath + "Lf3Tiles2");

            new SpriteSheet();
            new BoundManager();

            LfRef.Images.Init();//mt
            Data.BlockTextures.InitMaterials();
            Data.Block.Init();//mt
            VikingEngine.LootFest.Map.HDvoxel.BlockPatternMaterialsLib.Init();
            LfLib.Init();
            LfRef.Images.LoadStandardVobjects();
            createFolderStructure();

            LfRef.blockmaps.SaveLoad(false, false);
            
            Engine.LoadContent.LoadSteamVersion();
            SoundLib.LoadSound();

            // Load meshes
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");
            Engine.LoadContent.LoadMesh(LoadedMesh.sphere, Engine.LoadContent.ModelPath + "sphere");
           // Engine.LoadContent.LoadMesh(LoadedMesh.skydome, LfLib.ContentFolder + "skydome");
           // Engine.LoadContent.LoadMesh(LoadedMesh.cone45deg, LfLib.ContentFolder + "cone45deg");

            Map.PreparedFace.Init();
            Process.ModifiedImage.Init();
            Engine.ParticleHandler.Init();
            Voxels.FaceCornerColorYS.Init();
            VikingEngine.LootFest.Map.HDvoxel.AppearanceMaterial.Init();

            AbsHUD2.InitHUD();
        }

        void createFolderStructure()
        {
            DataStream.FilePath.CreateStorageFolder(LfLib.OverrideModelsFolder);
            DataStream.FilePath.CreateStorageFolder(Editor.DesignerStorage.UserVoxelObjFolder);

            for (int i = 0; i < VikingEngine.LootFest.Players.PlayerStorageGroup.FilesCount; ++i)
            {
                DataStream.FilePath.CreateStorageFolder(VikingEngine.LootFest.Players.PlayerStorageGroup.FileFolderName(i, true));
            }
            DataStream.FilePath.CreateStorageFolder(VikingEngine.LootFest.Players.PlayerStorageGroup.FileFolderName(0, false));

            LootFest.Editor.DesignerStorage.InitFolderStructure();
        }

        void loadingThreadException(Exception e)
        {
#if PCGAME
            //System.Windows.Forms.MessageBox.Show("Press Ctrl+C to copy this message: " + e.Message + " @" + e.StackTrace, "Lootfest Content Load Crash",
            //    System.Windows.Forms.MessageBoxButtons.OK,  System.Windows.Forms.MessageBoxIcon.Error);
#endif
        }

        void asynchStorageLoading()
        {
            new Players.PlayerStorageGroup();
            loadingDataComplete = true;
        }

        static bool doneLoading = false;
        bool bEndInitOnMainThread = false;
        void endInitOnMainThread()
        {
            if (PlatformSettings.DebugPerformanceText && !isReset)
            {
                Debug.createOutputWin();
            }
            doneLoading = true;
            GO.AbsVoxelObj.MakeTempImage();
            Graphics.LFHeightMap.InitEffect();
            bEndInitOnMainThread = true;
        }

        bool bStartText = false;
        void createStartButton()
        {
            if (!bStartText)
            {
                bStartText = true;
                float blockSize = (int)(Engine.Screen.Height * 0.07f);
                string text = "press start";
                float w = text.Length * blockSize;
                Vector2 letterPos = new Vector2(Engine.Screen.CenterScreen.X - w * PublicConstants.Half, Engine.Screen.Height * 0.82f);
                float timeDelay = 0;

                foreach (char c in text)
                {
                    Data.MaterialType m = lib.LetterBlockFromChar(c);
                    SpriteName tile = Data.BlockTextures.MaterialTile((byte)m);
                    Image img = new Image(tile, letterPos, new Vector2(blockSize), ImageLayers.Background4);
                    letterPos.X += blockSize;

                    img.Opacity = 0;
                    Graphics.Motion2d fadeIn = new Graphics.Motion2d(MotionType.OPACITY, img, Vector2.One, MotionRepeate.NO_REPEAT, 33 * 20, false);
                    new Timer.UpdateTrigger(timeDelay, fadeIn, true);
                }
            }
        }

        
        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (loadingContentComplete && loadingDataComplete)
            {
                Ref.main.criticalContentIsLoaded = true;
                if (!bEndInitOnMainThread)
                {
                    endInitOnMainThread();
                }

                Engine.XGuide.LocalHost.IsActive = true;

                bool xboxShortcut = Input.XInput.Instance(0).IsButtonDown(Buttons.LeftTrigger);
                if (xboxShortcut || Input.Keyboard.Alt)
                {
                    bool short2 = Input.Keyboard.Ctrl;

                    if (xboxShortcut)
                    {
                        XGuide.LocalHost.inputMap = new LootFest.Players.InputMap(XGuide.LocalHost.localPlayerIndex);
                        XGuide.LocalHost.inputMap.xboxSetup();
                        short2 = Input.XInput.Instance(0).IsButtonDown(Buttons.RightTrigger);
                    }

                    if (short2)
                    {
                        new VikingEngine.LootFest.BlockMap.EditorState();
                    }
                    else
                    {
                        new GameState.VoxelDesignState(XGuide.LocalHostIndex);
                    }
                }
                else
                {
                    //if (PlatformSettings.DevBuild && DebugSett.QuickStartCardGame)
                    //{
                    //    new CardGame.CardPlayState();
                    //}
                    //else 
                    if (PlatformSettings.DevBuild && DebugSett.BlockMapEditor)
                    {
                        new BlockMap.EditorState();
                    }
                    else
                    {
                        new GameState.LoadingMap(new Data.WorldData(true));
                    }
                }
            }
            

            if (Ref.music != null && !Ref.music.playingFromPlayList)
            {
                Ref.music.Update();
            }

            if (loadingContentComplete)
            {
                if (loadingAnimation != null)
                {
                    foreach (var img in loadingAnimation)
                    {
                        img.DeleteMe();
                    }
                    loadingAnimation = null;
                }
            }
            else
            {
                if (loadingAnimation == null)
                {
                    if (startLoadingAnimation.CountDown())
                    {
                        loadingAnimation = new Image[LoadingAnimationDotCount];

                        Vector2 sz = new Vector2(Convert.ToInt32(Engine.Screen.IconSize * 0.16f));
                        Vector2 pos = new Vector2(Engine.Screen.Width * 0.5f, Engine.Screen.Height * 0.80f);
                        for (int i = 0; i < LoadingAnimationDotCount; ++i)
                        {
                            loadingAnimation[i] = new Image(SpriteName.WhiteArea, pos, sz, ImageLayers.Lay9, true);
                            loadingAnimation[i].Xpos += (i + -(LoadingAnimationDotCount - 1) * 0.5f) * sz.X * 3f;
                        }
                    }
                }
                else
                {
                    if (nextBumpTimer.CountDown())
                    {
                        nextBumpTimer.MilliSeconds = 70;
                        new Graphics.Motion2d(MotionType.SCALE, loadingAnimation[currentLoadingAnimationBump.Value], 
                            loadingAnimation[currentLoadingAnimationBump.Value].Size * 1.2f, MotionRepeate.BackNForwardOnce, 160, true);
                        currentLoadingAnimationBump.Next();
                    }
                }
            }

            LfRef.net.update();
        }


    }
}
