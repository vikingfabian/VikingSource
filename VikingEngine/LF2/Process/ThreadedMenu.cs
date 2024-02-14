using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Process
{
    class ThreadedMenu : QueAndSynch
    {
        HUD.File file;
        Players.Player player;
        Players.Link link;
        int menuId;

        public ThreadedMenu(Players.Player player, Players.Link link, int menuId)
            :base(true, false)
        {
            this.menuId = menuId;
            this.player = player;
            this.link = link;
            start();
        }
        protected override bool quedEvent()
        {
            file = player.GenerateThreadedMenu(link);
            return file != null;
        }
        public override void Time_Update(float time)
        {
            player.OpenMenuFileFromThread(file, menuId);
        }
    }
}
