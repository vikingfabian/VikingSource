using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VikingEngine.LF2
{
    class PushForce : AbsUpdateable
    {
        const float PushTime = 200;
        const float FadeTime = 80;
        const float TotalTime = PushTime + FadeTime;
        Vector2 force;
        float timeGone = 0;
        LF2.AbsPhysics objPhys;

        public PushForce(GameObjects.WeaponAttack.DamageData damage, AbsPhysics physics)
            :base(true)
        {
            this.objPhys = physics;
            objPhys.AddForce(this);
            float power;
            switch (damage.Push)
            {
                case GameObjects.WeaponAttack.WeaponPush.Small:
                    power = 0.3f;
                    break;
                default:
                    power = 0.6f;
                    break;
                case GameObjects.WeaponAttack.WeaponPush.Large:
                    power = 0.8f;
                    break;
                case GameObjects.WeaponAttack.WeaponPush.Huge:
                    power = 1f;
                    break;
                case GameObjects.WeaponAttack.WeaponPush.GoFlying:
                    power = 2f;
                    break;
            }

            force = damage.PushDir.Direction(power);
        }
        public override void Time_Update(float time)
        {
            this.timeGone += time;

            if (timeGone < PushTime)
            {
                objPhys.PushForce(force);
            }
            else if (timeGone < TotalTime)
            {
               objPhys.PushForce(force * (1 - (timeGone - PushTime) / PushTime));
            }
            else
            {
                DeleteMe();
            }
        }
    }
}
