using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Event
{
    struct ProcessTime
    {   
        const float Quarter = 0.25f;
        
        float secondsToQuarter;
        int quarter;
        public ProcessEvent update()
        {
            secondsToQuarter += Ref.DeltaTimeSec;

            if (secondsToQuarter >= Quarter)
            {
                ++quarter;
                secondsToQuarter = 0;
                switch (quarter)
                {
                    case 0: return ProcessEvent.SubTileReload;
                    
                    case 1:
                        return ProcessEvent.FarLayerMap;
                    case 2:
                        return ProcessEvent.MidLayerMap;

                    case 4:
                        quarter = -1;
                        break;
                }
                
            }
            return ProcessEvent.None;
        }

        enum ProcessOrder
        {
            SubTileReload, //one second
            OverviewMap1, //500ms
            OverviewMap2, //500ms        
            NUM
        }
    }

    enum ProcessEvent
    {
        None,
        SubTileReload, //one second
        MidLayerMap, //500ms
        FarLayerMap,
    }
}
