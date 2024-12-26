using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.HUD;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Gadgets;
using VikingEngine.ToGG.Commander.Players;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.HeroQuest.QueAction;
using Microsoft.Xna.Framework.Input;
using VikingEngine.ToGG.HeroQuest.Players.Phase;
using VikingEngine.ToGG.Data;
//using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.HeroQuest.Data;
using VikingEngine.ToGG.ToggEngine.BattleEngine;
using System.Resources;

namespace VikingEngine.ToGG.HeroQuest.Players
{
    class LocalPlayer : AbsHQPlayer, ToggEngine.Move.IMoveUpdateListerner
    {
        Engine.PlayerData localPData;
        public PlayerHUD hud;
        List<ToggEngine.Display3D.AbsUnitMessage> waitingMessages = new List<ToggEngine.Display3D.AbsUnitMessage>();
        public PopupDialogue popupDialogue = null;
        public bool readyToEndTurn_Request;
        public int startTurnTime;
        static readonly Color DisabledMoveModelGray = ColorExt.GrayScale(0.4f);
        //UnitDragNDrop unitDragNDrop;
        ToggEngine.Move.MoveUpdate moveUpdate;
        public Gadgets.QuickItemDragNDrop itemDrag = null;

        AbsUnit prevUnit = null;

        ToggEngine.Display2D.MoveLengthToolTip moveToolTip = null;
        Display.StaminaUsageTooltip staminaMoveToolTip = null;
        
        public StrategyCardsHud strategyCardsHud = null;
        PlayerVisualData prevVisualState = new PlayerVisualData();
        //public Data.AbsSurgeOption surgeConvertion;
        public AbsSurgeOption surgeOption = null;

        PlayerDisplay DefaultDisplay;
        PlayerDisplay prevDisplay;
        bool newPlayerState = false;

        public MapSquareAvailableType availableMapSquare;
        MapSquareAvailableType prevAvailableMapSquare = MapSquareAvailableType.Enabled;

        List<AbsPlayerPhase> phaseStack = new List<AbsPlayerPhase>(4);
        public HeroInstanceColl heroInstances = new HeroInstanceColl();

        public LocalPlayer(Engine.AbsPlayerData pData)
            : base(pData)
        {            
            localPData = (Engine.PlayerData)pData;
            localPData.view.Viewport = Draw.defaultViewport;
            localPData.view.safeScreenArea = Engine.Screen.SafeArea;

            relationVisuals.faceRight = true;
            mapControls = new MapControls(this);

            //unitDragNDrop = new UnitDragNDrop(this);
            moveUpdate = new ToggEngine.Move.MoveUpdate(this, this);
            DefaultDisplay = new PlayerDisplay();
            DefaultDisplay.attackTerrain = EnableType.Hidden;
        }
        
        override public void update()
        {
            PlayerDisplay display = DefaultDisplay;
            
            availableMapSquare = MapSquareAvailableType.None;

            if (Input.Keyboard.KeyDownEvent(Keys.LeftAlt))
            {

            }

            if (toggRef.menu.Open)
            {
                updateMenu(ref display);
            }
            else if (popupDialogue != null)
            {
                setCurrentPlayerState(ref display, PlayerState.Other);
                if (popupDialogue.update())
                {
                    popupDialogue.DeleteMe();
                    popupDialogue = null;
                }
            }
            else
            {
                if (HasPhaseStack)
                {
                    updatePhase(ref display);
                }
                //else if (RespawnState != null)
                //{
                //    updateRespawn(ref display);
                //}
                else if (attackDisplay != null)
                {
                    updateAttack(ref display);
                }
                else if (que.Update() || otherPlayerLockQue.Update())
                {
                    lib.DoNothing();
                }
                else if (itemDrag != null)
                {
                    updateItemDrag(ref display);
                }
                else if (strategyCardsHud != null)
                {
                    updateStrategySelect(ref display);
                }
                else if (readyToEndTurn_Confirmed)
                {
                    hud.update(ref display, this, true);
                    mapUpdate(ref display, false);
                    updateBoardRoam(false);
                }
                else
                {
                    display.quickBelt = true;
                    hud.update(ref display, this, true);
                    
                    if (!display.mouseOverHud)
                    {
                        if (movingUnit != null)
                        {
                            mapUpdate(ref display, true);
                            setCurrentPlayerState(ref display, PlayerState.MoveUnit);
                            updateMoveUnit(ref display);
                        }
                        else if (itemLootStack.Count > 0)
                        {
                            new Phase.LootDialogue(this, itemLootStack);
                        }
                        else
                        {
                            viewMessages();
                            mapUpdate(ref display, false);
                            setCurrentPlayerState(ref display, PlayerState.BoardRoam);
                            updateBoardRoam(true);
                        }
                    }
                }
            }
            
            updatePlayerDisplay(display);
            hud.infoUpdate(this);
        }

        void viewMessages()
        {
            if (waitingMessages.Count > 0)
            {
                foreach (var m in waitingMessages)
                {
                    m.start();
                }

                waitingMessages.Clear();
            }
        }

