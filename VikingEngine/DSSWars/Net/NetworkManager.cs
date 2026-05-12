using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VikingEngine.DebugExtensions;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.GameObject;
using VikingEngine.DSSWars.Interface;
using VikingEngine.DSSWars.Net;
using VikingEngine.DSSWars.Players;
using VikingEngine.DSSWars.Players.PlayerControls.Casual;
using VikingEngine.DSSWars.Players.Profile;
using VikingEngine.HUD;
using VikingEngine.HUD.RichBox;
using VikingEngine.Input;
using VikingEngine.LootFest.GO.PlayerCharacter;
using VikingEngine.LootFest.Players;
using VikingEngine.Network;
using VikingEngine.SteamWrapping;
using VikingEngine.ToGG.HeroQuest.Display;
using VikingEngine.ToGG.MoonFall;
using static System.Net.Mime.MediaTypeNames;

namespace VikingEngine.DSSWars
{
    partial class PlayState
    {
        bool factionHandOverComplete = false;

        public const SpriteName NetworkIcon = SpriteName.birdPlayerCount;
        ConcurrentQueue<FactionHandover> factionHandovers = new ConcurrentQueue<FactionHandover>();

        const int MaxSendLoops = 8;

        bool asynchClientNetUpdate(int id, float time)
        {
            if (remotePlayers.Count > 0 && factionHandOverComplete)
            {
                bool sentAnything = true;

                for (int loop = 0; loop < MaxSendLoops && sentAnything; loop++)
                {
                    sentAnything = false;

                    var remoteC = remotePlayers.counter();
                    while (remoteC.Next())
                    {
                        if (remoteC.sel.networkPeer.peer.lowLoad())
                        {
                            netSendMapObjectsInView(remoteC.sel, ref sentAnything);
                        }
                    }
                }
            }

            return exitThreads;
        }

        bool asynchHostNetUpdate(int id, float time)
        {
            AbsNetworkPeer handoverPlayer = null;

            if (remotePlayers.Count > 0)
            {
                if (factionHandovers.Count > 0)
                {
                    if (factionHandovers.TryPeek(out var factionHandover))
                    {
                        handoverPlayer = factionHandover.peer;

                        if (factionHandover.Next() == false)
                        {
                            //remove
                            factionHandovers.TryDequeue(out _);
                        }
                    }
                }

                bool sentAnything = true;

                for (int loop = 0; loop < MaxSendLoops && sentAnything; loop++)
                {
                    sentAnything = false;

                    var remoteC = remotePlayers.counter();
                    while (remoteC.Next())
                    {
                        if (remoteC.sel.networkPeer.peer != handoverPlayer &&
                            remoteC.sel.ready &&
                            remoteC.sel.networkPeer.peer.lowPotensialLoad(0.5f))
                        {
                            bool sentAnythingToPlayer = false;

                            if (!sendMap(remoteC.sel, ref sentAnythingToPlayer))
                            {
                                netSendMapObjectsInView(remoteC.sel, ref sentAnythingToPlayer);

                                if (!sentAnythingToPlayer)
                                {
                                    sentAnythingToPlayer = remoteC.sel.Net_FullMapSend_async();
                                }
                            }

                            sentAnything |= sentAnythingToPlayer;
                        }
                    }                    
                }                
            }
            else
            {
                factionHandovers.Clear();
            }
            return exitThreads;
        }
        bool sendMap(RemotePlayer player, ref bool sentAnything)
        {
            
            //var remoteC = remotePlayers.counter();
            //while (remoteC.Next())
            //{
            //    var netPeer_sp = remoteC.sel.networkPeer;

            if (player.gotStatus)
            {
                int sendPacketCount = player.networkPeer.peer.packetsLeft();

                while (player.Net_HostMapUpdate_async())
                {
                    sentAnything = true;
                    if (--sendPacketCount <= 0)
                    {
                        player.gotStatus = false;
                        return true;
                    }
                }
            }

            //}

            return false;
        }
        private void netSendMapObjectsInView(RemotePlayer player, ref bool sentAnything)
        {
            //var remoteC = remotePlayers.counter();
            //while (remoteC.Next())
            //{
                if (player.gotStatus)
                {
                    player.gotStatus = false;
                    int maxPackets = player.networkPeer.peer.maxPacketCount;

                    var cities = player.GetAllCitiesInView();
                    foreach (var c in cities)
                    {
                        DssRef.world.cities[c].net_roundtrip_asyncupdate(out int packetCount);
                        sentAnything |= packetCount > 0;
                        maxPackets -= packetCount;
                        if (maxPackets <= 0)
                        {
                            break;
                        }
                    }
                    player.Net_UpdateArmies(ref maxPackets);
                }
            //}
        }

