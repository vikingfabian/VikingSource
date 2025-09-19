using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Conscript;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Work;
using VikingEngine.LootFest.Data;
using VikingEngine.ToGG.ToggEngine.QueAction;

namespace VikingEngine.DSSWars.XP
{
    

    struct SchoolStatus
    {
        public const ExperienceLevel MaxLevel = ExperienceLevel.Expert_3;
        public const int MaxQue = 4;

        public WorkExperienceType learnExperience;
        public ExperienceLevel toLevel;
        public TimeInGameCountdown countdown;
        public int idAndPosition;
        public int que;

        public void copyFrom(SchoolStatus template)
        {
            learnExperience = template.learnExperience;
            toLevel = template.toLevel;
        }
        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write(idAndPosition);
            w.Write((byte)learnExperience);
            w.Write((byte)toLevel);
            w.Write((byte)que);
        }

        public void readGameState(System.IO.BinaryReader r, int subVersion)
        {
            idAndPosition = r.ReadInt32();
            learnExperience = (WorkExperienceType)r.ReadByte();
            toLevel = (ExperienceLevel)r.ReadByte();
            que = r.ReadByte();
        }

        public void defaulSetup()
        {
            toLevel = ExperienceLevel.Practitioner_2;
        }

        public string shortActiveString(City city)
        {
            string result = null;

            bool active = city.workerInSchoolCheckup(idAndPosition, out float time);

            if (active)
            {
                countdown.endTimeSec = time;
                result = string.Format(DssRef.lang.Conscription_Status_Training, countdown.RemainingLength().ShortString());
            }
            else
            {
                result = DssRef.lang.Hud_Idle;
            }

            return result;
        }

        
        //public string shortActiveString()
        //{ }
    }
}
