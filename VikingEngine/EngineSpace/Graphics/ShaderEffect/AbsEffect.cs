using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.Graphics
{
    abstract class AbsEffect
    {
        public Effect shader = null;

        abstract public void Draw(Mesh obj);

        abstract protected void SetVertexBufferEffect(AbsVoxelObj obj);

        virtual public void DrawVB(int frame, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB)
        {
            if (VB != null)
            {
                VB.SetBuffer();

                SetVertexBufferEffect(obj);
                               
                shader.CurrentTechnique.Passes[0].Apply();

                VB.Draw(frame);
            }
        }

        //virtual public void DrawVB(int cameraIndex, AbsVoxelObj obj, AbsVertexAndIndexBuffer VB, Texture2D texture)
        //{
        //    if (VB != null)
        //    {
        //        VB.SetBuffer();

        //        SetVertexBufferEffect(obj);

        //        shader.CurrentTechnique.Passes[0].Apply();

        //        VB.Draw(cameraIndex);
        //    }
        //}

        virtual public void SetColor(Vector3 col) { throw new NotImplementedException(); }

        public void SetColor(Vector4 color)
        {
            shader.Parameters[CustomEffect.ColorArgument].SetValue(color);
        }

    }
    
}
