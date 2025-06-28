using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.XP
{
    struct ResearchBuilding
    {
        public int idAndPosition;
        public TechnologyTreeType assignedTech;
        public bool isResearchCenter;

        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(idAndPosition);
            w.Write((byte)assignedTech);
            w.Write(isResearchCenter);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            idAndPosition = r.ReadInt32();
            assignedTech = (TechnologyTreeType)r.ReadByte();
            isResearchCenter = r.ReadBoolean();
        }
    }
}
