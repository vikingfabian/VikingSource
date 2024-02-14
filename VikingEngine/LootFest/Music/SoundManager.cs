using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LootFest.Music
{
   
    static class SoundManager
    {
        
        //static readonly IntervalF EnemySoundFreq = new IntervalF(6, 16);
        //public static float EnemySoundTimer = 0;
        public static Time weaponClinkTimer = 0;

        public static void WeaponClink(Vector3 pos)
        {
            if (weaponClinkTimer.TimeOut)
            {
                weaponClinkTimer.Seconds = 0.4f;
                PlaySound(LoadedSound.weaponclink, pos);
            }
        }

        public static void PlayFlatSound(LoadedSound sound)
        {
            float volume = 1;
            int variations = 1;
            float randomPitch = 0;
            float pitchAdd = 0;

            switch (sound)
            {
                case LoadedSound.NON:
                    return;
                case LoadedSound.MenuMove:
                    volume = 0.16f;
                    //randomPitch = 0.02f;
                    break;
                case LoadedSound.MenuSelect:
                    volume = 0.11f;
                    break;
                case LoadedSound.Coin1:
                    variations = 3;
                    volume = 0.6f;
                    break;
                case LoadedSound.open_map:
                    volume = 0.6f;
                    break;
                case LoadedSound.HealthUp:
                    volume = 0.8f;
                    break;
                case LoadedSound.buy:
                    volume = 0.8f;
                    break;
                case LoadedSound.PickUp:
                    volume = 0.4f;
                    break;
                case LoadedSound.MenuBack:
                    volume = 0.11f;
                    break;
                case LoadedSound.Dialogue_DidYouKnow:
                    volume = 0.6f;
                    break;
                case LoadedSound.Dialogue_Neutral:
                    volume = 0.6f;
                    break;
                case LoadedSound.Dialogue_QuestAccomplished:
                    volume = 0.6f;
                    break;
                case LoadedSound.Dialogue_Question:
                    volume = 0.6f;
                    break;
                case LoadedSound.out_of_ammo:
                    volume = 1.6f;
                    break;
                
            }
            Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), volume, Pan.Center, 
                Ref.rnd.Plus_MinusF(randomPitch) + pitchAdd);
        }


        public static void PlaySound(LoadedSound sound, Vector3 position)
        {

            float volume = 1;
            float randomPitch = 0;
            int variations = 1;

            switch (sound)
            {
                case LoadedSound.NON:
                    return;
                case LoadedSound.Sword1:
                    variations = 3;
                    break;
                case LoadedSound.MonsterHit1:
                    variations = 2;
                    volume = 0.9f;
                    break;
                case LoadedSound.express_hi1:
                    variations = 3;
                    break;
                case LoadedSound.express_teasing1:
                    variations = 2;
                    break;
                case LoadedSound.express_thumbup1:
                    variations = 2;
                    break;
                case LoadedSound.TakeDamage1:
                    variations = 2;
                    break;
                case LoadedSound.Bow:
                    volume = 2;
                    break;
                case LoadedSound.shieldcrash:
                    volume = 3.5f;
                    break;
                case LoadedSound.deathpop:
                    randomPitch = 0.2f;
                    volume = 1.2f;
                    break;
                case LoadedSound.SmallExplosion:
                    randomPitch = 0.2f;
                    break;
                
            }

            FindMinValue distance = new FindMinValue(true);

            for (int i = 0; i < Ref.draw.ActivePlayerScreens.Count; i++)
            {
                Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[i].view.Camera;
                distance.Next( 
                    lib.LargestAbsoluteValue(cam.LookTarget.X - position.X,
                    cam.LookTarget.Z - position.Z), i);
            }
            const float MaxSoundDist = 30;
            if (distance.AbsMinValue < MaxSoundDist)
            {
                volume *= 1 - (distance.AbsMinValue / MaxSoundDist);
                Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[distance.minMemberIndex].view.Camera;
                Vector2 diff = new Vector2(cam.LookTarget.X - position.X, cam.LookTarget.Z - position.Z);
                Rotation1D dir = Rotation1D.FromDirection(diff);
                dir.Add(cam.TiltX - MathHelper.PiOver2);
                Vector2 direction = dir.Direction(diff.Length());
                Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), volume, new Pan(direction.X/ MaxSoundDist), 
                    Ref.rnd.Plus_MinusF(randomPitch));
            }
        }
    }
}
