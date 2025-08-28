using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.DSSWars.Map;
using VikingEngine.EngineSpace.Graphics.DrawProcess;

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

        public void Draw()
        {
            Effect.DrawVB(Frame, this, VB);
        }
        public override void DrawShadow(int cameraIndex, AbsEffect shader)
        {
            if (Effect == MapLayer_Detail.ModelEffect)
            {
                Draw();
            }
            else
            {
                shader.DrawVB(Frame, this, VB);
            }
        }
       

        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            VB.SetBuffer();
            Matrix world = Matrix.CreateScale(scale) *
                           Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
                           Matrix.CreateTranslation(position);
            shader.Parameters["World"].SetValue(world);
            shader.Parameters["WorldViewIT"].SetValueTranspose(Matrix.Invert(world * view));
            shader.CurrentTechnique.Passes[0].Apply();
            VB.Draw(Frame);
        }
        public override void DrawDeferredDepthOnly(Effect shader, LightProjection light, int cameraIndex)
        {
            //// Sun direction: straight down (from +Y toward -Y)
            //Vector3 lightDirection = new Vector3(-0.1f, -1, -0.1f);

            //// Light "position" isn’t important for directional light, 
            //// but we place the camera above the scene looking down:
            //Vector3 lightPos = new Vector3(0, 100, 0); // 100 units up
            //Vector3 target = Vector3.Zero;           // look at scene center
            //Vector3 up = Vector3.Up;        // choose a stable up (Y is up)

            //// View matrix: light’s "camera"
            //Matrix LightViewMatrix = Matrix.CreateLookAt(lightPos, target, up);

            //// Projection matrix: orthographic (parallel rays, no perspective)
            //float sceneWidth = 200;
            //float sceneHeight = 200;
            //float nearPlane = 1f;
            //float farPlane = 500f;

            //Matrix LightProjectionMatrix = Matrix.CreateOrthographic(sceneWidth, sceneHeight, nearPlane, farPlane);


            VB.SetBuffer();
            //shader.Parameters["World"].SetValue(Matrix.CreateScale(scale) *
            //    Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
            //    Matrix.CreateTranslation(position));

            // Build your object's world matrix (use your own transform data).
            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateFromQuaternion(Rotation.QuadRotation) *
                Matrix.CreateTranslation(position);

            // Compute model->light (same as example, just no bones/meshes loop).
            // Replace LightViewMatrix/LightProjectionMatrix with your actual values,
            // e.g. _lightViewMatrix / _lightProjectionMatrix if those live on your effect/base.
            

            // Send to the shader and draw.
            shader.Parameters["ModelToLight"].SetValue(light.modelToLight(world));
            shader.CurrentTechnique.Passes[0].Apply();
            VB.Draw(Frame);
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
