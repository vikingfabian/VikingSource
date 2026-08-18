using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Map.Map2
{
    struct DrawMapOptions
    {
        public bool add;
        public float addHeight;

        public float? heightCap;
        public float centerHeight;
        public float edgeHeight;

        public float radius;
        public float flatRadius;
        public float hillRadius;
        public float flatness;
        public bool noise;
        public float noiseStrength;
        public double quadChance;

        public void refreshRadius()
        {
            flatRadius = radius * flatness;
            hillRadius = radius - flatRadius;

            if (addHeight > 0) //add
            {
                edgeHeight = centerHeight - (hillRadius * 0.1f * addHeight);
            }

            else
            {
                edgeHeight = Bound.Min(centerHeight - (hillRadius * 0.1f * addHeight), Map2Generator.Height_WaterBottom);
            }
            //centerHeight = addHeight;
        }

        public void adjustHeight(float add)
        {
            if (centerHeight + add > Map2Generator.Height_LowGround)
            {
                centerHeight += add;
                edgeHeight += add;
            }
        }
    }
}
