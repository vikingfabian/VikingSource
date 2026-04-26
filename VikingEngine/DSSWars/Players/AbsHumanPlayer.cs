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

        public AbsHumanPlayer(Faction faction, bool newGame)
            : base(faction, newGame)
        { }

        public AbsHumanPlayer()
            : base()
        { }

        virtual public void AssignFaction(Faction faction)
        {
            setPlayerFaction(faction);
            base.AssignFaction(faction);
           
        }

        override public AbsHumanPlayer GetHumanPlayer()
        {
            return this;
        }

        public override bool IsHumanPlayer()
        {
            return true;
        }

        public void setPlayerFaction(Faction faction)
        {
            faction.factiontype = FactionType.Player;
            faction.availableForPlayer = false;
        }
    }
}
