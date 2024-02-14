//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace VikingEngine.LF2
//{
//    class AbsCraftingButton : HUD.AbsBlockMenuMember
//    {
//        protected const float TextScale = 0.8f;
//        protected List<Graphics.AbsDraw2D> images = new List<Graphics.AbsDraw2D>();

//        public AbsCraftingButton(HUD.MemberDataArgs args, HUD.IMenuLink link, string desc)
//            : base(args, link, desc)
//        {
//        }

//        public override void GoalY(float y, bool set)
//        {
//            const float Edge = 8;
//            base.GoalY(y, set);
//            foreach (Graphics.AbsDraw2D img in images)
//            {
//                img.Ypos = background.Ypos + Edge;
//            }
//        }
//        public override bool Visible
//        {
//            set
//            {
//                base.Visible = value;
//                foreach (Graphics.AbsDraw2D img in images)
//                {
//                    img.Visible = value;
//                }
//            }
//        }
//        //public override float Transparentsy
//        //{
//        //    get
//        //    {
//        //        return base.Transparentsy;
//        //    }
//        //    set
//        //    {
//        //        base.Transparentsy = value;
//        //        foreach (Graphics.AbsDraw2D img in images)
//        //        {
//        //            img.Transparentsy = value;
//        //        }
//        //    }
//        //}
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            foreach (Graphics.AbsDraw2D img in images)
//            {
//                img.DeleteMe();
//            }
//        }
//        public override HUD.ClickFunction Function
//        {
//            get { return HUD.ClickFunction.Link; }
//        }
//    }
//}
