//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;

//namespace Game1.RTS.Display
//{
//    /// <summary>
//    /// Create a bar with info about the obj
//    /// </summary>
//    abstract class AbsSelectedObjDialogue
//    {
//        protected WorldData world;
//        //protected Graphics.Image bg;
//        //protected Graphics.TextBoxSimple info;
//        protected ButtonsOverview buttonsOverview;

//        public AbsSelectedObjDialogue(WorldData world, ButtonsOverview buttonsOverview)
//        {
//           // RTSlib.SetHUDLayer();
//            this.world = world;
//            this.buttonsOverview = buttonsOverview;
//        }

//        //protected void createBar(string infoTxt)
//        //{
//        //    const float BarHeight = 60;

//        //    VectorRect area = Engine.Screen.SafeArea;
//        //    area.Position.Y = Engine.Screen.SafeArea.RightMostY - BarHeight;

//        //    bg = new Graphics.Image(TileName.WhiteArea, area.Position, area.Size, RTSlib.LayerUnitDialogueBackground);
//        //    bg.Color = Color.LightGreen;
//        //    bg.Transparentsy = 0.7f;

//        //    //title = new Graphics.TextG(LoadedFont.PhoneText, new Vector2(area.Center.X, area.Y + 10), new Vector2(1f), Graphics.Align.CenterWidth,
//        //    //    titleTxt, Color.Black, ImageLayers.Background6);
//        //    area.Y += 5;
//        //    info = new Graphics.TextBoxSimple(LoadedFont.PhoneText, area.Position, new Vector2(0.8f), Graphics.Align.Zero,
//        //        //info text
//        //        infoTxt,
//        //        Color.Black, RTSlib.LayerUnitDialogueBackground - 1, area.SizeX);
//        //}

//        /// <returns>Close dialogue</returns>
//        abstract public bool UpdateInput(Input.AbsControllerInstance controller);

//        virtual public void DeleteMe()
//        {
//            //Ref.draw.CurrentRenderLayer = RTSlib.HUDLayer;
//           // RTSlib.SetHUDLayer();
//            //bg.DeleteMe();
//            ////title.DeleteMe();
//            //info.DeleteMe();
//        }
//    }
//}
