using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.CardDesign.CardData;
using VikingEngine.CardDesign.CardEditor;
using VikingEngine.CardDesign.CardGraphics;
using VikingEngine.CardDesign.Entity;
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

namespace VikingEngine.CardDesign
{
    class CardDesignPlayState : Engine.GameState
    {
        const string Menu_Settings = "sett";
        

        public RichMenu menu;
        EditorBackground bg;
        //EditorState editorState = EditorState.EditGame;
        EditorMenu editorMenu = null;
        CardEntity card = new CardEntity();
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
                new RbAction(() => { editorMenu = new EditorMenu(menu); })));

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
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Restart game state") },
                new RbAction(() => { new CardDesignPlayState(); })));
            content.newLine();
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
            if (editorMenu != null)
            {
                RichBoxContent content = new RichBoxContent();
                editorMenu.refreshPage(content);
                Refresh(content);
            }
            else
            {
                    switch (menu.menuStack.LastOrDefault())
                    {
                        default:
                            mainMenu();
                            break;
                        case Menu_Settings:
                            optionsMenu();
                            break;
                    }

                
            }
        }
    }

    enum EditorState
    { 
        EditGame,
        EditCard,
    }
}