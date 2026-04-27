using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Map;
using VikingEngine.Graphics;

namespace VikingEngine.DSSWars.Players.Orders
{
    class GodBuild : AbsUpdateable
    {
        protected VoxelModelInstance model;
        float frameTime = 0;
        CircleCounter frameCounter = new CircleCounter(0,0, 2);
        TimeStamp timeStamp = TimeStamp.None;

        public GodBuild(IntVector2 subTile)
            :base(true)
        { 
            createModel(subTile);
            timeStamp.setNow();
        }

        void createModel(IntVector2 subTile)
        {
            //Debug.CrashIfThreaded();
            model = DssRef.models.ModelInstance_drawbatch(LootFest.VoxelModelName.godfire, WorldData.SubTileWidth * 1.0f);
            model.position = WP.SubtileToWorldPosXZgroundY_Centered(subTile);

        }

        public override void Time_Update(float time_ms)
        {
            frameTime += time_ms;
            if (frameTime > 120)
            {
                frameTime = 0;
                frameCounter.Next(1);
                model.Frame = frameCounter.Value;

                if (timeStamp.secPassed(1f))
                {
                    DeleteMe();
                }
            }
        }

        public override void DeleteMe()
        {
            model.preRemoveFromDrawBatch();
            base.DeleteMe();
        }
    }
}
