using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest;
using VikingEngine.ToGG.HeroQuest.Data.Property;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Data.Property
{
    class UnitActionProperty : AbsUnitProperty
    {
        AbsMonsterAction action;

        public UnitActionProperty(AbsMonsterAction action)
        {
            this.action = action;
        }

        public override SpriteName Icon => action.Icon;

        public override string Name => action.Name;

        public override string Desc => action.Desc;

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return action.DescriptionKeyWords();
        }

        public override void OnEvent(EventType eventType, bool local, object tag, AbsUnit parentUnit)
        {
            if (eventType == EventType.TurnStart)
            {
                action.used = false;
            }
            base.OnEvent(eventType, local, tag, parentUnit);
        }

        public override AbsMonsterAction Action => action;

        public override UnitPropertyType Type => UnitPropertyType.Action_group;
    }

    abstract class AbsPerformUnitAction : ToggEngine.QueAction.AbsQueAction
    {
        protected AbsUnit parentUnit;
        protected AbsProperty parentAction;
        protected AbsUnit target;
        //protected List<AbsUnit> targets;

        public IntVector2 spectatorPos = IntVector2.NegativeOne;
        
        public AbsPerformUnitAction(System.IO.BinaryReader r)
            :base(r)
        { }

        public AbsPerformUnitAction(AbsUnit parentUnit, AbsProperty parentAction)
            :base(parentUnit.hq().PlayerHQ, null)
        {
            this.parentUnit = parentUnit;
            this.parentAction = parentAction;
        }
        
        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);

            parentUnit.hq().netWriteUnitId(w);
            parentAction.writeProperty(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            parentUnit = HeroQuest.Unit.NetReadUnitId(r);
            parentAction = AbsProperty.ReadProperty(r);
        }

        protected void sayAction()
        {
            if (parentUnit != null)
            {
                spectatorPos = parentUnit.squarePos;
                parentUnit.textAnimation(parentAction.Icon, parentAction.Name);
            }
        }

        virtual public void DeleteMe()
        { }

        public override bool IsPlayerQue => true;
    }

    abstract class AbsMonsterAction : AbsProperty
    {
        public bool used = false;

        abstract public SpecialActionClass ActionClass { get; }

        virtual public AbsPerformUnitAction ai_useAction(AbsUnit activeUnit, SpecialActionPriority priority)
        {
            return null;
        }


        abstract public MonsterActionType Type { get; }

        public override void writePropertyType(BinaryWriter w)
        {
            base.writePropertyType(w);
            w.Write((byte)Type);
        }
        
        public static AbsMonsterAction ReadType(System.IO.BinaryReader r)
        {
            MonsterActionType type = (MonsterActionType)r.ReadByte();
            
            switch (type)
            {
                case MonsterActionType.Throw:
                    return new ThrowAction();
                case MonsterActionType.Grapple:
                    return new HeroQuest.Data.Grapple();
                case MonsterActionType.DarkHeal:
                    return new DarkHeal();
                default:
                    throw new NetworkException("AbsUnitAction " + type.ToString(), (int)type);
            }
        }

        public override PropertyMainType MainType => PropertyMainType.MonsterAction;
    }
    

    enum SpecialActionClass
    {
        None,
        MeleeAttackVariant,
        RangedAttackVariant,
        FriendlyBuff,
        Other,

        NUM,
        Any
    }

    enum MonsterActionType
    {
        Throw,
        DarkHeal,
        Grapple,
    }

    enum SpecialActionPriority
    {
        InsteadOfAttack,
        AfterNoMovement,
        AfterFullActiviation,
    }
}
