using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.HeroQuest.Players;
using VikingEngineShared.ToGG.ToggEngine.Data;

namespace VikingEngine.ToGG.HeroQuest
{
    class Unit : AbsUnit
    {
        public HqUnitData data;
        public AttackTargetCollection attackTargets = null;
        public IntVector2 restartPos;
        public Data.Condition.UnitConditions condition;
        
        public Unit(IntVector2 startPos, HqUnitType type, AbsHQPlayer player)
           : this(startPos, hqRef.unitsdata.Get(type), player)
        {
            lib.DoNothing();
        }

        public Unit(IntVector2 startPos, HqUnitData data, AbsHQPlayer player)
            : base()
        {
            this.data = data;
            assignPlayer(player);
            basicInit(startPos, player.unitsColl, null, true);

            onNewUnit();
        }

        public Unit(System.IO.BinaryReader r, DataStream.FileVersion version, UnitCollection coll, bool placeOnMap = true) 
            : base(r, version, coll, placeOnMap)
        {
            //PlayerHQ.onNewUnit(this);
            onNewUnit();
        }

        public override void onNewUnit()
        {
            base.onNewUnit();
            PlayerHQ.onNewUnit(this);
        }

        protected override void basicInit(IntVector2 startPos, ToggEngine.GO.UnitCollection coll, int? storedStartHealth, bool placeOnMap)
        {
            condition = new Data.Condition.UnitConditions(this);
            attackTargets = new AttackTargetCollection(this);
            restartPos = startPos;

            if (StartUpSett.AlertAllMonsters)
            {
                Alert();
            }

            base.basicInit(startPos, coll, storedStartHealth, placeOnMap);
        }

        public override void update_asynch()
        {
            if (gotStartPosition)
            {
                base.update_asynch();

                condition.update_asynch();
            }
        }

        override public void OnEvent(ToGG.Data.EventType eventType, bool local, object tag)
        {
            base.OnEvent(eventType, local, tag);

            condition.OnEvent(eventType, tag);            
        }

        protected override void writeDataType(System.IO.BinaryWriter w)
        {
            w.Write((byte)data.Type);
        }

        protected override void readDataType(System.IO.BinaryReader r, DataStream.FileVersion version)
        {
            var type = (HqUnitType)r.ReadByte();
            data = hqRef.unitsdata.Get(type);
        }

        public bool isGroupTarget(IntVector2 targetPosition, out AttackTargetGroup targets)
        {
            if (data.hero != null && data.hero.availableStrategies.active.groupAttack)
            {
                List<AttackTarget> valid;
                targets = new AttackTargetGroup(data.hero.availableStrategies.active.collectAttackGroup(
                    this, targetPosition, out valid));

                return valid.Count > 0;
            }

            targets = null;
            //valid = null;
            return false;
        }

        public override void OnTurnStart()
        {
            orderStartPos = squarePos;
            deleteMoveLines();
            hasEndedMovement = false;
        }

        public bool heroCanRestartHere(IntVector2 pos, bool ignoreUnits)
        {
            if (toggRef.board.MovementRestriction(pos, this, ignoreUnits) ==
                ToggEngine.Map.MovementRestrictionType.NoRestrictions)
            {
                return bAdjacentOpponents(pos) == false;
            }
            return false;
        }

        public override void moveInfo(out int hasMoved, out int movementLeft, out int staminaMoves, out int max, out int backStabs)
        {
            hasMoved = 0;
            backStabs = 0;
            max = data.move;

            if (data.hero != null && data.hero.availableStrategies.active != null)
            {
                max = data.hero.availableStrategies.active.movement;
            }

            max += condition.buffs.total.movement;

            if (movelines != null)
            {
                hasMoved = movelines.MoveLength;
            }

            movementLeft = max - hasMoved;
            if (data.hero == null)
            {
                staminaMoves = 0;
            }
            else
            {
                staminaMoves = data.hero.stamina.Value;
            }
        }

