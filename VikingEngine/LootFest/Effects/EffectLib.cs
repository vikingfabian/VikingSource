using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest.Effects
{
    struct EffectLib
    {
        public const float Gravity = -0.006f;
     

        public static readonly IntervalF StandardStartUpSpeed = IntervalF.FromCenter(0.048f, 0.06f);
        public static readonly IntervalF StandardStartSideSpeed = IntervalF.FromCenter(0.04f, 0.1f);
        public static readonly IntervalF PickUpStartSideSpeed = IntervalF.FromCenter(0.048f, 0.07f);
        public const float StandardBounceDamp = -0.7f;
        public const int StandardNumBounces = 2;

       // public static bool Use3dEffects = false;

        public static void VibrationCenter(Vector3 position, float strength, float time, float radius)
        {
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                //const float Reduce = 0.025f;
                float l = (position - LfRef.LocalHeroes[i].Position).Length();
                if (l < radius)
                {
                    float percDistance = 1 - (l / radius);

                    strength = Bound.Min(strength * percDistance, 0.04f);
                    strength = Bound.Max(strength, 1f);
                    LfRef.LocalHeroes[i].player.localPData.inputMap.Vibrate(time, strength, strength);
                }
            }
        }

        public static void CameraShakeCenter(Vector3 position, float strength)
        {
            const float StrengthToShakeVal = 100f;
            const float Radius = 20;

            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)
            {
                //const float Reduce = 0.025f;
                float l = (position - LfRef.LocalHeroes[i].Position).Length();
                if (l < Radius)
                {
                    float percDistance = 1 - (l / Radius);

                    strength = Bound.Min(strength * percDistance, 0.1f);
                    strength = Bound.Max(strength, 1f);
                    LfRef.LocalHeroes[i].player.localPData.view.Camera.camShakeValue = StrengthToShakeVal * strength + 250;//.Vibrate(time, strength, strength);
                }
            }
        }

        public static void Force(ISpottedArrayCounter<GO.AbsUpdateObj> localMembersCounter, Vector3 center, float force)
        {
            localMembersCounter.Reset();
            while (localMembersCounter.Next())
            {
                localMembersCounter.GetSelection.Force(center, force);
            }
        }
        public static void Force(ISpottedArrayCounter<GO.AbsUpdateObj> localMembersCounter, GO.WeaponAttack.WeaponUserType targetCharacters,
            bool targetItems, Vector3 center, float force, float radius, float StunForce)
        {
            localMembersCounter.Reset();
            while (localMembersCounter.Next())
            {
                var m = localMembersCounter.GetSelection;
                bool push;
                if (m is GO.Characters.AbsCharacter)
                {
                    push = m.WeaponTargetType == targetCharacters;
                }
                else
                {
                    push = targetItems;
                }

                if (push && (m.Position - center).Length() <= radius)
                {
                    m.Force(center, force);
                    if (StunForce > 0)
                    {
                        m.stunForce(StunForce, LfLib.HeroStunDamage, false, true);
                    }
                }
            }
        }

        public static void DamageBlocks(int amount, Graphics.AbsVoxelObj model, Effects.BouncingBlockColors damageColors)
        {
            int numDummies = 0;
            int numBlocks = 0;

            if (Engine.Update.IsRunningSlow || Ref.gamesett.DetailLevel == 0)
            {
                numDummies = amount / 2;
            }
            else if (Ref.gamesett.DetailLevel == 1)
            {
                numDummies = amount / 2;
                numBlocks = amount - numDummies;
            }
            else  //DetailLevel == 2
            {
                numBlocks = amount;
            }


            Vector3 pos = model.position;
            pos.Y += model.scale.Y * 8f;
            float scale = lib.SmallestValue(model.Scale1D * 1.6f, 3f);
            for (int i = 0; i < numBlocks; i++)
            {
                new Effects.BouncingBlock2(pos, damageColors.GetRandom(), scale);
            }
            for (int i = 0; i < numDummies; i++)
            {
                new Effects.BouncingBlock2Dummie(pos, damageColors.GetRandom(), scale);
            }
        }
    }

    enum EffectNetType
    {
        BossKey,
        BossDeathItem,
        ChainLightning,
    }
}
