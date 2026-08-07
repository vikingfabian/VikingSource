using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Sound;

namespace VikingEngine.DSSWars.Data
{
    struct MessageTimer
    {
        public SoundContainerBase sound;
        public TimeStamp lastPlayed;

        public MessageTimer(SoundContainerBase sound)
        { 
            this.sound = sound;
            lastPlayed = TimeStamp.None;
        }
    }
}
