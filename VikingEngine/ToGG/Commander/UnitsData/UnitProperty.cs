using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.UnitsData
{
    class Aim : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Aim;

        public override string Desc => "+" + toggLib.AimPropertyBonus.ToString() + " ranged attack bonus if the unit don't move";
    }

    class Block : AbsUnitProperty
    {
        public const string Block1Desc = "Will ignore one hit from a combat roll";
        public override UnitPropertyType Type => UnitPropertyType.Block;

        public override string Desc => Block1Desc;
    }

    class Arrow_Block : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Arrow_Block;

        public override string Desc => "Will ignore one hit from a ranged attack.";
    }
    class Flank_support : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Flank_support;

        public override string Desc => "Has +2 support bonus in close combat";
    }
    class Light : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Light;

        public override string Desc => "Lower hit chance in close combat. Can retreat through blocking terrain.";
    }

    class Over_shoulder : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Over_shoulder;

        public override string Desc => "Will support a unit in close combat if standing adjacent to it. Can shoot through adjacent friendly units.";
    }

    class Base : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Base;

        public override string Desc => "Destroying your opponents base will give you " + toggLib.VP_DestroyEnemyBase.ToString() + "VP";
    }

    class Leader : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Leader;

        public override string Desc => "Adjacent friendly units will ignore one retreat and gains +" + TextLib.PercentText(toggLib.LeaderPropertyRetreatBonus) + " chance to force opponent retreat";
    }

    class Valuable : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Valuable;

        public override string Desc => "Destroying a valuable unit will give the opponent " + toggLib.VP_DestroyValuableUnit.ToString() + "VP";
    }

    class Expendable : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Expendable;

        public override string Desc => "Will not give any Victory Points when destroyed";
    }

    class Shield_dash : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Shield_dash;

        public override string Desc => "Plus " + TextLib.PercentText(toggLib.ShieldDashPropertyRetreatBonus) + " chance to force opponent retreat";
    }

    class Static_target : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Static_target;

        public override string Desc => "A static unit can't defend itself, 100% hit chance";
    }

    class Charge : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Charge;

        public override string Desc => "+1 attack strength if the unit has moved during the same turn";
    }

    class Frenzy : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Frenzy;

        public override string Desc => "One bonus attack if the unit makes a follow up move";
    }

    class Slippery : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Slippery;

        public override string Desc => "The unit will retreat away from hits";
    }

    class Ignore_terrain : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Ignore_terrain;

        public override string Desc => "Ignores terrain that has \"Must stop\"";
    }

    class Cant_retreat : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Cant_retreat;

        public override string Desc => "Unit wont retreat and will take a hit instead";
    }

    class Fear : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Fear;

        public override string Desc => "Swaps hit chance with retreat chance";
    }


    class Fear_support : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Fear_support;

        public override string Desc => "Supported attacks will gain +" + TextLib.PercentText(toggLib.FearSupportPropertyRetreatBonus) + " chance to force retreats";
    }

    class Necromancer : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Necromancer;

        public override string Desc => "(Under Construction!) Units that die adjacent to a necromancer will be raised as Undead";
    }


    class Sucide_attack : AbsUnitProperty
    {
        

        public override UnitPropertyType Type => UnitPropertyType.Sucide_attack;

        public override string Desc => "Will gain \"Open taget\" condition after attacking"; //Open target efter attack

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.AttackComplete)
            {
                parentUnit.cmd().AddCondition(new OpenTargetCondition());
            }
        }

        public override AbsExtToolTip[] ExtendedTooltipKeyWords()
        {
            return new AbsExtToolTip[] { new PropertyTooltip(new OpenTargetCondition()) };
        }
        //20% chans att offra sig och döda en moståndare
    }

    class Body_snatcher : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Body_snatcher;

        public override string Desc => "Fully restores health when destroying a unit";

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.DestroyedTarget)
            {
                var heal = parentUnit.health.ValueRemoved;

                if (heal > 0)
                {
                    new ToggEngine.Display3D.UnitMessageValueChange(
                        parentUnit, ValueType.Health, heal);

                    parentUnit.health.add(heal);
                    parentUnit.soldierModel.refreshModel(parentUnit.soldierCount());
                }
            }
        }
    }

    class Backstab_expert : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Backstab_expert;

        public override string Desc => "Wont miss a backstab";
    }


    class Flying : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Flying;

        public override string Desc => "Ignores all terrain. All melee attacks against a flying unit gets " + HitWheelsBonus(-1) + ".";
    }

    class Catapult : AbsUnitProperty
    {
        protected const string CatapultDesc = "Launch an attack that will destroy both buildings and units. It has a scatter of one square. Catapults may not move and attack in the same turn.";

        public override UnitPropertyType Type => UnitPropertyType.Catapult;

        public override string Desc => CatapultDesc;
    }


    class Catapult_Plus : Catapult
    {
        public override UnitPropertyType Type => UnitPropertyType.Catapult_Plus;

        public override string Desc => CatapultDesc + " " + TextLib.PercentText(toggLib.CatapultPlusCenterHitChance) + " chance to hit center tile.";
    }

    class Pierce : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Pierce;

        public override string Desc => "Hits can't be blocked";
    }


    class Spawn_point : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Spawn_point;

        public override string Desc => "May spawn a unit";
    }

    class Level_2 : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Level_2;

        public override string Desc => HitChanceBonus(toggLib.LevelUpHitChanceBonus);
    }


    class Level_3 : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Level_3;

        public override string Desc => HitChanceBonus(toggLib.LevelUpHitChanceBonus) + HitWheelsBonus(1);
    }

    class Max_Level : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Max_Level;

        public override string Desc => HitChanceBonus(toggLib.LevelUpHitChanceBonus) + HitWheelsBonus(1) + Block.Block1Desc;
    }



}
