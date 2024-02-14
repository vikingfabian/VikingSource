using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Commander;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using VikingEngine.ToGG.Commander.Battle;
using VikingEngine.ToGG.Commander.CommandCard;

namespace VikingEngine.ToGG
{   

    class GamePhase_Attack : Commander.GamePhase.AbsAttackPhase
    {
        //static bool AutoAttackSingleTarget = false;
        AttackTargetCollection attackTargets = null;

        AbsUnit aiAttacker;
        AttackTarget aiDefender;

        AiState aiState = 0;
        Time stateTime = 0;

        public GamePhase_Attack(Commander.Players.AbsLocalPlayer player)
            : base(player)
        {
            if (player.LocalHumanPlayer)
            {
                player.updateAttackAbility(true, true);
                player.commandCard.orders.Unselect();
            }

            PhaseMark();
        }

        public void PhaseMark()
        {
            foreach (OrderedUnit ord in CommandCard.orders.list)
            {
                SpriteName sprite;
                if (ord.attackTargets != null && ord.attackTargets.attackType.IsRanged)
                {
                    sprite = SpriteName.cmdUnitRangedGui;
                }
                else
                {
                    sprite = SpriteName.cmdUnitMeleeGui;
                }

                ord.PhaseMark(sprite, 1.5f);
            }
        }

        public override bool UpdateAi()
        {

            switch (aiState)
            {
                default:
                    aiState++;
                    new Timer.AsynchActionTrigger(asynchGetNextAttacker);
                    break;
                case AiState.RunningProcess:
                    //Waiting for asynch process
                    break;
                case AiState.ProcessComplete:
                    if (aiAttacker == null)
                    {
                        aiState = AiState.Exit;
                    }
                    else //start attack
                    {
                        aiPlayer.SpectatorTargetPos = (aiAttacker.squarePos + aiDefender.unit.squarePos) / 2;

                        absPlayer.getAttackerArrow().updateAttackerArrow(aiAttacker, aiDefender.unit.squarePos);

                        CommandCard.selectUnit(aiAttacker);
                        attackDisplay = new AttackDisplay(absPlayer, aiAttacker, aiDefender, settings.commandCardDeck.SelectedCommand);

                        aiState = AiState.AttackIntro;
                        stateTime.MilliSeconds = 300;
                    }
                    break;
                case AiState.AttackIntro:
                    if (stateTime.CountDown())
                    {
                        aiState = AiState.Attack;
                        attackDisplay.beginAttack();
                    }
                    break;
                case AiState.Attack:
                    if (updateAttack(ref PhaseUpdateArgs.None) == false)
                    {
                        aiState = AiState.AttackOutro;
                        stateTime.MilliSeconds = 300;
                    }
                    break;
                case AiState.AttackOutro:
                    if (stateTime.CountDown())
                    {
                        aiState = AiState.StartProcess;

                        //attackDisplay.DeleteMe();
                        //attackDisplay = null;
                        absPlayer.removeAttackerArrow();
                    }
                    break;
                case AiState.Exit:
                    return true;
            }

            return false;

            //if (updateAttack(ref args))
            //{
            //    return false;
            //}

            //if (aiNextAttackerProcess)
            //{
            //    if (aiNextAttackerComplete)
            //    {
            //        if (aiAttacker == null)
            //        {
            //            return true;
            //        }
            //        else //start attack
            //        {
            //            CommandCard.selectUnit(aiAttacker);
            //            attackDisplay = new AttackDisplay(absPlayer, aiAttacker, aiDefender, settings.commandCardDeck.SelectedCommand);
            //            attackDisplay.beginAttack();
            //            //beginAttackRoll(aiDefender);
            //        }
            //    }
            //}
            //else
            //{
            //    aiNextAttackerProcess = true;
            //    aiNextAttackerComplete = false;
            //    new Timer.AsynchActionTrigger(asynchGetNextAttacker);
            //}

            //return false;
        }


        
        //bool aiNextAttackerProcess = false;
        //bool aiNextAttackerComplete = false;

        void asynchGetNextAttacker()
        {
            AbsUnit mostValueAttacker = null;
            AttackTarget mostValueTarget = null;
            int mostValueAttack = int.MinValue;

            foreach (var order in CommandCard.orders.list)
            {
                if (!order.unit.AttackedThisTurn)
                {
                    attackTargets = new AttackTargetCollection(order.unit, order.unit.squarePos);
                    if (attackTargets.targets.Count > 0)
                    {
                        foreach (var target in attackTargets.targets.list)
                        {
                            int attackValue = order.unit.AttackWithUnitValue(target.unit, target.attackType.IsMelee);
                            if (attackValue > mostValueAttack)
                            {
                                mostValueAttack = attackValue;
                                mostValueAttacker = order.unit;
                                mostValueTarget = target;
                            }
                        }
                    }
                }
            }

            if (mostValueAttack > 0)
            {
                aiAttacker = mostValueAttacker;
                aiDefender = mostValueTarget;
            }
            else
            { //The attack is not worth the risk
                aiAttacker = null;
            }
            aiState = AiState.ProcessComplete;
        }

        

