using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.MoonFall.Players;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    

    abstract class AbsPhaseButton : HeroQuest.Display.Button
    {
        public string tooltip = null, toolwarning = null;
        Graphics.ImageGroup endWarningTooltip = new Graphics.ImageGroup(8);
        Graphics.ImageGroup pressNextKeyTip = new Graphics.ImageGroup(3);

        protected Graphics.Image icon;
        bool enableToolTip = true;
        protected string tooltipText;
        public VectorRect? tooltipFromArea = null;

        Graphics.Image pulsatingEdge = null;

        public AbsPhaseButton(VectorRect area)//, bool next)
           : base(area, HudLib.ContentLayer, HeroQuest.Display.ButtonTextureStyle.Standard)
        {
            icon = addCoverImage(SpriteName.pjForwardIcon, 0.9f);
            icon.Layer = layer - 1;

            //if (next)
            //{
            //    tooltipText = "End turn";
            //}
            //else
            //{
            //    icon.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            //    tooltipText = "Back to Strategy Selection";
            //}
        }

        public override bool update()
        {
            if (pulsatingEdge != null)
            {
               float perc = MathExt.Sinf(Ref.TotalTimeSec * 4f);
                pulsatingEdge.Opacity = 0.3f + perc * 0.2f;
            }
            return base.update();
        }

        public void viewPulsatingEdge(bool view)
        {
            if (view)
            {
                if (pulsatingEdge == null)
                {
                    VectorRect area = this.area;
                    area.AddRadius(Engine.Screen.BorderWidth * 1f);
                    pulsatingEdge = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
                    pulsatingEdge.LayerBelow(this.highlight);
                    pulsatingEdge.Opacity = 0;
                }
            }
            else
            {
                pulsatingEdge?.DeleteMe();
                pulsatingEdge = null;
            }
        }

        

        protected override void createToolTip()
        {
            base.createToolTip();
            this.addTooltipText(null, tooltipText, Dir4.N, tooltipFromArea);
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;

                if (!value)
                {
                    endWarningTooltip.DeleteAll();
                    pressNextKeyTip.DeleteAll();
                    viewPulsatingEdge(false);
                }
            }
        }

        public void EnableToolTip(bool enableToolTip)
        {
            this.enableToolTip = enableToolTip;
            if (!enableToolTip)
            {
                endWarningTooltip.DeleteAll();
            }
        }

        public void viewPressNextKeyTip(bool view)
        {
            if (view)
            {
                if (pressNextKeyTip.IsEmpty)
                {
                    Graphics.TextG pressText = new Graphics.TextG(LoadedFont.Regular,
                        Vector2.Zero, Engine.Screen.TextSizeV2, Graphics.Align.Zero, "Press ", Color.Black, HudLib.PopupLayer);

                    Vector2 bottomRight = new Vector2(area.Right, area.Y - Engine.Screen.IconSize);

                    VectorRect bgarea = VectorRect.Zero;
                    bgarea.Size = pressText.MeasureText();
                    Vector2 inputIconSz = new Vector2(bgarea.Height);
                    bgarea.AddToLeftSide(inputIconSz.X);

                    bgarea.Position = bottomRight - bgarea.Size;

                    Graphics.Image inputIcon = new Graphics.Image(toggRef.inputmap.nextPhase.Icon,//cmdLib.Button_NextPhase.Icon,
                        bgarea.RightCenter, inputIconSz, HudLib.PopupLayer);
                    inputIcon.origo = new Vector2(1f, 0.5f);

                    pressText.Position = bgarea.Position;

                    Graphics.Image bobbleArrow = new Graphics.Image(SpriteName.LfNpcSpeechArrow, new Vector2(area.Center.X, bgarea.Bottom), Engine.Screen.SmallIconSizeV2, ImageLayers.AbsoluteBottomLayer);
                    bobbleArrow.origo = new Vector2(1f, 0.5f);
                    bobbleArrow.Rotation = -MathHelper.PiOver2;

                    bgarea.AddRadius(Engine.Screen.BorderWidth);

                    Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, bgarea.Position, bgarea.Size, ImageLayers.AbsoluteBottomLayer);
                    bg.LayerBelow(pressText);
                    bobbleArrow.LayerBelow(bg);


                    pressNextKeyTip.Add(pressText);
                    pressNextKeyTip.Add(inputIcon);
                    pressNextKeyTip.Add(bg);
                    pressNextKeyTip.Add(bobbleArrow);
                }
            }
            else
            {
                pressNextKeyTip.DeleteAll();
            }
        }
             

        public override void DeleteMe()
        {
            base.DeleteMe();
            pressNextKeyTip.DeleteAll();
            endWarningTooltip.DeleteAll();
        }
    }
}
