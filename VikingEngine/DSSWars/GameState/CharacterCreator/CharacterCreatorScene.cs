using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Display.Translation;
using VikingEngine.DSSWars.Display;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.LootFest.Players;

namespace VikingEngine.DSSWars.GameState.CharacterCreator
{
    class CharacterCreatorScene : AbsDssState
    {
        public const float MinScale = 0.6f;
        public const float MaxScale = 1.4f;

        const float DefaultIconScale = 0.8f;

        public RichMenu menu;
        RichBoxSettings rbSettings;
        public CharacterCreatorScene() 
            :base()
        {
            openMenu();
            mainMenu();

            new Display.EditorBackground();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);
            bool mouseOver = false;
            menu.updateMouseInput(ref mouseOver);
        }

        void openMenu()
        {            
            if (menu == null)
            {

                var objectMenuArea = Screen.SafeArea;
                objectMenuArea.Width = (int)(Engine.Screen.IconSize * 9f);

                menu = new RichMenu(HudLib.RbSettings, objectMenuArea, new Vector2(8), RichMenu.DefaultRenderEdge, HudLib.GUILayer, XGuide.LocalHost);
                var bgTex = menu.addBackground(HudLib.HudMenuBackground, HudLib.GUILayer + 2);

                bgTex.SetColor(ColorExt.GrayScale(0.9f));

            }
        }
        public void Refresh(RichBoxContent content)
        {
            //openMenu();
            menu.Refresh(content);
        }

        int faceOption = 0;
        int bodyOption = 0;
        float scale = 1f;
        CharacterCreatorTab tab = 0;
        
        // int faceOption = 0;

        void mainMenu()
        { 
            RichBoxContent content = new RichBoxContent();
            content.h1("Character creator");

            List<CharacterCreatorTab> availableTabs = new List<CharacterCreatorTab> {
                CharacterCreatorTab.Soldiers,
                CharacterCreatorTab.Workers,
                CharacterCreatorTab.Animals
            };

            var tabs = new List<ArtTabMember>(availableTabs.Count);

            
            for (int i = 0; i < availableTabs.Count; ++i)
            {
                var text = new RbText(availableTabs[i].ToString());
                text.overrideColor = HudLib.RbSettings.tabSelected.Color;

                //AbsRbAction enter = null;
                //if (description != null)
                //{
                //    enter = new RbAction(() =>
                //    {
                //        RichBoxContent content = new RichBoxContent();
                //        content.text(description).overrideColor = HudLib.InfoYellow_Light;

                //        player.hud.tooltip.create(player, content, true);
                //    });
                //}

                tabs.Add(new ArtTabMember(new List<AbsRichBoxMember>
                { text }));
            }
            var tabGroup = new ArtTabgroup(tabs, arraylib.IndexFromValue(availableTabs, tab), 
                (int tabIx)=> { tab = availableTabs[tabIx]; }, null, SoundLib.menutab, null);



            content.h2("Default setup");

            content.newLine();
            HudLib.Label(content, "Scale");
            RbDragButton.RbDragButtonGroup(content, new List<float> { 0.1f }, new DragButtonSettings(MinScale, MaxScale, 0.1f), ScaleProperty);
            content.newParagraph();
            for (int i = 0; i < 4; i++)
            {
                content.Add(new ArtOption(i == faceOption, new List<AbsRichBoxMember> { new RbText("Face " + TextLib.IndexToString(i)) },
                    null));
            }
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { 
                new RbImage(SpriteName.WarsHudIconOpen),
                new RbSpace(),
                new RbText("Import model")
            }, null));

            content.newParagraph();
            for (int i = 0; i < 5; i++)
            {
                content.Add(new ArtOption(i == bodyOption, new List<AbsRichBoxMember> { new RbText("Body " + TextLib.IndexToString(i)) },
                    null));
            }
            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.WarsHudIconOpen),
                new RbSpace(),
                new RbText("Import model")
            }, null));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                new RbImage(SpriteName.pjNumPlus, 1, Color.Green),
                new RbSpace(),
                new RbText("Add accessory")
            }, null));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> {
                 new RbImage(SpriteName.WarsHudIconExit, DefaultIconScale), new RbSpace(), new RbText(DssRef.lang.Hud_Exit) },
               new RbAction(()=> { new ExitGamePlay(); })));


            Refresh(content);
        }


        float ScaleProperty(bool set, float value)
        {
            if (set) { scale = value; }
            return scale;
        }
    }

    enum CharacterCreatorTab
    { 
        Soldiers,
        Workers,
        Animals,
        NUM
    }
}
