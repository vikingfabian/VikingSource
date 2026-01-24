using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.GameState;
using VikingEngine.DSSWars.Interface;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.ToGG.HeroQuest.QueAction;

namespace VikingEngine.CardDesign
{
    class CardDesignPlayState : Engine.GameState
    {
        const string Menu_Cost = "cost";

        public RichMenu menu;
        EditorBackground bg;
        EditorState editorState = EditorState.EditGame;
        CreatureCard card = new CreatureCard();
        public CardDesignPlayState()
            : base()
        {
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
            content.h2("Card Games - Editor");

            content.newLine();
            DSSWars.HudLib.Label(content, "Add card");
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("+ Creature") },
                new RbAction(() => { editorState = EditorState.EditCard; })));

            content.newParagraph();
            content.Add(new RbText("You can only modify one card", DSSWars.HudLib.InfoYellow_Light));
            content.newLine();
            content.Add(new RbText("Proof of concept - prototype 1", Color.LightGray));

            Refresh(content);
        }

        void createCardMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h2("Creature");

            content.newLine();
            DSSWars.HudLib.Label(content, "Cost");

            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbImage(SpriteName.WarsHudIconSettings) },
                new RbAction(() => { menu.menuStack.Add(Menu_Cost); })));


            Refresh(content);
        }

        void costMenu()
        {
            RichBoxContent content = new RichBoxContent();
            DSSWars.HudLib.returnButton(content, menu, true, null);
            content.h2("Cost");
            for (ResourceType resource = 0; resource < ResourceType.NUM_NONE; resource++)
            {
                IconName.Resource(resource, out var icon, out var name);
                content.newLine();
                content.Add(new RbImage(icon));
                content.hspace();
                content.Add(new RbText(name));
                content.Add(new RbTab(0.3f));
                RbDragButton.RbDragButtonGroup(content, new List<float> { 1f }, new DragButtonSettings(Resources.CostBounds, 1),
                            SeedProperty, false);
            }

            Refresh(content);
        }

        int SeedProperty(bool set, int value)
        {
            if (set)
            {
                card.resources.mana = value;
            }
            return card.resources.mana;
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
                    }
                    break;
                case EditorState.EditCard:
                    switch (menu.menuStack.LastOrDefault())
                    {
                        default:
                            createCardMenu();
                            break;
                        case Menu_Cost:
                            costMenu();
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