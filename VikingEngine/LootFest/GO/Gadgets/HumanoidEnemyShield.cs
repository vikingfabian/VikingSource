using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.GO.Gadgets
{
    class HumanoidEnemyShield : VikingEngine.LootFest.GO.AbsChildObject
    {
        public Graphics.AbsVoxelObj model;
        Vector3 posOffset;
        bool isDeleted = false;
        Effects.BouncingBlockColors damageColors;
        public bool indestructable = false;

        public HumanoidEnemyShield(VoxelModelName modelName, float scale,
            Vector3 posOffset, GO.Characters.AbsCharacter parent, Effects.BouncingBlockColors damageColors)
        {
            this.damageColors = damageColors;
            model = LfRef.modelLoad.AutoLoadModelInstance(modelName, scale, 0, true);
            this.posOffset = posOffset;
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(Characters.AbsCharacter parent)
        {
 	        var rotation = parent.FireDir3D(GameObjectType.NUM_NON);

            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation = rotation;
            return isDeleted;
        }

        public void TakeHit()
        {
            float scale = model.Scale1D * 1.6f;

            for (int i = 0; i < 8; ++i)
            {
                if (Ref.gamesett.DetailLevel < 2)
                {
                    new Effects.BouncingBlock2Dummie(model.position, damageColors.GetRandom(), scale);
                }
                else
                {
                    new Effects.BouncingBlock2(model.position, damageColors.GetRandom(), scale);
                }
            }
            DeleteMe();

            new Effects.DropEnemyShield(model, damageColors);
        }

        public override void ChildObject_OnParentRemoval(Characters.AbsCharacter parent)
        {
            DeleteMe();
            model.DeleteMe();
        }

        public void DeleteMe()
        {
            //model.DeleteMe();
            isDeleted = true;
        }
    }
    
}
