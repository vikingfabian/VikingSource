using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.DSSWars.Players.PlayerControls
{
    struct TutorialProgressPoint
    {
        public bool completed = false;
        public bool playedSound = false;
        public bool returnToUncomplete;

        public TutorialProgressPoint(bool returnToUncomplete)
        {
            this.returnToUncomplete = returnToUncomplete;
        }

        public bool NeedUpdate => !completed || returnToUncomplete;
    }
}
