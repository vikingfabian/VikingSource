using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsEffect
    {
        protected Effect shader = null;

        abstract public void Draw(Mesh obj);

        abstract protected void SetVertexBufferEffect(AbsVoxelObj obj);

        virtual public void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            if (VB != null)
            {
                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.SetVertexBuffer(vertexBuffer_GPU);
                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.Indices = indexBuffer;
                VB.SetBuffer();

                //basicEffect.World = Matrix.CreateScale(obj.scale) * Matrix.CreateFromQuaternion(obj.Rotation.QuadRotation) * Matrix.CreateTranslation(obj.position);
                //basicEffect.Projection = Ref.draw.Camera.Projection;
                //basicEffect.View = Ref.draw.Camera.ViewMatrix;
                SetVertexBufferEffect(obj);
                               

                shader.CurrentTechnique.Passes[0].Apply();

                //Engine.Draw.graphicsDeviceManager.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, f.startDrawOrderIndex, f.primitiveCount);
                VB.Draw(frame);
            }
        }

        virtual public void SetColor(Vector3 col) { throw new NotImplementedException(); }

    }
    
}
