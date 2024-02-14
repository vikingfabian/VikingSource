using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsChildModel : AbsChildObject
    {
        public Graphics.AbsVoxelObj model;
        public Vector3 posOffset;
        public bool isDeleted = false;


        public AbsChildModel()
        { }

        public AbsChildModel(Graphics.AbsVoxelObj model, Vector3 posOffset, GO.Characters.AbsCharacter parent)
        { 
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(GO.Characters.AbsCharacter parent)
        {
            var rotation = parent.FireDir3D(GameObjectType.NUM_NON);
            model.Rotation = rotation;
            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            return isDeleted;
        }

        public void DeleteMe()
        {
            model.DeleteMe();
            isDeleted = true;
        }

        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            DeleteMe();
        }

        public Vector3 Position { get { return model.position; } }
    }


}
