using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardGraphics;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Interface;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.Input;
using VikingEngine.ToGG.HeroQuest.QueAction;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VikingEngine.CardDesign
{
    class CardDesignPlayState : Engine.GameState
    {
        const string Menu_Settings = "sett";
        const string Menu_Image = "image";
        const string Menu_Cost = "cost";
        const string Menu_UnitProperties = "u properties";
        const string Menu_Trigger = "trigger";
        public const string Menu_Action = "action";

        public RichMenu menu;
        EditorBackground bg;
        EditorState editorState = EditorState.EditGame;
        CreatureCard card = new CreatureCard();
        CardGraphics.CardFace cardPreview = null;
        int editTriggerIndex = -1;
        public int editActionIndex = -1;
        public CardDesignPlayState()
            : base()
        {
            CardRef.playState = this;
            DSSWars.HudLib.Init();
            bg = new EditorBackground();
            openMenu();
        }

        void openMenu()
        {
            if (menu == null)
            {
                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = (int)(Engine.Screen.IconSize * 9f);

                menu = new RichMenu(DSSWars.HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, DSSWars.HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(DSSWars.HudLib.HudMenuBackground, DSSWars.HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));
                mainMenu();
            }
        }

        void mainMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Card Games - Editor", DSSWars.HudLib.TitleColor_Head);

            content.newLine();
            DSSWars.HudLib.Label(content, "Add card");
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Creature") },
                new RbAction(() => { editorState = EditorState.EditCard; })));

            content.newParagraph();
            content.Add(new RbText("You can only modify one card", DSSWars.HudLib.InfoYellow_Light));
            content.newLine();
            content.Add(new RbText("Proof of concept - prototype 2", Color.LightGray));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudIconSettings), new RbSpace(), new RbText(DssRef.lang.Lobby_Category_Options) },
                 new RbAction(() => { menu.menuStack.Add(Menu_Settings); })));

            content.newLine();
            content.Add(new RbSeperationLine());
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(DSSWars.DssRef.lang.Hud_Exit) },
                new RbAction(Ref.update.ExitToDash)));

            Refresh(content);
        }

        void optionsMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);

            content.h2(DssRef.lang.Settings_Title_Monitor, HudLib.TitleColor_Head);
            Ref.gamesett.monitorOptions(content, menu);

            Refresh(content);
        }

        void createCardMenu()
        {
            RichBoxContent content = new RichBoxContent();
            //DSSWars.HudLib.returnButton(content, menu, true, null);

            content.h1("Creature", DSSWars.HudLib.TitleColor_Head);
            content.h2(card.guid.ToString(), Color.DarkGray);

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

            Refresh(content);
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
        void imageOptions()
        {
            RichBoxContent content = new RichBoxContent();
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
            Refresh(content);
        }

        void selectImage(SpriteName sprite)
        { 
            card.image = sprite;
            menu.menuBack();
        }
        void costMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Cost", DSSWars.HudLib.TitleColor_Head);
            for (ResourceType resource = 0; resource < ResourceType.NUM_NONE; resource++)
            {
                IconName.Resource(resource, out var icon, out var name);
                content.newLine();
                content.Add(new RbImage(icon));
                content.Add(new RbText(name));
                content.Add(new RbTab(0.3f));
                RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Const.PositiveBounds, 1),
                            card.CostProperty, false, resource);
            }

            Refresh(content);
        }

        void unitPropertiesMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Properties", DSSWars.HudLib.TitleColor_Head);
            card.unitProperties.ToEditor(content, menu);
            Refresh(content);
        }

        void triggerMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Trigger", DSSWars.HudLib.TitleColor_Head);
            card.eventTriggers[editTriggerIndex].ToEditor(content, menu);
            Refresh(content);
        }

        void triggerActionMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h1("Action", DSSWars.HudLib.TitleColor_Head);
            card.eventTriggers[editTriggerIndex].actionList[editActionIndex].ToEditor(content, menu);
            Refresh(content);
        }

        public void Refresh(RichBoxContent content)
        {
            //openMenu();
            menu.Refresh(content);
        }
        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOver = false;

            menu.updateMouseInput(ref mouseOver);

            if (menu.needRefresh)
            {
                refreshPage();
                menu.needRefresh = false;

                if (card != null)
                {
                    if (cardPreview == null)
                    {
                        cardPreview = new CardGraphics.CardFace(card);
                        cardPreview.position = menu.backgroundArea.RightTop;
                        cardPreview.size = CardFace.FullTargetSize * 1f;

                    }
                    else
                    { 
                        cardPreview.generateTexture();  
                    }
                }
            }            
        }

        void refreshPage()
        {
            switch (editorState)
            {
                case EditorState.EditGame:
                    switch (menu.menuStack.LastOrDefault())
                    {
                        default:
                            mainMenu();
                            break;
                        case Menu_Settings:
                            optionsMenu();
                            break;
                    }
                    break;
                case EditorState.EditCard:
                    switch (menu.menuStack.LastOrDefault())
                    {
                        default:
                            createCardMenu();
                            break;
                        case Menu_Image:
                            imageOptions();
                            break;
                        case Menu_Cost:
                            costMenu();
                            break;
                        case Menu_UnitProperties:
                            unitPropertiesMenu();
                            break;
                        case Menu_Trigger: 
                            triggerMenu();
                            break;
                        case Menu_Action:
                            triggerActionMenu();
                            break;
                    }
                    break;
            }
        }
    }

    enum EditorState
    { 
        EditGame,
        EditCard,
    }
}