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
                SetVertexBufferEffect(obj);

                VB.SetBuffer();

                shader.CurrentTechnique.Passes[0].Apply();
                VB.Draw(frame);
            }
        }

        virtual public void SetColor(Vector3 col) { throw new NotImplementedException(); }

    }
    
}
