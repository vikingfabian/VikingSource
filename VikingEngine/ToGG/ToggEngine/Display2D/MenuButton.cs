using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class MenuButton : HeroQuest.Display.Button
    {
        public MenuButton(VectorRect area)
            : base(area, HudLib.ContentLayer, HeroQuest.Display.ButtonTextureStyle.Standard)
        {
            var icon = addCoverImage(SpriteName.pjMenuIcon, 0.8f);
            icon.Layer = layer - 1;
            addInputIcon(Dir4.W, toggRef.inputmap.menuInput.OpenCloseKeyBoard);
        }

        protected override void createToolTip()
        {
            base.createToolTip();
            HudLib.AddTooltipText(tooltip, null, "Main Menu",
                Dir4.S, this.area, null);
        }
    }

    class LineOfSightUi : HUD.ToggleImageButtton
    {
        public LineOfSightUi(VectorRect area)
            : base(SpriteName.lineofsightEyeButton, SpriteName.lineofsightEyeButtonHighlighted,
                  HudLib.ContentLayer, area)
        {
            inputIcon = addInputIcon(Dir4.W, toggRef.inputmap.lineOfSight, null);
        }

        protected override void createToolTip()
        {
            base.createToolTip();
            HudLib.AddTooltipText(tooltip, "LOS tool", "View all squares that are in \"Line Of Sight\" from a position",
                Dir4.S, this.area, null);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            inputIcon.DeleteMe();
        }
    }

    abstract class AbsIconOutlineButton : HUD.AbsButtonGui
    {
        Graphics.Image inputIcon;
        protected Graphics.Image image;
        Graphics.RectangleLines outline;

        public AbsIconOutlineButton(VectorRect area, Input.IButtonMap button)
            : base()
        {
            this.area = area;

            inputIcon = addInputIcon(Dir4.W, button, null);
            inputIcon.Layer = HudLib.ContentLayer + 1;
            outline = new Graphics.RectangleLines(area, 2, 1, HudLib.ContentLayer + 1);
            outline.Visible = false;
        }

        protected override void onMouseEnter(bool enter)
        {
            base.onMouseEnter(enter);
            outline.Visible = enter;
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                base.Visible = value;
                inputIcon.Visible = value;
                image.Visible = value;

                if (!value)
                {
                    outline.Visible = false;
                }
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            inputIcon.DeleteMe();
            image.DeleteMe();
            outline.DeleteMe();
        }
    }

    //class SpyglassUi : AbsIconOutlineButton
    //{
    //    public SpyglassUi(VectorRect area)
    //        : base(area, toggRef.inputmap.moreInfo)
    //    {
    //        image = new Graphics.Image(SpriteName.cmdSpyglass, area.Position, area.Size, HudLib.ContentLayer);
    //    }

    //    public void setToggle(bool spyglassOn)
    //    {
    //        image.SetSpriteName(spyglassOn ? SpriteName.cmdSpyglassHighlighted : SpriteName.cmdSpyglass);
    //    }

    //    protected override void createToolTip()
    //    {
    //        base.createToolTip();
    //        HudLib.AddTooltipText(tooltip, "Investigate", "Will display extended tool tips",
    //            Dir4.S, this.area, null);
    //    }
    //}

    class CommunicationsUi : AbsIconOutlineButton
    {        
        public CommunicationsUi(VectorRect area)
            : base(area, toggRef.inputmap.communications)
        {
            image = new Graphics.Image(SpriteName.speachBobbleWhite, area.CenterTop, 
                area.Width * 0.8f * new Vector2(1, 2), 
                HudLib.ContentLayer);
            image.OrigoAtCenterWidth();
        }

        protected override void createToolTip()
        {
            base.createToolTip();
            HudLib.AddTooltipText(tooltip, "Communications", "Messages and emotes display",
                Dir4.S, this.area, null);
        }
    }

    class BackpackButton : HeroQuest.Display.Button
    {
        public BackpackButton(VectorRect area)
            : base(area, HudLib.ContentLayer, HeroQuest.Display.ButtonTextureStyle.Standard)
        {
            var icon = addCoverImage(SpriteName.cmdBackpack, 0.9f);
            icon.Layer = layer - 1;
        }

        protected override void createToolTip()
        {
            base.createToolTip();
            HudLib.AddTooltipText(tooltip, null, "Backpack",
                Dir4.N, this.area, null);
        }
    }
}
