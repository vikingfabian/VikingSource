using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Map;
using VikingEngine.Engine;
using VikingEngine.EngineSpace.Graphics.DrawProcess;
using VikingEngine.LootFest.Map;
using VikingEngine.ToGG.HeroQuest.Data.Condition;

namespace VikingEngine.Graphics
{
    class VoxelModel : AbsVoxelObj
    {
        public override int NumFrames { get { return VB.NumFrames; } }

        public override float SizeToScale { get { return sizeToScale; } }
        public override int GridSideLength { get { return gridSideLength; } }

        /* Fields */
        public IntVector3 GridSize;
        private float sizeToScale = 1f;
        public int gridSideLength;
        
        public AbsEffect Effect;
        
        private VertexAndIndexBufferAnimated VB;
        //public bool visualProcessStarted = false;

        /* Constructors */
        public VoxelModel(bool addToRender)
            : base(addToRender)
        {
            Effect = EffectBasicVertexColor.GetSingletonSafe();
            //Effect = FlagWaveEffect.GetSingletonSafe();
        }

        /* Family methods */
        public override void Draw(int cameraIndex)
        {
            if (VisibleInCamera(cameraIndex))
            {
                Draw();
            }
        }

        //public void Draw(int cameraIndex, Texture2D texture)
        //{ 
            
        //}

        public void Draw()
        {
            Effect.DrawVB(Frame, this, VB);
        }
        //public override void DrawShadow(int cameraIndex, AbsEffect shader)
        //{
        //    if (Effect == MapLayer_Detail.ModelEffect)
        //    {
        //        Draw();
        //    }
        //    else
        //    {
        //        shader.DrawVB(Frame, this, VB);
        //    }
        //}
       

        //public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        //{
        //    VB.SetBuffer();
        //    Matrix world = Matrix.CreateScale(scale) *
        //                   Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
        //                   Matrix.CreateTranslation(position);
        //    shader.Parameters["World"].SetValue(world);
        //    shader.Parameters["WorldViewIT"].SetValueTranspose(Matrix.Invert(world * view));
        //    shader.CurrentTechnique.Passes[0].Apply();
        //    VB.Draw(Frame);
        //}
        public override void DrawDepthOnly(bool drawDepth, Effect shader, LightProjection light, int cameraIndex)
        {
            if (drawDepth)
            {
                if (VB != null)
                {
                    VB.SetBuffer();

                    Matrix world =
                        Matrix.CreateScale(scale) *
                        Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
                        Matrix.CreateTranslation(position);

                    // Send to the shader and draw.
                    shader.Parameters["ModelToLight"].SetValue(light.modelToLight(world));
                    shader.CurrentTechnique.Passes[0].Apply();
                    VB.Draw(Frame);
                }
            }
            else
            {
                Effect.DrawVB(Frame, this, VB);
            }
        }

        public override void DrawWithShadow(int cameraIndex, AbsCamera camera, Effect shader, LightProjection light)
        {
            if (customShader)
            {
                Effect.DrawVB(Frame, this, VB);
            }
            else if (VB != null)
            {
                VB.SetBuffer();
                Matrix modelWorld = Matrix.CreateScale(scale) *
                            Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
                            Matrix.CreateTranslation(position);
                //shader.Parameters["Texture"]?.SetValue(LoadContent.Texture(LoadedTexture.WhiteArea));
                shader.Parameters["Color"]?.SetValue(Color.ToVector4());

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

                shader.CurrentTechnique.Passes[0].Apply();
                VB.Draw(Frame);
            }
        }

        /* Novelty methods */
        public void BuildFromPolygons(PolygonsAndTrianglesColor polygonsAndTriangles, List<int> numPolysPerFrame, LoadedTexture spriteSheet)
        {
            IVerticeData verticeData = PolygonLib.BuildVDFromPolygons(polygonsAndTriangles);
            BuildFromVerticeData(verticeData, numPolysPerFrame, spriteSheet);
        }

        public void BuildFromPolygons(PolygonsAndTrianglesNormal polygonsAndTriangles, List<int> numPolysPerFrame, LoadedTexture spriteSheet)
        {
            //throw new NotImplementedException(); //Måste lägga till polygon normal 

            IVerticeData verticeData = PolygonLib.BuildVDFromPolygons(polygonsAndTriangles);
            BuildFromVerticeData(verticeData, numPolysPerFrame, spriteSheet);
        }

        

        public void BuildFromVerticeData(IVerticeData verticeData, List<int> numPolysPerFrame, LoadedTexture spriteSheet)
        {
            visible = false;
            texture = spriteSheet;
            VB = new VertexAndIndexBufferAnimated(verticeData, numPolysPerFrame);
            visible = true;
        }

        public void BuildFromVerticeData(IVerticeData verticeData, List<Frame> numPolysPerFrame, LoadedTexture spriteSheet)
        {
            visible = false;
            texture = spriteSheet;
            VB = new VertexAndIndexBufferAnimated(verticeData, numPolysPerFrame);
            visible = true;
        }

        public override void NextAnimationFrame()
        {
            if (++Frame >= NumFrames)
            { Frame = 0; }
        }

        
        public void SetOneScale(IntVector3 gridSz)
        {
            GridSize = gridSz;
            gridSideLength = GridSize.LargestSideLength();
            sizeToScale = 1f / gridSideLength;
        }

        public void SetBlockSize(float sizeOfABlock)
        {
            this.scale = new Vector3(sizeOfABlock * gridSideLength);
        }

        public override AbsDraw CloneMe() { throw new NotImplementedException(); }
        public override void copyAllDataFrom(AbsDraw master) { throw new NotImplementedException(); }

        public override VoxelModel GetMaster()
        {
            return this;
        }

        public override void SetSpriteName(SpriteName name)
        {
            throw new NotImplementedException();
        }
    }
}
