using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

using System.Threading;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;


namespace VikingEngine.LF2.GameState
{
    class IntroScene : Engine.GameState, DataLib.IStorageSelectionComplete, IDialogueCallback
    {
        List<AbsDraw> backGimages;
        Graphics.ImageAdvanced introImage;

        public IntroScene()
            : base()
        {
            draw.ClrColor = Color.Black;
            Texture2D introImageTex = Engine.LoadContent.Content.Load<Texture2D>(LootfestLib.ContentFolder + "IntroSceneLF");
            introImage = new ImageAdvanced(SpriteName.NO_IMAGE, Engine.Screen.CenterScreen,
               new Vector2(Engine.Screen.Width, (float)Engine.Screen.Width / introImageTex.Width * introImageTex.Height),
               ImageLayers.Background9, true);
            introImage.Texture = introImageTex;
            introImage.SetFullTexureSource();

            if (doneLoading)
            {
                createStartButton();
            }
            else
            {
                Thread t = new Thread(loadingThread);
                t.Name = "Load content";
                t.Start();
            }
        }

        void loadingThread()
        {
            if (PlatformSettings.PlayMusic)
                Music.MusicDirector.PlayIntroSong();

            Engine.XGuide.UpdateTrialCheck();
            new Network.Session();
            LanguageManager.Init();

            //texturerna borde kunna multitrådas med infade av text
            Engine.LoadContent.LoadTextures(new List<LoadedTexture> {
               LoadedTexture.LFTiles,
               LoadedTexture.WhiteArea,
              // LoadedTexture.box_lineup,
               LoadedTexture.square_particle,
               //LoadedTexture.waterheigth,

            });
            Engine.LoadContent.LoadTexture(LoadedTexture.BlockTextures, LootfestLib.ContentFolder + "blocktxt");
            new LF2.LoadTiles();



            LootfestLib.Images.Init();//mt
            Data.MaterialBuilder.InitMaterials();
            Data.Block.Init();//mt
            LootfestLib.Images.LoadStandardVobjects();
           // Engine.Storage.AddToSaveQue(StartQuedProcess, false);
           // Ref.update.AddThreadQueObj(this);

            LoadContent();

            Map.Terrain.AbsScreenMeshBuilder.Init();
            Map.PreparedFace.Init();
            Process.ModifiedImage.Init();

            Map.Terrain.ObstacleBuilder.Init();
            Engine.ParticleHandler.Init();
            GameObjects.NPC.Worker.GameInit();
            Data.Gadgets.BluePrintLib.GameInit();
            Voxels.FaceCornerColorYS.Init();
           // handlersInited = true;

            new Timer.Action0ArgTrigger(loadingDoneEvent);
        }