        bool asynchAiPlayersUpdate(int id, float time)
        {
            if (cutScene == null)
            {
                var factions = DssRef.world.factions.counter();
                while (factions.Next())
                {
                    factions.sel.asynchAiPlayersUpdate(time);
                }
            }

            return exitThreads;
        }

        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case PacketType.DssJoined_WantWorld:
                    {
                        var w = Ref.netSession.BeginWritingPacket(PacketType.DssSendWorld, PacketReliability.Reliable, SendPacketTo.OneSpecific, packet.sender.fullId,  null);
                        var meta = new SaveStateMeta();
                        meta.netSetup();
                        var saveGamestate = new SaveGamestate(meta);
                        saveGamestate.writeNet(w);
                    }
                    break;

                case PacketType.DssPlayerStatus:
                    {
                        var player = GetRemotePlayer(packet);
                        if (player != null)
                        {
                            player.Net_readStatus(packet.r);
                            player.pointer.netRead(packet.r);

                            if (player.newPlayer)
                            {
                                //Present yourself
                                player.newPlayer = false;
                                netPresentYourself(packet);
                            }
                        }
                    }
                    break;

                case PacketType.DssPlayerEnterPresentation:
                    {

                        var player = GetOrCreateRemotePlayer(packet.sender, 0);
                        int count = packet.r.ReadByte();

                        if (player.profile.flag == null)
                        {
                            player.profile.flag = new FlagAndColor(packet.r);
                            player.flagTexture = player.profile.flag.flagDesign.CreateTexture(player.profile.flag);
                            //DssRef.world.BordersUpdated = true;

                            RichBoxContent content = new RichBoxContent();

                            content.h2(NetworkIcon, ".Player joined", HudLib.TitleColor_Head);
                            content.newLine();
                            player.addNetGamerToHud(content, false);
                            LocalHost().hud.messages.Add(content, SoundLib.netJoined);


                            if (host)
                            {
                                //Assign faction
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        Faction faction = DssRef.world.getPlayerAvailableFaction2(localPlayers, false, true);

                                        if (faction != null && faction.player.IsBot())
                                        {
                                            Ref.update.AddSyncAction(new SyncAction(() =>
                                            {
                                                AbsHumanPlayer remote = GetOrCreateRemotePlayer(packet.sender, 0);
                                                remote.AssignFaction(faction);

                                                Ref.steam.P2PManager.OnSendingLargeDataChunk();

                                                {
                                                    var w = Ref.netSession.BeginWritingPacket(PacketType.DssFactionStatus, PacketReliability.Reliable);
                                                    w.Write((ushort)faction.myIndex);
                                                    faction.writeNet_Status(w);

                                                }
                                                {
                                                    var w = Ref.netSession.BeginWritingPacket(PacketType.DssAssignFaction, PacketReliability.Reliable);
                                                    NetWritePlayer(w, remote);
                                                    w.Write((ushort)faction.myIndex);
                                                }

                                                factionHandovers.Enqueue(new FactionHandover(packet.sender, faction));
                                            }));
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        BlueScreen.ThreadException = ex;
                                    }
                                });
                            }
                        }
                    }
                    break;

                case PacketType.DssFactionStatus:
                    {
                        int faction = packet.r.ReadUInt16();
                        DssRef.world.factions.Array[faction].readNet_Status(packet.r);
                    }
                    break;

                case PacketType.DssAssignFaction:
                    {
                        var player = NetReadPlayer(packet.r);
                        int factionIx = packet.r.ReadUInt16();
                        var faction = DssRef.world.faction(factionIx);
                        player.AssignFaction(faction);
                    }
                    break;
                case PacketType.DssAssignFactionCities:
                    {
                        int factionIx = packet.r.ReadUInt16();
                        var faction = DssRef.world.faction(factionIx);

                        IntVector2 centerCamera = IntVector2.FromReadUshort(packet.r);
                        if (centerCamera.X > 0)
                        {
                            foreach (var lp in localPlayers)
                            {
                                if (lp.faction == faction)
                                {
                                    lp.gameControls.map.setCameraPos(centerCamera);
                                }
                            }
                        }
                        SpottedPointerArray cities = new SpottedPointerArray();
                        cities.read_ushort_compressed(packet.r);

                        SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                        while (citiesC.Next(ref cities, DssRef.world.cities, out City city))
                        {
                            city.setFaction(faction, false, true, false);
                        }
                    }
                    break;
                case PacketType.DssAssignFactionComplete:
                    {
                        factionHandOverComplete = true;
                        int factionIx = packet.r.ReadUInt16();
                        var faction = DssRef.world.faction(factionIx);
                        if (faction != null)
                        {
                            bool hosted = faction.IsNetHosted();
                            SpottedPointerArrayCounter citiesC = new SpottedPointerArrayCounter();
                            while (citiesC.Next(ref faction.cities, DssRef.world.cities, out City city))
                            {
                                city.IsNetHosted = hosted;
                            }
                        }
                    }
                    break;
                case PacketType.DssWorldTiles:
                    DssRef.world.readNet_Tile(packet.r);//l 32 * 4 * 4
                    overviewMap.bRefreshDataRecieved = true;
                    DssRef.world.BordersUpdated = true;
                    break;

                case PacketType.DssWorldSubTiles:
                    DssRef.world.readNet_SubTile(packet.r);//l 522
                    break;

                case PacketType.DssFactions:
                    DssRef.world.readNet_Factions(packet.r);
                    break;

                case PacketType.DssCities:
                    DssRef.world.readNet_Cities(packet.r);
                    break;

                case PacketType.DssCityStatus:
                    {
                        int cityIx = packet.r.ReadUInt16();
                        var city = DssRef.world.cities[cityIx];
                        int part = packet.r.ReadByte();
                        city.readNet_update(packet.r, part);
                    }
                    break;

                case PacketType.DssSetCityFaction:

                    break;
                
                case PacketType.DssArmyStatus:
                    Army.NetReadArmy(packet.r);
                    break;

                case PacketType.DssSoldierGroupStatus_Army:
                    readGroupStatus(true);
                    break;
                case PacketType.DssSoldierGroupStatus_City:
                    readGroupStatus(false);
                    break;

                case PacketType.TextChat:
                    {
                        string text = StreamLib.ReadString_safe(packet.r);
                        RichBoxContent content = new RichBoxContent();
                        var player = GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex);
                        
                        player.addNetGamerToHud(content, false);
                        content.icontext(SpriteName.LfChatBobbleIcon, text);

                        LocalHost().hud.messages.Add(content, SoundLib.netMessage);
                    }
                    break;

               

                case PacketType.DssDiplomacyRelation:
                    Communication.DiplomaticRelation.NetReadRelation(packet.r);
                    break;

                case PacketType.DssPlayerToPlayerRelation:
                    new DiplomacyDisplay(LocalHost()).netReadP2pRelation(packet.r, GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex));
                    break;

                case PacketType.DssEnterBattle:
                    ObjectId.ReadSoldierGroup(packet.r, out _)?.enterBattleState(true, false);
                    break;

                case PacketType.DssAttackDamage:
                    AbsSoldierUnit.ReadAttackDamage(packet.r);
                    break;

                case PacketType.DssSoldierDeath:
                    var soldier = ObjectId.ReadSoldier(packet.r, out _);
                    if (soldier != null)
                    {
                        soldier.DeleteMe(DeleteReason.Death, true);
                    }
                    break;

            }

            void readGroupStatus(bool bArmy)
            {
                if (ObjectId.NetReadMapObjId(packet.r, out Faction faction, bArmy, out AbsArmy mapObj, out bool needInit))
                {
                    if (mapObj != null)
                    {
                        bool more = false;
                        do
                        {
                            more = AbsArmy.NetReadGroup(packet.r, mapObj);
                        } while (more);
                    }
                }
            }
        }

        public override void NetEvent_LargePacket(ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case PacketType.DssCityHandOver:
                    City.NetReadHandOver(packet.r);
                    break;
                case PacketType.DssWorldDiplomacy:
                    DssRef.world.diplomacy.readRelations(packet.r, int.MaxValue);                    
                    break;
            }
        }

        void netPresentYourself(ReceivedPacket packet)
        {
            System.IO.BinaryWriter w;

            if (packet.sender == null)
            {
                w = Ref.netSession.BeginWritingPacket(PacketType.DssPlayerEnterPresentation, PacketReliability.Reliable);
            }
            else
            {
                w = Ref.netSession.BeginWritingPacket(PacketType.DssPlayerEnterPresentation, PacketReliability.Reliable,SendPacketTo.OneSpecific, packet.sender.fullId,  null);
            }
            w.Write((byte)localPlayers.Count);
            foreach (var local in localPlayers)
            {
                var profile = DssRef.storage.localPlayers[local.playerData.localPlayerIndex].Profile();
                profile.flag.write(w);
            }
        }

        public Players.RemotePlayer GetRemotePlayer(ReceivedPacket packet)
        {
            return packet.sender.instancePeers?[packet.senderLocalIndex].Tag as Players.RemotePlayer;
        }

        public override void NetEvent_GotNetworkId()
        {
            //doesnt run
            base.NetEvent_GotNetworkId();


        }

        public override void NetworkStatusMessage(NetworkStatusMessage message)
        {
            //base.NetworkStatusMessage(message);

            switch (message)
            {
                case Network.NetworkStatusMessage.Created_session:
                    foreach (var p in localPlayers)
                    {
                        p.initNetwork();
                    }
                    break;
            }
        }

        public override void NetEvent_ErrorMessage(string message, AbsNetworkPeer peer, bool peerIsSender)
        {
            RichBoxContent content = new RichBoxContent();

            content.h1(SpriteName.RedErrorCross, "Network error", HudLib.NotAvailableColor);
            content.text(message);

            RemotePlayer player = peer.Tag as RemotePlayer;

            if (player != null)
            {
                content.newLine();
                HudLib.Label(content, peerIsSender ? "Sender" : "Reciever");
                content.newLine();
                player.addNetGamerToHud(content, false);
            }
        }
        public override void NetEvent_ConnectionLost(string reason)
        {
            base.NetEvent_ConnectionLost(reason);
            if (!this.host)
            {
                new GameState.ExitToLobby(false);
            }
        }

        public override void NetUpdate()
        {
            foreach (var player in localPlayers)
            {
                player.NetUpdate();
            }
        }

        public override void NetEvent_PeerJoined(AbsNetworkPeer peer)
        {
            base.NetEvent_PeerJoined(peer);
            GetOrCreateRemotePlayer(peer, 0);

            if (Ref.netsett.voiceOption == VoiceOption.AlwaysOn)
            {                
                Ref.steam.StartRecording();                
            }

            NetEvent_ErrorMessage("test test", peer, true);
        }

        public override void NetEvent_PeerLost(AbsNetworkPeer peer)
        {
            var player = peer.Tag as RemotePlayer;

            if (player == null)
            {
                var remotePlayerC = remotePlayers.counter();
                while (remotePlayerC.Next())
                {
                    if (remotePlayerC.sel.networkPeer != null &&
                        remotePlayerC.sel.networkPeer.peer == peer)
                    {
                        player = remotePlayerC.sel;
                    }
                }
            }

            if (player != null)
            {

                player.DeleteMe();
                remotePlayers.Remove(player);

                RichBoxContent content = new RichBoxContent();

                content.h2(NetworkIcon, ".Player left", HudLib.TitleColor_Head);
                
                content.newLine();

                player.addNetGamerToHud(content, false);

                if (player.faction != null)
                {
                    var aiPlayer = player.previousPlayer;
                    if (aiPlayer != null)
                    {
                        aiPlayer.AssignFaction(player.faction);
                        DssRef.world.BordersUpdated = true;
                    }
                }

                LocalHost().hud.messages.Add(content, SoundLib.netJoined);
            }
        }

        


        public void NetWritePlayer(System.IO.BinaryWriter w, AbsHumanPlayer player)
        {
            player.networkPeer.writeNetID(w);
        }

        public AbsHumanPlayer NetReadPlayer(System.IO.BinaryReader r)
        {
            NetworkInstancePeer.ReadNetID(r, out var peer, out int index);

            return GetOrCreateRemotePlayer(peer, index);
        }
    }
}
