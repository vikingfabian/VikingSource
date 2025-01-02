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
                var w = Ref.netSession.BeginWritingPacket(Network.PacketType.hqEnteredLobby,
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
                    //var meta = new SaveStateMeta();
                    //var saveGamestate = new SaveGamestate(meta);
                    //saveGamestate.readNet(packet.r);
                    state = new PlayState(host, loadMeta, packet.r);

                    break;
            }
        }
    }
}
