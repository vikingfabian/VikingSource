using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.PJ;

namespace VikingEngine.Sound
{
    static class SoundStackManager
    {
        const float StackTimeRamgeMs = 300;
        const int MaxSoundStack = 2;

        static float time = 0;
        static int stack = 0;

        public static void Update()
        {
            time += Ref.DeltaTimeMs;
            if (time > StackTimeRamgeMs)
            {
                time = 0;
                stack = 0;
            }
        }

        public static bool Available()
        { 
            return stack++ <= MaxSoundStack;
        }
    }


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
        public void Play(Vector3 position)
        {
            if (SoundStackManager.Available())
            {
                FindMinValue distanceFinder = new FindMinValue(true);

                for (int i = 0; i < Ref.draw.ActivePlayerScreens.Count; i++)
                {
                    Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[i].view.Camera;

                    distanceFinder.Next(
                        lib.LargestValue(
                            Math.Abs(cam.LookTarget.X - position.X),
                            Math.Abs(cam.LookTarget.Z - position.Z)), i);

                }

                const float MaxSoundDist = 4;
                if (distanceFinder.minValue < MaxSoundDist)
                {
                    float outvolume = volume * (1f - (distanceFinder.minValue / MaxSoundDist));
                    Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[distanceFinder.minMemberIndex].view.Camera;
                    Vector2 diff = new Vector2(position.X - cam.LookTarget.X, position.Z - cam.LookTarget.Z);
                    Rotation1D dir = Rotation1D.FromDirection(diff);
                    dir.Add(cam.TiltX - MathHelper.PiOver2);
                    Vector2 direction = dir.Direction(diff.Length());

                    //float pan = direction.X / MaxSoundDist * Ref.gamesett.reversedStereoValue;
                    float pan = Bound.Set(direction.X / MaxSoundDist, -1, 1) * Ref.gamesett.reversedStereoValue;

                    float pitch = pitchAdd;
                    if (randomPitch != 0)
                    {
                        pitch = Bound.Set(pitch + Ref.rnd.Plus_MinusF(randomPitch), -1, 1);
                    }

                    File().Play(Bound.Max(outvolume * Ref.gamesett.SoundVol(), 1), pitch, pan);
                }
            }
        }
        
        public void Play(Pan pan)
        {
            float pitch = pitchAdd;
            if (randomPitch != 0)
            {
                pitch = Bound.Set(pitch + Ref.rnd.Plus_MinusF(randomPitch), -1, 1);
            }

            File().Play(Bound.Max(volume * Ref.gamesett.SoundVol(), 1), pitch, pan.Value);
            
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
            files = new SoundEffect[filePath.Length];
            for (int i = 0; i < filePath.Length; i++)
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

    struct LoopingSoundData
    { 
        public string filePath;
        public float basevolume;
    }

    class LoopingSound
    {
        SoundEffect file;
        SoundEffectInstance ins;
        float basevolume = 1f;
        float volume = 1f;
        public void Play()
        {
            ins = file.CreateInstance();
            ins.IsLooped = true;
            ins.Volume = basevolume * volume;
            ins.Play();
        }

        public void setVolume(float volume)
        {
            this.volume = volume;
            if (ins != null)
            {
                ins.Volume = basevolume * volume;
            }
        }
    }
}
