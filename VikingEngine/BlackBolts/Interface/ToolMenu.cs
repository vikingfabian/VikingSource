using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using VikingEngine.DSSWars;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using static VikingEngine.PJ.Bagatelle.BagatellePlayState;

namespace VikingEngine.Core.BlackBolts.Interface
{
    class ToolMenu
    {
        RichMenu menu;

        public ToolMenu()
        {
            HudLib.Init();

            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top2, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top2_Back);

            refreshMenu();
        }
        public void refreshMenu()
        {
            iconMenu();
        }

        void iconMenu()
        {
            RichBoxContent content = new RichBoxContent();           
            content.h1("Hello Jam", HudLib.TitleColor_Head);
            content.newLine();
            content.Add(new RbButton(new List<AbsRichBoxMember> { new RbText("Click me") }, null));
            menu.Refresh(content);
        }
        public void update(ref bool mouseOver)
        {
            menu.updateMouseInput(ref mouseOver);
            if (menu.needRefresh)
            {
                refreshMenu();
            }
        }

    }
}
