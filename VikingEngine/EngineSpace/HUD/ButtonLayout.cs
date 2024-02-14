using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;

namespace VikingEngine.HUD
{
    class ButtonLayout : IDeleteable
    {
        public const float BackgTranparantsy = 0.6f;
        public static readonly Color BackgColor = Color.Black;
        public const ImageLayers ContentLayer = ImageLayers.Foreground1;
        public const ImageLayers BackgLayer = ContentLayer +1;


        List<IDeleteable> addOns = new List<IDeleteable>();
        List<Graphics.AbsDraw> images = new List<AbsDraw>();
        Image bg;

        public static List<ButtonDescriptionData> MenuDescription(bool tabs)
        {
            List<ButtonDescriptionData> data = new List<ButtonDescriptionData>
            {
                new ButtonDescriptionData("Move", SpriteName.LeftStick),
                new ButtonDescriptionData("Select", SpriteName.ButtonA),
                new ButtonDescriptionData("Back", SpriteName.ButtonB),
               // new ButtonDescriptionData("Scroll description", SpriteName.BoardCtrlRS),
            };
            if (tabs)
            {
                data.Add(new ButtonDescriptionData("Tab Left", SpriteName.ButtonLB));
                data.Add(new ButtonDescriptionData("Tab Right", SpriteName.ButtonRB));
            }
            else
            {
                data.Add(new ButtonDescriptionData("Page up", SpriteName.ButtonLB));
                data.Add(new ButtonDescriptionData("Page down", SpriteName.ButtonRB));
            }
            
            return data;
        }

        //public ButtonLayout(bool tabs)
        //{
        //    createImages(MenuDescription(tabs));
        //}

        //public ButtonLayout(List<ButtonDescriptionData> data)
        //{
        //    createImages(data);
        //}
        public ButtonLayout(List<ButtonDescriptionData> data, VectorRect screenArea, VectorRect safeArea, string title, string tip)
        {
            createImages(data, screenArea, safeArea, title, tip);
        }

        void createImages(List<ButtonDescriptionData> data)
        {
            this.createImages(data, new VectorRect(Vector2.Zero, Engine.Screen.ResolutionVec), Engine.Screen.SafeArea, null, null);
        }

        void createImages(List<ButtonDescriptionData> data, VectorRect screenArea, VectorRect safeArea, string title, string tip)
        {
            const float TextScale = 0.6f;
            const float Height = 32;
            if (screenArea.Y > 0) screenArea.AddToTopSide(-2);
            Vector2 currentPos = new Vector2(safeArea.X, safeArea.Bottom - Height);
            foreach (ButtonDescriptionData d in data)
            {
                TextG text = new TextG(LoadedFont.Regular, Vector2.Zero, VectorExt.V2(TextScale), Align.Zero, d.Title, Color.White, ContentLayer);
                images.Add(text);
                float lenght = text.MeasureText().X + Height;
                if (d.Ctrl != SpriteName.NO_IMAGE)
                    lenght += 40;
                //check if the current row is to long
                if (lenght + currentPos.X > safeArea.Right)
                {
                    //new row
                    currentPos.X = safeArea.X;
                    currentPos.Y -= (Height + 8);
                }
                if (d.Ctrl != SpriteName.NO_IMAGE)
                {
                    images.Add(new Image(d.Ctrl, currentPos, VectorExt.V2(Height), ContentLayer));
                    currentPos.X += Height;
                    images.Add(new TextG(LoadedFont.Regular, currentPos, VectorExt.V2(TextScale), Align.Zero, "+", Color.White, ContentLayer));
                    currentPos.X += 10;
                }
                images.Add(new Image(d.Button, currentPos, VectorExt.V2(Height), ContentLayer));
                currentPos.X += Height;
                text.Position = currentPos;
                currentPos.X += lenght;
            }
            //title
            if (screenArea.Width > 700 && title != null)
            {
                currentPos.Y -= 30;
                currentPos.X = safeArea.X;
                images.Add(new TextG(LoadedFont.Regular, currentPos, VectorExt.V2(TextScale * 0.9f), Align.Zero, title, Color.Gray, ContentLayer));
            }

            
            //black background
           // currentPos.Y -= 10;
            bg = new Image(SpriteName.WhiteArea, new Vector2(screenArea.Position.X, currentPos.Y),
                new Vector2(screenArea.Width + 20, screenArea.Bottom - currentPos.Y), BackgLayer);
            images.Add(bg);
            bg.Color = BackgColor;
            bg.Opacity = BackgTranparantsy;


            if (tip != null)
            {
                Graphics.TextBoxSimple tipText = new TextBoxSimple(LoadedFont.Regular, safeArea.Position, VectorExt.V2(TextScale), Align.Zero, "Tip: " + tip, 
                    Color.White, ContentLayer, safeArea.Width);
                images.Add(tipText);
                Image bg2 = new Image(SpriteName.WhiteArea, screenArea.Position,
                new Vector2(screenArea.Width, tipText.MeasureText().Y + 5 + safeArea.Y - screenArea.Y), BackgLayer);
                images.Add(bg2);
                bg2.Color = Color.Black;
                bg2.Opacity = BackgTranparantsy;
            }

            
        }

        public void AddOn(IDeleteable img)
        {
            addOns.Add(img);
        }

        public void DeleteMe()
        {
            foreach (Graphics.AbsDraw img in images)
            {
                img.DeleteMe();
            }
            images.Clear();
            DeleteAddOns();
        }
        public bool IsDeleted
        {
            get { return images.Count == 0; }
        }
        public void DeleteAddOns()
        {
            foreach (IDeleteable ao in addOns)
            {
                ao.DeleteMe();
            }
        }

        public float Y { get { return bg.Ypos; } }
    }

    struct ButtonDescriptionData
    {
        public string Title;
        public SpriteName Ctrl;
        public SpriteName Button;

        public ButtonDescriptionData(string title, SpriteName button)
            :this(title, SpriteName.NO_IMAGE, button)
        { }
        public ButtonDescriptionData(string title, SpriteName ctrl, SpriteName button)
        {
            this.Title = title;
            this.Ctrl = ctrl;
            this.Button = button;
        }
    }
}
