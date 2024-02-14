using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.GameObjects.Characters
{
    class AbsGiant
    {
    }
    class Limb
    {
        public Vector3 RelRotation = Vector3.Zero;
        Vector3 relpos;
        public Vector3 Position = Vector3.Zero;
        Graphics.VoxelModelInstance image;

        public Limb(bool mirrorX, Vector3 relpos, Graphics.VoxelModel orgImage, float scale)
        {
            this.relpos = relpos;
            if (mirrorX)
            {
                this.relpos.X = -relpos.X;
            }

            image = new Graphics.VoxelModelInstance(orgImage);
            image.scale = Vector3.One * scale;
        }

        public void Update(Graphics.AbsVoxelObj parent)
        {
            image.position = parent.Rotation.TranslateAlongAxis(relpos + Position, parent.position);
            image.Rotation = parent.Rotation;
            image.Rotation.RotateAxis(RelRotation);
        }
        public void DeleteMe()
        {
            image.DeleteMe();
        }
    }
}
