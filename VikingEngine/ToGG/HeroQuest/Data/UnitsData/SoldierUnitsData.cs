using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.HeroQuest.QueAction;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class KingsGuardInfantry : HqUnitData
    {
        public KingsGuardInfantry()
            : base()
        {
            move = 3;
            wep.meleeStrength = 4;
            startHealth = 10;

            defence.set(hqLib.ArmorDie);
            properties.Add(new AutoAttack());

            modelSettings.image = SpriteName.cmdRoyalGuardMelee;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.9f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.KingsGuardSpearman;
        public override string Name => "Royal guard spearman";
    }

    class KingsGuardArcher : HqUnitData
    {
        public KingsGuardArcher()
            : base()
        {
            move = 4;
            wep.meleeStrength = 1;
            wep.projectileStrength = 3;
            wep.projectileRange = 4;

            startHealth = 7;

            defence.set(hqLib.DodgeDie); 
            properties.Add(new AutoAttack());

            modelSettings.image = SpriteName.cmdRoyalGuardArcher;
            modelSettings.facingRight = true;
            modelSettings.modelScale = 0.9f;
            modelSettings.shadowOffset = -0.05f;
        }

        public override HqUnitType Type => HqUnitType.KingsGuardArcher;
        public override string Name => "Royal guard archer";
    }

    class AutoAttack : AbsUnitProperty
    {
        public override string Name => "Auto attack";

        public override UnitPropertyType Type => UnitPropertyType.AutoAttack;

        public override string Desc => LanguageLib.AtTurnEndDescStart + 
            "Quick attack the nearest opponent.";

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (hqRef.netManager.host && 
                eventType == EventType.TurnEnd)
            {
                new UnitPropertyQueAction(this, 0, parentUnit);
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override void QuedAction(int actionId, bool local, AbsUnit parentUnit)
        {
            var target = Ai.HasTargetFromPos(parentUnit, parentUnit.squarePos,
                   AttackTarget.Conv(hqRef.players.CollectEnemyUnits(parentUnit.Player)));

            if (target != null)
            {
                BellValue damVal = new BellValue(
                    parentUnit.Data.WeaponStats.Strength(target.attackType.IsMelee));

                Damage damage = new Damage(damVal.Next(parentUnit.hq().PlayerHQ.Dice));

                var damageAction = new PassiveSkillDamageUnit(parentUnit.hq(), target.unit.hq(), damage);
                damageAction.setNextInQue();
            }
        }
    }
}
