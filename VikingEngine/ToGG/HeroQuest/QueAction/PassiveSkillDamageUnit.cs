using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VikingEngine.ToGG.HeroQuest.QueAction
{    
    class PassiveSkillDamageUnit : ToggEngine.QueAction.AbsQueAction
    {
        Unit attacker, defender;
        Damage damage;

        RecordedDamageEvent rec;

        public PassiveSkillDamageUnit(Unit attacker, Unit defender, Damage damage)
            :base()
        {
            this.attacker = attacker;
            this.defender = defender;
            this.damage = damage;
        }

        public PassiveSkillDamageUnit(BinaryReader r)
            : base(r)
        { 
        }

        protected override void netWrite(System.IO.BinaryWriter w)
        {
            base.netWrite(w);

            attacker.netWriteUnitId(w);
            rec.write(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            attacker = Unit.NetReadUnitId(r);
            rec = new RecordedDamageEvent(r);
            defender = rec.reciever;
        }

        public override void onBegin()
        {
            viewTime.Seconds = 2f;

            if (defender != null)
            {
                new CloseCombatEffect(attacker.squarePos, defender.squarePos);
                if (isLocalAction)
                {
                    defender.TakeDamage(damage, DamageAnimationType.AttackSlash, null, out rec);
                }
                else
                {
                    rec.apply();
                }
            }
        }

        public override bool CameraTarget(out IntVector2 camTarget, out bool inCamCheck)
        {
            inCamCheck = false;

            if (defender != null)
            {
                camTarget = defender.squarePos;
                return true;
            }

            camTarget = IntVector2.NegativeOne;
            return false;
        }

        override public ToggEngine.QueAction.QueActionType Type { get { return ToggEngine.QueAction.QueActionType.PassiveSkill_DamageUnit; } }

        public override bool IsTopPrio => false;
    }
}
