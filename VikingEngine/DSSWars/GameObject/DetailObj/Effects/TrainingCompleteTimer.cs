using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.GameObject
{
    class TrainingCompleteTimer : AbsInGameUpdateable
    {
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
            if (time.CountDownGameTime())
            {
                group.completeTransform(SoldierTransformType.TraningComplete);
                DeleteMe();
            }
        }

        public override bool RunDuringPause => false;
    }
}
