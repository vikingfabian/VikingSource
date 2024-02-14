using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.Sound;

namespace VikingEngine.PJ.Joust
{
    class Sounds
    {
        public Sound.SoundSettings airTrick = new Sound.SoundSettings(LoadedSound.flap, 0.3f, 1, 0.05f, 0.5f);
        public Sound.SoundSettings diveBomb = new Sound.SoundSettings(LoadedSound.flowerfire, 1.6f, 0, 0.05f, -0.3f);
        public Sound.SoundSettings bounceSound = new Sound.SoundSettings(LoadedSound.bass_pluck, 0.12f);

        public SoundData shieldPop;

        public Sounds()
        {
            JoustRef.sounds = this;

            shieldPop = new SoundData(PjLib.SoundFolder + "deathpop", 0.6f);
        }
    }
}
