using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    struct MinuteStats
    {
        public float displayValue_perMinute;
        float collectValue;

        public void add(float value)
        { 
            collectValue += value;
        }

        public void minuteUpdate()
        { 
            float value = collectValue;
            collectValue -= value;

            displayValue_perMinute = value /*/ 60f*/;
        }

        public float DisplayValuePerSec => displayValue_perMinute / TimeExt.MinuteInSeconds;

        public int DisplayInt => Convert.ToInt32(displayValue_perMinute);
    }
}
