using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.ToggEngine.Map;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.HUD.RichBox;
using VikingEngine.ToGG.Commander.UnitsData;

namespace VikingEngine.ToGG.Commander.Battle
{
    class BattleAttackerSetup : AbsBattleAttackerSetup
    {
    }

    class BattleDefenderSetup : AbsBattleDefenderSetup
    {
    }

    

    class BattleSetup : AbsBattleSetup
    {
        //AttackType AttackType;
        Data.Property.AbsUnitProperty unitProperty;
        AbsUnitCondition condition;
        TerrainProperty terrainProperty;

        public BattleAttackerSetup attackerSetup = new BattleAttackerSetup();
        //protected List<BattleDefenceModifier> defence = new List<BattleDefenceModifier>();

        public BattleModifierLabel battleModifierAttack = new BattleModifierLabel();
        public BattleModifierLabel battleModifierDefence = new BattleModifierLabel();
        bool UseDefenderTerrain => !backStab;
        bool backStab;

        public BattleDice dice;
        //public BattleAttackAndDefenceCollection()
        //{ }
        public BattleSetup(List<AbsUnit> attacker, AttackTarget target, CommandCard.CommandType CommandType)
            : base(attacker, new AttackTargetGroup(target))
        {
            BattleDice originalDice;

            if (target.attackType.Is(AttackUnderType.BackStab))
            {
                originalDice = Commander.BattleLib.BackstabDie;
            }
            else if (target.attackType.IsMelee)
            {
                originalDice = Commander.BattleLib.MeleeDie;
            }
            else
            {
                originalDice = Commander.BattleLib.RangedDie;
            }
            dice = originalDice.Clone();

            getDiceValues();

            this.CommandType = CommandType;
            backStab = target.attackType.Is(AttackUnderType.BackStab);
            //useDefenderTerrain = !target.attackType.Is(AttackUnderType.BackStab);

            calcDefences();
            calcAttackCount();

            if (backStab)
            {
                calcBackstabHitChance();
            }
            else
            {
                calcHitChance();
            }

            setDiceValues();
        }

        void getDiceValues()
        {
            BattleDiceSide? hitSide = dice.getSide(BattleDiceResult.Hit1);
            if (hitSide == null)
            {
                hitChance = 0;
            }
            else
            {
                hitChance = hitSide.Value.chance;
            }
            BattleDiceSide? retreatSide = dice.getSide(BattleDiceResult.Retreat);
            if (retreatSide == null)
            {
                retreatChance = 0;
            }
            else
            {
                retreatChance = retreatSide.Value.chance;
            }
        }

        void setDiceValues()
        {
            dice.setChance(BattleDiceResult.Hit1, hitChance);
            dice.setChance(BattleDiceResult.Retreat, retreatChance);
        }

        protected void calcDefences()
        {            

            var defender = targets.First.unit;

            if (defender != null)
            {
                if (UseDefenderTerrain && defender.OnSquare.TryGetProperty(TerrainPropertyType.SittingDuck, out terrainProperty))
                {
                    battleModifierDefence.SourceProperty(terrainProperty);
                    battleModifierDefence.Arrow();
                    battleModifierDefence.text("No defence");
                }
                else
                {
                    if (MainAttacker.HasProperty(UnitPropertyType.Pierce) == false)
                    {
                        if (defender.TryGetProperty(UnitPropertyType.Block, out unitProperty))
                        {
                            battleModifierDefence.SourceProperty(unitProperty);
                            battleModifierDefence.Arrow();
                            battleModifierDefence.ResultBlock();
                            hitBlocks++;
                        }
                        if (defender.TryGetProperty(UnitPropertyType.Max_Level, out unitProperty))
                        {
                            battleModifierDefence.SourceProperty(unitProperty);
                            battleModifierDefence.Arrow();
                            battleModifierDefence.ResultBlock();
                            hitBlocks++;
                        }
                        if (targets.AttackType.IsRanged && defender.TryGetProperty(UnitPropertyType.Arrow_Block, out unitProperty))
                        {
                            battleModifierDefence.SourceProperty(unitProperty);
                            battleModifierDefence.Arrow();
                            battleModifierDefence.ResultBlock();
                            hitBlocks++;
                        }
                        if (UseDefenderTerrain && defender.OnSquare.TryGetProperty(TerrainPropertyType.Block1, out terrainProperty))
                        {
                            //Terrain protection, if not the same terrain for both
                            if (defender.OnSquare.squareType != MainAttacker.OnSquare.squareType)
                            {
                                battleModifierDefence.SourceProperty(terrainProperty);
                                battleModifierDefence.Arrow();
                                battleModifierDefence.ResultBlock();
                                hitBlocks++;
                            }
                        }
                    }

                    defenderIgnoreReteats(defender);
                }
            }
        }

