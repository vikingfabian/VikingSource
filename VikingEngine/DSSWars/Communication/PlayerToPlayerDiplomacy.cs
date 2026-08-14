using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.GameObject.ObjectPointer;
using VikingEngine.DSSWars.Players;

namespace VikingEngine.DSSWars.Communication
{
    class PlayerToPlayerDiplomacy
    {
        public PFaction pfaction;

        public bool suggestingNewRelation = false;
        public RelationType suggestedRelation;
        public PFaction suggestedBy;

        public PlayerToPlayerDiplomacy(PFaction factionIndex)
        {
            this.pfaction = factionIndex;
        }

        public void writeGameState(BinaryWriter w)
        {
            w.Write((short)suggestedRelation);
            //w.Write((ushort)suggestedBy);
            suggestedBy.write(w);
        }

        public void readGameState(BinaryReader r, int subversion)
        {
            suggestedRelation = (RelationType)r.ReadInt16();
            suggestedBy = new PFaction(r);//r.ReadUInt16();
        }

        public void refresh(RelationType relation)
        {
            if (relation == suggestedRelation)
            {
                suggestingNewRelation = false;
            }
        }

        public void writeNet(BinaryWriter w)
        {
            w.Write(suggestingNewRelation);

            if (suggestingNewRelation)
            {
                w.Write((short)suggestedRelation);
            }
        }

        public void readNet(BinaryReader r, AbsHumanPlayer fromPlayer)
        {
            suggestingNewRelation = r.ReadBoolean();

            if (suggestingNewRelation)
            {
                suggestedRelation = (RelationType)r.ReadInt16();
            }
            suggestedBy = fromPlayer.pfaction;
        }
    }
}
