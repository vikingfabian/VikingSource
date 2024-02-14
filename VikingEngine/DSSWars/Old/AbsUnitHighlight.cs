//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.RTS.GameObject;
//using Game1.RTS.Map;

//namespace Game1.RTS.Display
//{
//    /// <summary>
//    /// Let info about the unit be shown around it
//    /// </summary>
//    abstract class AbsUnitHighlight
//    {
       

//        protected List<IDeleteable> images;

//        public void DeleteMe(Player p)
//        {
//            //Ref.draw.CurrentRenderLayer = RTSlib.MapLayer;
//            p.SetPlayerLayer();

//            foreach (IDeleteable img in images)
//                img.DeleteMe();
//        }

//        bool visible = true;
//        public bool Visible
//        {
//            set
//            {
//                if (value != visible)
//                {
//                    visible = value;
//                    foreach (IDeleteable img in images)
//                    {
//                        if (img is Graphics.AbsDraw)
//                        {
//                            ((Graphics.AbsDraw)img).Visible = visible;
//                        }
//                    }
//                }
//            }
//        }

//        abstract public void update();
//    }

    

    
//}
