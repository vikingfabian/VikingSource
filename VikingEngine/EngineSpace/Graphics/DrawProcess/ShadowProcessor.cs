using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using VikingEngine.DSSWars;
using VikingEngine.Graphics;
using VikingEngine.LootFest.Players;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.EngineSpace.Graphics.DrawProcess
{
    class ShadowProcessor
    {
        
        private RenderTarget2D _shadowMap;
        public Effect shader;
        
        public Vector3 SunColor = new Vector3(0.5f, 0.45f, 0.45f);

        // Shadow map resolution
        private int _shadowMapSize = 2048;

        public LightProjection light;

        public ShadowProcessor() 
        {
            refreshMapSize();
            //LoadContent(); //temporary
            shader = Engine.Draw.shadowEffect;
            light = new LightProjection();
        }

        public void refreshMapSize()
        {
            _shadowMapSize = Resolution(Ref.gamesett.shadowResolution);
            CreateRenderTargets();
        }

        private void CreateRenderTargets()
        {
            _shadowMap?.Dispose();

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

        //public void EndShadowMapPass()
        //{
        //    Ref.draw.SetMainRenderTarget();
        //}

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

            
            //var SunIntensity = 0.5f;

            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            var modelToLight = shader.Parameters["ModelToLight"];

            var lp = Vector3.Normalize(Vector3.TransformNormal(light.lightPos, camera.ViewMatrix));
            float SpecularIntensity = 0.3f; // Intensity of specular highlights
            float Shininess = 8f; // Power of the specular highlights

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
            shader.Parameters["LightColor"]?.SetValue(SunColor * Ref.gamesett.modelBrightness);
            shader.Parameters["AmbientIntensity"]?.SetValue(0.8f * Ref.gamesett.modelBrightness);
            shader.Parameters["SpecularIntensity"]?.SetValue(SpecularIntensity * Ref.gamesett.modelBrightness);
            shader.Parameters["Shininess"]?.SetValue(Shininess * Ref.gamesett.modelBrightness);
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

        public static int Resolution(ShadowResolution shadowResolution)
        {
            switch (shadowResolution)
            {
                default:
                    return 1024;
                case ShadowResolution.Medium_2048:
                    return 2048;
                case ShadowResolution.High_4096:
                    return 4096;
                case ShadowResolution.VeryHigh_8192:
                    return 8196;
            }
        }

    }

    enum ShadowResolution
    { 
        Low_1024,
        Medium_2048,
        High_4096,
        VeryHigh_8192,
        NUM
    }
    
}
