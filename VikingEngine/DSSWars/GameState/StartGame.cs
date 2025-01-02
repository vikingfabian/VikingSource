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

        public StartGame(bool host, NetworkLobby netLobby, SaveStateMeta loadMeta, MapBackgroundLoading loading)
            :base(false)
        {
            this.loadMeta = loadMeta;
            Ref.music.stop(true);
            new PlaySettings();

            if (loading == null)
            {
                Ref.netSession.LobbyPublicity = Network.LobbyPublicity.Public;
                loading = new MapBackgroundLoading(null);
            }

            this.loading=loading;
            this.netLobby = netLobby;

            loadingStatusText = new Graphics.TextG(LoadedFont.Regular, 
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Bottom - Engine.Screen.IconSize * 2), 
                new Vector2(Engine.Screen.TextSize * 2f),
                Graphics.Align.Zero, "...", Color.White, ImageLayers.Lay1);

            Ref.lobby.startSearchLobbies(false);

            if (host)
            {
                Ref.lobby.startCreateLobby(true);
            }
           
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            loading.Update();
            loadingStatusText.TextString = loading.ProgressString();

            if (loading.Complete() && state == null)
            {
                state = new PlayState(true, loadMeta);
            }

            if (Ref.music != null)
            {
                Ref.music.Update();
            }
            //if (storage.loadComplete)
            //{
            //    loadingStatusText.TextString = "Waiting for players";
            //}
            //else
            //{
            //    loadingStatusText.TextString = "Loading map";
            //}

            //if (storage.loadComplete && netLobby.allReady())
            //{
            //    if (map_start_process_done == 0)
            //    {
            //        map_start_process_done = 1; 
            //        Task.Factory.StartNew(() =>
            //            {
            //                new Map.Generate.GenerateMap().postLoadGenerate(DssRef.world);
            //                map_start_process_done = 2;
            //            }
            //        );
            //    }
            //    else if (map_start_process_done == 2)
            //    {
            //        new PlayState(true);
            //    }
            //}
        }
    }
}
