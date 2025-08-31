using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//xna
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sentry.Protocol;
using System;
using System.Collections.Generic;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.LootFest.Map;

namespace VikingEngine.Graphics
{

    class Mesh : Point3D
    {
        const string WVP = "wvp";

        
        protected override bool drawable { get { return true; } }
        
        public TextureEffectType EffectType
        {
            get { return effectType; }
            set
            {
                if (value >= TextureEffectType.NUM_NON)
                    effectType = TextureEffectType.Flat;
                else
                    effectType = value;
            }
        }

        /* Fields */
        public Sprite TextureSource;
        public Texture2D texture;
        public LoadedMesh LoadedMeshType = LoadedMesh.plane;
        public TextureEffectType effectType = TextureEffectType.Flat;
        
        /* Constructors */
        public Mesh()
            : base()
        {
            
        }
        public Mesh(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName sprite, Color col, 
             bool addToRender = true)
            : base(pos, scale, addToRender)
        {
            LoadedMeshType = mesh;
            colorAndAlpha = col.ToVector4();
            EffectType = effectType;
            SetSpriteName(sprite);
        }

        public Mesh(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            ModelTextureSettings textureSettings,
             bool addToRender = true)
            : base(pos, scale, addToRender)
        {
            LoadedMeshType = mesh;
            colorAndAlpha = textureSettings.ColorAndAlpha;
            EffectType = textureSettings.effectType;
            TextureSource = textureSettings.TextureSource;
            if (textureSettings.texture == null)
            {
                texture = TextureSource.Texture();
            }
            else
            {
                texture = textureSettings.texture;
            }
        }

        /* Family methods */
        public override string ToString()
        {
            return "Mesh:" + LoadedMeshType.ToString() + " txt:" + TextureSource.ToString();
        }

        public override void Draw(int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                Engine.Draw.TextureEffects[(int)EffectType].Draw(this);
            }
        }

        //public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        //{
        //    if (VisibleInCamera(cameraIndex))
        //    {
        //        shader.Parameters["SourcePos"].SetValue(TextureSource.SourceF.Position);
        //        shader.Parameters["SourceSize"].SetValue(TextureSource.SourceF.Size);
        //        shader.Parameters["Texture"].SetValue(texture);

        //        Model model = Engine.LoadContent.Mesh(LoadedMeshType);

        //        Matrix[] transforms = new Matrix[model.Bones.Count];
        //        model.CopyAbsoluteBoneTransformsTo(transforms);

        //        Matrix modelWorld = Matrix.CreateScale(Scale) *
        //                    Matrix.CreateFromQuaternion(QuatRotation) *
        //                    Matrix.CreateTranslation(Position);

        //        foreach (ModelMesh modelMesh in model.Meshes)
        //        {
        //            foreach (ModelMeshPart part in modelMesh.MeshParts)
        //            {
        //                // Set buffers
        //                device.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
        //                device.Indices = part.IndexBuffer;

        //                // Set matrices
        //                Matrix world = transforms[modelMesh.ParentBone.Index] * modelWorld;

        //                // Set matrices and textures
        //                shader.Parameters["World"].SetValue(world);
        //                shader.Parameters["WorldViewIT"].SetValueTranspose(Matrix.Invert(world * view));

        //                // Apply pass
        //                shader.CurrentTechnique.Passes[0].Apply();

