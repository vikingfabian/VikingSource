using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.Graphics
{
    class Billboard2D : Mesh
    {
        public bool FaceCamera = true;

        public Billboard2D(Vector3 pos, SpriteName sprite, float scale, bool addToRender)
            :base(LoadedMesh.plane, pos, new Vector3(scale), TextureEffectType.Flat, sprite, Color.White, true)
        {
        }
        public override void Draw(int cameraIndex)
        {
            if (FaceCamera)
                this.Rotation = Ref.draw.Camera.BillBoard2DRotation;
            base.Draw(cameraIndex);
        }
        
    }
}
