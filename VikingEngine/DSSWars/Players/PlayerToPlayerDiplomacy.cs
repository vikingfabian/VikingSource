using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players
{
    class PlayerToPlayerDiplomacy
    {
        public int index;

        public bool suggestingNewRelation = false;
        public RelationType suggestedRelation;
        public int suggestedBy;

        public void writeGameState(BinaryWriter w)
        {          
            w.Write((short)suggestedRelation);
            w.Write((ushort)suggestedBy);
        }

        public void readGameState(BinaryReader r, int version)
        {
            suggestedRelation = (RelationType)r.ReadInt16();
            suggestedBy = r.ReadUInt16();
        }
    }
}
