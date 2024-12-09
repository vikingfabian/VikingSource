using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.LootFest.Data;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.XP
{
    struct SchoolStatus
    {
        public WorkExperienceType learnExperience;
        public TimeInGameCountdown countdown;
        public int idAndPosition;

        public void writeGameState(System.IO.BinaryWriter w)
        {
           
            w.Write(idAndPosition);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            idAndPosition = r.ReadInt32();
        }

        //public string shortActiveString()
        //{ }
    }
}
