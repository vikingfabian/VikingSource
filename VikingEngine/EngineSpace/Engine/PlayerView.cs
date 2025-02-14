using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.LootFest.Players;

namespace VikingEngine.Engine
{
    class PlayerView
    {
        public Graphics.CameraType camType = Graphics.CameraType.TopView;
        public Graphics.AbsCamera Camera, LightCamera;
        public int ScreenIndex = -1;

        public Rectangle DrawArea;
        
        public VectorRect DrawAreaF;
        public VectorRect safeScreenArea;
        public VectorRect DrawAreaPercent;
        public bool verticalSplit, horizontalSplit;
        
        public Viewport Viewport;

        public Viewport RenderTargetViewport;

        public PlayerView()
        { }

        public Vector2 From3DToScreenPos(Vector3 objectPos)
        {
            return Camera.From3DToScreenPos(objectPos, Viewport);
        }

        public Vector3 ScreenPosTo3D(Vector3 screenPos)
        {
            Vector3 worldLocation = Viewport.Unproject(screenPos, Camera.Projection, Camera.ViewMatrix, Matrix.CreateTranslation(Camera.LookTarget));
            return worldLocation;

        }

        public PlayerView Clone()
        {
            PlayerView c = new PlayerView();
            c.camType = camType;
            c.Camera = Camera;
            c.DrawArea = DrawArea;
            c.DrawAreaF = DrawAreaF;
            c.safeScreenArea = safeScreenArea;
            c.DrawAreaPercent = DrawAreaPercent;
            c.Viewport = Viewport;
            c.RenderTargetViewport = RenderTargetViewport;
            return c;
        }

        public Rectangle GetDrawArea(int numplayers, int myScreenIx, bool bHorizontalSplit, 
            out float zoom)
        {
            ScreenIndex = myScreenIx;
            verticalSplit = false; horizontalSplit = false;

            zoom = 38 * LootFest.LfLib.ModelsScaleUp;
            switch (numplayers)
            {
                default:
                    DrawArea = new Rectangle(0, 0, Engine.Screen.Width, Engine.Screen.Height);
                    break;
                case 2:

                    if (bHorizontalSplit)
                    {
                        horizontalSplit = true;
                        zoom *= 0.95f;
                        int height = (int)(Engine.Screen.Height * PublicConstants.Half);
                        DrawArea = new Rectangle(0, height * myScreenIx, Engine.Screen.Width, height);
                    }
                    else
                    {
                        verticalSplit = true;
                        zoom *= 1.6f;

                        int width = (int)(Engine.Screen.Width * PublicConstants.Half);
                        DrawArea = new Rectangle(width * myScreenIx, 0, width, Engine.Screen.Height);
                    }
                    break;
                case 3:
                    if (bHorizontalSplit)
                    {
                        DrawArea = Rectangle.Empty;
                        horizontalSplit = true;
                        DrawArea.Height = (int)(Engine.Screen.Height * PublicConstants.Half);
                        if (myScreenIx == 0)
                        {
                            DrawArea.Width = Engine.Screen.Width;
                            zoom *= 0.95f;
                        }
                        else
                        {
                            verticalSplit = true;
                            DrawArea.Width = (int)(Engine.Screen.Width * PublicConstants.Half);
                            DrawArea.X = (myScreenIx - 1) * DrawArea.Width;
                            DrawArea.Y = DrawArea.Height;
                        }
                    }
                    else
                    {
                        verticalSplit = true;
                        DrawArea = Rectangle.Empty;

                        DrawArea.Width = (int)(Engine.Screen.Width * PublicConstants.Half);
                        if (myScreenIx == 0)
                        {
                            DrawArea.Height = Engine.Screen.Height;
                            zoom *= 1.1f;
                        }
                        else
                        {
                            horizontalSplit = true;
                            DrawArea.X = DrawArea.Width;
                            DrawArea.Height = (int)(Engine.Screen.Height * PublicConstants.Half);
                            DrawArea.Y = (myScreenIx - 1) * DrawArea.Height;
                        }
                    }
                    break;
                case 4:
                    verticalSplit = true; horizontalSplit = true;
                    int height2 = (int)(Engine.Screen.Height * PublicConstants.Half);
                    int width2 = (int)(Engine.Screen.Width * PublicConstants.Half);
                    DrawArea = new Rectangle(
                        (myScreenIx == 1 || myScreenIx == 3) ? width2 : 0, //x
                        myScreenIx >= 2 ? height2 : 0, //y
                        width2, height2);
                    break;
            }

            return DrawArea;
        }

        public void FullScreenSetup()
        {
            SetDrawArea(1, 0, false, null);
        }

        public void SetDrawArea(int numplayers, int myScreenIx, bool updateCam, Player player)
        {
            float zoom;
            GetDrawArea(numplayers, myScreenIx, Engine.Draw.horizontalSplit, out zoom);
           
            //Add camera
            if (updateCam)
            {
                float startAngle = 0;
                if (Camera != null)
                    startAngle = Camera.TiltX;

                if (camType == Graphics.CameraType.TopView)
                {
                    Camera = new Graphics.TopViewCamera(zoom, new Vector2(MathHelper.PiOver4, MathHelper.PiOver4),
                    DrawArea.Width, DrawArea.Height);
                    Camera.TiltX = startAngle;
                }
                else
                {
                    VikingEngine.Graphics.IFirstPerson person = null;
                    if (Camera != null && Camera.CamType == Graphics.CameraType.FirstPerson)
                        person = ((Graphics.FirstPersonCamera)Camera).Person;
                    Camera = new Graphics.FirstPersonCamera(zoom, new Vector2(MathHelper.PiOver4, MathHelper.PiOver4),
                        DrawArea.Width, DrawArea.Height, person);
                    //if (settings != null)
                    //    Camera.Settings = settings.Value;
                    if (player != null && player.hero != null)
                        Camera.TiltX = player.hero.Rotation.Radians + MathHelper.PiOver2;
                    else
                        Camera.TiltX = startAngle;
                }
                Camera.Position = Vector3.One * 250;
            }
            else if (Camera != null)
            {
                Camera.setAspectRatio(DrawArea.Width, DrawArea.Height);
            }


            Viewport = new Viewport(DrawArea.X, DrawArea.Y, DrawArea.Width, DrawArea.Height);
            RenderTargetViewport = new Viewport(0, 0, DrawArea.Width, DrawArea.Height);
            DrawAreaPercent = new VectorRect(
                new Vector2((float)DrawArea.X / Engine.Screen.Width, (float)DrawArea.Y / Engine.Screen.Height),
                new Vector2((float)DrawArea.Width / Engine.Screen.Width, (float)DrawArea.Height / Engine.Screen.Height));

            DrawAreaF = new VectorRect(DrawArea);
            safeScreenArea = DrawAreaF;

            safeScreenArea.Position += new Vector2(1);
            safeScreenArea.Width = Bound.Max(safeScreenArea.Width, Engine.Screen.SafeArea.Width);
            safeScreenArea.Height = Bound.Max(safeScreenArea.Height, Engine.Screen.SafeArea.Height);

            safeScreenArea = Engine.Screen.SafeArea.KeepSmallerRectInsideBound_Position(safeScreenArea);

        }
    }
}
