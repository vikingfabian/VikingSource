using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warmashines
{
    class SiegeCannonBronzeModel : AbsWarmashineModel
    {
        //int loadedFrame = 3;

        public SiegeCannonBronzeModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
            const float Xdiff = 0.18f;
            const float ZdiffFront = 0.30f;
            const float ZdiffCenter = 0f;
            const float ZdiffBack = -0.30f;

            workers = new WarmashineWorkerCollection();

            float scale = soldier.soldierData.modelScale;

            workers.Add(soldier.GetFaction(),
                scale * Xdiff, scale * ZdiffFront);
            workers.Add(soldier.GetFaction(),
                scale * -Xdiff, scale * ZdiffFront);

            workers.Add(soldier.GetFaction(),
               scale * Xdiff, scale * ZdiffCenter);
            workers.Add(soldier.GetFaction(),
                scale * -Xdiff, scale * ZdiffCenter);

            workers.Add(soldier.GetFaction(),
                scale * Xdiff, scale * ZdiffBack);
            workers.Add(soldier.GetFaction(),
                scale * -Xdiff, scale * ZdiffBack);
        }

        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            updateAnimationBoth(soldier);
        }

        void updateAnimationBoth(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                model.Frame = 0;
            }
            else
            {
                model.Frame = 1;
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            workers.update(soldier);
        }
    }
}
