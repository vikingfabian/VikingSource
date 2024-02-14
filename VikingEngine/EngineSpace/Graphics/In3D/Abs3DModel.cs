
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.EngineSpace.Graphics.DeferredRendering.Lights;
using VikingEngine.EngineSpace.Graphics.DeferredRendering;

namespace VikingEngine.Graphics
{
    /// <summary>
    /// Class to connect the common properties of a loaded model and generated mesh
    /// </summary>
    abstract class Abs3DModel : AbsDraw
    {
        public Vector3 position = Vector3.Zero;
        public Vector3 scale = Vector3.One;
#if DEBUG
        public string DebugName = null;
#endif
        /* Static */
        public static void ModelData(Model model)
        {
            ModelMeshPart part = model.Meshes[0].MeshParts[0];

            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[part.VertexBuffer.VertexCount];
            part.VertexBuffer.GetData<VertexPositionNormalTexture>(vertices);

            ushort[] drawOrder = new ushort[part.IndexBuffer.IndexCount];
            part.IndexBuffer.GetData<ushort>(drawOrder);

        }

        /* Fields */
        public Vector4 colorAndAlpha = Vector4.One;
        public RotationQuarterion Rotation = RotationQuarterion.Identity;

        /* Constructors */
        public Abs3DModel(bool add)
            : base(add)
        { }

        public override Color Color
        {
            get { return new Color(colorAndAlpha); }
            set { colorAndAlpha = value.ToVector4(); }
        }
        public override float Opacity { get { return colorAndAlpha.W; } set { colorAndAlpha.W = value; } }

        public override void ColorAndAlpha(Color color, float alpha)
        {
            colorAndAlpha = color.ToVector4();
            colorAndAlpha.W = alpha;
        }

        /* Methods */
        public abstract void DrawDeferred(GraphicsDevice device, Effect shader, Matrix view, int cameraIndex);
        public abstract void DrawDeferredDepthOnly(Effect shader, int cameraIndex);

#region CAMERA CULLING
        private static BoundingSphere boundingSphere = new BoundingSphere();
        public abstract void updateBoundingSphere(ref BoundingSphere boundingSphere);

        public bool AlwaysInCameraCulling = false;
        public bool UseCameraCulling = true;

        public bool InCameraView = true;
        public EightBit inPlayerCamera = new EightBit(byte.MaxValue);

        public void setVisibleCamera(int playerIndex)
        {
            inPlayerCamera = EightBit.Zero;
            inPlayerCamera.Set(playerIndex, true);
        }

        public override void UpdateCulling()
        {
            if (UseCameraCulling)
            {
                if (AlwaysInCameraCulling)
                {
                    inPlayerCamera.bitArray = byte.MaxValue;
                    InCameraView = true;
                }
                else
                {
                    bool inCameraView = false;
                    updateBoundingSphere(ref boundingSphere);
                    for (int i = 0; i < Ref.draw.ActivePlayerScreens.Count; ++i)
                    {
                        PlayerData p = Ref.draw.ActivePlayerScreens.Array[i];
                        if (p != null)
                        {
                            BoundingSphere copy = boundingSphere;
                            if (Ref.draw is DeferredRenderer)
                            {
                                copy.Center *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
                                copy.Radius *= DeferredRenderer.FLOATING_POINT_PRECISION_MODIFIER;
                            }
                            bool inCam = p.view.Camera.Frustum.Intersects(copy);
                            inPlayerCamera.Set(i, inCam);
                            inCameraView = inCameraView || inCam;
                        }
                    }
                    this.InCameraView = inCameraView;
                }
            }
        }

        public bool VisibleInCamera(int playerIndex)
        {
            if (playerIndex == -1) // shadow mapping only
                return visible; 
            return visible && inPlayerCamera.Get(playerIndex);
        }

        //public GetVisibleInCamera(int playerIndex)
        //{
        //    if (playerIndex == -1) // shadow mapping only
        //        return visible;
        //    return visible && inPlayerCamera.Get(playerIndex);
        //}

        public virtual float Scale1D
        {
            get { return scale.X; }
            set { scale.X = value; scale.Y = value; scale.Z = value; }
        }
        #endregion

        override public float PositionX { get { return position.X; } set { position.X = value; } }
        override public float PositionY { get { return position.Y; } set { position.Y = value; } }
        override public float PositionZ { get { return position.Z; } set { position.Z = value; } }

        override public Vector2 PositionXY { get { return new Vector2(position.X, position.Y); } set { position.X = value.X; position.Y = value.Y; } }
        override public Vector2 PositionXZ { get { return new Vector2(position.X, position.Z); } set { position.X = value.X; position.Z = value.Y; } }
        override public Vector3 PositionXYZ { get { return position; } set { position = value; } }

#if DEBUG
        public override string ToString()
        {
            return "3DModel: " + DebugName;
        }
#endif


        public override void AddXY(Vector2 value)
        {
            position.X += value.X;
            position.Y += value.Y;
        }
    }
}