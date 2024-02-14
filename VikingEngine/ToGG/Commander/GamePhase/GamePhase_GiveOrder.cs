using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.Display2D;
using VikingEngine.ToGG.Commander.CommandCard;
using VikingEngine.ToGG.ToggEngine.GO;
using VikingEngine.ToGG.Commander;

namespace VikingEngine.ToGG
{
    class GamePhase_GiveOrder: AbsGamePhase
    {
        
        AbsUnit[] orderValueSortedUnits;
        bool aiOrderValuationComplete = false;
        ListWithSelection<AbsUnit> canBeOrdered = new ListWithSelection<AbsUnit>();
        //StrategyCardsHud strategyCards = null;
        MapSquareAvailableType availableMapSquare = MapSquareAvailableType.None;
        MapSquareAvailableType prevAvailableMapSquare = MapSquareAvailableType.Enabled;

        public GamePhase_GiveOrder(Commander.Players.AbsLocalPlayer player)
            : base(player)
        {
            unitsAvailableForOrder();
            

            if (player.LocalHumanPlayer)
            {
                if (canBeOrdered.Count <= absPlayer.commandCard.ordersLeft)
                { //Suggest selecting everyone
                    foreach (var u in canBeOrdered.list)
                    {
                        CommandCard.OrderUnit(u);
                    }
                }

                nextAvailableUnit(true);

                if (toggRef.gamestate.gameSetup.level == LevelEnum.OrderMoveAttack)
                {
                    tutVid = new TutVideo.Order3Video();
                }

                cmdRef.hud.viewLocalPlayerHud();
            }
            else
            {//Ai
                if (!aiPlayer.IsPassive)
                {
                    new Timer.AsynchActionTrigger(asynchAiCalcOrderValue);
                }
            }
        }

        //public GamePhase_GiveOrder(Commander.Players.AiPlayer aiplayer)
        //    : base(aiplayer)
        //{
        //    if (!aiplayer.IsPassive)
        //    {
        //        basicInit();
        //        new Timer.AsynchActionTrigger(asynchAiCalcOrderValue);
        //    }
        //}

        /// <summary>
        /// Go through each unit and see what value the get for moving/attacking
        /// </summary>
        void asynchAiCalcOrderValue()
        {
            foreach (var u in canBeOrdered.list)
            {
                u.calcOrderValue();
            }

            orderValueSortedUnits = canBeOrdered.list.ToArray();
            if (orderValueSortedUnits.Length > 1)
            {
                arraylib.Quicksort(orderValueSortedUnits, false);
            }
            aiOrderValuationComplete = true;
        }

        bool aiorderingIsComplete = false;
        Time aiEndTimeout = new Time(1000);

        public override bool UpdateAi()
        {
            if (aiorderingIsComplete)
            {
                return aiEndTimeout.CountDown();
            }

            if (aiOrderValuationComplete)
            {
                int orderCount = lib.SmallestValue(orderValueSortedUnits.Length, CommandCard.OrderStartCount);
                for (int i = 0; i < orderCount; ++i)
                {
                    AbsUnit unit = orderValueSortedUnits[i];
                    CommandCard.OrderUnit(unit);
                    aiPlayer.SpectatorTargetPos = unit.squarePos;
                }
                
                aiOrderValuationComplete = false;

                aiorderingIsComplete = true;
            }

            return false;
        }


        //void basicInit()
        //{
        //    if (toggRef.gamestate.gameSetup.useStrategyCards)
        //    {   
        //        strategyCards = new StrategyCardsHud(absPlayer.settings.commandCardDeck, onSelectStrategyCard, null);
        //    }
        //    else
        //    {
        //        onSelectStrategyCard((int)CommandType.Order_3);
        //    }
        //}

        //void pickCommand(CommandType command)
        //{
        //    //var localPlayer = player as Players.LocalPlayer;

        //    if (player.commandCard != null)
        //    {
        //        player.commandCard.ClearOrders();
        //    }
        //    player.settings.commandCardDeck.SelectedCommand = command;
        //    //cmdRef.menu.CloseMenu();


