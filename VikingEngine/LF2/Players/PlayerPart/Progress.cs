using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VikingEngine.LF2.Players
{
    partial class Player
    {
        public void EggnestDestroyed()
        {
            Progress.guardCaptainRewards++;
            
        }

        public void UnlockBuildHammer()
        {
            CloseMenu();
            if (!Settings.UnlockedBuildHammer)
            {
                Settings.UnlockedBuildHammer = true;
                SettingsChanged();
            }
            //make sure the hammer is available on RB
            progress.UnlockBuildHammer(true);
        }
    }
}
