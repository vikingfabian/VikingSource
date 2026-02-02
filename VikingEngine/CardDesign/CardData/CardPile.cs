using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.CardDesign.CardData
{
    class CardPile : IHasText
    {
        public CardPileType pileType;
        PileInformationLevel informationLevel;

        public Id id;

        public Text name = Text.Empty;
        public Number limit = Number.Endless;
        public bool defaultPile;
        public PileAddOrder drawOrder = PileAddOrder.Top;
        public PileAddOrder dicardOrder = PileAddOrder.Top;

        public CardPile(CardPileType pileType)
        {
            this.pileType = pileType;
            this.id = Id.CreateNew(false);
            //this.name = new Text(pileType.ToString());
        }

        public void ToMenu(RichBoxContent content)
        {
            content.Add(new RbImage(SpriteName.CardBack));
            content.space();
            content.Add(new RbText(this.ToString()));
        }
        public void ToEditButton(RichBoxContent content, PlayerSupply supply)
        {
            ToMenu(content);
            content.space();

            cHud.EditButton(content, new RbAction(() => {
                cref.current.cardPile = this;
                cref.playState.menu.menuStack.Add(CardEditor.EditorMenu.Menu_CardPile);
            }));
            content.space(2);
            cHud.DeleteButton(content, new RbAction(() => {
                supply.cardPileDic.Remove(id);
            }));
        }

        public void ToEditor(RichBoxContent content, RichMenu menu, PlayerSupply supply)
        {
            DSSWars.HudLib.Label(content, ToString());
            content.newLine();
            DropDownBuilder dropDown = new DropDownBuilder("cards " + id.hash.ToString());
            for (CardPileType t = 0; t < CardPileType.NUM; t++)
            {
                dropDown.AddOption(t.ToString(), t == this.pileType, false,
                    new RbAction1Arg<CardPileType>((CardPileType value) => { pileType = value; menu.CloseDropDown(); }, t), null);
            }
            dropDown.Build(content, SpriteName.NO_IMAGE, "Type", menu);
            content.newLine();
            content.Add(new ArtCheckbox(new List<AbsRichBoxMember> { new RbText("Default pile for: " + pileType.ToString()) },
                supply.DefaultCardPileProperty)
            { propertyTag = this });

            content.newLine();
            new TextEditor(this, TextType.Name).ToEditor(content, menu, "Name");

            content.newLine();
            new NumberEditor().DragButton(content, menu, "Limit", Number.PositiveBounds, limitProperty); 

            content.newLine();
            DropDownBuilder drawDropDown = new DropDownBuilder("draw " + id.hash.ToString());
            for (PileAddOrder t = 0; t < PileAddOrder.NUM; t++)
            {
                drawDropDown.AddOption(t.ToString(), t == drawOrder, false,
                    new RbAction1Arg<PileAddOrder>((PileAddOrder value) => { drawOrder = value; }, t), null);
            }
            drawDropDown.Build(content, SpriteName.NO_IMAGE, "Draw order", menu);

            content.newLine();
            DropDownBuilder discardDropDown = new DropDownBuilder("disc " + id.hash.ToString());
            for (PileAddOrder t = 0; t < PileAddOrder.NUM; t++)
            {
                discardDropDown.AddOption(t.ToString(), t == drawOrder, false,
                    new RbAction1Arg<PileAddOrder>((PileAddOrder value) => { dicardOrder = value; }, t), null);
            }
            discardDropDown.Build(content, SpriteName.NO_IMAGE, "Discard order", menu);
        }
        

        int limitProperty(object tag, bool set, int value)
        {
            if (set)
            {
                limit.value = value;
            }
            return limit.value;
        }

        public Text GetText(TextType type) { return name; }
        public void SetText(TextType type, Text name) { this.name = name; }

        public override string ToString()
        {
            string nameText;
            if (name.IsEmpty)
            {
                nameText = pileType.ToString();
            }
            else
            {
                nameText = name.ToString();
            }
            return string.Format( "Card pile ({0})", nameText);
        }
    }


    enum CardPileType
    { 
        Deck,
        Hand,
        Discard,
        NUM
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
        NUM
    }
}
