using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Network;

namespace VikingEngine.DSSWars.Players
{
    abstract class AbsHumanPlayer : AbsPlayer
    {
        public NetworkInstancePeer networkPeer;

        public AbsHumanPlayer(Faction faction)
            : base(faction)
        { }

        public AbsHumanPlayer()
            : base()
        { }

        override public AbsHumanPlayer GetHumanPlayer()
        {
            return this;
        }
    }
}
