using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace VikingEngine.LootFest.Effects
{
    /// <summary>
    /// Rain coins out of the hands
    /// </summary>
    class CoinExpression : AbsInGameUpdateable
    {
        VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero;
        Time lifeTime = new Time(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero.ExpressionTime * 0.8f);

        Time nextCoin = 0;
        Sound.SoundSettings sound;

        public CoinExpression(VikingEngine.LootFest.GO.PlayerCharacter.AbsHero hero)
            :base(true)
        {
            this.hero = hero;
            sound = SoundLib.CoinSound;
            sound.volume *= 0.6f;
        }

        public override void Time_Update(float time_ms)
        {
            if (nextCoin.CountDown())
            {
                nextCoin.MilliSeconds = Ref.rnd.Float(40, 120);

                Vector3 pos = hero.Position;

                if (Ref.gamesett.DetailLevel >= 1)
                {
                    pos += VectorExt.V2toV3XZ(hero.Rotation.Direction(0.8f), 1f);
                    new Coin(pos, hero.Rotation);
                }

                sound.Play(pos);
                //Music.SoundManager.PlaySound(LoadedSound.Coin1, pos);
            }

            if (lifeTime.CountDown())
            {
                DeleteMe();
            }
        }

        class Coin : AbsUpdateable
        {
            const float Gravity = EffectLib.Gravity;

            Graphics.AbsVoxelObj model;
            Velocity v;
            Vector3 rotateSpeed;

            public Coin(Vector3 startPos, Rotation1D heroDir)
                : base(true)
            {
                model = LfRef.modelLoad.AutoLoadModelInstance(VoxelModelName.Coin, 0.1f, 0, true, true);
                model.position = startPos;
                model.Rotation = RotationQuarterion.Random;
                heroDir.Add(Ref.rnd.Plus_MinusF(2f));
                v.Set(heroDir, Ref.rnd.Float(0.005f, 0.01f));
                v.Y = Ref.rnd.Float(0.002f, 0.01f);

                rotateSpeed = Ref.rnd.Vector3_Sq(new Vector3(0.01f));
            }

            public override void Time_Update(float time_ms)
            {
                if (Ref.TimePassed16ms)
                {
                    v.Y += Gravity;
                }

                model.position += v.Value * time_ms;
                model.Rotation.RotateWorld(rotateSpeed * time_ms);

                if (model.Size1D < 0.6f)
                {
                    model.Scale1D += 4f * Ref.DeltaTimeSec;
                }

                if (model.position.Y < 0)
                {
                    DeleteMe();
                }
            }

            public override void DeleteMe()
            {
                base.DeleteMe();
                model.DeleteMe();
            }
        }
    }

   
}
