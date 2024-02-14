using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2.Effects
{
    struct EffectLib
    {
        public const float Gravity = -0.0001f;
        public static readonly IntervalF StandardStartUpSpeed = IntervalF.FromRadius(0.008f, 0.012f);
        public static readonly IntervalF StandardStartSideSpeed = IntervalF.FromRadius( 0.006f, 0.02f);
        public static readonly IntervalF PickUpStartSideSpeed = IntervalF.FromRadius(0.01f, 0.016f);
        public const float StandardBounceDamp = -0.7f;
        public const int StandardNumBounces = 2;

        public static bool Use3dEffects = false;

        public static void VibrationCenter(Vector3 position, float strength, float time, float radius)
        {
            for (int i = 0; i < LfRef.LocalHeroes.Count; ++i)//foreach (GameObjects.Characters.Hero h in LfRef.LocalHeroes)
            {
                //const float Reduce = 0.025f;
                float l = (position - LfRef.LocalHeroes[i].Position).Length();
                if (l < radius)
                {
                    float percDistance = 1 - (l / radius);

                    strength = Bound.Min(strength * percDistance, 0.04f);
                    strength = lib.SetMaxFloatVal(strength, 1f);
                    LfRef.LocalHeroes[i].Player.localPData.Vibrate(time, strength, strength);

                    //if (strength >= 0.05f)
                    //{
                    //new Timer.Vibration(h.Player.Index, length, lib.SetMaxFloatVal(strength, 1), Pan.Center);
                    //}
                }
            }
        }
        public static void Force(ISpottedArrayCounter<GameObjects.AbsUpdateObj> localMembersCounter, Vector3 center, float force)
        {
            localMembersCounter.Reset();
            while (localMembersCounter.Next())
            {
                localMembersCounter.GetMember.Force(center, force);
            }
            //foreach (GameObjects.AbsUpdateObj obj in args.localMembersCounter)
            //{
            //    obj.Force(center, force);
            //}
        }
    }

    enum EffectNetType
    {
        BossKey,
        BossDeathItem,
        ChainLightning,
    }
}
