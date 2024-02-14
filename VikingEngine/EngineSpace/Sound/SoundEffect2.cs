using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.PJ;

namespace VikingEngine.Sound
{
    abstract class SoundContainerBase
    {
        protected float volume = 1;
        protected float randomPitch = 0;
        protected float pitchAdd = 0;

        abstract protected SoundEffect File();

        public void Play()
        {
            Play(Pan.Center);
        }

        public void Play(Pan pan)
        {
            float pitch = pitchAdd;
            if (randomPitch != 0)
            {
                pitch = Bound.Set(pitch + Ref.rnd.Plus_MinusF(randomPitch), -1, 1);
            }

            File().Play(Bound.Max(volume * Engine.Sound.SoundVolume, 1), pitch, pan.Value);
        }
    }

    class SoundContainerSingle: SoundContainerBase
    {
        SoundEffect file;

        public SoundContainerSingle(string filePath, float volume = 1, float randomPitch = 0, float pitchAdd = 0)
        {
            file = LoadContent.Content.Load<SoundEffect>(filePath);
            this.volume = volume;
            this.randomPitch = randomPitch;
            this.pitchAdd = pitchAdd;
        }

        protected override SoundEffect File()
        {
            return file;
        }
    }

    class SoundContainerMultiple: SoundContainerBase
    {
        SoundEffect[] files;

        public SoundContainerMultiple(string[] filePath, float volume = 1, float randomPitch = 0, float pitchAdd = 0)
        {
            files = new SoundEffect[files.Length];
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = LoadContent.Content.Load<SoundEffect>(filePath[i]);
            }
            this.volume = volume;
            this.randomPitch = randomPitch;
            this.pitchAdd = pitchAdd;
        }

        protected override SoundEffect File()
        {
            return files[Ref.rnd.Int(files.Length)];
        }
    }
}
