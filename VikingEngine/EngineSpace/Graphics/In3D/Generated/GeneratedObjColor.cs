using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.Graphics
{
    class GeneratedObjColor: Abs3DModel
    {
        /* Static fields */
        public static Microsoft.Xna.Framework.Graphics.BasicEffect effectGround;
        
        /* Static methods */
        public static void ChangeFog(bool start, float add)
        {
            if (start)
            {
                effectGround.FogStart = Bound.Set(effectGround.FogStart + add, 0, effectGround.FogEnd);
            }
            else
            {
                effectGround.FogEnd = Bound.Set(effectGround.FogEnd + add, effectGround.FogStart, 10000);
            }
        }
        public static void UpdateFog(float camZoom)
        {
            effectGround.FogStart = camZoom;
            effectGround.FogEnd = camZoom * 1.3f;
        }

        /* Properties */
        public override DrawObjType DrawType { get { return DrawObjType.MeshGenerated; } }
        //public override float GetPositionX { get { return 0; } }
        //public override float GetPositionZ { get { return 0; } }
        public override float Opacity
        {
            get { return opacity; }
            set { this.opacity = value; ; }
        }
        public override Color Color
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
        float opacity = 1f;

        protected override bool drawable { get { return true; } }

        /* Fields */
        private VertexAndIndexBuffer vertexAndIndexBuffers;
        private LoadedTexture spriteSheet;
        public Vector3 position = Vector3.Zero;

        /* Constructors */
        public GeneratedObjColor(PolygonsAndTrianglesColor polygonsAndTriangles, LoadedTexture spriteSheet, bool addToRender)
            : base(addToRender)
        {
            this.spriteSheet = spriteSheet;
            visible = false;
            effectGround = new Microsoft.Xna.Framework.Graphics.BasicEffect(Engine.Draw.graphicsDeviceManager.GraphicsDevice);
            effectGround.VertexColorEnabled = true;
            effectGround.AmbientLightColor = Vector3.One;
            effectGround.TextureEnabled = true;
           // effectGround.Texture = Engine.LoadContent.Texture(LoadedTexture.NO_TEXTURE);
            effectGround.Texture = Engine.LoadContent.Texture(spriteSheet);
            effectGround.FogEnabled = false;
            effectGround.LightingEnabled = false;
           

            if (polygonsAndTriangles.NumPolygons == 0)
                return;
            vertexAndIndexBuffers = (VertexAndIndexBuffer)PolygonLib.BuildVBFromPolygons(polygonsAndTriangles);
            
            visible = true;
        }

        /* Family methods */
        public override void Draw(int cameraIndex)
        {
            if (visible)
            {
                effectGround.World = Matrix.CreateTranslation(position);
                effectGround.Projection = Ref.draw.Camera.Projection;
                effectGround.View = Ref.draw.Camera.ViewMatrix;
                vertexAndIndexBuffers.SetBuffer();
                effectGround.Alpha = opacity;

                for (int pass = 0; pass < effectGround.CurrentTechnique.Passes.Count; pass++)
                {
                    effectGround.CurrentTechnique.Passes[pass].Apply();
                    vertexAndIndexBuffers.Draw();
                }
            }
        }
        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
        {
            device.Textures[0] = Engine.LoadContent.Texture(spriteSheet);
            vertexAndIndexBuffers.SetBuffer();
            shader.CurrentTechnique.Passes[0].Apply();
            vertexAndIndexBuffers.Draw();
        }
        public override void DrawDeferredDepthOnly(Effect shader, int cameraIndex)
        {
            vertexAndIndexBuffers.SetBuffer();
            shader.CurrentTechnique.Passes[0].Apply();
            vertexAndIndexBuffers.Draw();
        }
        public override AbsDraw CloneMe()
        {
            throw new NotImplementedException();
        }
        public override void copyAllDataFrom(AbsDraw master)
        {
            throw new NotImplementedException();
        }
        public override void updateBoundingSphere(ref BoundingSphere boundingSphere)
        { }
    }
}
