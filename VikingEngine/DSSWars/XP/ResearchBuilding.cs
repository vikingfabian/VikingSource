using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;

namespace VikingEngine.DSSWars.XP
{
    struct ResearchBuilding
    {
        public int idAndPosition;
        public TechnologyTreeType assignedTech;
        public bool isResearchCenter;


        public string assignmentString()
        {
            if (assignedTech == TechnologyTreeType.NUM_NONE)
            {
                return DssRef.lang.Hud_NeedToBeAssigned;
            }
            else
            {
                LangLib.Technology(assignedTech, out SpriteName icon, out string name);
                return name;
            }
        }
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
