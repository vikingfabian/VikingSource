using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DSSWars;
using VikingEngine.DSSWars.Communication;
using VikingEngine.DSSWars.Data;
using VikingEngine.EngineSpace;
using VikingEngine.Network;
using VikingEngine.Timer;

namespace VikingEngine.Network
{
    struct PlayerToPlayerDiplomacy
    {
        public PlayerDiplomacyAllowType allianceAllow;
        public bool canBreakAlliance;

        public PlayerDiplomacyAllowType warAllow;

        /// <summary>
        /// Larger player group cant attack a smaller one
        /// </summary>
        public bool allianceLimit;
        public bool mustAsk;
        public bool usePreparationTime;
        public TimeLength preparationTime;

        public PlayerToPlayerDiplomacy(bool host)
        {
            if (host)
            {
                allianceAllow = PlayerDiplomacyAllowType.PlayersChoose;
                warAllow = PlayerDiplomacyAllowType.PlayersChoose;
            }
            else
            {
                allianceAllow = PlayerDiplomacyAllowType.Allow;
                warAllow = PlayerDiplomacyAllowType.Blocked;
            }
            canBreakAlliance = true;

            allianceLimit = false;
            mustAsk = false;
            usePreparationTime = true;
            preparationTime = TimeLength.FromMinutes(5);
        }

       
    }
    class NetworkSettings
    {
        public bool hostNetwork = true;
        public bool findNetwork = true;

        public bool unlockPublicGames = false;
        public bool unlockPvp = false;

        public Network.LobbyPublicity lobbyPublicity = Network.LobbyPublicity.FriendsOnly;

        public int maxPlayerCount = 64;
        //public Network.LobbyPublicity findSessionJoinType = Network.LobbyPublicity.FriendsOnly;

        float netVoiceVolume = 1f;
        public float NetVoiceVol() { return MathHelper.Clamp(netVoiceVolume * Ref.gamesett.MasterVolume, 0.0f, 1.0f); }
        public bool NetVoiceMuted() { return netVoiceVolume * Ref.gamesett.MasterVolume <= 0; }

        public VoiceOption voiceOption = VoiceOption.ButtonHold;

        public PlayerToPlayerDiplomacy hostDiplomacy = new PlayerToPlayerDiplomacy(true);
        public RelationType startDiplomacy = RelationType.RelationType0_Neutral;
        public PlayerToPlayerDiplomacy clientDiplomacy = new PlayerToPlayerDiplomacy(false);
       

        /// <summary>
        /// Distance between players
        /// </summary>
        public int PlayerSpacing = 0;

        StructList<StoredNetworkGamer> storedGamers = new StructList<StoredNetworkGamer>(8);
        
        public NetworkSettings()
        {
            Ref.netsett = this;
        }
        public bool OfflineProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                hostNetwork = !value;
            }
            return !hostNetwork;
        }

        public int MaxPlayerCountProperty(object tag, bool set, int value)
        {
            if (set)
            {
                maxPlayerCount = value;
            }
            return maxPlayerCount;
        }

        public bool canBreakAllianceProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.canBreakAlliance = value;
            }
            return pd.canBreakAlliance;
        }
        public bool warAllianceLimitProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.allianceLimit = value;
            }
            return pd.allianceLimit;
        }
        public bool warMustAskProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.mustAsk = value;
            }
            return pd.mustAsk;
        }
        public bool warUsePreparationTimeProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.usePreparationTime = value;
            }
            return pd.usePreparationTime;
        }

        ref PlayerToPlayerDiplomacy ptpDiplomacy(bool host)
        {
            if (host)
            {
                return ref hostDiplomacy;
            }
            else
            {
                return ref clientDiplomacy;
            }
        }

        public StoredNetworkGamer getStoredGamer(ulong id)
        {
            for (int i = 0; i < storedGamers.Count; i++)
            {
                if (storedGamers.array[i].id == id)
                { 
                    return storedGamers.array[i];
                }
            }

            StoredNetworkGamer gamer = new StoredNetworkGamer(id);
            gamer.index = storedGamers.Count;
            storedGamers.Add(gamer);

            return gamer;
        }

        public string JoinPermissionString()
        {
            if (hostNetwork)
            {
                switch (lobbyPublicity)
                {
                    default:
                        return TextLib.Error;
                    case LobbyPublicity.Private:
                        return DssRef.todoLang.JoinPermission_Private;
                    case LobbyPublicity.FriendsOnly:
                        return DssRef.todoLang.JoinPermission_FriendsOnly;
                    case LobbyPublicity.Public:
                        return DssRef.todoLang.JoinPermission_Public;

                }
            }
            else
            {
                return DssRef.todoLang.Network_PlayOffline;
            }
        }
    }
}
