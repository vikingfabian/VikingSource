using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.Engine;
using VikingEngine.HUD.RichBox;
using VikingEngine.HUD.RichBox.Artistic;
using VikingEngine.HUD.RichMenu;
using VikingEngine.PJ;

namespace VikingEngine.DSSWars.GameState.MapEditor
{
    class MapGeneratorDisplay
    {
        RichMenu menu;
        MapEditor_Generator state;
        public MapGeneratorDisplay(MapEditor_Generator state) 
        { 
            this.state = state;

            var area = Screen.SafeArea;
            area.Width = Screen.IconSize * 8;

            menu = new RichMenu(HudLib.RbSettings, area, new Vector2(10), RichMenu.DefaultRenderEdge, ImageLayers.Top1, new PlayerData(PlayerData.AllPlayers));
            menu.addBackground(HudLib.HudMenuBackground, ImageLayers.Top1_Back);

            refreshMenu();
        }

        void refreshMenu()
        {
            RichBoxContent content = new RichBoxContent();
            content.h1("Map editor - generate", HudLib.TitleColor_Head);

            content.newLine();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbText("Generate") }, new RbAction(state.generate)));

            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbText(DssRef.lang.Settings_NewGame) }, new RbAction(state.startNewGame)));

            content.newParagraph();
            content.Add(new ArtButton(RbButtonStyle.Primary,
                new List<AbsRichBoxMember> { new RbText(DssRef.lang.Lobby_ExitGame) }, new RbAction(exit)));


            menu.Refresh(content);
        }

        void exit()
        {
            new ExitToLobbyState();
        }
    }
}
