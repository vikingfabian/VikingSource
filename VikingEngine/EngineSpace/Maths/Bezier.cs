using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Maths
{
    class Bezier
    {
        List<Vector2> points;

        public void SetControlPoints(List<Vector2> controlPoints)
        {
            points = controlPoints;
        }

        public void ClearControlPoints()
        {
            points = null;
        }

        public Vector2 GetPoint(float t)
        {
            if (points.Count != 0)
                return DeCasteljauGetPoint(points.Count - 1, 0, t);
            return Vector2.Zero;
        }

        private Vector2 DeCasteljauGetPoint(int r, int i, float t)
        {
            if (r == 0)
                return points[i];

            Vector2 u = DeCasteljauGetPoint(r - 1, i, t);
            Vector2 v = DeCasteljauGetPoint(r - 1, i + 1, t);

            return MathExt.Lerp(u, v, t);
        }
    }
}
