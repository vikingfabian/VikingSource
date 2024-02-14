using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class HumanoidEnemyPreBowEffect : GO.AbsChildModel
    {
        //static readonly Data.TempBlockReplacementSett TempImage = new Data.TempBlockReplacementSett(new Color(147, 81, 28), new Vector3(0.2f, 3, 0.4f));

        //new Vector3(0.8f, -0.1f, 0.65f)
        public HumanoidEnemyPreBowEffect(VoxelModelName modelName, Vector3 posOffset, GO.Characters.AbsCharacter parent)
            : base()
        {
            this.posOffset = posOffset;
            model = LfRef.modelLoad.AutoLoadModelInstance(modelName, 3.5f, 0, true);
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(GO.Characters.AbsCharacter parent)
        {
            base.ChildObject_Update(parent);
            model.Rotation = parent.RotationQuarterion;
            model.Rotation.RotateAxis(new Vector3(0, 0, -0.8f));
            return isDeleted;
        }
    }
}
