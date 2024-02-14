//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.HUD;

//namespace VikingEngine.Commander
//{
//    class WarningMessage
//    {
//        Graphics.ImageGroup images;
//        Action okEvent, cancelEvent;

//        public WarningMessage(string title, string text, string okText, Action okEvent, string cancelText, Action cancelEvent, VectorRect area)
//        {
//            this.okEvent = okEvent;
//            this.cancelEvent = cancelEvent;

//            Graphics.TextBoxSimple textBox = new Graphics.TextBoxSimple(LoadedFont.PhoneText, Vector2.Zero, new Vector2(Engine.Screen.TextSize),
//                Graphics.Align.Zero, text, Color.White, ImageLayers.Foreground4, area.Width * 0.5f);

//            Vector2 boxSz = textBox.MeasureText();
//            textBox.Position = area.Center - boxSz * 0.5f;

//            Graphics.TextG titleText = new Graphics.TextG(LoadedFont.PhoneText, textBox.Position, new Vector2(Engine.Screen.TextSize * 1.3f),
//                Graphics.Align.Zero, title, Color.LightBlue, ImageLayers.Foreground4);
//            titleText.Ypos -= titleText.MeasureText().Y;

//            Vector2 iconSz = new Vector2(Engine.Screen.IconSize);
//            Vector2 optTextScale = new Vector2(Engine.Screen.TextSize);
//            Vector2 position = textBox.Position;
//            position.Y += boxSz.Y;

//            Graphics.Image Abutton = new Graphics.Image(SpriteName.ButtonA, position, iconSz, ImageLayers.Foreground4);
//            Graphics.TextG Atext = new Graphics.TextG(LoadedFont.PhoneText, Abutton.RightCenter, optTextScale, Graphics.Align.CenterHeight,
//                okText, Color.White, ImageLayers.Foreground4);

//            position.Y += iconSz.Y * 1.1f;
//            Graphics.Image Bbutton = new Graphics.Image(SpriteName.ButtonB, position, iconSz, ImageLayers.Foreground4);
//            Graphics.TextG Btext = new Graphics.TextG(LoadedFont.PhoneText, Bbutton.RightCenter, optTextScale, Graphics.Align.CenterHeight,
//                cancelText, Color.White, ImageLayers.Foreground4);

//            VectorRect bgarea = new VectorRect(titleText.Position, new Vector2(boxSz.X, Bbutton.Bottom - titleText.Ypos));
//            bgarea.AddRadius(6f);
//            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, bgarea.Position, bgarea.Size, ImageLayers.Background5);
//            bg.Color = Color.Black;
//            bg.Transparensy = 0.5f;

//            images = new Graphics.ImageGroup(new List<Graphics.AbsDraw> 
//            {
//                titleText, textBox, Abutton, Atext, Bbutton, Btext, bg
//            });
//        }


//        public DialogueResult Update(Input.AbsControllerInstance controller)
//        {
//            if (controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.A))
//            {
//                if (okEvent != null) okEvent();
//                return DialogueResult.Yes;
//            }
//            else if (controller.KeyDownEvent(Microsoft.Xna.Framework.Input.Buttons.B))
//            {
//                if (cancelEvent != null) cancelEvent();
//                return DialogueResult.No;
//            }
//            return DialogueResult.NoResult;
//        }

//        public void DeleteMe()
//        {
//            images.DeleteAll();
//        }
//    }

//}
