using System;
using Microsoft.Xna.Framework;


namespace VikingEngine.PJ.Match3
{
    class PointScoring
    {
        public int totalScore = 0;
        public int combos;

        public PointScoring(MatchCollection matches, int combos)
        {
            this.combos = combos;

            for (int matchIndex = 0; matchIndex < matches.matches.Count; ++matchIndex)
            {
                var match = matches.matches[matchIndex];

                int score = m3Lib.Scoring(match.bricks.Count, matchIndex, combos);
                totalScore += score;

                new ScoreText(score, matchIndex, match.center());
            }

        }
    }

    class ScoreText : AbsUpdateable
    {
        Vector2 center;
        int score;
        Display.SpriteText text;
        Time removeTimer = new Time(600);
        Time viewTimer;

        public ScoreText(int score, int index, Vector2 center)
            :base(true)
        {
            this.score = score;
            this.center = center;

            viewTimer.MilliSeconds = 200 * index;
        }

        public override void Time_Update(float time_ms)
        {
            if (text == null)
            {
                if (viewTimer.CountDown())
                {
                    float height = Engine.Screen.SmallIconSize * 0.9f;

                    if (score > 2000)
                    {
                        height *= 1.3f;
                    }
                    if (score > 500)
                    {
                        height *= 1.1f;
                    }
                    else if (score > 100)
                    {
                        height *= 1f;
                    }

                    text = new Display.SpriteText(score.ToString(), center,
                        height, m3Lib.LayerScoreText, VectorExt.V2Half, Color.White, true);
                }
            }
            else if (removeTimer.CountDown())
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            text.DeleteMe();
        }
    }

}
