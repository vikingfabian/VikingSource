using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;

namespace VikingEngine.CardDesign.CardData
{
    /// <summary>
    /// All pools that are instanced to players and the bank
    /// </summary>
    class PlayerSupply
    {
        public Id id;

        public Dictionary<Id, CardPile> cardPileDic = new Dictionary<Id, CardPile>(2);
        public Dictionary<Id, ResourcePool> resources = new Dictionary<Id, ResourcePool>(2);

        public void ToEditButton(RichBoxContent content, string title, bool isPlayer)
        {
            content.h2(title, DSSWars.HudLib.TitleColor_Head2);
            content.space();
            cHud.EditButton(content, new RbAction(() => {
                cref.current.supply = this;
                cref.playState.menu.menuStack.Add(CardEditor.EditorMenu.Menu_Supply);
            }));

            content.newLine();
            DSSWars.HudLib.Label(content, "Resource pool");
            content.space();
            foreach (var item in resources.Values)
            {
                DSSWars.HudLib.BulletSeperationPoint(content);
                item.ToMenu(content);
            }

            content.newLine();
            DSSWars.HudLib.Label(content, "Card piles"); content.space();
            foreach (var item in cardPileDic.Values)
            {
                DSSWars.HudLib.BulletSeperationPoint(content);
                item.ToMenu(content);
            }

            if (isPlayer)
            {
                content.newLine();
                if (cardPileDic.Count == 0)
                {
                    content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Create default setup") },
                        new RbAction(() =>
                        {
                            addCardPile(new CardPile(CardPileType.Deck) { defaultPile = true, limit = new Number(30) });
                            addCardPile(new CardPile(CardPileType.Hand) { defaultPile = true, limit = new Number(8) });
                            addCardPile(new CardPile(CardPileType.Discard) { defaultPile = true });

                        })));
                }
            }
        }

        void addCardPile(CardPile cardPile)
        { 
            cardPileDic.Add(cardPile.id, cardPile);
        }

        public void ToEditor(RichBoxContent content, HUD.RichMenu.RichMenu menu)
        {
            content.h2("Edit supply", DSSWars.HudLib.TitleColor_Head2);
            DSSWars.HudLib.Label(content, "Resource pool");
            
            foreach (var item in resources.Values)
            {
                content.newLine();
                item.ToEditButton(content, this);
            }

            content.newLine();
            cHud.AddButton(content, "resource pool", new RbAction(() => {
                var newPool = new ResourcePool();
                resources.Add(newPool.id, newPool);
                }));

            content.newParagraph();

            DSSWars.HudLib.Label(content, "Card pile");

            foreach (var item in cardPileDic.Values)
            {
                content.newLine();
                item.ToEditButton(content, this);
            }

            content.newLine();
            cHud.AddButton(content, "card pile", new RbAction(() => {
                var newPile = new CardPile(CardPileType.Deck);
                cardPileDic.Add(newPile.id, newPile);
            }));
        }
        public bool DefaultCardPileProperty(object tag, bool set, bool val)
        {
            CardPile cardPile = (CardPile)tag;
            if (set)
            {
                if (val)
                {
                    //Clear others
                    foreach (var item in cardPileDic.Values)
                    {
                        if (item.pileType == cardPile.pileType)
                        {
                            item.defaultPile = false;
                        }
                    }
                }
                cardPile.defaultPile = val;
            }

            return cardPile.defaultPile;
        }
    }
}
