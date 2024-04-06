using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using VikingEngine.DSSWars.Map.Generate;

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

        public StartGame(NetworkLobby netLobby, MapBackgroundLoading loading)
            :base(false)
        {
            Ref.music.stop(true);
            new PlaySettings();

            if (loading == null)
            { 
                loading = new MapBackgroundLoading();
            }

            this.loading=loading;
            //available.join();
            this.netLobby = netLobby;

            loadingStatusText = new Graphics.TextG(LoadedFont.Regular, 
                new Vector2(Engine.Screen.SafeArea.X, Engine.Screen.SafeArea.Bottom - Engine.Screen.IconSize * 2), 
                new Vector2(Engine.Screen.TextSize * 2f),
                Graphics.Align.Zero, "...", Color.White, ImageLayers.Lay1);

            //int loadingNumber = Ref.rnd.Int(MapFileGeneratorState.MapCountPerSize) + 1;

            //storage = new WorldDataStorage();

            //if (StartupSettings.SaveLoadSpecificMap.HasValue)
            //{
            //    DssRef.storage.mapSize = StartupSettings.SaveLoadSpecificMap.Value;
            //    loadingNumber = 1;
            //}
            //storage.loadMap(DssRef.storage.mapSize, loadingNumber);
            
        }

        public override void Time_Update(float time)
        {
            base.Time_Update(time);

            loading.Update();
            loadingStatusText.TextString = loading.ProgressString();

            if (loading.Complete() && state == null)
            {
                state = new PlayState(true);
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
