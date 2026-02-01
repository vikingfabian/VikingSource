using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikingEngine.CardDesign.CardData
{
    class CardPile : IHasText
    {
        CardPileType pileType;
        PileInformationLevel informationLevel;

        public Id id;

        public Text name = Text.Empty;
        public Number limit;
        public bool defaultPile;
        public PileAddOrder drawOrder;
        public PileAddOrder dicardOrder;

        public CardPile(CardPileType pileType)
        {
            this.pileType = pileType;
            this.id = Id.CreateNew(false);
            this.name = new Text(pileType.ToString());
        }

        public Text GetText(TextType type) { return name; }
        public void SetText(TextType type, Text name) { this.name = name; }
    }


    enum CardPileType
    { 
        Deck,
        Hand,
        Discard,
    }

    enum PileInformationLevel
    { 
        None,
        ViewCards,
        ViewCardsAndOrder,
        NUM
    }

    enum PileAddOrder
    { 
        None,
        Top,
        Bottom,
        Random,
    }
}
