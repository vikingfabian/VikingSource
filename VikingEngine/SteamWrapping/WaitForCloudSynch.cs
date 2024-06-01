using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    //TODO, does not actually know when the files are loaded
    class WaitForCloudSynch
    {
        Time timeout = new Time(1.5f, TimeUnit.Seconds);

        /// <returns>Ready or timed out</returns>
        public bool update()
        {
            if (Ref.steam.isInitialized)
            {
                if (timeout.CountDown())
                {
                    return true;
                }

                bool isActive = SteamAPI.SteamRemoteStorage().IsCloudEnabledForAccount();
                return !isActive;
            }

            return true;
        }

    }
}
