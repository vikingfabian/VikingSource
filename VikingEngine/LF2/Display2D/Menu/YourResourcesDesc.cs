//using System;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Game1.HUD;

//namespace VikingEngine.LF2
//{
//    /// <summary>
//    /// Mainly used to view how much money the player got left in the menu, data
//    /// </summary>
//    struct YourResourcesDescData : HUD.IMemberData
//    {
//        IconAndText[] resources;
//        public YourResourcesDescData(IconAndText[] resources)
//        {
//            this.resources = resources;
//        }
//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new YourResourcesDesc(args, resources);
//        }
//        public override string ToString()
//        {
//            return "Menu YourResourcesDesc data";
//        }
//        public string LinkCaption { get { return null; } }
//    }
//    /// <summary>
//    /// Mainly used to view how much money the player got left in the menu
//    /// </summary>
//    class YourResourcesDesc : HUD.AbsBlockMenuMember
//    {
//        List<Graphics.AbsDraw2D> images;
//        public YourResourcesDesc(HUD.MemberDataArgs args, IconAndText[] resources)
//            : base(args.menu.TitleBackg, args.width, args.layer)
//        {
//            Vector2 currentPos = new Vector2(10, 0);
//            Vector2 iconScale = new Vector2(24);
//            Vector2 textScale = new Vector2(0.5f);

//            images = new List<Graphics.AbsDraw2D>(resources.Length * 2);
//            foreach (IconAndText r in resources)
//            {
//                Graphics.Image icon = new Graphics.Image(r.Icon, currentPos, iconScale, args.layer);
//                currentPos.X += iconScale.X;
//                Graphics.TextS text = new Graphics.TextS(LoadedFont.PhoneText, currentPos, textScale, Graphics.Align.Zero, r.Text, Color.White, args.layer);
//                currentPos.X += text.MesureText().X + 10;

//                images.Add(icon); images.Add(text);
//            }

//            background.Height = 32;
//        }

//        public override ClickFunction Function
//        {
//            get { return ClickFunction.NotSelectable; }
//        }
//        public override string ToString()
//        {
//            return "Menu YourResourcesDesc";
//        }
//        public override void GoalY(float y, bool set)
//        {
//            const float Edge = 8;
//            base.GoalY(y, set);

//            for (int i = 0; i < images.Count; i++)
//            {
//                images[i].Ypos = background.Ypos + Edge;
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
//        public override void DeleteMe()
//        {
//            base.DeleteMe();
//            foreach (Graphics.AbsDraw2D img in images)
//            {
//                img.DeleteMe();
//            }
//        }
//        public override float Height
//        {
//            get
//            {
//                return base.Height;
//            }
//        }
//    }

    
//}

//    struct BlueprintIngredientButtonData : HUD.IMemberData
//    {
//        HUD.Link link;
//        Data.Gadgets.Ingredient ingredient;
//        SpriteName overridingButtonImage;

//        public BlueprintIngredientButtonData(HUD.Link link, Data.Gadgets.Ingredient ingredient, SpriteName overridingButtonImage)
//        {
//            this.overridingButtonImage = overridingButtonImage;
//            this.link = link;
//            this.ingredient = ingredient;
//        }

//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new BlueprintIngredientButton(args.OverridingButtonImage(overridingButtonImage), link, ingredient);
//        }
//        public string LinkCaption { get { return null; } }
//    }
//    class BlueprintIngredientButton : HUD.AbsBlockMenuMember
//    {
//        const float RowHeight = 30;
//        List<Graphics.AbsDraw2D> images = new List<Graphics.AbsDraw2D>();
//        List<float> imageYadjusting = new List<float>();

//        public BlueprintIngredientButton(HUD.MemberDataArgs args, HUD.Link link,
//            Data.Gadgets.Ingredient ingrediece)
//            : base(args, link)
//        {
//            background.Height = GadgetImage.ItemImageSize.Y;
//            background.PaintLayer += PublicConstants.LayerMinDiff;

//            Vector2 pos = background.Position;
//            pos.X += 10;
//            float startX = pos.X;
//            pos.Y = 0;

//            bool first = true;

//            foreach (GameObjects.Gadgets.GoodsType i in ingrediece.Alternatives)
//            {
//                string num = ingrediece.Amount.ToString();
//                if (!first)
//                {
//                    num = "or " + num;
//                }
//                images.Add(new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * 0.8f, Graphics.Align.Zero, num, Color.White, args.layer));
//                pos.X += num.Length * 10 + 12;
//                imageYadjusting.Add(pos.Y);

//                images.Add(new Graphics.Image(GameObjects.Gadgets.GadgetLib.GadgetIcon(i), pos,
//                    GadgetImage.IconSize * Vector2.One, args.layer));
//                pos.X += GadgetImage.IconSize;
//                imageYadjusting.Add(pos.Y);

//                if (pos.X > args.width - 10)
//                {
                    
//                    pos.X = startX;
//                    pos.Y += RowHeight;
//                    background.Height += RowHeight;
//                }

//                first = false;
//            }
//            description = "Materials used for the " + ingrediece.ToString();
//        }
        
//        public override void GoalY(float y, bool set)
//        {
//            const float Edge = 8;
//            base.GoalY(y, set);
        
//            for (int i = 0; i < images.Count; i++)
//            {
//                images[i].Ypos = background.Ypos + Edge + imageYadjusting[i];
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
//}
