using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LootFest.GO.Physics
{
    /// <summary>
    /// Limits a gameObject to a limited area
    /// </summary>
    class RectangleAreaBoundary
    {
        public Rectangle2 areaWorldXZ;
        public bool hardBound = true;

        public RectangleAreaBoundary(Rectangle2 areaWorldXZ)
        {
            this.areaWorldXZ = areaWorldXZ;
        }

        public void update(GO.Characters.AbsCharacter go)
        {
            if (go.WorldPos.WorldGrindex.X < areaWorldXZ.X)
            {
                if (hardBound)
                { go.X = areaWorldXZ.X; }
                go.HandleObsticle(true, null);
            }
            else if (go.WorldPos.WorldGrindex.X > areaWorldXZ.Right)
            {
                if (hardBound)
                { go.X = areaWorldXZ.Right; }
                go.HandleObsticle(true, null);
            }

            if (go.WorldPos.WorldGrindex.Z < areaWorldXZ.Y)
            {
                if (hardBound)
                { go.Z = areaWorldXZ.Y; }
                go.HandleObsticle(true, null);
            }
            else if (go.WorldPos.WorldGrindex.Z> areaWorldXZ.Bottom)
            {
                if (hardBound)
                { go.Z = areaWorldXZ.Bottom; }
                go.HandleObsticle(true, null);
            }
        }
    }
}
