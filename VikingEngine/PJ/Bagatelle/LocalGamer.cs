using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.PJ.Bagatelle
{
    class LocalGamer : AbsGamer
    {
        Display.SpriteText autoShootWarningText;
        const float AutoShootSeconds = 10;
        int prevAutoShootSeconds = -1;
        Time autoShootTime;
        public GamerData gamerdata;
        int ballKillCount;
        int coin20pickups = 0;

        public LocalGamer(GamerData data, int localIx, VectorRect hudArea, BagatellePlayState state)
            :base(hudArea, localIx, state)
        {
            this.gamerdata = data;
            initGamer();

            autoShootWarningText = new Display.SpriteText("", border.Center, border.Height * 0.5f, ImageLayers.Top2, VectorExt.V2Half,
                PjLib.RedNumbersColor, true);
            
            autoShootTime.Seconds = AutoShootSeconds + data.GamerIndex * 0.5f;
            refreshHud();
        }

        public void update()
        {
            if (gamerdata.button.DownEvent)
            {
                if (balls.Count == 0)
                {
                    fireBall();
                }
                else
                {   
                    for (int i = balls.Count - 1; i >= 0; --i)
                    {
                        balls[i].tap();
                    }
                    refreshHud();
                }
            }

            if (balls.Count == 0 && ballsLeft > 0)
            {
                if (autoShootTime.CountDown())
                {
                    fireBall();
                }
                else if (prevAutoShootSeconds != (int)autoShootTime.Seconds)
                {
                    prevAutoShootSeconds = (int)autoShootTime.Seconds;
                    if (prevAutoShootSeconds <= 2)
                    {
                        autoShootWarningText.Text((prevAutoShootSeconds + 1).ToString());
                    }
                }
            }
        }

        public void collectPoint(int value, AbsGameObject collectingItem, AbsGameObject pointGivingItem)
        {
            levelScore += value;

            new PjEngine.Effect.GainCoinsEffect(pointGivingItem.bound.Area.CenterTop, value, BagLib.PointEffectLayer);

            if (pointGivingItem.Type == GameObjectType.BigCoin20)
            {
                if (++coin20pickups == 2)
                {
                    PjRef.achievements.bagatelleBoth20s.Unlock();
                }
            }
        }

        public int knockoutPointLoss()
        {
            int loss = lib.SmallestValue(levelScore, 8);
            levelScore -= loss;
            return loss;
        }

        void fireBall()
        {
            if (ballsLeft > 0)
            {
                ballKillCount = 0;
                ballsLeft--;
                
                Ball b = new Ball(state.board.ballStartPos(), state.board.ballStartSpeed(), state.board.ballStartDir(), this, true, state);
                fireBallEffects(b);
                refreshHud();
            }
            autoShootWarningText.Text("");
            autoShootTime.Seconds = AutoShootSeconds;
        }

        public void onBallKill()
        {
            if (++ballKillCount == 2)
            {
                PjRef.achievements.bagatelleDoubleKill.Unlock();
            }
        }
        
        public void bumpRefillPickup(Ball b)
        {
            if (b.alive)
            {
                b.refillBumps();
                refreshHud();
            }
        }

        public override Network.AbsNetworkPeer NetworkPeer
        {
            get { return Ref.netSession.LocalPeer(); }//Ref.steam.P2PManager.localHost; }
        }

        public override GamerData GetGamerData()
        {
            return gamerdata;
        }
    }

    
}
