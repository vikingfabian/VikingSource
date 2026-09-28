using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;

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

        public virtual void Close()
        {
            DssRef.state.cutScene = null;
            GC.Collect();
        }

        public abstract void Time_Update(float time);

        public virtual PlayerNetState NetState() { return PlayerNetState.InMenu; }
    }
}
