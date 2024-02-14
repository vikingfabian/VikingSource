using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace VikingEngine.LootFest.GO.PlayerCharacter
{
    class RidingHeroModel : VikingEngine.LootFest.GO.AbsChildObject
    {
        public Graphics.AbsVoxelObj model;
        public Vector3 posOffset;

        public RidingHeroModel(Vector3 posOffset, GO.Characters.AbsCharacter parentMount)
        {
            this.posOffset = posOffset;
            //new Graphics.AnimationsSettings(2, 0.1f)
            
            model =  new Graphics.VoxelModelInstance(LfRef.Images.StandardModel_Character);
            model.Scale1D = AbsHero.StandardHeroSize;

            parentMount.AddChildObject(this);
        }

        public void SetModel(Graphics.VoxelModel original, VoxelModelName modelName)
        {
           model.SetMaster(original);
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
            var rotation = parent.FireDir3D(GameObjectType.NUM_NON);

            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation = rotation;
            return false;
        }

        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            model.DeleteMe();
        }
    }
}
