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
        public UseTimeLimit warDeclarePreparationTime;
        public UseTimeLimit gameStartPreparationTime;

        public PlayerToPlayerDiplomacy(bool host)
        {
            if (host)
            {
                allianceAllow = PlayerDiplomacyAllowType.PlayersChoose;
                warAllow = PlayerDiplomacyAllowType.Blocked;
            }
            else
            {
                allianceAllow = PlayerDiplomacyAllowType.Allow;
                warAllow = PlayerDiplomacyAllowType.Blocked;
            }
            canBreakAlliance = true;

            allianceLimit = false;
            mustAsk = false;
            warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));

        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)allianceAllow);
            w.Write(canBreakAlliance);
            w.Write((byte)warAllow);
            w.Write(allianceLimit);
            w.Write(mustAsk);
            warDeclarePreparationTime.write_ushort(w, true);
            gameStartPreparationTime.write_ushort(w, true);
        }

        public void read(System.IO.BinaryReader r, int storageVersion)
        { 
            allianceAllow = (PlayerDiplomacyAllowType)r.ReadByte();
            canBreakAlliance = r.ReadBoolean();
            warAllow = (PlayerDiplomacyAllowType)r.ReadByte();
            allianceLimit = r.ReadBoolean();
            mustAsk = r.ReadBoolean();
            warDeclarePreparationTime.read_ushort(r, true);
            gameStartPreparationTime.read_ushort(r, true);

        }
    }
    class NetworkSettings
    {
        public bool settingsHasChanged = false;

        public bool hostNetwork = true;
        public bool findNetwork = true;

        public bool unlockPublicGames = false;
        public bool unlockPvp = false;

        public Network.LobbyPublicity lobbyPublicity = Network.LobbyPublicity.FriendsOnly;

        public int maxPlayerCount = 64;
        
        public VoiceOption voiceOption = VoiceOption.ButtonHold;

        public PlayerToPlayerDiplomacy hostPtoP = new PlayerToPlayerDiplomacy(true);
        public RelationType startDiplomacy = RelationType.RelationType0_Neutral;
        public PlayerToPlayerDiplomacy clientPtoP = new PlayerToPlayerDiplomacy(false);
        public bool fairProtection = true;
       
        /// <summary>
        /// Distance between players
        /// </summary>
        public int PlayerSpacing = 1;

        public GiftRecieveOption recieveGifts = GiftRecieveOption.FriendsOnly;

        public bool allowHandicap = true;
        public bool useHandicap = false;
        public HandicapLevel handicap_botAggression = HandicapLevel.Default;
        public bool handicap_extraHonorGuards = false;
        public bool handicap_resourceBoost = false;
        public HandicapLevel handicap_taxIncome = HandicapLevel.Default;
        public bool alsoBlockOnRequest = true;

        public StructList<StoredNetworkGamer> storedGamers = new StructList<StoredNetworkGamer>(8);
        
        public NetworkSettings()
        {
            Ref.netsett = this;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write(hostNetwork);
            w.Write(findNetwork);

            w.Write(unlockPublicGames);
            w.Write(unlockPvp);

            w.Write((byte)lobbyPublicity);
            w.Write(maxPlayerCount);

            w.Write((byte)voiceOption);

            hostPtoP.write(w);
            w.Write((int)startDiplomacy);
            clientPtoP.write(w);
            w.Write(fairProtection);

            w.Write(PlayerSpacing);

            w.Write((byte)recieveGifts);
            w.Write(allowHandicap);
            w.Write(useHandicap);
            w.Write((byte)handicap_botAggression);
            w.Write(handicap_extraHonorGuards);
            w.Write(handicap_resourceBoost);
            w.Write((byte)handicap_taxIncome);

            Debug.WriteCheck(w);

            w.Write(alsoBlockOnRequest);
            w.Write((ushort)storedGamers.Count);
            for (int i = 0; i < storedGamers.Count; i++)
            {
                storedGamers.array[i].write(w);
            }

            Debug.WriteCheck(w);
        }

        public void read(System.IO.BinaryReader r, int storageVersion)
        {
            hostNetwork = r.ReadBoolean();
            findNetwork = r.ReadBoolean();

            unlockPublicGames = r.ReadBoolean();
            unlockPvp = r.ReadBoolean();

            lobbyPublicity = (LobbyPublicity)r.ReadByte();
            maxPlayerCount = r.ReadInt32();

            voiceOption = (VoiceOption)r.ReadByte();

            hostPtoP.read(r, storageVersion);
            startDiplomacy = (RelationType)r.ReadInt32();
            clientPtoP.read(r, storageVersion);
            fairProtection = r.ReadBoolean();

            PlayerSpacing = r.ReadInt32();

            recieveGifts = (GiftRecieveOption)r.ReadByte();
            allowHandicap = r.ReadBoolean();
            useHandicap = r.ReadBoolean();
            handicap_botAggression = (HandicapLevel)r.ReadByte();
            handicap_extraHonorGuards = r.ReadBoolean();
            handicap_resourceBoost = r.ReadBoolean();
            handicap_taxIncome = (HandicapLevel)r.ReadByte();

            Debug.ReadCheck(r);

            alsoBlockOnRequest = r.ReadBoolean();
            int storedGamersCount = r.ReadUInt16();
            storedGamers = new StructList<StoredNetworkGamer>(storedGamersCount + 8);
            for (int i = 0; i < storedGamersCount; i++)
            {
                storedGamers.array[i].read(r, storageVersion);
            }

            Debug.ReadCheck(r);
        }
        public bool alsoBlockOnRequestProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                alsoBlockOnRequest = value;
                settingsHasChanged = true;
            }
            return alsoBlockOnRequest;
        }
        //public bool allowHandicap = true;
        //public bool useHandicap = false;
        //public HandicapLevel handicap_botAggression = HandicapLevel.Default;
        //public bool handicap_extraHonorGuards = false;
        //public bool handicap_resourceBoost = false;
        public bool allowHandicapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                allowHandicap = value;
                settingsHasChanged = true;
            }
            return allowHandicap;
        }
        public bool useHandicapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                useHandicap = value;
                settingsHasChanged = true;
            }
            return useHandicap;
        }
        public bool handicap_extraHonorGuardsProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                handicap_extraHonorGuards = value;
                settingsHasChanged = true;
            }
            return handicap_extraHonorGuards;
        }
       
        public bool handicap_resourceBoostProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                handicap_resourceBoost = value;
                settingsHasChanged = true;
            }
            return handicap_resourceBoost;
        }

        public bool OfflineProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                hostNetwork = !value;
                Ref.netSession.setLobbyJoinable(hostNetwork);
                settingsHasChanged = true;
            }
            return !hostNetwork;
        }

        public bool fairProtectionProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                fairProtection = value;
                settingsHasChanged = true;
            }
            return fairProtection;
        }

        public int MaxPlayerCountProperty(object tag, bool set, int value)
        {
            if (set)
            {
                maxPlayerCount = value;
                settingsHasChanged = true;
            }
            return maxPlayerCount;
        }

        public int PlayerSpacingProperty(object tag, bool set, int value)
        {
            if (set)
            {
                PlayerSpacing = value;
                settingsHasChanged = true;
            }
            return PlayerSpacing;
        }



        public bool canBreakAllianceProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.canBreakAlliance = value;
                settingsHasChanged = true;
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
                settingsHasChanged = true;
            }
            return pd.mustAsk;
        }
        public bool warUsePreparationTimeProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            settingsHasChanged |= set;
            
            return pd.warDeclarePreparationTime.UseProperty(null, set, value);
       
        }
        public float warPreparationTimeProperty(object tag, bool set, float value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            settingsHasChanged |= set;

            return pd.warDeclarePreparationTime.MinuteProperty(null, set, value);

        }
        public bool warUseGameStartTimeProperty(object tag, bool set, bool value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            settingsHasChanged |= set;

            return pd.gameStartPreparationTime.UseProperty(null, set, value);

        }
        public float warStartTimeProperty(object tag, bool set, float value)
        {
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            settingsHasChanged |= set;

            return pd.gameStartPreparationTime.MinuteProperty(null, set, value);

        }
        ref PlayerToPlayerDiplomacy ptpDiplomacy(bool host)
        {
            if (host)
            {
                return ref hostPtoP;
            }
            else
            {
                return ref clientPtoP;
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

        public void setUpdatedStoredGamer(StoredNetworkGamer gamer)
        {
            if (storedGamers.array[gamer.index].id == gamer.id)
            {
                storedGamers.array[gamer.index] = gamer;
            }
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
