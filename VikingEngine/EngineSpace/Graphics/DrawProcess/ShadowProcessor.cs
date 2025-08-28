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
        private SpriteBatch _spriteBatch;

        // Light properties
        public Vector3 LightDirection { get; set; }
        public Vector3 LightPosition
        {
            get
            {
                return TargetPosition + (LightDirection * 800.0f);
            }
        }

        public float SpecularIntensity { get; set; }
        public float Shininess { get; set; }
        public float SunIntensity { get; set; }
        public Vector3 SunColor { get; set; }

        public Vector3 TargetPosition { get; set; }
        public Vector3 UpVector { get; set; } = Vector3.Up;

        // Matrices
        private Matrix _lightViewMatrix;
        private Matrix _lightProjectionMatrix;

        // Shadow map resolution
        private int _shadowMapSize = 2048;

        LightProjection light;
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
            SpottedArrayCounter<AbsDraw> counter = new SpottedArrayCounter<AbsDraw>(Ref.draw.renderList[layer].GetList(objType));
            while (counter.Next())
            {
                Abs3DModel model = counter.sel as Abs3DModel;
                if (model != null)
                {
                    model.DrawDeferredDepthOnly(shader, light, cameraIndex);
                }
            }
        }

        public void DrawDebug()
        {
            VectorRect area = new VectorRect(Engine.Screen.CenterScreen, Engine.Screen.Area.Size * VectorExt.V2Half);
            Ref.draw.DebugDrawRenderTarget(_shadowMap, area.Rectangle);
        }

        //public void EndShadowMapPass()
        //{
        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
        //}

        //public void DrawEntityToShadowMap(Mesh obj)
        //{
        //    //var model = Engine.LoadContent.Models[(int)entity.LoadedMeshType];
        //    //if (model == null || !entity.Visible)
        //    //    return;

        //    //// Set the state needed to draw to the shadow map.
        //    //_graphicsDevice.BlendState = BlendState.Opaque;
        //    //_graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        //    //_graphicsDevice.DepthStencilState = DepthStencilState.Default;

        //    //var world = entity.WorldMatrix;

        //    //var modelToLight = _shadowEffect.Parameters["ModelToLight"];
        //    //var passes = _shadowEffect.CurrentTechnique.Passes;

        //    //Matrix[] transforms = new Matrix[model.Bones.Count];
        //    //model.CopyAbsoluteBoneTransformsTo(transforms);
        //    //foreach (ModelMesh mesh in model.Meshes)
        //    //{
        //    //    var meshWorld = transforms[mesh.ParentBone.Index] * entity.MeshTransforms[mesh.ParentBone.Index] * world;

        //    //    modelToLight.SetValue(meshWorld * _lightViewMatrix * _lightProjectionMatrix);

        //    //    foreach (ModelMeshPart part in mesh.MeshParts)
        //    //    {
        //    //        _graphicsDevice.SetVertexBuffer(part.VertexBuffer);
        //    //        _graphicsDevice.Indices = part.IndexBuffer;

        //    //        foreach (EffectPass pass in passes)
        //    //        {
        //    //            pass.Apply();
        //    //            _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
        //    //        }
        //    //    }
        //    //}


        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        //    Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

        //    var model = Engine.LoadContent.Models[(int)obj.LoadedMeshType]; //Engine.LoadContent.Mesh(obj.LoadedMeshType);

        //    var modelListMesh = model.Meshes[0];
        //    obj.CalcWorldMatrix(modelListMesh);
        //    //var world = entity.WorldMatrix;

        //    var modelToLight = shader.Parameters["ModelToLight"];
        //    var passes = shader.CurrentTechnique.Passes;

        //    //for (modelMeshIx = 0; modelMeshIx < model.Meshes.Count; modelMeshIx++)
        //    //{


        //    //for (int meshPartIx = 0; meshPartIx < modelListMesh.MeshParts.Count; meshPartIx++)
        //    //{ 
        //    modelListMesh.MeshParts[0].Effect = shader;
        //    //}
        //    modelListMesh.Draw();
        //    //}
        //}

        //public void DrawModelWithShadow(Entity entity, Camera camera, bool blendPass)
        //{
        //    Model model = entity.Model;
        //    if (model == null || !entity.Visible)
        //        return;

        //    var color = Color.White;
        //    if (entity is not Player)
        //    {
        //        var FadeNear = 200.0f;
        //        var FadeFar = 300.0f;
        //        float d = Vector3.Distance(camera.Position, entity.Position);
        //        float alpha = 1.0f - MathHelper.Clamp((d - FadeFar) / (FadeNear - FadeFar), 0.0f, 1.0f);
        //        color.A = (byte)Math.Ceiling(255 * alpha);
        //    }

        //    if (color.A == 255)
        //    {
        //        if (blendPass)
        //            return;

        //        _graphicsDevice.BlendState = BlendState.Opaque;
        //        _graphicsDevice.DepthStencilState = DepthStencilState.Default;
        //    }
        //    else
        //    {
        //        if (!blendPass)
        //            return;

        //        _graphicsDevice.BlendState = BlendState.NonPremultiplied;
        //        _graphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
        //    }
        //    _graphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
        //    _graphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;

        //    var view = camera.ViewMatrix;
        //    var world = entity.WorldMatrix;

        //    var lp = Vector3.Normalize(Vector3.TransformNormal(LightPosition, view));

        //    Effect effect = shader;
        //    effect.CurrentTechnique = effect.Techniques["RenderTextured"];
        //    effect.Parameters["LightPosition"]?.SetValue(lp);
        //    effect.Parameters["LightColor"]?.SetValue(SunColor * SunIntensity);
        //    effect.Parameters["AmbientIntensity"]?.SetValue(0.8f);
        //    effect.Parameters["Color"]?.SetValue(color.ToVector4());
        //    effect.Parameters["SpecularIntensity"]?.SetValue(entity.SpecularIntensity);
        //    effect.Parameters["Shininess"]?.SetValue(entity.Shininess);
        //    effect.Parameters["ShadowMap"]?.SetValue(_shadowMap);
        //    effect.Parameters["EdgeFadeScale"]?.SetValue(10.0f);
        //    effect.Parameters["ShadowMap"]?.SetValue(_shadowMap);

        //    Matrix[] transforms = new Matrix[model.Bones.Count];
        //    model.CopyAbsoluteBoneTransformsTo(transforms);
        //    foreach (ModelMesh mesh in model.Meshes)
        //    {
        //        // Calculate the world matrix for the mesh
        //        Matrix meshWorld = transforms[mesh.ParentBone.Index] * entity.MeshTransforms[mesh.ParentBone.Index] * world;

        //        // Calculate all the necessary matrices
        //        Matrix worldViewMatrix = meshWorld * view;
        //        Matrix worldViewProjMatrix = meshWorld * view * camera.ProjectionMatrix;
        //        Matrix lightWorldViewProjMatrix = meshWorld * _lightViewMatrix * _lightProjectionMatrix;

        //        // Calculate normal matrix (inverse transpose of the world-view matrix)
        //        Matrix temp = worldViewMatrix;
        //        temp.Translation = Vector3.Zero;
        //        Matrix worldViewIT = Matrix.Transpose(Matrix.Invert(temp));

        //        effect.Parameters["NormalToView"]?.SetValue(worldViewIT);
        //        effect.Parameters["ModelToScreen"]?.SetValue(worldViewProjMatrix);
        //        effect.Parameters["ModelToLight"]?.SetValue(lightWorldViewProjMatrix);
        //        effect.Parameters["ModelToView"]?.SetValue(worldViewMatrix);

        //        foreach (ModelMeshPart part in mesh.MeshParts)
        //        {
        //            // Get the texture from the original effect.
        //            BasicEffect originalEffect = part.Effect as BasicEffect;
        //            effect.Parameters["Texture"]?.SetValue(originalEffect.Texture);

        //            _graphicsDevice.SetVertexBuffer(part.VertexBuffer);
        //            _graphicsDevice.Indices = part.IndexBuffer;

        //            foreach (EffectPass pass in shader.CurrentTechnique.Passes)
        //            {
        //                pass.Apply();
        //                _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
        //            }
        //        }
        //    }
    }
    
}
