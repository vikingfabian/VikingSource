using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LootFest
{
    class PushForce : AbsUpdateable
    {
        const float PushTime = 200;
        const float FadeTime = 80;
        const float TotalTime = PushTime + FadeTime;
        Vector2 forceVector;
        float timeGone = 0;
        LootFest.AbsPhysics objPhys;
        const float ForceMultiplier = 0.2f;

        public PushForce(GO.WeaponAttack.DamageData damage, AbsPhysics physics)
            : base(true)
        {
            basicInit(physics);
            float power;
            switch (damage.Push)
            {
                case GO.WeaponAttack.WeaponPush.Small:
                    power = 0.3f;
                    break;
                default:
                    power = 0.6f;
                    break;
                case GO.WeaponAttack.WeaponPush.Large:
                    power = 0.8f;
                    break;
                case GO.WeaponAttack.WeaponPush.Huge:
                    power = 1f;
                    break;
                case GO.WeaponAttack.WeaponPush.GoFlying:
                    power = 2f;
                    break;
            }

            forceVector = damage.PushDir.Direction(power * ForceMultiplier);
        }

        public PushForce(float power, Vector2 dir, AbsPhysics physics)
            :base(true)
        {
            basicInit(physics);

            dir.Normalize();
            forceVector = dir * (power * ForceMultiplier);
        }

        void basicInit(AbsPhysics physics)
        {
            if (physics.parent == null)
            { throw new NullReferenceException("PushForce: " + physics.ToString()); }

            this.objPhys = physics;
            objPhys.AddForce(this);
        }

        public override void Time_Update(float time)
        {
            this.timeGone += time;

            if (timeGone < PushTime)
            {
                objPhys.PushForce(forceVector);
            }
            else if (timeGone < TotalTime)
            {
               objPhys.PushForce(forceVector * (1 - (timeGone - PushTime) / PushTime));
            }
            else
            {
                DeleteMe();
            }
        }
    }
}
