using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.Engine;
using VikingEngine.Graphics;


namespace VikingEngine.LF2.Music
{
   
    static class SoundManager
    {
        static readonly IntervalF EnemySoundFreq = new IntervalF(6, 16);
        public static float EnemySoundTimer = 0;

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
                    volume = 0.14f;
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
                    volume = 0.3f;
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
                //case LoadedSound.platform_jump:
                //    randomPitch = 0.02f;
                //    break;
                //case LoadedSound.platform_tramplione:
                //    sound = LoadedSound.platform_jump;
                //    pitchAdd = 0.2f;
                //    break;
                
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
#if CMODE
                case LoadedSound.zombie_hurt_1:
                    variations = 4;
                    break;
                case LoadedSound.zombie_moan_1:
                    variations = 4;
                    break;
                case LoadedSound.zombie_6_a:
                    variations = 5;//baby scream
                    break;
                case LoadedSound.zombie_6_e:
                    variations = 2;//baby hurt
                    break;
                case LoadedSound.zombie_2_e:
                    variations = 2;//harpy & dog hurt
                    break;
                case LoadedSound.zombie_3_a:
                    variations = 7;//leader scream
                    break;
                case LoadedSound.zombie_3_h:
                    variations = 2;//leader hurt
                    break;
                case LoadedSound.zombie_fatty_moan1:
                    variations = 4;
                    break;
                case LoadedSound.fatty_explode:
                    volume = 2f;
                    break;
#endif
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
                //case LoadedSound.ogre_hurt1:
                //    variations = 2;
                //    volume = 2f;
                //    break;
                //case LoadedSound.terrain_destruct1:
                //    variations = 2;
                //    volume = 0.9f;
                //    break;
                //case LoadedSound.EnemySound1:
                //    if (EnemySoundTimer <= 0)
                //    {
                //        randomPitch = 0.2f;
                //        variations = 4;
                //        volume = 0.4f;
                //        EnemySoundTimer = EnemySoundFreq.GetRandom();
                //    }
                //    else
                //        return;
                //    break;
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
                case LoadedSound.barrel_explo:
                    randomPitch = 0.2f;
                    break;
                
            }

            LowestValue distance = new LowestValue(true);

            for (int i = 0; i < Ref.draw.ActivePlayerScreens.Count; i++)
            {
                Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[i].view.Camera;
                distance.Next( lib.LargestOfTwoValues(Math.Abs(cam.Target.X - position.X),
                    Math.Abs(cam.Target.Z - position.Z)), i);
            }
            const float MaxSoundDist = 30;
            if (distance.Lowest < MaxSoundDist)
            {
                volume *= 1 - (distance.Lowest / MaxSoundDist);
                Graphics.AbsCamera cam = Ref.draw.ActivePlayerScreens[distance.LowestMemberIndex].view.Camera;
                Vector2 diff = new Vector2(cam.Target.X - position.X, cam.Target.Z - position.Z);
                Rotation1D dir = Rotation1D.FromDirection(diff);
                dir.Add(cam.TiltX - MathHelper.PiOver2);
                Vector2 direction = dir.Direction(diff.Length());
                Engine.Sound.PlaySound(sound + Ref.rnd.Int(variations), volume, new Pan(direction.X/ MaxSoundDist), 
                    Ref.rnd.Plus_MinusF(randomPitch));
            }
        }
    }
}
