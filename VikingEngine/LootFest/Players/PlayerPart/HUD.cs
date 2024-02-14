using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using VikingEngine.LootFest.GO.Gadgets;
using VikingEngine.EngineSpace.Maths;
using VikingEngine.Input;

namespace VikingEngine.LootFest.Players
{
    //HUD
    partial class Player
    {
        public PlayerStatusDisplay statusDisplay;
        LoadingScreen loadingScreen = null;
        Display.SaveIcon saveIcon;
        public Display.AbsInteractDisplay interactDisplay = null;
        VikingEngine.LootFest.Display.InputOverview inputOverview = null;

        public LoadingScreen startLoadingScreen(Map.WorldPosition teleportingToWp)
        {
            if (loadingScreen == null)
            {
                loadingScreen = new LoadingScreen(this, teleportingToWp);
            }
            return loadingScreen;
        }

        public void OnLoadingScreenComplete()
        {
            loadingScreen = null;

            if (hero.teleportReason == TeleportReason.LevelTeleport)
            { Save(); }
        }

        public bool InLoadingScene { get { return loadingScreen != null; } }
        public bool NotInLoadingScreen()
        {
            return loadingScreen == null;
        }

        void updateMessageHandlerPos()
        {
            if (messageHandler != null)
            {
                Vector2 messageHandlerPos = safeScreenArea.Position;
                //messageHandlerPos.Y = PlayerStatusDisplay.BottomLeftPos(pData).Y;
                messageHandler.Position = messageHandlerPos;
            }
        }

     
        void clearGamerNames()
        {
            foreach (GamerName gn in gamerNames)
            {
                gn.DeleteMe();
            }
            gamerNames.Clear();
        }

        Image pauseBorder;

        VikingEngine.LootFest.Display.PauseIcon pauseIcon;

        //TextG pauseText;
        public void Pause(bool isPaused)
        {
            const float TransfereTime = 200;
            bool change = true;

            if (isPaused)
            {
                if (pauseBorder == null)
                {
                    pauseBorder = new Image(SpriteName.LFEdge, ScreenArea.Position, ScreenArea.Size, ImageLayers.Background8);
                    pauseBorder.Color = Color.Black;
                    pauseBorder.Opacity = 0;

                    if (inMenu)
                    {
                        //pauseText = new TextG(LoadedFont.Lootfest, menuSystem.menu.area.RightCenter, new Vector2(1.2f, 0.8f), Align.CenterAll, "PAUSE",
                        //    Color.White, ImageLayers.Background6);
                        //pauseText.Rotation = MathHelper.PiOver2;
                        pauseIcon = new Display.PauseIcon(safeScreenArea);

                    }
                }
                else
                {
                    change = false;
                }
            }
            else
            {
                //if (pauseText != null)
                //    new Graphics.Motion2d(MotionType.OPACITY, pauseText, VectorExt.V2NegOne, MotionRepeate.NO_REPEAT, TransfereTime, true);
                if (pauseIcon != null)
                {
                    pauseIcon.BeginDelete();
                    pauseIcon = null;
                }
                new Timer.TerminateCollection(TransfereTime * 10, new List<IDeleteable> { pauseBorder });
                
            }
            if (change)
            {
                if (pauseBorder != null)
                {
                    new Graphics.Motion2d(MotionType.OPACITY, pauseBorder, Vector2.One * (0.7f * lib.BoolToLeftRight(isPaused)), MotionRepeate.NO_REPEAT, TransfereTime, true);

                }
                //if (pauseText != null)
                //{
                //    new Graphics.Motion2d(MotionType.MOVE, pauseText, new Vector2(40 * lib.BoolToDirection(isPaused), 0), MotionRepeate.NO_REPEAT, TransfereTime, true);
                //}
                if (!isPaused)
                    pauseBorder = null;
            }
        }


        public void refreshKeyCount(IntVector2 count)
        {
            if (interactDisplay == null ||
                interactDisplay is Display.KeyCountDisplay == false)
            {
                deleteInteractDisplay();
                interactDisplay = new Display.KeyCountDisplay(this, count);
            }
            else
            {
                interactDisplay.refresh(this, count);
            }
        }

