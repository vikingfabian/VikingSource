using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Data
{
    /// <summary>
    /// Track what happened during a game
    /// </summary>
    class Progress
    {        
       
        bool archerCultureBuild = false;
        bool warriorCultureBuild = false;
        int decorBuilt = 0;
        int statuesBuilt = 0;

        public void onDecorBuild_async(bool statue)
        { 
            decorBuilt++;
            if (statue)
            {
                DssRef.achieve.UnlockAchievement_async(AchievementIndex.statue);
                statuesBuilt++;
            }

            if (decorBuilt >= Achievements.DecorationsTotalCount &&
                statuesBuilt >= Achievements.DecorationsStatueCount)
            {
                DssRef.achieve.UnlockAchievement_async(AchievementIndex.decorations);
            }
        }

        public void onCultureBuild(bool archer)
        {
            if (archer)
            {
                archerCultureBuild = true;
            }
            else
            { 
                warriorCultureBuild = true;
            }

            if (archerCultureBuild && warriorCultureBuild)
            {
                DssRef.achieve.UnlockAchievement_async(AchievementIndex.soldier_culture);
            }
        }



        public void writeGameState(System.IO.BinaryWriter w)
        {
            w.Write((ushort)decorBuilt);
            w.Write((ushort)statuesBuilt);

            w.Write(archerCultureBuild);
            w.Write(warriorCultureBuild);
        }
        public void readGameState(System.IO.BinaryReader r, int subversion, ObjectPointerCollection pointers)
        {
            decorBuilt = r.ReadUInt16();
            statuesBuilt = r.ReadUInt16();

            archerCultureBuild = r.ReadBoolean();
            warriorCultureBuild = r.ReadBoolean();
        }
    }
}
