using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Interface.CutScene
{
    abstract class AbsCutScene
    {
        public AbsCutScene()
        {
            if (DssRef.state.cutScene != null)
            {
                throw new Exception("Multiple cutscenes");
            }
            DssRef.state.cutScene = this;
        }

        virtual public void Close()
        {
            DssRef.state.cutScene = null;
            GC.Collect();
        }

        abstract public void Time_Update(float time);
    }
}
