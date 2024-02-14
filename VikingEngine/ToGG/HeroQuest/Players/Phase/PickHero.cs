using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG.HeroQuest.Players.Phase
{
    class PickHero : AbsPlayerPhase
    {
        Graphics.ImageGroup titleImages = new Graphics.ImageGroup();
        HeroButton[] cards;
        public VectorRect cardsArea;
        PulsatingHud pulsating;

        public PickHero(LocalPlayer player)
            : base(player)
        { }

        public override void onBegin()
        {
            base.onBegin();

            string titleText = "Pick a Hero";

            Vector2 cardSize = new Vector2(Engine.Screen.IconSize * 1.5f);

            float bgWidth = cardSize.X * 2f;
            Graphics.Text2 title = new Graphics.Text2(titleText, LoadedFont.Bold, Vector2.Zero, Engine.Screen.TextTitleHeight,
                HudLib.TitleTextBronze, HudLib.ContentLayer, bgWidth);
            //float spacing = Height * 0.4f;
            Vector2 titleSz = title.MeasureText();

            float totalHeight = titleSz.Y + Engine.Screen.BorderWidth + 
                Table.TotalWidth(player.heroInstances.Count, cardSize.Y, Engine.Screen.BorderWidth);

            Vector2 topLeft = new Vector2(Engine.Screen.SafeArea.Right - bgWidth, Engine.Screen.SafeArea.Bottom - totalHeight);
            float centerX = topLeft.X + bgWidth * 0.5f;

           // topLeft.Y -= spacing * 2f + Engine.Screen.TextTitleHeight * 2f;

            title.position = new Vector2(centerX, topLeft.Y);           
            title.OrigoAtCenterWidth();
            titleImages.Add(title);

            Vector2 cardPos = new Vector2(centerX, title.position.Y + titleSz.Y + Engine.Screen.BorderWidth);
            cardPos.X -= cardSize.X * 0.5f;

            cards = new HeroButton[player.heroInstances.Count];
            for (int i = 0; i < player.heroInstances.Count; ++i)
            {
                HeroButton button = new HeroButton(cardPos, cardSize, player, player.heroInstances[i]);//StrategyCardsHudMember m = new StrategyCardsHudMember(cardPos, cardSize, deckcards[i], userTag);
                cards[i] = button;

                cardPos.Y += cardSize.Y + Engine.Screen.BorderWidth;
            }

            //float resourseSpace = cardSize.Y * 0.4f;
            Vector2 bottomRight = Engine.Screen.SafeArea.RightBottom;

            //selection = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, HudLib.BgLayer);
            //hover = new Graphics.Image(SpriteName.WhiteArea, Vector2.Zero, Vector2.One, HudLib.ContentLayer);
            //hover.PaintLayer -= PublicConstants.LayerMinDiff;
            //hover.Visible = false;
            //hoverFlashTime.SetZeroTimeLeft();

            cardsArea = VectorRect.FromTwoPoints(topLeft, bottomRight);
            cardsArea.AddRadius(HudLib.ThickBorderEdgeSize + Engine.Screen.BorderWidth);

            var bg = HudLib.ThickBorder(cardsArea, HudLib.BgLayer + 1);
            titleImages.images.AddRange(bg.images);

            pulsating = new PulsatingHud(cardsArea, true);
        }

        public override void update(ref PlayerDisplay display)
        {
            player.setCurrentPlayerState(ref display, PlayerState.PickHero);

            if (cards != null)
            {
                for (int i = 0; i < cards.Length; ++i)
                {
                    if (cards[i].update())
                    {
                        player.heroInstances.SelectIndex(i);
                        end();
                        player.onPickHero();
                        
                        return;
                    }
                }
            }
            display.mouseOverHud |= cardsArea.IntersectPoint(Input.Mouse.Position);

            player.mapUpdate(ref display, false);
            player.updateBoardRoam(false);//.mapUpdate(ref display, false);
            //player.hud.update(ref display, player, false);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            titleImages.DeleteAll();
            pulsating.DeleteMe();

            for (int i = 0; i < cards.Length; ++i)
            {
                cards[i].DeleteMe();
            }
        }

        public override PhaseType Type => PhaseType.PickHero;


        class HeroButton : HeroQuest.Display.Button
        {
            public HeroInstance hero;
            LocalPlayer player;

            public HeroButton(Vector2 pos, Vector2 sz, LocalPlayer player, HeroInstance hero)
                : base(new VectorRect(pos, sz), HudLib.ContentLayer, HeroQuest.Display.ButtonTextureStyle.Standard)
            {
                this.player = player;
                this.hero = hero;

                useMouseOverOnDisabled = true;

                Graphics.ImageAdvanced unitImage = new Graphics.ImageAdvanced(SpriteName.NO_IMAGE, area.Center,
                    area.Size * 0.9f, HudLib.ContentLayer - 1, true);
                hero.heroUnit.Data.modelSettings.IconSource(unitImage, false);
                imagegroup.Add(unitImage);

                if (hero.activated)
                {
                    Enabled = false;
                }
            }

            public override bool update()
            {
                bool result = base.update();
                if (mouseOver)
                {
                    toggRef.cam.spectate(hero.heroUnit.squarePos);
                }

                return result;
            }

            //public override bool update_EvenIfDisabled()
            //{
            //    return true;
            //}

            protected override void createToolTip()
            {
                base.createToolTip();

                HUD.RichBox.RichBoxContent members = new HUD.RichBox.RichBoxContent();
                members.h2(hero.heroUnit.data.Name);
                addTooltipText(members, Dir4.W);
            }
        }
    }
}