        public void stackPhase(AbsPlayerPhase phase)
        {            
            phaseStack.Add(phase);
        }

        bool HasPhaseStack => phaseStack.Count > 0;

        public AbsPlayerPhase currentPhase;

        public void updatePhase(ref PlayerDisplay display)
        {
            currentPhase = phaseStack[phaseStack.Count - 1];

            if (currentPhase.state == ToggEngine.QueAction.QueState.QuedUp)
            {
                currentPhase.onBegin();
                currentPhase.state = ToggEngine.QueAction.QueState.Started;
            }

            currentPhase.update(ref display);

            if (currentPhase.state == ToggEngine.QueAction.QueState.Completed)
            {
                currentPhase.DeleteMe();
                phaseStack.Remove(currentPhase);
                currentPhase = null;

                beginBoardRoam();
            }
        }

        public bool InPhase(Players.Phase.PhaseType phaseType)
        {
            return currentPhase != null && currentPhase.Type == phaseType;
        }

        public bool EndPhase(Players.Phase.PhaseType phaseType)
        {
            if (InPhase(phaseType))
            {
                currentPhase.end();
                return true;
            }
            return false;
        }

        public void setCurrentPlayerState(ref PlayerDisplay display, PlayerState newstate)
        {
            display.playerState = newstate;
            newPlayerState = display.playerState != prevDisplay.playerState;
            visualState.state = newstate;
        }

        public void toggleReadyEndTurn()
        {
            lib.Invert(ref readyToEndTurn_Request);

            if (readyToEndTurn_Request)
            {
                mapControls.removeAvailableTiles();
            }
            else
            {
                beginBoardRoam();
            }
        }

        //void updateRespawn(ref PlayerDisplay display)
        //{
        //    mapUpdate(ref display, false);

        //    bool isTarget = HeroUnit.heroCanRestartHere(mapControls.selectionIntV2, false) &&
        //        RespawnState.available.Contains(mapControls.selectionIntV2);
        //    availableMapSquare = isTarget ? MapSquareAvailableType.Enabled : MapSquareAvailableType.None;

        //    if (mapControls.isOnNewTile)
        //    {
        //        if (isTarget)
        //        {
        //            new RespawnTooltip(mapControls);
        //        }
        //        else
        //        {
        //            mapControls.removeToolTip();
        //        }
        //    }

        //    if (isTarget && toggRef.inputmap.click.DownEvent)
        //    {
        //        RespawnState.respawnHere(mapControls.selectionIntV2);
        //        RespawnState = null;
        //        regularTurnStartSetup(); 
        //    }
        //}
                
        void updatePlayerDisplay(PlayerDisplay display)
        {
            if (!display.hudUpdate)
            {
                hud.viewGamePlayHud(display, this);
            }
            mapControls.setSelectionVisible(!display.mouseOverHud && display.viewSelection);

            if (!visualState.Equals(prevVisualState))
            {
                prevVisualState = visualState;
                if (nameDisplay != null)
                {
                    nameDisplay.refreshVisualState(visualState);
                }
            }

            if (heroInstances.HasSelection)
            {
                if (!InPhase(PhaseType.Backpack))
                {
                    Backpack().equipment.quickbelt.setVisible(display.quickBelt);
                }

                hud.backpackButton.Visible = display.quickBelt ||
                    InPhase(PhaseType.Backpack);
            }

            hud.extendedTooltip.update(display.playerState);

            if (strategyCardsHud != null)
            {
                strategyCardsHud.setVisible(display.phasesAndStrategy);
            }

            prevDisplay = display;

            if (availableMapSquare != prevAvailableMapSquare)
            {
                prevAvailableMapSquare = availableMapSquare;
                mapControls.setAvailable(availableMapSquare);
            }

            if (display.mouseOverHud)
            {
                hqRef.playerHud.removeInfoCardDisplay();
            }
        }        

        void updateItemDrag(ref PlayerDisplay display)
        {
            setCurrentPlayerState(ref display, PlayerState.Other);

            display.quickBelt = true;
            Backpack().equipment.quickbelt.update(this, ref display);

            mapUpdate(ref display, false);
            
            if (itemDrag.update(this))
            {
                itemDrag = null;
                beginBoardRoam();
            }
            else
            {
                availableMapSquare = itemDrag.availableTile ? MapSquareAvailableType.Enabled : MapSquareAvailableType.Disabled;
            }
        }

        void updateStrategySelect(ref PlayerDisplay display)
        {
            setCurrentPlayerState(ref display, PlayerState.SelectStrategy); 
            display.mouseOverHud |= strategyCardsHud.update();
            display.viewSelection = true;
            display.quickBelt = true;
            display.phasesAndStrategy = true;          

            if (strategyCardsHud != null)
            {
                var card = strategyCardsHud.HoverCard();
                if (card == null)
                {
                    visualState.strategy = HeroStrategyType.NONE;
                    visualState.strategyAvailable = true;
                }
                else
                {
                    visualState.strategy = (HeroStrategyType)card.data.Id;
                    visualState.strategyAvailable = card.Enabled;
                }
            }
            mapUpdate(ref display, false);
            updateBoardRoam(false);

            availableMapSquare = MapSquareAvailableType.None;

            hud.update(ref display, this, false);

            if (!display.mouseOverHud &&
                toggRef.inputmap.click.DownEvent &&
                strategyCardsHud != null)
            {
                strategyCardsHud.createAlertFlash();
            }
        }

