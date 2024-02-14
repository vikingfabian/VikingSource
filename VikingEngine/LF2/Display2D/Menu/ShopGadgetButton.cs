//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LF2
//{
//    struct ShopGadgetButtonData : HUD.IMemberData
//    {
//        HUD.Link link;
//        GameObjects.Gadgets.ShopItem gadget;

//        public ShopGadgetButtonData(HUD.Link link, GameObjects.Gadgets.ShopItem gadget)
//        {
//            this.link = link;
//            this.gadget = gadget;
//        }

//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new ShopGadgetButton(args, link, gadget);
//        }
//        public string LinkCaption { get { return null; } }
//    }


//    class ShopGadgetButton : HUD.AbsBlockMenuMember
//    {
//        protected GadgetImage image;
//        Graphics.TextG priceLabel;

//        public ShopGadgetButton(HUD.MemberDataArgs args, HUD.Link link, GameObjects.Gadgets.ShopItem gadget)
//            : base(SpriteName.LFMenuItemBackgroundShop, 0, args.layer, link, gadget.ToString())//link, dialoge)
//        {
//            background.Size = GadgetImage.ShopImageSize;
//            background.PaintLayer += PublicConstants.LayerMinDiff;
//            image = new GadgetImage(args.layer, gadget.Item, true);
//                //description = gadget.ToString();

//            priceLabel = new Graphics.TextG(LoadedFont.PhoneText, Vector2.Zero, Vector2.One * 0.5f, Graphics.Align.CenterAll, gadget.Price.ToString(),
//                Color.Black, args.layer);
//            priceLabel.Rotation = -0.5f;
//        }

//        override public void SetGroupPos(int xIndex)
//        {
//            background.Xpos += background.Width * xIndex;
//            image.Xpos = background.Xpos;
//            priceLabel.Xpos = background.Xpos + background.Width * 0.8f;
//        }
//        public override void GoalY(float y, bool set)
//        {
//            base.GoalY(y, set);
//            image.Ypos = background.Ypos;
//            priceLabel.Ypos = background.Ypos + background.Height * 0.5f;
//        }
//        public override bool Visible
//        {
//            set
//            {
//                base.Visible = value;
//                image.Visible = value;
//                priceLabel.Visible = value;
//            }
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            image.DeleteMe();
//            priceLabel.DeleteMe();
//        }
//        override public bool WidthStackable { get { return true; } }
//        public override HUD.ClickFunction Function
//        {
//            get { return HUD.ClickFunction.Link; }
//        }
//    }
//}
