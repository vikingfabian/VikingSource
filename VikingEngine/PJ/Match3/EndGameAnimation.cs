using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Match3
{
    class EndGameAnimation
    {
        PJ.Display.TimesUpDisplay timesUp;
        AnimationState state;
        Time stateTime;
        List<Gamer> winners = new List<Gamer>();

        public EndGameAnimation(bool time)
        {
            if (time)
            {
                timesUp = new Display.TimesUpDisplay(0);
                state = AnimationState.TimeUpWatch;
                stateTime.Seconds = 2;
            }
            else
            {
                state = AnimationState.WaitForGamerDeath;
                stateTime.Seconds = 2;
            }
        }

        public void update()
        {
            switch (state)
            {
                case AnimationState.WaitForGamerDeath:
                    if (stateTime.CountDown())
                    {
                        state = AnimationState.BeginViewWinner;
                    }
                    break;
                case AnimationState.TimeUpWatch:
                    if (stateTime.CountDown())
                    {
                        timesUp.fadeOut();
                        state++;
                    }
                    break;

                case AnimationState.BeginViewWinner:
                    collectWinners();

                    if (winners.Count > 0)
                    {
                        m3Ref.sounds.winner.PlayFlat();

                        foreach (var m in winners)
                        {
                            Graphics.Image winnerIcon = new Graphics.Image(SpriteName.BirdThrophy,
                                m.animal.RealArea().PercentToPosition(new Vector2(0.5f, 0.3f)),
                                Engine.Screen.IconSizeV2, ImageLayers.Top9, true);

                            winnerIcon.Opacity = 0f;

                            const float FadeTime = 200;
                            new Graphics.Motion2d(Graphics.MotionType.MOVE, winnerIcon, new Vector2(0, -m.animal.Height * 0.8f),
                                Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);
                            new Graphics.Motion2d(Graphics.MotionType.OPACITY, winnerIcon, Vector2.One,
                                Graphics.MotionRepeate.NO_REPEAT, FadeTime, true);

                            new PJ.WinnerParticleEmitter(m.animal);
                        }

                        stateTime.Seconds = 1;
                        state = AnimationState.AnimateThrophy;
                    }
                    else
                    {
                        //NO WINNER
                        Graphics.Image winnerIcon = new Graphics.Image(SpriteName.BirdNoThrophy,
                            Engine.Screen.CenterScreen, Engine.Screen.IconSizeV2 * 2f, ImageLayers.Top9, true);

                        new Graphics.Motion2d(Graphics.MotionType.SCALE, winnerIcon, winnerIcon.Size * 0.5f,
                               Graphics.MotionRepeate.BackNForwardOnce, 120, true);

                        state = AnimationState.ExitButton;
                    }
                    break;
                case AnimationState.AnimateThrophy:
                    if (stateTime.CountDown())
                    {
                        state++;
                    }
                    break;
                case AnimationState.ExitButton:
                    m3Ref.gamestate.createExitButton();
                    state++;
                    break;
            }
        }

        void collectWinners()
        {
            FindMaxValuePointer<Gamer> winnerScore = new FindMaxValuePointer<Gamer>();
            winnerScore.maxValue = -1;

            foreach (var m in m3Ref.gamestate.gamers)
            {
                if (m.alive)
                {
                    winnerScore.Next(m.score, m);
                }
            }

            if (winnerScore.maxMember != null)
            {
                foreach (var m in m3Ref.gamestate.gamers)
                {
                    if (m.alive && m.score == winnerScore.maxValue)
                    {
                        winners.Add(m);
                    }
                }
            }
        }

        enum AnimationState
        {
            WaitForGamerDeath,
            TimeUpWatch,
            BeginViewWinner,
            AnimateThrophy,
            ExitButton,
        }
    }
}
