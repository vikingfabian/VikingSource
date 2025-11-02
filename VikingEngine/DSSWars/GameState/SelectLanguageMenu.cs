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

            DssRef.stats.pickLanguageStart.addOne();
        }

        void selectLanguegeLink(LanguageType language)
        {
            switch (language)
            {
                case LanguageType.English:
                    DssRef.stats.language_english.addOne();
                    break;
                case LanguageType.Japanese:
                    DssRef.stats.language_japanese.addOne();
                    break;
                case LanguageType.Russian:
                    DssRef.stats.language_russian.addOne();
                    break;
                case LanguageType.Spanish:
                    DssRef.stats.language_spanish.addOne();
                    break;
                case LanguageType.German:
                    DssRef.stats.language_german.addOne();
                    break;
                case LanguageType.French:
                    DssRef.stats.language_french.addOne();
                    break;
                case LanguageType.Turkish:
                    DssRef.stats.language_turkish.addOne();
                    break;
                case LanguageType.Portuguese:
                    DssRef.stats.language_brazilian_portuguese.addOne();
                    break;
                case LanguageType.Italian:
                    DssRef.stats.language_italian.addOne();
                    break;
                case LanguageType.Korean:
                    DssRef.stats.language_korean.addOne();
                    break;
                case LanguageType.Chinese:
                    DssRef.stats.language_simplified_chinese.addOne();
                    break;
            }

            Ref.gamesett.language = language;
            Ref.gamesett.fullscreenProperty(null, true, true);
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
