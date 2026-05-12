using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Communication
{
    class PlayerToPlayerDiplomacy
    {
        //public int index;
        public int factionIndex;

        public bool suggestingNewRelation = false;
        public RelationType suggestedRelation;
        public int suggestedBy;

        public PlayerToPlayerDiplomacy(int factionIndex)
        {
            this.factionIndex = factionIndex;
        }

        public void writeGameState(BinaryWriter w)
        {
            //w.Write((short)factionIndex);
            w.Write((short)suggestedRelation);
            w.Write((ushort)suggestedBy);
        }

        public void readGameState(BinaryReader r, int subversion)
        {
            //factionIndex = r.ReadUInt16();
            suggestedRelation = (RelationType)r.ReadInt16();
            suggestedBy = r.ReadUInt16();
        }
    }
}