        public override bool OverridingUpdate(ref PhaseUpdateArgs args)
        {
            if (followUp != null)
            {
                if (followUp.update())
                {
                    removeFollowUp();
                }
                return true;
            }

            if (updateAttack(ref args))
            {
                return true;
            }

            return false;
        }

        public override void Update(ref PhaseUpdateArgs args)
        {
            //Attack sker i två steg
            //1. välj attackerare
            //2. välj target
            mapControls.updateMapMovement(true);

            if (SquareUpdate)
            {
                removeAttackDisplay();
                player.mapControls.removeToolTip();
            }
            
            if (attackTargets == null) //Select attacker
            {
                //if (catapultTargetGUI != null)
                //{
                //    catapultTargetGUI.DeleteMe();
                //    catapultTargetGUI = null;
                //}

                AbsUnit selectedUnit = mapControls.selectedTile.unit;

                bool availableSquare = CommandCard.containsEnabledUnit(selectedUnit);
                availableSquare = availableSquare && selectedUnit.canAttackSkillCheck();

                if (SquareUpdate)
                {
                    mapControls.setAvailable(availableSquare);
                    if (availableSquare)
                    {
                        new Commander.Display.PickAttackerTooltip(true, player);
                    }
                }

                if (availableSquare && toggRef.inputmap.click.DownEvent)
                {//select unit, and check its targets
                    
                    attackTargets = new AttackTargetCollection(selectedUnit, selectedUnit.squarePos);
                    
                    CommandCard.selectUnit(selectedUnit);
                    if (attackTargets.targets.Selected().unit != null)
                    {
                        mapControls.setSelectionPos(attackTargets.targets.Selected().unit.soldierModel.Position);
                    }
                    mapControls.SetAvailableTiles(attackTargets.targetPositions());
                    PhaseMarkVisible(false);
                }
            }
            else //select target
            {
                args.blockTooltip = true;
                absPlayer.getAttackerArrow().updateAttackerArrow(attackTargets.friendlyUnit, mapControls.selectionIntV2);
                
                //
                bool availableSquare;
                AbsUnit target = null;

                if (attackTargets.friendlyUnit.CatapultAttackType)
                {
                    int dist = (attackTargets.friendlyUnit.squarePos - mapControls.selectionIntV2).SideLength();
                    availableSquare = Bound.IsWithin(dist, attackTargets.friendlyUnit.Data.wep.projectileRange, attackTargets.friendlyUnit.Data.wep.projectileRange2);
                }
                else
                {
                    target = mapControls.selectedTile.unit;
                    availableSquare = attackTargets.Contains(target) != null;
                }

                if (SquareUpdate)
                {
                    MapSquareAvailableType availableType;
                    if (availableSquare)
                    {
                        availableType = MapSquareAvailableType.Enabled;
                        if (availableSquare)
                        {
                            new Commander.Display.PickAttackerTooltip(false, player);
                        }
                    }
                    else if (target == null)
                    {
                        availableType = MapSquareAvailableType.None;
                    }
                    else
                    {
                        availableType = MapSquareAvailableType.Disabled;
                    }

                    mapControls.setAvailable(availableType);

                    if (availableSquare)
                    {
                        createAttackDisplay();
                    }
                }
                //if (catapultTargetGUI != null)
                //{
                //    if (availableSquare)
                //    { catapultTargetGUI.setPosition(mapControls.selectionIntV2); }
                //    else
                //    { catapultTargetGUI.setVisible(false); }
                //}

                if (toggRef.inputmap.back.DownEvent)
                { //Cancel
                    clearAttackerSelection(true);
                    absPlayer.updateAttackAbility(false, true);
                    PhaseMarkVisible(true);
                    
                    player.mapControls.removeToolTip();
                }
                else if (availableSquare && toggRef.inputmap.click.DownEvent)
                {//start attack
                    attackDisplay.beginAttack(); 
                }
            }
        }

        void clearAttackerSelection(bool cancel)
        {
            attackDisplay?.DeleteMe();
            attackDisplay = null;

            attackTargets = null;
            //mapControls.removeAvailableTiles();
            //player.highlightAbleToAttack();
            //absPlayer.updateAttackAbility(!cancel, true);
            absPlayer.removeAttackerArrow();
        }

