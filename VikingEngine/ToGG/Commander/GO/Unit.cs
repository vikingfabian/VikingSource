using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.Commander.UnitsData;
using VikingEngine.ToGG.Data;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.ToggEngine.GO;

namespace VikingEngine.ToGG.Commander.GO
{
    class Unit : AbsUnit
    {
        public CmdUnitData data;
        public List<AbsUnitCondition> conditions = null;

        public Unit(System.IO.BinaryReader r, DataStream.FileVersion version, UnitCollection coll)
            : base(r, version, coll)
        { }

        public Unit(IntVector2 startPos, UnitType type, AbsGenericPlayer player)
            : this(startPos, cmdRef.units.GetUnit(type), player)
        { }

        public Unit(IntVector2 startPos, CmdUnitData data, AbsGenericPlayer player)
            : base()
        {
            this.data = data;

            assignPlayer(player);
            basicInit(startPos, player.unitsColl, null, true);
        }

        protected override void writeDataType(BinaryWriter w)
        {
            w.Write((byte)data.Type);
        }

        protected override void readDataType(BinaryReader r, DataStream.FileVersion version)
        {
            UnitType type = (UnitType)r.ReadByte();
            data = cmdRef.units.GetUnit(type);
        }

        public bool CanBeOrdered(bool ignoreResting, out CantOrderReason cantOrderReason)
        {
            if (data.underType == UnitUnderType.Special_TacticalBase ||
                data.mainType == UnitMainType.StaticObject)
            {
                cantOrderReason = CantOrderReason.StaticUnit;
                return false;
            }

            if (Resting && !ignoreResting)
            {
                cantOrderReason = CantOrderReason.Resting;
                return false;
            }

            cantOrderReason = CantOrderReason.NONE;
            return true;
            //return !Resting || ignoreResting;
        }


        public override void AddToUnitCard(ToggEngine.Display2D.UnitCardDisplay card, ref Vector2 position)
        {
            base.AddToUnitCard(card, ref position);

            card.startSegment(ref position);
            string name = data.Name;
            if (toggLib.ViewDebugInfo)
            {
                name += " " + TextLib.Parentheses(UnitId.ToString());
            }
            card.portrait(ref position, data.modelSettings.image, name);

            card.statBoxesRow(ref position, this);

            {//VALUE BARS
                if (card.settings.health && hasHealth())
                {
                    card.valueBar(ref position, SpriteName.cmdHeartValueBox,
                        health, new HealthTooltip());
                }
            }

            if (data.properties.HasMembers())//settings.properties && 
            {
                card.properties(ref position, data.properties);
                //card.spaceY(ref position);
            }

            if (arraylib.HasMembers(conditions))
            {
                card.propertyList(ref position, arraylib.CastObject<AbsUnitCondition, Data.Property.AbsProperty>(conditions), SpriteName.toggConditionNegativeTex);
            }
        }

        public override bool IsOpponent(AbsUnit otherUnit)
        {
            return globalPlayerIndex != otherUnit.globalPlayerIndex;
            //throw new NotImplementedException();

        }

        public override bool IsAlly(AbsUnit otherUnit)
        {
            throw new NotImplementedException();
        }

        override public Unit cmd()
        {
            return this;
        }
        override public HeroQuest.Unit hq()
        {
            throw new InvalidCastException();
        }

        public override void moveInfo(out int hasMoved, out int movementLeft, out int staminaMoves, out int max, out int backStabs)
        {
            hasMoved = 0;
            backStabs = 0;
            max = data.move;

            if (movelines != null)
            {
                hasMoved = movelines.MoveLength;
                backStabs = movelines.backStabbersFullCount();
            }

            movementLeft = max - hasMoved;

            
            staminaMoves = 0;
        }

        override public int MoveLengthWithModifiers(bool useOrderStartPos)
        {
            //int movementLeft = base.MoveLengthWithModifiers(useOrderStartPos, out staminaMoves);
            int hasMoved, movementLeft, max, staminaMoves, backStabs;
            moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);

