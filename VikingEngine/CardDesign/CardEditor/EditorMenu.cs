using Microsoft.Xna.Framework;
using Sentry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.CardDesign.Entity;
using VikingEngine.EngineSpace.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.PJ.CarBall;
using VikingEngine.PJ.MiniGolf;

namespace VikingEngine.CardDesign.CardEditor
{
    class EditorMenu
    {
        const string Menu_ListCards = "list cards";
        public const string Menu_EditCard = "edit card";
        const string Menu_Image = "image";
        const string Menu_Cost = "cost";
        public const string Menu_UnitProperties = "u properties";
        public const string Menu_Trigger = "trigger";
        public const string Menu_Action = "action";
        public const string Menu_GameTags = "game tags";
        public const string Menu_ResourcePool = "r pool";
        public const string Menu_CardPile = "c pile";
        public const string Menu_Supply = "supply";

        RichMenu menu;

        public EditorMenu(RichMenu menu) 
        { 
            this.menu = menu;
        }

        public void refreshPage(RichBoxContent content)
        {
            
            switch (menu.menuStack.LastOrDefault())
            {
                default:
                    gameSetupMenu(content);
                    break;
                case Menu_Supply:
                    DSSWars.HudLib.returnButton(content, menu, true, null);
                    cref.current.supply.ToEditor(content, menu); 
                    break;
                case Menu_ResourcePool:
                    DSSWars.HudLib.returnButton(content, menu, true, null);
                    cref.current.resourcePool.ToEditor(content, menu);
                    break;
                case Menu_CardPile:
                    DSSWars.HudLib.returnButton(content, menu, true, null);
                    cref.current.cardPile.ToEditor(content, menu, cref.current.supply);
                    break;
                case Menu_GameTags:
                    DSSWars.HudLib.returnButton(content, menu, true, null);
                    new TagEditor().AllToEditor(content, menu, cref.current.editIsTag);
                    break;
                case Menu_Image:
                    imageOptions(content);
                    break;
                case Menu_Cost:
                    costMenu(content);
                    break;
                case Menu_UnitProperties:
                    unitPropertiesMenu(content);
                    break;
                case Menu_Trigger:
                    triggerMenu(content);
                    break;
                case Menu_Action:
                    triggerActionMenu(content);
                    break;
                case Menu_ListCards:
                    listCardsMenu(content);
                    break;
                case Menu_EditCard:
                    editCardMenu(content);
                    break;
            }
            
        }