        public void defenderIgnoreReteats(AbsUnit unit)
        {
            if (backStab)
            {
                return;
            }

            if (unit.HasProperty(UnitPropertyType.Base))
            {
                WontRetreat = true;
                return;
            }

            List<AbsUnit> friendlySupporters = new List<AbsUnit>(8);

            //will ignore retreat when adjacent to friendly units or when adjacent to leader
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = unit.squarePos + dir;
                if (toggRef.board.tileGrid.InBounds(pos))
                {
                    BoardSquareContent t = toggRef.board.tileGrid.Get(pos);
                    if (t.unit != null && t.unit.globalPlayerIndex == unit.globalPlayerIndex)
                    {
                        friendlySupporters.Add(t.unit);
                        if (t.unit.TryGetProperty(UnitPropertyType.Leader, out unitProperty))
                        {
                            battleModifierDefence.SourceProperty(unitProperty);
                            battleModifierDefence.Arrow();
                            battleModifierDefence.ResultIgnoreRetreat();
                            retreatIgnores++;
                        }
                    }
                }
            }

            if (friendlySupporters.Count >= 2)
            {
                battleModifierDefence.SourceDescription("2 friendly adjacent units");
                battleModifierDefence.Arrow();
                battleModifierDefence.ResultIgnoreRetreat();
                retreatIgnores++;
            }
        }

        

        protected void calcAttackCount()
        {
            if (backStab)
            {
                supportingUnits = new AttackSupport();
            }
            else
            {
                attackerSetup.attackStrength = MainAttacker.Data.WeaponStats.Strength(targets.IsMelee);
                supportingUnits = collectAttackingSupporters(MainAttacker, targets.sel);

                if (supportingUnits.total > 0)
                {
                    battleModifierAttack.icon(SpriteName.cmdSupporterIcon1);
                    battleModifierAttack.text("Support");
                    battleModifierAttack.Arrow();
                    battleModifierAttack.text("+" + supportingUnits.total.ToString());
                    battleModifierAttack.icon(SpriteName.cmdDiceMelee);
                }
            }            
            
            if (targets.AttackType.mainType == AttackMainType.Melee)
            {
                if (targets.AttackType.underType == AttackUnderType.BackStab)
                {
                    attackerSetup.attackStrength = attacker.Count;
                }
                else
                {
                    if (MainAttacker.TryGetProperty(UnitPropertyType.Charge, out unitProperty) &&
                       MainAttacker.MovedThisTurn)
                    {
                        battleModifierAttack.SourceProperty(unitProperty);
                        battleModifierAttack.Arrow();
                        battleModifierAttack.ResultAttackModifier(1);
                        attackerSetup.attackStrength++;
                    }
                    if (CommandType == CommandCard.CommandType.Close_encounter)
                    {
                        battleModifierAttack.SourceCommand(CommandType);
                        battleModifierAttack.Arrow();
                        battleModifierAttack.ResultAttackModifier(1);
                        attackerSetup.attackStrength++;
                    }
                }
            }
            else //Ranged
            {
                if (CommandType == CommandCard.CommandType.Dark_Sky)
                {
                    battleModifierAttack.SourceCommand(CommandType);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultAttackModifier(1);
                    attackerSetup.attackStrength++;
                }

                if (MainAttacker.TryGetProperty(UnitPropertyType.Aim, out unitProperty) &&
                    !MainAttacker.MovedThisTurn)
                {
                    battleModifierAttack.SourceProperty(unitProperty);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultAttackModifier(toggLib.AimPropertyBonus);
                    attackerSetup.attackStrength += toggLib.AimPropertyBonus;
                }
            }                 
            

            if (!backStab)
            {
                if (MainAttacker.OnSquare.TryGetProperty(TerrainPropertyType.ReducedTo1, out terrainProperty))
                {
                    battleModifierAttack.SourceProperty(terrainProperty);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultAttackReducedTo(1);
                    setReducedAttacks(1);
                }
                else if (MainAttacker.OnSquare.TryGetProperty(TerrainPropertyType.ReducedTo2, out terrainProperty))
                {
                    battleModifierAttack.SourceProperty(terrainProperty);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultAttackReducedTo(2);
                    setReducedAttacks(2);
                }
            }

            attackerSetup.baseAttackStrength = attackerSetup.attackStrength;
            attackerSetup.attackStrength = attackerSetup.baseAttackStrength + supportingUnits.total;
        }


