using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.CardDesign.Entity;
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
        const string Menu_EditCard = "edit card";
        const string Menu_Image = "image";
        const string Menu_Cost = "cost";
        const string Menu_UnitProperties = "u properties";
        const string Menu_Trigger = "trigger";
        public const string Menu_Action = "action";

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
                    createCardMenu(content);
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
            }
            
        }

        void gameSetupMenu(RichBoxContent content)
        {
            content.h1("Game editor", DSSWars.HudLib.TitleColor_Head);

            content.newLine();
            new TextEditor(GameDb.Current, TextType.Name).ToEditor(content, menu, null);

            content.newLine();
            GameDb.Current.Id.toMenu(content);

            content.newParagraph();
            GameDb.Current.commonSupply.ToEditor(content, menu, "Common supply");

            content.newParagraph();
            GameDb.Current.playerSupply.ToEditor(content, menu, "Player supply");

            content.newParagraph();
            content.h2("Cards", DSSWars.HudLib.TitleColor_Head2);
            
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Unit card") },
                new RbAction1Arg<CardActionType>(createCard, CardActionType.FieldUnit), new RbTooltip_Text("For spawning creatures")));

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Action card") },
                new RbAction1Arg<CardActionType>(createCard, CardActionType.ActionTrigger), new RbTooltip_Text("For casting spells")));

            content.newParagraph();
            foreach (var kv in GameDb.Current.cards)
            {
                content.newLine();
                kv.Value.toEditButton(content);
            }

            void createCard(CardActionType actionType)
            { 
                var card = new CardEntity(actionType);
                EditorLib.CurrentCard = card.id;
                menu.menuStack.Add(Menu_EditCard);
            }
        }



        void createCardMenu(RichBoxContent content)
        {
            content.h1("Creature", DSSWars.HudLib.TitleColor_Head);
            content.h2(card.Id.ToString(), Color.DarkGray);

            content.newParagraph();
            RbText name;
            if (string.IsNullOrEmpty(card.name))
            {
                name = new RbText("Name", Color.Gray);
            }
            else
            {
                name = new RbText(card.name);
            }
            content.Add(new RbButton(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.InterfaceTextInput),
                new RbSpace(),
                name }, new RbAction(beginEditName), null, true, Color.White));

            content.newLine();
            RbText flavor;
            if (string.IsNullOrEmpty(card.flavor))
            {
                flavor = new RbText("Flavor text", Color.Gray);
            }
            else
            {
                flavor = new RbText(card.flavor);
            }
            content.Add(new RbButton(new List<AbsRichBoxMember> {
                new RbImage(SpriteName.InterfaceTextInput),
                new RbSpace(),
                flavor }, new RbAction(beginEditFlavor), null, true, Color.White));

            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbImage(card.image), new RbSpace(), new RbText("Image") },
                new RbAction(() => { menu.menuStack.Add(Menu_Image); })));

            content.newLine();
            DSSWars.HudLib.Label(content, "Cost");
            content.space();
            card.cost.ToMenu(content);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                new RbAction(() => { menu.menuStack.Add(Menu_Cost); })));

            content.newLine();
            DSSWars.HudLib.Label(content, "Properties");
            content.space();
            card.unitProperties.ToMenu(content);
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                new RbAction(() => { menu.menuStack.Add(Menu_UnitProperties); })));

            content.newParagraph();

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Add trigger") },
                new RbAction(() => { card.eventTriggers.Add(new Trigger()); })));
            content.newLine();

            for (int i = 0; i < card.eventTriggers.Count; i++)
            {
                card.eventTriggers[i].ToMenu(content);
                content.space();
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                    new RbAction1Arg<int>((int index) => { editTriggerIndex = index; menu.menuStack.Add(Menu_Trigger); }, i)));
                content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("X") },
                    new RbAction1Arg<int>((int index) => { card.eventTriggers.RemoveAt(index); }, i)));

                content.newLine();
            }

            content.newLine();
            content.Add(new RbSeperationLine());


            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DSSWars.DssRef.lang.Hud_Exit) },
                new RbAction(() => { editorState = EditorState.EditGame; })));

        }
        public void beginEditName()
        {
            new TextInputState(card.name, nameEditEvent, null);
        }
        void nameEditEvent(string result, object tag)
        {
            card.name = result;
            menu.needRefresh = true;
        }

        public void beginEditFlavor()
        {
            new TextInputState(card.flavor, flavorEditEvent, null);
        }
        void flavorEditEvent(string result, object tag)
        {
            card.flavor = result;
            menu.needRefresh = true;
        }
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
            card.image = sprite;
            menu.menuBack();
        }
        void costMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Cost", DSSWars.HudLib.TitleColor_Head);
            for (DefaultResourceType resource = 0; resource < DefaultResourceType.NUM_NONE; resource++)
            {
                IconName.Resource(resource, out var icon, out var name);
                content.newLine();
                content.Add(new RbImage(icon));
                content.Add(new RbText(name));
                content.Add(new RbTab(0.3f));
                RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Number.PositiveBounds, 1),
                            card.CostProperty, false, resource);
            }
        }

        void unitPropertiesMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Properties", DSSWars.HudLib.TitleColor_Head);
            card.unitProperties.ToEditor(content, menu);
          
        }

        void triggerMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Trigger", DSSWars.HudLib.TitleColor_Head);
            card.eventTriggers[editTriggerIndex].ToEditor(content, menu);
            
        }

        void triggerActionMenu(RichBoxContent content)
        {
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Action", DSSWars.HudLib.TitleColor_Head);
            card.eventTriggers[editTriggerIndex].actionList[editActionIndex].ToEditor(content, menu);
         
        }
    }
}