        void gameSetupMenu(RichBoxContent content)
        {
            content.h1("Game editor", DSSWars.HudLib.TitleColor_Head);
            content.newLine();
            cref.current.game.Id.toMenu(content);

            content.newLine();
            new TextEditor(cref.current.game, TextType.Name).ToEditor(content, menu, null);
            content.newLine();
            new TextEditor(cref.current.game, TextType.Description).ToEditor(content, menu, "Description");


            content.newParagraph();
            //content.h2("Map type", DSSWars.HudLib.TitleColor_Head2);
            DropDownBuilder mapDropDown = new DropDownBuilder("Map type");
            {
                for (MapType map = 0; map < MapType.NUM; map++)
                {
                    mapDropDown.AddOption(map.ToString(), cref.current.game.mapType == map, MapType.Lanes == map,
                        new RbAction1Arg<MapType>((MapType value) => { cref.current.game.mapType = value; menu.CloseDropDown(); }, map), null);
                }
                mapDropDown.Build(content, SpriteName.NO_IMAGE, "Map type", menu);
            }

            content.newParagraph();
            //content.h2("Resources", DSSWars.HudLib.TitleColor_Head2);
            new TagEditor().AllToEditButton(content, false);

            content.newLine();
            new TagEditor().AllToEditButton(content, true);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbText(string.Format("Edit {0} ({1})", "cards", cref.current.game.cards.Count))
            }, new RbAction(() => { cref.playState.menu.menuStack.Add(Menu_ListCards); })));

            content.newParagraph();
            cref.current.game.commonSupply.ToEditButton(content, "Common supply", false);


            content.newParagraph();
            cref.current.game.playerSupply.ToEditButton(content, "Player supply", true);
            content.newLine();
            content.Add(new RbSeperationLine());

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DSSWars.DssRef.lang.Hud_Exit) },
                new RbAction(() => { cref.playState.closeEditor(); })));
        }

        void listCardsMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);

            content.h1("Cards", DSSWars.HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Unit card") },
                new RbAction1Arg<CardActionType>(createCard, CardActionType.FieldUnit), new RbTooltip_Text("For spawning creatures")));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Action card") },
                new RbAction1Arg<CardActionType>(createCard, CardActionType.ActionTrigger), new RbTooltip_Text("For casting spells")));
            content.newParagraph();
            foreach (var kv in cref.current.game.cards)
            {
                content.newLine();
                kv.Value.toEditButton(content, menu);
            }
            void createCard(CardActionType actionType)
            {
                var card = new CardEntity(actionType);
                cref.current.card = card;
                menu.menuStack.Add(Menu_EditCard);
            }
        }


        void editCardMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);

            //CardEntity card = cref.current.game.cards[EditorLib.CurrentCard];
            CardEntity card = cref.current.card;



            content.h1("Edit card", DSSWars.HudLib.TitleColor_Head);
            content.h2(card.Id.ToString(), Color.DarkGray);

            content.newParagraph();
            card.CardContent.toEditor(content, menu);
            //new TextEditor(card, TextType.Name).ToEditor(content, menu, "Name");

            //content.newLine();
            //new TextEditor(cref.current.game, TextType.Flavor).ToEditor(content, menu, "Flavor text");

            //RbText name;
            //if (string.IsNullOrEmpty(card.name))
            //{
            //    name = new RbText("Name", Color.Gray);
            //}
            //else
            //{
            //    name = new RbText(card.name);
            //}
            //content.Add(new RbButton(new List<AbsRichBoxMember> {
            //    new RbImage(SpriteName.InterfaceTextInput),
            //    new RbSpace(),
            //    name }, new RbAction(beginEditName), null, true, Color.White));

            //content.newLine();
            //RbText flavor;
            //if (string.IsNullOrEmpty(card.flavor))
            //{
            //    flavor = new RbText("Flavor text", Color.Gray);
            //}
            //else
            //{
            //    flavor = new RbText(card.flavor);
            //}
            //content.Add(new RbButton(new List<AbsRichBoxMember> {
            //    new RbImage(SpriteName.InterfaceTextInput),
            //    new RbSpace(),
            //    flavor }, new RbAction(beginEditFlavor), null, true, Color.White));

            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbImage(card.CardContent.image), new RbSpace(), new RbText("Image") },
                new RbAction(() => { menu.menuStack.Add(Menu_Image); })));

            content.newLine();
            DSSWars.HudLib.Label(content, "Cost");
            content.space();
            card.cost.ToMenu(content, "Free");
            content.space(2);
            cHud.EditButton(content, new RbAction(() => { menu.menuStack.Add(Menu_Cost); }));
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
            //    new RbAction(() => { menu.menuStack.Add(Menu_Cost); })));

            content.newLine();
            card.action.toEditor(content, menu);
            //DSSWars.HudLib.Label(content, "Properties");
            //content.space();
            //card.unitProperties.ToMenu(content);
            //content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
            //    new RbAction(() => { menu.menuStack.Add(Menu_UnitProperties); })));

            

           

            

        }
        //public void beginEditName()
        //{
        //    new TextInputState(cref.current.card.name, nameEditEvent, null);
        //}
        //void nameEditEvent(string result, object tag)
        //{
        //    card.name = result;
        //    menu.needRefresh = true;
        //}

        //public void beginEditFlavor()
        //{
        //    new TextInputState(card.flavor, flavorEditEvent, null);
        //}
        //void flavorEditEvent(string result, object tag)
        //{
        //    card.flavor = result;
        //    menu.needRefresh = true;
        //}
        void imageOptions(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);

            content.Add(new RichBoxScale(2f));
            content.newParagraph();
            for (SpriteName creature = SpriteName.CardCreatureImageStart; creature < SpriteName.CardCreatureImageEnd; creature++)
            {
                option(creature);
            }
            content.newLine();
            for (SpriteName spell = SpriteName.CardSpellImageStart; spell < SpriteName.CardSpellImageEnd; spell++)
            {
                option(spell);
            }

            void option(SpriteName sprite)
            {
                content.Add(new ArtButton(RbButtonStyle.Outline, new List<AbsRichBoxMember> { new RbImage(sprite) },
                    new RbAction1Arg<SpriteName>(selectImage, sprite)));
            }
        }

        void selectImage(SpriteName sprite)
        {
            cref.current.card.CardContent.image = sprite;
            menu.menuBack();
        }
        void costMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Cost", DSSWars.HudLib.TitleColor_Head);
            //for (DefaultResourceType resource = 0; resource < DefaultResourceType.NUM_NONE; resource++)
            foreach (var resource in cref.current.game.tagDic.Values)
            {
                if (resource.IsResource)
                {
                    //IconName.Resource(resource, out var icon, out var name);
                    content.newLine();
                    content.Add(new RbImage(resource.icon));
                    content.Add(new RbText(resource.name.ToString()));
                    content.Add(new RbTab(0.3f));
                    RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Number.PositiveBounds, 1),
                                cref.current.card.cost.CostProperty, false, resource.id);
                }
            }

            if (cref.current.game.tagDic.Count == 0)
            { 
                content.newLine();
                content.text("Please add resources", DSSWars.HudLib.InfoYellow_Light);
            }
        }

        void unitPropertiesMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Properties", DSSWars.HudLib.TitleColor_Head);
            cref.current.card.action.GetUnit().unitProperties.ToEditor(content, menu);

        }

        void triggerMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Trigger", DSSWars.HudLib.TitleColor_Head);
            cref.current.editTrigger.ToEditor(content, menu);

        }

        void triggerActionMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1(cref.current.editAction.Type.ToString() + " action", DSSWars.HudLib.TitleColor_Head);
            cref.current.editAction.ToEditor(content, menu, cref.current.card.action.ActionType == CardActionType.FieldUnit);

        }
    }
}
