using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map.Settings;

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

        //public BiomType biom;

        public void refreshHeight()
        {
            centerHeight = addHeight;
        }

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

        public void refreshRadius_PaintTool()
        {
            flatRadius = radius * flatness;
            hillRadius = radius - flatRadius;

            //if (addHeight > 0) //add
            //{
            //    edgeHeight = centerHeight - (hillRadius * 0.1f * addHeight);
            //}

            //else
            //{
                edgeHeight = 0.05f * centerHeight;
            //}
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
