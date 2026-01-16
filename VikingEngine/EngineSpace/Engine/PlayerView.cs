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
        public const float SafeSpaceBetweenPlayers = 8;
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

        /// <summary>
        /// For split screen play
        /// </summary>
        public Rectangle GetDrawArea(int numplayers, int myScreenIx, out float zoom)
        {
            ScreenIndex = myScreenIx;
            verticalSplit = false;
            horizontalSplit = false;

            zoom = 38 * LootFest.LfLib.ModelsScaleUp;

            float screenW = Engine.Screen.Width;
            float screenH = Engine.Screen.Height;

            SplitScreenOptions splitOption = Engine.Screen.splitScreenOptions;

            bool horizontalFirst = splitOption == SplitScreenOptions.HorizontalFirst;
            bool verticalFirst = splitOption == SplitScreenOptions.VerticalFirst;
            bool horizontalOnly = splitOption == SplitScreenOptions.HorizontalOnly;
            bool verticalOnly = splitOption == SplitScreenOptions.VerticalOnly;

            // Fallback if something weird is passed in
            if (!horizontalFirst && !verticalFirst && !horizontalOnly && !verticalOnly)
                horizontalFirst = true;

            // All adjustments use this size so movement feels consistent
            float adjustmenSize = screenH * 0.1f;

            switch (numplayers)
            {
                default:
                    DrawArea = new Rectangle(0, 0, (int)screenW, (int)screenH);
                    break;

                case 2:
                    if (horizontalFirst || horizontalOnly)
                    {
                        horizontalSplit = true;
                        zoom *= 0.95f;

                        float adjustMenInPixels = Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize;
                        float topHeight = screenH * 0.5f + adjustMenInPixels;

                        float minH = screenH * 0.1f;
                        float maxH = screenH * 0.9f;
                        if (topHeight < minH) topHeight = minH;
                        else if (topHeight > maxH) topHeight = maxH;

                        float bottomHeight = screenH - topHeight;

                        int y = (myScreenIx == 0) ? 0 : (int)topHeight;
                        int h = (myScreenIx == 0) ? (int)topHeight : (int)bottomHeight;

                        DrawArea = new Rectangle(0, y, (int)screenW, h);
                    }
                    else // verticalFirst or verticalOnly
                    {
                        verticalSplit = true;
                        zoom *= 1.6f;

                        float adjustMenInPixels = Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize;
                        float leftWidth = screenW * 0.5f + adjustMenInPixels;

                        float minW = screenW * 0.1f;
                        float maxW = screenW * 0.9f;
                        if (leftWidth < minW) leftWidth = minW;
                        else if (leftWidth > maxW) leftWidth = maxW;

                        float rightWidth = screenW - leftWidth;

                        int x = (myScreenIx == 0) ? 0 : (int)leftWidth;
                        int w = (myScreenIx == 0) ? (int)leftWidth : (int)rightWidth;

                        DrawArea = new Rectangle(x, 0, w, (int)screenH);
                    }
                    break;

                case 3:

                    // HorizontalOnly: 3 horizontal stripes (factor ~0.33) + adjustable splits
                    if (horizontalOnly)
                    {
                        horizontalSplit = true;

                        int split1 = Convert.ToInt32(screenH * 0.33f + Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize);
                        int split2 = Convert.ToInt32(screenH * 0.66f + Engine.Screen.splitScreenDivideAdjustment2 * adjustmenSize);

                        int y, h;
                        switch (myScreenIx)
                        {
                            default:
                                y = 0;
                                h = split1;
                                break;
                            case 1:
                                y = split1;
                                h = split2 - split1;
                                break;
                            case 2:
                                y = split2;
                                h = (int)screenH - split2;
                                break;
                        }

                        DrawArea = new Rectangle(0, y, (int)screenW, h);
                        break;
                    }

                    if (verticalOnly)
                    {
                        // Follow this solution on all
                        verticalSplit = true;

                        int split1 = Convert.ToInt32(screenW * 0.33f + Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize);
                        int split2 = Convert.ToInt32(screenW * 0.66f + Engine.Screen.splitScreenDivideAdjustment2 * adjustmenSize);

                        int x, w;
                        switch (myScreenIx)
                        {
                            default:
                                x = 0;
                                w = split1;
                                break;
                            case 1:
                                x = split1;
                                w = split2 - split1;
                                break;
                            case 2:
                                x = split2;
                                w = (int)screenW - split2;
                                break;
                        }

                        DrawArea = new Rectangle(x, 0, w, (int)screenH);
                        break;
                    }

                    // Original mixed layout with adjustments
                    if (horizontalFirst)
                    {
                        DrawArea = Rectangle.Empty;
                        horizontalSplit = true;

                        // Main horizontal split with adjustment1
                        float adjustMenInPixels = Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize;
                        float topHeight = screenH * 0.5f + adjustMenInPixels;

                        float minH = screenH * 0.1f;
                        float maxH = screenH * 0.9f;
                        if (topHeight < minH) topHeight = minH;
                        else if (topHeight > maxH) topHeight = maxH;

                        float bottomHeight = screenH - topHeight;

                        if (myScreenIx == 0)
                        {
                            DrawArea.X = 0;
                            DrawArea.Y = 0;
                            DrawArea.Width = (int)screenW;
                            DrawArea.Height = (int)topHeight;
                            zoom *= 0.95f;
                        }
                        else
                        {
                            // Bottom half: vertical split with adjustment3
                            verticalSplit = true;

                            float adjustMenInPixels3 = Engine.Screen.splitScreenDivideAdjustment3 * adjustmenSize;
                            float leftWidth = screenW * 0.5f + adjustMenInPixels3;

                            float minW = screenW * 0.1f;
                            float maxW = screenW * 0.9f;
                            if (leftWidth < minW) leftWidth = minW;
                            else if (leftWidth > maxW) leftWidth = maxW;

                            float rightWidth = screenW - leftWidth;

                            DrawArea.Y = (int)topHeight;
                            DrawArea.Height = (int)bottomHeight;

                            if (myScreenIx == 1)
                            {
                                DrawArea.X = 0;
                                DrawArea.Width = (int)leftWidth;
                            }
                            else // myScreenIx == 2
                            {
                                DrawArea.X = (int)leftWidth;
                                DrawArea.Width = (int)rightWidth;
                            }
                        }
                    }
                    else // verticalFirst
                    {
                        verticalSplit = true;
                        DrawArea = Rectangle.Empty;

                        // Main vertical split with adjustment1
                        float adjustMenInPixels = Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize;
                        float leftWidth = screenW * 0.5f + adjustMenInPixels;

                        float minW = screenW * 0.1f;
                        float maxW = screenW * 0.9f;
                        if (leftWidth < minW) leftWidth = minW;
                        else if (leftWidth > maxW) leftWidth = maxW;

                        float rightWidth = screenW - leftWidth;

                        if (myScreenIx == 0)
                        {
                            DrawArea.X = 0;
                            DrawArea.Y = 0;
                            DrawArea.Width = (int)leftWidth;
                            DrawArea.Height = (int)screenH;
                            zoom *= 1.1f;
                        }
                        else
                        {
                            // Right half: horizontal split with adjustment3
                            horizontalSplit = true;

                            float adjustMenInPixels3 = Engine.Screen.splitScreenDivideAdjustment3 * adjustmenSize;
                            float topHeight = screenH * 0.5f + adjustMenInPixels3;

                            float minH = screenH * 0.1f;
                            float maxH = screenH * 0.9f;
                            if (topHeight < minH) topHeight = minH;
                            else if (topHeight > maxH) topHeight = maxH;

                            float bottomHeight = screenH - topHeight;

                            DrawArea.X = (int)leftWidth;
                            DrawArea.Width = (int)rightWidth;

                            if (myScreenIx == 1)
                            {
                                DrawArea.Y = 0;
                                DrawArea.Height = (int)topHeight;
                            }
                            else // myScreenIx == 2
                            {
                                DrawArea.Y = (int)topHeight;
                                DrawArea.Height = (int)bottomHeight;
                            }
                        }
                    }
                    break;

                case 4:

                    // HorizontalOnly: 4 horizontal stripes (factor 0.25) + adjustable splits
                    if (horizontalOnly)
                    {
                        horizontalSplit = true;

                        int split1 = Convert.ToInt32(screenH * 0.25f + Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize);
                        int split2 = Convert.ToInt32(screenH * 0.50f + Engine.Screen.splitScreenDivideAdjustment2 * adjustmenSize);
                        int split3 = Convert.ToInt32(screenH * 0.75f + Engine.Screen.splitScreenDivideAdjustment3 * adjustmenSize);

                        int y, h;
                        switch (myScreenIx)
                        {
                            default:
                                y = 0;
                                h = split1;
                                break;
                            case 1:
                                y = split1;
                                h = split2 - split1;
                                break;
                            case 2:
                                y = split2;
                                h = split3 - split2;
                                break;
                            case 3:
                                y = split3;
                                h = (int)screenH - split3;
                                break;
                        }

                        DrawArea = new Rectangle(0, y, (int)screenW, h);
                        break;
                    }

                    // VerticalOnly: 4 vertical stripes (factor 0.25) + adjustable splits
                    if (verticalOnly)
                    {
                        verticalSplit = true;

                        int split1 = Convert.ToInt32(screenW * 0.25f + Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize);
                        int split2 = Convert.ToInt32(screenW * 0.50f + Engine.Screen.splitScreenDivideAdjustment2 * adjustmenSize);
                        int split3 = Convert.ToInt32(screenW * 0.75f + Engine.Screen.splitScreenDivideAdjustment3 * adjustmenSize);

                        int x, w;
                        switch (myScreenIx)
                        {
                            default:
                                x = 0;
                                w = split1;
                                break;
                            case 1:
                                x = split1;
                                w = split2 - split1;
                                break;
                            case 2:
                                x = split2;
                                w = split3 - split2;
                                break;
                            case 3:
                                x = split3;
                                w = (int)screenW - split3;
                                break;
                        }

                        DrawArea = new Rectangle(x, 0, w, (int)screenH);
                        break;
                    }

                    // Original 2x2 grid with adjustable dividers
                    verticalSplit = true;
                    horizontalSplit = true;

                    // Horizontal divider with adjustment1
                    float adjustMenInPixelsH = Engine.Screen.splitScreenDivideAdjustment1 * adjustmenSize;
                    float topHeight4 = screenH * 0.5f + adjustMenInPixelsH;

                    float minH4 = screenH * 0.1f;
                    float maxH4 = screenH * 0.9f;
                    if (topHeight4 < minH4) topHeight4 = minH4;
                    else if (topHeight4 > maxH4) topHeight4 = maxH4;

                    float bottomHeight4 = screenH - topHeight4;

                    // Vertical divider with adjustment2
                    float adjustMenInPixelsW = Engine.Screen.splitScreenDivideAdjustment2 * adjustmenSize;
                    float leftWidth4 = screenW * 0.5f + adjustMenInPixelsW;

                    float minW4 = screenW * 0.1f;
                    float maxW4 = screenW * 0.9f;
                    if (leftWidth4 < minW4) leftWidth4 = minW4;
                    else if (leftWidth4 > maxW4) leftWidth4 = maxW4;

                    float rightWidth4 = screenW - leftWidth4;

                    bool rightSide = (myScreenIx == 1 || myScreenIx == 3);
                    bool bottom = (myScreenIx >= 2);

                    int x4 = rightSide ? (int)leftWidth4 : 0;
                    int y4 = bottom ? (int)topHeight4 : 0;
                    int w4 = rightSide ? (int)rightWidth4 : (int)leftWidth4;
                    int h4 = bottom ? (int)bottomHeight4 : (int)topHeight4;

                    DrawArea = new Rectangle(x4, y4, w4, h4);
                    break;
            }

            return DrawArea;
        }



        ///// <summary>
        ///// For split screen play
        ///// </summary>
        //public Rectangle GetDrawArea(int numplayers, int myScreenIx, bool bHorizontalSplit, out float zoom)
        //{
        //    ScreenIndex = myScreenIx;
        //    verticalSplit = false; horizontalSplit = false;

        //    zoom = 38 * LootFest.LfLib.ModelsScaleUp;
        //    switch (numplayers)
        //    {
        //        default:
        //            DrawArea = new Rectangle(0, 0, Engine.Screen.Width, Engine.Screen.Height);
        //            break;
        //        case 2:

        //            if (bHorizontalSplit)
        //            {
        //                horizontalSplit = true;
        //                zoom *= 0.95f;
        //                int height = (int)(Engine.Screen.Height * PublicConstants.Half);
        //                DrawArea = new Rectangle(0, height * myScreenIx, Engine.Screen.Width, height);
        //            }
        //            else
        //            {
        //                verticalSplit = true;
        //                zoom *= 1.6f;

        //                int width = (int)(Engine.Screen.Width * PublicConstants.Half);
        //                DrawArea = new Rectangle(width * myScreenIx, 0, width, Engine.Screen.Height);
        //            }
        //            break;
        //        case 3:
        //            if (bHorizontalSplit)
        //            {
        //                DrawArea = Rectangle.Empty;
        //                horizontalSplit = true;
        //                DrawArea.Height = (int)(Engine.Screen.Height * PublicConstants.Half);
        //                if (myScreenIx == 0)
        //                {
        //                    DrawArea.Width = Engine.Screen.Width;
        //                    zoom *= 0.95f;
        //                }
        //                else
        //                {
        //                    verticalSplit = true;
        //                    DrawArea.Width = (int)(Engine.Screen.Width * PublicConstants.Half);
        //                    DrawArea.X = (myScreenIx - 1) * DrawArea.Width;
        //                    DrawArea.Y = DrawArea.Height;
        //                }
        //            }
        //            else
        //            {
        //                verticalSplit = true;
        //                DrawArea = Rectangle.Empty;

        //                DrawArea.Width = (int)(Engine.Screen.Width * PublicConstants.Half);
        //                if (myScreenIx == 0)
        //                {
        //                    DrawArea.Height = Engine.Screen.Height;
        //                    zoom *= 1.1f;
        //                }
        //                else
        //                {
        //                    horizontalSplit = true;
        //                    DrawArea.X = DrawArea.Width;
        //                    DrawArea.Height = (int)(Engine.Screen.Height * PublicConstants.Half);
        //                    DrawArea.Y = (myScreenIx - 1) * DrawArea.Height;
        //                }
        //            }
        //            break;
        //        case 4:
        //            verticalSplit = true; horizontalSplit = true;
        //            int height2 = (int)(Engine.Screen.Height * PublicConstants.Half);
        //            int width2 = (int)(Engine.Screen.Width * PublicConstants.Half);
        //            DrawArea = new Rectangle(
        //                (myScreenIx == 1 || myScreenIx == 3) ? width2 : 0, //x
        //                myScreenIx >= 2 ? height2 : 0, //y
        //                width2, height2);
        //            break;
        //    }

        //    return DrawArea;
        //}

        public void FullScreenSetup()
        {
            SetDrawArea(1, 0, false, null);
        }

        public void SetDrawArea(int numplayers, int myScreenIx, bool updateCam, Player player)
        {
            float zoom;
            GetDrawArea(numplayers, myScreenIx, out zoom);
           
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

            //safeScreenArea.X//.Position += new Vector2(1);
            //safeScreenArea.Width = Bound.Max(safeScreenArea.Width, Engine.Screen.SafeArea.Width);
            //safeScreenArea.Height = Bound.Max(safeScreenArea.Height, Engine.Screen.SafeArea.Height);

            safeScreenArea = Engine.Screen.SafeArea.KeepSmallerRectInsideBound_Position(safeScreenArea);
            if (safeScreenArea.X == DrawAreaF.X)
            {
                safeScreenArea.AddToLeftSide( -SafeSpaceBetweenPlayers);
            }
            if (safeScreenArea.Right == DrawAreaF.Right)
            {
                safeScreenArea.Width -= SafeSpaceBetweenPlayers;
            }

        }
    }
}
