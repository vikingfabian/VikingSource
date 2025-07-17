using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Presentation;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichMenu;

namespace VikingEngine.DSSWars.GameState
{
    
    class SelectLanguageMenu : Engine.GameState
    {
        RichMenu menu;

        public SelectLanguageMenu()
            : base()
        {
            VectorRect menuArea = Engine.Screen.SafeArea;
            HudLib.Init();
            menuArea.Width = HudLib.HeadDisplayWidth;
            menuArea.X = Engine.Screen.CenterScreen.X - menuArea.Width / 2;

            menu = new RichMenu(HudLib.RbOnGuiSettings, menuArea, new Vector2(8), RichMenu.DefaultRenderEdge, ImageLayers.Lay0, new PlayerData(PlayerData.AllPlayers));

            Presentation.Translation translate = new Presentation.Translation();
            var options = translate.available();
            //GuiLayout layout = new GuiLayout(string.Empty, menuSystem.menu);
            //{
            RichBoxContent content = new RichBoxContent();
            
            foreach (var option in options)
            {
                content.newLine();
                var btn = new RbButton(new List<AbsRichBoxMember> { new RbImage(translate.sprite(option)) },
                    new RbAction1Arg<LanguageType>(selectLanguegeLink, option));
                btn.overrideBgColor = ColorExt.VeryDarkGray;
                content.Add(btn);
                //new GuiImageButton(translate.sprite(option), null, new GuiAction1Arg<LanguageType>(selectLanguegeLink, option), false, layout);
            }
            //}
            //layout.End();

            menu.Refresh(content);
        }

        void selectLanguegeLink(LanguageType language)
        {
            Ref.gamesett.language = language;
            new ChangeLanguageRefresh();
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            bool mouseOver = false;
            menu.updateMouseInput(ref mouseOver);
        }
    }
}
