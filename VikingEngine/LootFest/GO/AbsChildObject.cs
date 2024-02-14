using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.GO
{
    abstract class AbsChildObject : IChildObject
    {
        public IChildObject childObjects = null;
        virtual public IChildObject LinkedChildObject { get { return childObjects; } set { childObjects = value; } }
        virtual public bool ChildObject_Update(GO.Characters.AbsCharacter parent) { throw new NotImplementedException("UpdateChildObject"); }
        public void AddChildObject(IChildObject child)
        {
            if (childObjects == null)
            {
                childObjects = child;
            }
            else
            {
                IChildObject prevchild = childObjects;
                while (prevchild.LinkedChildObject != null)
                {
                    prevchild = prevchild.LinkedChildObject;
                }
                prevchild.LinkedChildObject = child;
            }
        }

        abstract public void ChildObject_OnParentRemoval(GO.Characters.AbsCharacter parent);
    }

    //class AbsChildModel : VikingEngine.LootFest.GO.AbsChildObject
    //{ 
    //    public Graphics.AbsVoxelObj model;
    //    public Vector3 posOffset;

    //    public AbsChildModel(Graphics.AbsVoxelObj model, Vector3 posOffset)
    //    {
    //        //model = Data.VoxModelAutoLoad.AutoLoadModelInstance(VoxelModelName.baby,
    //        //    LfRef.Images.StandardModel_TempBlockAnimated, parentModel.Size1D * 0.3f, 0, false, new Graphics.AnimationsSettings(2, 1));
    //        //posOffset = new Vector3(0, 1f, 1f);
    //        this.model = model;
    //        this.posOffset = posOffset;

    //    }

    //    public override bool ChildObject_Update(Characters.AbsCharacter parent)
    //    {
    //        var rotation = parent.RotationQuarterion;

    //        model.Position = rotation.TranslateAlongAxis(posOffset, parent.Position);
    //        model.Rotation = rotation;
    //        // model.Rotation.RotateWorldX(MathHelper.PiOver2);

    //        return false;
    //    }

    //    public override void ChildObject_OnParentRemoval(AbsCharacter parent)
    //    {
    //        model.DeleteMe();
    //    }
    //}
}