        public void TakeDamage(Damage damage, DamageAnimationType animationType, IntVector2? damageSourcePos, out RecordedDamageEvent rec, bool local = true)
        {
            if (damageSourcePos == null)
            {
                damageSourcePos = squarePos;
            }

            if (hasHealth())
            {
                takeDamageModifiers(ref damage);
                rec = new RecordedDamageEvent(this, damage, animationType, damageSourcePos.Value);

                if (damage.Value > 0)
                {
                    if (health.HasValue)
                    {
                        health.add(-damage.Value);
                        viewDamage(damage.Value, animationType, damageSourcePos.Value, local);
                    }
                }
                rec.endhealth = health.Value;
            }
            else
            {
                rec = RecordedDamageEvent.Empty();
            }

            OnEvent(ToGG.Data.EventType.DamageTarget, local, rec);
        }

        void takeDamageModifiers(ref Damage damage)
        {
            if (damage.applyType != DamageApplyType.Pure)
            {
                data.defence.checkDamageResistance(ref damage);

                var poision = condition.Get(HeroQuest.Data.Condition.ConditionType.Poision);
                if (poision != null)
                {
                    damage.Value += poision.Level;
                }
            }
        }

        public void TakeDamage(RecordedDamageEvent rec)
        {
            health.Value = rec.endhealth;

            viewDamage(rec.damage.Value, rec.animationType, rec.damageSource, false);

            hqRef.netManager.historyAdd(this.ToString() +
                " recieved " + rec.damage.value.ToString() + " damage , pos" + rec.damageSource.ToString());

            OnEvent(ToGG.Data.EventType.DamageTarget, false, rec);
        }

        void viewDamage(int damage, DamageAnimationType animationType, IntVector2 damageSource,  bool local)
        {
            new ToggEngine.Display3D.UnitMessageValueChange(
                this, ValueType.Health, -damage);

            if (health.IsZero)
            {
                //Unit is terminated
                onDeath(local);
                if (data.hero == null)
                {
                    new UnitDeathAnimation(this);
                }
            }
            else
            {
                soldierModel.refreshModel(soldierCount());                
            }

            if (animationType == DamageAnimationType.AttackSlash)
            {
                new Effects.DamageSlashEffect(this, damageSource);
            }
            statusGui?.refresh();
        }

        //public Net.HealUnit heal(HealSettings add)
        //{
        //    Net.HealUnit rec = new Net.HealUnit(this);

        //    if (add.heal > 0)
        //    {
        //        rec.heal = add;

        //        if (add.heal > health.ValueRemoved)
        //        {
        //            add.heal = health.ValueRemoved;
        //        }

        //        new ToggEngine.Display3D.UnitMessageValueChange(
        //            this, ValueType.Health, add.heal);

        //        health.add(add.heal);
        //        OnEvent(ToGG.Data.EventType.Heal, true, null);
        //        statusGui?.refresh();
        //    }

        //    return rec;
        //}

        public void addStamina(int add)
        {
            if (IsHero && !data.hero.stamina.IsMax)
            {
                add = Bound.Max(add, data.hero.stamina.ValueRemoved);

                data.hero.stamina.add(add);

                new ToggEngine.Display3D.UnitMessageValueChange(
                    this, ValueType.Stamina, add);

                if (movelines != null)
                {
                    movelines.availableMoveLength += add;
                }
            }
        }

        public void addBloodrage(int add)
        {
            if (data.hero != null &&
                data.hero.bloodrage.willAddChangeValue(add))
            {
                data.hero.bloodrage.add(add);

                new ToggEngine.Display3D.UnitMessageValueChange(
                    this, ValueType.BloodRage, add);
            }
        }

