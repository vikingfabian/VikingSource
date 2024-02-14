using VikingEngine.Sound;

namespace VikingEngine.PJ.CarBall
{
    class Sounds
    {
        public Sound.SoundSettings bounceSound = new Sound.SoundSettings(LoadedSound.bass_pluck, 0.1f);
        public Sound.SoundSettings flap = new Sound.SoundSettings(LoadedSound.flap, 0.4f, 1, 0.05f, 0);
       
        public SoundData
            airhorn,
            softBallBump,
            hardBallBump,
            ballPop,
            rcDrive,
            rcTireScretch,
            turn1, turn2,
            turnGoalie,
            winner,
            bassExplosion;

        public Sounds()
        {
            cballRef.sounds = this;

            airhorn = new SoundData(PjLib.SoundFolder + "airhorn", 2.5f);
            ballPop = new SoundData(PjLib.SoundFolder + "ballong pop", 3.5f);

            softBallBump = new SoundData(PjLib.SoundFolder + "Ballong bump soft", 1.2f);
            hardBallBump = new SoundData(PjLib.SoundFolder + "Ballong bump hard", 1.2f);
            rcDrive = new SoundData(PjLib.SoundFolder + "rc drive");
            rcTireScretch = new SoundData(PjLib.SoundFolder + "rc tire screetch", 1f, 0.06f);

            const float TurnVol = 0.8f;
            turn1 = new SoundData(PjLib.SoundFolder + "rc turn1", TurnVol);
            turn2 = new SoundData(PjLib.SoundFolder + "rc turn2", TurnVol);

            bassExplosion = new SoundData(PjLib.SoundFolder + "BassExplo");
            winner = new SoundData(PjLib.SoundFolder + "Heal");
            turnGoalie = new SoundData(PjLib.SoundFolder + "open_map");
        }
    }
}
