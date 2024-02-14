//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.ToGG.ToggEngine.Display2D
//{
    //class NextPhaseWarningPopup
    //{
    //    Graphics.ImageGroup images;
    //    Time viewTime = new Time(2f, TimeUnit.Seconds);

    //    public NextPhaseWarningPopup(string toolwarning)
    //    {
    //        VectorRect warningArea;

    //        //Warning text
    //        {
    //            float textBorder = (int)(Engine.Screen.IconSize * 0.4f);
    //            float width = HudLib.PrevPhaseButtonsSz.X * 8f;

    //            Graphics.TextBoxSimple warningText = new Graphics.TextBoxSimple(
    //                LoadedFont.Regular,
    //                Vector2.Zero,
    //                Engine.Screen.TextSizeV2 * 1.2f,
    //                Graphics.Align.Zero,
    //                toolwarning, Color.White,
    //                HudLib.PopupLayer,
    //                width - textBorder * 2f);

    //            Graphics.TextBoxSimple textborder = (Graphics.TextBoxSimple)warningText.CloneMe();
    //            textborder.Color = Color.Black;
    //            textborder.LayerBelow(warningText);

    //            Vector2 sz = warningText.MeasureText();

    //            warningArea = VectorRect.FromCenterSize(Engine.Screen.CenterScreen, sz);
    //            warningText.Position = warningArea.Position;
    //            textborder.Position = warningText.Position + new Vector2(2);

    //            warningArea.AddRadius(textBorder);
    //            Graphics.Image warningBg = new Graphics.Image(SpriteName.WhiteArea,
    //                warningArea.Position, warningArea.Size, ImageLayers.AbsoluteTopLayer);
    //            warningBg.Color = Color.Red;
    //            warningBg.LayerBelow(textborder);

    //            Graphics.Image warningIcon = new Graphics.Image(SpriteName.cmdWarningTriangle,
    //                VectorExt.AddX(warningArea.Position, -Engine.Screen.IconSize),
    //                Engine.Screen.IconSizeV2, ImageLayers.AbsoluteBottomLayer);
    //            warningIcon.PaintLayer = warningBg.PaintLayer;

    //            images = new Graphics.ImageGroup(
    //                warningBg, warningText, textborder, warningIcon);
    //        }

    //        //Press Again
    //        {
    //            VectorRect pressAgainArea = new VectorRect(
    //                warningArea.X,
    //                warningArea.Bottom + Engine.Screen.BorderWidth,
    //                warningArea.Width,
    //                Engine.Screen.IconSize + Engine.Screen.BorderWidth * 2f);

    //            Graphics.TextG press = new Graphics.TextG(LoadedFont.Regular, pressAgainArea.LeftCenter, Vector2.One, Graphics.Align.CenterHeight,
    //                "Press ", Color.Black, HudLib.PopupLayer);
    //            press.SetHeight(Engine.Screen.IconSize);

    //            Graphics.TextG again = (Graphics.TextG)press.CloneMe();
    //            again.TextString = " again";

    //            float pressAgainWidth = press.MeasureText().X + Engine.Screen.IconSize + again.MeasureText().X;

    //            press.Xpos = pressAgainArea.Center.X - pressAgainWidth * 0.5f;
    //            Graphics.Image inputIcon = new Graphics.Image(toggRef.inputmap.nextPhase.Icon,//cmdLib.Button_NextPhase.Icon,
    //                new Vector2(press.MeasureRightPos(), pressAgainArea.Center.Y), Engine.Screen.IconSizeV2, HudLib.PopupLayer);
    //            inputIcon.origo = VectorExt.V2HalfY;

    //            again.Xpos = inputIcon.Right;

    //            Graphics.Image pressAgainBg = new Graphics.Image(SpriteName.WhiteArea,
    //                pressAgainArea.Position, pressAgainArea.Size,
    //                ImageLayers.AbsoluteBottomLayer);
    //            pressAgainBg.LayerBelow(press);

    //            images.Add(press); images.Add(inputIcon); images.Add(again); 
    //            images.Add(pressAgainBg); 
    //        }

    //    }

    //    public bool Update()
    //    {
    //        return viewTime.CountDown();
    //    }

    //    public void DeleteMe()
    //    {
    //        images.DeleteAll();
    //    }
    //}
//}
