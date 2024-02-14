using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander.CommandCard;

namespace VikingEngine.ToGG.ToggEngine.Display2D
{
    class StrategyCardsHud
    {
        int hoverIx = byte.MaxValue;

        StrategyCardsHudMember[] cards;
        //Graphics.Image selection, hover;
        //Graphics.ImageGroup tooltip = new Graphics.ImageGroup();
        Timer.Basic hoverFlashTime = new Timer.Basic(120, false);
        Action<int> onSelect;
        float totalHeight;
        public VectorRect cardsArea;
        Graphics.ImageGroup titleImages = new Graphics.ImageGroup();

        Graphics.VisualFlash alertFlash = null;
        Graphics.Image alertFlashOutline = null;
        PulsatingHud pulsating;
        List<AbsStrategyCard> deckcards;
        object userTag;

        public StrategyCardsHud(AbsStrategyCardDeck deck, Action<int> onSelect, object userTag)
        {
            this.onSelect = onSelect;
            this.userTag = userTag;
            deckcards = deck.Cards();

            setVisible(true);
        }

        public static void Scale(out float Height, out Vector2 cardSize, out float spacing, out float bgWidth)
        {
            Height = Engine.Screen.SmallIconSize * 1.4f;

            cardSize = StrategyCardsHudMember.CardSize(Height);
            spacing = Height * 0.4f;

           
            bgWidth = cardSize.X * 1.6f;
        }

        public void setVisible(bool visible)
        {
            if (visible)
            {
                if (cards == null)
                {
                    float Height; Vector2 cardSize; float spacing; float bgWidth;
                    Scale(out Height, out cardSize, out spacing, out bgWidth);

                    totalHeight = Table.TotalWidth(deckcards.Count, cardSize.Y, spacing);
                    Vector2 topLeft = new Vector2(Engine.Screen.SafeArea.Right - bgWidth, Engine.Screen.SafeArea.Bottom - totalHeight);
                    float centerX = topLeft.X + bgWidth * 0.5f;

                    topLeft.Y -= spacing * 2f + Engine.Screen.TextTitleHeight * 2f;

                    string titleText = "Select Strategy";

                    Vector2 textPos = new Vector2(centerX, topLeft.Y);
                    Graphics.Text2 title = new Graphics.Text2(titleText, LoadedFont.Bold, textPos, Engine.Screen.TextTitleHeight,
                        HudLib.TitleTextBronze, HudLib.ContentLayer, bgWidth);
                    title.OrigoAtCenterWidth();
                    titleImages.Add(title);

                    Vector2 cardPos = new Vector2(centerX, title.MeasureBottomPos() + spacing);
                    cardPos.X -= cardSize.X * 0.5f;

                    cards = new StrategyCardsHudMember[deckcards.Count];
                    for (int i = 0; i < cards.Length; ++i)
                    {
                        StrategyCardsHudMember m = new StrategyCardsHudMember(cardPos, cardSize, deckcards[i], userTag);
                        cards[i] = m;

                        cardPos.Y += cardSize.Y + spacing;
                    }

                    //float resourseSpace = cardSize.Y * 0.4f;
                    Vector2 bottomRight = Engine.Screen.SafeArea.RightBottom;

                    //selection = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, HudLib.BgLayer);
                    //hover = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, HudLib.ContentLayer);
                    //hover.PaintLayer -= PublicConstants.LayerMinDiff;
                    //hover.Visible = false;
                    hoverFlashTime.SetZeroTimeLeft();

                    cardsArea = VectorRect.FromTwoPoints(topLeft, bottomRight);
                    cardsArea.AddRadius(HudLib.ThickBorderEdgeSize + Engine.Screen.BorderWidth);

                    var bg = HudLib.ThickBorder(cardsArea, HudLib.BgLayer + 1);
                    titleImages.images.AddRange(bg.images);

                    pulsating = new PulsatingHud(cardsArea, true);
                }
            }
            else
            {
                if (cards != null)
                {
                    DeleteMe();

                    cards = null;
                }
            }
        }

        /// <returns>mouse over</returns>
        public bool update()
        {
            if (cards != null)
            {
                hoverIx = byte.MaxValue;

                for (int i = 0; i < cards.Length; ++i)
                {
                    if (cards[i].update())
                    {
                        onSelect(cards[i].data.Id);
                    }

                    if (cards[i].mouseOver)
                    {
                        hoverIx = i;
                    }
                }
            }
            return cardsArea.IntersectPoint(Input.Mouse.Position);
        }        

