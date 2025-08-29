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

        
        
        private RenderTarget2D _shadowMap;
        Effect shader;
        
        public float SpecularIntensity { get; set; }
        public float Shininess { get; set; }

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
            //LoadContent(); //temporary
            shader = Engine.Draw.shadowEffect;
            light = new LightProjection(_shadowMapSize);
        }
        //public void LoadContent()
        //{
        //    shader = Engine.LoadContent.LoadShader("ShadowEffect");
        //}

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


        public void BeginShadowMapPass()
        {
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(_shadowMap);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

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
                    model.DrawDepthOnly(true, shader, light, cameraIndex);
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

        public void Dispose()
        {
            _shadowMap?.Dispose();
            _shadowMap = null;
        }

    }

    enum ShadowResolution
    { 
        Low_1024,
        Medium_2048,
        High_4096,
        NUM
    }
    
}