        public void dodgeAnimation(Unit attacker, bool isLocal)
        {
            IntVector2 towards;
            if (attacker != null)
            {
                towards = attacker.squarePos.mirrorTilePos(this.squarePos);
            }
            else
            {
                towards = IntVector2.Zero;
            }

            new Effects.BounceUnitAnim(this, towards, 0.3f);//animateBounceMotion(source, 0.3f, 120f);

            textAnimation(SpriteName.cmdDodge, "DODGE!");

            if (isLocal)
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqDodgeEffect, Network.PacketReliability.Reliable);
                netWriteUnitId(w);
                attacker.netWriteUnitId(w);
            }
        }

        public void AlertEffect()
        {
            new ToggEngine.Display3D.UnitMessageEmote(this, ToggEngine.Display3D.EmoteType.Alerted);
        }

        public List<SpriteName> unitHoverStatusIcons()
        {
            List<SpriteName> sprites = new List<SpriteName>();

            var statuses = condition.conditions.counter();
            while (statuses.Next())
            {
                sprites.Add(statuses.sel.Icon);
            }
            
            var buffs = condition.buffs.total.properties();
            if (buffs != null)
            {
                foreach (var m in buffs)
                {
                    sprites.Add(m.Icon);
                }
            }

            var properties = data.properties.Loop();
            while (properties.next())
            {
                var ic = properties.sel.HoverIcon;
                if (ic != SpriteName.NO_IMAGE)
                {
                    sprites.Add(ic);
                }
            }

            if (data.hero != null && data.hero.bloodrage.IsMax)
            {
                sprites.Add(SpriteName.cmdIconBloodrageSmall);
            }

            return sprites;
        }

        

        //public void attackCountModifiers(BattleSetup coll,
        //   bool isAttacker)
        //{
        //    condition.attackCountModifiers(coll, isAttacker);
        //}

        public override void onAttack()
        {
            base.onAttack();
            if (data.hero != null)
            {
                data.hero.availableStrategies.active.attacks.minusOne();
            }

            PlayerHQ.onAttack();
        }

        public void onAttackEnded(AttackDisplay attack, bool attacker)
        {
            if (Alive)
            {
                if (data.properties.HasMembers())
                {
                    foreach (var m in data.properties.members)
                    {
                        m.onAttackEnded(this, attack, attacker);
                    }
                }

                var ai = PlayerHQ as HeroQuest.Players.AiPlayer;
                if (ai != null)
                {
                    ai.alertedOnAttack(this);
                }
            }
        }

        protected override void onDeath(bool local)
        {
            if (data.hero == null)
            {
                DeleteMe();

                if (PlayerHQ.IsScenarioOpponent)
                {
                    OnSquare.tileObjects.AddLootDrop(squarePos);

                    if (IsLocal)
                    {
                        var carry = data.properties.Get(UnitPropertyType.CarryItem);
                        if (carry != null)
                        {
                            var item = ((Gadgets.CarryProperty)carry).item;
                            if (!item.CarryOnly)
                            {
                                //HeroQuest.hqRef.items.spawnChest(pos, HeroQuest.Data.LootFinder.ChestLvl);
                                var items = HeroQuest.hqRef.items.groundCollection(squarePos, true);
                                items.Add(item);
                                Gadgets.TileItemCollection.NetWrite(squarePos);
                            }
                        }
                    }
                }
            }
            else
            {
                if (local)
                {
                    new QueAction.HeroDeath(this);
                }
            }

            unitTag?.onDeath();

            OnEvent(ToGG.Data.EventType.UnitDeath, local, null);
        }

        public override bool canTargetUnit(AbsUnit attackTarget)
        {
            return !attackTarget.HasProperty(UnitPropertyType.Pet) && 
                attackTarget.hq().condition.GetBase(HeroQuest.Data.Condition.BaseCondition.NoTarget) == null &&
                attackTarget.Alive &&
                IsOpponent(attackTarget);
        }

        public List<Display.AbsToDoAction> actionList()
        {
            List<Display.AbsToDoAction> result = new List<Display.AbsToDoAction>();

            if (data.hero != null && data.hero.availableStrategies.active != null)
            {
                data.hero.availableStrategies.active.toDoList(this, result);
            }
            else if (data.move > 0)
            {
                result.Add(new Display.ToDoMove(this));
            }

            if (data.properties.HasMembers())
            {
                foreach (var m in data.properties.members)
                {
                    m.toDoList(this, result);
                }
            }

            return result;
        }
        
        public void collectActions(List<Data.UnitAction.AbsUnitAction> unitActions)
        {
            if (data.hero != null)
            {
                data.hero.availableStrategies.active?.collectActions(unitActions);
            }
            else
            {
                var loop = data.properties.Loop();
                while (loop.next())
                {
                    if (loop.sel is Data.UnitAction.AbsUnitAction)
                    {
                        unitActions.Add((Data.UnitAction.AbsUnitAction)loop.sel);
                    }
                }
            }
        }

        override public void writeAllData(System.IO.BinaryWriter w)
        {
            toggRef.absPlayers.writeGenericPlayer(globalPlayerIndex, w);

            writePlayerCollection(w);
        }

        public static Unit ReadAllData(System.IO.BinaryReader r, DataStream.FileVersion version, 
            bool placeOnMap)
        {
            var player = toggRef.absPlayers.readGenericPlayer(r) as Players.AbsHQPlayer;

            return new Unit(r, version, player.hqUnits);
        }

        public void netWriteUnitId(System.IO.BinaryWriter w)
        {
            writeIndex(w);
        }

        public static Unit NetReadUnitId(System.IO.BinaryReader r)
        {
            int globalPlayerIndex, unitIndex;
            ReadIndex(r, out globalPlayerIndex, out unitIndex);

            AbsGenericPlayer player;
            return (Unit)hqRef.gamestate.GetUnit(globalPlayerIndex, unitIndex, out player);
            //hqRef.gamestate.GetUnit(r, out player);
        }

        public void netSendCreate()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqCreateUnit, 
                Network.PacketReliability.Reliable);
            writeCreate(w);
        }

        public void writeCreate(System.IO.BinaryWriter w)
        {
            hqRef.players.netWritePlayer(w, this);
            writePlayerCollection(w);
        }
        public static Unit ReadCreate(System.IO.BinaryReader r)
        {
            var player = hqRef.players.netReadPlayer(r);
            return new Unit(r, DataStream.FileVersion.Max, player.hqUnits);
        }

        public void netWriteStatus(System.IO.BinaryWriter w)
        {
            health.WriteByteStream(w);
            data.hero?.netWrite(w);
        }

        public void netReadStatus(System.IO.BinaryReader r)
        {
            health.ReadByteStream(r);
            data.hero?.netRead(r);
        }

        public override bool canBeAlerted()
        {
            return HasProperty(UnitPropertyType.Static_target) == false;
        }

        public override AbsGenericPlayer Player
        {
            get
            {
                return hqRef.players.player(this.globalPlayerIndex);
            }
        }

        public AbsHQPlayer PlayerHQ
        {
            get
            {
                return hqRef.players.player(this.globalPlayerIndex);
            }
        }

        public Players.LocalPlayer HqLocalPlayer
        {
            get
            {
                return hqRef.players.player(this.globalPlayerIndex) as Players.LocalPlayer;
            }
        }

        public override bool IsScenarioOpponent()
        {
            return hqRef.players.player(this.globalPlayerIndex).IsScenarioOpponent;
        }

        public override bool IsOpponent(AbsUnit otherUnit)
        {
            return hqRef.players.IsOpponent(this, otherUnit);
        }

        public override bool IsAlly(AbsUnit otherUnit)
        {
            return hqRef.players.isAlly(this, otherUnit);
        }

        public List<Unit> collectAdjacentHeroes(IntVector2 center, bool itemUse)
        {
            List<Unit> result = new List<Unit>();
            
            foreach (var dir in IntVector2.Dir8Array)
            {
                var unit = toggRef.board.getUnit(center + dir);
                if (unit != null && unit.hq().data.hero != null)
                {
                    result.Add(unit.hq());
                }
            }

            if (itemUse && data.hero != null)
            {//modified adj for heroes
                List<AbsUnit> deliverUnits = new List<AbsUnit>();
                hqRef.players.unitsWithProperty(UnitPropertyType.DeliveryService, 
                    deliverUnits, Players.PlayerCollection.HeroTeam);
                foreach (var m in deliverUnits)
                {
                    var deliverTo = m.hq().collectAdjacentHeroes(m.squarePos, false);
                    arraylib.AddIfMissing(result, deliverTo);
                }
            }

            return result;
        }

        override public bool hasHealth()
        {
            return !HasProperty(UnitPropertyType.Pet);
        }
        public override bool hasStamina()
        {
            return data.hero != null;
        }

        public bool ableToMove()
        {
            return condition.GetBase(HeroQuest.Data.Condition.BaseCondition.Immobile) == null &&
                data.HasMoveAbility;
        }

        public override bool canMoveThrough(AbsUnit otherUnit)
        {
            if (hqRef.players.isAlly(this, otherUnit))
            {
                return true;
            }

            PropertyMoveModifiers otherUnitMove = otherUnit.Data.properties.moveModifiers();
            PropertyMoveModifiers thisUnitMove = data.properties.moveModifiers();

            return otherUnitMove.otherMoveThroughYou || thisUnitMove.moveThroughUnits;
        }

        public override float expectedMoveDamage(IntVector2 from, IntVector2 to)
        {
            float damage = 0;
            
            var objloop = toggRef.Square(to).objLoop();
            while (objloop.next())
            {
                damage += objloop.sel.ExpectedMoveDamage(this);                
            }

            return damage;
        }

        public override void AddToUnitCard(UnitCardDisplay card, ref Vector2 position)
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
                if (toggRef.mode == GameMode.HeroQuest)
                {
                    if (data.hero != null)
                    {
                        if (card.settings.stamina)
                        {
                            card.staminaHighlightPositions = card.valueBar(ref position, SpriteName.cmdStaminaValueBox,
                                data.hero.stamina, new StaminaTooltip());
                        }

                        if (card.settings.bloodrage)
                        {
                            card.valueBar(ref position, SpriteName.cmdBloodrageValueBox,
                                data.hero.bloodrage, new BloodRageTooltip());
                        }
                    }
                }
            }

            if (data.properties.HasMembers())//settings.properties && 
            {
                card.properties(ref position, data.properties);
                //card.spaceY(ref position);
            }
            condition.AddToUnitCard(card, ref position);
        }

        public void staminaToRichbox(List<HUD.RichBox.AbsRichBoxMember> richBox)
        {
            valuebarToRichbox(data.hero.stamina, new StaminaTooltip(), richBox);
        }
        public void bloodrageToRichbox(List<HUD.RichBox.AbsRichBoxMember> richBox)
        {
            valuebarToRichbox(data.hero.bloodrage, new BloodRageTooltip(), richBox);
        }

        void valuebarToRichbox(ValueBar value, AbsValuebarTooltip tooltip, List<HUD.RichBox.AbsRichBoxMember> richBox)
        {
            richBox.Add(new HUD.RichBox.RichBoxText(value.BarText(false)));

            for (int i = 0; i < value.maxValue; ++i)
            {
                bool available = i < value.Value;
                SpriteName sprite = available ? tooltip.Icon : tooltip.IconGrayed;

                richBox.Add(new HUD.RichBox.RichBoxImage(sprite));
            }
        }

        override public Commander.GO.Unit cmd()
        {
            throw new InvalidCastException();
        }

        override public HeroQuest.Unit hq()
        {
            return this;
        }

        public override void Alert()
        {
            if (!aiAlerted)
            {
                aiAlerted = true;
                PlayerHQ.AlertedUnit(this);
            }
        }

        public bool IsHero => data.hero != null;

        public override AbsUnitData Data { get => data; set => data = value as HqUnitData; }
    }
}
