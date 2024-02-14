using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{

    //struct EmptyGadgetButtonData : HUD.IMemberData
    //{
    //    bool equip;
    //    GadgetLink link;
    //    //int dialoge;
    //    SpriteName icon; 
    //    string desc;

    //    public EmptyGadgetButtonData(LF2.GadgetLinkEvent link, SpriteName icon, string desc, bool equip)
    //        : this(new GadgetLink(link, null, null, null), icon, desc, equip)
    //    { }

    //    public EmptyGadgetButtonData(GadgetLink link, SpriteName icon, string desc, bool equip)
    //    {
    //        this.link = link;
    //        //this.dialoge = dialoge;
    //        this.icon = icon;
    //        this.desc = desc;
    //        this.equip = equip;
    //    }

    //    public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
    //    {
    //        return new EmptyGadgetButton(args, link, icon, desc, equip);
    //    }
    //    public string LinkCaption { get { return null; } }
    //}

    //class EmptyGadgetButton: HUD.AbsBlockMenuMember
    //{
    //    protected  Graphics.Image image;
    //    const float IconTransparentsy = 0.5f;

    //    public EmptyGadgetButton(HUD.MemberDataArgs args, GadgetLink link, SpriteName icon, string desc, bool equip)
    //        : base(equip? SpriteName.LFMenuItemBackgroundGray : SpriteName.LFMenuItemBackground, GadgetImage.ItemImageSize.X, args.layer, link, desc)
    //    {
    //        background.Height = GadgetImage.ItemImageSize.Y;
    //        background.PaintLayer += PublicConstants.LayerMinDiff;

    //        image = new Graphics.Image(icon, Vector2.Zero, GadgetImage.IconSize * Vector2.One, args.layer);
    //        image.Transparentsy = IconTransparentsy;
    //        //description = desc;
    //    }
    //    override public void SetGroupPos(int xIndex) 
    //    {
    //        background.Xpos += background.Width * xIndex;
    //        image.Xpos = background.Xpos + GadgetImage.ItemImageSize.X * 0.35f;
    //    }
    //    public override void GoalY(float y, bool set)
    //    {
    //        base.GoalY(y, set);
    //        image.Ypos = background.Ypos + GadgetImage.Edge;
    //    }
    //    public override bool Visible
    //    {
    //        set
    //        {
    //            base.Visible = value;
    //            image.Visible = value;
    //        }
    //    }
        
    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        image.DeleteMe();
    //    }
    //    override public bool WidthStackable { get { return true; } }
    //    public override HUD.ClickFunction Function
    //    {
    //        get { return HUD.ClickFunction.Link; }
    //    }

    //    public override string ToString()
    //    {
    //        return "Empty Gadget button";
    //    }
    //}
}
