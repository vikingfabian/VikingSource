//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace VikingEngine.Graphics
//{
//    /// <summary>
//    /// Temporary master model while the real model is loading
//    /// </summary>
//    class TempVoxelObj : VoxelModel
//    {
//        /* Static */
//        private static Mesh model = null;

//        /* Properties */
//        public override int Frame { set { } }
//        public override int NumFrames { get { return 1; } }
//        public override bool Animated { get { return false; } }

//        /* Fields */
//        private Color color;
//        private Vector3 tempScale;

//        /* Constructors */
//        public TempVoxelObj(Color color, Vector3 scale)
//            : base(false)
//        {
//            this.tempScale = scale;
//            this.color = color;
//            if (model == null)
//            {
//                model = new Mesh(LoadedMesh.cube_repeating, Vector3.Zero, 
//                    new TextureEffect(TextureEffectType.LambertFixed, SpriteName.BlockTexture),
//                    1, false);
//            }
//        }

//        /* Family methods */
//        public override void Draw()
//        {
//            model.Position = this.Position;
//            model.Scale = tempScale;
//            model.Rotation = this.Rotation;
//            model.Color = this.color;
//            model.Draw(0);
//        }
//        public override void Draw(int cameraIndex)
//        { }

//        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
//        {
//            model.Position = this.Position;
//            model.Scale = tempScale;
//            model.Rotation = this.Rotation;
//            model.DrawDeferred(device, shader, view, cameraIndex);
//        }
//        public override void DrawDeferredDepthOnly(GraphicsDevice device, Effect shader, int cameraIndex)
//        {
//            model.Position = this.Position;
//            model.Scale = tempScale;
//            model.Rotation = this.Rotation;
//            model.DrawDeferredDepthOnly(device, shader, cameraIndex);
//        }
//    }

//    /// <summary>
//    /// Invisible voxelobject replacement
//    /// </summary>
//    class VoxelObjPoint : VoxelModel
//    {
//        public override int Frame { set { } }
//        public override int NumFrames { get { return 1; } }
//        public override bool Animated { get { return false; } }

//        public VoxelObjPoint()
//            : base(false)
//        {
//        }

//        public override void Draw()
//        { //do nothing
//        }
//        public override void Draw(int cameraIndex)
//        { }

//        public override void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex)
//        {
//        }

//        public override void DrawDeferredDepthOnly(GraphicsDevice device, Effect shader, int cameraIndex)
//        {
//        }
//    }
//}
