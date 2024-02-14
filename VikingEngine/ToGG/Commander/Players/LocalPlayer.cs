using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine;
using VikingEngine.ToGG.ToggEngine.GO;
//xna

namespace VikingEngine.ToGG.Commander.Players
{
    class LocalPlayer : AbsLocalPlayer
    {
        public const int PlayerNumber_NoPlayer = -1;
        public const int PlayerNumberOne = 0;
        public const int PlayerNumberTwo = 1;

        //public MapControls mapControls;

        //int playerIndex;
        public AbsOverridingGamePhase overridingGamePhase = null;

        public LocalPlayer(Engine.AbsPlayerData pData, int globalIndex, Data.PlayerSetup setup)
            :base(pData, globalIndex, setup)
        {
            //this.playerIndex = playerIndex;
            //inputMap = Engine.XGuide.GetPlayer(playerIndex).inputMap;
            relationVisuals.setLocalUser();

            mapControls = new MapControls(this);
            mapControls.setMapselectionVisible(false);
        }

        protected override void closePhase()
        {
            if (gamePhase.Type != GamePhaseType.Deployment && 
                !gamePhase.autoSkippedPhase)
            {
                previousPhases.Push(gamePhase);
            }
            base.closePhase();
        }

        public override void onTurnStart(bool deployPhase)
        {
            Hud.viewLocalPlayerHud();
            previousPhases.Clear();
            base.onTurnStart(deployPhase);
        }

        override public void StartPhase(GamePhaseType phase)
        {
            mapControls.removeToolTip();
            gamePhase = AbsGamePhase.GetPhase(phase, this);
            
            if (!toggRef.menu.Open)
                mapControls.setMapselectionVisible(true);
        }

        //override public string Name()
        //{
        //    return Engine.XGuide.GetPlayer(playerIndex).PublicName;
        //}

        override public void onOpenMenu(bool open)
        {
            if (gamePhase != null)
            {
                Commander.cmdRef.hud.onOpenMenu(open, gamePhase.Type);
            }
        }
        
        override public void Update()
        {
            if (toggRef.menu.Open)
            {
                if (toggRef.menu.Update() ||
                    toggRef.inputmap.menuInput.openCloseInputEvent() ||//inputMap.openCloseMenuKeyInput() ||
                    toggRef.inputmap.moreInfo.DownEvent)//cmdLib.Button_MoreInfo.DownEvent)
                {
                    toggRef.menu.CloseMenu();
                }
            }
            else
            {
                PhaseUpdateArgs updateArgs = new PhaseUpdateArgs();

                if (overridingGamePhase != null)
                {
                    if (overridingGamePhase.update())
                    {
                        overridingGamePhase.DeleteMe();
                        overridingGamePhase = null;
                    }

                    return;
                }
                else if (Commander.cmdRef.hud.OverridingUpdate())
                {
                    return;
                }
                else if (gamePhase.OverridingUpdate(ref updateArgs))
                {
                    return;
                }
                else if (toggRef.inputmap.menuInput.openCloseInputEvent())//cmdLib.Button_OpenMenu.DownEvent)//inputMap.openCloseMenuKeyInput())
                {
                    toggRef.menu.MainMenu();
                    return;
                }
                else
                {
                   
                    //bool mouseOverHud = false;

                    

                    gamePhase.Update(ref updateArgs);
                    updateGui(ref updateArgs);
                    updateHud(ref updateArgs);
                    if (updateArgs.exitUpdate)
                    {
                        return;
                    }

                    Hud.extendedTooltip.update(updateArgs.ViewToolTip);
                    mapControls.prevMouseOverHud = updateArgs.mouseOverHud;
                }
            }
        }

        //public bool updateSpyGlass()
        //{
        //    mapControls.updateMapMovement(false);

        //    if (Input.Mouse.ButtonDownEvent(MouseButton.Right))
        //    {
        //        Commander.cmdRef.hud.spyglass.Visible = true;
        //        return true;
        //    }

        //    if (Input.Mouse.ButtonDownEvent(MouseButton.Left))
        //    {
        //        toggRef.menu.squareInfoMenu(mapControls.selectionIntV2);
        //        Commander.cmdRef.hud.spyglass.Visible = true;
        //        return true;
        //    }

        //    return false;
        //}

        EnableType prevCanExit = EnableType.NON;
        bool prevTurnOver = true;
        void updateHud(ref PhaseUpdateArgs args)
        {
            
            Hud.Update(ref args);
            if (args.exitUpdate)
            { return; }

            if (commandCard != null)
            {
                EnableType canExit = gamePhase.canExitPhase();
                bool turnOver = nextPhaseIsTurnOver();
                if (canExit != prevCanExit || turnOver != prevTurnOver)
                {
                    prevCanExit = canExit;
                    prevTurnOver = turnOver;

                    if (gamePhase.Type == ToGG.GamePhaseType.SelectArmy)
                    {
                        Commander.cmdRef.hud.setInputPromptVisible(true);
                    }
                    else
                    {
                        Commander.cmdRef.hud.hideInputPrompt();
                    }

                    args.refreshPhaseStatus = true;
                    
                }
            }


            if (args.refreshPhaseStatus)
            {
                EnableType canExit = gamePhase.canExitPhase();
                Commander.cmdRef.hud.setNextPhaseStatus(canExit, nextPhaseIsTurnOver(), this);
                Commander.cmdRef.hud.setPrevPhaseStatus(canGoToPreviousPhase());
            }
        }

