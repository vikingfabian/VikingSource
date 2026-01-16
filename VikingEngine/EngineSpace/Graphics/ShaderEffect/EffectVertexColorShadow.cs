using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using VikingEngine.ToGG.ToggEngine;

namespace VikingEngine.Graphics
{
    class EffectVertexColorShadow : AbsEffect
    {
        public const string ColorArgument = "ColorAndAlpha";

        static protected ModelMesh modelListMesh;
        static int modelMeshIx = 0;
        protected string TechniqueName;
        protected bool usesWorldPos;
        //Texture2D prevTexture = null;

        public RenderTarget2D shadowMapRenderTarget;

        public static Effect depthWriter, shadowEffect;/*, shadowEffect*/
        Matrix lightView, lightProjection;
        public EffectVertexColorShadow(string techniqueName = "Default", bool usesWorldPosition = true)
        {
            this.shader = shadowEffect;//Engine.Draw.effectVertexColorShadow;
            this.TechniqueName = techniqueName;
            this.usesWorldPos = usesWorldPosition;

            shadowMapRenderTarget = new RenderTarget2D(Engine.Draw.graphicsDeviceManager.GraphicsDevice, 2048, 2048, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.PlatformContents);
        }

        public static void LoadContent()
        {
            depthWriter = Engine.LoadContent.LoadShader("DeferredRenderer\\DepthWriter");
            depthWriter.CurrentTechnique = depthWriter.Techniques[0];
            shadowEffect = Engine.LoadContent.LoadShader("VertexColorShadow");
            //shadowEffect = Engine.LoadContent.LoadShader("VoxelShadows");
        }

        public void DrawShadowMap(Engine.PlayerData p, int cameraIndex)
        {
            //GraphicsDevice device
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetRenderTarget(shadowMapRenderTarget);
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            //graphicsDeviceManager.GraphicsDevice.BlendState = BlendState.Opaque;
            Engine.Draw.graphicsDeviceManager.GraphicsDevice.Clear(Color.Transparent);

            Vector3 lightPos = p.view.Camera.LookTarget + new Vector3(4, 5, -4);

            //Matrix lightViewProjection = lightView * lightProjection;
            lightView = Matrix.CreateLookAt(lightPos, p.view.Camera.LookTarget, Vector3.UnitY);
            float orthoSize = 8f;
            float zNear = 2f;
            float zFar = 10f;
            lightProjection = Matrix.CreateOrthographic(orthoSize, orthoSize, zNear, zFar);

            //depthWriter.Parameters["View"].SetValue(lightView);
            //depthWriter.Parameters["Projection"].SetValue(lightProjection);
            //depthWriter.Parameters["LightPosition"].SetValue(lightPos);
            depthWriter.Parameters["ZNear"].SetValue(zNear);
            depthWriter.Parameters["ZFar"].SetValue(zFar);
            depthWriter.Parameters["FloatingPointPrecisionModifier"].SetValue(1f);

        }

        bool debug = false;

        public void BeginDrawShadow()
        {
            if (Input.Keyboard.KeyDownEvent(Microsoft.Xna.Framework.Input.Keys.LeftControl))
            {
                debug = !debug;

                if (debug)
                {
                    TechniqueName = "ShadowDebug";
                }
                else
                {
                    TechniqueName = "Default";
                }
            }

            shadowEffect.Parameters["LightView"].SetValue(lightView);
            shadowEffect.Parameters["LightProjection"].SetValue(lightProjection);

            shadowEffect.Parameters["AmbientColor"]?.SetValue(new Vector3(0.9f));
            shadowEffect.Parameters["ShadowMap"].SetValue(shadowMapRenderTarget);
            shadowEffect.Parameters["ZBias"].SetValue(0.7f);
        }

        public override void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            shadowEffect.Parameters["ShadowMap"].SetValue(shadowMapRenderTarget);
            base.DrawVB(frame, obj, VB);
        }

        public override void Draw(Mesh obj)
        {
            shader.CurrentTechnique = shader.Techniques[TechniqueName];

            //obj.TextureSource.SetCustomShaderParameters(ref shader);

            //if (prevTexture != obj.texture)
            //{
            //    shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(obj.texture);
            //    prevTexture = obj.texture;
            //}

            shader.Parameters[ColorArgument].SetValue(obj.colorAndAlpha);

            var model = Engine.LoadContent.Models[(int)obj.LoadedMeshType];
            modelListMesh = model.Meshes[0];
            obj.CalcWorldMatrix(modelListMesh);

            modelListMesh.MeshParts[0].Effect = shader;
            modelListMesh.Draw();
        }

        protected override void SetVertexBufferEffect(AbsVoxelObj obj)
        {
            base.shader.CurrentTechnique = base.shader.Techniques[TechniqueName];

            //base.shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(Engine.LoadContent.Texture(obj.texture));
            //base.shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
            //base.shader.Parameters["SourceSize"].SetValue(Vector2.One);

            Matrix world = Matrix.CreateScale(obj.scale)
                          * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation)
                          * Matrix.CreateTranslation(obj.position);
            Ref.draw.worldMatrix = world;

            //if (usesWorldPos)
            //{
                shader.Parameters["World"].SetValue(world);
                shader.Parameters["View"].SetValue(Ref.draw.Camera.ViewMatrix);
                shader.Parameters["Projection"].SetValue(Ref.draw.Camera.Projection);
                shader.Parameters["LightView"].SetValue(lightView);
                shader.Parameters["LightProjection"].SetValue(lightProjection);
                //shader.Parameters["ShadowMap"].SetValue(shadowMapRenderTarget);
                //shader.Parameters["ZBias"].SetValue(0.001f);
            //}

            //Matrix wvp = world * Ref.draw.Camera.ViewProjection;
            //shader.Parameters["wvp"].SetValue(wvp);
            //shader.Parameters[ColorArgument].SetValue(obj.colorAndAlpha);
        }
    }
}
