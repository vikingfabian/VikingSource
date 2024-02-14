using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VikingEngine.ToGG.Commander.CommandCard;
using Microsoft.Xna.Framework.Input;
using VikingEngine.HUD;
using Microsoft.Xna.Framework;
using VikingEngine.ToGG.ToggEngine.Display2D;

namespace VikingEngine.ToGG
{
    class GamePhase_SelectCommand : AbsGamePhase
    {
        public const string Name = "Select Strategy";

        StrategyCardsHud strategyCardsHud;

        public GamePhase_SelectCommand(Commander.Players.AbsLocalPlayer player)
            : base(player)
        {
            if (player.LocalHumanPlayer)
            {
                player.settings.commandCardDeck.SelectedCommand = CommandType.NUM_NONE;
                //selectCommandPromt();
                //Commander.cmdRef.hud.buttonsOverview.DeleteAll();

                //highlight available units
                List<IntVector2> available = new List<IntVector2>();
                player.unitsColl.unitsCounter.Reset();
                CantOrderReason cantOrderReason;

                while (player.unitsColl.unitsCounter.Next())
                {
                    if (player.unitsColl.unitsCounter.sel.cmd().CanBeOrdered(false, out cantOrderReason))
                    {
                        available.Add(player.unitsColl.unitsCounter.sel.squarePos);
                    }
                }
                player.mapControls.SetAvailableTiles(available);
                player.mapControls.setAvailable(MapSquareAvailableType.None);

                strategyCardsHud = new StrategyCardsHud(settings.commandCardDeck, onSelectStrategy, player);
                Commander.cmdRef.hud.hidePhaseButtons();
            }
            else
            {//Ai
                settings.commandCardDeck.SelectedCommand = CommandType.Order_3;
                new Effects.ViewSelectedCommand(settings.commandCardDeck.SelectedCommand);
            }
        }

        //public GamePhase_SelectCommand(Commander.Players.AiPlayer aiplayer)
        //    : base(aiplayer)
        //{
        //    settings.commandCardDeck.SelectedCommand = CommandType.Order_3;
        //    new Effects.ViewSelectedCommand(settings.commandCardDeck.SelectedCommand);
        //}

        //void selectCommandPromt()
        //{
        //    Commander.cmdRef.hud.viewInputPrompt(Name, toggRef.inputmap.click.Icon);//player.inputMap.ButtonIcon(Input.ButtonActionType.MenuClick));
        //}

        public override void Update(ref PhaseUpdateArgs args)
        {
            isNewState = false;

            args.mouseOverHud |= strategyCardsHud.update();

            if (!args.mouseOverHud)
            {
                mapControls.updateMapMovement(true);

                if (toggRef.inputmap.click.DownEvent &&
                    strategyCardsHud != null)
                {
                    strategyCardsHud.createAlertFlash();
                }
            }
            //if (cmdRef.gamestate.gameSetup.useStrategyCards)
            //{
            //    mapControls.updateMapMovement(true);
            //    if (player.inputMap.DownEvent(Input.ButtonActionType.MenuClick))
            //    {
            //        settings.commandCardDeck.menuFile(OnMenuSelect);
            //    }
            //}
            //else
            //{
            //    autoSkippedPhase = true;
            //    settings.commandCardDeck.SelectedCommand = CommandType.Order_3;
            //    new Effects.ViewSelectedCommand(settings.commandCardDeck.SelectedCommand);
            //    pickCommandCard();
            //}

        }

        public override bool UpdateAi()
        {
            pickCommandCard();
            return false;
        }


        void onSelectStrategy(int type)
        {
            settings.commandCardDeck.SelectedCommand = (CommandType)type;
            pickCommandCard();
        }

            public void OnMenuSelect(CommandType command)
        {
            //var command = settings.commandCardDeck.CardIxToCommand(buttonIndex);
            if (command == CommandType.Close_encounter)
            {
                //if (CommandCard.Command_CloseEncounter.AvailableUnits(player).Count == 0)
                //{
                //    cmdRef.menu.OpenMenu(true);
                //    GuiLayout layout = new GuiLayout("No available units", cmdRef.menu.menu);
                //    {
                //        new GuiLabel("You don't have any rested units adjacent to opponents", layout);
                //        new GuiTextButton("OK", null, cmdRef.menu.menu.PopLayout, false, layout);
                //    }
                //    layout.End();
                    

                //    return;
                //}
            }

            //settings.commandCardDeck.SelectedCommand = command;
            //toggRef.menu.CloseMenu();
            //pickCommandCard();
           // settings.commandCardDeck.OnCommandSelect(command);
            //cmdRef.menu.CloseMenu();
        }

        void pickCommandCard()
        {

            //switch (settings.commandCardDeck.SelectedCommand)
            //{
            //    case CommandType.Order_3:
            //        CommandCard = new Command_Order3(absPlayer);
            //        break;
            //    case CommandType.Close_encounter:
            //        CommandCard = new Command_CloseEncounter(player);
            //        CommandCard.orders.EnableAll(true);
            //        //nextPhase = Commander.GamePhaseType.Attack;
            //        break;
            //    case CommandType.Dark_Sky:
            //        CommandCard = new Command_DarkSky(player);
            //        break;
            //    case CommandType.Wide_Advance:
            //        CommandCard = new Command_WideAdvance(player);
            //        break;
            //    case CommandType.DoubleTime:
            //        CommandCard = new Command_DoubleTime(player);
            //        break;
            //    case CommandType.Rush:
            //        CommandCard = new Command_Rush(player);
            //        break;
            //}

            //var w = Ref.netSession.BeginWritingPacket(Network.PacketType.cmdSelectedCommand, Network.PacketReliability.Reliable);
            //w.Write((byte)CommandCard.CommandType);

            absPlayer.commandCard = AbsCommandCard.Get(settings.commandCardDeck.SelectedCommand);
            absPlayer.commandCard.init(absPlayer);
            absPlayer.nextPhase(true);
        }

        public override void EndTurnNotRecommendedText(out string title, out string message, out string okText)
        {
            throw new NotImplementedException();
        }
        public override EnableType canExitPhase()
        {
            return EnableType.Disabled;    
        } 

        public override void OnCancelMenu()
        {
            //selectCommandPromt();
        }

        //public override bool borderVisuals(out Color bgColor, out SpriteName iconTile, out Color iconColor)
        //{
        //    bgColor = Color.SaddleBrown;
        //    iconTile = SpriteName.LfCardItemIcon;
        //    iconColor = Color.White;

        //    return true;
        //}
        public override void OnExitPhase(bool forwardToNext)
        {
            base.OnExitPhase(forwardToNext);
            strategyCardsHud?.DeleteMe();
        }


        public override void returnToThisPhaseBeginning()
        {
            CommandCard.ClearOrders();
            //player.StartPhase(GamePhaseType.SelectCommand);
        }

        protected override string name
        {
            get { return Name; }
        }

        public override GamePhaseType Type
        {
            get { return GamePhaseType.SelectCommand; }
        }
    }
}
