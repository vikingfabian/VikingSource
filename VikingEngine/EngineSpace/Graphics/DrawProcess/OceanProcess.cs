//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VikingEngine.Graphics;
//using VikingEngine.ToGG.Commander.UnitsData;

//namespace VikingEngine.EngineSpace.Graphics.DrawProcess
//{
//    class OceanProcess
//    {
//        Mesh waterPlane;
//        public Effect shader;
//        public OceanProcess(Mesh waterPlane)
//        { 
//            this.waterPlane = waterPlane;
//            shader = Engine.Draw.oceanEffect;

//            initShader();
//        }

//        public void draw(int layer, AbsCamera camera, int cameraIndex, LightProjection light, RenderTarget2D SceneDepthMap)
//        {
//            Engine.Draw.PreviousVertexBuffer = -1;

//            initShader();
//            //var SunIntensity = 0.5f;

//            Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.AlphaBlend;
//            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

//            //var modelToLight = shader.Parameters["ModelToLight"];

//            var lp = Vector3.Normalize(Vector3.TransformNormal(light.lightPos, camera.ViewMatrix));
//            float SpecularIntensity = 0.3f; // Intensity of specular highlights
//            float Shininess = 8f; // Power of the specular highlights

            
//            shader.CurrentTechnique = shader.Techniques["Flat"];

//            //shader.Parameters["WaterTint"].SetValue(new Vector3(0f, 0.45f, 0.9f));
//            //shader.Parameters["WaterAlpha"].SetValue(1f);

//            //shader.Parameters["Time"]?.SetValue(Ref.TotalGameTimeSec);
//            //shader.Parameters["LightPosition"]?.SetValue(lp);
//            //shader.Parameters["LightColor"]?.SetValue(light.SunColor * Ref.gamesett.modelBrightness);
//            //shader.Parameters["AmbientIntensity"]?.SetValue(0.8f * Ref.gamesett.modelBrightness);
//            //shader.Parameters["SpecularIntensity"]?.SetValue(SpecularIntensity * Ref.gamesett.modelBrightness);
//            //shader.Parameters["Shininess"]?.SetValue(Shininess * Ref.gamesett.modelBrightness);
//            shader.Parameters["SceneDepthMap"]?.SetValue(SceneDepthMap);

//            waterPlane.DrawOcean(cameraIndex, camera, shader, light);
//        }

//        void initShader()
//        {
//#if DEBUG
//            //shader.Parameters["WaveDirection"].SetValue(new Vector2(1f, 0f));
//            //shader.Parameters["WaveSpeed"].SetValue(0.3f);
//            //shader.Parameters["WaveScale"].SetValue(0.001f);
//            //shader.Parameters["WaveAmplitude"].SetValue(0.25f);

//            //shader.Parameters["WaterAlbedo"].SetValue(new Vector3(0.06f, 0.25f, 0.55f));
//            //shader.Parameters["WaterAlpha"].SetValue(1f);

//            //shader.Parameters["FoamColor"].SetValue(new Vector3(1f, 1f, 1f));
//            //shader.Parameters["FoamDepthThreshold"].SetValue(0.008f);
//            //shader.Parameters["FoamSoftness"].SetValue(0.01f);
//            //shader.Parameters["FoamNoiseScale"].SetValue(1.5f);

//            //// Depth remap (D3D-style). For GL NDC [-1,1], use (0.5f, 0.5f) instead.
//            //shader.Parameters["DepthRemapA"].SetValue(1f);
//            //shader.Parameters["DepthRemapB"].SetValue(0f);

//            //// Toon
//            //shader.Parameters["ToonBands"].SetValue(3);
//            //shader.Parameters["HighlightColor"].SetValue(new Vector3(1f, 1f, 1f));
//            //shader.Parameters["SpecularThreshold"].SetValue(0.6f);
//#endif
//        }
//    }
//}
