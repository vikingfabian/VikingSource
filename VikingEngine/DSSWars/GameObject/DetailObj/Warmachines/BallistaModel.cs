using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Graphics;
using VikingEngine.LootFest;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warmachines
{
    abstract class AbsWarmachineModel : SoldierUnitAdvancedModel
    {
        protected WarmachineWorkerCollection workers;

        public AbsWarmachineModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
        }

        public override void onNewModel(VoxelModelName name, VoxelModel master, AbsSoldierUnit unit)
        {
            base.onNewModel(name, master, unit);

            workers.onNewModel_asynch(name, master);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            workers.DeleteMe();
        }
    }

    class BallistaModel : AbsWarmachineModel
    {       
        int loadedFrame = 3;

        public BallistaModel(AbsSoldierUnit soldier)
           : base(soldier)
        {
            const float Xdiff = 0.2f;
            const float Zdiff = -0.37f;

            workers = new WarmachineWorkerCollection();

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
