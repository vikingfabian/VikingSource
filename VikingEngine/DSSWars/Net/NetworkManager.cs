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
using VikingEngine.HUD.RichBox.Artistic;
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
                        if ((remotePlayers.Count <= 2 || remoteC.sel.networkPeer.peer != handoverPlayer) &&
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
            var player = GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex) as RemotePlayer;

            switch (packet.type)
            {
                case PacketType.DssJoined_WantWorld:
                    {
                        var w = Ref.netSession.BeginWritingPacket(PacketType.DssSendWorld, PacketReliability.Reliable, SendPacketTo.OneSpecific, packet.sender.fullId, null);
                        var meta = new SaveStateMeta();
                        meta.netSetup();
                        var saveGamestate = new SaveGamestate(meta);
                        saveGamestate.writeNet(w);
                    }
                    break;

                case PacketType.DssPlayerStatus:
                    {
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

                        int count = packet.r.ReadByte();

                        if (player.profile.flag == null)
                        {
                            player.profile.flag = new FlagAndColor(packet.r);
                            player.flagTexture = player.profile.flag.flagDesign.CreateTexture(player.profile.flag);
                            //DssRef.world.BordersUpdated = true;

                            RichBoxContent content = new RichBoxContent();

                            content.h2(NetworkIcon, ".Player joined", HudLib.TitleColor_Head);
                            content.newLine();
                            player.addNetGamerToHud(content, true, false);
                            LocalHost().hud.messages.Add(content, SoundLib.netJoined);


                            if (host)
                            {
                                //Assign faction
                                Task.Run(() =>
                                {
                                    try
                                    {
                                        Faction faction = null;

                                        var hash = PlayerMapHistory.GetGamerHash(false, packet.sender.fullId, packet.senderLocalIndex);
                                        if (previousRemotePlayers.TryGetValue(hash, out var history))
                                        {
                                            previousRemotePlayers.Remove(hash);
                                            Faction prevFaction = DssRef.world.faction(history.faction);
                                            if (prevFaction != null &&
                                                prevFaction.cities.Count > 0 &&
                                                prevFaction.player.IsBot()
                                                )
                                            {
                                                faction = prevFaction;
                                            }
                                        }

                                        if (faction == null)
                                        {
                                            faction = DssRef.world.getPlayerAvailableFaction2(localPlayers, false, true);
                                        }

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
                        var tplayer = NetReadPlayer(packet.r);
                        int factionIx = packet.r.ReadUInt16();
                        var faction = DssRef.world.faction(factionIx);
                        tplayer.AssignFaction(faction);
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
                    City.NetReadSetFaction(packet.r);
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

                        player.addNetGamerToHud(content, true, false);
                        content.icontext(SpriteName.LfChatBobbleIcon, text);

                        LocalHost().hud.messages.Add(content, SoundLib.netMessage);
                    }
                    break;
                case PacketType.DssGiftAchievement:
                    readGiftedAchievement(packet);
                    break;

                case PacketType.DssDiplomacyRelation:
                    Communication.DiplomaticRelation.NetReadRelation(packet.r);
                    break;

                case PacketType.DssPlayerToPlayerRelation:
                    new DiplomacyDisplay(LocalHost()).netReadP2pRelation(packet.r, GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex));
                    break;

                case PacketType.DssEnterBattle:
                    ObjectId.ReadSoldierGroup(packet.r, true, out _)?.enterBattleState(true, false);
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

                case PacketType.DssDeleteArmy:
                    if (ObjectId.NetReadMapObjId(packet.r, out _, true, false, out var army, out _))
                    {
                        army.DeleteMe(DeleteReason.NetworkEvent, true);
                    }
                    break;

                case PacketType.WarnPlayer:
                    {
                        BadBehaviourType behaviourType = (BadBehaviourType)packet.r.ReadByte();
                        BanWarning(LocalHost(), GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex), behaviourType);
                    }
                    break;
                case PacketType.RequestPlayerBan:
                    {
                        var badActor = NetReadPlayer(packet.r);
                        BadBehaviourType behaviourType = (BadBehaviourType)packet.r.ReadByte();

                        if (badActor != null)
                        {
                            RichBoxContent content = new RichBoxContent();
                            GetOrCreateRemotePlayer(packet.sender, packet.senderLocalIndex).addNetGamerToHud(content, true, false);
                            content.h1("Ban request", HudLib.TitleColor_Head);
                            HudLib.LabelAndText(content, SpriteName.NO_IMAGE, "Reason", behaviourType.ToString());
                            HudLib.Label(content, "Bad actor");
                            badActor.addNetGamerToHud(content, true, false);

                            content.newLine();
                            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText("Accept") },
                                new RbAction(() =>
                                {
                                    LocalHost().gameControls.clearSelection();
                                    LocalHost().hud.objMenu.netSessionDisplay.selectedPlayer = badActor.GetRemotePlayer();
                                    LocalHost().hud.objMenu.menu.OpenMenu(NetSessionDisplay.PAGE_BLOCK, HUD.RichMenu.StackOption.Stack);
                                })));

                            LocalHost().hud.messages.Add(content);
                        }
                    }
                    break;

                case PacketType.DssPing:
                    {
                        int pinIndex = packet.r.ReadUInt16();
                        var pin = player.netReadPin(pinIndex, packet.r);
                        if (pin != null && pin.Net_IsVisible())
                        {
                            pin.setInRenderState();

                            RichBoxContent content = new RichBoxContent();
                            content.h1("Ping!", HudLib.TitleColor_Head);
                            if (pin.pingMessage != PingMessage.None)
                            {
                                content.text(pin.pingMessage.ToString(), HudLib.InfoYellow_Light);
                            }

                            content.newParagraph();
                            content.Add(new ArtButton(RbButtonStyle.Primary, new List<AbsRichBoxMember> { new RbText(pin.Name(out _)) },
                                new RbAction1Arg<AbsGameObject>(LocalHost().hud.messages.goToMapObject, pin, RbSoundType.Default))
                            { fillWidth = true });

                            LocalHost().hud.messages.Add(content, SoundLib.message_loud);
                        }
                    }
                    break;

                case PacketType.DssPinUpdate:
                    {
                        LocationPin pin;
                        do
                        {
                            int pinIndex = packet.r.ReadUInt16();
                            pin = player.netReadPin(pinIndex, packet.r);

                        } while (pin != null);
                    }
                    break;

                case PacketType.DssPinHide:
                    {
                        int pinIndex = packet.r.ReadUInt16();
                        player.pins.GetIndex_Safe(pinIndex)?.Hide();
                    }
                    break;
                case PacketType.DssPinDelete:
                    {
                        int pinIndex = packet.r.ReadUInt16();
                        player.pins.GetIndex_Safe(pinIndex)?.DeleteMe(DeleteReason.NetworkEvent, true);
                    }
                    break;
            }
           

            void readGroupStatus(bool bArmy)
            {
                if (ObjectId.NetReadMapObjId(packet.r, out Faction faction, bArmy, true, out AbsArmy mapObj, out bool needInit))
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
                w = Ref.netSession.BeginWritingPacket(PacketType.DssPlayerEnterPresentation, PacketReliability.Reliable, SendPacketTo.OneSpecific, packet.sender.fullId,  null);
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

        public AbsHumanPlayer GetPlayer(ulong id)
        {
            if (Ref.netSession.LocalPeer().fullId == id)
            {
                return LocalHost();
            }

            var remotePlayerC = remotePlayers.counter();
            while (remotePlayerC.Next())
            {
                if (remotePlayerC.sel.networkPeer.peer.fullId == id)
                {
                    //TODO return region to AI
                    return remotePlayerC.sel;
                }
            }

            return null;
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
                player.addNetGamerToHud(content, true, false);
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

                var history = player.GetMapHistory();
                arraylib.AddOrReplace(previousRemotePlayers, history.GetHashCode(), history);
                remotePlayers.Remove(player);

                RichBoxContent content = new RichBoxContent();

                content.h2(NetworkIcon, ".Player left", HudLib.TitleColor_Head);
                
                content.newLine();

                player.addNetGamerToHud(content, true, false);

                if (player.faction != null)
                {
                    var aiPlayer = player.previousPlayer;
                    player.faction.factiontype = player.previousFactionType;

                    if (aiPlayer != null)
                    {
                        aiPlayer.AssignFaction(player.faction);
                        DssRef.world.BordersUpdated = true;
                    }
                }

                LocalHost().hud.messages.Add(content, SoundLib.netJoined);
                Ref.netsett.settingsHasChanged = true;
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

        public void sendGiftedAchievement(GiftedAchievementType type, RemotePlayer toPlayer)
        {
            var w = Ref.netSession.BeginWritingPacket(PacketType.DssGiftAchievement, PacketReliability.Reliable);
            toPlayer.networkPeer.writeNetID(w);
            w.Write((byte)type);

            giftMessage(LocalHost(), toPlayer, type);

            DssRef.stats.onSendGift(type);
        }

        void readGiftedAchievement(ReceivedPacket packet)
        {
            AbsHumanPlayer toPlayer = NetReadPlayer(packet.r);
            GiftedAchievementType type = (GiftedAchievementType)packet.r.ReadByte();
            giftMessage(GetRemotePlayer(packet), toPlayer, type);

            if (toPlayer.IsLocal)
            {
                var gift = GiftedAchievementCollection.Get(type);
                DssRef.achieve.UnlockAchievement(gift.achievement);
            }
        }

        void giftMessage(AbsHumanPlayer from, AbsHumanPlayer to, GiftedAchievementType type)
        {
            to.giftedAchievements.Add(type, from);

            RichBoxContent content = new RichBoxContent();

            
            from.addNetGamerToHud(content, true, false);
            content.hspace();
            content.Add(new RbImage(SpriteName.cmdConvertArrow));
            content.newLine();
            to.addNetGamerToHud(content, true, false);

            content.newParagraph();
            content.h1(NetworkIcon, "Gifted achievement", HudLib.TitleColor_Head2);
            content.newLine();

            var gift = GiftedAchievementCollection.Get(type);
            content.h1(GiftedAchievement.DefaultIcon, gift.name, HudLib.TitleColor_TypeName);
            content.text(gift.description, HudLib.InfoYellow_Light);

            LocalHost().hud.messages.Add(content, SoundLib.netJoined);
        }

        public void BanWarning(AbsHumanPlayer from, AbsHumanPlayer to, BadBehaviourType behaviourType)
        {
            RichBoxContent content = new RichBoxContent();


            from.addNetGamerToHud(content, true, false);
            content.hspace();
            content.Add(new RbImage(SpriteName.cmdConvertArrow));
            content.newLine();
            to.addNetGamerToHud(content, true, false);

            content.newParagraph();
            content.h1(NetworkIcon, "Ban warning!", HudLib.TitleColor_Head2);
            content.newLine();

            content.text("Reason: " + behaviourType.ToString(), HudLib.InfoYellow_Light);

            LocalHost().hud.messages.Add(content, SoundLib.netJoined);
        }

        public void KickPlayer(AbsNetworkPeer networkPeer)
        { 
            Ref.netSession.kickFromNetwork(networkPeer);
        }

        public void BlockPlayer(AbsNetworkPeer networkPeer)
        {
            networkPeer.storedData.ban = BanStatus.Banned;
            Ref.netSession.kickFromNetwork(networkPeer);

        }
    }
}
