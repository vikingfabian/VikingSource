using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.ToggEngine.Map;

namespace VikingEngine.ToGG.Commander.Players
{
    abstract class AbsLocalPlayer : AbsCmdPlayer
    {
        protected Stack<AbsGamePhase> previousPhases = new Stack<AbsGamePhase>();

        public AbsGamePhase gamePhase = null;
        public AbsCommandCard commandCard;
        //public AttackTargetCollection attackTargets = null;


        public AbsLocalPlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup setup)
            : base(pData, globalIndex, setup)
        {
            
        }

        //public override void prevPhaseAction()
        //{
        //    gamePhase.goToGoToPreviousPhase();
        //}

        override public void nextPhase(bool forwardToNext)
        {
            if (forwardToNext)
            {
                //if (gamePhase.Type == Commander.GamePhaseType.GiveOrder)
                //{
                //    unitsCounter.Reset();
                //    while (unitsCounter.Next())
                //    {
                //        unitsCounter.Member.Resting = false;
                //    }
                //}

                closePhase();

                if (nextPhaseIsTurnOver())
                {
                    toggRef.gamestate.BeginNextPlayer();
                }
                else
                { //just move to next phase
                    bool autoEnableAllUnits;
                    var next = getNextPhase(gamePhase.Type, out autoEnableAllUnits);

                    if (autoEnableAllUnits)
                    {
                        commandCard.orders.EnableAll(true);
                    }

                    StartPhase(next);
                }
            }
            else //Back to Previous
            {
                if (previousPhases.Count > 0)
                {
                    //gamePhase.returnToThisPhaseBeginning();
                    gamePhase.returnToThisPhaseBeginning();

                    AbsGamePhase prev = previousPhases.Pop();
                    //prev.returnToThisPhaseBeginning();
                    StartPhase(prev.Type);
                }
            }
        }

        virtual protected void closePhase()
        {
            gamePhase.OnExitPhase(true);
        }

        public GamePhaseType getNextPhase(GamePhaseType current, out bool autoEnableAllUnits)
        {
            bool hasMovePhase;
            bool hasAttackPhase;

            CommandCard.AbsCommandCard.HasPhase(settings.commandCardDeck.SelectedCommand, out hasMovePhase, out hasAttackPhase);


            if (!hasMovePhase && 
                current == ToGG.GamePhaseType.GiveOrder)
            {
                autoEnableAllUnits = true;
                return GamePhaseType.Attack;
            }
            else
            {
                autoEnableAllUnits = false;
                return current + 1;
            }
        }

        //protected void beginEndTurn()
        //{
        //    collectVP();
        //    cmdRef.gamestate.NextPlayer();
        //}



        public bool nextPhaseIsTurnOver()
        {
            if (gamePhase.Type == GamePhaseType.Deployment || gamePhase.Type == ToGG.GamePhaseType.Attack)
                return true;
            else if ((settings.commandCardDeck.SelectedCommand == CommandType.Wide_Advance || settings.commandCardDeck.SelectedCommand == CommandType.Rush) &&
                gamePhase.Type == ToGG.GamePhaseType.Move)
            {
                return true;
            }
            else if (gamePhase.Type == ToGG.GamePhaseType.Move ||
                (gamePhase.Type == ToGG.GamePhaseType.GiveOrder && settings.commandCardDeck.SelectedCommand == CommandType.Dark_Sky))
            {//if no attacks are available, end the player turn
                bool endTurn = !updateAttackAbility(false, false);
                return endTurn;
            }
            else if (gamePhase.Type == ToGG.GamePhaseType.GiveOrder)
            {
                return commandCard == null || commandCard.IsEmpty();
            }
            else
                return false;
        }

        protected List<IntVector2> attackAblePositions = new List<IntVector2>();

        public bool updateAttackAbility(bool updateOrders, bool refreshHighlights)
        {
            
            attackAblePositions.Clear();
            bool canAttack = false;
            if (commandCard != null)
            {
                foreach (OrderedUnit order in commandCard.orders.list)
                {
                    bool enableUnit = false;
                    if (!order.unit.AttackedThisTurn && order.unit.canAttackSkillCheck())
                    {
                        order.attackTargets = new AttackTargetCollection(order.unit, order.unit.squarePos);
                        if (order.attackTargets.targets.Count > 0)
                        {
                            canAttack = true;
                            enableUnit = true;
                            attackAblePositions.Add(order.unit.squarePos);
                        }
                    }
                    if (updateOrders)
                    {
                        order.CheckList_Enabled = enableUnit;
                      //  order.checkMark.Visible = enableUnit;
                    }
                }
            }
            //attackTargets = null;
            if (refreshHighlights)
            {
                removeAvailableTiles();

                highlightAbleToAttack();
            }
            return canAttack;
        }

        virtual protected void removeAvailableTiles()
        { }

        virtual public void highlightAbleToAttack()
        { }

        override public GamePhaseType GamePhase
        {
            get
            {
                if (gamePhase == null)
                    return ToGG.GamePhaseType.Init;
                else
                    return gamePhase.Type;
            }
        }

        public List<IntVector2> listDeploymentTiles(bool forAutoDeploy)
        {
            

            List<IntVector2> deploymentTiles = new List<IntVector2>(64);
            ForXYLoop loop = new ForXYLoop(toggRef.board.Size);
            while (loop.Next())
            {
                if (toggRef.board.tileGrid.Get(loop.Position).playerPlacement == pData.globalPlayerIndex)
                {
                    deploymentTiles.Add(loop.Position);
                }
            }

            return deploymentTiles;
        }

        public override void onTurnStart(bool deployPhase)
        {
            if (!deployPhase)
            {
                checkTerrainTrample();
            }

            base.onTurnStart(deployPhase);
        }

        void checkTerrainTrample()
        {
            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                if (unitsColl.unitsCounter.sel.OnSquare.HasProperty(TerrainPropertyType.Trample))
                {
                    unitsColl.unitsCounter.sel.OnSquare.destroyTerrain();
                }
            }
        }
        
        public override void EndTurn()
        {
            toggRef.gamestate.gameSetup.WinningConditions.onEndTurn(this);

            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                if (unitsColl.unitsCounter.sel.OnSquare.tileObjects.HasObject(TileObjectType.EscapePoint) &&
                    this is LocalPlayer)
                {
                    escapedUnits.Add(unitsColl.unitsCounter.sel);
                    unitsColl.unitsCounter.sel.DeleteMe();
                }
                unitsColl.unitsCounter.sel.Resting = false;
            }

            collectBannerVP();
            //new NetActionEndTurn_Host(this);

            base.EndTurn();
        }
    }
}
