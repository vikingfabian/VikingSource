//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.HUD;
//using Game1.Graphics;
//using Game1.HUD;

//namespace VikingEngine.LF2
//{

//    struct LargeShopButtonData : HUD.IMemberData
//    {
//        SpriteName iconTile; string text, description; int price; IMenuLink link;
//        bool canPay;

//        public LargeShopButtonData(SpriteName iconTile, string text, string description, int price, int playerCoins, IMenuLink link)
//        {
//            this.iconTile = iconTile;
//            this.text = text;
//            this.description = description;
//            this.price = price;
//            this.link = link;
//            canPay = price <= playerCoins;
//        }

//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new LargeShopButton(args, iconTile, text, description, price, canPay, link);
//        }
//        public string LinkCaption { get { return null; } }
//    }
    
//    class LargeShopButton : AbsBlockMenuMember
//    {
//        protected TextS text;
//        Graphics.TextG priceLabel;
//        protected Image icon = null;
//        const float IconEdge = 4;
//        const float EdgeTwice = IconEdge * PublicConstants.Twice;
//        static readonly Vector2 PosAdd = new Vector2(IconEdge);

//        public LargeShopButton(MemberDataArgs args, SpriteName iconTile, string text, string description, int price, bool canPay, IMenuLink link)
//            : base(args.OverridingButtonImage(SpriteName.LFMenuLargeShopButton), link, description)
//        {
//           // this.description = description;
//            background.Height = args.Height;
//            float xpos = IconEdge;
//            if (iconTile != SpriteName.NO_IMAGE)
//            {
//                this.icon = new Image(iconTile, background.Position + PosAdd, new Vector2(args.Height - EdgeTwice), args.layer);
//                xpos = icon.Right;
//            }
//            this.text = new TextS(args.menu.TextFormat.Font, background.Position, args.menu.TextFormat.Scale, Align.Zero,
//                text, args.menu.TextFormat.Color, args.layer);
//            this.text.Xpos += xpos;

//            this.text.SetMaxWidth(background.Width - xpos);

//            priceLabel = new Graphics.TextG(LoadedFont.PhoneText, Vector2.Zero, Vector2.One * 0.5f, Graphics.Align.CenterAll, price.ToString(),
//                canPay? Color.Black : Color.Red, args.layer);
//            priceLabel.Rotation = -0.5f;
//            priceLabel.Xpos = background.Xpos + background.Width * 0.9f;


//            detailColor = args.menu.TextFormat.Color;
//        }
//        public override void GoalY(float y, bool set)
//        {
//            base.GoalY(y, set);
//            if (icon != null) icon.Ypos = y + IconEdge;
//            text.Ypos = y +
//                (PlatformSettings.RunProgram == StartProgram.TableTop ? 0 : 5);

//            priceLabel.Ypos = background.Ypos + background.Height * 0.5f;
//        }

//        public override bool Selected
//        {
//            set
//            {
//                if (value)
//                {
//                    background.Color = detailColor;

//                    text.Color = BackgroundCol;
//                    if (icon != null) icon.Color = BackgroundCol;
//                }
//                else
//                {
//                    background.Color = BackgroundCol;

//                    text.Color = detailColor;
//                    if (icon != null) icon.Color = detailColor;
//                }
//            }
//        }

//        public override bool Visible
//        {
//            set
//            {
//                base.Visible = value;
//                text.Visible = value;
//                if (icon != null) icon.Visible = value;
//                priceLabel.Visible = value;
//            }
//        }
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            text.DeleteMe();
//            if (icon != null) icon.DeleteMe();
//            priceLabel.DeleteMe();
//        }

//        public override ClickFunction Function
//        {
//            get { return ClickFunction.Link; }
//        }

//        public override string ToString()
//        {
//            return "Icon & Text button {" + text.TextString + "}";
//        }
//    }

//}