        //    //if (settings.commandCardDeck.SelectedCommand != CommandType.NUM_NONE)
        //    //{
        //    //GamePhaseType nextPhase = Commander.GamePhaseType.GiveOrder;
        //    switch (player.settings.commandCardDeck.SelectedCommand)
        //    {
        //        case CommandType.Order_3:
        //            player.commandCard = new Command_Order3(player);
        //            break;
        //        case CommandType.Close_encounter:
        //            player.commandCard = new Command_CloseEncounter(player);
        //            player.commandCard.orders.EnableAll(true);
        //            break;
        //        case CommandType.Dark_Sky:
        //            player.commandCard = new Command_DarkSky(player);
        //            break;
        //        case CommandType.Wide_Advance:
        //            player.commandCard = new Command_WideAdvance(player);
        //            break;
        //        case CommandType.DoubleTime:
        //            player.commandCard = new Command_DoubleTime(player);
        //            break;
        //        case CommandType.Rush:
        //            player.commandCard = new Command_Rush(player);
        //            break;
        //    }

        //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdSelectedCommand, Network.PacketReliability.Reliable);
        //    w.Write((byte)player.commandCard.CommandType);

        //    // absPlayer.commandCard = commandCard;
        //    //absPlayer.nextPhase(true);//.StartPhase(nextPhase);
        //    //}
        //}

        //void onSelectStrategyCard(int command)
        //{

        //    if (absPlayer.commandCard != null)
        //    {
        //        absPlayer.commandCard.ClearOrders();
        //    }
        //    absPlayer.settings.commandCardDeck.SelectedCommand = (CommandType)command;
        //    //cmdRef.menu.CloseMenu();


        //    //if (settings.commandCardDeck.SelectedCommand != CommandType.NUM_NONE)
        //    //{
        //    //GamePhaseType nextPhase = Commander.GamePhaseType.GiveOrder;
        //    switch (absPlayer.settings.commandCardDeck.SelectedCommand)
        //    {
        //        case CommandType.Order_3:
        //            absPlayer.commandCard = new Command_Order3(absPlayer);
        //            break;
        //        case CommandType.Close_encounter:
        //            player.commandCard = new Command_CloseEncounter(player);
        //            player.commandCard.orders.EnableAll(true);
        //            break;
        //        case CommandType.Dark_Sky:
        //            player.commandCard = new Command_DarkSky(player);
        //            break;
        //        case CommandType.Wide_Advance:
        //            player.commandCard = new Command_WideAdvance(player);
        //            break;
        //        case CommandType.DoubleTime:
        //            player.commandCard = new Command_DoubleTime(player);
        //            break;
        //        case CommandType.Rush:
        //            player.commandCard = new Command_Rush(player);
        //            break;
        //    }

        //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdSelectedCommand, Network.PacketReliability.Reliable);
        //    w.Write((byte)absPlayer.commandCard.CommandType);

            
        //    unitsAvailableForOrder();
        //    if (canBeOrdered.Count <= absPlayer.commandCard.ordersLeft)
        //    { //Suggest selecting everyone
        //        foreach (var u in canBeOrdered.list)
        //        {
        //            CommandCard.OrderUnit(u);
        //        }
        //        //return true;
        //    }
        //    //return false;

        //    if (player != null)
        //    {
        //        player.commandCard.createToolTip();//player.mapControls.pointerIcon);
        //    }
        //}


        void unitsAvailableForOrder()
        {
            //ListWithSelection<AbsUnit> canBeOrdered = new ListWithSelection<AbsUnit>();
            //canBeOrdered.list.Clear();
            //List<IntVector2> positions = new List<IntVector2>(absPlayer.unitsColl.units.Count);
            //absPlayer.unitsColl.unitsCounter.Reset();
            //while (absPlayer.unitsColl.unitsCounter.Next())
            //{
            //    if (absPlayer.unitsColl.unitsCounter.sel.cmd().data.underType != UnitUnderType.Special_TacticalBase)
            //    {
            //        if (CommandCard.CanBeOrdered(absPlayer.unitsColl.unitsCounter.sel, true))
            //        {
            //            positions.Add(absPlayer.unitsColl.unitsCounter.sel.squarePos);
            //            canBeOrdered.Add(absPlayer.unitsColl.unitsCounter.sel, false);
            //        }
            //    }
            //}
            //canBeOrdered.selectedIndex = 0;
            List<IntVector2> positions;
            CommandCard.unitsAvailableForOrder(absPlayer, out canBeOrdered, out positions);

            if (player != null)
            {
                player.mapControls.SetAvailableTiles(positions);
                foreach (var m in canBeOrdered.list)
                {
                    if (m.order == null)
                    {
                        new OrderedUnit(m, CheckState.Suggested);
                    }
                    else
                    {
                        m.order.PhaseMarkVisible(false);
                    }
                }
            }
        }

        //public override void updateButtonDesc()
        //{
            
