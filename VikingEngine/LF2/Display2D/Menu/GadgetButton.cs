using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Graphics;

namespace VikingEngine.LF2
{
    //struct GadgetButtonData : HUD.IMemberData
    //{
    //    SpriteName type;
    //    HUD.IMenuLink link;
    //    GameObjects.Gadgets.IGadget gadget;
    //    bool viewAmount;

    //    public GadgetButtonData(GameObjects.Gadgets.IGadget gadget, SpriteName type, bool viewAmount)
    //        : this(new GadgetLink(), gadget, type, viewAmount)
    //    { }
    //    public GadgetButtonData(GadgetLinkEvent link, GameObjects.Gadgets.IGadget gadget, SpriteName type, bool viewAmount)
    //        : this(new GadgetLink(link, null, null, null), gadget, type, viewAmount)
    //    { }
    //    public GadgetButtonData(HUD.IMenuLink link, GameObjects.Gadgets.IGadget gadget, SpriteName type, bool viewAmount)
    //    {
    //        if (link is GadgetLink)
    //        {
    //            GadgetLink glink = (GadgetLink)link;

    //            if (glink.Gadget == null)
    //            {
    //                glink.Gadget = gadget;
    //                link = glink;
    //            }
    //        }
    //        this.viewAmount = viewAmount;
    //        this.link = link;
    //        this.gadget = gadget;
    //        this.type = type;
    //    }

    //    public HUD.AbsBlockMenuMember Generate(HUD.MemberDataArgs args)
    //    {
    //        return new GadgetButton(args, link, gadget, type, viewAmount);
    //    }
    //    public string LinkCaption { get { return null; } }
    //}

    
    //class GadgetButton : HUD.AbsBlockMenuMember
    //{
    //    protected GadgetImage image;

    //    public GadgetButton(HUD.MemberDataArgs args, HUD.IMenuLink link, 
    //        GameObjects.Gadgets.IGadget gadget, SpriteName type, bool viewAmount)
    //        : base(type, 0, args.layer, link, null)
    //    {
    //        if (equipButtonType(type))
    //        {
    //            background.Size = LoadTiles.EquipButtonSize;
    //        }
    //        else
    //        {
    //            background.Size = GadgetImage.ItemImageSize;
    //        }
            
    //        background.PaintLayer += PublicConstants.LayerMinDiff;

    //        if (gadget != null)
    //        {
    //            image = new GadgetImage(args.layer, gadget, viewAmount);
    //            description = gadget.ToString();
    //        }
    //        else
    //        {
    //            description = "Empty, click to equip";
    //        }
    //    }

    //    bool equipButtonType(SpriteName type)
    //    {
    //        return 
    //            type == SpriteName.LFMenuEquipButtonA || 
    //            type == SpriteName.LFMenuEquipButtonB || 
    //            type == SpriteName.LFMenuEquipButtonX || 
    //            type == SpriteName.LFMenuEquipButtonLB || 
    //            type == SpriteName.LFMenuEquipButtonRB;
    //    }

    //    override public void SetGroupPos(int xIndex) 
    //    {
    //        background.Xpos += background.Width * xIndex;
    //        if (image != null)
    //            image.Xpos = background.Xpos;
    //    }
    //    public override void GoalY(float y, bool set)
    //    {
    //        base.GoalY(y, set);
    //        if (image != null)
    //            image.Ypos = background.Ypos;
    //    }
    //    public override bool Visible
    //    {
    //        set
    //        {
    //            base.Visible = value;
    //            if (image != null)
    //                image.Visible = value;
    //        }
    //    }
    //    public override void DeleteMe()
    //    {
    //        base.DeleteMe();
    //        if (image != null)
    //            image.DeleteMe();
    //    }
    //    override public bool WidthStackable { get { return true; } }
        
    //    public override HUD.ClickFunction Function
    //    {
    //        get { return 
    //            Link.HasLink?
    //            HUD.ClickFunction.Link :
    //            HUD.ClickFunction.NotSelectable; }
    //    }

    //    public override string ToString()
    //    {
    //        return "Gadget button {" + description + "}";
    //    }
    //}
    
}
