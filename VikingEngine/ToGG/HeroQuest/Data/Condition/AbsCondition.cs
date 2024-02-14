using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data.Condition
{
    abstract class AbsUnitStatus : AbsProperty, Battle.IBattleModification
    {
        abstract public int StatusIsPositive { get; }

        virtual public bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return false;
        }
        virtual public void modLabel(ToggEngine.BattleEngine.BattleModifierLabel label)
        {
            throw new NotImplementedException();
        }
        virtual public void applyMod(BattleSetup setup)
        {
            throw new NotImplementedException();
        }

        virtual public void netWriteMod(System.IO.BinaryWriter w)
        { }

        virtual public void netReadMod(System.IO.BinaryReader r)
        { }

        abstract public Battle.BattleModificationType ModificationType { get; }
        abstract public int ModificationUnderType { get; }
    }

    abstract class AbsCondition : AbsUnitStatus
    {
        protected int applyTurn;

        public override SpriteName Icon => SpriteName.MissingImage;

        virtual public bool Contains(BaseCondition status)
        {
            return false;
        }

        virtual public bool update_asynch(Unit unit)
        {
            return false;
        }

        public void netWriteApply(AbsUnit reciever, bool apply)
        {
            var w = Ref.netSession.BeginWritingPacket(
                Network.PacketType.hqApplyStatusEffect, Network.PacketReliability.Reliable);
            w.Write(apply);
            writeCondition(w);
            w.Write((byte)Level);
            reciever.hq().netWriteUnitId(w);
        }

        public static void NetReadAppliedStatusEffect(System.IO.BinaryReader r)
        {
            bool apply = r.ReadBoolean();
            //AbsCondition condition = ReadCondition(r);
            ConditionType type = (ConditionType)r.ReadByte();
            int level = r.ReadByte();
            Unit reciever = Unit.NetReadUnitId(r);

            if (reciever != null)
            {
                if (apply)
                {
                    //condition.apply(reciever, false);
                    reciever.condition.Set(type, level, false, true, false);
                }
                else
                {
                    reciever.condition.Remove(type, false);
                }
            }
        }

        void writeCondition(System.IO.BinaryWriter w)
        {
            w.Write((byte)ConditionType);
        }

        public static AbsCondition Create(ConditionType type, int level)
        {
            AbsCondition result;
            switch (type)
            {
                case ConditionType.Grappled:
                    result = new Grappled();
                    break;
                case ConditionType.Immobile:
                    result = new Immobile(level);
                    break;
                case ConditionType.Spotted:
                    result = new Spotted();
                    break;
                case ConditionType.Hidden:
                    result = new Hidden();
                    break;
                case ConditionType.Bleed:
                    result = new Bleed();
                    break;
                case ConditionType.Poision:
                    result = new Poision(level);
                    break;
                case ConditionType.Idle:
                    result = new Idle();
                    break;

                case ConditionType.Stunned:
                    result = new Stunned();
                    break;
                case ConditionType.StunImmune:
                    result = new StunImmune(level);
                    break;

                default:
                    throw new NotImplementedExceptionExt("Get Condition " + type.ToString(), (int)type);
            }

            return result;
        }

        //virtual public void attackCountModifiers(BattleSetup coll, bool isAttacker)
        //{
        //}

        protected bool filterOneUseOnly(BattleSetup coll)
        {
            //Kan ersättas med att gå igenom modlistan
            if (coll.attackerSetup.defendersConditions.Contains(ConditionType))
            {
                return false;
            }
            else
            {
                coll.attackerSetup.defendersConditions.Add(ConditionType);
                return true;
            }
        }

        virtual public void onApply(Unit unit)
        {
            applyTurn = unit.PlayerHQ.TurnsCount;
        }

        virtual public void onRemoved(Unit unit, bool local, bool asynch)
        { }
        
        virtual public PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {            
            return PropertyEventAction.None;
        }

        virtual public int Level
        {
            get { return 0; }
            set { lib.DoNothing(); }
        }

        abstract public ConditionType ConditionType { get; }
        override public int ModificationUnderType { get { return (int)ConditionType; } }
        public override BattleModificationType ModificationType => BattleModificationType.UnitCondition;
    }

    class Grappled : AbsCondition
    {
        //Ankare som icon
        public override SpriteName Icon => SpriteName.cmdGrapple;

        public override string Name => "Grappled";

        public override string Desc => GrappledDesc();
        
        public static string GrappledDesc()
        {
            return "Unit is " +
                Immobile.ImmobileName + " and " +
                DefencelessAffectTooltip.DefenselessName +
                " while adjacent to enemy units.";
        }

        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.Defenseless || status == BaseCondition.Immobile;
        }

        public override bool update_asynch(Unit unit)
        {
            return unit.bAdjacentOpponents() == false;
        }

        public override ConditionType ConditionType => ConditionType.Grappled;

        public override int StatusIsPositive => -1;
    }

    class Spotted : AbsCondition
    {
        public const int SpottedAttackBonus = 2;
        //IntVector2 unitPos = IntVector2.NegativeOne;

        public override SpriteName Icon => SpriteName.cmdPetTargetGui;

        public override string Name => "Spotted";

        public override string Desc => SottedDesc();

        public static string SottedDesc()
        {
            return "Attackers gains " + SpottedAttackBonus.ToString() + " dice. Effect is removed when the unit moves.";
        }

        public override bool HasBattleModifier(BattleSetup setup, bool isAttacker)
        {
            return !isAttacker && filterOneUseOnly(setup);
        }

        public override void applyMod(BattleSetup setup)
        {
            setup.attackerSetup.attackStrength += SpottedAttackBonus;
        }
        
        public override void modLabel(BattleModifierLabel label)
        {
            //base.modLabel(label);
            label.modSource(this);
            label.attackModifier(SpottedAttackBonus);
        }
        //public override void attackCountModifiers(BattleSetup setup, 
        //    bool isAttacker)
        //{
        //    //if (!isAttacker && filterOneUseOnly(setup))
        //    //{
        //    //    setup.attackerSetup.attackStrength += SpottedAttackBonus;

        //        var label = setup.attackerSetup.beginModLabel();
        //        label.modSource(this);
        //        label.attackModifier(SpottedAttackBonus);
        //    //}
        //}

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            if (eventType == EventType.UnitPositionChange)
            {
                return PropertyEventAction.Remove;
            }
            return PropertyEventAction.None;
        }

        public override ConditionType ConditionType => ConditionType.Spotted;

        public override int StatusIsPositive => -1;
    }

    class Webbed : AbsCondition
    {
        //Ankare som icon
        public override SpriteName Icon => SpriteName.cmdGrapple;

        public override string Name => "Webbed";

        public override string Desc => GrappledDesc();

        public static string GrappledDesc()
        {
            return "Unit is " +
                Immobile.ImmobileName;
        }

        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.Defenseless || status == BaseCondition.Immobile;
        }

        public override bool update_asynch(Unit unit)
        {
            return unit.bAdjacentOpponents() == false;
        }

        public override ConditionType ConditionType => ConditionType.Webbed;

        public override int StatusIsPositive => -1;
    }

    class Hidden : AbsCondition
    {
        public override SpriteName Icon => HiddenIcon;

        public const string HiddenName = "Hidden";
        public override string Name => HiddenName;

        public override string Desc => HiddenDesc();

        public const SpriteName HiddenIcon = SpriteName.cmdHiddenIcon;

        public static string HiddenDesc()
        {
            return "Can't be targeted by enemies. Will be lost when either: attacking, or adjacent to opponent";
        }

        public override void onApply(Unit unit)
        {
            base.onApply(unit);
            refreshColor(unit);
        }

        void refreshColor(Unit unit)
        {
            unit.soldierModel.model.Color = Color.DarkBlue;
        }

        public override PropertyEventAction OnEvent(Unit unit, EventType eventType, object tag)
        {
            if (eventType == EventType.ParentUnitMoved)
            {
                MoveLinesGroup movelines = (MoveLinesGroup)tag;
                foreach (var m in movelines.lines)
                {
                    if (unit.adjacentOpponentsCount(m.toPos) > 0)
                    {
                        return PropertyEventAction.Remove;
                    }
                }

                refreshColor(unit);
            }
            else if (eventType == EventType.Attack)
            {
                return PropertyEventAction.Remove;
            }

            return base.OnEvent(unit, eventType, tag);
        }

        public override void onRemoved(Unit unit, bool local, bool asynch)
        {
            unit.soldierModel.model.Color = Color.White;

            if (local)
            {
                if (asynch)
                {
                    new Timer.Action2ArgTrigger<AbsUnit, bool>(netWriteApply, unit, false);
                }
                else
                {
                    netWriteApply(unit, false);
                }

                new Timer.Asynch1ArgTrigger<AbsUnit>(
                    hqRef.players.dungeonMaster.checkAlertFromMovingOpponent_Asynch, unit);
            }
        }

        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.NoTarget;
        }

        public override ConditionType ConditionType => ConditionType.Hidden;

        public override int StatusIsPositive => 1;
    }

    class Idle : AbsCondition
    {
        public override bool Contains(BaseCondition status)
        {
            return status == BaseCondition.CantActivate;
        }

        public override bool update_asynch(Unit unit)
        {
            return unit.aiAlerted;
        }

        public override SpriteName Icon => SpriteName.cmdInactiveMonster;

        public override string Name => "Inactive";

        public override string Desc => "Will not activate during it's turn";

        public override ConditionType ConditionType => ConditionType.Idle;

        public override int StatusIsPositive => 0;
    }

    enum BaseCondition
    {
        Immobile,
        Defenseless,
        NoTarget,
        CantActivate,
    }

    enum ConditionType
    {
        Grappled,
        Webbed,
        Hidden,
        Spotted,
        Idle,
        Bleed,
        Immobile,
        Poision,
        Stunned,
        StunImmune,
    }
}
