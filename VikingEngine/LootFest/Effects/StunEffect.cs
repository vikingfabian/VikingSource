using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    class StunEffect: VikingEngine.LootFest.GO.AbsChildModel
    {
      // public //static readonly Data.TempBlockReplacementSett TempModel = new Data.TempBlockReplacementSett(Color.Yellow, new Vector3(2f, 0.1f, 2f));
        public Time stunTimer;
        public StunEffect(Vector3 posOffset, float scale, Time stunTimer, GO.Characters.AbsCharacter parent)
        {
            this.stunTimer = stunTimer;
            this.posOffset = posOffset;
            model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.stun, scale, 0, false);
            parent.AddChildObject(this);
        }

        public override bool ChildObject_Update(GO.Characters.AbsCharacter parent)
        {
            model.position = parent.RotationQuarterion.TranslateAlongAxis(posOffset, parent.Position);
            model.Rotation.RotateWorldX(2 * Ref.DeltaTimeSec);
            if (stunTimer.CountDown())
            {
                DeleteMe();
                return true;
            }
            return false;
        }

        
    }
}
