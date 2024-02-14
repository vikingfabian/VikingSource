using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using VikingEngine.LF2.GameObjects.Gadgets;

namespace VikingEngine.LF2
{
    //struct RefineMaterialButtonData : HUD.IMemberData
    //{
    //    HUD.IMenuLink link;
    //    Goods[] fromItems; 
    //    Goods toItem;
    //    SpriteName overridingButtonImg;

    //    public RefineMaterialButtonData(HUD.IMenuLink link, Goods[] fromItems, Goods toItem, SpriteName overridingButtonImg)
    //    {
    //        this.overridingButtonImg = overridingButtonImg;
    //        this.link = link;
    //        this.fromItems = fromItems;
    //        this.toItem = toItem;
    //    }

    //    public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
    //    {
    //        return new RefineMaterialButton(args, link, fromItems, toItem, overridingButtonImg);
    //    }
    //    public string LinkCaption { get { return null; } }
    //}
    //class RefineMaterialButton : AbsCraftingButton
    //{
    //    public RefineMaterialButton(HUD.MemberDataArgs args, HUD.IMenuLink link,
    //        Goods[] fromItems, Goods toItem, SpriteName overridingButtonImg)
    //        : base(args.OverridingButtonImage(overridingButtonImg), link, "Refine " + TextLib.EnumName(fromItems[0].Type.ToString()))
    //    {
    //        const float IconToQualSpace = GadgetImage.IconSize * 0.2f;
    //        background.Height = GadgetImage.ItemImageSize.Y;
    //        background.PaintLayer += PublicConstants.LayerMinDiff;

    //        Vector2 pos = background.Position;
    //        pos.X += 10;

            
    //        SpriteName qualityImg = (SpriteName)((int)SpriteName.IconQualityLow + (int)fromItems[0].Quality);

    //        for (int i = 0; i < fromItems.Length; ++i)
    //        {
    //            if (i != 0)
    //            {
    //                images.Add(new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, "+", Color.White, args.layer));
    //                pos.X += 14;
    //            }

    //            if (fromItems[i].Amount > 1)
    //            {
    //                string amountText = fromItems[i].Amount.ToString();
    //                images.Add(new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, amountText, Color.White, args.layer));
    //                pos.X += amountText.Length * 12;
    //            }

    //            images.Add(new Graphics.Image(GameObjects.Gadgets.GadgetLib.GadgetIcon(fromItems[i].Type), pos,
    //                GadgetImage.IconSize * Vector2.One, args.layer));
    //            pos.X += IconToQualSpace;

    //            //view quality
    //            images.Add(new Graphics.Image(qualityImg, pos,
    //               GadgetImage.IconSize * Vector2.One, args.layer));
    //            pos.X += GadgetImage.IconSize * 1.1f;
    //        }

    //        Graphics.TextG resultText = new Graphics.TextG(LoadedFont.Lootfest, pos, Vector2.One * TextScale, Graphics.Align.Zero, "=>", Color.White, args.layer);
    //        images.Add(resultText);
            
    //        //result image
    //        pos.X += resultText.MesureText().X + 5;
    //        images.Add(new Graphics.Image(toItem.Icon, pos,
    //                GadgetImage.IconSize * Vector2.One, args.layer));
            
    //        pos.X += IconToQualSpace;
    //        images.Add(new Graphics.Image(++qualityImg, pos,
    //                GadgetImage.IconSize * Vector2.One, args.layer));

    //       // description = "Refine " + TextLib.EnumName(fromItems[0].Type.ToString());
    //    }
    //}
}