        void setReducedAttacks(int reducedTo)
        {
            attackerSetup.attackStrength = Bound.Max(attackerSetup.attackStrength, reducedTo);
        }



        public void calcHitChance()
        {
            var defender = targets.First.unit;

            if (defender != null)
            {

                //hitChance = dice.getSide(BattleDiceResult.Hit1).Value.chance; retreatChance = dice.getSide(BattleDiceResult.Retreat).Value.chance;

                if (defender.OnSquare.TryGetProperty(TerrainPropertyType.SittingDuck, out terrainProperty))
                {
                    battleModifierAttack.SourceProperty(terrainProperty);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultSetHitChance(1f);
                    hitChance = 1f;
                    retreatChance = 0f;
                }
                else if (defender.TryGetProperty(UnitPropertyType.Static_target, out unitProperty))
                {
                    battleModifierAttack.SourceProperty(unitProperty);
                    battleModifierAttack.Arrow();
                    battleModifierAttack.ResultSetHitChance(1f);
                    hitChance = 1f;
                    retreatChance = 0f;
                }
                else
                {
                    if (targets.AttackType.IsMelee)
                    {
                        if (MainAttacker.TryGetProperty(UnitPropertyType.Light, out unitProperty))
                        {
                            battleModifierAttack.SourceProperty(unitProperty);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.ResultSetHitChance(toggLib.CloseCombatHitChance_LightUnit);
                            battleModifierAttack.ResultSetRetreatChance(toggLib.CloseCombatRetreatChance_LightUnit);

                            hitChance = toggLib.CloseCombatHitChance_LightUnit;
                            retreatChance = toggLib.CloseCombatRetreatChance_LightUnit;
                        }
                        //else
                        //{
                        //    hitChance = toggLib.CloseCombatHitChance;
                        //    retreatChance = toggLib.CloseCombatRetreatChance;
                        //}

                        if (targets.AttackType.Is(AttackUnderType.BackStab) == false)
                        {
                            if (MainAttacker.TryGetProperty(UnitPropertyType.Shield_dash, out unitProperty))
                            {
                                battleModifierAttack.SourceProperty(unitProperty);
                                battleModifierAttack.Arrow();
                                battleModifierAttack.ResultAddRetreatChance(toggLib.ShieldDashPropertyRetreatBonus);
                                retreatChance += toggLib.ShieldDashPropertyRetreatBonus;
                            }
                        }
                    }
                    else //Ranged attack
                    {
                        hitChance = toggLib.RangedCombatHitChance;
                        retreatChance = toggLib.RangedCombatRetreatChance;
                    }


                    //if (!backStab)
                    {//Both ranged and CC
                        if (defender.cmd().TryGetCondition(UnitPropertyType.OpenTarget, out condition))
                        {
                            battleModifierAttack.SourceProperty(condition);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.ResultAddHitChance(OpenTargetCondition.AddChanceToHit);
                            hitChance += OpenTargetCondition.AddChanceToHit;
                        }

                        if (defender.TryGetProperty(UnitPropertyType.Cant_retreat, out unitProperty) ||
                            defender.TryGetProperty(UnitPropertyType.Base, out unitProperty))
                        {
                            battleModifierAttack.SourceProperty(unitProperty);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.ResultSetRetreatChance(0);
                            hitChance += retreatChance;
                            retreatChance = 0f;
                        }

                        if (MainAttacker.TryGetProperty(UnitPropertyType.Fear, out unitProperty) &&
                            !targets.AttackType.Is(AttackUnderType.BackStab))
                        {
                            battleModifierAttack.SourceProperty(unitProperty);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.text("Swap %");
                            battleModifierAttack.icon(BattleDice.ResultIcon(BattleDiceResult.Hit1));
                            battleModifierAttack.text(" and %");
                            battleModifierAttack.icon(BattleDice.ResultIcon(BattleDiceResult.Retreat));

                            float store = hitChance;
                            hitChance = retreatChance;
                            retreatChance = store;
                        }

                        if (MainAttacker.TryGetProperty(UnitProgress.LevelProperty, out unitProperty))
                        {
                            battleModifierAttack.SourceProperty(unitProperty);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.ResultAddHitChance(toggLib.LevelUpHitChanceBonus);
                            hitChance += toggLib.LevelUpHitChanceBonus;
                        }

                        if (defender.TryGetProperty(UnitPropertyType.Slippery, out unitProperty) && !targets.AttackType.Is(AttackUnderType.BackStab))
                        {
                            battleModifierAttack.SourceProperty(unitProperty);
                            battleModifierAttack.Arrow();
                            battleModifierAttack.ResultSetHitChance(0);

                            retreatChance += hitChance;
                            hitChance = 0f;
                        }
                    }

                    collectHitChanceSupport();
                }
            }

            //Check adjacent unit boosts
            foreach (IntVector2 dir in IntVector2.Dir8Array)
            {
                IntVector2 pos = dir + MainAttacker.squarePos;
                BoardSquareContent square;
                if (toggRef.board.tileGrid.TryGet(pos, out square))
                {
                    if (square.unit != null && square.unit.globalPlayerIndex == MainAttacker.globalPlayerIndex)
                    {
                        //Friendly unit
                        if (square.unit.TryGetProperty(UnitPropertyType.Leader, out unitProperty))
                        {
                            if (retreatChance > 0f)
                            {
                                battleModifierAttack.SourceProperty(unitProperty);
                                battleModifierAttack.Arrow();
                                battleModifierAttack.ResultAddRetreatChance(toggLib.LeaderPropertyRetreatBonus);
                                retreatChance += toggLib.LeaderPropertyRetreatBonus;
                            }

                            break;
                        }
                    }
                }
            }
        }