        void LoadContent()
        {
            string soundDir = LootfestLib.ContentFolder + "Sound\\";

            Engine.LoadContent.LoadSound(LoadedSound.MenuSelect, soundDir + "Button_Clicked");

            //Engine.LoadContent.LoadSound(new List<Engine.FileDirAndName>
            //{
                //High prio
            Engine.LoadContent.LoadSound(LoadedSound.MenuSelect,  soundDir + "Button_Clicked");
            Engine.LoadContent.LoadSound(LoadedSound.MenuMove,  soundDir + "Cursor_moved");
            Engine.LoadContent.LoadSound(LoadedSound.MenuBack,  soundDir + "Returning");
            //Normal prio
            Engine.LoadContent.LoadSound(LoadedSound.Coin1,  soundDir + "coin1");
            Engine.LoadContent.LoadSound(LoadedSound.Coin2,  soundDir + "coin2");
            Engine.LoadContent.LoadSound(LoadedSound.Coin3,  soundDir + "coin3");
            Engine.LoadContent.LoadSound(LoadedSound.HealthUp,  soundDir + "Healthy");
            Engine.LoadContent.LoadSound(LoadedSound.Sword1,  soundDir + "sword1");
            Engine.LoadContent.LoadSound(LoadedSound.Sword2 ,  soundDir + "sword2");
            Engine.LoadContent.LoadSound(LoadedSound.Sword3 ,  soundDir + "sword3");
            Engine.LoadContent.LoadSound(LoadedSound.TakeDamage1 ,  soundDir + "takedamage1");
            Engine.LoadContent.LoadSound(LoadedSound.TakeDamage2 ,  soundDir + "takedamage2"); 
            Engine.LoadContent.LoadSound(LoadedSound.PickUp ,  soundDir + "pickup"); 
            Engine.LoadContent.LoadSound(LoadedSound.MonsterHit1,  soundDir + "animal_hurt1");
            Engine.LoadContent.LoadSound(LoadedSound.MonsterHit2,  soundDir + "animal_hurt2");
            Engine.LoadContent.LoadSound(LoadedSound.Bow,  soundDir + "bow");
            Engine.LoadContent.LoadSound(LoadedSound.EnemyProj1,  soundDir + "enemyproj1");
            Engine.LoadContent.LoadSound(LoadedSound.EnemyProj2,  soundDir + "enemyproj2");

            Engine.LoadContent.LoadSound(LoadedSound.door,  soundDir + "door");
            Engine.LoadContent.LoadSound(LoadedSound.chat_message,  soundDir + "chat_message");

            Engine.LoadContent.LoadSound(LoadedSound.express_anger,  soundDir + "express_anger");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi1,  soundDir + "express_hi1");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi2,  soundDir + "express_hi2");
            Engine.LoadContent.LoadSound(LoadedSound.express_hi3,  soundDir + "express_hi3");
            Engine.LoadContent.LoadSound(LoadedSound.express_laugh,  soundDir + "express_laugh");
            Engine.LoadContent.LoadSound(LoadedSound.express_teasing1,  soundDir + "express_teasing1");
            Engine.LoadContent.LoadSound(LoadedSound.express_teasing2,  soundDir + "express_teasing2");
            Engine.LoadContent.LoadSound(LoadedSound.express_thumbup1,  soundDir + "express_thumbup1");
            Engine.LoadContent.LoadSound(LoadedSound.express_thumbup2,  soundDir + "express_thumbup2");
            Engine.LoadContent.LoadSound(LoadedSound.player_enters,  soundDir + "player_enters");
            Engine.LoadContent.LoadSound(LoadedSound.chat_message,  soundDir + "chat_message");
            Engine.LoadContent.LoadSound(LoadedSound.enter_build,  soundDir + "enter_build");
            Engine.LoadContent.LoadSound(LoadedSound.block_place_1,  soundDir + "block_place_1");
            Engine.LoadContent.LoadSound(LoadedSound.tool_dig,  soundDir + "tool_dig");
            Engine.LoadContent.LoadSound(LoadedSound.tool_select,  soundDir + "tool_select");


            Engine.LoadContent.LoadSound(LoadedSound.buy,  soundDir + "buy");
            Engine.LoadContent.LoadSound(LoadedSound.Trophy,  soundDir + "Trophy");
            
               
            //Engine.LoadContent.LoadSound(LoadedSound.ogre_hurt1,  soundDir + "");
            //Engine.LoadContent.LoadSound(LoadedSound.ogre_hurt2,  soundDir + "");
            //Engine.LoadContent.LoadSound(LoadedSound.terrain_destruct1,  soundDir + "");
            //Engine.LoadContent.LoadSound(LoadedSound.terrain_destruct2,  soundDir + "");
            Engine.LoadContent.LoadSound(LoadedSound.open_map,  soundDir + "open_map");
            Engine.LoadContent.LoadSound(LoadedSound.shieldcrash,  soundDir + "shieldcrash");
            Engine.LoadContent.LoadSound(LoadedSound.weaponclink,  soundDir + "weaponclink");