        //    bool availableSquare = CommandCard.CanBeOrdered(mapControls.selectedTile.unit, true);
        //    if (availableSquare)
        //    {
        //        bool containsOrder = CommandCard.containsUnit(mapControls.selectedTile.unit);

        //        if (containsOrder)
        //        {
        //            List<HUD.ButtonDescriptionData> ButtonDesc_RemoveOrder = new List<HUD.ButtonDescriptionData>();

        //            actionButton(ButtonDesc_RemoveOrder, "Remove order");
        //            //undoCommandButton(ButtonDesc_RemoveOrder);
        //            //EndPhaseButton(ButtonDesc_RemoveOrder);

        //            Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_RemoveOrder);
        //        }
        //        else
        //        {
        //            List<HUD.ButtonDescriptionData> ButtonDesc_AddOrder = new List<HUD.ButtonDescriptionData>();

        //            actionButton(ButtonDesc_AddOrder, "Order unit");
        //            //undoCommandButton(ButtonDesc_AddOrder);
        //            //EndPhaseButton(ButtonDesc_AddOrder);

        //            Commander.cmdRef.hud.buttonsOverview.Generate(ButtonDesc_AddOrder);
        //        }
        //    }
        //    else
        //    {
        //        viewBoardRoamButtonDesc();
        //    }

        //    updatePhaseInfoText();
        //}

        public override void Update(ref PhaseUpdateArgs args)
        {
            isNewState = false;

            //bool mouseOverStrategyCards = false;

            //if (strategyCards != null)
            //{
            //    mouseOverStrategyCards = strategyCards.update();
            //}
            //mapControls.tooltip.SetVisible(!mouseOverStrategyCards);
            //bool availableSquare = false;
            bool needRefresh = false;
            bool availableUnit = false;
            CantOrderReason cantOrderReason = CantOrderReason.NONE;

            if (!args.mouseOverHud)
            {
                mapControls.updateMapMovement(true);

                availableMapSquare = MapSquareAvailableType.None;
                

                if (mapControls.selectedTile.unit != null)
                {
                    availableMapSquare = MapSquareAvailableType.Disabled;
                   
                    availableUnit = CommandCard.CanBeOrdered(mapControls.selectedTile.unit, player, true, out cantOrderReason);

                    if (availableUnit)
                    {
                        if (CommandCard.containsUnit(mapControls.selectedTile.unit) || CommandCard.ordersLeft > 0)
                        {
                            availableMapSquare = MapSquareAvailableType.Enabled;

                            if (toggRef.inputmap.click.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuClick))
                            {
                                //Order unit
                                CommandCard.OrderUnit(mapControls.SelectedUnit);
                                refreshSuggestions();
                                needRefresh = true;


                            }
                        }
                    }
                    else if (cantOrderReason == CantOrderReason.Resting)
                    { 
                        
                    }
                }
               //// mapControls.setAvailable(availableUnit);
               // if (availableUnit)
               // {
               //     if (toggRef.inputmap.click.DownEvent)//inputMap.DownEvent(Input.ButtonActionType.MenuClick))
               //     {
               //         //Order unit
               //         CommandCard.OrderUnit(mapControls.SelectedUnit);
               //         needRefresh = true;
               //         //CommandCard.createToolTip();//mapControls.pointerIcon);
               //         //updateButtonDesc();
               //         //CommandCard.createToolTip();
               //     }
               // }
            }

            if (mapControls.mapLostFocus(args.mouseOverHud))
            {
                player.mapControls.removeToolTip();
            }
            else if (mapControls.isOnNewTile || mapControls.mapGotFocus(args.mouseOverHud) || needRefresh)
            {
                if (availableUnit)
                {
                    new OrderToolTip(mapControls.SelectedUnit.cmd().HasOrder,
                        CommandCard.orders.Count, CommandCard.OrderStartCount, player);
                }
                else if (cantOrderReason == CantOrderReason.Resting)
                {
                    new CantOrderTooltip(cantOrderReason, player);
                }
                else
                {
                    player.mapControls.removeToolTip();
                }
            }

            if (availableMapSquare != prevAvailableMapSquare)
            {
                prevAvailableMapSquare = availableMapSquare;
                mapControls.setAvailable(availableMapSquare);
            }

            if (toggRef.inputmap.menuInput.tabLeftUp.DownEvent)
            {
                nextAvailableUnit(false);
            }
            else if (toggRef.inputmap.menuInput.tabRightDown.DownEvent)
            {
                nextAvailableUnit(true);
            }
        }

