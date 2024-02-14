using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.Input;

namespace VikingEngine.PJ.Story
{
    class StoryPlayState : Engine.GameState
    {
        public VikingEngine.Graphics.TopViewCamera camera;
        HUD.Gui menu = null;
        bool waitForMouseUp = false;
        Editor editor;

        public StoryPlayState()
            : base()
        {
            storyRef.state = this;
            Engine.LoadContent.LoadMesh(LoadedMesh.plane, Engine.LoadContent.ModelPath + "plane");
            Engine.LoadContent.LoadMesh(LoadedMesh.cube_repeating, Engine.LoadContent.ModelPath + "cube_repeating");

            camera = new VikingEngine.Graphics.TopViewCamera(46, new Vector2(MathHelper.PiOver2, MathHelper.PiOver2));//70, new Vector2(MathHelper.PiOver2, OverviewCamAngle));
            camera.FarPlane = 400;
            camera.FieldOfView = 30;
            camera.positionChaseLengthPercentage = 0.24f;
            camera.LookTarget = new Vector3(0, Chunk.Size * 0.5f, 0.5f);

            //camera.Time_Update(0);
            camera.RecalculateMatrices();
            Engine.XGuide.LocalHost.view.Camera = camera;
            Engine.XGuide.LocalHost.view.Viewport = VikingEngine.Engine.Draw.defaultViewport;
            draw.Camera = camera;

            storyRef.map.endGenerateChunks();
            //new WorldMap();

            Vector2 cameraStart = new Vector2(Chunk.Size * 0.5f);
            CameraCenter = cameraStart;

            editor = new Editor();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            if (menu != null)
            {
                if (menu.Update() || Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    closeMenu();
                }
                return;
            }

            //if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.Escape))
            //{
            //    new LobbyState(null);
            //}

            editor.update();

            camera.Time_Update(Ref.DeltaTimeMs);
        }
        

        public Vector2 CameraCenter
        {
            get
            {
                return storyLib.ToV2(camera.goalLookTarget);
            }
            set
            {
                camera.goalLookTarget = storyLib.ToV3(value);
            }
        }

        public HUD.Gui openMenu()
        {
            //Engine.XGuide.LocalHost.inputMap.SetGameStateLayout(ControllerActionSetType.MenuControls);
            if (menu == null)
            {
                menu = new HUD.Gui(LootFest.MenuSystem2.GuiStyle(),
                    Engine.Screen.SafeArea, 0f, ImageLayers.Top4,
                    Engine.XGuide.LocalHost.inputMap.inputSource);
            }

            return menu;
        }

        public void closeMenu()
        {
            if (menu != null)
            {
                menu.DeleteMe();
                menu = null;

                if (Input.Mouse.IsButtonDown(MouseButton.Left))
                {
                    waitForMouseUp = true;
                }
            }
            //Engine.XGuide.LocalHost.inputMap.SetGameStateLayout(ControllerActionSetType.CardGameControls);
        }
    }

    enum PaintTool
    {
        Eraser,
        Brush,
        Marqee,
        NUM
    }
}
