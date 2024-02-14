using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using VikingEngine.Graphics;

namespace VikingEngine.Sound
{
    class SoundData
    {
        public SoundEffect soundeffect;
        public float volume;
        public float randomPitch;
        public float pitchAdd;

        public SoundData(string filepath, float volume = 1f,
            float randomPitch = 0, float pitchAdd = 0)
        {
            soundeffect = Engine.LoadContent.Content.Load<SoundEffect>(filepath);
            this.volume = volume;
            this.randomPitch = randomPitch;
            this.pitchAdd = pitchAdd;
        }

        public SoundData(SoundEffect soundeffect, float volume = 1f,
            float randomPitch = 0, float pitchAdd = 0)
        {
            this.soundeffect = soundeffect;
            this.volume = volume;
            this.randomPitch = randomPitch;
            this.pitchAdd = pitchAdd;
        }

        public SoundData Clone()
        {
            return new SoundData(soundeffect, volume, randomPitch, pitchAdd);
        }

        public SoundEffect PlayFlat(float volumeMultiplier = 1f)
        {
            return Engine.Sound.PlaySound(soundeffect, volume * volumeMultiplier, Pan.Center,
                Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }
        public SoundEffect Play(Vector2 screenPosition, float volumeMultiplier = 1f)
        {
            return Engine.Sound.PlaySound(soundeffect,
                volume * volumeMultiplier,
                Pan.PositionToPan(screenPosition.X, 0, Engine.Screen.Width),
                Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }

        public SoundEffectInstance PlayInstance(Vector2 screenPosition, bool looped = false, float volume = 1f, float pitch = 0)
        {
            var soundInstance = soundeffect.CreateInstance();
            Engine.Sound.PlayInstance(soundInstance, volume, Pan.PositionToPan(screenPosition.X, 0, Engine.Screen.Width), looped, pitch);
            
            return soundInstance;
        }

        public SoundEffect Play(Vector3 position, float volumeMultiplier = 1f)
        {
            FindMinValue distanceFinder = new FindMinValue(true);

            for (int i = 0; i < Ref.draw.ActivePlayerScreens.Count; i++)
            {
                AbsCamera cam = Ref.draw.ActivePlayerScreens[i].view.Camera;

                distanceFinder.Next(
                    lib.LargestValue(
                        Math.Abs(cam.LookTarget.X - position.X),
                        Math.Abs(cam.LookTarget.Z - position.Z)), i);

            }

            //const float MaxSoundDist = 30;
            if (distanceFinder.minValue < Ref.sound.Sound3DMaxLength)
            {
                float outvolume = Ref.sound.Sound3DVolumeMultiplier * volumeMultiplier * volume * (1f - (distanceFinder.minValue / Ref.sound.Sound3DMaxLength));
                AbsCamera cam = Ref.draw.ActivePlayerScreens[distanceFinder.minMemberIndex].view.Camera;
                Vector2 diff = new Vector2(position.X - cam.LookTarget.X, position.Z - cam.LookTarget.Z);
                Rotation1D dir = Rotation1D.FromDirection(diff);
                dir.Add(cam.TiltX - MathHelper.PiOver2);
                Vector2 direction = dir.Direction(diff.Length());

                return Engine.Sound.PlaySound(soundeffect, outvolume, new Pan(direction.X / Ref.sound.Sound3DMaxLength),
                    Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
            }

            return null;
        }
    }
}

