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
        
        //DlcDescriptor //[] dlcs;
        
        Callback<DlcInstalled_t> DlcInstalledCB;

        
        public SteamDLC()
        {
            DlcInstalledCB = new Callback<DlcInstalled_t>(OnDlcInstalled, false);

#if DSS
            DssRef.InitDLC();
#endif
            //dlcs = GetAvailableDlcAppIds(PlatformSettings.RunProgram);

            //if (dlcs != null)
            //{
            //    foreach (var dlc in dlcs)
            //    {
            //        dlc.UpdateAndCheckIfOwned();
            //    }
            //}
        }

        //public void AddMenuOptionToDisplayDlcsIfAvailable(GuiLayout layout)
        //{
        //    //if (dlcs.Length == 0)
        //    //{
        //    //    // no dlc.
        //    //    return;
        //    //}

        //    if (!SteamUtils.IsOverlayEnabled())
        //    {
        //        // overlay is not available.
        //        return;
        //    }

        //    //var action = new GuiAction2Arg<AppId_t, EOverlayToStoreFlag>(
        //    //    SteamFriends.ActivateGameOverlayToStore(
        //    //        Ref.steam.applicationSettings.appId,
        //    //        EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
        //    new GuiTextButton("DLC", null, ()=> {
        //        SteamFriends.ActivateGameOverlayToStore(
        //            Ref.steam.applicationSettings.appId,
        //            EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
        //    }, true, layout);
        //}
        /*
        DlcDescriptor[] GetAvailableDlcAppIds(StartProgram program)
        {
            switch(program)
            {
                case StartProgram.PartyJousting:
                    return new DlcDescriptor[]
                    {
                        new DlcDescriptor(new AppId_t(439450), "Party Jousting - Character Pack"),
                        new DlcDescriptor(new AppId_t(442830), "Party Jousting - Bling Pack"),
                        new DlcDescriptor(new AppId_t(451890), "Party Jousting - Zombie Pack"),
                        new DlcDescriptor(new AppId_t(111111), "Error test"),
                    };
                case StartProgram.DSS:
                    return new DlcDescriptor[]
                    {
                        new DlcDescriptor(new AppId_t(4820280), "DSS 2: War Industry - Supporter Pack"),
                        new DlcDescriptor(new AppId_t(4820290), "DSS 2: War Industry - Blood and Gore"),
                        new DlcDescriptor(new AppId_t(111111), "Error test"),
                    };

                default:
                    return null;
            }
        }
        */

        //public bool JoustingCharacterPack
        //{
        //    get
        //    {
        //        if (dlcs == null)
        //            return false;
        //        return dlcs[0].owned;
        //    }
        //}
        //public bool JoustingBlingPack
        //{
        //    get
        //    {
        //        if (dlcs == null)
        //            return false;
        //        return dlcs[1].owned;
        //    }
        //}
        //public bool JoustingZombiePack
        //{
        //    get
        //    {
        //        if (dlcs == null)
        //            return false;
        //        return dlcs[2].owned;
        //    }
        //}

        //public int Count()
        //{
        //    int result = 0;

        //    if (dlcs != null)
        //    {
        //        for (int i = 0; i < dlcs.Length; ++i)
        //        {
        //            if (dlcs[i].owned)
        //            {
        //                result++;
        //            }
        //        }
        //    }

        //    return result;
        //}

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
            //foreach (DlcDescriptor dlc in dlcs)
            //{
            //    if (dlc.appId == dlcAppId)
            //        dlc.owned = true;
            //}

#if PJ
            if (Ref.gamestate is PJ.LobbyState)
            {
                ((PJ.LobbyState)Ref.gamestate).onDlcChanged();
            }
#endif
        }
    }
}
#endif