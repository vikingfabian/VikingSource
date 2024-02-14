using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.PJ.CarBall
{
    class GoalieSpot
    {
        public float startDir;
        public Graphics.Image image;
        public Physics.CircleBound bound;

        public IntervalF moveRange;
        public float speedMultiplier;

        public GoalieSpot(bool top, FieldHalf field, VectorRect fieldArea)
        {
            Vector2 sz = new Vector2(fieldArea.Height * 0.036f);
            float leftEdge = sz.X * 0.4f;
            float topEdge = sz.Y * 2f;
            Vector2 center = Vector2.Zero;

            if (field.leftHalf)
            {
                center.X = field.goalArea.Right + leftEdge;
            }
            else
            {
                center.X = field.goalArea.X - leftEdge;
            }

            if (top)
            {
                startDir = 1;
                center.Y = field.goalArea.Y + topEdge;
            }
            else
            {
                startDir = -1;
                center.Y = field.goalArea.Bottom - topEdge;
            }

            image = new Graphics.Image(field.leftHalf? SpriteName.cballGloveBlue : SpriteName.cballGloveRed, center, sz, cballLib.LayerGoalieSpot, true);
            image.Opacity = 0.9f;
            bound = new Physics.CircleBound(center, sz.X * 0.5f);

            float moveRangeEdge = -fieldArea.Height * 0.02f;
            moveRange = new IntervalF(field.goalArea.Y + moveRangeEdge, field.goalArea.Bottom - moveRangeEdge);

            speedMultiplier = field.percSize < 0.6f ? 0.8f : 1f;
        }
    }
}