        public void calcBackstabHitChance()
        {
            retreatChance = 0f;

            if (TryGetProperty(true, UnitPropertyType.Backstab_expert, out unitProperty))
            {
                battleModifierAttack.SourceProperty(unitProperty);
                battleModifierAttack.Arrow();
                battleModifierAttack.ResultSetHitChance(1f);
                hitChance = 1f;
            }
        }

        bool TryGetProperty(bool bAttacker, UnitPropertyType type, out Data.Property.AbsUnitProperty unitProperty)
        {
            var units = bAttacker ? attacker : targets.listUnits();

            foreach (var m in units)
            {
                if (m.TryGetProperty(type, out unitProperty))
                {
                    return true;
                }
            }

            unitProperty = null;
            return false;
        }

        void collectHitChanceSupport()
        {
            

            if (targets.AttackType.Is(true, AttackUnderType.None) || targets.AttackType.Is(true, AttackUnderType.CounterAttack))
            {
                var friendlyUnits = toggRef.absPlayers.getGenericPlayer(MainAttacker.globalPlayerIndex).unitsColl.units.counter();

                while (friendlyUnits.Next())
                {
                    if (friendlyUnits.sel.TryGetProperty(UnitPropertyType.Fear_support, out unitProperty) && 
                        friendlyUnits.sel.AdjacentTo(targets.First.unit))
                    {
                        battleModifierAttack.SourceProperty(unitProperty);
                        battleModifierAttack.Arrow();
                        battleModifierAttack.ResultAddRetreatChance(toggLib.FearSupportPropertyRetreatBonus);
                        retreatChance += toggLib.FearSupportPropertyRetreatBonus;
                        return;
                    }
                }
            }
        }

        

        

        public override AbsBattleAttackerSetup AttackerSetup => attackerSetup;

        //public bool IsMelee
        //{
        //    get
        //    {
        //        return attackType != AttackType.Ranged;
        //    }
        //}

        //public void write(System.IO.BinaryWriter w)
        //{
        //    attacker.writeIndex(w);
        //    targets.First.unit.writeIndex(w);
        //    // w.Write((byte)attackType);
        //    targets.AttackType.write(w);
        //    w.Write((byte)attackStrength);

        //    DataStream.DataStreamLib.WritePercent(retreatChance, w);
        //    DataStream.DataStreamLib.WritePercent(hitChance, w);

        //    supportingUnits.write(w);
        //}

        //public void read(System.IO.BinaryReader r, out AbsGenericPlayer player)
        //{
        //    attacker = toggRef.gamestate.GetUnit(r, out player);
        //    targets.First.unit = toggRef.gamestate.GetUnit(r, out player);
        //    targets.AttackType.read(r);// = (Battle.AttackType)r.ReadByte();
        //    attackStrength = r.ReadByte();

        //    retreatChance = DataStream.DataStreamLib.ReadPercent(r);
        //    hitChance = DataStream.DataStreamLib.ReadPercent(r);

        //    Battle.AttackSupport supportingUnits = new AttackSupport();
        //    supportingUnits.read(r, targets.AttackType);
        //}


    }
}
