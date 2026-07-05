using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players
{
    struct AllHumansLoop
    {
        public AbsHumanPlayer sel;
        int lpIndex;
        SpottedArrayCounter<RemotePlayer> remote;
        public AllHumansLoop() 
        {
            Reset();
        }

        public void Reset()
        {
            lpIndex = -1;
            remote = DssRef.state.remotePlayers.counter();
        }

        public bool Next(out bool ready)
        {
            lpIndex++;

            if (lpIndex < DssRef.state.localPlayers.Count)
            {
                sel = DssRef.state.localPlayers[lpIndex];
                ready = true;
                return true;
            }

            if (remote.Next())
            { 
                sel = remote.sel;
                ready = remote.sel.ready;
                return true;
            }

            ready = false;
            return false;
        }
    }
}
