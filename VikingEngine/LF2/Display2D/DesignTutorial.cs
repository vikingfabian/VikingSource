using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;
using System.Threading;

namespace VikingEngine.LF2
{
    class DesignTutorial :  AbsInput
    {
        //static readonly Range Pages = new Range(1, 8);
        static int NumPages = PlatformSettings.ViewUnderConstructionStuff? 10 : 9;
        int currentPage = 2;
        static readonly IntVector2 TextureSize = new IntVector2(512, 256);
        ImageAdvanced image;
        Image background;
        Thread loadingThread = null;
        TextBoxSimple desc;

        public DesignTutorial()
        {
            background = new Image(SpriteName.WhiteArea, new Vector2(-100), Engine.Screen.Resolution + new Vector2(200), 
                ImageLayers.Foreground3);
            background.Color = Color.Black;
            Vector2 imageSize = Vector2.Zero;
            imageSize.X = Engine.Screen.Width * 0.9f;
            imageSize.Y = (imageSize.X / TextureSize.X) * TextureSize.Y;
            image = new ImageAdvanced(SpriteName.WhiteArea, Engine.Screen.Resolution * 0.05f, imageSize,
                ImageLayers.Foreground2, false);
            

            desc = new TextBoxSimple(LoadedFont.Lootfest, new Vector2(Engine.Screen.Resolution.X * 0.1f, 
                Engine.Screen.Resolution.Y * 0.85f),
                new Vector2(1), Align.Zero, TextLib.EmptyString, Color.White, ImageLayers.Foreground1, 
                Engine.Screen.Resolution.X * 0.8f);

            updateImage();
        }

        new public bool Button_Event(ButtonValue e)
        {
            if (e.KeyDown)
            {
                if (e.Button == numBUTTON.B || e.Button == numBUTTON.Back || e.Button == numBUTTON.Start)
                    return true;
                if (e.Button == numBUTTON.A || e.Button == numBUTTON.X || e.Button == numBUTTON.RB)
                {
                    currentPage++;
                    if (currentPage <= NumPages)
                    {
                        updateImage();
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void updateImage()
        {
            string text;
            switch (currentPage)
            {
                default:
                    text = "Dpad Down: Start building";
                    break;
                case 2:
                    text = "RB: Add";
                    break;
                case 3:
                    text = "LB: Remove";
                    break;
                case 4:
                    text = "START: Options";
                    break;
                case 5:
                    text = "LT: Camera Zoom/Rotate";
                    break;
                case 6:
                    text = "RT: Select";
                    break;
                case 7:
                    text = "Selection, move and stamp";
                    break;
                case 8:
                    text = "START: selection options";
                    break;
                case 9:
                    text = "With the pyramid and cone tools, draw the base first";
                    break;
                case 10:
                    text = "Create doors";
                    break;

            }
            desc.TextString = text;

            if (loadingThread != null)
            {
                loadingThread.Abort();
            }
            image.Texture = Engine.LoadContent.Texture(LoadedTexture.WhiteArea);
            image.Color = Color.Black;
            loadingThread = new Thread(loadTexture);
            loadingThread.Name = "DesignTutorial";
            loadingThread.Start();
        }
        void loadTexture()
        {
            image.Texture = Engine.LoadContent.LoadTexture("Texture\\tutorial" + currentPage.ToString());
            image.ImageSource = new Rectangle(0, 0, TextureSize.X, TextureSize.Y);
            image.Color = Color.White;
            loadingThread = null;
        }
        public override void DeleteMe()
        {
            desc.DeleteMe();
            if (loadingThread != null)
            {
                loadingThread.Abort();
            }
            image.DeleteMe();
            background.DeleteMe();
        }
    }
}
