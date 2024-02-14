using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace VikingEngine.Sound
{
    class SoundManager
    {
        public float Sound3DMaxLength = 30;
        public float Sound3DVolumeMultiplier = 1f;

        public SoundManager()
        {
            Ref.sound = this;
        }
    }

    struct SoundSettings
    {
        public float volume;
        public int variations;
        public float randomPitch;
        public float pitchAdd;
        public LoadedSound sound;

        public SoundSettings(LoadedSound sound)
            :this(sound, 1)
        {
        }

        public SoundSettings(LoadedSound sound, float volume)
            :this(sound, volume, 1, 0, 0)
        {
        }

        public SoundSettings(LoadedSound sound, float volume, int variations)
            : this(sound, volume, variations, 0, 0)
        {
        }

        public SoundSettings(LoadedSound sound, float volume, int variations,
            float randomPitch, float pitchAdd)
        {
            this.sound = sound;
            this.volume = volume;
            this.variations = variations;
            this.randomPitch = randomPitch;
            this.pitchAdd = pitchAdd;
        }

        public SoundEffect PlayFlat(float volumeMultiplier = 1f)
        {
            return Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), volume * volumeMultiplier, Pan.Center, 
                Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }
        public SoundEffect Play(Vector2 screenPosition, float volumeMultiplier = 1f)
        {
            return Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), 
                volume * volumeMultiplier, 
                Pan.PositionToPan(screenPosition.X, 0, Engine.Screen.Width),
                Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }

        //public SoundEffectInstance GetLoopedPlayingInstance()
        //{
        //    return Engine.Sound.PlayLoopedSound(sound, volume);
        //}

        public SoundEffect Play(Vector3 position, float volumeMultiplier = 1f)
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

            //const float MaxSoundDist = 30;
            if (distanceFinder.minValue < Ref.sound.Sound3DMaxLength)
            {
                float outvolume = Ref.sound.Sound3DVolumeMultiplier * volumeMultiplier * volume * (1f - (distanceFinder.minValue / Ref.sound.Sound3DMaxLength));
                Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[distanceFinder.minMemberIndex].view.Camera;
                Vector2 diff = new Vector2(position.X - cam.LookTarget.X, position.Z - cam.LookTarget.Z);
                Rotation1D dir = Rotation1D.FromDirection(diff);
                dir.Add(cam.TiltX - MathHelper.PiOver2);
                Vector2 direction = dir.Direction(diff.Length());

                return Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), outvolume, new Pan(direction.X / Ref.sound.Sound3DMaxLength),
                    Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
            }

            return null;
        }
    }
    
}
