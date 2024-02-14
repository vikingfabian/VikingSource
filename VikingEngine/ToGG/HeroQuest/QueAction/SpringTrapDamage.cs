using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{
    class SpringTrapDamage : ToggEngine.QueAction.AbsQueAction
    {
        Unit defender;
        Damage damage;

        RecordedDamageEvent rec;

        public SpringTrapDamage(Unit defender, Damage damage)
            : base()
        {
            this.defender = defender;
            this.damage = damage;
        }

        public SpringTrapDamage(BinaryReader r)
            : base(r)
        {
        }

        protected override void netWrite(System.IO.BinaryWriter w)
        {
            base.netWrite(w);

            rec.write(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            rec = new RecordedDamageEvent(r);
            defender = rec.reciever;
        }

        public override void onBegin()
        {
            viewTime.Seconds = 1f;

            if (isLocalAction)
            {
                defender.TakeDamage(damage, DamageAnimationType.AttackSlash, null, out rec);
            }
            else
            {
                rec.apply();
            }
        }
        
        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            camTarget = defender.squarePos;
            inCamCheck = false;

            return true;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.SpringTrapDamage; } }
    }
}