        public void refreshLevelCollectItem()
        {
            if (interactDisplay == null ||
                interactDisplay is Display.LevelCollectItemDisplay == false)
            {
                deleteInteractDisplay();
                interactDisplay = new Display.LevelCollectItemDisplay(this);
            }
            else
            {
                interactDisplay.refresh(this, null);
            }
        }

        public void deleteInteractDisplay()
        {
            if (interactDisplay != null)
            {
                interactDisplay.DeleteMe();
                interactDisplay = null;
            }
        }

        public HUD.Gui createMenu()
        {
            if (menuSystem == null)
            {
                removeInputOverView();

                menuSystem = new MenuSystem2(this);

                //if (PlatformSettings.DevBuild && DebugSett.UseCameraMenuMove) //Ta bort check när kameran är buggfri
                //{
                //    CinematicScriptHandler cc = pData.view.Camera.cinematicControls;

                //    const float TIME = 0.5f;

                //    cc.ClearQueue();

                //    var timer = new TimerCriterion(TIME);
                //    var menuClosed = new EventCriterion(isMenuNull);

                //    Vector3 pos = pData.view.Camera.Position;
                //    // into menu
                //    cc.Enqueue(new using System;(new DelFunc<Vector3>(calcCameraMenuOffset), timer),
                //               new CamCmd_Zoom(new ConFunc<float>(10), timer));
                //    // in menu
                //    cc.Enqueue(new CamCmd_Offset(new DelFunc<Vector3>(calcCameraMenuOffset), menuClosed));
                //    // out of menu
                //    cc.Enqueue(new CamCmd_Offset(new ConFunc<Vector3>(Vector3.Zero), timer),
                //               new CamCmd_Zoom(new ConFunc<float>(pData.view.Camera.Zoom), timer));
                //}
            }

            //mode = PlayerMode.InMenu;
            //inputMap.SetGameStateLayout(ControllerActionSetType.MenuControls);
            return menuSystem.menu;
        }

        public MenuSystem2 MenuSystem()
        {
            createMenu();
            return menuSystem;
        }

        void removeInputOverView()
        {
            if (inputOverview != null)
            {
                inputOverview.DeleteMe();
                inputOverview = null;
            }
        }

        public void beginInputOverview()
        {
            removeInputOverView();

            int state = 0;
            if (voxelDesigner != null)
            {
                if (voxelDesigner.HasSelection)
                {
                    state = 2;
                }
                else
                {
                    state = 1;
                }
            }
            inputOverview = new Display.InputOverview(safeScreenArea, inputMap, state);
        }

        public void CloseMenu()
        {
            if (hero.Alive)
            {
                if (menuSystem != null)
                {
                    menuSystem.DeleteMe();
                    menuSystem = null;
                    Save();
                    Ref.draw.settingsChanged2dImagesRefresh();

                    beginInputOverview();

                    if (appearanceChanged)
                    {
                        NetShareAppearance();

                    }
                }
            } 

        }

        //Vector3 calcCameraMenuOffset()
        //{
        //    AbsCamera camera = localPData.view.Camera;
        //    Viewport viewport = Draw.graphicsDeviceManager.GraphicsDevice.Viewport;

        //    Vector2 l = new Vector2(menuArea().Right, Screen.Height / 2);
        //    Vector2 m = new Vector2(Screen.Width / 2, Screen.Height / 2);
        //    Vector2 r = new Vector2(Screen.Width,     Screen.Height / 2);

        //    Plane plane = new Plane(camera.Forward, -camera.Forward.Dot(hero.Position));

        //    bool non;
        //    Vector3 lpos = camera.CastRayInto3DPlane(l, viewport, plane, out non);
        //    Vector3 mpos = camera.CastRayInto3DPlane(m, viewport, plane, out non);
        //    Vector3 rpos = camera.CastRayInto3DPlane(r, viewport, plane, out non);

        //    Vector3 value = ((lpos + rpos) / 2) - mpos;
        //    //if ((camera.positionOffset + value).LengthSquared() < 0.0001f)
        //    //    return camera.positionOffset;
        //    return -value;
        //}

        void mainMenu()
        {
            createMenu();
            menuSystem.MainMenu();
        }
    }
}
