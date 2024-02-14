using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights
{
    class DirectionalLight
    {
        /* Properties */
        public Vector3 Direction { get { return direction; } set { direction = value; direction.Normalize(); } }
        public Vector4 Color     { get { return color; }     set { color = value; } }
        public float Intensity   { get { return intensity; } set { intensity = value; } }

        /* Fields */
        Vector3 direction;
        Vector4 color;
        float intensity;

        /* Constructors */
        public DirectionalLight(Vector3 direction, Vector4 color, float intensity)
        {
            Direction = direction;
            Color     = color;
            Intensity = intensity;
        }

        public DirectionalLight(Vector3 direction, Color color, float intensity)
        {
            Direction = direction;
            Color     = color.ToVector4();
            Intensity = intensity;
        }

        /* Novelty methods */
        public void SetColor(Color color)
        {
            Color = color.ToVector4();
        }
    }
}
