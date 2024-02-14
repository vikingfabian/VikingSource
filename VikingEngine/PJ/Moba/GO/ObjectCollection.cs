using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Moba.GO
{
    class ObjectCollection
    {
        public SpottedArray<AbsUnit> units = new SpottedArray<AbsUnit>();
        public SpottedArrayCounter<AbsUnit> unitCounter;// = new SpottedArray<AbsUnit>();
        public SpottedArrayCounter<AbsUnit> unitCounter_asynch;// = new SpottedArray<AbsUnit>();

        public List<Tower> towers = new List<Tower>();

        public ObjectCollection()
        {
            MobaRef.objects = this;
            unitCounter = new SpottedArrayCounter<AbsUnit>(units);
            unitCounter_asynch = new SpottedArrayCounter<AbsUnit>(units);
        }

        public void Update()
        {
            unitCounter.Reset();
            while (unitCounter.Next())//for (int i = units.Count -1; i >= 0; --i)
            {
                unitCounter.sel.Update();
            }
            foreach (var m in towers)
            {
                m.Update();
            }
        }

        public void UpdateAsynch()
        {
            unitCounter_asynch.Reset();
            while (unitCounter_asynch.Next())
            {
                unitCounter_asynch.sel.UpdateAsynch();
            }
        }
    }
}
