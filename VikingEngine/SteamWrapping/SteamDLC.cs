#if PCGAME
using VikingEngine.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Valve.Steamworks;

namespace VikingEngine.SteamWrapping
{
    class DlcDescriptor
    {
        /* Fields */
        public uint appId;
        public string name;
        public bool owned;

        /* Constructors */
        public DlcDescriptor(uint dlcAppId, string storeName)
        {
            appId = dlcAppId;
            name = storeName;
            owned = false;
        }

        /* Novelty Methods */
        public bool UpdateAndCheckIfOwned()
        {
            owned = SteamAPI.SteamApps().BIsDlcInstalled(appId);
            return owned;
        }

        /* Family Methods */
        public override string ToString()
        {
            return "DLC: \"" + name + "\", " + appId.ToString();
        }
    }

    class SteamDLC
    {
        /* Fields */
        DlcDescriptor[] dlcs;
        SteamCallback<DlcInstalled_t> DlcInstalledCB;

        /* Constructors */
        public SteamDLC()
        {
            DlcInstalledCB = new SteamCallback<DlcInstalled_t>(OnDlcInstalled, false);

            dlcs = GetAvailableDlcAppIds(PlatformSettings.RunProgram);

            if (dlcs != null)
            {
                foreach (var dlc in dlcs)
                {
                    dlc.UpdateAndCheckIfOwned();
                }
            }
        }

        /* Novelty Methods */
        public void AddMenuOptionToDisplayDlcsIfAvailable(GuiLayout layout)
        {
            if (dlcs.Length == 0)
            {
                // no dlc.
                return;
            }

            if (!SteamAPI.SteamUtils().IsOverlayEnabled())
            {
                // overlay is not available.
                return;
            }

            var action = new GuiAction2Arg<uint, EOverlayToStoreFlag>(
                SteamAPI.SteamFriends().ActivateGameOverlayToStore,
                Ref.steam.applicationSettings.appId,
                EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
            new GuiTextButton("DLC", null, action, true, layout);
        }

        DlcDescriptor[] GetAvailableDlcAppIds(StartProgram program)
        {
            switch(program)
            {
                case StartProgram.PartyJousting:
                    return new DlcDescriptor[]
                    {
                        new DlcDescriptor(439450, "Party Jousting - Character Pack"),
                        new DlcDescriptor(442830, "Party Jousting - Bling Pack"),
                        new DlcDescriptor(451890, "Party Jousting - Zombie Pack"),
                        new DlcDescriptor(111111, "Error test"),
                    };

                default:
                    return null;
            }
        }

        public bool JoustingCharacterPack
        {
            get
            {
                if (dlcs == null)
                    return false;
                return dlcs[0].owned;
            }
        }
        public bool JoustingBlingPack
        {
            get
            {
                if (dlcs == null)
                    return false;
                return dlcs[1].owned;
            }
        }
        public bool JoustingZombiePack
        {
            get
            {
                if (dlcs == null)
                    return false;
                return dlcs[2].owned;
            }
        }

        public int Count()
        {
            int result = 0;

            if (dlcs != null)
            {
                for (int i = 0; i < dlcs.Length; ++i)
                {
                    if (dlcs[i].owned)
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        public void OpenDlcStore(int dlcIndex)
        {
            if (dlcs != null)
            {
                SteamAPI.SteamFriends().ActivateGameOverlayToStore(
                    dlcs[dlcIndex].appId,
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_AddToCartAndShow);
            }
        }

        public void openGameStore(uint appid)
        {
            SteamAPI.SteamFriends().ActivateGameOverlayToStore(
                    appid,
                    EOverlayToStoreFlag.k_EOverlayToStoreFlag_None);
        }

        public int DlcCount_FromApi
        {
            get
            {
                return SteamAPI.SteamApps().GetDLCCount();
            }
        }

        /* Callback Responses */
        void OnDlcInstalled(DlcInstalled_t callback)
        {
            uint dlcAppId = callback.m_nAppID;

            foreach (DlcDescriptor dlc in dlcs)
            {
                if (dlc.appId == dlcAppId)
                    dlc.owned = true;
            }

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