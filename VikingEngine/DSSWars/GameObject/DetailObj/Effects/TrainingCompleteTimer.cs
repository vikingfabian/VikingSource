using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class TrainingCompleteTimer : AbsInGameUpdateable
    {
        int nextCityCheck = 15;
        float trainingSpeed = 2f;
        Time time;
        SoldierGroup group;
        public TrainingCompleteTimer(SoldierGroup group)
            : base(true)
        {
            var typeData = DssRef.unitsdata.Get(group.type);
            time = new Time(typeData.recruitTrainingTimeSec, TimeUnit.Seconds);
            this.group = group;
        }

        public override void Time_Update(float time_ms)
        {
            if (--nextCityCheck < 0)
            {
                trainingSpeed = adjacentToCity()? 2f : 1f;
                nextCityCheck = 30;
            }

            if (time.CountDown(Ref.DeltaGameTimeMs * trainingSpeed))
            {
                group.completeTransform(SoldierTransformType.TraningComplete);
                DeleteMe();
            }
        }

        bool adjacentToCity()
        {
            var tile = DssRef.world.tileGrid.Get(group.army.tilePos);
            var city = tile.City();
            if (city.faction == group.army.faction)
            {
                if ((group.army.tilePos - city.tilePos).SideLength() <= 2)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool RunDuringPause => false;
    }
}
