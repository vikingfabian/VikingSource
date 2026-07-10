using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players
{
    struct HumanPlayerCounter
    {
        bool local;
        int index;
        public AbsHumanPlayer sel;

        public HumanPlayerCounter()
        {
            local = true;
        }

        public bool Next()
        {
            sel = null;
            if (local)
            {
                if (index < DssRef.state.localPlayers.Count)
                {
                    sel = DssRef.state.localPlayers[index];
                    index++;
                }
                else
                {
                    local = false;
                    index = 0;
                }
            }

            if (!local)
            {
                if (index < DssRef.state.remotePlayers.Count)
                {
                    sel = DssRef.state.remotePlayers[index];
                    index++;
                }
            }

            return sel != null;
        }
    }
}
