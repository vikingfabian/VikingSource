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
            lpIndex = -1;
            remote = DssRef.state.remotePlayers.counter();
        }

        public bool Next()
        {
            lpIndex++;

            if (lpIndex < DssRef.state.localPlayers.Count)
            {
                sel = DssRef.state.localPlayers[lpIndex];
                return true;
            }

            if (remote.Next())
            { 
                sel = remote.sel;
                return true;
            }

            return false;
        }
    }
}
