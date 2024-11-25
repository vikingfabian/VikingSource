using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.LootFest.GO;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warmashines
{
    class CannonModel : AbsWarmashineModel
    {
        public CannonModel(AbsSoldierUnit soldier)
          : base(soldier)
        {
            const float Xdiff = 0.2f;
            const float Zdiff = -0.37f;

            //switch (soldier.group.soldierConscript.conscript.weapon)
            //{
            //    case Resource.ItemResourceType.ManCannonBronze:

            //        break;
            //    case Resource.ItemResourceType.SiegeCannonIron:

            //        break;
            //    case Resource.ItemResourceType.ManCannonIron:

            //        break;

            //    default:
            //        throw new NotImplementedException();

            //}


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
                model.Frame = 0;
            }
            else
            {
                model.Frame = 1;
                //if (soldier.attackCooldownTime.HasTime)
                //{
                //    float percReloadTime = soldier.attackCooldownTime.MilliSeconds / soldier.prevAttackTime;
                //    model.Frame = (int)(1 + (1f - percReloadTime) * 3);
                //}
                //else
                //{
                //    model.Frame = loadedFrame;
                //}
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            workers.update(soldier);
        }
    }
}
