using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data
{
    class Grapple : AbsMonsterAction
    {
        public override SpriteName Icon => SpriteName.cmdGrapple;

        public override string Name => "Grapple";

        public override string Desc => LanguageLib.SpecialActionDescStart +
            "Grappel a unit: " + Condition.Grappled.GrappledDesc();

        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new ImmobileAffectTooltip(), new DefencelessAffectTooltip() };
        }

        public override SpecialActionClass ActionClass => SpecialActionClass.MeleeAttackVariant;

        public override MonsterActionType Type => MonsterActionType.Grapple;

        public override AbsPerformUnitAction ai_useAction(AbsUnit activeUnit, SpecialActionPriority priority)
        {
            if (priority == SpecialActionPriority.AfterNoMovement)
            {
                var adj = Ai.CollectAdjacentEnemies(activeUnit, activeUnit.squarePos);
                if (arraylib.HasMembers(adj))
                {
                    for (int i = adj.Count - 1; i >= 0; --i)
                    {
                        if (adj[i].hq().condition.Get(Condition.ConditionType.Grappled) != null)
                        {
                            adj.RemoveAt(i);
                        }
                    }

                    if (adj.Count > 0)
                    {
                        return new PerformGrapple(activeUnit, this, Ai.WeakestUnit(adj));
                    }
                }
            }

            return null;
        }
    }

    class PerformGrapple : AbsPerformUnitAction
    {
        public PerformGrapple(System.IO.BinaryReader r)
            : base(r)
        {
            lib.DoNothing();
        }

        public PerformGrapple(AbsUnit parentUnit, 
            AbsMonsterAction parentAction, AbsUnit target)
            : base(parentUnit, parentAction)
        {
            this.target = target;
            lib.DoNothing();
        }
        
        public override void onBegin()
        {
            sayAction();
        }

        public override bool update()
        {
            base.update();

            if (timeStamp.event_ms(400))
            {
                spectatorPos = target.squarePos;

                target.hq().condition.Set(Data.Condition.ConditionType.Grappled, 1,
                    false, true, false);
                //new Condition.Grappled().apply(target, false);
                //sendStatusEffect(target, new StatusEffect.Grappled());

                return true;
            }

            return false;
        }
        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);
            target.hq().netWriteUnitId(w);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);
            target = HeroQuest.Unit.NetReadUnitId(r);
        }

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.PerformGrapple;
    }
 }
