using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights;
using VikingEngine.Graphics;
using VikingEngine.LootFest;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.EngineSpace.Graphics.DeferredRendering
{
    class DeferredRenderer : Engine.Draw
    {
        /* Enums */
        enum DeferredRendererShaders
        {
            Clear,
            GBuffer,
            DirectionalLight,
            PointLight,
            SpotLight,
            Composition,
            NUM
        }

        /* Constants */
        //public const float FLOATING_POINT_PRECISION_MODIFIER = 1f / 64;
        public const float FLOATING_POINT_PRECISION_MODIFIER = 1f;

        /* Properties */
        public RenderTargetBinding[] GBuffer { get { return gBufferTargets; } }
        public bool EnableSSAO { get { return enableSSAO; } set { enableSSAO = value; } }
        public bool DebugSSAO { get { return debugSSAO; } set { debugSSAO = value; } }
        public bool DebugTargets { get { return debugTargets; } set { debugTargets = value; } }
        public float SSAOAmount { get { return ssaoAmount; } set { ssaoAmount = value; } }
        public float LightMapAmount { get { return lightMapAmount; } set { lightMapAmount = value; } }

        /* Fields */
        Effect[] shaders;
        BlendState lightMapBlendState;

        RenderTargetBinding[] gBufferTargets;
        Vector2 gBufferTextureSize;
        RenderTarget2D lightMap;
        FullscreenQuad fullscreenQuad;

        public LightManager lightManager; // Maybe this should be outside the class
        Model pointLightGeometry;
        Model spotLightGeometry;
        public SSAO ssao;
        RenderTarget2D ssaoTarget;
        bool enableSSAO;
        bool debugSSAO;
        bool debugTargets;
        float ssaoAmount;
        float lightMapAmount;
        GaussianBlur gaussianBlur = new GaussianBlur(0.05f);


        /* Constructors */
        public DeferredRenderer()
            : base()
        {
            ssao = new SSAO(graphicsDeviceManager.GraphicsDevice, Screen.RenderingResolution.X, Screen.RenderingResolution.Y);
            ssaoTarget = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice, Screen.RenderingResolution.X, Screen.RenderingResolution.Y, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            ssaoAmount = 0.5f;
            lightMapAmount = 0.5f;

            shaders = new Effect[(int)DeferredRendererShaders.NUM];
            for (int i = 0; i < (int)DeferredRendererShaders.NUM; ++i)
            {
                LoadShader((DeferredRendererShaders)i);
            }

            lightMapBlendState = new BlendState();
            lightMapBlendState.ColorSourceBlend = Blend.One;
            lightMapBlendState.ColorDestinationBlend = Blend.One;
            lightMapBlendState.ColorBlendFunction = BlendFunction.Add;
            lightMapBlendState.AlphaSourceBlend = Blend.One;
            lightMapBlendState.AlphaDestinationBlend = Blend.One;
            lightMapBlendState.AlphaBlendFunction = BlendFunction.Add;

            gBufferTextureSize = Engine.Screen.RenderingResolution.Vec; // TODO(Martin): Change on resize!

            gBufferTargets = new RenderTargetBinding[3];

            gBufferTargets[0] = new RenderTargetBinding(
                new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
                    (int)gBufferTextureSize.X, (int)gBufferTextureSize.Y,
                    false, SurfaceFormat.Rgba64,
                    DepthFormat.Depth24Stencil8));
            gBufferTargets[1] = new RenderTargetBinding(
                new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
                    (int)gBufferTextureSize.X, (int)gBufferTextureSize.Y,
                    false, SurfaceFormat.Rgba64,
                    DepthFormat.Depth24Stencil8));
            gBufferTargets[2] = new RenderTargetBinding(
                new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
                    (int)gBufferTextureSize.X, (int)gBufferTextureSize.Y,
                    false, SurfaceFormat.Vector2,
                    DepthFormat.Depth24Stencil8));

            lightMap = new RenderTarget2D(graphicsDeviceManager.GraphicsDevice,
                (int)gBufferTextureSize.X, (int)gBufferTextureSize.Y,
                false, SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);

            fullscreenQuad = new FullscreenQuad(graphicsDeviceManager.GraphicsDevice);

            //spotLightGeometry = Engine.LoadContent.Mesh(LoadedMesh.cone45deg);
            pointLightGeometry = Engine.LoadContent.Mesh(LoadedMesh.sphere);

            // Maybe this should be outside the class...
            lightManager = new LightManager();
            lightManager.AddLight(new Lights.DirectionalLight(
                new Vector3((float)Math.Cos(0), -1, (float)Math.Sin(0)),
                Color.White, 0.5f));
            lightManager.AddLight(new Lights.DirectionalLight(
                new Vector3((float)Math.Cos(MathHelper.Pi / 3), -1, (float)Math.Sin(MathHelper.Pi / 3)),
                Color.White, 0.5f));
            lightManager.AddLight(new Lights.DirectionalLight(
                new Vector3((float)Math.Cos(2 * MathHelper.Pi / 3), -1, (float)Math.Sin(2 * MathHelper.Pi / 3)),
                Color.White, 0.5f));

            //Texture2D spotCookie = LoadContent.Texture(LoadedTexture.SpotlightCookie);
            //lightManager.AddLight(
            //    new SpotLight(
            //        GraphicsDevice,
            //        new Vector3(1100, 40.0f, 1100),
            //        new Vector3(0, -1, 0),
            //        Color.White.ToVector4(),
            //        0.5f,
            //        true,
            //        1024,
            //        spotCookie));

            //lightManager.AddLight(
            //    new SpotLight(
            //        GraphicsDevice,
            //        new Vector3(1100, 40.0f, 1100),
            //        new Vector3(0, -1, 0),
            //        Color.Green.ToVector4(),
            //        0.7f,
            //        true,
            //        2048,
            //        spotCookie));

            //lightManager.AddLight(
            //    new PointLight(GraphicsDevice,
            //        new Vector3(1100, 40, 1100),
            //        50,
            //        Color.White.ToVector4(),
            //        1.0f,
            //        true,
            //        512));

            //lightManager.AddLight(
            //    new PointLight(GraphicsDevice,
            //        new Vector3(1100, 40, 1100),
            //        50,
            //        Color.Blue.ToVector4(),
            //        1.0f,
            //        true,
            //        512));
            //lightManager.AddLight(
            //    new SpotLight(
            //        GraphicsDevice,
            //        new Vector3(1100, 40.0f, 1100),
            //        new Vector3(0, -1, 0),
            //        Color.White.ToVector4(),
            //        0.7f,
            //        true,
            //        2048,
            //        spotCookie));
            //lightManager.AddLight(new Lights.DirectionalLight(
            //    new Vector3((float)Math.Cos(MathHelper.TwoPi / 3f), -1, (float)Math.Sin(MathHelper.TwoPi / 3f)),
            //    Color.White, 0.5f));
            //lightManager.AddLight(new Lights.DirectionalLight(
            //    new Vector3((float)Math.Cos(2f * MathHelper.TwoPi / 3f), -1, (float)Math.Sin(2f * MathHelper.TwoPi / 3f)),
            //    Color.White, 0.5f));

            if (DebugSett.DebugDeferredRenderer)
            {
                enableSSAO = true;
                debugSSAO = true;
                debugTargets = true;
            }
        }

        /* Family methods */
        protected override void drawEvent()
        {
            // Draw shadows
            lightManager.DrawShadowMaps(graphicsDeviceManager.GraphicsDevice);

            // Draw 3D deferred
            graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            graphicsDeviceManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            ClearGBuffer(graphicsDeviceManager.GraphicsDevice);

            Viewport savedViewport = graphicsDeviceManager.GraphicsDevice.Viewport;
            for (int camIx = 0; camIx < ActivePlayerScreens.Count; ++camIx)
            {
                PlayerData p = ActivePlayerScreens[camIx];
                Camera = p.view.Camera;
                Camera.RecalculateMatrices();
                Camera.SetPersonVisible(false);
                graphicsDeviceManager.GraphicsDevice.SetRenderTargets(gBufferTargets);
                graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;
                //SkyDome.X = Camera.Target.X;
                //SkyDome.Z = Camera.Target.Z;
                MakeGBuffer(graphicsDeviceManager.GraphicsDevice, Camera.ViewMatrix, Camera.Projection, (int)p.localPlayerIndex);
                Camera.SetPersonVisible(true);
                //lightManager.PointLights[camIx].Position = LfRef.LocalHeroes[camIx].Player.hero.Position + new Vector3(0, 10, 0);
                //lightManager.SpotLights[camIx].Position = LfRef.LocalHeroes[camIx].Player.hero.Position + new Vector3(0, 10, 0);
                //lightManager.SpotLights[camIx].Direction = new Vector2toV3(LfRef.LocalHeroes[camIx].Player.hero.Rotation.Direction(1)) + new Vector3(0, -1, 0);
            }

            graphicsDeviceManager.GraphicsDevice.Viewport = savedViewport;

            graphicsDeviceManager.GraphicsDevice.SetRenderTarget(lightMap);
            graphicsDeviceManager.GraphicsDevice.Clear(Color.Transparent);
            for (int camIx = 0; camIx < ActivePlayerScreens.Count; ++camIx)
            {
                PlayerData p = ActivePlayerScreens[camIx];
                Camera = p.view.Camera;
                graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;
                MakeLightMap(graphicsDeviceManager.GraphicsDevice, lightManager, Camera, p.view.Viewport);
            }

            // Spinning the light around
            //Vector3 oldDir = lightManager.DirectionalLights[0].Direction;
            //Vector3 newDir = oldDir;
            //float theta = 0.1f;
            //float cosT = (float)Math.Cos(theta);
            //float sinT = (float)Math.Sin(theta);
            //newDir.X = oldDir.X * cosT + oldDir.Y * sinT;
            //newDir.Y = oldDir.Y * cosT - oldDir.X * sinT;
            //lightManager.DirectionalLights[0].Direction = newDir;

            if (enableSSAO)
            {
                ssao.Draw(graphicsDeviceManager.GraphicsDevice, gBufferTargets, Camera, ssaoTarget);
                MakeFinal(graphicsDeviceManager.GraphicsDevice, null);
                if (DebugSSAO)
                    ssao.Debug(spriteBatch);
            }
            else
            {
                MakeFinal(graphicsDeviceManager.GraphicsDevice, null);
            }

            if (debugTargets)
            {
                Debug(graphicsDeviceManager.GraphicsDevice, spriteBatch);
            }

            // Draw particles
            for (int camIx = 0; camIx < ActivePlayerScreens.Count; ++camIx)
            {
                PlayerData p = ActivePlayerScreens[camIx];
                Camera = p.view.Camera;
                Camera.updateBillboard();
                graphicsDeviceManager.GraphicsDevice.Viewport = p.view.Viewport;
                if (DebugSett.Debug3DParticles)
                {
                    // TODO(Martin): This only works for in single client mode
                    // Also, lots more to fix on particle system
                    instancing.Draw(ref Camera.ViewMatrix, ref Camera.Projection);
                }
                Engine.ParticleHandler.Draw();
            }
            graphicsDeviceManager.GraphicsDevice.Viewport = savedViewport;

            // Draw GUI
            Draw2d(0);
        }

        /* Novelty methods */
        void LoadShader(DeferredRendererShaders shaderName)
        {
            Effect shader = Engine.LoadContent.LoadShader("DeferredRenderer\\" + shaderName.ToString());
            shader.CurrentTechnique = shader.Techniques[0];
            shaders[(int)shaderName] = shader;
        }

        void MakeFinal(GraphicsDevice device, RenderTarget2D output)
        {
            device.SetRenderTarget(output);
            device.Clear(Color.Transparent);

            device.Textures[0] = gBufferTargets[0].RenderTarget;
            device.SamplerStates[0] = SamplerState.LinearClamp;

            device.Textures[1] = lightMap;
            device.SamplerStates[1] = SamplerState.LinearClamp;

            device.Textures[2] = ssaoTarget;
            device.SamplerStates[2] = SamplerState.LinearClamp;

            Effect fx = shaders[(int)DeferredRendererShaders.Composition];
            fx.Parameters["GBufferTextureSize"].SetValue(gBufferTextureSize);
            if (enableSSAO)
            {
                fx.Parameters["SSAOAmount"].SetValue(ssaoAmount);
                fx.Parameters["LightMapAmount"].SetValue(lightMapAmount);
            }
            else
            {
                fx.Parameters["SSAOAmount"].SetValue(0);
                fx.Parameters["LightMapAmount"].SetValue(1);
            }
            fx.CurrentTechnique.Passes[0].Apply();

            fullscreenQuad.ReadyAndDraw(device);
        }

        public void DrawRenderListMembers(GraphicsDevice device, Effect shader, Matrix view, int playerIndex, RenderLayer layer, DrawObjType objType)
        {
            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(renderList[(int)layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawDeferred(device, shader, view, playerIndex);
                }
            }
        }

        public void DrawRenderListMembersDepthOnly(GraphicsDevice device, Effect shader, RenderLayer layer, DrawObjType objType)
        {
            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(renderList[(int)layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawDeferredDepthOnly(shader, -1);
                }
            }
        }

        public void DrawDepthOnly(GraphicsDevice device, Effect shader)
        {
            DrawRenderListMembersDepthOnly(device, shader, RenderLayer.Layer2, DrawObjType.Mesh);

            if (DrawGround)
            {
                heightmap.DrawDeferredDepthOnly(shader, -1);
            }

            DrawRenderListMembersDepthOnly(device, shader, RenderLayer.Basic, DrawObjType.MeshGenerated);
            DrawRenderListMembersDepthOnly(device, shader, RenderLayer.Basic, DrawObjType.Mesh);
        }

        void MakeGBuffer(GraphicsDevice device, Matrix view, Matrix projection, int playerIndex)
        {
            device.DepthStencilState = DepthStencilState.Default;

            Effect gBuf = shaders[(int)DeferredRendererShaders.GBuffer];
            gBuf.Parameters["View"].SetValue(view);
            gBuf.Parameters["Projection"].SetValue(projection);
            gBuf.Parameters["FloatingPointPrecisionModifier"].SetValue(FLOATING_POINT_PRECISION_MODIFIER);
            gBuf.Parameters["GBufferTextureSize"].SetValue(Engine.Screen.ResolutionVec);
            //gBuf.Parameters["FarClip"].SetValue(ActivePlayerScreens[playerIndex].view.Camera.FarPlane);

            DrawRenderListMembers(device, gBuf, view, playerIndex, RenderLayer.Layer2, DrawObjType.Mesh); // Skybox

            if (DrawGround)
            {
                heightmap.DrawDeferred(device, gBuf, view, playerIndex);
            }
            device.Textures[0] = Engine.LoadContent.Texture(VikingEngine.LootFest.LfLib.BlockTexture);
            DrawRenderListMembers(device, gBuf, view, playerIndex, RenderLayer.Basic, DrawObjType.MeshGenerated);
            DrawRenderListMembers(device, gBuf, view, playerIndex, RenderLayer.Basic, DrawObjType.Mesh);
        }

        void ClearGBuffer(GraphicsDevice device)
        {
            device.DepthStencilState = DepthStencilState.DepthRead;

            device.SetRenderTargets(gBufferTargets);

            shaders[(int)DeferredRendererShaders.Clear].CurrentTechnique.Passes[0].Apply();

            fullscreenQuad.ReadyAndDraw(device);
        }

        void MakeLightMap(GraphicsDevice device, LightManager lightManager, AbsCamera camera, Viewport viewport)
        {
            Vector2 viewportUVPosition = new Vector2(viewport.X, viewport.Y) / gBufferTextureSize;
            Vector2 viewportUVSize = new Vector2(viewport.Width, viewport.Height) / gBufferTextureSize;

            device.BlendState = lightMapBlendState;
            device.DepthStencilState = DepthStencilState.DepthRead;

            device.Textures[0] = gBufferTargets[0].RenderTarget;
            device.SamplerStates[0] = SamplerState.LinearClamp;
            device.Textures[1] = gBufferTargets[1].RenderTarget;
            device.SamplerStates[1] = SamplerState.LinearClamp;
            device.Textures[2] = gBufferTargets[2].RenderTarget;
            device.SamplerStates[2] = SamplerState.PointClamp;
            device.SamplerStates[3] = SamplerState.LinearClamp; // spotlight cookie
            device.SamplerStates[4] = SamplerState.PointClamp; // shadowmap

            Matrix inverseView = Matrix.Invert(camera.ViewMatrix);
            Matrix inverseViewProjection = Matrix.Invert(camera.ViewProjection);

            // Directional lights
            Effect dirLightFx = shaders[(int)DeferredRendererShaders.DirectionalLight];
            dirLightFx.Parameters["InverseView"].SetValue(inverseView);
            dirLightFx.Parameters["InverseViewProjection"].SetValue(inverseViewProjection);
            dirLightFx.Parameters["CameraPosition"].SetValue(camera.Position);
            dirLightFx.Parameters["ViewportUVPosition"].SetValue(viewportUVPosition );
            dirLightFx.Parameters["ViewportUVSize"].SetValue(viewportUVSize);
            dirLightFx.Parameters["GBufferTextureSize"].SetValue(gBufferTextureSize);

            fullscreenQuad.ReadyBuffers(device);
            foreach (Lights.DirectionalLight light in lightManager.DirectionalLights)
            {
                dirLightFx.Parameters["L"].SetValue(light.Direction);
                dirLightFx.Parameters["LightColor"].SetValue(light.Color);
                dirLightFx.Parameters["LightIntensity"].SetValue(light.Intensity);
                dirLightFx.CurrentTechnique.Passes[0].Apply();
                fullscreenQuad.Draw(device);
            }

            // Spot lights
            Effect spotLightFx = shaders[(int)DeferredRendererShaders.SpotLight];
            spotLightFx.Parameters["View"].SetValue(camera.ViewMatrix);
            spotLightFx.Parameters["InverseView"].SetValue(inverseView);
            spotLightFx.Parameters["Projection"].SetValue(camera.Projection);
            spotLightFx.Parameters["InverseViewProjection"].SetValue(inverseViewProjection);
            spotLightFx.Parameters["CameraPosition"].SetValue(camera.Position);
            spotLightFx.Parameters["ViewportUVPosition"].SetValue(viewportUVPosition);
            spotLightFx.Parameters["ViewportUVSize"].SetValue(viewportUVSize);
            spotLightFx.Parameters["GBufferTextureSize"].SetValue(gBufferTextureSize);

            ModelMeshPart spotMeshPart = spotLightGeometry.Meshes[0].MeshParts[0];
            device.SetVertexBuffer(spotMeshPart.VertexBuffer,
                                   spotMeshPart.VertexOffset);
            device.Indices = spotMeshPart.IndexBuffer;

            foreach (Lights.SpotLight light in lightManager.SpotLights)
            {
                device.Textures[3] = light.AttenuationTexture;
                device.Textures[4] = light.ShadowMap;

                float lightAngle = light.CalculateAngleCos();

                spotLightFx.Parameters["World"].SetValue(light.World);
                spotLightFx.Parameters["LightViewProjection"].SetValue(light.View * light.Projection);
                spotLightFx.Parameters["LightPosition"].SetValue(light.Position * FLOATING_POINT_PRECISION_MODIFIER);
                spotLightFx.Parameters["LightColor"].SetValue(light.Color);
                spotLightFx.Parameters["LightIntensity"].SetValue(light.Intensity);
                spotLightFx.Parameters["S"].SetValue(light.Direction);
                spotLightFx.Parameters["LightAngleCos"].SetValue(lightAngle);
                spotLightFx.Parameters["LightHeight"].SetValue(light.FarPlane);
                spotLightFx.Parameters["Shadows"].SetValue(light.CastShadows);
                spotLightFx.Parameters["ShadowMapSize"].SetValue(light.ShadowMapResolution);

                Vector3 L = camera.Position - light.Position;
                float SL = Math.Abs(Vector3.Dot(L, light.Direction));
                if (SL < lightAngle)
                {
                    device.RasterizerState = RasterizerState.CullCounterClockwise;
                }
                else
                {
                    device.RasterizerState = RasterizerState.CullClockwise;
                }

                spotLightFx.CurrentTechnique.Passes[0].Apply();

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 
                    spotMeshPart.StartIndex,
                    spotMeshPart.PrimitiveCount);
            }

            // Point lights
            Effect pointLightFx = shaders[(int)DeferredRendererShaders.PointLight];
            pointLightFx.Parameters["View"].SetValue(camera.ViewMatrix);
            pointLightFx.Parameters["InverseView"].SetValue(inverseView);
            pointLightFx.Parameters["Projection"].SetValue(camera.Projection);
            pointLightFx.Parameters["InverseViewProjection"].SetValue(inverseViewProjection);
            pointLightFx.Parameters["CameraPosition"].SetValue(camera.Position);
            pointLightFx.Parameters["ViewportUVPosition"].SetValue(viewportUVPosition);
            pointLightFx.Parameters["ViewportUVSize"].SetValue(viewportUVSize);
            pointLightFx.Parameters["GBufferTextureSize"].SetValue(gBufferTextureSize);

            ModelMeshPart pointMeshPart = pointLightGeometry.Meshes[0].MeshParts[0];
            device.SetVertexBuffer(pointMeshPart.VertexBuffer, pointMeshPart.VertexOffset);
            device.Indices = pointMeshPart.IndexBuffer;

            foreach (Lights.PointLight light in lightManager.PointLights)
            {
                device.Textures[4] = light.ShadowMap;
                device.SamplerStates[4] = SamplerState.PointWrap;

                pointLightFx.Parameters["World"].SetValue(Matrix.CreateScale(light.Radius) * Matrix.CreateTranslation(light.Position * FLOATING_POINT_PRECISION_MODIFIER));
                pointLightFx.Parameters["LightPosition"].SetValue(light.Position * FLOATING_POINT_PRECISION_MODIFIER);
                pointLightFx.Parameters["LightRadius"].SetValue(light.Radius * FLOATING_POINT_PRECISION_MODIFIER);
                pointLightFx.Parameters["LightColor"].SetValue(light.Color);
                pointLightFx.Parameters["LightIntensity"].SetValue(light.Intensity);
                pointLightFx.Parameters["Shadows"].SetValue(light.CastShadows);
                pointLightFx.Parameters["ShadowMapSize"].SetValue(light.ShadowMapResolution);

                Vector3 diff = camera.Position - light.Position;

                float camToLight = (float)Math.Sqrt((float)Vector3.Dot(diff, diff));

                if (camToLight <= light.Radius)
                    device.RasterizerState = RasterizerState.CullClockwise;
                else if (camToLight > light.Radius)
                    device.RasterizerState = RasterizerState.CullCounterClockwise;

                pointLightFx.CurrentTechnique.Passes[0].Apply();

                //device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                //    pointMeshPart.NumVertices,
                //    pointMeshPart.StartIndex,
                //    pointMeshPart.PrimitiveCount);
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 
                    pointMeshPart.StartIndex,
                    pointMeshPart.PrimitiveCount);
            }

            device.BlendState = BlendState.Opaque;
            device.RasterizerState = RasterizerState.CullCounterClockwise;
            device.DepthStencilState = DepthStencilState.Default;
        }

        void Debug(GraphicsDevice device, SpriteBatch spriteBatch)
        {
            graphicsDeviceManager.GraphicsDevice.SetRenderTargets(null);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
                SamplerState.PointClamp, null, null);

            int width = Engine.Screen.Width / 4;
            int height = Engine.Screen.Height / 4;

            Rectangle rect = new Rectangle(width * 2, 0, width, height);

            spriteBatch.Draw((Texture2D)gBufferTargets[0].RenderTarget, rect, Color.White);
            rect.X += width;
            spriteBatch.Draw((Texture2D)gBufferTargets[1].RenderTarget, rect, Color.White);
            rect.X -= width;
            rect.Y += height;
            spriteBatch.Draw((Texture2D)gBufferTargets[2].RenderTarget, rect, Color.White);
            rect.X += width;
            spriteBatch.Draw((Texture2D)lightManager.SpotLights[0].ShadowMap, rect, Color.White);
            //rect.X -= width;
            rect.Y += height;
            spriteBatch.Draw((Texture2D)lightMap, rect, Color.White);

            spriteBatch.End();
        }
    }
}