            int moveLength = useOrderStartPos ? max : movementLeft;

            if (onSquare(useOrderStartPos).HasProperty(ToggEngine.Map.TerrainPropertyType.MoveBonus))
            {
                moveLength += 1;
            }

            var p = Player;
            if (p != null && p is Commander.Players.AbsLocalPlayer && Commander.cmdRef.players.allPlayers.Selected() == p)
            {
                var card = ((Commander.Players.AbsLocalPlayer)p).commandCard;
                if (card != null)
                {
                    card.ModifyMoveLength(this, ref moveLength);
                }
            }

            return moveLength;
        }

        override public int AttackWithUnitValue(AbsUnit targetUnit, bool closeCombat)
        {
            int value = targetUnit.TargetValue();
            AttackTarget target = new AttackTarget(targetUnit, closeCombat ? AttackType.Melee : AttackType.Ranged);

            Commander.Battle.BattleSetup attacks = new Commander.Battle.BattleSetup(
                new List<AbsUnit> { this }, target, CommandCard.CommandType.Order_3);

            float expectedDamage = attacks.attackerSetup.attackStrength * attacks.hitChance - attacks.hitBlocks;
            float expectedRetreats = attacks.attackerSetup.attackStrength * attacks.retreatChance - attacks.retreatIgnores;

            //int attackPower = (attacks.attackerSetup.attackStrength - attacks.hitBlocks) * 4 - attacks.retreatIgnores;

            //int counterPower = 0;
            if (closeCombat)
            {
                //Commander.Battle.BattleSetup counterattacks = new Commander.Battle.BattleSetup(
                //    new List<AbsUnit> { targetUnit }, new AttackTarget(this, AttackType.CounterAttack), CommandCard.CommandType.Order_3);

                //counterPower = (counterattacks.attackerSetup.attackStrength - counterattacks.hitBlocks) * 3;

                //Value declines if unit is on a flag, dont wanna risk losing the spot
                if (OnSquare.tileObjects.HasObject(TileObjectType.TacticalBanner))
                {
                    value -= 100;
                }
            }



            //value += (attackPower - counterPower) * 20;

            int damageValue = (int)(MathExt.Square(expectedDamage * 6f + expectedRetreats * 3f));
            value += damageValue;

            //Debug.Log("damage val " + damageValue.ToString());

            float expectedTotalDamage = expectedDamage + expectedRetreats * 0.5f;

            if (expectedTotalDamage <= 0)
            {
                //Will probably do no harm at all
                value -= 100;
            }
            else
            {
                float expectedTargetHealthAfter = targetUnit.health.Value - expectedTotalDamage;

                if (expectedTargetHealthAfter <= 0)
                { //Expected to kill, bonus points
                    if (expectedTargetHealthAfter <= -3)
                    { //Too overkill, wasted strength
                        value += 40;
                    }
                    else
                    {
                        value += 60;
                    }
                }
            }

            return value;
        }

        public override void OnTurnEnd()
        {
            base.OnTurnEnd();

            if (order != null)
            {
                Resting = true;
                order.DeleteMe();
            }
        }

        public void AddCondition(AbsUnitCondition condition)
        {
            arraylib.AddOrCreate(ref conditions, condition);
        }

        public bool TryGetCondition(UnitPropertyType type, out AbsUnitCondition result)
        {
            if (conditions != null)
            {
                foreach (var m in conditions)
                {
                    if (m.Type == type)
                    {
                        result = m;
                        return true;
                    }
                }
            }

            result = null;
            return false;
        }

        public override void OnEvent(EventType eventType, bool local, object tag)
        {
            base.OnEvent(eventType, local, tag);
            if (conditions != null)
            {
                for (int i =  conditions.Count -1; i>= 0; --i)
                {
                    conditions[i].OnEvent(eventType, local, tag, this);
                }
            }
        }

        public override AbsUnitData Data { get => data; set => data = value as CmdUnitData; }

        public bool HasOrder => order != null && order.state == CommandCard.CheckState.Enabled;
    }
}
