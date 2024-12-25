using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warmashines
{
    class ManuballistaModel : AbsWarmashineModel
    {
        int loadedFrame = 3;

        public ManuballistaModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
            const float Xdiff = 0.2f;
            const float Zdiff = -0.37f;

            workers = new WarmashineWorkerCollection();

            float scale = soldier.soldierData.modelScale;

            workers.Add(soldier.GetFaction(),
                scale * Xdiff, scale * Zdiff);
            workers.Add(soldier.GetFaction(),
                scale * -Xdiff, scale * Zdiff);
        }



        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            updateAnimationBoth(soldier);
        }

        void updateAnimationBoth(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                model.Frame = loadedFrame;
            }
            else
            {
                if (soldier.attackCooldownTime.HasTime)
                {
                    float percReloadTime = soldier.attackCooldownTime.MilliSeconds / soldier.prevAttackTime;
                    model.Frame = (int)(1 + (1f - percReloadTime) * 3);
                }
                else
                {
                    model.Frame = loadedFrame;
                }
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            workers.update(soldier);
        }
    }
}
