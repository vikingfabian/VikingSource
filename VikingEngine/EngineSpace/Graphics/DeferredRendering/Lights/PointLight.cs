using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights
{
    class PointLight
    {
        /* Properties */
        public Vector3 Position { get { return position; } set { position = value; } }
        public float Radius { get { return radius; } set { radius = value; } }
        public Vector4 Color { get { return color; } set { color = value; } }
        public float Intensity { get { return intensity; } set { intensity = value; } }
        public bool CastShadows { get { return castShadows; } set { castShadows = value; } }
        public int ShadowMapResolution { get { return Math.Min(shadowMapResolution, 2048); } set { shadowMapResolution = value; } }
        public RenderTargetCube ShadowMap { get { return shadowMap; } }

        /* Fields */
        Vector3 position;
        float radius;
        Vector4 color;
        float intensity;
        RenderTargetCube shadowMap;
        bool castShadows;
        int shadowMapResolution;

        /* Constructors */
        public PointLight(GraphicsDevice device, Vector3 position, float radius, Vector4 color, float intensity, bool castShadows, int shadowMapResolution)
        {
            Position = position;
            Radius = radius;
            Color = color;
            Intensity = intensity;
            CastShadows = castShadows;
            ShadowMapResolution = shadowMapResolution;

            shadowMap = new RenderTargetCube(device, ShadowMapResolution, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
        }

        /* Novelty methods */
        public void SetColor(Color color)
        {
            Color = color.ToVector4();
        }
    }
}
