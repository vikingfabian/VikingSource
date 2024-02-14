using VikingEngine.Sound;

namespace VikingEngine.PJ.Match3
{
    class Sounds
    {
        public SoundData
            bassExplosion,
            fallClonk,
            clear,
            clearCombo,
            topMove,
            winner,
            rotate;

        public Sounds()
        {
            m3Ref.sounds = this;

            bassExplosion = new SoundData(PjLib.SoundFolder + "BassExplo");
            fallClonk = new SoundData(PjLib.SoundFolder + "Tetris fall clonk", 0.4f);
            clear = new SoundData(PjLib.SoundFolder + "Tetris success1");
            clearCombo = new SoundData(PjLib.SoundFolder + "Tetris success2");
            topMove = new SoundData(PjLib.SoundFolder + "Tetris top move", 0.2f);
            rotate = new SoundData(PjLib.SoundFolder + "Tetris turn block", 0.2f);
            winner = new SoundData(PjLib.SoundFolder + "Heal");
        }
    }
}
