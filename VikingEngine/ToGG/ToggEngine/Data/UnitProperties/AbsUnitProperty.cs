using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Battle;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.HeroQuest.Data.Condition;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.ToggEngine.Data;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngineShared.ToGG.ToggEngine.Data;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG.Data.Property
{
    abstract class AbsUnitProperty : AbsProperty
    {
        public AbsPropertyCounter useCount = null;

        abstract public UnitPropertyType Type { get; }

        public override string Name => TextLib.EnumName(Type.ToString());

        virtual public SpriteName HoverIcon => SpriteName.NO_IMAGE;

        virtual public List<AbsUnit> collectTargetUnits(IntVector2 pos, AbsUnit parentUnit)
        {
            throw new NotImplementedException();
        }

        virtual public void collectDefence(DefenceData defence, bool onCommit)
        { }

        virtual public void collectOpponentDefence(DefenceData defence, bool onCommit)
        { }

        virtual public void OnEvent(ToGG.Data.EventType eventType, bool local, object tag, AbsUnit parentUnit)
        { }

        virtual public void OnApplied(AbsUnit unit)
        { }

        virtual public void OnRemoved(AbsUnit unit)
        { }


        virtual public void toDoList(HeroQuest.Unit parentUnit, List<HeroQuest.Display.AbsToDoAction> list) { }

        public override bool EqualType(AbsProperty obj)
        {
            AbsUnitProperty uProp = obj as AbsUnitProperty;

            return uProp != null &&
                uProp.Type == this.Type &&
                uProp.Level == this.Level;
        }

        virtual public AbsBoardArea AoeAttack(bool melee, AbsUnit parentUnit, out AttackSettings attackSettings)
        {
            attackSettings = null;
            return null;
        }

        virtual public AbsMonsterAction Action => null;

        virtual protected int Level => int.MinValue;

        virtual public bool HasBuff => false;

        virtual public bool IsAiState => false;

        virtual public bool buffUnit(AbsUnit parent, AbsUnit unit, bool friendly, out HeroQuest.Data.Buff buff)
        {
            buff = Buff.Empty;
            return false;
        }

        virtual public void onAttackEnded(HeroQuest.Unit unit, HeroQuest.AttackDisplay attack, bool attacker)
        { }

        public override void writePropertyType(BinaryWriter w)
        {
            base.writePropertyType(w);
            //w.Write((byte)Type);
            writeUnitPropertyType(w);
        }

        public void writeUnitPropertyType(BinaryWriter w)
        {
            w.Write((byte)Type);
        }

        public static AbsUnitProperty ReadUnitPropertyType(BinaryReader r)
        {
            UnitPropertyType type = (UnitPropertyType)r.ReadByte();
            switch (type)
            {
                case UnitPropertyType.Retaliate:
                    return new Retaliate();
                case UnitPropertyType.Parry:
                    return new Parry();
                case UnitPropertyType.MonsterCommander:
                    return new MonsterCommander();
                case UnitPropertyType.MonsterBoss:
                    return new MonsterBoss();
               
                default:
                    throw new NotImplementedExceptionExt(
                        "Read Unit Property type " + type.ToString(),
                        (int)type);
            }
        }

        public void netWritePropertyStatus(HeroQuest.Unit unit)
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.toggUnitPropertyStatus, Network.PacketReliability.Reliable);
            unit.netWriteUnitId(w);
            w.Write((byte)Type);

            writeStatus(w);
        }

        public static void NetReadPropertyStatus(BinaryReader r)
        {
            HeroQuest.Unit unit = HeroQuest.Unit.NetReadUnitId(r);
            UnitPropertyType type = (UnitPropertyType)r.ReadByte();

            if (unit != null)
            {
                var property = unit.data.properties.Get(type);
                if (property != null)
                {
                    property.readStatus(r);
                }
            }
        }

        virtual protected void writeStatus(BinaryWriter w)
        { }

        virtual protected void readStatus(BinaryReader r)
        { }

        virtual public void QuedAction(int actionId, bool local, AbsUnit parentUnit)
        { }

        virtual public void moveModifiers(ref PropertyMoveModifiers modifiers)
        { }

        public void removeFromUnit(AbsUnit parentUnit)
        {
            parentUnit.Data.properties.remove(this);
        }

        public override PropertyMainType MainType => PropertyMainType.UnitProperty;

        virtual public bool overridingAiActions(
            HeroQuest.Players.Ai.UnitAiActions aiActions,
            HeroQuest.Players.Ai.UnitActionCount actionCount,
            out HeroQuest.Players.AiState result)
        {
            result = HeroQuest.Players.AiState.None;
            return false;
        }

        protected string HitChanceBonus(float bonus)
        {
            return TextLib.PercentAddText(bonus) + " hit chance on attacks. ";
        }
        protected string HitWheelsBonus(int bonus)
        {
            return TextLib.ValuePlusMinus(bonus) + " attack strength. ";
        }

        public override string ToString()
        {
            return "Unit property - " + Name;
        }
    }

    class DullWeapon : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdDullWeapon;

        public override UnitPropertyType Type => UnitPropertyType.Dull_Weapon;

        public override string Desc => "Removes " + TextLib.Quote(BattleDice.ResultDesc(BattleDiceResult.CriticalHit)) +
            " from their " + LanguageLib.BattleDie;
    }

    class SlowAttacker : AbsUnitProperty
    {
        static readonly Percent DodgeIncrease = new Percent(40);

        public override UnitPropertyType Type => UnitPropertyType.SlowAttack;

        public override string Desc => "Defender gains +" + DodgeIncrease.ToString() + 
            " on Aviod dice side";

        public override void collectOpponentDefence(DefenceData defence, bool onCommit)
        {
            //base.collectOpponentDefence(defence, onCommit);
            arraylib.AddOrCreate(ref defence.modifications, 
                new BattleDiceModification(BattleDiceResult.Avoid, DodgeIncrease.TextValue));
            
        }
    }

    class Sleepy : AbsUnitProperty
    {
        public override UnitPropertyType Type => UnitPropertyType.Sleepy;

        public override string Desc => "Activates at a closer distance";
    }

    class Swing : AbsUnitProperty
    {
        ModifiedBattleDiceCount modifier; //TODO, förklarande icon
        SwingArea area;
        public Swing(int swingCount = 3)
        {
            area = new SwingArea(swingCount);
            modifier = new ModifiedBattleDiceCount(-1, this);
        }

        public override AbsBoardArea AoeAttack(bool melee, AbsUnit parentUnit, out AttackSettings attackSettings)
        {
            attackSettings = parentUnit.Data.attackSettings(melee);
            attackSettings.Add(modifier);
            return area;
        }

        public override UnitPropertyType Type => UnitPropertyType.Swing;

        protected override int Level => area.swingLength;

        public override SpriteName Icon => SpriteName.cmdSwing;

        public override string Name => "Swing " + Level.ToString();

        public override string Desc => "Unit may attack " + area.swingLength.ToString() + " opponents, with -1 " + LanguageLib.BattleDie;
    }

    class TargetX : AbsUnitProperty
    {
        TargetXArea area;

        public TargetX(int targetCount = 2)
        {
            area = new TargetXArea(targetCount);
        }

        public override AbsBoardArea AoeAttack(bool melee, AbsUnit parentUnit, out AttackSettings attackSettings)
        {
            attackSettings = parentUnit.Data.attackSettings(melee);
            return area;
        }

        public override UnitPropertyType Type => UnitPropertyType.TargetX;

        protected override int Level => area.targetCount;

        public override SpriteName Icon => area.targetCount == 2 ? 
            SpriteName.cmdTarget2 : SpriteName.cmdTarget3;

        public override string Name => "Target " + Level.ToString();

        public override string Desc => "Unit may attack " + area.targetCount.ToString() + " opponents, at the same time";
    }

    class Undead : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdUndeadIcon;
        public override UnitPropertyType Type => UnitPropertyType.Undead;

        public override string Desc => "This creature is animated by dark magic";
    }

    class Regenerate : AbsUnitProperty
    {
        BellValue value;

        public Regenerate(int value)
        {
            this.value = new BellValue(value);
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (local && eventType == EventType.TurnEnd)
            {
                new HealUnit(parentUnit.hq(), value.Next(), HealType.Nature, true);
                //parentUnit.hq().heal(new HealSettings(value.Next(), HealType.Nature));
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override SpriteName Icon => SpriteName.cmdRegenrate;
        public override UnitPropertyType Type => UnitPropertyType.Regenerate;
        public override string Name => "Regenerate " + value.ValueToString();
        public override string Desc => LanguageLib.AtTurnEndDescStart +
            "Restore " + value.IntervalToString() + " " + LanguageLib.Health;
    }

    class DeathPoisionArea : AbsUnitProperty
    {
        const int PoisionValue = 2;
        public DeathPoisionArea()
        { }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.UnitDeath)
            {
                var adj = parentUnit.adjacentUnits(IntVector2.NegativeOne);
                var loop = new ForListLoop<AbsUnit>(adj);
                while (loop.next())
                {
                    loop.sel.hq().condition.Set(ConditionType.Poision, PoisionValue, true, true, false);
                }
            }

            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override UnitPropertyType Type => UnitPropertyType.DeathPoisionArea;

        public override string Name => "Death fart";

        public override string Desc => "On death: Gives " + PoisionValue.ToString() + 
            Poision.PoisionName + " to all adjacent units";
    }

    class Flying : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdFlying;

        public override UnitPropertyType Type => UnitPropertyType.Flying;

        public override string Desc => "Ignores ground terrain. May move through any units.";

        public override void moveModifiers(ref PropertyMoveModifiers modifiers)
        {
            modifiers.ignoresTerrain = true;
            modifiers.moveThroughUnits = true;
            modifiers.otherMoveThroughYou = true;
            modifiers.flyOverObsticles = true;
        }
    }

    class SlipThrough : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdSlipThrough;

        public override UnitPropertyType Type => UnitPropertyType.SlipThrough;

        public override string Name => "Slip-through";
        public override string Desc => "May move through any units.";

        public override void moveModifiers(ref PropertyMoveModifiers modifiers)
        {
            modifiers.moveThroughUnits = true;
        }
    }

    abstract class AbsMonsterCommander : AbsUnitProperty
    {
        //const int Range = 3;
        public override string Desc => LanguageLib.FormatSentences(
            "Friendly units gain +1 " + LanguageLib.BattleDie + " and +1 movement",
            "Range " + CommandRange.ToString());

        public override bool buffUnit(AbsUnit parent, AbsUnit unit, bool friendly, out Buff buff)
        {
            if (friendly && parent.InRangeAndSight(unit.squarePos, CommandRange, true, false))
            {
                buff = Buff.Empty;
                buff.movement += 1;
                buff.attackDice += 1;

                return true;
            }

            return base.buffUnit(parent, unit, friendly, out buff);
        }

        abstract protected int CommandRange { get; }
    }

    class MonsterCommander : AbsMonsterCommander
    {
        const int Range = 3;

        public override string Name => "Commander";
        public override SpriteName Icon => SpriteName.cmdMonsterCommander;

        public override UnitPropertyType Type => UnitPropertyType.MonsterCommander;

        protected override int CommandRange => Range;
    }

    class MonsterBoss : AbsMonsterCommander
    {
        const int Range = 5;

        public override string Name => "Boss";
        public override SpriteName Icon => SpriteName.cmdMonsterCommander;

        public override UnitPropertyType Type => UnitPropertyType.MonsterBoss;

        protected override int CommandRange => Range;
    }

    class DoubleAttack : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdFrenzy;

        public override UnitPropertyType Type => UnitPropertyType.DoubleAttack;

        public override string Name => "Frenzy";
        public override string Desc => "May attack twice, instead of moving.";
    }
    class SurgeOptionGain : AbsUnitProperty
    {
        HeroQuest.Data.AbsSurgeOption surgeOption;
        public SurgeOptionGain(HeroQuest.Data.AbsSurgeOption surgeOption)
        {
            this.surgeOption = surgeOption;
        }
        public override UnitPropertyType Type => UnitPropertyType.SurgeOptionGain_group;

        public override string Desc => surgeOption.Desc;

        public override List<AbsRichBoxMember> AdvancedCardDisplay()
        {
            var display = new List<AbsRichBoxMember>(surgeOption.surgeCost + 3);
            for (int i = 0; i < surgeOption.surgeCost; ++i)
            {
                display.Add(new RbImage(SpriteName.cmdIconSurgeSmall, 1f, -0.2f, -0.2f));
            }

            display.Add(new RbImage(SpriteName.cmdConvertArrow, 0.6f));
            display.Add(new RbImage(surgeOption.Icon));
            display.Add(new RbText(surgeOption.Name));

            return display;
        }

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return surgeOption.DescriptionKeyWords();
        }
    }

    class Pet : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdPetTargetGui;

        public override UnitPropertyType Type => UnitPropertyType.Pet;

        public override string Desc => "Is ignored by enemies";

        public override void moveModifiers(ref PropertyMoveModifiers modifiers)
        {
            modifiers.otherMoveThroughYou = true;
        }
    }    

    class Scratchy : AbsUnitProperty
    {
        const int MaxTargets = 3;
        public static readonly Damage PetScratchDamage = Damage.BellDamage(2);

        public override List<AbsUnit> collectTargetUnits(IntVector2 pos, AbsUnit parentUnit)
        {
            var targets = hqRef.players.adjacentOpponents(parentUnit, pos);
            var trimmed = arraylib.RandomListMembers(targets, MaxTargets);
            return trimmed;
        }

        public override UnitPropertyType Type => UnitPropertyType.Scratchy;

        public override string Desc => LanguageLib.AtTurnEndDescStart + "Gives " + PetScratchDamage.description() +
            " to adjacent opponents. Max " + MaxTargets.ToString() + " targets.";
    }

    class StaticTarget : AbsUnitProperty
    {
        public override SpriteName Icon => SpriteName.cmdUnitPracticeDummy;
        public override UnitPropertyType Type => UnitPropertyType.Static_target;

        public override string Desc => "Unit does not move or attack";
    }
}
