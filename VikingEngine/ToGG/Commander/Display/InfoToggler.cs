//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using VikingEngine.HUD;

//namespace VikingEngine.ToGG.ToggEngine.Display2D
//{
//    class InfoToggler : AbsButtonGui
//    {
//        Graphics.ImageGroup textBox;
//        Graphics.TextBoxSimple text;
//        Graphics.Image textBg;

//        public InfoToggler(float bottomY)
//            :base(ButtonGuiSettings.Empty)
//        {
//            area = new VectorRect(new Vector2(Engine.Screen.SafeArea.X, bottomY - Engine.Screen.IconSize), Engine.Screen.IconSizeV2);

//            baseImage = new Graphics.Image(SpriteName.cmdInfoBobble, area.Position, area.Size, ImageLayers.Background4);

//            float textW = Engine.Screen.Height * 0.5f;

//            text = new Graphics.TextBoxSimple(LoadedFont.Regular, Vector2.Zero,
//                Engine.Screen.TextSizeV2, Graphics.Align.Zero, "", Color.Black, ImageLayers.AbsoluteTopLayer,
//                textW);
                        
//            text.PaintLayer = baseImage.PaintLayer;

            
//            textBg = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, ImageLayers.AbsoluteTopLayer);
//            textBg.LayerBelow(text);

//            textBox = new Graphics.ImageGroup(text, textBg);
//            textBox.SetVisible(false);
//        }

//        protected override void onMouseEnter(bool enter)
//        {
//            base.onMouseEnter(enter);

//            if (enter)
//            {
//                string info;

//                if (toggRef.gamestate.gameSetup.level == LevelEnum.NONE)
//                {
//                    info = "Reach " + toggLib.WinnerScore.ToString() + " victory points to win";
//                }
//                else
//                {
//                    info = toggRef.gamestate.gameSetup.missionName + ":" + Environment.NewLine +
//                        toggRef.gamestate.gameSetup.missionDescription;

//                    if (toggRef.gamestate.gameSetup.WinningConditions.HasTurnLimit)
//                    {
//                        info += Environment.NewLine + "Turn " + Commander.cmdRef.players.ActiveLocalPlayer().TurnsCount + "/" + toggRef.gamestate.gameSetup.WinningConditions.timeLimit.ToString();
//                    }
//                }

//                text.TextString = info;

//                Vector2 sz = text.MeasureText();

//                VectorRect textArea = new VectorRect(
//                    new Vector2(area.Right + Engine.Screen.SmallIconSize, area.Center.Y - sz.Y), sz);

//                text.Position = textArea.Position;

//                textArea.AddRadius(Engine.Screen.SmallIconSize * 0.5f);

//                textBg.Position = textArea.Position;
//                textBg.Size = textArea.Size;

//            }
//            textBox.SetVisible(enter);
//        }
//    }
//}
