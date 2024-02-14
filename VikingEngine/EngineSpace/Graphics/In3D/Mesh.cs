using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//xna
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VikingEngine.Graphics
{

    class Mesh : Point3D
    {
        /* Constants */
        //const string World = "world";
        const string WVP = "wvp";

        /* Properties */
        //public override ObjType Type { get { return ObjType.Mesh; } }
        
        
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
            //TextureSource = DataLib.SpriteCollection.Get(SpriteName.NO_IMAGE);//new TextureSource(SpriteName.NO_IMAGE);
        }
        //public Mesh(LoadedMesh mesh, Vector3 pos, TextureEffect textureEffect, float basicScale)
        //    : this(mesh, pos, textureEffect, VectorExt.V3(basicScale), Vector3.Zero)
        //{ }
        //public Mesh(LoadedMesh mesh, Vector3 pos, TextureEffect textureEffect, float basicScale, bool addToRender)
        //    : this(mesh, pos, textureEffect, VectorExt.V3(basicScale), Vector3.Zero, addToRender)
        //{ }
        //public Mesh(LoadedMesh mesh, Vector3 pos, TextureEffect textureEffect, Vector3 scale, Vector3 rotation)
        //    : this(mesh, pos, textureEffect,  SpriteName.WhiteArea, Color.White, scale, rotation, true)
        //{ }

        public Mesh(LoadedMesh mesh, Vector3 pos, Vector3 scale,
            TextureEffectType effectType, SpriteName sprite, Color col, 
             bool addToRender = true)
            : base(pos, scale, addToRender)
        {
            //if (mesh == LoadedMesh.cube_repeating)
            //{
            //    lib.DoNothing();
            //}
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

        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                shader.Parameters["SourcePos"].SetValue(TextureSource.SourceF.Position);
                shader.Parameters["SourceSize"].SetValue(TextureSource.SourceF.Size);
                shader.Parameters["Texture"].SetValue(texture);

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
                        device.SetVertexBuffer(part.VertexBuffer, part.VertexOffset);
                        device.Indices = part.IndexBuffer;

                        // Set matrices
                        Matrix world = transforms[modelMesh.ParentBone.Index] * modelWorld;

                        // Set matrices and textures
                        shader.Parameters["World"].SetValue(world);
                        shader.Parameters["WorldViewIT"].SetValueTranspose(Matrix.Invert(world * view));

                        // Apply pass
                        shader.CurrentTechnique.Passes[0].Apply();

                        // Draw mesh
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
                    }
                }
            }
        }

        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
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
                        shader.Parameters["World"].SetValue(world);

                        // Apply pass
                        shader.CurrentTechnique.Passes[0].Apply();

                        // Draw mesh
                        Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, part.NumVertices, part.StartIndex, part.PrimitiveCount);
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
