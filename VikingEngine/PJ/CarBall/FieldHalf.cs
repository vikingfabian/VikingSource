using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class FieldHalf
    {
        public float percSize = 1f;

        public bool leftHalf;
        public FieldHalf opposite;
        public int score = 0;
        //public Graphics.TextG scoreText;
        public VectorRect goalArea;
        public Grid2D<Vector2> startPositions;
        public Rotation1D startDir;
        public int teamCount = 0;
        Graphics.Image[] scoreMarkers;
        public bool availableGoalSpots = true;
        public Goalie goalie = null;

        public GoalieSpot[] golieSpots;
        public Graphics.Image goalImg;

        public FieldHalf(VectorRect fieldArea, bool leftHalf)
        {
            this.leftHalf = leftHalf;

            startPositions = new Grid2D<Vector2>(new IntVector2(2, 3));
            float x1 = fieldArea.Width * 0.16f;
            float x2 = fieldArea.Width * 0.34f;

            if (leftHalf)
            {
                x1 = fieldArea.X + x1;
                x2 = fieldArea.X + x2;
            }
            else
            {
                x1 = fieldArea.Right - x1;
                x2 = fieldArea.Right - x2;
            }

            float yEdge = 0.24f * fieldArea.Height;

            float top, bottom;
            float yAdj = lib.BoolToLeftRight(leftHalf) * cballRef.ballScale * 0.8f;

            if (leftHalf)
            {
                top = fieldArea.Y + yEdge;
                bottom = fieldArea.Bottom - yEdge;
                startDir = Rotation1D.D90;
            }
            else
            {
                bottom = fieldArea.Y + yEdge;
                top = fieldArea.Bottom - yEdge;
                startDir = Rotation1D.D270;
            }

            top += yAdj;
            float midY = fieldArea.Center.Y + yAdj;
            bottom += yAdj;

            startPositions.Set(new IntVector2(0, 0), new Vector2(x1, top));
            startPositions.Set(new IntVector2(1, 0), new Vector2(x2, top));

            startPositions.Set(new IntVector2(0, 1), new Vector2(x1, midY));
            startPositions.Set(new IntVector2(1, 1), new Vector2(x2, midY));

            startPositions.Set(new IntVector2(0, 2), new Vector2(x1, bottom));
            startPositions.Set(new IntVector2(1, 2), new Vector2(x2, bottom));

            if (cballLib.DebugStartPoistions)
            {
                startPositions.LoopBegin();
                while (startPositions.LoopNext())
                {
                    new Graphics.Image(SpriteName.WhiteArea, startPositions.LoopValueGet(), new Vector2(2), ImageLayers.Background2, true);
                }
            }           
        }

        public void scorePoint()
        {
            score++;

            int i = score - 1;
            if (arraylib.InBound(scoreMarkers, i))
            {
                Graphics.Image img = scoreMarkers[i];
                Graphics.Image wave = (Graphics.Image)img.CloneMe();
                wave.SetSpriteName(SpriteName.ClickCirkleEffect);
                wave.size *= 1.5f;

                img.SetSpriteName(SpriteName.cballScoreFilled);

                new Graphics.Motion2d(Graphics.MotionType.SCALE, img, img.Size * 0.6f, Graphics.MotionRepeate.BackNForwardOnce, 120, true);

                const float WaveTime = 4000;
                new Graphics.Motion2d(Graphics.MotionType.SCALE, wave, wave.Size * 16f, Graphics.MotionRepeate.NO_REPEAT, WaveTime, true);
                new Graphics.Motion2d(Graphics.MotionType.OPACITY, wave, VectorExt.V2NegOne, Graphics.MotionRepeate.NO_REPEAT, WaveTime, true);
                new Timer.Terminator(WaveTime, wave);
            }
            //for (int i = 0; i < scoreMarkers.Length; ++i)
            //{
            //    if (i < score)
            //    {
            //        scoreMarkers[i].SetSpriteName(SpriteName.birdBallTrace);
            //    }
            //}

            if (score >= cballLib.GoalCount)
            {
                cballRef.state.onFinalGoal(leftHalf);
            }
        }

        public void initGoal(VectorRect fieldArea, bool leftHalf)
        {
            Color goalCol;
            SpriteName sprite;
            goalArea.Height = fieldArea.Height * 0.4f;

            if (teamCount < opposite.teamCount)
            {
                percSize = (float)teamCount / opposite.teamCount;

                goalArea.Height *= percSize;
            }
            goalArea.Width = goalArea.Height / SpriteSheet.CballGoalTextSz.Y * SpriteSheet.CballGoalTextSz.X;
            goalArea.Center = fieldArea.Center;

            VectorRect spriteArea = goalArea;
            float xAdj = goalArea.Width * 0.04f;

            if (leftHalf)
            {
                sprite = SpriteName.cballGoalBlue;
                goalCol = Color.Blue;

                goalArea.SetRight(fieldArea.X, false);
                spriteArea.X = goalArea.X + xAdj;
            }
            else
            {
                sprite = SpriteName.cballGoalRed;
                goalCol = Color.Red;
                goalArea.X = fieldArea.Right;
                spriteArea.X = goalArea.X - xAdj;
            }

            goalArea.AddYRadius(-(9f / 160f) * spriteArea.Height);
            
            goalImg = new Graphics.Image(sprite, spriteArea.Center, spriteArea.Size, cballLib.LayerGoal, true);

            Graphics.Image goalAreaBG = new Graphics.Image(SpriteName.WhiteArea_LFtiles, goalArea.Position, goalArea.Size, cballLib.LayerGoalBG, false, true);
            float spriteNetEdge = spriteArea.Width * 0.12f;
            if (leftHalf)
            { goalAreaBG.SetLeft(spriteArea.X + spriteNetEdge); }
            else
            { goalAreaBG.SetRight(spriteArea.Right - spriteNetEdge, true); }
            goalAreaBG.Color = new Color(117, 145, 42);

            Graphics.Image goalAreaBGLine = (Graphics.Image)goalAreaBG.CloneMe();
            goalAreaBGLine.Color = Color.Black;
            goalAreaBGLine.Opacity = 0.1f;

            goalAreaBGLine.Width = Convert.ToInt32(fieldArea.Height * 0.01f);
            goalAreaBGLine.LayerAbove(goalAreaBG);

            if (leftHalf)
            {
                goalAreaBGLine.SetRight(goalArea.Right, false);
            }

            Vector2 scoreMarkPos = fieldArea.Center;
            scoreMarkPos.X += fieldArea.Height * 0.27f * lib.BoolToLeftRight(!leftHalf);
            float spacing = fieldArea.Height * 0.06f * lib.BoolToLeftRight(!leftHalf);
            Vector2 scoreMarkScale = new Vector2(fieldArea.Height * 0.05f);

            scoreMarkers = new Graphics.Image[cballLib.GoalCount];
            for (int i = 0; i < cballLib.GoalCount; ++i)
            {
                scoreMarkers[i] = new Graphics.Image(SpriteName.cballScoreOutline, scoreMarkPos, scoreMarkScale, ImageLayers.Background7, true);
                scoreMarkers[i].Color = goalCol;
                scoreMarkers[i].Opacity = 0.2f;

                scoreMarkPos.X += spacing;
            }

            golieSpots = new GoalieSpot[]
                {
                    new GoalieSpot(true, this, fieldArea),
                    new GoalieSpot(false, this, fieldArea)
                };
        }
        
        public void resetGolieSpots()
        {
            golieVisible(true);
        }

        public void golieVisible(bool visible)
        {
            availableGoalSpots = visible;
            foreach (var m in golieSpots)
            {
                m.image.Visible = visible;
            }
        }
    }
}
