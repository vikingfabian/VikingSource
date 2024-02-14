using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.LF2
//{
//    struct CraftingButtonData : HUD.IMemberData
//    {
//        Data.Gadgets.BluePrint bpType;
//        HUD.Link link;
//        Data.Gadgets.BlueprintIngrediens ingredieces;

//        public CraftingButtonData(HUD.Link link, Data.Gadgets.BlueprintIngrediens ingredieces, Data.Gadgets.BluePrint bpType)
//        {
//            this.bpType = bpType;
//            this.link = link;
//            this.ingredieces = ingredieces;
//        }

//        public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
//        {
//            return new CraftingButton(args, link, ingredieces, bpType);
//        }
//        public string LinkCaption { get { return null; } }
//    }
//    class CraftingButton : AbsCraftingButton
//    {
        
//        public CraftingButton(HUD.MemberDataArgs args, HUD.Link link, 
//            Data.Gadgets.BlueprintIngrediens ingredieces, Data.Gadgets.BluePrint bpType)
//            : base(args, link, "Craft " + ingredieces.Name)
//        {
            
//            background.Height = GadgetImage.ItemImageSize.Y;
//            background.PaintLayer += PublicConstants.LayerMinDiff;

//            Vector2 pos = background.Position;
//            pos.X += 10;

//            bool first = true;

//            foreach (Data.Gadgets.Ingredient i in ingredieces.Requierd)
//            {
//                if (!first)
//                {
//                    images.Add(new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, "+", Color.White, args.layer));
//                    pos.X += 14;
//                }

//                if (i.Amount > 1)
//                {
//                    string amountText = i.Amount.ToString();
//                    images.Add(new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, amountText, Color.White, args.layer));
//                    pos.X += amountText.Length * 12;
//                }

//                images.Add(new Graphics.Image(GameObjects.Gadgets.GadgetLib.GadgetIcon(i.Alternatives[0]), pos, 
//                    GadgetImage.IconSize * Vector2.One, args.layer));
//                pos.X += GadgetImage.IconSize * 1.1f;

//                first = false;
//            }

//            Graphics.TextG resultText = new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, "=>", Color.White, args.layer);
//            images.Add(resultText);
//            if (Data.Gadgets.BluePrintLib.ProductionAmount(bpType) > 1)
//            {
//                resultText.TextString += " " + Data.Gadgets.BluePrintLib.ProductionAmount(bpType).ToString();
//            }
//            pos.X += resultText.MesureText().X + 5;
//            images.Add(new Graphics.Image(ingredieces.Icon, pos,
//                    GadgetImage.IconSize * Vector2.One, args.layer));

//            //description = "Craft " + ingredieces.Name;
//        }

//    }
//}
