using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Core.BlackBolts.GO.Creature;

namespace VikingEngine.Core.BlackBolts.Render
{
    class FlyingDragonEffect : AbsUpdateable
    {
        AbsCreature creature;
        public FlyingDragonEffect(AbsCreature creature) : base(true)
        {
            this.creature = creature;
        }
        public override void Time_Update(float time_ms)
        {
            creature.model.position.Y += 0.015f * Ref.DeltaGameTimeMs;

            if (creature.model.position.Y > 40)
            {
                DeleteMe();
            }
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            creature.model.DeleteMe();
        }
    }
}