        private void createAttackDisplay()
        {
            var attacker = CommandCard.orders.Selected().unit;
            AbsUnit target = mapControls.selectedTile.unit;

            AttackTarget at;

            if (target == null)
            {
                at = new AttackTarget(mapControls.selectionIntV2);
            }
            else
            {
                at = attackTargets.GetTarget(target);
            }

            attackDisplay = new AttackDisplay(player, attacker, at, settings.commandCardDeck.SelectedCommand);
        }

        void removeAttackDisplay()
        {
            attackDisplay?.DeleteMe();
            attackDisplay = null;
        }

        //public override void updateButtonDesc()
        //{
        //    mapControls.removeToolTip();

        //    AbsUnit selectedUnit = mapControls.selectedTile.unit;
        //    if (attackTargets == null) //Select attacker
        //    {
                
        //        bool availableSquare = CommandCard.containsEnabledUnit(selectedUnit);

        //        if (availableSquare)
        //        {
        //            List<HUD.ButtonDescriptionData> ButtonDesc_SelectAttacker = new List<HUD.ButtonDescriptionData>();
        //            actionButton(ButtonDesc_SelectAttacker, "Select Attacker");

        //            Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_SelectAttacker);
        //        }
        //        else
        //        {
        //            viewBoardRoamButtonDesc();
        //        }
        //    }
        //    else //select target
        //    {
        //        List<HUD.ButtonDescriptionData> ButtonDesc_SelectTarget = new List<HUD.ButtonDescriptionData>();

        //        var attacker = CommandCard.orders.Selected().unit;
        //        mapControls.pointer.SetSpriteName(SpriteName.cmdPointerAttack);

        //        AbsUnit target = mapControls.selectedTile.unit;
        //        AttackTarget hasTargetOnSquare = attackTargets.Contains(target);

        //        if (hasTargetOnSquare != null)
        //        {
        //            actionButton(ButtonDesc_SelectTarget, "Select Target");

        //            //todo, order ska med
        //            AttackType attackType = new AttackType(
        //                hasTargetOnSquare.attackType.IsMelee ? AttackMainType.Melee : AttackMainType.Ranged,
        //                 AttackUnderType.None);

        //            new ToggEngine.Display2D.AttackTargetToolTip(attacker, target,
        //                attackType, player.mapControls, player.commandCard.CommandType);
        //        }

        //        cancelButton(ButtonDesc_SelectTarget);
        //        Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_SelectTarget);
        //    }
        //}

        public override void OnSelectUnit(bool select)
        {
            if (select)
            {
                mapControls.SetFramePos(player.movingUnit.squarePos);
            }
        }

        public override EnableType canExitPhase()
        {
            if (attackTargets != null || followUp != null)
            {
                return EnableType.Locked;
            }

            if (CommandCard.orders.HasEnabledMember())
            {
                return EnableType.Able_NotRecommended;
            }
            else
            {
                return EnableType.Enabled;
            }
        }

        public override void onNewTile()
        {
            if (followUp != null)
            {
                followUp.onNewTile();
            }
            else
            {
                base.onNewTile();
            }
        }

        public override void OnExitPhase(bool forwardToNext)
        {
            absPlayer.removeAttackerArrow();

            if (player != null)
            {
                player.mapControls.removeToolTip();
            }
            //if (player != null)
            //{
            //    absPlayer.attackerArrow.Visible = false;
            //}
        }

        override protected void OnAttackComplete()
        {
            base.OnAttackComplete();
            attackDisplay.attackRoll.MainAttacker.AttackedThisTurn = true;

            if (attackDisplay.AbleToFollowUp)
            {
                viewFollowUpPrompt();
            }
            else
            {
                absPlayer.updateAttackAbility(true, true);
                PhaseMarkVisible(true);
            }
            clearAttackerSelection(false);
            //aiNextAttackerProcess = false;
            
        }

        public override void EndTurnNotRecommendedText(out string title, out string message, out string okText)
        {
            title = EndTurnTitle;
            message = "You can still perform attacks";
            okText = EndTurnOkText;
        }

        protected override string name
        {
            get { return "Attacks"; }
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.Attack; }
        }

        override public bool ViewHoverUnitDisplay 
        { get { 
            return attackTargets == null && followUp == null; 
        } }

        enum AiState
        { 
            StartProcess,
            RunningProcess,
            ProcessComplete,

            AttackIntro,
            Attack,
            AttackOutro,

            Exit,
        }
    }
}
