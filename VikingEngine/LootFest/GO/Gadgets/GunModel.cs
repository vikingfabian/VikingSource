using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO.Gadgets
{
    class GunModel : VikingEngine.LootFest.GO.AbsChildObject
    {
        public Graphics.AbsVoxelObj model;
        Vector3 posOffset;
        bool isDeleted = false;
        Time fireBlastTime = 0;

        public GunModel(VoxelModelName modelName, float scale,
            Vector3 posOffset, GO.Characters.AbsCharacter parent)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(modelName, scale, 0, true);
            this.posOffset = posOffset;
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
 	        var rotation = parent.FireDir3D(GameObjectType.NUM_NON);

            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation = rotation;

            model.Frame = fireBlastTime.CountDown() ? 0 : 1;

            return isDeleted;
        }

        public void onFire()
        {
            model.Frame = 1;
            fireBlastTime.MilliSeconds = 60;
        }

        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            DeleteMe();
            model.DeleteMe();
        }

        public void DeleteMe()
        {
            model.DeleteMe();
            isDeleted = true;
        }
    }
    
}