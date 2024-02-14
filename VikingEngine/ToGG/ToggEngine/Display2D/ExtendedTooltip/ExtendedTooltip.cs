using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class ExtendedTooltip
    {
        bool visible = false;
        List2<AbsExtToolTip> toolTips = new List2<AbsExtToolTip>(16);
        List<ToolTipColumn> columns = new List<ToolTipColumn>(2);
        VectorRect screenArea;
        Timer.Basic hooverTime = new Timer.Basic(1000, false);

        public void update(PlayerState state)
        {
            bool view = state == PlayerState.BoardRoam ||
                state == PlayerState.SelectStrategy ||
                state == PlayerState.PickHero;

            update(view);
        }

        public void update(bool viewToolTips)
        {
            if (viewToolTips)
            {
                if (!visible && hooverTime.Update())
                {
                    viewToolTip();
                }
            }
            else
            {
                removeTooltip();
            }
        }

        public void onNewTile()
        {
            toolTips.Clear();
            hooverTime.Reset();
            removeTooltip();
        }

        public void onMapPan()
        {
            hooverTime.Reset();
            removeTooltip();
        }

        public void add(params AbsExtToolTip[] toolTip)
        {
            foreach (var m in toolTip)
            {
                add(m, 0);
            }
        }

        public void add(AbsExtToolTip toolTip, int keyWordLevel = 0)
        {
            foreach (var m in toolTips)
            {
                if (m.EqualType(toolTip))
                {
                    return;
                }
            }

            toolTips.Add(toolTip);
            if (visible)
            {
                generateTooltipImage(toolTip);
            }

            if (keyWordLevel < 2)
            {
                var keyWords = toolTip.DescriptionKeyWords();
                if (keyWords != null)
                {
                    foreach (var m in keyWords)
                    {
                        add(m, keyWordLevel + 1);
                    }
                }
            }
        }

        void viewToolTip()
        {
            visible = true;

            screenArea = Engine.Screen.SafeArea;
            screenArea.SetBottom(toggRef.hud.infoCardTop() - Engine.Screen.SmallIconSize, true);

            foreach (var m in toolTips)
            {
                generateTooltipImage(m);
            }
            finalizePlacement();
        }

        void finalizePlacement()
        {
            if (columns.Count > 0)
            {
                float x = screenArea.X;

                foreach (var m in columns)
                {
                    m.finalizePlacement(screenArea, x);
                    x += m.nextArea.Width + Spacing;
                }
            }
        }

        void generateTooltipImage(AbsExtToolTip toolTip)
        {
            ToolTipColumn column;

            if (columns.Count == 0)
            {
                columns.Add(new ToolTipColumn());
            }

            column = arraylib.Last(columns);

            Graphics.ImageGroup tipImages = new Graphics.ImageGroup(8);
            Vector2 pos = new Vector2(Engine.Screen.BorderWidth);
            var iconSprite = toolTip.Icon;
            if (iconSprite != SpriteName.NO_IMAGE)
            {
                
                Rectangle rect = DataLib.SpriteCollection.Get(iconSprite).Source;
                Vector2 boxsize = new Vector2(Engine.Screen.SmallIconSize / rect.Height * rect.Width, Engine.Screen.SmallIconSize);

                Graphics.Image icon = new Graphics.Image(toolTip.Icon,
                    pos, boxsize, HudLib.TooltipLayer);
                tipImages.Add(icon);

                pos.X = icon.Right;
                pos.Y += Engine.Screen.SmallIconSize * 0.5f;
            }
            else
            {
                pos.Y += Engine.Screen.TextTitleHeight * 0.5f;
            }
            
            Graphics.Text2 title = new Graphics.Text2(toolTip.Title, LoadedFont.Bold,
                pos, Engine.Screen.TextTitleHeight, HudLib.TitleTextBronze, HudLib.TooltipLayer);
            title.OrigoAtCenterHeight();
            tipImages.Add(title);

            pos.X = Engine.Screen.BorderWidth;
            pos.Y += Engine.Screen.SmallIconSize * 0.5f;
            Graphics.Text2 text = new Graphics.Text2(toolTip.Text, LoadedFont.Regular, 
                pos, Engine.Screen.TextBreadHeight, Color.White, HudLib.TooltipLayer,
                column.nextArea.Width - Engine.Screen.BorderWidth * 2f);
            tipImages.Add(text);

            float bottom = text.MeasureBottomPos() + Engine.Screen.BorderWidth;

            Vector2 sz = new Vector2(column.nextArea.Width, bottom);
            VectorRect area = new VectorRect(Vector2.Zero, sz);
            column.nextArea.Height = bottom;

            const float Opacity = 0.8f;
            const int OutlineSz = 2;

            Graphics.RectangleLines rectangle = new Graphics.RectangleLines(area, OutlineSz, 0, HudLib.TooltipLayer + 1);
            rectangle.setColor(Color.White, Opacity);
            tipImages.Add(rectangle.lines);

            area.AddRadius(-OutlineSz);
            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, area.Position, area.Size, HudLib.TooltipLayer + 1);
            bg.ColorAndAlpha(Color.Black, Opacity);
            tipImages.Add(bg);

            
            if (bottom > screenArea.Height)
            {
                column = new ToolTipColumn();
                columns.Add(column);
            }

            tipImages.Move(column.nextArea.Position);
            column.add(tipImages);

            column.nextArea.nextAreaY(1, Spacing);
        }

        void removeTooltip()
        {
            if (visible)
            {
                foreach (var m in columns)
                {
                    m.DeleteMe();
                }
                columns.Clear();

                visible = false;
            }
        }
                
        float Spacing => Engine.Screen.BorderWidth * 2f;

        class ToolTipColumn
        {
            Graphics.ImageGroup images = new Graphics.ImageGroup(128);
            public VectorRect nextArea;
            public bool isFilled = false;
            public float totalHeight = 0;

            public ToolTipColumn()
            {
                nextArea.Position = Vector2.Zero;
                nextArea.Width = Engine.Screen.Height * 0.3f;
            }

            public void add(Graphics.ImageGroup tipImages)
            {
                images.Add(tipImages);
                totalHeight = nextArea.Bottom;
            }

            public void finalizePlacement(VectorRect screenArea, float x)
            {
                VectorRect area = new VectorRect(
                    x,
                    screenArea.Bottom - totalHeight,
                    nextArea.Width,
                    totalHeight);

                images.Move(area.Position);
            }

            public void DeleteMe()
            {
                images.DeleteAll();
            }
        }
    }
}
