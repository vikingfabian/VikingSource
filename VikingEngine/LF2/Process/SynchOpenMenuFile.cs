using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Process
{
    class SynchOpenMenuFile : OneTimeTrigger
    {
        HUD.File file;
        HUD.AbsMenu menu;

        public SynchOpenMenuFile(HUD.File file, HUD.AbsMenu menu)
            :base(false)
        {
            this.file = file;
            this.menu = menu;
            AddToUpdateList(true);
        }

        public override void Time_Update(float time)
        {
            if (menu.Visible)
                menu.File = file;
        }

    }
}
