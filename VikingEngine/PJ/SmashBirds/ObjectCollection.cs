using System;
using System.Collections.Generic;
using System.Text;

namespace VikingEngine.PJ.SmashBirds
{
    class ObjectCollection
    {
        public SpottedArray<AbsProjectile> projectiles;
        SpottedArrayCounter<AbsProjectile> eggsCounter;

        public ObjectCollection()
        {
            projectiles = new SpottedArray<AbsProjectile>(16);
            eggsCounter = new SpottedArrayCounter<AbsProjectile>(projectiles);

            SmashRef.objects = this;
        }

        public void update()
        {
            foreach (var m in SmashRef.gamers)
            {
                m.update();
            }

            eggsCounter.Reset();
            int eggIx = 0;
            while (eggsCounter.Next())
            {
                if (eggsCounter.sel.update())
                {
                    eggsCounter.RemoveAtCurrent();
                }
                else
                {
                    eggsCounter.sel.objectCollisionsUpdate(eggIx);
                }
                ++eggIx;
            }

            int gamerIx = 0;
            foreach (var m in SmashRef.gamers)
            {
                m.objectCollisionsUpdate(gamerIx);
                ++gamerIx;
            }
        }

        public void update_asynch()
        {
            foreach (var m in SmashRef.gamers)
            {
                m.update_asynch();
            }

            var counter = eggsCounter.IClone();
            while (counter.Next())
            {
                counter.GetSelection.update_asynch();
            }
        }
    }
}
