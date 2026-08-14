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
    struct PlayerToPlayerDiplomacyData
    {        
        public PlayerDiplomacyAllowType allianceAllow;//i
        public bool canBreakAlliance;//i

        public PlayerDiplomacyAllowType warAllow;//i

        /// <summary>
        /// Larger player group cant attack a smaller one
        /// </summary>
        public bool allianceLimit;//i
        public bool mustAsk;//i
        public bool fairProtection;//i
        public UseTimeLimit warDeclarePreparationTime;//i
        public UseTimeLimit gameStartPreparationTime;//i

        public PlayerToPlayerDiplomacyData(bool host)
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
            fairProtection = true;

            allianceLimit = false;
            mustAsk = false;
            warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));
        }

        public void ApplyHostSettings(PlayerToPlayerDiplomacyData hostSettings)
        {
            if (hostSettings.allianceAllow != PlayerDiplomacyAllowType.PlayersChoose)
            {
                allianceAllow = hostSettings.allianceAllow;
                canBreakAlliance = hostSettings.canBreakAlliance;
            }

            if (hostSettings.warAllow != PlayerDiplomacyAllowType.PlayersChoose)
            { 
                warAllow = hostSettings.warAllow;
                allianceLimit = hostSettings.allianceLimit;
                mustAsk = hostSettings.mustAsk;
                fairProtection = hostSettings.fairProtection;
                warDeclarePreparationTime = hostSettings.warDeclarePreparationTime;
                gameStartPreparationTime = hostSettings.gameStartPreparationTime;
            }
        }

        public void ApplyFairProtection()
        {
            if (fairProtection)
            {
                ApplyFairProtection(Ref.netsett.clientPtoP);
            }
        }

        public void ApplyFairProtection(PlayerToPlayerDiplomacyData PtoP)
        {
            if (PtoP.warAllow == PlayerDiplomacyAllowType.Blocked)
            {
                warAllow = PlayerDiplomacyAllowType.Blocked;
            }
            
            canBreakAlliance |= PtoP.canBreakAlliance;
            allianceLimit |= PtoP.allianceLimit;
            mustAsk |= PtoP.mustAsk;

            warDeclarePreparationTime.use |= PtoP.warDeclarePreparationTime.use;
            warDeclarePreparationTime.time.seconds = Math.Max(warDeclarePreparationTime.time.seconds, PtoP.warDeclarePreparationTime.time.seconds);

            gameStartPreparationTime.use |= PtoP.gameStartPreparationTime.use;
            gameStartPreparationTime.time.seconds = Math.Max(gameStartPreparationTime.time.seconds, PtoP.gameStartPreparationTime.time.seconds);

        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)allianceAllow);
            w.Write(canBreakAlliance);
            w.Write((byte)warAllow);
            w.Write(allianceLimit);
            w.Write(mustAsk);
            w.Write(fairProtection);
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
            fairProtection = r.ReadBoolean();
            warDeclarePreparationTime.read_ushort(r, true);
            gameStartPreparationTime.read_ushort(r, true);
        }
    }

    struct HostSettings
    {
        public LobbyPublicity lobbyPublicity;//i
        public bool allowHandicap = true;//i
        public bool allowCasualControls = true;//i
        public bool autoReColorFlags = false;//i

        public RelationType startDiplomacy = RelationType.RelationType0_Neutral;//i

        public HostSettings()
        {
            lobbyPublicity = Ref.netsett.lobbyPublicity;
        }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)lobbyPublicity);
            w.Write((sbyte)startDiplomacy);
            //w.Write(allowHandicap);
            //w.Write(allowCasualControls);
            EightBit bits = new EightBit(allowHandicap, allowCasualControls, autoReColorFlags);
            bits.write(w);

        }
        public void read(System.IO.BinaryReader r, int storageVersion)
        {
            lobbyPublicity = (LobbyPublicity)r.ReadByte();
            startDiplomacy = (RelationType)r.ReadSByte();
            //allowHandicap = r.ReadBoolean();
            //allowCasualControls = r.ReadBoolean();
            EightBit bits = new EightBit(r);
            bits.Get(out allowHandicap, out allowCasualControls, out autoReColorFlags);
        }
    }

    struct ClientSettings
    {
        public GiftRecieveOption recieveGifts = GiftRecieveOption.FriendsOnly;//i
        public bool useHandicap = true;//i
        public HandicapLevel handicap_botAggression = HandicapLevel.Default;//i
        public bool handicap_extraHonorGuards = true;//i
        public bool handicap_resourceBoost = true;//i
        public HandicapLevel handicap_taxIncome = HandicapLevel.Default;//i

        public ClientSettings()
        { }

        public void write(System.IO.BinaryWriter w)
        {
            w.Write((byte)recieveGifts);
            w.Write(useHandicap);
            w.Write((byte)handicap_botAggression);
            w.Write(handicap_extraHonorGuards);
            w.Write(handicap_resourceBoost);
            w.Write((byte)handicap_taxIncome);
        }
        public void read(System.IO.BinaryReader r, int storageVersion)
        {
            recieveGifts = (GiftRecieveOption)r.ReadByte();
            useHandicap = r.ReadBoolean();
            handicap_botAggression = (HandicapLevel)r.ReadByte();
            handicap_extraHonorGuards = r.ReadBoolean();
            handicap_resourceBoost = r.ReadBoolean();
            handicap_taxIncome = (HandicapLevel)r.ReadByte();
        }

        public void ApplyHostSettings(HostSettings hostSettings)
        {
            useHandicap &= hostSettings.allowHandicap;
        }
    }

    struct NetSharedClientSettings
    {        
        public PlayerToPlayerDiplomacyData clientPtoP;
        public ClientSettings clientSettings;

        public NetSharedClientSettings()
        {
            clientPtoP = Ref.netsett.clientPtoP;
            clientSettings = Ref.netsett.clientSettings;
        }

        public void write(System.IO.BinaryWriter w)
        {
            clientSettings.write(w);
            clientPtoP.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            clientSettings.read(r, int.MaxValue);
            clientPtoP.read(r, int.MaxValue);
        }

        public void ApplyHostSettings()
        {
            clientPtoP.ApplyHostSettings(Ref.netsett.remoteHostSettings.hostPtoP);
            //clientPtoP.ApplyFairProtection(Ref.netsett.clientPtoP);
            clientSettings.ApplyHostSettings(Ref.netsett.remoteHostSettings.hostSettings);
        }
    }

    struct NetSharedHostSettings
    {       
        public PlayerToPlayerDiplomacyData hostPtoP;
        public HostSettings hostSettings;

        public NetSharedHostSettings()
        {
            hostPtoP = Ref.netsett.hostPtoP;
            hostSettings = Ref.netsett.hostSettings;
        }

        public void write(System.IO.BinaryWriter w)
        {
            hostSettings.write(w);
            hostPtoP.write(w);
        }
        public void read(System.IO.BinaryReader r)
        {
            hostSettings.read(r, int.MaxValue);
            hostPtoP.read(r, int.MaxValue);
        }
    }

    class NetworkSettings
    {
        public NetSharedHostSettings remoteHostSettings;

        public bool settingsHasChanged = false;

        public bool hostNetwork = true;
        public bool findNetwork = true;

        public bool unlockPublicGames = false;
        public bool unlockPvp = false;

        public Network.LobbyPublicity lobbyPublicity = Network.LobbyPublicity.FriendsOnly;

        public int maxPlayerCount = 64;
        
        public VoiceOption voiceOption = VoiceOption.ButtonHold;

        public PlayerToPlayerDiplomacyData hostPtoP = new PlayerToPlayerDiplomacyData(true);
        public HostSettings hostSettings;
        public PlayerToPlayerDiplomacyData clientPtoP = new PlayerToPlayerDiplomacyData(false);
        
       
        /// <summary>
        /// Distance between players
        /// </summary>
        public int PlayerSpacing = 2;
      
        public ClientSettings clientSettings = new ClientSettings();
        public bool alsoBlockOnRequest = true;

        public StructList<StoredNetworkGamer> storedGamers = new StructList<StoredNetworkGamer>(8);
        
        public bool HasPvp => hostPtoP.warAllow == PlayerDiplomacyAllowType.Allow;


        public void SendStats(bool host)
        {
            bool allowPvp = true;
            if (host)
            {
                allowPvp &= HasPvp;
            }

            allowPvp &= clientPtoP.warAllow == PlayerDiplomacyAllowType.Allow;

            if (allowPvp)
            {
                DssRef.stats.startMultiplayer_AllowPvp.addOne();
            }
            else
            {
                DssRef.stats.startMultiplayer_BlockPvp.addOne();
            }

            if (unlockPublicGames && lobbyPublicity == LobbyPublicity.Public)
            {
                DssRef.stats.startMultiplayer_AllowPublic.addOne();
            }
            else
            {
                DssRef.stats.startMultiplayer_BlockPublic.addOne();
            }
        }
        public NetworkSettings()
        {
            Ref.netsett = this;
            hostSettings = new HostSettings();
        }

        public void write(System.IO.BinaryWriter w)
        {
            hostSettings.write(w);

            w.Write(hostNetwork);
            w.Write(findNetwork);

            w.Write(unlockPublicGames);
            w.Write(unlockPvp);

            w.Write((byte)lobbyPublicity);
            w.Write(maxPlayerCount);

            w.Write((byte)voiceOption);

            hostPtoP.write(w);
            
            clientPtoP.write(w);
            
            w.Write(PlayerSpacing);

            
            //w.Write(allowCasualControls);
            
            clientSettings.write(w);
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
            hostSettings.read(r, storageVersion);

            hostNetwork = r.ReadBoolean();
            findNetwork = r.ReadBoolean();

            unlockPublicGames = r.ReadBoolean();
            unlockPvp = r.ReadBoolean();

            lobbyPublicity = (LobbyPublicity)r.ReadByte();
            maxPlayerCount = r.ReadInt32();

            voiceOption = (VoiceOption)r.ReadByte();

            hostPtoP.read(r, storageVersion);
            
            clientPtoP.read(r, storageVersion);
            if (clientPtoP.allianceAllow == PlayerDiplomacyAllowType.PlayersChoose)
            {
                clientPtoP.allianceAllow = PlayerDiplomacyAllowType.Allow;
            }
            
            PlayerSpacing = r.ReadInt32();

            
            //allowCasualControls = r.ReadBoolean();

            clientSettings.read(r, storageVersion);
            alsoBlockOnRequest = r.ReadBoolean();
            int storedGamersCount = r.ReadUInt16();
            storedGamers = new StructList<StoredNetworkGamer>(storedGamersCount + 8);
            for (int i = 0; i < storedGamersCount; i++)
            {
                StoredNetworkGamer gamer = new StoredNetworkGamer();
                gamer.read(r, storageVersion);
                storedGamers.Add(gamer);
            }

            Debug.ReadCheck(r);
        }

        public void resetPeaceful()
        {
            hostPtoP.allianceAllow = PlayerDiplomacyAllowType.PlayersChoose;
            hostPtoP.canBreakAlliance = true;
            hostPtoP.warAllow = PlayerDiplomacyAllowType.Blocked;

            hostPtoP.allianceLimit = true;
            hostPtoP.mustAsk = true;
            hostPtoP.warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            hostPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));

            clientPtoP.allianceAllow = PlayerDiplomacyAllowType.Allow;
            clientPtoP.canBreakAlliance = true;
            clientPtoP.warAllow = PlayerDiplomacyAllowType.Blocked;

            clientPtoP.allianceLimit = true;
            clientPtoP.mustAsk = true;
            clientPtoP.warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            clientPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));

            PlayerSpacing = 2;
            hostSettings.allowHandicap = true;
            hostSettings.allowCasualControls = true;
            //allowCasualControls = true;
            clientSettings.useHandicap = true;
            clientSettings.handicap_botAggression = HandicapLevel.Low;
            clientSettings.handicap_extraHonorGuards = true;
            clientSettings.handicap_resourceBoost = false;
            clientSettings.handicap_taxIncome = HandicapLevel.Default;
        }
        public void resetMixed()
        {
            hostPtoP.allianceAllow = PlayerDiplomacyAllowType.PlayersChoose;
            hostPtoP.canBreakAlliance = true;
            hostPtoP.warAllow = unlockPvp? PlayerDiplomacyAllowType.PlayersChoose : PlayerDiplomacyAllowType.Blocked;

            hostPtoP.allianceLimit = true;
            hostPtoP.mustAsk = false;
            hostPtoP.warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            hostPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));

            clientPtoP.allianceAllow = PlayerDiplomacyAllowType.Allow;
            clientPtoP.canBreakAlliance = true;
            clientPtoP.warAllow = unlockPvp ? PlayerDiplomacyAllowType.Allow : PlayerDiplomacyAllowType.Blocked;

            clientPtoP.allianceLimit = true;
            clientPtoP.mustAsk = false;
            clientPtoP.warDeclarePreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));
            clientPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(10));

            PlayerSpacing = 2;
            hostSettings.allowHandicap = true;
            hostSettings.allowCasualControls = true;
            //allowCasualControls = true;
            clientSettings.useHandicap = false;
            clientSettings.handicap_botAggression = HandicapLevel.Default;
            clientSettings.handicap_extraHonorGuards = false;
            clientSettings.handicap_resourceBoost = false;
            clientSettings.handicap_taxIncome = HandicapLevel.Default;
        }
        public void resetHardcore()
        {
            hostPtoP.allianceAllow = PlayerDiplomacyAllowType.Allow;
            hostPtoP.canBreakAlliance = true;
            hostPtoP.warAllow = PlayerDiplomacyAllowType.Allow;

            hostPtoP.allianceLimit = false;
            hostPtoP.mustAsk = false;
            hostPtoP.warDeclarePreparationTime = new UseTimeLimit(false, TimeLength.FromMinutes(5));
            hostPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));

            clientPtoP.allianceAllow = PlayerDiplomacyAllowType.Allow;
            clientPtoP.canBreakAlliance = true;
            clientPtoP.warAllow = PlayerDiplomacyAllowType.Allow;

            clientPtoP.allianceLimit = false;
            clientPtoP.mustAsk = false;
            clientPtoP.warDeclarePreparationTime = new UseTimeLimit(false, TimeLength.FromMinutes(5));
            clientPtoP.gameStartPreparationTime = new UseTimeLimit(true, TimeLength.FromMinutes(5));

            PlayerSpacing = 3;
            hostSettings.allowHandicap = false;
            hostSettings.allowCasualControls = false;
            //allowCasualControls = false;
            clientSettings.useHandicap = false;
            clientSettings.handicap_botAggression = HandicapLevel.High;
            clientSettings.handicap_extraHonorGuards = false;
            clientSettings.handicap_resourceBoost = false;
            clientSettings.handicap_taxIncome = HandicapLevel.Default;
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

        public bool allowHandicapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                hostSettings.allowHandicap = value;
                settingsHasChanged = true;
            }
            return hostSettings.allowHandicap;
        }
        public bool allowCasualControlsProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                hostSettings.allowCasualControls = value;
                settingsHasChanged = true;
            }
            return hostSettings.allowCasualControls;
        }

        public bool autoRecolorFlagsProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                hostSettings.autoReColorFlags = value;
                settingsHasChanged = true;
            }
            return hostSettings.autoReColorFlags;
        }

        //public bool allowCasualControlsProperty(object tag, bool set, bool value)
        //{
        //    if (set)
        //    {
        //        allowCasualControls = value;
        //        settingsHasChanged = true;
        //    }
        //    return allowCasualControls;
        //}
        public bool useHandicapProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                clientSettings.useHandicap = value;
                settingsHasChanged = true;
            }
            return clientSettings.useHandicap;
        }
        public bool handicap_extraHonorGuardsProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                clientSettings.handicap_extraHonorGuards = value;
                settingsHasChanged = true;
            }
            return clientSettings.handicap_extraHonorGuards;
        }
       
        public bool handicap_resourceBoostProperty(object tag, bool set, bool value)
        {
            if (set)
            {
                clientSettings.handicap_resourceBoost = value;
                settingsHasChanged = true;
            }
            return clientSettings.handicap_resourceBoost;
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

        public bool fairProtectionProperty(object tag, bool set, bool value)
        {           
            bool host = (bool)tag;
            ref var pd = ref ptpDiplomacy(host);

            if (set)
            {
                pd.fairProtection = value;
                settingsHasChanged = true;
            }
            return pd.fairProtection;
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
        ref PlayerToPlayerDiplomacyData ptpDiplomacy(bool host)
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

        public BanStatus IsBanned(ulong id)
        {
            for (int i = 0; i < storedGamers.Count; i++)
            {
                if (storedGamers.array[i].id == id)
                {
                    return storedGamers.array[i].ban;
                }
            }
            return BanStatus.None;
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
                        return DssRef.lang.JoinPermission_Private;
                    case LobbyPublicity.FriendsOnly:
                        return DssRef.lang.JoinPermission_FriendsOnly;
                    case LobbyPublicity.Public:
                        return DssRef.lang.JoinPermission_Public;
                }
            }
            else
            {
                return DssRef.lang.Network_PlayOffline;
            }
        }
    }
}
