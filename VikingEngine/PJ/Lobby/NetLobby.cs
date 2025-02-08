using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.PJ.Lobby
{
    class NetLobby : Network.NetLobby
    {
        public NetLobby() 
            :base()
        { 
            
        }

        public override void EnterLobby(bool enter)
        {
            base.EnterLobby(enter);
            autoCreateSession = enter;
            searchLobbies = enter;
        }
    }
}
