using VikingEngine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights
{
    class LightManager
    {
        /* Properties */
        public List<DirectionalLight> DirectionalLights { get { return directionalLights; } }
        public List<SpotLight> SpotLights { get { return spotLights; } }
        public List<PointLight> PointLights { get { return pointLights; } }

        /* Fields */
        private List<DirectionalLight> directionalLights;
        private List<SpotLight> spotLights;
        private List<PointLight> pointLights;
        private Effect depthWriter;

        /* Constructors */
        public LightManager()
        {
            directionalLights = new List<DirectionalLight>();
            spotLights = new List<SpotLight>();
            pointLights = new List<PointLight>();

            depthWriter = Engine.LoadContent.LoadShader("DeferredRenderer\\DepthWriter");
            depthWriter.CurrentTechnique = depthWriter.Techniques[0];
        }

        /* Novelty methods */
        public void AddLight(DirectionalLight light)
        {
            directionalLights.Add(light);
        }

        public void AddLight(SpotLight light)
        {
            spotLights.Add(light);
        }

        public void AddLight(PointLight light)
        {
            pointLights.Add(light);
        }

        public void RemoveLight(DirectionalLight light)
        {
            directionalLights.Remove(light);
        }

        public void RemoveLight(SpotLight light)
        {
            spotLights.Remove(light);
        }

        public void RemoveLight(PointLight light)
        {
            pointLights.Remove(light);
        }

        public void DrawShadowMaps(GraphicsDevice device)
        {
            device.BlendState = BlendState.Opaque;
            device.DepthStencilState = DepthStencilState.Default;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.CullCounterClockwiseFace;
            device.RasterizerState = rasterizerState;

            foreach (SpotLight light in spotLights)
            {
                light.Update();
                if (light.CastShadows)
                {
                    DrawShadowMap(device, light);
                }
            }

            foreach (PointLight light in pointLights)
            {
                if (light.CastShadows)
                {
                    DrawShadowMap(device, light);
                }
            }
        }

        public void DrawShadowMap(GraphicsDevice device, SpotLight light)
        {
            device.SetRenderTarget(light.ShadowMap);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(light.View);
            depthWriter.Parameters["Projection"].SetValue(light.Projection);
            depthWriter.Parameters["LightPosition"].SetValue(light.Position * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER);
            depthWriter.Parameters["FloatingPointPrecisionModifier"].SetValue(DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER);
            DrawModels(device, depthWriter);
        }

        public void DrawShadowMap(GraphicsDevice device, PointLight light)
        {
            Matrix[] views = new Matrix[6];

            Vector3 pos = light.Position * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;

            views[0] = Matrix.CreateLookAt(pos, pos + Vector3.Forward, Vector3.Up);
            views[1] = Matrix.CreateLookAt(pos, pos + Vector3.Backward, Vector3.Up);
            views[2] = Matrix.CreateLookAt(pos, pos + Vector3.Left, Vector3.Up);
            views[3] = Matrix.CreateLookAt(pos, pos + Vector3.Right, Vector3.Up);
            views[4] = Matrix.CreateLookAt(pos, pos + Vector3.Down, Vector3.Forward);
            views[5] = Matrix.CreateLookAt(pos, pos + Vector3.Up, Vector3.Backward);

            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 1.0f, 1.0f * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER, light.Radius * DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER);

            depthWriter.Parameters["Projection"].SetValue(projection);
            depthWriter.Parameters["LightPosition"].SetValue(pos);
            depthWriter.Parameters["FloatingPointPrecisionModifier"].SetValue(DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER);

            // Forward
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.PositiveZ);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[0]);
            DrawModels(device, depthWriter);

            // Backward
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.NegativeZ);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[1]);
            DrawModels(device, depthWriter);

            // Left
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.NegativeX);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[2]);
            DrawModels(device, depthWriter);

            // Right
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.PositiveX);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[3]);
            DrawModels(device, depthWriter);

            // Down
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.NegativeY);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[4]);
            DrawModels(device, depthWriter);

            // Up
            device.SetRenderTarget(light.ShadowMap, CubeMapFace.PositiveY);
            device.Clear(Color.Transparent);
            depthWriter.Parameters["View"].SetValue(views[5]);
            DrawModels(device, depthWriter);
        }

        public void DrawModels(GraphicsDevice device, Effect shader)
        {
            DeferredRenderer renderer = Ref.draw as DeferredRenderer;
            if (renderer != null)
            {
                renderer.DrawDepthOnly(device, shader);
            }
        }
    }
}
