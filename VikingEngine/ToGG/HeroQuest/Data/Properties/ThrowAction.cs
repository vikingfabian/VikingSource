using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Data.Property;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.HeroQuest.Data.Property
{
    class ThrowAction : AbsMonsterAction
    {
        Damage damage;
        public ThrowAction(int damage, int push)
        {
            this.damage = new Damage(damage, DamageApplyType.Direct, ValueRangeType.BellValue,
                DamageElementType.None, DamageEffectType.Push, push);
        }

        public ThrowAction()
        { }

        public override AbsPerformUnitAction ai_useAction(AbsUnit activeUnit, SpecialActionPriority priority)
        {
            if (priority == SpecialActionPriority.AfterNoMovement)
            {
                var targets = Ai.CollectAdjacentEnemies(activeUnit, activeUnit.squarePos);

                //remove those who are against a wall
                //TODO, checka alla steg bakom och räkna in de som kan flygas över
                if (arraylib.HasMembers(targets))
                {
                    for (int i = targets.Count - 1; i >= 0; --i)
                    {
                        IntVector2 dir = targets[i].squarePos - activeUnit.squarePos;
                        IntVector2 behind = targets[i].squarePos + dir;
                        var restrict = toggRef.board.MovementRestriction(behind, targets[i]);
                        if (restrict == ToggEngine.Map.MovementRestrictionType.Impassable)
                        {
                            targets.RemoveAt(i);
                        }
                    }
                }

                if (arraylib.HasMembers(targets))
                {
                    return new PerformThrowAction(activeUnit, this, Ai.StrongestMeeleUnit(targets), damage.NextDamage(activeUnit));
                }
            }

            return null;
        }

        public override string Name => "Throw Action";
        public override string Desc => LanguageLib.SpecialActionDescStart + damage.description();
        
        public override AbsExtToolTip[] DescriptionKeyWords()
        {
            return new AbsExtToolTip[] { new PushXTooltip(), new SpecialActionTooltip() };
        }

        public override SpecialActionClass ActionClass => SpecialActionClass.MeleeAttackVariant;

        public override MonsterActionType Type => MonsterActionType.Throw;
    }

    class PerformThrowAction : AbsPerformUnitAction
    {
        Damage damage;
        IntVector2 movedir;
        IntVector2 moveTo;

        bool wallHitDamage = false;
        AbsUnit hitOtherUnit = null;

        public PerformThrowAction(System.IO.BinaryReader r)
            : base(r)
        { }

        public PerformThrowAction(AbsUnit parentUnit, AbsMonsterAction parentAction, AbsUnit target, Damage damage)
            : base(parentUnit, parentAction)
        {
            this.target = target;
            this.damage = damage;

            movedir = parentUnit.vecTowards(target);
            IntVector2 from = target.squarePos;
            moveTo = from;

            for (int i = 0; i < damage.effectValue; ++i)
            {
                IntVector2 next = moveTo + movedir;
                if (passableSquare(next, ref hitOtherUnit))
                {
                    moveTo = next;
                }
                else
                {
                    wallHitDamage = true;
                    break;
                }
            }

            bool passableSquare(IntVector2 pos, ref AbsUnit hitOtherUnit)
            {
                ToggEngine.Map.BoardSquareContent sq;
                if (toggRef.board.tileGrid.TryGet(pos, out sq))
                {
                    if (sq.HasProperty(ToggEngine.Map.TerrainPropertyType.Impassable))
                    {
                        return false;
                    }
                    else if (sq.unit != null)
                    {
                        hitOtherUnit = sq.unit;
                        return false;
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        protected override void netWrite(BinaryWriter w)
        {
            base.netWrite(w);

            target.hq().netWriteUnitId(w);
            toggRef.board.WritePosition(w, moveTo);
            movedir.writeSByte(w);
            w.Write(wallHitDamage);
        }

        protected override void netRead(BinaryReader r)
        {
            base.netRead(r);

            target = HeroQuest.Unit.NetReadUnitId(r);
            moveTo = toggRef.board.ReadPosition(r);
            movedir.readSByte(r);
            wallHitDamage = r.ReadBoolean();
        }

        public override void onBegin()
        {
            sayAction();
        }
        
        public override bool update()
        {
            base.update();

            if (timeStamp.event_ms(200))
            {
                spectatorPos = moveTo;
                new ToGG.Effects.MoveUnitAnim(target, moveTo, onMoveComplete, true);
            }

            return false;
        }

        void onMoveComplete()
        {
            if (isLocalAction)
            {
                var totalDamage = damage.NextDamage();

                if (wallHitDamage)
                {
                    totalDamage.Add(HeroQuest.hqLib.WallHitDamage.NextDamage());

                    if (hitOtherUnit != null)
                    {
                        HeroQuest.RecordedDamageEvent rec2;

                        hitOtherUnit.hq().TakeDamage(HeroQuest.hqLib.WallHitDamage.NextDamage(), DamageAnimationType.None,
                           null, out rec2);

                        rec2.NetSend();
                    }
                }
                else
                {
                    state = ToggEngine.QueAction.QueState.Completed;
                }

                HeroQuest.RecordedDamageEvent rec1;

                target.hq().TakeDamage(totalDamage, DamageAnimationType.None,
                    null, out rec1);

                rec1.NetSend();
            }


            if (wallHitDamage)
            {
                new ToGG.Effects.BounceUnitAnim(target, target.squarePos + movedir, 0.2f, onBounceComplete);
            }
        }

        void onBounceComplete()
        {
            state = ToggEngine.QueAction.QueState.Completed;
        }

        public override ToggEngine.QueAction.QueActionType Type => ToggEngine.QueAction.QueActionType.PerformThrow;
    }
}