        public override void onAttack()
        {
            base.onAttack();
            onStrategyAction();
        }

        public void onUnitSkillUse()
        {
            onStrategyAction();
            //beginBoardRoam();
        }

        public void onStrategyAction()
        {
            hud.refreshTodo(this);
            heroInstances.sel.lockedInStrategySelection = true;
            hud.refreshPhaseHud(this, true);
        }

        public void onBackPackAccess(bool equipment)
        {
            if (!heroInstances.sel.lockedInStrategySelection)
            {
                onStrategyAction();
            }

            if (equipment)
            {
                Backpack().NetShare();
            }
        }

        public void returnToStartegySelect()
        {
            if (!heroInstances.sel.lockedInStrategySelection)
            {
                Hero.availableStrategies.active.payForStrategy(false, HeroUnit);
                openStrategyHud();
            }
        }
        
        public override void add(AbsItem item, bool tryEquip, bool viewMessage)
        {
            if (viewMessage)
            {
                waitingMessages.Add(
                    new ToggEngine.Display3D.UnitMessageItem(HeroUnit, item, false));
            }
            //base.add(item);
            Backpack().add(item, tryEquip);
        }

        List<AbsItem> itemLootStack = new List<AbsItem>(8);
        public override void lootItem(AbsItem item)
        {
            if (item.Equip.Contains(SlotType.AnyWearingEquipment))
            {
                itemLootStack.Add(item);
            }
            else
            {
                add(item, true, true);
            }
        }

        public void recieveItemFromPlayer(AbsItem item)
        {
            lootItem(item);
        }

        public HeroStrategyType HoverStrategy()
        {
            if (strategyCardsHud != null)
            {
                var card = strategyCardsHud.HoverCard();
                if (card == null)
                {
                    return HeroStrategyType.NONE;
                }
                return (HeroStrategyType)card.data.Id;
            }

            return HeroStrategyType.NONE;
        }

        void onSelectStrategy(int type)
        {
            heroInstances.sel.lockedInStrategySelection = false;
            hud.prevPhaseButton.Visible = true;

            Hero.availableStrategies.selectId(type);
            visualState.strategy = Hero.availableStrategies.active.Type;

            strategyCardsHud.DeleteMe();
            strategyCardsHud = null;

            Hero.availableStrategies.active.ApplyToHero(HeroUnit, true);
            Hero.availableStrategies.active.payForStrategy(true, HeroUnit);
            
            hud.refreshTodo(this);

            beginBoardRoam();

            hqRef.events.SendToAll(ToGG.Data.EventType.StrategySelected, this);
        }
        
        public void UpdateSpectating(IntVector2 pos)
        {
            mapControls.zoomInput();
            //mapControls.updateSpectateCamera(pos);
            toggRef.cam.spectate(pos);
        }

        public void cameraOnHero()
        {
            if (HeroUnit != null)
            {
                toggRef.cam.spectate(HeroUnit.squarePos);
            }
            else
            {
                toggRef.cam.spectate(heroInstances.First.heroUnit.squarePos);
            }
        }

        void updateAttack(ref PlayerDisplay display)
        {
            setCurrentPlayerState(ref display, PlayerState.Attack);
            toggRef.cam.spectate(VectorExt.AddY(attackDisplay.attackRoll.defenders.Selected().unit.squarePos, +1));

            if (attackDisplay.buttons != null)
            {
                attackDisplay.options.update();
                attackDisplay.attackRoll.idleUpdate();

                if (attackDisplay.buttons.rollButton.update())
                {
                    //onStrategyAction();
                    attackDisplay.beginAttack();
                    return;
                }

                if (attackDisplay.buttons.rollButton.mouseOver != attackDisplay.buttons.rollButton.prevMouseOver)
                {
                    attackDisplay.attackRoll.attackDice.dice.onRollHover(attackDisplay.buttons.rollButton.mouseOver);
                    foreach (var m in attackDisplay.attackRoll.defenders.list)
                    {
                        m.rollDisplay.dice.onRollHover(attackDisplay.buttons.rollButton.mouseOver);
                    }
                }

                if (attackDisplay.buttons.cancelButton.update())
                {
                    removeAttackDisplay();
                    return;
                }
            }
            else
            {
                if (attackDisplay.updateAttack())
                {
                    removeAttackDisplay();
                }
            }
        }

        protected override void removeAttackDisplay()
        {
            base.removeAttackDisplay();

            hud.removeInfoCardDisplay();

            beginBoardRoam();
        }

