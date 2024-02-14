using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest
{
    class ModelSpammer : AbsUpdateable
    {
        Timer.Basic spawnTimer = new Timer.Basic(200, true);
        VoxelModelName model = VoxelModelName.CATEGORY_CHARACTER_0 + 1;
        Rotation1D dir = Rotation1D.Random();
        float distance = 1f;
        Vector3 center;

        public ModelSpammer(Vector3 heroPos)
            : base(true)
        {
            this.center = heroPos;
            center.Y += 5;
        }

        public override void Time_Update(float time_ms)
        {
            if (spawnTimer.Update())
            {
                Vector3 pos = VectorExt.AddXZ(center, dir.Direction(distance));
                distance += 0.1f;
                dir.Add(0.5f / distance);

                LfRef.modelLoad.AutoLoadModelInstance(model).position = pos;

                model++;

                if (model == VoxelModelName.CATEGORY_WEAPON_1)
                {
                    model++;
                }
                if (model >= VoxelModelName.CATEGORY_APPEARANCE_2)
                {
                    this.DeleteMe();
                }

            }
        }
    }
}
