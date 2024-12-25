using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VikingEngine.LootFest;
using VikingEngine.LootFest.GO.EnvironmentObj;

namespace VikingEngine.DSSWars.GameObject.DetailObj.Warmashines
{
    class HaubitzModel : AbsWarmashineModel
    {
        Graphics.AbsVoxelObj barrel;
        Vector3 diff;
        public HaubitzModel(AbsSoldierUnit soldier)
          : base(soldier)
        {
            float Xdiff = 0.3f;
            float Zdiff = -0.25f;
            

            workers = new WarmashineWorkerCollection();

            float scale = soldier.soldierData.modelScale;
            diff = new Vector3(0, 0.11f, 0.18f) * scale;

            workers.Add(soldier.GetFaction(),
                scale * Xdiff, scale * Zdiff);
            workers.Add(soldier.GetFaction(),
                scale * -Xdiff, scale * Zdiff);

            barrel = soldier.group.army.faction.AutoLoadModelInstance(
                 VoxelModelName.wars_ironsiegecannon, scale, false);
            barrel.Frame = 1;
            barrel.AddToRender(DrawGame.UnitDetailLayer);
        }



        protected override void updateAnimation(AbsSoldierUnit soldier)
        {
            updateAnimationBoth(soldier);
        }

        void updateAnimationBoth(AbsSoldierUnit soldier)
        {
            if (soldier.state.walking)
            {
                barrel.Visible = false;
                model.Frame = 0;
            }
            else
            {
                barrel.Visible = true;
                //barrel.Rotation = model.Rotation;
                barrel.Rotation = RotationQuarterion.FromWorldRotation(new Vector3(soldier.rotation.radians, -0.7f, 0));
                barrel.position = model.Rotation.TranslateAlongAxis(
                    diff, model.position);

                model.Frame = 2;
                
            }

            WP.Rotation1DToQuaterion(model, soldier.rotation.Radians);

            workers.update(soldier);
        }

        public override void DeleteMe()
        {
            base.DeleteMe();
            barrel.DeleteMe();
        }
    }
}