        public void mapUpdate(ref PlayerDisplay display, bool movingUnit)
        {
            prevUnit = mapControls.SelectedUnit;
            if (Input.Keyboard.Ctrl)
            {
                lib.DoNothing();
            }
            if (Input.Keyboard.Ctrl && Input.Keyboard.Alt && 
                Input.Mouse.ButtonDownEvent(MouseButton.Left))
            {
                SquareDebugOptions();
            }
            mapControls.updateMapMovement(true, movingUnit);
            toggRef.hud.tileInfoUpdate(mapControls);

            display.viewSelection = mapControls.selectedTile != null && 
                mapControls.selectedTile.Square.ModelType != ToggEngine.Map.SquareModelType.Wall;
        }

        

        public void onSurgeOption(AbsSurgeOption option)
        {
            this.surgeOption = option;
        }

        //public override void OnMapPan(Vector2 screenPan)
        //{
        //    base.OnMapPan(screenPan);
            
        //}

        public override void onNewTile()
        {
            base.onNewTile();
            hud.extendedTooltip.onNewTile();
        }

        public override void onNewUnit(AbsUnit unit)
        {
            base.onNewUnit(unit);

            var hero = unit.hq().data.hero;
            if (hero != null)
            {
                heroInstances.Add(new HeroInstance(unit.hq(), this), false);
            }
        }

        //void tileInfoUpdate()
        //{
        //    if (mapControls.isOnNewTile)
        //    {
        //        ToggEngine.Display2D.UnitCardDisplay.CreateCardDisplay(mapControls.selectionIntV2, hud);                
        //    }
        //}

        public void NetUpdate()
        {
            var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqPlayerStatus, Network.PacketReliability.Unrelyable);
            SaveLib.WriteVector(w, mapControls.selectionV2);

            bool moveArrows = movingUnit != null && movingUnit.movelines != null;
            bool bAttackArrow = attackerArrow != null;
            bool bItemDrag = itemDrag != null && itemDrag.item != null;

            EightBit states = new EightBit(moveArrows, bAttackArrow, bItemDrag);
            states.write(w);

            if (moveArrows)
            {
                ((Unit)movingUnit).netWriteUnitId(w);
                movingUnit.movelines.Write(w);
            }
            
            if (bAttackArrow)
            {
                toggRef.board.WritePosition(w, attackerArrow.storedTarget);//SaveLib.WriteVector(w, attackerArrow.storedTarget);
            }

            if (bItemDrag)
            {
                itemDrag.item.writeItem(w, null);
            }
            
            visualState.write(this, w);

            HeroUnit.netWriteStatus(w);
        }

        public override void startGame()
        {
            base.startGame();
            heroInstances.First.backpack.NetShare();
        }

        override public void onTurnStart()
        {
            turnStartGenericSetup();
            startTurnTime = Ref.gamestate.UpdateCount;

            base.onTurnStart();

            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                unitsColl.unitsCounter.sel.OnTurnStart();
            }

            if (heroInstances.Count == 1)
            {
                heroInstances.selectFirst();
            }
            else
            {
                heroInstances.Unselect();
            }

            foreach (var hero in heroInstances)
            {
                hero.onTurnStart();
            }

