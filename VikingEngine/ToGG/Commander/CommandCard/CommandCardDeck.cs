using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VikingEngine.HUD;

namespace VikingEngine.ToGG.Commander.CommandCard
{
    class CmdStrategyCardDeck : AbsStrategyCardDeck
    {
        static readonly CommandType[] CardOrder = new CommandType[]
        {
            CommandType.Order_3,
            //CommandType.Wide_Advance,
            CommandType.Rush,
            CommandType.Close_encounter,
            CommandType.Dark_Sky,
            CommandType.DoubleTime,
        };

       // public StrategyCardDeck deck;//List<CommandType> deck;

        public CommandType SelectedCommand = CommandType.NUM_NONE;
        public List<StrategyCard> cards;

        public CmdStrategyCardDeck(ArmyRace race)
        {
            List<CommandType> deck;

            if (toggRef.gamestate.gameSetup.availableStrategyCards == null)
            {
                deck = new List<CommandType>(CardOrder);
            }
            else
            {
                deck = new List<CommandType>(toggRef.gamestate.gameSetup.availableStrategyCards.Count);
                foreach (var m in CardOrder)
                {
                    if (toggRef.gamestate.gameSetup.availableStrategyCards.Contains(m))
                    {
                        deck.Add(m);
                    }
                }
            }

            //deck = new StrategyCardDeck(cards);
            cards = new List<StrategyCard>(deck.Count);

            foreach (var m in deck)
            {
                cards.Add(
                    new StrategyCard((int)m,
                        AbsCommandCard.Name(m, toggRef.gamestate.gameSetup.armyScale, race),
                        AbsCommandCard.CommandDesc(m, toggRef.gamestate.gameSetup.armyScale, race),
                        AbsCommandCard.CardImage(m))
                    );
            }
        }

        public override List<AbsStrategyCard> Cards()
        {
            return arraylib.CastObject<StrategyCard, AbsStrategyCard>(cards);
        }
        //public void menuFile(Action<CommandType> callback)
        //{
        //    cmdRef.menu.OpenMenu(true);
        //    GuiLayout layout = new GuiLayout("Select Strategy", cmdRef.menu.menu);
        //    {
        //        foreach (CommandType command in deck)
        //        {
        //            AbsCommandCard.Card(command, , callback, layout);
        //        }
        //    }
        //    layout.End();
        //}

        public CommandType CardIxToCommand(int index)
        {
            return (CommandType)cards[index].Id;
        }
    }
}


