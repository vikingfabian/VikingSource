using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Data;
using VikingEngine.DSSWars.Map.Generate;
using VikingEngine.LootFest.GO.Characters.CastleEnemy;
using VikingEngine.Network;
using VikingEngine.ToGG.HeroQuest;

namespace VikingEngine.DSSWars
{
    class StartGame : Engine.GameState
    {
        Graphics.TextG loadingStatusText;
        NetworkLobby netLobby;
        WorldDataStorage storage;
        int map_start_process_done = 0;
        MapBackgroundLoading loading;
        PlayState state = null;
        SaveStateMeta loadMeta;
        bool host;
        
        public StartGame(bool host, NetworkLobby netLobby, SaveStateMeta loadMeta, MapBackgroundLoading loading)
            :base(false)
        {
            this.host = host;
            loadingStatusText = new Graphics.TextG(LoadedFont.Regular,
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Bottom - Engine.Screen.IconSize * 2),
                new Vector2(Engine.Screen.TextSize * 2f),
                Graphics.Align.Zero, "...", Color.White, ImageLayers.Lay1);

            this.loadMeta = loadMeta;
            Ref.music.stop(true);
            new PlaySettings();

            if (loadMeta == null)
            {
                // new game
                switch (DssRef.difficulty.setting_gameMode)
                {
                    case GameMode.FullStory:
                        DssRef.stats.startNewStory.addOne();
                        break;
                    case GameMode.Sandbox:
                        DssRef.stats.startNewSandbox.addOne();
                        break;
                    case GameMode.Peaceful:
                        DssRef.stats.startNewPeaceful.addOne();
                        break;
                }

                switch (DssRef.difficulty.PercDifficulty)
                {
                    case 25:
                        DssRef.stats.startNew25perc.addOne();
                        break;
                    case 50:
                        DssRef.stats.startNew50perc.addOne();
                        break;
                    case 75:
                        DssRef.stats.startNew75perc.addOne();
                        break;
                    case 100:
                        DssRef.stats.startNew100perc.addOne();
                        break;
                    case 125:
                        DssRef.stats.startNew125perc.addOne();
                        break;
                    case 150:
                        DssRef.stats.startNew150perc.addOne();
                        break;
                    case 175:
                        DssRef.stats.startNew175perc.addOne();
                        break;
                    case 200:
                        DssRef.stats.startNew200perc.addOne();
                        break;

                }

                if (DssRef.storage.runTutorial)
                {
                    DssRef.stats.startTutorial.addOne();
                }

                if (DssRef.storage.playerCount > 1)
                {
                    DssRef.stats.startNewLocalMultiplayer.addOne();
                }

                if (DssRef.storage.localPlayers[0].inputSource.IsController)
                {
                    DssRef.stats.controller_user.addOne();
                }
                else
                {
                    DssRef.stats.keyboard_user.addOne();
                }

                switch (DssRef.storage.mapSize)
                {
                    case MapSize.Tiny:
                    case MapSize.Small:
                        DssRef.stats.startNew_MapSmall.addOne();
                        break;
                    case MapSize.Medium:
                    case MapSize.Large:
                        DssRef.stats.startNew_MapLarge.addOne();
                        break;
                    case MapSize.Huge:
                    case MapSize.Epic:
                        DssRef.stats.startNew_MapHuge.addOne();
                        break;
                }
            }

            Ref.lobby.startSearchLobbies(false);
            if (host)
            {

                if (loading == null)
                {
                    Ref.netSession.LobbyPublicity = Network.LobbyPublicity.Public;
                    loading = new MapBackgroundLoading(null);
                }

                this.loading = loading;
                this.netLobby = netLobby;

                Ref.lobby.startCreateLobby(true);
            }
            else
            {
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.DssJoined_WantWorld,
                    Network.PacketReliability.Reliable, Ref.netSession.Host().Id);
            }
        }

        public override void Time_Update(float time)
        {

            base.Time_Update(time);

            if (loading != null)
            {
                loading.Update();
                loadingStatusText.TextString = loading.ProgressString();

                if (loading.Complete() && state == null)
                {
                    state = new PlayState(host, loadMeta, null);
                }

                if (Ref.music != null)
                {
                    Ref.music.Update();
                }
            }
        }

        public override void NetworkReadPacket(ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case PacketType.DssSendWorld:
                    state = new PlayState(host, loadMeta, packet.r);

                    break;
            }
        }

        public override void NetEvent_LargePacket(ReceivedPacket packet)
        {
            switch (packet.type)
            {
                case PacketType.DssSendWorld:
                    state = new PlayState(host, loadMeta, packet.r);
                    break;
            }
        }
    }
}
