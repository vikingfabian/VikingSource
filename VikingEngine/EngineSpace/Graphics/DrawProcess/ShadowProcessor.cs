using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.EngineSpace.Graphics.DrawProcess
{
    class ShadowProcessor
    {
        //const bool DebugMode = true;

        //private GraphicsDevice _graphicsDevice;
        private RenderTarget2D _shadowMap;
        private Effect shader;
        

        // Light properties
        //public Vector3 LightDirection { get; set; }
        //public Vector3 LightPosition
        //{
        //    get
        //    {
        //        return TargetPosition + (LightDirection * 800.0f);
        //    }
        //}

        public float SpecularIntensity { get; set; }
        public float Shininess { get; set; }
        //public float SunIntensity { get; set; }
        //public Vector3 SunColor { get; set; }

        public Vector3 TargetPosition { get; set; }
        public Vector3 UpVector { get; set; } = Vector3.Up;

        // Matrices
        private Matrix _lightViewMatrix;
        private Matrix _lightProjectionMatrix;

        // Shadow map resolution
        private int _shadowMapSize = 2048;

        public LightProjection light;
        Rectangle debugDrawArea;

        public ShadowProcessor() 
        {
            CreateRenderTargets();
            LoadContent(); //temporary

            light = new LightProjection(_shadowMapSize);
        }
        public void LoadContent()
        {
            shader = Engine.LoadContent.LoadShader("ShadowEffect");
        }

        private void CreateRenderTargets()
        {
            _shadowMap = new RenderTarget2D(
                Engine.Draw.graphicsDeviceManager.GraphicsDevice,
                _shadowMapSize,
                _shadowMapSize,
                false,
                SurfaceFormat.Single, // Use Single for higher precision depth values
                DepthFormat.Depth24);
        }


        //private void UpdateLightMatrices()
        //{
        //    // Create view matrix from light's perspective
        //    _lightViewMatrix = Matrix.CreateLookAt(
        //        LightPosition,
        //        TargetPosition, // look at target
        //        UpVector);

        //    // Create orthographic projection for directional light
        //    _lightProjectionMatrix = Matrix.CreateOrthographic(
        //        2048, 2048, 0.1f, 5000f);
        //}



        public void BeginShadowMapPass()
        {
            //    // Update light matrices based on current light position.
            //    UpdateLightMatrices();

            //    // Set render target to shadow map.
                Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(_shadowMap);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //    // Clear with white (meaning far depth).
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.Clear(Color.White);

               shader.CurrentTechnique = shader.Techniques["RenderDepth"];
        }

        public void EndShadowMapPass()
        {
            Ref.draw.SetMainRenderTarget();
        }

        public void DrawRenderListMembersDepthOnly(int layer, DrawObjType objType, int cameraIndex)
        {
            Engine.Draw.PreviousVertexBuffer = -1;

            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(Ref.draw.renderList[layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawDepthOnly(shader, light, cameraIndex);
                }
            }
        }

        public void DrawDebug()
        {
            VectorRect area = new VectorRect(Engine.Screen.CenterScreen, Engine.Screen.Area.Size * VectorExt.V2Half);
            Ref.draw.DebugDrawRenderTarget(_shadowMap, area.Rectangle);
        }


        public void DrawModelsWithShadow(int layer, DrawObjType objType, AbsCamera camera, int cameraIndex)
        {
            Engine.Draw.PreviousVertexBuffer = -1;

            var SunColor = new Vector3(1.0f, 0.9f, 0.9f);
            var SunIntensity = 1.1f;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            var modelToLight = shader.Parameters["ModelToLight"];

            var lp = Vector3.Normalize(Vector3.TransformNormal(light.lightPos, camera.ViewMatrix));
            float SpecularIntensity = 0.5f; // Intensity of specular highlights
            float Shininess = 16f; // Power of the specular highlights

            switch (objType)
            {
               default:
                    shader.CurrentTechnique = shader.Techniques["RenderTextured"];
                    break;
                case DrawObjType.MeshGenerated:
                    shader.CurrentTechnique = shader.Techniques["RenderVertexColor"];
                    break;
            }
            shader.Parameters["LightPosition"]?.SetValue(lp);
            shader.Parameters["LightColor"]?.SetValue(SunColor * SunIntensity);
            shader.Parameters["AmbientIntensity"]?.SetValue(0.8f);
            //effect.Parameters["Color"]?.SetValue(color.ToVector4());
            shader.Parameters["SpecularIntensity"]?.SetValue(SpecularIntensity);
            shader.Parameters["Shininess"]?.SetValue(Shininess);
            shader.Parameters["ShadowMap"]?.SetValue(_shadowMap);
            shader.Parameters["EdgeFadeScale"]?.SetValue(10.0f);
            shader.Parameters["ShadowMap"]?.SetValue(_shadowMap);

            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(Ref.draw.renderList[layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawWithShadow(cameraIndex, camera, shader, light);
                }
            }
        }

    }
    
}
