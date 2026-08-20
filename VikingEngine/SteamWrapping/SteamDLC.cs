#if PCGAME
using VikingEngine.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Steamworks;
using VikingEngine.DSSWars;

namespace VikingEngine.SteamWrapping
{
    struct DlcDescriptor
    {
        public AppId_t appId;
        public bool owned;

        public DlcDescriptor(AppId_t dlcAppId)
        {
            appId = dlcAppId;
            owned = false;

            UpdateAndCheckIfOwned();
        }

        public bool UpdateAndCheckIfOwned()
        {
            owned = SteamApps.BIsDlcInstalled(appId);
            return owned;
        }

        public void OnDlcInstalled(AppId_t dlcAppId)
        {
            if (appId == dlcAppId)
            { 
                owned = true;
            }
        }

        public override string ToString()
        {
            return $"DLC: {appId}, owned {owned}";
        }
    }

    class SteamDLC
    {        
        Callback<DlcInstalled_t> DlcInstalledCB;
                
        public SteamDLC()
        {
            DlcInstalledCB = new Callback<DlcInstalled_t>(OnDlcInstalled, false);

#if DSS
            DssRef.InitDLC();
#endif
           
        }


        public void OpenDlcStore(DlcDescriptor dlcDescriptor)
        {
            if (dlcDescriptor.appId.m_AppId != 0)
            {
                SteamFriends.ActivateGameOverlayToStore(
                    dlcDescriptor.appId,
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
            }
        }

        public void openGameStore(AppId_t appid)
        {
            SteamFriends.ActivateGameOverlayToStore(
                    appid,
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
        }

        public int DlcCount_FromApi
        {
            get
            {
                return SteamApps.GetDLCCount();
            }
        }

        /* Callback Responses */
        void OnDlcInstalled(DlcInstalled_t callback)
        {
            AppId_t dlcAppId = callback.m_nAppID;

            Ref.gamestate.OnDlcInstalled(dlcAppId);
            

//#if PJ
//            if (Ref.gamestate is PJ.LobbyState)
//            {
//                ((PJ.LobbyState)Ref.gamestate).onDlcChanged();
//            }
//#endif
        }
    }
}
#endif