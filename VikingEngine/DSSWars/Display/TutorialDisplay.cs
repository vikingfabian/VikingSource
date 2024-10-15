using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls;
using VikingEngine.HUD.RichBox;

namespace VikingEngine.DSSWars.Display
{
    class TutorialDisplay : HUD.RichBox.RichboxGui
    {
        public bool refresh = true;
        TutorialDisplayPart displayPart;
        LocalPlayer player;

        public TutorialDisplay(LocalPlayer player)
            : base(HudLib.richboxGui, player.input)
        {
            this.player = player;
            displayPart = new TutorialDisplayPart(player, this);
            parts = new List<HUD.RichBox.RichboxGuiPart>()
            {
                displayPart, 
            };
        }

        public void update()
        {
            if (refresh || DssRef.time.oneSecond)
            {
                refresh = false;
                displayPart.refresh(player, player.tutorial);
            }
        }
    }

    class TutorialDisplayPart : RichboxGuiPart
    {
        Vector2 pos;
        public TutorialDisplayPart(Players.LocalPlayer player, RichboxGui gui)
            : base(gui)
        {
            pos = VectorExt.AddX(player.playerData.view.safeScreenArea.RightTop, -(HudLib.richboxGui.width+ gui.settings.edgeWidth));
        }

        public void refresh(Players.LocalPlayer player, Tutorial tutorial)
        {
            beginRefresh();
            //RichBoxContent content = new RichBoxContent();
            tutorial.tutorial_ToHud(content);
            endRefresh(pos, false);
        }
    }
}
