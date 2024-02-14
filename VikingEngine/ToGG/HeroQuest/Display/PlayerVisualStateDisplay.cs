using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.HeroQuest.Players;

namespace VikingEngine.ToGG.HeroQuest
{
    class PlayerVisualStateDisplay
    {
        public int AttackResults = 0;
        Graphics.ImageGroup images = new Graphics.ImageGroup();
        Vector2 right;
        Vector2 cardSize;
        public PlayerVisualData visualState;

        public PlayerVisualStateDisplay(PlayerVisualData visualState, VectorRect nameDisplayArea, ref bool hiddenStrategy)
        {
            this.visualState = visualState;

            Vector2 pos = nameDisplayArea.RightTop;
            pos.X += Engine.Screen.BorderWidth;
            cardSize = new Vector2(nameDisplayArea.Height * 0.8f);
            
            if (visualState.state == PlayerState.SelectStrategy)
            {
                hiddenStrategy = false;
            }
            else if (visualState.state != PlayerState.Other &&
                visualState.state != PlayerState.BoardRoam)
            {
                hiddenStrategy = true;

                SpriteName stateSprite = SpriteName.NO_IMAGE;
                switch (visualState.state)
                {
                    case PlayerState.Attack:
                        stateSprite = visualState.meleeAttack ? SpriteName.cmdStatsMelee : SpriteName.cmdStatsRanged;
                        break;
                    case PlayerState.Menu:
                        stateSprite = SpriteName.birdLobbyMenuButton;
                        break;
                    case PlayerState.MoveUnit:
                        stateSprite = SpriteName.cmdStatsMove;
                        break;
                    case PlayerState.Backpack:
                        stateSprite = SpriteName.cmdBackpack;
                        break;
                }

                Graphics.Image stateBg = new Graphics.Image(SpriteName.WhiteArea, pos, cardSize, HudLib.StatusHudLayer + 1);
                Graphics.Image stateImg = new Graphics.Image(stateSprite, stateBg.Center, stateBg.Size * 0.9f, HudLib.StatusHudLayer, true);

                images.Add(stateBg); images.Add(stateImg);
                right = stateBg.RightTop;
            }

            if (!hiddenStrategy)
            {
                cardSize = ToggEngine.Display2D.StrategyCardsHudMember.CardSize(cardSize.Y); 
                
                Graphics.Image strategyBg = new Graphics.Image(SpriteName.cmdStrategyBg,
                    pos, cardSize, HudLib.StatusHudLayer + 1);
                images.Add(strategyBg);

                Graphics.Image strategyIcon = null;
                if (visualState.strategy != HeroStrategyType.NONE)
                {
                    strategyIcon = ToggEngine.Display2D.StrategyCardsHudMember.CardIcon(strategyBg, 
                        HeroStrategy.AbsHeroStrategy.GetStrategy(visualState.strategy).cardSprite, visualState.strategyAvailable);
                    
                    images.Add(strategyIcon);
                }

                if (visualState.state == PlayerState.SelectStrategy)
                {
                    Graphics.TextG question = new Graphics.TextG(LoadedFont.Regular, strategyBg.Center,
                        Vector2.One, Graphics.Align.CenterAll, "...", Color.White, HudLib.StatusHudLayer);
                    question.SetHeight(strategyBg.Height * 1.2f);
                    images.Add(question);

                    if (strategyIcon != null)
                    {
                        strategyIcon.Opacity = 0.5f;
                    }
                }

                right = strategyBg.RightTop;
            }
        }

        public void viewAttackResult(BattleDiceResult result)
        {
            SpriteName icon = BattleDice.ResultIcon(result);

            right.X += Engine.Screen.BorderWidth;

            Graphics.Image bg = new Graphics.Image(SpriteName.WhiteArea, right, cardSize, HudLib.StatusHudLayer + 1);
            Graphics.Image img = new Graphics.Image(icon, bg.Center, bg.Size * 0.8f, HudLib.StatusHudLayer, true);

            images.Add(bg); images.Add(img);

            right.X = bg.Right;
        }

        public void DeleteMe()
        {
            images.DeleteAll();
        }
    }

    struct PlayerVisualData
    {
        public PlayerState state;
        public HeroStrategyType strategy;
        public bool strategyAvailable;
        public bool meleeAttack;

        public void write(LocalPlayer player, System.IO.BinaryWriter w)
        {
            w.Write((byte)state);
            w.Write((byte)strategy);
            w.Write(strategyAvailable);
            w.Write(meleeAttack);
        }

        public void read(System.IO.BinaryReader r)
        {
            state = (PlayerState)r.ReadByte();
            strategy = (HeroStrategyType)r.ReadByte();
            strategyAvailable = r.ReadBoolean();
            meleeAttack = r.ReadBoolean();
        }

        public override bool Equals(object obj)
        {
            PlayerVisualData other = (PlayerVisualData)obj;
            
            if (this.state != other.state)
            {
                return false;
            }

            if (state == PlayerState.SelectStrategy)
            {
                return this.strategy == other.strategy;
            }

            return true;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
