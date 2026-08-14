using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players.Profile
{
    struct FactionTypeData
    {
        public AiConscript aiConscript;
        public string name;
        public bool mayAttackPlayer;
        public DiplomaticSide diplomaticSide;
        public int aggressionLevel;
        public float growthMultiplier;
        public bool storyProtectedFaction;
        public bool viewOnLargeMap;
        public bool personality_loner;

        public static FactionTypeData Default = new FactionTypeData()
        { 
            aiConscript = AiConscript.Default,
            diplomaticSide = DiplomaticSide.None,
            aggressionLevel = AbsPlayer.AggressionLevel0_Passive,
            growthMultiplier = 1f,

        };
    }
}
