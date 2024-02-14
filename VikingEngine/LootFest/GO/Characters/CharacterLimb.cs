using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Characters
{
    class CharacterLimb
    {
        public Vector3 RelRotation = Vector3.Zero;
        public Vector3 relpos;
        public Vector3 Position = Vector3.Zero;
        public Graphics.AbsVoxelObj model;

        virtual public void Update(Graphics.AbsVoxelObj parent)
        {
            model.position = parent.Rotation.TranslateAlongAxis(relpos + Position, parent.position);
            model.Rotation = parent.Rotation;
            model.Rotation.RotateAxis(RelRotation);
        }
        virtual public void DeleteMe()
        {
            model.DeleteMe();
        }
    }
}