        void refreshSuggestions()
        {
            CheckState disabled = CommandCard.ordersLeft > 0 ? CheckState.Suggested : CheckState.Disabled;

            foreach (var m in canBeOrdered.list)
            {
                if (m.order.state != CheckState.Enabled)
                {
                    m.order.SetState(disabled);
                }
            }
        }

        //void updateToolTip()
        //{ 

        //}

        public override bool canGoToPreviousPhase()
        {
            return true;
        }
        

        void nextAvailableUnit(bool next)
        {
            canBeOrdered.Next_IsEnd(next);
            mapControls.setSelectionPos(canBeOrdered.Selected().soldierModel.Position);
        }

        public override EnableType canExitPhase()
        {


            if (CommandCard == null || CommandCard.Complete || CommandCard.orders.Count >= canBeOrdered.Count)
                return EnableType.Enabled;
            else if (CommandCard.orders.Count > 0)
                return EnableType.Able_NotRecommended;
            else
                return EnableType.Disabled;
        }
        public override void EndTurnNotRecommendedText(out string title, out string message,out string okText)
        {
            title = "Start move phase?";
            message = "You can order more units";
            okText = "Start movement";
        }

        public override void OnExitPhase(bool forwardToNext)
        {
            base.OnExitPhase(forwardToNext);

            foreach (var m in canBeOrdered.list)
            {
                if (m.order != null && m.order.state != CheckState.Enabled)
                {
                    m.order.DeleteMe();
                }
            }
        }

        protected override string PhaseInfoText
        {
            get
            {
                if (CommandCard != null && CommandCard.HasOrderCount)
                {
                    return name + " " + CommandCard.orders.Count.ToString() + "/" + CommandCard.OrderStartCount;
                }
                else
                {
                    return name;
                }
            }
        }

        //public override void OnExitPhase(bool forwardToNext)
        //{
        //    base.OnExitPhase(forwardToNext);

        //    if (strategyCards != null)
        //    {
        //        strategyCards.DeleteMe();
        //    }
        //}

        //void pickCard()
        //{
        //    settings.commandCardDeck.SelectedCommand = command;
        //    //cmdRef.menu.CloseMenu();
        //    pickCommandCard();
        //}

        //void pickCommandCard()
        //{
        //    //if (settings.commandCardDeck.SelectedCommand != CommandType.NUM_NONE)
        //    //{
        //    //GamePhaseType nextPhase = Commander.GamePhaseType.GiveOrder;
        //    switch (settings.commandCardDeck.SelectedCommand)
        //    {
        //        case CommandType.Order_3:
        //            commandCard = new Command_Order3(absPlayer);
        //            break;
        //        case CommandType.Close_encounter:
        //            commandCard = new Command_CloseEncounter(player);
        //            commandCard.orders.EnableAll(true);
        //            //nextPhase = Commander.GamePhaseType.Attack;
        //            break;
        //        case CommandType.Dark_Sky:
        //            commandCard = new Command_DarkSky(player);
        //            break;
        //        case CommandType.Wide_Advance:
        //            commandCard = new Command_WideAdvance(player);
        //            break;
        //        case CommandType.DoubleTime:
        //            commandCard = new Command_DoubleTime(player);
        //            break;
        //        case CommandType.Rush:
        //            commandCard = new Command_Rush(player);
        //            break;
        //    }

        //    var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdSelectedCommand, Network.PacketReliability.Reliable);
        //    w.Write((byte)commandCard.CommandType);

        //    absPlayer.commandCard = commandCard;
        //    absPlayer.nextPhase(true);//.StartPhase(nextPhase);
        //    //}
        //}
        //public override bool borderVisuals(out Color bgColor, out SpriteName iconTile, out Color iconColor)
        //{
        //    bgColor = cmdLib.OrderActionReadyCol;
        //    iconTile = SpriteName.cmdOrderCheckFlat;
        //    iconColor = Color.White;

        //    return true;
        //}

        protected override string name
        {
            get { return "Order units"; }
        }

        public override void returnToThisPhaseBeginning()
        {
            //foreach (var m in CommandCard.orders.list)
            //{
            //    m.unit.undoMovement();
            //}
            CommandCard.ClearOrders();
            //refreshSuggestions();
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.GiveOrder; }
        }
    }

    enum CantOrderReason
    { 
        NONE,
        NotMyUnit,
        StaticUnit,
        Resting,
        AlreadyOrdered,
    }
}
