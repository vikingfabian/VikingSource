using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace VikingEngine.ToGG.HeroQuest
{
    class OptionsWindow<T>
    {
        Graphics.ImageGroup images = new Graphics.ImageGroup();

        Vector2 topLeft;
        Vector2 optionPos;
        float buttonW;
        float width;
        Graphics.RectangleLines highlightBorder;
        List2<OptionButton<T>> buttons = new List2<OptionButton<T>>();
        OptionButton<T> hoverOption = null;
        int selectedOption = 0;

        public Action<T> clickEvent, hoverEvent = null;
        Graphics.ImageGroup tooltip = new Graphics.ImageGroup();
        
        public OptionsWindow(string titleText, Vector2 topLeft, float width, Action<T> clickEvent)
        {
            this.clickEvent = clickEvent;
            this.topLeft = topLeft;
            this.width = width;
            buttonW = width - HudLib.ThickBorderEdgeSize * 2f;

            var title = new Graphics.TextG(LoadedFont.Bold,
                new Vector2(topLeft.X + width * 0.5f, topLeft.Y + Engine.Screen.IconSize * 0.5f),
                Engine.Screen.TextIconFitSize,
                Graphics.Align.CenterAll, titleText, HudLib.TitleTextBronze, HudLib.AttackWheelLayer);
            images.Add(title);

            optionPos = topLeft;
            optionPos.X += HudLib.ThickBorderEdgeSize;
            optionPos.Y += Engine.Screen.IconSize + HudLib.ThickBorderEdgeSize - Engine.Screen.BorderWidth;
        }

        public OptionButton<T> addOption(T opt, 
            List<HUD.RichBox.AbsRichBoxMember> label, 
            List<HUD.RichBox.AbsRichBoxMember> tooltip)
        {
            OptionButton<T> option = new OptionButton<T>(opt, label, tooltip, ref optionPos, buttonW);
            buttons.Add(option);

            return option;
        }

        public Vector2 completeWindow(T startSelection)
        {
            {//Vertical Line to connect options
                Graphics.Line line = new Graphics.Line(buttons[0].optionsDot.Width * 3f / 17f, ImageLayers.AbsoluteBottomLayer,
                    new Color(165, 144, 82), buttons[0].optionsDot.RealCenter, arraylib.Last(buttons).optionsDot.RealCenter, true);
                line.LayerBelow(buttons[0].optionsDot);
                images.Add(line);
            }

            highlightBorder = new Graphics.RectangleLines(VectorRect.ZeroOne, HudLib.SelectionOutlineThickness, 0.5f, HudLib.AttackWheelLayer - 1);
            highlightBorder.Visible = false;

            images.images.AddRange(highlightBorder.lines);

            var bottomLeft = new Vector2(topLeft.X, optionPos.Y + HudLib.SelectionOutlineThickness);

            VectorRect convertarea = VectorRect.FromTwoPoints(topLeft, new Vector2(bottomLeft.X + width, bottomLeft.Y));
            var bg = HudLib.ThickBorder(convertarea, HudLib.AttackWheelLayer + 1);
            images.Add(bg);
            
            setOptionFromValue(startSelection);

            return bottomLeft;
        }

        public void update()
        {
            var prevHover = hoverOption;
            hoverOption = null;
            
            buttons.loopBegin();
            while(buttons.loopNext())
            {
                if (buttons.sel.area.IntersectPoint(Input.Mouse.Position))
                {
                    hoverOption = buttons.sel;

                    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
                    {
                        setOption(buttons.selIndex);
                    }
                    break;                    
                }
            }

            if (prevHover != hoverOption)
            {
                if (hoverOption == null)
                {
                    highlightBorder.Visible = false;
                    tooltip.DeleteAll();
                }
                else
                {
                    var area = buttons.sel.area;

                    highlightBorder.SetRectangle(area);
                    highlightBorder.Visible = true;
                    
                    tooltip.DeleteAll();

                    if (hoverEvent != null)
                    {
                        hoverEvent.Invoke(hoverOption.option);
                    }
                    else if (hoverOption.tooltip != null)
                    {
                        HudLib.AddTooltipText(tooltip, hoverOption.tooltip, Dir4.S, hoverOption.area, hoverOption.area);
                    }
                }
            }
        }

        public void AddHoverToolTip(List<HUD.RichBox.AbsRichBoxMember> richbox)
        {
            HudLib.AddTooltipText(tooltip, richbox, Dir4.S, hoverOption.area, hoverOption.area);
        }

        public void DeleteMe()
        {
            images.DeleteAll();
            buttons.loopBegin();
            while (buttons.loopNext())
            {
                buttons.sel.images.DeleteAll();
            }
        }

        void setOptionFromValue(T value)
        {
            if (value == null)
            {
                setOption(0);
            }
            else
            {
                selectedOption = -1;

                for (int i = 0; i < buttons.Count; ++i)
                {
                    bool selected = buttons[i].option.Equals(value);
                    buttons[i].set(selected);
                    if (selected)
                    {
                        selectedOption = i;
                    }
                }

                if (selectedOption < 0)
                {
                    setOption(0);
                }
            }
        }

        public void setOption(int index)
        {
            selectedOption = index;
            for (int i = 0; i < buttons.Count; ++i)
            {
                buttons[i].set(i == index);
            }

            clickEvent(buttons[selectedOption].option);
        }
    }

    class OptionButton<T>
    {
        public VectorRect area;
        public Graphics.ImageGroup images = new Graphics.ImageGroup();
        public T option;
        public Graphics.Image optionsDot;
        public List<HUD.RichBox.AbsRichBoxMember> tooltip;

        public OptionButton(T opt, 
            List<HUD.RichBox.AbsRichBoxMember> label,
            List<HUD.RichBox.AbsRichBoxMember> tooltip,
            ref Vector2 pos, float width)
        {
            this.tooltip = tooltip;
            this.option = opt;

            area = new VectorRect(pos.X, pos.Y, width, Engine.Screen.IconSize);

            optionsDot = new Graphics.Image(SpriteName.cmdHudOptionsOn, VectorExt.AddX(area.LeftCenter, Engine.Screen.BorderWidth),
                Engine.Screen.SmallIconSizeV2 * 0.6f, HudLib.AttackWheelLayer);
            optionsDot.origo = VectorExt.V2HalfY;
            images.Add(optionsDot);

            var sett = new HUD.RichBox.RichBoxSettings(
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.White, ColorExt.Empty),
                new TextFormat(LoadedFont.Regular, Engine.Screen.TextBreadHeight, Color.Gray, ColorExt.DarkGrayer),
                Engine.Screen.SmallIconSize, 1f);

            HUD.RichBox.RichBoxGroup richBox = new HUD.RichBox.RichBoxGroup(
                 VectorExt.AddY(optionsDot.RightTop, -sett.breadIconHeight * 0.5f), 
                 area.Right - optionsDot.Right,
                 HudLib.AttackWheelLayer, sett,
                 label);

            images.Add(richBox);

            pos.Y = area.Bottom;
        }

        public OptionButton(SpriteName toIcon, string text, T opt, 
            ref Vector2 pos, float width)
        {
            this.option = opt;

            area = new VectorRect(pos.X, pos.Y, width, Engine.Screen.IconSize);
            
            optionsDot = new Graphics.Image(SpriteName.cmdHudOptionsOn, VectorExt.AddX(area.LeftCenter, Engine.Screen.BorderWidth),
                Engine.Screen.SmallIconSizeV2 * 0.6f, HudLib.AttackWheelLayer);
            optionsDot.origo = VectorExt.V2HalfY;
            images.Add(optionsDot);

            Graphics.Image surgeIcon = new Graphics.Image(SpriteName.cmdIconSurge, optionsDot.RightTop, Engine.Screen.SmallIconSizeV2,
                HudLib.AttackWheelLayer);
            surgeIcon.origo = VectorExt.V2HalfY;
            images.Add(surgeIcon);


            Graphics.Image toArrow = new Graphics.Image(SpriteName.cmdConvertArrow, surgeIcon.RightTop, Engine.Screen.SmallIconSizeV2 * 0.6f,
               HudLib.AttackWheelLayer);
            toArrow.origo = VectorExt.V2HalfY;
            images.Add(toArrow);

            Graphics.Image toIconImg = new Graphics.Image(toIcon, toArrow.RightTop, Engine.Screen.SmallIconSizeV2,
                 HudLib.AttackWheelLayer);
            toIconImg.origo = VectorExt.V2HalfY;
            images.Add(toIconImg);

            Graphics.TextG textImg = new Graphics.TextG(LoadedFont.Regular, VectorExt.AddX(toIconImg.RightTop, Engine.Screen.SmallIconSize * 0.5f),
                Engine.Screen.TextSmallIconFitSize, Graphics.Align.CenterHeight, text, Color.White, HudLib.AttackWheelLayer);
            images.Add(textImg);

            pos.Y = area.Bottom;
        }

        public void set(bool selected)
        {
            optionsDot.SetSpriteName(selected ? SpriteName.cmdHudOptionsOn : SpriteName.cmdHudOptionsOff);
        }
    }
}