            Engine.LoadContent.LoadSound(LoadedSound.barrel_explo,  soundDir + "barrel_explo");
            Engine.LoadContent.LoadSound(LoadedSound.deathpop,  soundDir + "deathpop");
            Engine.LoadContent.LoadSound(LoadedSound.out_of_ammo,  soundDir + "out_of_ammo");

            
            Engine.LoadContent.LoadSound(LoadedSound.MenuNotAllowed,  soundDir + "Not_Allowed");  
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_Neutral,  soundDir + "Talking");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_DidYouKnow,  soundDir + "Did_You_Know");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_Question,  soundDir + "Question");
            Engine.LoadContent.LoadSound(LoadedSound.Dialogue_QuestAccomplished,  soundDir + "Quest_Accomplished");
            Engine.LoadContent.LoadSound(LoadedSound.CraftSuccessful,  soundDir + "Craft_Successful");


            //});

            Engine.LoadContent.LoadMesh(LoadedMesh.plane, LootfestLib.ContentFolder + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, LootfestLib.ContentFolder + "cube_repeating");
            Engine.LoadContent.LoadMesh(LoadedMesh.sphere, LootfestLib.ContentFolder + "sphere");
            Engine.LoadContent.LoadMesh(LoadedMesh.skydome, LootfestLib.ContentFolder + "skydome");


            //Engine.LoadContent.LoadMeshes(new List<LoadedMesh>
            //{
            //    LoadedMesh.plane,
            //    LoadedMesh.cube_repeating,
            //    LoadedMesh.sphere,
            //    LoadedMesh.skydome,
            //});
            System.Diagnostics.Debug.WriteLine("Sounds Done loading!");
            Data.Characters.NPCdata.LoadStationImages();
            Map.Terrain.Area.AbsUrban.LoadImages();
            Map.Terrain.Area.EndTomb.LoadImages();
        }

        static bool doneLoading = false;
        void loadingDoneEvent()
        {
            doneLoading = true;
            GameObjects.AbsVoxelObj.MakeTempImage();
            Data.MaterialBuilder.InitRenderTarget();//cant be threaded
            createStartButton();
        }

        //Graphics.TextG pressStartText = null;
        void createStartButton()
        {
            float blockSize =(int)(Engine.Screen.Height * 0.07f);
            string text = "press start";
            float w = text.Length * blockSize;
            Vector2 letterPos = new Vector2(Engine.Screen.CenterScreen.X - w * PublicConstants.Half, Engine.Screen.Height * 0.82f);
            float timeDelay = 0;

            foreach (char c in text)
            {
                Data.MaterialType m = lib.LetterBlockFromChar(c);
                SpriteName tile = Data.MaterialBuilder.MaterialTile((byte)m);
                Image img = new Image(tile, letterPos, new Vector2(blockSize), ImageLayers.Background4);
                letterPos.X += blockSize;

                img.Transparentsy = 0;
                Graphics.Motion2d fadeIn = new Graphics.Motion2d(MotionType.TRANSPARENSY, img, Vector2.One, MotionRepeate.NO_REPEATE, 33 * 20, false);
                new Timer.UpdateTrigger(timeDelay, fadeIn, true);
                //timeDelay += 33 * 4;
            }
           
            
        }

        public override void Time_Update(float time)
        {
            if (doneLoading)
            {
                for (int ix = 0; ix <= int.Player4; ++ix)
                {
                    if (Input.Controller.Instance(ix).KeyDownEvent(Buttons.Start, Buttons.A))
                    {
                        if (dialogue != null)
                        {
                            dialogue.Click();
                        }
                        else
                        {

                            if (PlatformSettings.RunningWindows)
                            {
                                StorageSelectionCompleteEvent();
                                return;
                            }

                            Engine.XGuide.LocalHostIndex = ix;
                            if (Engine.XGuide.GetPlayer(ix).SignedIn)
                            {
                                //bring up storage selection

                                new DataLib.PromptStorageSelection(this, ix, true);

                            }
                            else
                            {
                                dialogue = new Dialogue(new HUD.DialogueData("", "You need to be signed in"), Engine.Screen.SafeArea, this);
                            }
                        }
                    }
                }
            }

        }


        
        Dialogue dialogue;
        public void DialogueClosedEvent()
        {
            dialogue = null;
        }
        public void StorageSelectionCompleteEvent()
        {
            new MainMenuState(true);
        }
        public void StorageSelectionFailedEvent()
        {
            dialogue = new Dialogue(new HUD.DialogueData("", "This game requires a storage device"), Engine.Screen.SafeArea, this);
            //reset the storage prompt

        }


        public override Engine.GameStateType Type
        {
            get { return Engine.GameStateType.LoadingContent; }
        }

    }
}