        void updateGui(ref PhaseUpdateArgs args)
        {
            if (mapControls.isOnNewTile)
            {
                UnitMoveAndAttackGUI.Clear();

                if (mapControls.SelectedUnit != null && args.ViewToolTip)
                {
                    new UnitMoveAndAttackGUI(mapControls.SelectedUnit, mapControls.selectionIntV2, false, false);
                }
            }
        }

        bool canGoToPreviousPhase()
        {
            return previousPhases.Count > 0 && gamePhase != null && gamePhase.canGoToPreviousPhase();
        }


        public override void nextPhaseButtonAction(bool forwardToNext)
        {
            if (forwardToNext)
            {
                EnableType canExit = gamePhase.canExitPhase();

                bool canForward = canExit == EnableType.Able_NotRecommended || canExit == EnableType.Enabled;

                if (!canForward)
                {
                    return;
                }
            }

            base.nextPhaseButtonAction(forwardToNext);
        }

        void closeMenu()
        {
            toggRef.menu.CloseMenu();
        }

        public override void UpdateSpectating()
        {
            mapControls.zoomInput();
            toggRef.cam.spectate(Commander.cmdRef.players.allPlayers.Selected().SpectatorTargetPos);
        }

        private void viewHudForMenu(bool openMenu)
        {
            if (openMenu)
            {
                //Commander.cmdRef.hud.RemoveTileInfoCard();
                //Commander.cmdRef.hud.buttonsOverview.DeleteAll();
                mapControls.setMapselectionVisible(false);
                Commander.cmdRef.hud.hideInputPrompt();
            }
            else
            {
                mapControls.setMapselectionVisible(true);
                onNewTile();
            }
        }

        public void selectUnit(AbsUnit u)
        {
            movingUnit = u;
            bool select = movingUnit != null;

            mapControls.setAvailable(select); 
            gamePhase.OnSelectUnit(select);
        }

        public void focusOnCommandedUnit()
        {
            OrderedUnit order = commandCard.orders.Selected();
            if (order != null)
                mapControls.setSelectionPos(order.unit.soldierModel.Position);
        }

        protected override void removeAvailableTiles()
        {
            mapControls.removeAvailableTiles();
        }

        override public void highlightAbleToAttack()
        {
            mapControls.SetAvailableTiles(attackAblePositions);
        }


        public void markAvailableCommandUnits()
        {
            mapControls.SetAvailableTiles(commandCard.EnabledTiles());
        }

        public bool myUnit()
        {
            return myUnit(mapControls.SelectedUnit);
        }
        public bool myUnit(AbsUnit u)
        {
            return u != null && u.globalPlayerIndex == pData.globalPlayerIndex;
        }

        override public void onNewTile()
        {
            //bool removeHoverDots = true;
            Hud.extendedTooltip.onNewTile();
            //mapControls.pointer.SetSpriteName(SpriteName.cmdPointer);

            ToggEngine.Display2D.UnitCardDisplay.CreateCardDisplay(mapControls.selectionIntV2, toggRef.hud);
            //Commander.cmdRef.hud.RemoveTileInfoCard();

            //if (mapControls.selectedTile.unit == null)
            //{
            //    var square = toggRef.board.tileGrid.Get(mapControls.selectionIntV2);

            //    if (square.tileObjects.HasObject(TileObjectType.TacticalBanner))
            //    {
            //        Commander.cmdRef.hud.tileInfoCard = toggLib.VpSquareCard();
            //    }
            //    else if (square.tileObjects.HasObject(TileObjectType.EscapePoint))
            //    {
            //        Commander.cmdRef.hud.tileInfoCard = toggLib.EscapePointCard();
            //    }
            //    else
            //    {
            //        Commander.cmdRef.hud.tileInfoCard = mapControls.selectedTile.Card(null);
            //    }
            //}
            //else //On Unit
            //{
            //    Commander.cmdRef.hud.tileInfoCard = mapControls.selectedTile.unit.Card(null, null);
            //    if (gamePhase == null || gamePhase.ViewHoverUnitDisplay)
            //    {
            //        //removeHoverDots = false;
            //        new UnitMoveAndAttackGUI(mapControls.selectedTile.unit, mapControls.selectionIntV2, 
            //            false, false);
            //    }
            //}

            ////if (removeHoverDots)
            ////{
            ////    cmdRef.gamestate.hud.removeHoverUnitMoveAndAttackDots();
            ////}

            //if (Commander.cmdRef.hud.tileInfoCard != null)
            //{
            //    Commander.cmdRef.hud.tileInfoCard.ParentPosition =  Commander.cmdRef.hud.viewArea.LeftBottom;
            //}
            if (gamePhase != null)
                gamePhase.onNewTile();
        }

        Display.PlayerHUD Hud { get { return Commander.cmdRef.hud; } } 

        override public void EndTurn()
        {
            base.EndTurn();

            Hud.clearAll();
            UnitMoveAndAttackGUI.Clear();
            mapControls.EndTurn();
            gamePhase = null;
        }

        protected override Vector2 selectionPos
        {
            get
            {
                return mapControls.selectionV2;
            }
        }

        override public bool IsScenarioOpponent { get { return false; } }

        public override bool LocalHumanPlayer => true;
    }
}
