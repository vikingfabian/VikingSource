using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class PreRangedAttackWeaponEffect : VikingEngine.LootFest.GO.AbsChildModel
    {
        Time scaleAnimationTime;
        float scaleUpSpeed;
        float endScale;

        public PreRangedAttackWeaponEffect(VoxelModelName modelName, float startScale, float endScale,
            Time scaleAnimationTime, Vector3 posOffset, GO.Characters.AbsCharacter parent)
        {
            model = LfRef.modelLoad.AutoLoadModelInstance(modelName, startScale, 0, false);
            this.scaleAnimationTime = scaleAnimationTime;
            scaleUpSpeed = (endScale - startScale) / scaleAnimationTime.MilliSeconds;
            scaleUpSpeed *= model.Scale1D / startScale;
            this.endScale = endScale * (model.Scale1D / startScale);
            this.posOffset = posOffset;
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(GO.Characters.AbsCharacter parent)
        {
            if (!scaleAnimationTime.CountDown())
            {
                model.Scale1D += scaleUpSpeed * Ref.DeltaTimeMs;
            }
            else
            {
                model.Scale1D = endScale;
            }

            var rotation = parent.FireDir3D(GO.GameObjectType.PreRangedAttackWeaponEffect);

            model.position = rotation.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation = rotation;
            return isDeleted;
        }
    }
}
