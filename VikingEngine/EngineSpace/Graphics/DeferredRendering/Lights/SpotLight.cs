using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights
{
    class SpotLight
    {
        /* Properties */
        public Vector3 Position { get { return position; } set { position = value; } }
        public Vector3 Direction { get { return direction; } set { direction = value; direction.Normalize(); } }
        public Vector4 Color { get { return color; } set { color = value; } }
        public float Intensity { get { return intensity; } set { intensity = value; } }
        public float NearPlane { get { return nearPlane; } set { nearPlane = value; } }
        public float FarPlane { get { return farPlane; } set { farPlane = value; } }
        public float FOV { get { return fov; } set { fov = value; } }
        public bool CastShadows { get { return castShadows; } set { castShadows = value; } }
        public int ShadowMapResolution { get { return Math.Min(shadowMapResolution, 2048); } set { shadowMapResolution = value; } }
        public Matrix World { get { return world; } set { world = value; } }
        public Matrix View { get { return view; } set { view = value; } }
        public Matrix Projection { get { return projection; } set { projection = value; } }
        public RenderTarget2D ShadowMap { get { return shadowMap; } set { shadowMap = value; } }
        public Texture2D AttenuationTexture { get { return attenuationTexture; } set { attenuationTexture = value; } }

        /* Fields */
        Vector3 position;
        Vector3 direction;
        Vector4 color;
        float intensity;
        float nearPlane;
        float farPlane;
        float fov;
        bool castShadows;
        int shadowMapResolution;
        Matrix world;
        Matrix view;
        Matrix projection;
        RenderTarget2D shadowMap;
        Texture2D attenuationTexture;

        /* Constructors */
        public SpotLight(GraphicsDevice device, Vector3 position,
            Vector3 direction, Vector4 color, float intensity,
            bool castShadows, int shadowMapResolution,
            Texture2D attenuationTexture)
        {
            Position = position;
            Direction = direction;
            Color = color;
            Intensity = intensity;
            NearPlane = 2.0f * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            FarPlane = 100.0f * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            FOV = MathHelper.PiOver2;
            CastShadows = castShadows;
            ShadowMapResolution = shadowMapResolution;
            Projection = Matrix.CreatePerspectiveFieldOfView(FOV, 1.0f, NearPlane, FarPlane);
            //Projection = Matrix.CreateOrthographic(ShadowMapResolution, ShadowMapResolution, NearPlane, FarPlane);
            ShadowMap = new RenderTarget2D(device, ShadowMapResolution, ShadowMapResolution, false, SurfaceFormat.Single, DepthFormat.Depth24Stencil8);
            AttenuationTexture = attenuationTexture;
        }

        /* Novelty methods */
        public float CalculateAngleCos()
        {
            return (float)Math.Cos(FOV);
        }

        public void SetColor(Color color)
        {
            this.color = color.ToVector4();
        }

        public void Update()
        {
            Vector3 pos = position * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
            Vector3 target = pos + direction;
            if (target == Vector3.Zero)
            {
                target = Vector3.Down; // What, I can't look at the origin?
            }

            Vector3 up = Vector3.Cross(direction, Vector3.Up);

            if (up == Vector3.Zero)
                up = Vector3.Right;
            else
                up = Vector3.Up; // Almost always this... and why do the cross product at all then? :S

            View = Matrix.CreateLookAt(pos, target, up);
            float radial = (float)Math.Tan(FOV * 0.5f) * 2.0f * FarPlane;
            Matrix scaling = Matrix.CreateScale(radial, radial, FarPlane);
            Matrix translation = Matrix.CreateTranslation(pos);
            Matrix inverseView = Matrix.Invert(View);
            Matrix semiProduct = scaling * inverseView;
            Vector3 S;
            Vector3 P;
            Quaternion Q;
            semiProduct.Decompose(out S, out Q, out P);
            Matrix rotation = Matrix.CreateFromQuaternion(Q);

            World = scaling * rotation * translation;
        }
    }
}
