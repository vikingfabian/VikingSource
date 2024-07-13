using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct MinuteStats
    {
        public float displayValue_sec;
        float collectValue;

        public void add(float value)
        { 
            collectValue += value;
        }

        public void minuteUpdate()
        { 
            float value = collectValue;
            collectValue -= value;

            displayValue_sec = value / 60f;
        }
    }
}
