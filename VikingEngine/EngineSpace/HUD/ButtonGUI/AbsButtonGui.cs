using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.HUD
{
    struct ButtonGuiSettings
    {
        public static readonly ButtonGuiSettings Empty = new ButtonGuiSettings();

        public Color bgColor;
        public float highlightThickness;
        public Color highlightColor;
        public Color highlightDisabledColor;


        public ButtonGuiSettings(Color bgColor, float highlightThickness,
            Color highlightColor, Color highlightDisabledColor)
        {
            this.bgColor = bgColor;
            this.highlightThickness = highlightThickness;

            this.highlightColor = highlightColor;
            this.highlightDisabledColor = highlightDisabledColor;
        }
    }

    abstract class AbsButtonGui : IDeleteable
    {
        protected ButtonGuiSettings sett;
        public Graphics.Image baseImage, highlight;
        public Graphics.ImageGroup tooltip = null;
        public VectorRect area;

        public Input.IButtonMap buttonMap;
        public Action clickAction;
        public Action<Graphics.ImageGroup> createTooltipAction;
        protected bool enabled = true, visible = true;
        protected bool useMouseOverOnDisabled = false;
        public bool mouseOver = false, prevMouseOver = false;
        public Input.InputSourceType clickSource;
        public bool toggleHighLight = false;
        
        public AbsButtonGui()
        { }

        public AbsButtonGui(ButtonGuiSettings sett)
        {
            this.sett = sett;
        }

        protected void createHighlight()
        {
            highlight = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, ImageLayers.AbsoluteBottomLayer);
            highlight.LayerBelow(baseImage);
            highlight.Visible = false;
            
            refreshSelection();
        }

        virtual public bool update()
        {
            prevMouseOver = mouseOver;

            if (visible && (enabled || useMouseOverOnDisabled))
            {
                return intersectUpdate();
            }
            return false;
        }

        bool intersectUpdate()
        {
            if (area.IntersectPoint(Input.Mouse.Position) &&
                    detailedIntersectionCheck(Input.Mouse.Position))
            {
                if (!mouseOver)
                {
                    onMouseEnter(true);
                }

                if (enabled && Input.Mouse.ButtonDownEvent(MouseButton.Left))
                {
                    clickSource = Input.InputSourceType.Mouse;
                    return clickReturn();
                }
            }
            else
            {
                if (mouseOver)
                {
                    onMouseEnter(false);
                }
            }

            if (enabled && buttonMap != null && buttonMap.DownEvent)
            {
                clickSource = Input.InputSourceType.Keyboard;
                return clickReturn();
            }
            return false;
        }

        public void emptyUpdate()
        {
            if (mouseOver)
            {
                onMouseEnter(false);
            }
        }

        virtual public bool update_EvenIfDisabled()
        {
            prevMouseOver = mouseOver;

            if (visible)
            {
                return intersectUpdate();
            }
            return false;
        }

        //public void disableUpdate()
        //{
        //    if (mouseOver)
        //    {
        //        onMouseEnter(false);
        //    }
        //}

        virtual protected bool detailedIntersectionCheck(Vector2 point)
        {
            return true;
        }

        virtual protected void onMouseEnter(bool enter)
        {
            mouseOver = enter;

            if (enter)
            {
                if (tooltip == null)
                {
                    tooltip = new Graphics.ImageGroup();
                    createToolTip();
                }
            }
            else
            {
                removeTooltip();
            }

            if (highlight != null)
            {
                highlight.Visible = enter;
                if (toggleHighLight)
                {
                    baseImage.Visible = !highlight.Visible;
                }
            }

        }

        public bool HasMouseEnterLeaveEvent => prevMouseOver != mouseOver;

        void removeTooltip()
        {
            if (tooltip != null)
            {
                tooltip.DeleteAll();
                tooltip = null;
            }
        }

        protected bool clickReturn()
        {
            onClick();
            return true;
        }

        virtual protected void onClick()
        {
            clickAction?.Invoke();
        }

        virtual protected void createToolTip()
        {
            createTooltipAction?.Invoke(tooltip);
        }

        virtual public Graphics.Image addInputIcon(Dir4 side, Input.IButtonMap buttonMap, Graphics.ImageGroup imagegroup)
        {
            this.buttonMap = buttonMap;
            Vector2 sz = Engine.Screen.SmallIconSizeV2;
            Vector2 center = area.Center + IntVector2.FromDir4(side).Vec * (sz * new Vector2(0.6f) + area.Size * new Vector2(0.5f));

            var inputIcon = new Graphics.Image(buttonMap.Icon, center, sz, ImageLayers.AbsoluteBottomLayer, true);
            if (baseImage != null)
            {
                inputIcon.PaintLayer = baseImage.PaintLayer;
            }
            imagegroup?.Add(inputIcon);

            return inputIcon;
        }

        public void refresh()
        {
            area = baseImage.RealArea();
            refreshSelection();
        }

        public void refreshSelection()
        {
            refreshSelectionArea(sett.highlightThickness);

            highlight.Color = enabled ? sett.highlightColor : sett.highlightDisabledColor;
        }

        protected void refreshSelectionArea(float thickness)
        {
            VectorRect hlArea = area;
            hlArea.AddRadius(thickness);

            highlight.Position = hlArea.Position;
            highlight.Size = hlArea.Size;
        }

        virtual public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
                if (baseImage != null)
                {
                    baseImage.Visible = value;
                }

                if (!value)
                {
                    if (highlight != null) highlight.Visible = false;
                    removeTooltip();
                }
            }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled != value)
                {
                    enabled = value;
                    onEnableChange();
                }
            }
        }

        public void SetEnableType(EnableType enable)
        {
            if (enable == EnableType.Hidden)
            {
                Visible = false;
            }
            else
            {
                Visible = true;
                Enabled = enable == EnableType.Enabled;
            }
        }

        virtual protected void onEnableChange()
        {
            if (!enabled)
            {
                highlight.Visible = false;
                if (mouseOver)
                {
                    onMouseEnter(false);
                }
            }
        }

        virtual public void Move(Vector2 move)
        {
            baseImage.position += move;
            highlight.position += move;
            area.Position += move;
        }

        virtual public void DeleteMe()
        {
            baseImage.DeleteMe();
            if (highlight != null) highlight.DeleteMe();
            if (tooltip != null) tooltip.DeleteAll();
        }

        public bool IsDeleted
        {
            get { return baseImage.IsDeleted; }
        }
    }
}