        //                // Draw mesh
        //                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
        //            }
        //        }
        //    }
        //}
        public override void DrawDepthOnly(bool drawDepth, Effect shader, LightProjection light, int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                Model model = Engine.LoadContent.Mesh(LoadedMeshType);

                Matrix[] transforms = new Matrix[model.Bones.Count];
                model.CopyAbsoluteBoneTransformsTo(transforms);

                Matrix modelWorld = Matrix.CreateScale(Scale) *
                            Matrix.CreateFromQuaternion(QuatRotation) *
                            Matrix.CreateTranslation(Position);

                foreach (ModelMesh modelMesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in modelMesh.MeshParts)
                    {
                        // Set buffers
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = part.IndexBuffer;

                        // Set matrices
                        Matrix world = transforms[modelMesh.ParentBone.Index] * modelWorld;

                        // Set matrices and textures
                        //shader.Parameters["World"].SetValue(world);
                        shader.Parameters["ModelToLight"].SetValue(light.modelToLight(world));
                        // Apply pass
                        shader.CurrentTechnique.Passes[0].Apply();

                        // Draw mesh
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public override void DrawWithShadow(int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            if (VisibleInCamera(cameraIndex))
            {
                Model model = Engine.LoadContent.Mesh(LoadedMeshType);

                //Matrix[] transforms = new Matrix[model.Bones.Count];
                //model.CopyAbsoluteBoneTransformsTo(transforms);

                Matrix modelWorld = Matrix.CreateScale(Scale) *
                        Matrix.CreateFromQuaternion(QuatRotation) *
                        Matrix.CreateTranslation(Position);
                shader.Parameters["Texture"]?.SetValue(texture);
                //shader.Parameters["Color"]?.SetValue(Color.ToVector4());


                foreach (ModelMesh modelMesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in modelMesh.MeshParts)
                    {
                        // Set matrices
                        Matrix world = /*transforms[modelMesh.ParentBone.Index] **/ modelWorld;

                        Matrix worldViewMatrix = world * camera.ViewMatrix;
                        Matrix worldViewProjMatrix = world * camera.ViewProjection;
                        Matrix lightWorldViewProjMatrix = world * light.ViewProjection;

                        Matrix temp = worldViewMatrix;
                        temp.Translation = Vector3.Zero;
                        Matrix worldViewIT = Matrix.Transpose(Matrix.Invert(temp));

                        shader.Parameters["NormalToView"]?.SetValue(worldViewIT);
                        shader.Parameters["ModelToScreen"]?.SetValue(worldViewProjMatrix);
                        shader.Parameters["ModelToLight"]?.SetValue(lightWorldViewProjMatrix);
                        shader.Parameters["ModelToView"]?.SetValue(worldViewMatrix);


                        // Set buffers
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = part.IndexBuffer;

                        // Apply pass
                        shader.CurrentTechnique.Passes[0].Apply();

                        // Draw mesh
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public void DrawOcean(int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            if (VisibleInCamera(cameraIndex))
            {
                Model model = Engine.LoadContent.Mesh(LoadedMeshType);

                //Matrix[] transforms = new Matrix[model.Bones.Count];
                //model.CopyAbsoluteBoneTransformsTo(transforms);

                //Matrix modelWorld = Matrix.CreateScale(Scale) *
                //        Matrix.CreateFromQuaternion(QuatRotation) *
                //        Matrix.CreateTranslation(Position);
                //shader.Parameters["Texture"]?.SetValue(texture);
                //shader.Parameters["Color"]?.SetValue(Color.ToVector4());


                foreach (ModelMesh modelMesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in modelMesh.MeshParts)
                    {
                        // Set matrices
                        //Matrix world = /*transforms[modelMesh.ParentBone.Index] **/ modelWorld;

                        //Matrix worldViewMatrix = world * camera.ViewMatrix;
                        //Matrix worldViewProjMatrix = world * camera.ViewProjection;
                        ////Matrix lightWorldViewProjMatrix = world * light.ViewProjection;

                        ////Matrix temp = worldViewMatrix;
                        ////temp.Translation = Vector3.Zero;
                        ////Matrix worldViewIT = Matrix.Transpose(Matrix.Invert(temp));

                        //shader.Parameters["ModelToScreen"]?.SetValue(worldViewProjMatrix);
                        //shader.Parameters["ModelToView"]?.SetValue(worldViewMatrix);
                        shader.Parameters[Graphics.TextureSourceLib.ColorMap].SetValue(texture);
                        TextureSource.SetCustomShaderParameters(ref shader);
                        //shader.Parameters["SourcePos"].SetValue(Vector2.Zero);
                        //shader.Parameters["SourceSize"].SetValue(Vector2.One);

                        Ref.draw.worldMatrix = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(Rotation.QuadRotation) * Matrix.CreateTranslation(position);//Matrix.CreateTranslation(obj.Position);

                        //const string CameraPositionSetting = "CameraPosition";
                        //shader.Parameters[CameraPositionSetting].SetValue(Ref.draw.Camera.Position);
                        //shader.Parameters["world"].SetValue(Ref.draw.worldMatrix);
                        Matrix lightWorldViewProjMatrix = Ref.draw.worldMatrix * light.ViewProjection;
                        shader.Parameters["ModelToLight"]?.SetValue(lightWorldViewProjMatrix);
                        shader.Parameters["wvp"].SetValue(Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection);
                        shader.Parameters[CustomEffect.ColorArgument]?.SetValue(colorAndAlpha);

                        // Set buffers
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = part.IndexBuffer;

                        // Apply pass
                        shader.CurrentTechnique.Passes[0].Apply();

                        // Draw mesh
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        {
            boundingSphere.Center = Position;
            boundingSphere.Radius = Scale.X;
        }

        public override AbsDraw CloneMe()
        {
            Mesh m = new Mesh();
            copyAllDataFrom(m);
            return m;
        }

        public virtual void CalcWorldMatrix(ModelMesh mesh)
        {
            Ref.draw.worldMatrix = Matrix.CreateScale(Scale) *
                    Matrix.CreateFromQuaternion(QuatRotation) *
                    Matrix.CreateTranslation(Position);
            Ref.draw.wvpMatrix = Ref.draw.worldMatrix * Ref.draw.Camera.ViewProjection;//Ref.draw.vp;

            //Engine.Draw.effectBR.Parameters[World].SetValue(Ref.draw.worldMatrix);
            Engine.Draw.effectBR.Parameters[WVP].SetValue(Ref.draw.wvpMatrix);
        }

        public override void copyAllDataFrom(AbsDraw master)
        {
            base.copyAllDataFrom(master);//pos, sz, rot
            Mesh m = (Mesh)master;
            m.LoadedMeshType = LoadedMeshType;
            m.TextureSource = TextureSource;
            m.EffectType = EffectType;
            m.colorAndAlpha = colorAndAlpha;
        }

        /* Novelty methods */
        public void InitMe(LoadedMesh mesh, Vector3 pos, float basicScale,
             LoadedTexture image) 
        {
            LoadedMeshType = mesh;
            this.Position = pos;
            scale.X = basicScale;
            scale.Y = basicScale;
            scale.Z = basicScale;            
        }

        override public void SetSpriteName(SpriteName sprite)
        {
#if DEBUG
            if (string.IsNullOrEmpty(DebugName))
            {
                DebugName = sprite.ToString();
            }
#endif
            TextureSource = DataLib.SpriteCollection.Get(sprite);
            texture = TextureSource.Texture();
        }

        public void setFullTextureSource(Texture2D tex)
        {
            this.texture = tex;
            TextureSource = new Sprite(tex);
        }

        public void repeatingTextureSource(Texture2D tex, IntVector2 repeat)
        {
            this.texture = tex;
            TextureSource = new Sprite(tex);
            TextureSource.Source.Width *= repeat.X;
            TextureSource.Source.Height *= repeat.Y;

            TextureSource.UpdateSourceF(false, tex);
        }

        public Vector3 childPosition(Vector3 childOffset)
        {
            return Rotation.TranslateAlongAxis(childOffset, Position);
        }

        public ModelTextureSettings TextureSettings
        {
            get
            {
                ModelTextureSettings value = new ModelTextureSettings();
                value.ColorAndAlpha = this.colorAndAlpha;
                value.effectType = this.effectType;
                value.texture = this.texture;
                value.TextureSource = this.TextureSource;

                return value;
            }
            set
            {
                this.colorAndAlpha = value.ColorAndAlpha;
                this.effectType = value.effectType;
                this.texture = value.texture;
                this.TextureSource = value.TextureSource;
            }
        }
    }
}