            regularTurnStartSetup();
        }

        public void regularTurnStartSetup()
        {
            if (heroInstances.HasSelection)
            {
                if (HeroUnit.Dead)
                {
                    new RespawnState(this);
                }
                else
                {
                    hud.refreshEndTurnButton(this, true);
                    openStrategyHud();
                }
            }
            else
            {
                new Phase.PickHero(this);
            }
        }

        void openStrategyHud()
        {
            hud.removeToDo();
            strategyCardsHud = new ToggEngine.Display2D.StrategyCardsHud(Hero.availableStrategies, 
                onSelectStrategy, HeroUnit);

            List<IntVector2> availableUnits = new List<IntVector2>(hqUnits.units.Count);
            hqUnits.unitsCounter.Reset();
            while (hqUnits.unitsCounter.Next())
            {
                availableUnits.Add(hqUnits.unitsCounter.sel.squarePos);
            }
            mapControls.SetAvailableTiles(availableUnits);
        }

        public override void onTurnEnd()
        {
            if (hasStartedTurn)
            {
                base.onTurnEnd();
                Hero.availableStrategies.active?.onTurnEnd(HeroUnit);
                hud.removeToDo();
                mapControls.removeToolTip();
                removeAttackerArrow();

                mapControls.removeAvailableTiles();
                PlayerDisplay endDisplay = new PlayerDisplay();
                updatePlayerDisplay(endDisplay);
            }
        }

        public override void OnEvent(EventType eventType, object tag)
        {
            switch (eventType)
            {
                case EventType.GameStart:
                    hud = new PlayerHUD();
                    
                    break;
                case EventType.TurnStart:
                    lib.DoNothing();
                    break;
            }

            base.OnEvent(eventType, tag);
        }

        public override void onMovedUnit(AbsUnit unit)
        {
            base.onMovedUnit(unit);

            if (unit.gotStartPosition &&
                unit.hq().condition.Get(Data.Condition.ConditionType.Hidden) == null)
            {
                new Timer.Asynch1ArgTrigger<AbsUnit>(
                    hqRef.players.dungeonMaster.checkAlertFromMovingOpponent_Asynch, unit);
            }
        }

        List<IntVector2> boardUiTargets = new List<IntVector2>(16);
        List<IntVector2> boardUiMovable = new List<IntVector2>(4);

        void beginBoardRoam()
        {
            movingUnit = null;

            boardUiMovable.Clear();

            unitsColl.unitsCounter.Reset();
            while (unitsColl.unitsCounter.Next())
            {
                if (unitsColl.unitsCounter.sel.Alive &&
                    canBeMoved(unitsColl.unitsCounter.sel, out _))
                {
                    boardUiMovable.Add(unitsColl.unitsCounter.sel.squarePos);
                }
            }

            boardUiTargets.Clear();
            if (canAttack())
            {
                Hero.availableStrategies.active.collectBoardUiTargets(
                    HeroUnit, boardUiTargets);

                if (boardUiTargets.Count == 0)
                {
                    HeroUnit.attackTargets.refresh(HeroUnit.squarePos);
                    HeroUnit.attackTargets.targetPositions(boardUiTargets);
                }
            
             }
            mapControls.SetAvailableTiles(boardUiMovable, boardUiTargets);
            hud.refreshPhaseHud(this, true);
                        
            mapControls.removeToolTip();

            refreshActionButtons();
        }

        public override void onActionComplete(ToggEngine.QueAction.AbsQueAction action, bool emptyQue)
        {
            base.onActionComplete(action, emptyQue);

            if (emptyQue)
            {
                beginBoardRoam();
            }
        }

        public bool RefreshUi => mapControls.isOnNewTile || newPlayerState;

        public void updateBoardRoam(bool allowInput)
        {
            if (RefreshUi)
            {
                if (Input.Keyboard.IsKeyDown(Keys.LeftControl))
                {
                    lib.DoNothing();
                }

                mapControls.removeToolTip();
            }

            if (allowInput  && heroInstances.HasSelection)
            {
                bool movableFriendlyUnit = updateBoardRoam_Moveable();

                if (!movableFriendlyUnit)
                {
                    if (updateBoardRoam_Attacktargets())
                    {
                        return;
                    }

                    updateBoardRoam_Interactable();
                }
            }

            if (RefreshUi)
            {
                if (prevUnit != null && prevUnit.movelines != null)
                {
                    prevUnit.movelines.setFocus(2);
                }

                if (mapControls.SelectedUnit != null && mapControls.SelectedUnit.movelines != null)
                {
                    mapControls.SelectedUnit.movelines.setFocus(1);
                }

                if (mapControls.SelectedUnit != null)
                {
                    new UnitMoveAndAttackGUI(mapControls.SelectedUnit, mapControls.selectionIntV2, false, false);
                }
                else
                {
                    UnitMoveAndAttackGUI.Clear();
                }
                               
                hud.unitsGui.refresh(mapControls.selectionIntV2);
            }
        }

        bool updateBoardRoam_Moveable()
        {
            bool movableFriendlyUnit = false;
            string cantMoveReason = null;

            if (mapControls.SelectedUnit != null)
            {
                if (mapControls.SelectedUnit.globalPlayerIndex == this.pData.globalPlayerIndex)
                {
                    if (canBeMoved(mapControls.SelectedUnit, out cantMoveReason))
                    {
                        movableFriendlyUnit = true;
                    }
                }
            }

            if (movableFriendlyUnit)
            {
                availableMapSquare = MapSquareAvailableType.Enabled;
                updateMoveToolTip();

                moveUpdate.updateHover();
                //if (moveUpdate.unitDragNDrop.updateUnitSelection())
                //{
                //    movingUnit = mapControls.SelectedUnit;
                //    new AvailableMovement(this, movingUnit, false, false);
                //    if (movingUnit.movelines == null)
                //    {
                //        new MoveLinesGroup(movingUnit);
                //    }
                //    else
                //    {
                //        movingUnit.movelines.setFocus(0);
                //    }
                //}
            }
            else
            {
                if (RefreshUi && cantMoveReason != null)
                {
                    new CantMoveToolTip(mapControls, cantMoveReason);
                }
            }

            return movableFriendlyUnit;
        }

        bool updateBoardRoam_Attacktargets()
        {
            AttackTargetGroup attackSelection = null;

            if (canAttack())
            {
                AttackTargetGroup grouptargets;

                if (HeroUnit.isGroupTarget(mapControls.selectionIntV2, out grouptargets))
                {
                    attackSelection = grouptargets;
                }
                else if (mapControls.SelectedUnit != null)
                {
                    var attacktype = HeroUnit.IsAvailableTarget(HeroUnit.squarePos, mapControls.SelectedUnit);
                    if (attacktype != AttackMainType.NONE)
                    {
                        attackSelection = new AttackTargetGroup(new AttackTarget(mapControls.SelectedUnit,
                            new AttackType(attacktype, AttackUnderType.None)));
                    }
                }

                if (arraylib.HasMembers(attackSelection))
                {
                    availableMapSquare = MapSquareAvailableType.Enabled;

                    if (toggRef.inputmap.click.DownEvent)
                    {
                        openAttackDisplay(attackSelection);
                        return true;
                    }
                }
            }

            if (RefreshUi)
            {
                if (attackSelection != null)
                {
                    getAttackerArrow().updateAttackerArrow(HeroUnit, mapControls.selectionIntV2);

                    new AttackTargetToolTip(HeroUnit, attackSelection.sel, mapControls);

                    if (attackSelection.Count > 1)
                    {
                        mapControls.GroupFrame(attackSelection);
                    }
                }
                else
                {
                    removeAttackerArrow();
                }

            }

            return false;
        }

        void updateBoardRoam_Interactable()
        {
            if (HeroUnit.squarePos.SideLength(mapControls.selectionIntV2) <= 1)
            {
                AbsTileObject interactObj = mapControls.selectedTile.tileObjects.GetInteractObject(HeroUnit);
                if (interactObj != null)
                {
                    if (RefreshUi)
                    {
                        new Display.InteractTooltip(interactObj, mapControls);
                    }
                    if (toggRef.inputmap.click.DownEvent)
                    {
                        new QueAction.Interact(this, HeroUnit, interactObj);
                    }
                }
            }
        }

        public void openAttackDisplay(AttackTargetGroup targets)
        {
            mapControls.removeToolTip();

            targets.clearOutInvalid();
            visualState.meleeAttack = targets.AttackType.IsMelee;

            targets.attackSettings = HeroUnit.data.attackSettings(targets.AttackType.IsMelee);
            attackDisplay = new AttackDisplay(HeroUnit, targets, this);

            hqRef.events.SendToAll(ToGG.Data.EventType.OpenAttackDisplay, null);
        }
        
        void updateMenu(ref PlayerDisplay display)
        {
            setCurrentPlayerState(ref display, PlayerState.Menu);

            if (toggRef.menu.Update() ||
                    toggRef.inputmap.menuInput.openCloseInputEvent() ||
                    toggRef.inputmap.moreInfo.DownEvent)
            {
                toggRef.menu.CloseMenu();
            }

            hud.menuUpdate();
        }

        void updateMoveUnit(ref PlayerDisplay display)
        {
            setCurrentPlayerState(ref display, PlayerState.MoveUnit);
            moveUpdate.updateMove(ref availableMapSquare);
        }

            //    bool dropInput, undoMove, availableSquare;
            //    unitDragNDrop.updateUnitMove(out dropInput, out undoMove, out availableSquare);

            //    if (availableSquare)
            //    {
            //        availableMapSquare = MapSquareAvailableType.Enabled;
            //        movingUnit.soldierModel.model.Color = Color.White;
            //    }
            //    else
            //    {
            //        movingUnit.soldierModel.model.Color = DisabledMoveModelGray;
            //    }

            //    mapControls.viewErrorCross(unitDragNDrop.errorMoveWarning);

            //    if (dropInput)
            //    {//complete movement
            //        moveUnit();
            //    }
            //    if (undoMove)
            //    {
            //        movingUnit.undoMovement();
            //        if (movingUnit.movelines != null)
            //        {
            //            movingUnit.movelines.setFocus(2);
            //        }
            //    }

            //    if (dropInput || undoMove)
            //    {
            //        movingUnit.soldierModel.model.Color = Color.White;
            //        movingUnit = null;
            //        beginBoardRoam();
            //    }

            //    if (movingUnit != null)
            //    {
            //        if (mapControls.isOnNewTile || newPlayerState)
            //        {
            //            updateMoveToolTip();
            //            toggRef.board.soundFromSquare(mapControls.selectionIntV2);

            //            hud.unitsGui.refresh(mapControls.selectionIntV2, movingUnit);

            //            var actions = new QueAction.SquareActionList(movingUnit.movelines,
            //                (Unit)movingUnit, true);
            //            actions.createActionIcons(movingUnit.movelines);
            //        }
            //    }
            //}

            public bool canAttack()
        {
            return heroInstances.HasSelection && Hero.availableStrategies.active != null && Hero.availableStrategies.active.attacks.HasValue;
        }

        override public Unit HeroUnit
        {
            get
            {
                if (heroInstances.HasSelection)
                {
                    return heroInstances.sel.heroUnit;
                }

                return null;
            }
        }

        public HeroData Hero
        {
            get { return HeroUnit.data.hero; }
        }

        public IntVector2 ActiveUnitPos
        {
            get { return HeroUnit.squarePos; }
        }

        bool canBeMoved(AbsUnit u, out string cantMoveReason)
        {
            var status = u.hq().condition.GetBase(Data.Condition.BaseCondition.Immobile);
            if (status != null)
            {
                cantMoveReason = status.Name;
                return false;
            }

            if (u.hasEndedMovement)
            {
                cantMoveReason = "Has ended movement";
                return false;
            }

            if (isInactiveHero(u))
            {
                cantMoveReason = "Not active hero";
                return false;
            }

            int hasMoved, movementLeft, max, staminaMoves, backStabs;
            u.moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);
            if (movementLeft > 0 || staminaMoves > 0)
            {
                cantMoveReason = null;
                return true;
            }
            else
            {
                if (u.Data.move > 0)
                {
                    cantMoveReason = "Out of moves";
                }
                else
                {
                    cantMoveReason = null;
                }
                return false;
            }
        }

        public bool isInactiveHero(AbsUnit u)
        {
            return u.hq().IsHero && u != HeroUnit;
        }

        public override AbsHeroInstance HeroInstance => heroInstances.sel;

        public void onMoveUpdateEnd(AbsUnit unit, bool canceled)
        {
            beginBoardRoam();
        }

        public void onMoveUpdateNewSquare(AbsUnit unit)
        {
            updateMoveToolTip();
            hud.unitsGui.refresh(mapControls.selectionIntV2, movingUnit);

            var actions = new ToggEngine.QueAction.SquareActionList(movingUnit.movelines,
                movingUnit.hq(), true);
            actions.createActionIcons(movingUnit.movelines);
        }

        //void moveUnit()
        //{
        //    if (movingUnit.movelines.HasNewMoves())
        //    {
        //        onStrategyAction();
        //        movingUnit.movelines.moveActionIcons.DeleteAll();
        //        movingUnit.movelines.checkOnMoveCompleted();

        //        toggRef.board.onUnitMovement(movingUnit, movingUnit.movelines);

        //        int staminaUse = movingUnit.movelines.staminaCost();
                
        //        HeroData hero = ((Unit)movingUnit).data.hero;
        //        if (hero != null)
        //        {
        //            hero.stamina.spend(staminaUse);
        //        }

        //        //request om att flytta här
        //        new QueAction.MoveAction(this, movingUnit.movelines, (Unit)movingUnit);
        //    }
        //    else
        //    {
        //        movingUnit.SetVisualPosition(movingUnit.squarePos);
        //    }
        //}

        void updateMoveToolTip()
        {
            bool refresh = mapControls.isOnNewTile;
            
            Unit u;
            if (movingUnit == null)
            {
                u = mapControls.SelectedUnit as Unit;
            }
            else
            {
                u = movingUnit as Unit;
            }

            if (u != null)
            {
                int hasMoved, movementLeft, max, staminaMoves, backStabs;
                u.moveInfo(out hasMoved, out movementLeft, out staminaMoves, out max, out backStabs);

                if (hasMoved <= max)
                {
                    if (moveToolTip == null || moveToolTip.IsDeleted)
                    {
                        mapControls.removeToolTip();
                        moveToolTip = new ToggEngine.Display2D.MoveLengthToolTip(mapControls);
                        refresh = true;
                    }

                    if (refresh)
                    {
                        moveToolTip.refresh(u);
                    }
                }
                else
                {
                    if (staminaMoveToolTip == null || staminaMoveToolTip.IsDeleted)
                    {
                        mapControls.removeToolTip();
                        staminaMoveToolTip = new Display.StaminaUsageTooltip(mapControls, u);
                        refresh = true;
                    }

                    if (refresh)
                    {
                        int staminaUse = u.movelines.staminaCost();
                        staminaMoveToolTip.refresh(u, staminaUse);
                    }
                }
            }
        }
        
        public void SquareDebugOptions()
        {
            toggRef.menu.OpenMenu(true);

            GuiLayout layout = new GuiLayout("Square Debug Options", toggRef.menu.menu);
            {
                if (mapControls.selectedTile.unit == null)
                {
                    new GuiTextButton("Place hero", null,
                        new GuiAction1Arg<Unit>(debugSquarePlaceUnit, HeroUnit), false, layout);

                    var pet = hqUnits.petUnit();
                    if (pet != null)
                    {
                        new GuiTextButton("Place pet", null,
                            new GuiAction1Arg<Unit>(debugSquarePlaceUnit, pet), false, layout);
                    }
                }
                else
                {
                    new GuiTextButton("Bleed condition", null,
                        new GuiAction1Arg<Data.Condition.ConditionType>(
                            debugSquareAddCondition, Data.Condition.ConditionType.Bleed), false, layout);
                    new GuiTextButton("Poision condition", null,
                        new GuiAction1Arg<Data.Condition.ConditionType>(
                            debugSquareAddCondition, Data.Condition.ConditionType.Poision), false, layout);
                }

                if (mapControls.selectedTile.hidden)
                {
                    new GuiTextButton("Remove fog", null, debugSquareRemoveFog, false, layout);
                }

                new GuiTextButton("Cancel", null, toggRef.menu.CloseMenu, false, layout);
            }
            layout.End();
        }

        public void refreshActionButtons()
        {
            arraylib.DeleteAndClearArray(hud.actionButtons);
            hud.actionButtonsChanged = true;

            if (Hero.availableStrategies.active != null)
            {
                List<Data.UnitAction.AbsUnitAction> unitActions = new List<Data.UnitAction.AbsUnitAction>(4);

                hqUnits.unitsCounter.Reset();
                while (hqUnits.unitsCounter.Next())
                {
                    hqUnits.unitsCounter.sel.hq().collectActions(unitActions);

                    foreach (var action in unitActions)
                    {
                        hud.actionButtons.Add(new Display.ActionButton(nextArea(), this, 
                            hqUnits.unitsCounter.sel.hq(), action));
                    }

                    unitActions.Clear();
                }
            }

            VectorRect nextArea()
            {
                VectorRect result = hud.actionButtonsAreaStart;
                result.nextAreaX(hud.actionButtons.Count, Engine.Screen.BorderWidth);
                return result;
            }
        }

        void debugSquarePlaceUnit(Unit unit)
        {
            unit.SetPosition(mapControls.selectionIntV2);
            toggRef.menu.CloseMenu();
        }

        void debugSquareAddCondition(Data.Condition.ConditionType condition)
        {
            mapControls.selectedTile.unit.hq().condition.Set(condition, 1, true, true, true);
            toggRef.menu.CloseMenu();
        }

        void debugSquareRemoveFog()
        {
            new ToggEngine.Map.RevealMap(mapControls.selectionIntV2, false);
            toggRef.menu.CloseMenu();
        }

        
        public override void onDestroyedUnit(AbsUnit attacker, AbsUnit destroyedUnit)
        {
            //TODO quick wep

            Unit att = (Unit)attacker;
            Unit def = (Unit)destroyedUnit;

            attacker.hq().addBloodrage(hqLib.BloodRageForKill);
            new Killmark(Hero.killmark, def.squarePos);
        }

        public override void onDamagedUnit(AbsUnit attacker, AbsUnit defender)
        {
            attacker.hq().addBloodrage(hqLib.BloodRageForDamage);
        }

        public override void nextPhaseButtonAction(bool forwardToNext)
        {
            hud.nextPhaseClick(this);
        }

        public void onPickHero()
        {
            heroInstances.sel.activated = true;
            regularTurnStartSetup();
        }

        public bool nextPhaseIsEndTurn()
        {
            return heroInstances.AllAreActivated();
        }

        public void endTurnInput()
        {
            hqRef.gamestate.endTurn();
        }
        
        public EnableType canExitPhase(out string warningText)
        {
            if (HeroUnit.movelines == null)
            {
                warningText = "Hero hasn't moved";
                return EnableType.Able_NotRecommended;
            }
            else if (canAttack() && HeroUnit.attackTargets.targets.Count > 0)
            {
                warningText = "Hero can attack";
                return EnableType.Able_NotRecommended;
            }

            warningText = null;
            return EnableType.Enabled;
        }

        public void spawnHero()
        {
            int heroIx = this.heroIndex;
            HqUnitType petType = HqUnitType.Num_None;

            foreach (var heroSetup in hqRef.localPlayers.setups.First.visualSetups)
            {
                IntVector2 heroPos = toggRef.board.metaData.getEntrance(heroIx);
                HqUnitType heroType = heroSetup.unit;
                if (heroType == HqUnitType.Num_None)
                {
                    if (StartUpSett.QuickRunInSinglePlayer)
                    {
                        heroType = StartUpSett.QuickRunHeroClass;
                    }
                    else
                    {
                        heroType = HqUnitType.RecruitHeroSword;
                    }
                }
                HqUnitData hero = hqRef.unitsdata.Get(heroType);
                petType = hero.defaultPet();

                new Unit(heroPos, hero, this);

                heroIx++;
            }

            if (hqRef.players.PetsFillout &&
                hqRef.setup.quest != QuestName.TutorialPractice)
            {
                HqUnitData pet = hqRef.unitsdata.Get(petType);

                var petPos = toggRef.board.metaData.getEntrance(hqRef.players.localHost.heroIndex + hqLib.AddPetsToPlayerCount);
                new Unit(petPos, pet, hqRef.players.localHost);
            }

            new ToggEngine.Map.RevealMap(unitsColl.units.First().squarePos, false);
        }

        public override ItemCollection specialArrows()
        {
            ItemCollection arrows = Backpack().listItems(new ItemFilter(ItemMainType.SpecialArrow), true);

            return arrows;
        }

        override public Gadgets.Backpack Backpack()
        {
            if (heroInstances.HasSelection)
            {
                return heroInstances.sel.backpack;
            }
            return null;
        }

        public override bool IsScenarioOpponent
        {
            get
            {
                return false;
            }
        }

        override public bool IsHero { get { return true; } }

        public override bool LocalHumanPlayer => true;
        public override bool IsDungeonMaster => false;

        public override bool IsLocal => true;

    }

    struct PlayerDisplay
    {
        public PlayerState playerState;

        public bool mouseOverHud;
        public bool hudUpdate;

        public bool viewSelection;
        public bool quickBelt;
        public bool phasesAndStrategy;

        public EnableType attackTerrain;

    }

    enum PlayerState
    {
        NONE,
        BoardRoam,
        Menu,
        MoveUnit,
        Attack,
        Backpack,
        SelectStrategy,
        PickHero,
        Other,
    }

    
}