        public StrategyCardsHudMember HoverCard()
        {
            if (arraylib.InBound(cards, hoverIx))
            {
                return cards[hoverIx];
            }
            return null;
        }

        public StrategyCardsHudMember Get(int type)
        {
            for (int i = 0; i < cards.Length; ++i)
            {
                if (cards[i].data.Id == type)
                {
                    return cards[i];
                }
            }
            return null;
        }

        public void createAlertFlash()
        {
            if (alertFlash == null)
            {
                var ar = cardsArea;
                ar.AddRadius(Engine.Screen.BorderWidth);
                alertFlashOutline = new Graphics.Image(SpriteName.WhiteArea, ar.Position, ar.Size, HudLib.BgLayer + 3);
                alertFlashOutline.Opacity = 0.6f;
            }
            else
            {
                alertFlash.DeleteMe();
                alertFlash = null;
            }

            alertFlashOutline.Visible = true;
            alertFlash = new Graphics.VisualFlash(alertFlashOutline, 3, 120);
        }

        public void DeleteMe()
        {
            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i].DeleteMe();
            }

            //tooltip.DeleteAll();
            titleImages.DeleteAll();

            //selection.DeleteMe();
            //hover.DeleteMe();

            alertFlashOutline?.DeleteMe();
            alertFlash = null;

            pulsating.DeleteMe();
        }
    }

    class StrategyCardsHudMember : HeroQuest.Display.Button
    {
        public AbsStrategyCard data;
        List<StrategyCardResource> resources;

        public StrategyCardsHudMember(Vector2 pos, Vector2 sz, AbsStrategyCard card, object userTag)
            :base(new VectorRect(pos, sz), HudLib.ContentLayer, HeroQuest.Display.ButtonTextureStyle.None)
        {
            useMouseOverOnDisabled = true;
            this.data = card;
            baseImage.SetSpriteName(SpriteName.cmdStrategyBg);

            resources = card.Resources(userTag, out enabled);
            imagegroup.Add(CardIcon(baseImage, card.CardSprite, enabled));
            refreshSelection();

            if (arraylib.HasMembers(resources))
            {
                Vector2 rsz = new Vector2(sz.Y * 0.45f);
                Vector2 rpos = area.LeftBottom;
                rpos.Y -= sz.Y * 0.2f;//2f;

                foreach (var m in resources)
                {
                    for (int i = 0; i < m.count.maxValue; ++i)
                    {
                        Graphics.Image resourceIcon = new Graphics.Image(i < m.count.Value ? m.sprite : m.spriteDisabled,
                            rpos, rsz, ImageLayers.AbsoluteTopLayer);
                        resourceIcon.LayerAbove(baseImage);
                        imagegroup.Add(resourceIcon);

                        rpos.X += rsz.X * 0.9f;
                    }

                    rpos.X += rsz.X * 0.5f;
                }
            }

            //Enabled = (CommandType)card.Id
        }

        protected override void createToolTip()
        {
            //base.createToolTip();
            var members =  data.menuToolTip(resources);

            addTooltipText(members, Dir4.W);
        }

        public static Vector2 CardSize(float height)
        {
            Vector2 cardSize = VectorExt.V2FromY(height);
            cardSize.X = cardSize.Y / SpriteSheet.CmdStrategyCardPixSz.Y * SpriteSheet.CmdStrategyCardPixSz.X;
            return cardSize;
        }

        public static Graphics.Image CardIcon(Graphics.Image bg, SpriteName sprite, bool enabled)
        {
            VectorRect area = VectorRect.FromCenterSize(Vector2.Zero, bg.Size);

            area.Height *= 0.75f;
            area.Width = area.Height * 2f;
            area.Center = bg.Center;

            Graphics.Image icon = new Graphics.Image(sprite, area.Position, area.Size, ImageLayers.AbsoluteBottomLayer);
            icon.LayerAbove(bg);
            icon.Color = enabled? new Color(245, 217, 134) : new Color(41,28,22);

            bg.Color = enabled ? Color.White : Color.DarkGray;

            return icon;
        }

        protected override void onEnableChange()
        {
            base.onEnableChange();
        }
    }
}
