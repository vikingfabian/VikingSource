using VikingEngine.Sound;

namespace VikingEngine.PJ.MiniGolf
{
    class Sounds
    {
        public Sound.SoundSettings launchSound = new Sound.SoundSettings(LoadedSound.flowerfire, 1.6f, 0, 0.05f, 0);
        public Sound.SoundSettings coin = new Sound.SoundSettings(LoadedSound.Coin1, 0.1f, 3, 0.05f, 0);
        public Sound.SoundSettings fireKeydown = new Sound.SoundSettings(LoadedSound.flap, 0.2f, 1, 0.05f, 0.8f);
        public Sound.SoundSettings bumpBall = new Sound.SoundSettings(LoadedSound.flap, 0.4f, 1, 0.05f, 0);

        public SoundData
            hit,
            holeAppear,
            holeDrop,
            success,
            load,
            sand,
            sandShot,
            //tone250hz,
            bassExplosion,
            softExplosion;

        public Sounds()
        {
            GolfRef.sounds = this;

            hit = new SoundData(PjLib.SoundFolder + "golf hit");
            holeAppear = new SoundData(PjLib.SoundFolder + "golf hole appear");
            holeDrop = new SoundData(PjLib.SoundFolder + "golf hole", 3f);
            load = new SoundData(PjLib.SoundFolder + "golf load");
            success = new SoundData(PjLib.SoundFolder + "Heal");
            //tone250hz = new SoundData(PjLib.SoundFolder + "Tone250Hz");
            softExplosion = new SoundData(PjLib.SoundFolder + "SoftExplo", 0.3f);
            bassExplosion = new SoundData(PjLib.SoundFolder + "BassExplo");
            sand = new SoundData(PjLib.SoundFolder + "sand", 0.6f, 0.1f);
            sandShot = sand.Clone(); sandShot.pitchAdd = 0.2f;
        }
    }
}
